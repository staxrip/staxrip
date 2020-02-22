
#pragma once

#include "Common.h"
#include "FrameServer.h"
#include "avisynth_headers\avisynth.h"

#include <atomic>

const AVS_Linkage* AVS_linkage = NULL;


class AviSynthServer : IFrameServer
{

private:

    std::atomic<int>     m_References = 0;
    std::wstring         m_Error;
    ServerInfo           m_Info = {};

    IScriptEnvironment2* m_ScriptEnvironment = NULL;
    PClip                m_Clip;
    AVSValue             m_AVSValue;
    PVideoFrame          m_Frame;
    const AVS_Linkage*   m_Linkage = NULL;

    void Free();

public:

    ~AviSynthServer();

    // IUnknown

    HRESULT __stdcall QueryInterface(const IID& iid, void** ppv);
    ULONG   __stdcall AddRef();
    ULONG   __stdcall Release();

    // IFrameServer

    HRESULT     __stdcall OpenFile(WCHAR* file);
    void*       __stdcall GetFrame(int position);
    ServerInfo* __stdcall GetInfo();
    WCHAR*      __stdcall GetError();
};


extern "C" __declspec(dllexport) AviSynthServer* __stdcall CreateAviSynthServer();
