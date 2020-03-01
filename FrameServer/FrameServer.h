
#pragma once

#include <unknwn.h>


// {A933B077-7EC2-42CC-8110-91DE21116C1A}
static const GUID IID_IFrameServer = {0xa933b077,0x7ec2,0x42cc,{0x81,0x10,0x91,0xde,0x21,0x11,0x6c,0x1a}};


struct ServerInfo
{
    int Width;
    int Height;
    int FrameRateNum;
    int FrameRateDen;
    int FrameCount;
    int ColorSpace;
};


struct IFrameServer : IUnknown
{
    virtual HRESULT     __stdcall OpenFile(WCHAR* file) = NULL;
    virtual void*       __stdcall GetFrame(int position) = NULL;
    virtual ServerInfo* __stdcall GetInfo() = NULL;
    virtual WCHAR*      __stdcall GetError() = NULL;
};
