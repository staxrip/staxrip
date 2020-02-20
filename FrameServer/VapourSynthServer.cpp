
#include "VapourSynthServer.h"


// IUnknown

HRESULT __stdcall VapourSynthServer::QueryInterface(const IID& iid, void** ppv)
{
    if (!ppv)
        return E_POINTER;

    if (iid == IID_IUnknown || iid == IID_IFrameServer)
    {
        *ppv = this;
        AddRef();
        return S_OK;
    }

    *ppv = NULL;
    return E_NOINTERFACE;
}


ULONG __stdcall VapourSynthServer::AddRef()
{
    return ++m_References;
}


ULONG __stdcall VapourSynthServer::Release() {
    int refs = --m_References;

    if (!refs)
        delete this;

    return refs;
}


// IFrameServer

HRESULT __stdcall VapourSynthServer::OpenFile(WCHAR* file)
{
    try
    {
        WCHAR dllPath[512];
        DWORD dllPathSize = sizeof(dllPath);

        auto result = RegGetValue(
            HKEY_LOCAL_MACHINE, L"Software\\VapourSynth", L"VSScriptDLL",
            RRF_RT_REG_SZ, NULL, dllPath, &dllPathSize);

        if (result)
        {
            result = RegGetValue(
                HKEY_CURRENT_USER, L"Software\\VapourSynth", L"VSScriptDLL",
                RRF_RT_REG_SZ, NULL, dllPath, &dllPathSize);
        }

        if (result)
            throw std::exception("VapourSynth location not found in registry");

        static HMODULE dll = LoadLibrary(dllPath);

        if (!dll)
            throw std::exception("Failed to load VapourSynth library");

        static bool wasResolved = false;

        if (!wasResolved)
        {
            bool x64 = sizeof(void*) == 8;

            std::array resolvePairs = {
                std::pair((void**)&vs_init,           x64 ? "vsscript_init"           : "_vsscript_init@0"),
                std::pair((void**)&vs_finalize,       x64 ? "vsscript_finalize"       : "_vsscript_finalize@0"),
                std::pair((void**)&vs_evaluateScript, x64 ? "vsscript_evaluateScript" : "_vsscript_evaluateScript@16"),
                std::pair((void**)&vs_evaluateFile,   x64 ? "vsscript_evaluateFile"   : "_vsscript_evaluateFile@12"),
                std::pair((void**)&vs_freeScript,     x64 ? "vsscript_freeScript"     : "_vsscript_freeScript@4"),
                std::pair((void**)&vs_getError,       x64 ? "vsscript_getError"       : "_vsscript_getError@4"),
                std::pair((void**)&vs_getOutput,      x64 ? "vsscript_getOutput"      : "_vsscript_getOutput@8"),
                std::pair((void**)&vs_clearOutput,    x64 ? "vsscript_clearOutput"    : "_vsscript_clearOutput@8"),
                std::pair((void**)&vs_getCore,        x64 ? "vsscript_getCore"        : "_vsscript_getCore@4"),
                std::pair((void**)&vs_getVSApi,       x64 ? "vsscript_getVSApi"       : "_vsscript_getVSApi@0")
            };

            for (auto& i : resolvePairs)
            {
                if (nullptr == (*(i.first) = GetProcAddress(dll, i.second)))
                    throw std::exception("Failed to resolve VapourSynth vsscript functions");
            }
        }

        wasResolved = true;

        if (!(m_vsInit = vs_init()))
            throw std::exception("Failed to initialize VapourSynth");

        m_vsAPI = vs_getVSApi();

        if (!m_vsAPI)
            throw std::exception("Failed to call VapourSynth vsscript_getVSApi");

        std::string utf8file = ConvertWideToUtf8(file);

        if (vs_evaluateFile(&m_vsScript, utf8file.c_str(), 0))
            throw std::exception(vs_getError(m_vsScript));     

        m_vsNode = vs_getOutput(m_vsScript, 0);

        if (!m_vsNode)
            throw std::exception("Failed to get VapourSynth output");

        m_vsInfo = m_vsAPI->getVideoInfo(m_vsNode);

        if (!m_vsInfo)
            throw std::exception("Failed to get VapourSynth video info");        

        m_Info.Width = m_vsInfo->width;
        m_Info.Height = m_vsInfo->height;
        m_Info.FrameCount = m_vsInfo->numFrames;
        m_Info.FrameRateNumerator = m_vsInfo->fpsNum;
        m_Info.FrameRateDenominator = m_vsInfo->fpsDen;

        return S_OK;
    }
    catch (std::exception& e)
    {
        m_Error = ConvertAnsiToWide(e.what());
    }

    Free();
    return E_FAIL;
}


void* __stdcall VapourSynthServer::GetFrame(int position)
{
    if (!m_vsAPI || !m_vsNode)
        return NULL;

    const VSFrameRef* frame = m_vsAPI->getFrame(position, m_vsNode, m_vsErrorMessage, sizeof(m_vsErrorMessage));

    if (!frame)
    {
        m_Error = ConvertUtf8ToWide(m_vsErrorMessage);
        return NULL;
    }

    auto readPtr = m_vsAPI->getReadPtr(frame, 0);

    if (!readPtr)
        return NULL;

    if (m_vsFrame)
        m_vsAPI->freeFrame(m_vsFrame);

    m_vsFrame = frame;
    return (void*)readPtr;
}


ServerInfo* __stdcall VapourSynthServer::GetInfo()
{
    return &m_Info;
}


WCHAR* __stdcall VapourSynthServer::GetError()
{
    return (WCHAR*)m_Error.c_str();
}


/////////// local

VapourSynthServer::~VapourSynthServer()
{
    Free();
}


void VapourSynthServer::Free()
{
    if (m_vsAPI)
    {
        if (m_vsFrame)
        {
            m_vsAPI->freeFrame(m_vsFrame);
            m_vsFrame = NULL;
        }

        if (m_vsNode)
        {
            m_vsAPI->freeNode(m_vsNode);
            m_vsNode = NULL;
        }
    }
    
    if (m_vsScript)
    {
        vs_freeScript(m_vsScript);
        m_vsScript = NULL;
    }

    if (m_vsInit)
    {
        vs_finalize();
        m_vsInit = 0;
    }
}


// Extern

extern "C" __declspec(dllexport) VapourSynthServer* __stdcall
CreateVapourSynthServer()
{
    VapourSynthServer* server = new VapourSynthServer();
    server->AddRef();
    return server;
}
