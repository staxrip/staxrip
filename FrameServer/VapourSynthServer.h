
#pragma once

#define WIN32_LEAN_AND_MEAN

#include <Windows.h>

#include <atomic>

#include "FrameServer.h"


class VapourSynthServer : IFrameServer
{

private:

    std::atomic<int> m_References = 0;

public:

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


extern "C" __declspec(dllexport) VapourSynthServer* __stdcall CreateVapourSynthServer();
