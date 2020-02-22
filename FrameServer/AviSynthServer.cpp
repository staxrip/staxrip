
#include "AviSynthServer.h"


///////// IUnknown

HRESULT __stdcall AviSynthServer::QueryInterface(const IID& iid, void** ppv)
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


ULONG __stdcall AviSynthServer::AddRef()
{
    return ++m_References;
}


ULONG __stdcall AviSynthServer::Release() {
    int refs = --m_References;

    if (!refs)
        delete this;

    return refs;
}


////////// IFrameServer

HRESULT __stdcall AviSynthServer::OpenFile(WCHAR* file)
{
    try
    {
        memset(&m_Clip,  0, sizeof(PClip));
        memset(&m_Frame, 0, sizeof(PVideoFrame));

        static HMODULE dll = LoadLibrary(L"AviSynth");

        if (!dll)
            dll = LoadLibrary(L"AviSynth");

        if (!dll)
            throw std::exception("AviSynth+ installation cannot be found");

        IScriptEnvironment2* (*CreateScriptEnvironment2)(int version) =
            (IScriptEnvironment2 * (*)(int)) GetProcAddress(dll, "CreateScriptEnvironment2");

        if (!CreateScriptEnvironment2)
            throw std::exception("Cannot resolve AviSynth+ CreateScriptEnvironment2 function");

        m_ScriptEnvironment = CreateScriptEnvironment2(6);

        if (!m_ScriptEnvironment)
            throw std::exception("A newer AviSynth+ version is required");

        AVS_linkage = m_Linkage = m_ScriptEnvironment->GetAVSLinkage();

        std::string ansiFile = ConvertWideToANSI(file);
        AVSValue arg(ansiFile.c_str());
        m_AVSValue = m_ScriptEnvironment->Invoke("Import", AVSValue(&arg, 1));

        if (!m_AVSValue.IsClip())
            throw std::exception("AviSynth+ script does not return a video clip");

        m_Clip = m_AVSValue.AsClip();

        VideoInfo avsInfo = m_Clip->GetVideoInfo();

        m_Info.Width = avsInfo.width;
        m_Info.Height = avsInfo.height;
        m_Info.FrameCount = avsInfo.num_frames;
        m_Info.FrameRateNum = avsInfo.fps_numerator;
        m_Info.FrameRateDen = avsInfo.fps_denominator;

        return S_OK;
    }
    catch (AvisynthError& e)
    {
        m_Error = ConvertAnsiToWide(e.msg);
    }
    catch (std::exception& e)
    {
        m_Error = ConvertAnsiToWide(e.what());
    }
    catch (...)
    {
        m_Error = L"Exception: AviSynthServer::OpenFile";
    }

    Free();
    return E_FAIL;
}


void* __stdcall AviSynthServer::GetFrame(int position)
{
    if (m_Info.FrameCount == 0)
        return NULL;

    AVS_linkage = m_Linkage;

    try
    {
        m_Frame = m_Clip->GetFrame(position, m_ScriptEnvironment);
        auto readPtr = m_Frame->GetReadPtr();

        if (!readPtr)
            throw std::exception("AviSynth+ pixel data pointer is null");

        return (void*)readPtr;
    }
    catch (AvisynthError& e)
    {
        m_Error = ConvertAnsiToWide(e.msg);
    }
    catch (std::exception& e)
    {
        m_Error = ConvertAnsiToWide(e.what());
    }
    catch (...)
    {
        m_Error = L"Exception: AviSynthServer::GetFrame";
    }

    return NULL;
}


ServerInfo* __stdcall AviSynthServer::GetInfo()
{
    return &m_Info;
}


WCHAR* __stdcall AviSynthServer::GetError()
{
    return (WCHAR*)m_Error.c_str();
}


/////////// local

AviSynthServer::~AviSynthServer()
{
    Free();
}


void AviSynthServer::Free()
{
    AVS_linkage = m_Linkage;

    m_Frame     = NULL;
    m_Clip      = NULL;
    m_AVSValue  = NULL;

    if (m_ScriptEnvironment)
    {
        m_ScriptEnvironment->DeleteScriptEnvironment();
        m_ScriptEnvironment = NULL;
    }

    AVS_linkage = NULL;
    m_Linkage = NULL;
}


///////// extern

extern "C" __declspec(dllexport) AviSynthServer* __stdcall
CreateAviSynthServer()
{
    AviSynthServer* server = new AviSynthServer();
    server->AddRef();
    return server;
}
