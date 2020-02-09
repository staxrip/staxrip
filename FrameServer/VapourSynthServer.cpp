
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
    else
    {
        *ppv = NULL;
        return E_NOINTERFACE;
    }
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
    return NULL;
}


void* __stdcall VapourSynthServer::GetFrame(int position)
{
    return NULL;
}


ServerInfo* __stdcall VapourSynthServer::GetInfo()
{
    return NULL;
}


WCHAR* __stdcall VapourSynthServer::GetError()
{
    return NULL;
}

// Extern

extern "C" __declspec(dllexport) VapourSynthServer* __stdcall
CreateVapourSynthServer()
{
    VapourSynthServer* server = new VapourSynthServer();
    server->AddRef();
    return server;
}
