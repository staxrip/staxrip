
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
    else
    {
        *ppv = NULL;
        return E_NOINTERFACE;
    }
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
        memset(&m_Clip, 0, sizeof(PClip));
        memset(&m_Frame, 0, sizeof(PVideoFrame));

        HMODULE dll = LoadLibrary(L"AviSynth");

        if (!dll)
            return ErrorHelp(L"AviSynth installation cannot be found.");

        IScriptEnvironment* (*CreateScriptEnvironment)(int version) =
            (IScriptEnvironment * (*)(int)) GetProcAddress(dll, "CreateScriptEnvironment");

        if (!CreateScriptEnvironment)
            return ErrorHelp(L"Cannot load CreateScriptEnvironment.");

        m_ScriptEnvironment = CreateScriptEnvironment(6);

        if (!m_ScriptEnvironment)
            return ErrorHelp(L"A newer AviSynth version is required.");

        AVS_linkage = m_ScriptEnvironment->GetAVSLinkage();

        std::string ansiFile = ConvertWideToANSI(file);
        AVSValue arg(ansiFile.c_str());
        m_AVSValue = m_ScriptEnvironment->Invoke("Import", AVSValue(&arg, 1));

        if (!m_AVSValue.IsClip())
            return ErrorHelp(L"The script's return was not a video clip.");

        m_Clip = m_AVSValue.AsClip();

        VideoInfo avsInfo = m_Clip->GetVideoInfo();
        m_Info.Width = avsInfo.width;
        m_Info.Height = avsInfo.height;
        m_Info.FrameCount = avsInfo.num_frames;
        m_Info.FrameRateNumerator = avsInfo.fps_numerator;
        m_Info.FrameRateDenominator = avsInfo.fps_denominator;
    }
    catch (AvisynthError err)
    {
        m_Error = ConvertAnsiToWide(err.msg);
        Free();
        return E_FAIL;
    }
    catch (...)
    {
        return ErrorHelp(L"Unhandled exception: AviSynthServer::OpenFile");
    }

    return S_OK;
}


void* __stdcall AviSynthServer::GetFrame(int position)
{
    if (!m_ScriptEnvironment)
        return NULL;

    m_Frame = m_Clip->GetFrame(position, m_ScriptEnvironment);

    if (m_Frame)
        return (void*)m_Frame->GetReadPtr();

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

AviSynthServer::AviSynthServer()
{
}


AviSynthServer::~AviSynthServer()
{
    Free();
}


void AviSynthServer::Free()
{
    m_Frame = NULL;
    m_Clip = NULL;
    m_AVSValue = NULL;
    AVS_linkage = NULL;

    if (m_ScriptEnvironment)
    {
        m_ScriptEnvironment->DeleteScriptEnvironment();
        m_ScriptEnvironment = NULL;
    }
}


HRESULT AviSynthServer::ErrorHelp(const WCHAR* msg)
{
    m_Error = msg;
    Free();
    return E_FAIL;
}


///////// extern

extern "C" __declspec(dllexport) AviSynthServer* __stdcall
CreateAviSynthServer()
{
    AviSynthServer* server = new AviSynthServer();
    server->AddRef();
    return server;
}
