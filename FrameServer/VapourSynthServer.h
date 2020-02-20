
#pragma once

#define WIN32_LEAN_AND_MEAN


#include "Common.h"
#include "FrameServer.h"
#include "VSScript.h"
#include "VSHelper.h"
#include "Windows.h"

#include <atomic>
#include <array>

int          (__stdcall* vs_init)           (void);
int          (__stdcall* vs_finalize)       (void);
int          (__stdcall* vs_evaluateScript) (VSScript** handle, const char* script, const char* errorFilename, int flags);
int          (__stdcall* vs_evaluateFile)   (VSScript** handle, const char* scriptFilename, int flags);
void         (__stdcall* vs_freeScript)     (VSScript*  handle);
const char*  (__stdcall* vs_getError)       (VSScript*  handle);
VSNodeRef*   (__stdcall* vs_getOutput)      (VSScript*  handle, int index);
void         (__stdcall* vs_clearOutput)    (VSScript*  handle, int index);
VSCore*      (__stdcall* vs_getCore)        (VSScript*  handle);
const VSAPI* (__stdcall* vs_getVSApi)       (void);


class VapourSynthServer : IFrameServer
{

private:

    std::atomic<int>   m_References = 0;
    std::wstring       m_Error;
    ServerInfo         m_Info = {};

    int                m_vsInit = 0;
    const VSAPI*       m_vsAPI    = NULL;
    VSScript*          m_vsScript = NULL;
    VSNodeRef*         m_vsNode   = NULL;
    const VSFrameRef*  m_vsFrame  = NULL;
    const VSVideoInfo* m_vsInfo   = NULL;
    char               m_vsErrorMessage[1024];

    void Free();

public:

    ~VapourSynthServer();

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
