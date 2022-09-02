
#pragma once

#define WIN32_LEAN_AND_MEAN


#include "Common.h"
#include "FrameServer.h"
#include "avisynth.h"
#include "VSScript4.h"
#include "VSHelper4.h"
#include "Windows.h"

#include <atomic>
#include <array>

const VSSCRIPTAPI* (__stdcall* vss_getVSScriptAPI) (int version);

class VapourSynthServer : IFrameServer
{

private:

    std::atomic<int>   m_References = 0;
    std::wstring       m_Error;
    ServerInfo         m_Info = {};

    HMODULE            m_vssDLL      = nullptr;
    const VSSCRIPTAPI* m_vsScriptAPI = nullptr;
    const VSAPI*       m_vsAPI       = nullptr;
    VSCore*            m_vsCore      = nullptr;
    VSScript*          m_vsScript    = nullptr;
    VSNode*            m_vsNode      = nullptr;
    const VSFrame*     m_vsFrame     = nullptr;
    const VSVideoInfo* m_vsInfo      = nullptr;
    char               m_vsErrorMessage[1024];

    void Free();

public:

    ~VapourSynthServer();

    // IUnknown

    HRESULT __stdcall QueryInterface(const IID& iid, void** ppv);
    ULONG   __stdcall AddRef();
    ULONG   __stdcall Release();

    void __stdcall setError(const char* msg);

    // IFrameServer

    HRESULT     __stdcall OpenFile(WCHAR* file);
    HRESULT     __stdcall GetFrame(int position, void** data, int& pitch);
    ServerInfo* __stdcall GetInfo();
    WCHAR*      __stdcall GetError();
};


extern "C" __declspec(dllexport) VapourSynthServer* __stdcall CreateVapourSynthServer();
