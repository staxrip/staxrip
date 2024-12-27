
#include "VapourSynthServer.h"

void logMessageHandler(int msgType, const char* msg, void* userData)
{
    if (msgType > mtWarning) {
        reinterpret_cast<VapourSynthServer*>(userData)->setError(msg);
    }
}

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
        static bool wasResolved = false;

        if (!wasResolved)
        {
            m_vssDLL = LoadLibrary(L"VSScript.dll");

            if (!m_vssDLL)
            {
                std::string msg = GetWinErrorMessage(GetLastError());
                throw std::runtime_error("Failed to load VapourSynth:\r\n\r\n" + msg);
            }

            bool x64 = sizeof(void*) == 8;

            vss_getVSScriptAPI = reinterpret_cast<decltype(vss_getVSScriptAPI)>(GetProcAddress(m_vssDLL, x64 ? "getVSScriptAPI" : "_getVSScriptAPI@4"));

            if (!vss_getVSScriptAPI)
                throw std::exception("Failed to load getVSScriptAPI function. Upgrade Vapoursynth to R55 or newer!");
        }

        wasResolved = true;

        m_vsScriptAPI = vss_getVSScriptAPI(VSSCRIPT_API_VERSION);

        if (!m_vsScriptAPI)
            throw std::exception("Failed to initialize VapourSynth");

        m_vsAPI = m_vsScriptAPI->getVSAPI(VAPOURSYNTH_API_VERSION);

        if (!m_vsAPI)
            throw std::exception("Failed to get VapourSynth API");

        std::string utf8file = ConvertWideToUtf8(file);

        m_vsCore = m_vsAPI->createCore(0);
        m_vsAPI->addLogHandler(logMessageHandler, nullptr, this, m_vsCore);

        m_vsScript = m_vsScriptAPI->createScript(m_vsCore);
        m_vsScriptAPI->evalSetWorkingDir(m_vsScript, 1);

        m_vsScriptAPI->evaluateFile(m_vsScript, utf8file.c_str());
        if (m_vsScriptAPI->getError(m_vsScript))
        {
            m_Error = ConvertUtf8ToWide(m_vsScriptAPI->getError(m_vsScript));
            Free();
            return E_FAIL;
        }

        m_vsNode = m_vsScriptAPI->getOutputNode(m_vsScript, 0);

        if (!m_vsNode || m_vsAPI->getNodeType(m_vsNode) != mtVideo)
            throw std::exception("Failed to find usable output at VapourSynth node 0");

        m_vsInfo = m_vsAPI->getVideoInfo(m_vsNode);

        if (!m_vsInfo)
            throw std::exception("Failed to get VapourSynth info");        

        m_Info.Width = m_vsInfo->width;
        m_Info.Height = m_vsInfo->height;
        m_Info.FrameCount = m_vsInfo->numFrames;
        m_Info.FrameRateNum = m_vsInfo->fpsNum;
        m_Info.FrameRateDen = m_vsInfo->fpsDen;

        const VSVideoFormat format = m_vsInfo->format;
        int id = m_vsAPI->queryVideoFormatID(format.colorFamily, format.sampleType, format.bitsPerSample, format.subSamplingW, format.subSamplingH, m_vsCore);

        if (id == pfYUV444P8)          m_Info.ColorSpace = VideoInfo::CS_YV24;
        else if (id == pfYUV422P8)     m_Info.ColorSpace = VideoInfo::CS_YV16;
        else if (id == pfYUV420P8)     m_Info.ColorSpace = VideoInfo::CS_YV12;
        else if (id == pfYUV410P8)     m_Info.ColorSpace = VideoInfo::CS_YUV9;
        else if (id == pfYUV411P8)     m_Info.ColorSpace = VideoInfo::CS_YV411;
        else if (id == pfYUV444P10)    m_Info.ColorSpace = VideoInfo::CS_YUV444P10;
        else if (id == pfYUV422P10)    m_Info.ColorSpace = VideoInfo::CS_YUV422P10;
        else if (id == pfYUV420P10)    m_Info.ColorSpace = VideoInfo::CS_YUV420P10;
        else if (id == pfYUV444P12)    m_Info.ColorSpace = VideoInfo::CS_YUV444P12;
        else if (id == pfYUV422P12)    m_Info.ColorSpace = VideoInfo::CS_YUV422P12;
        else if (id == pfYUV420P12)    m_Info.ColorSpace = VideoInfo::CS_YUV420P12;
        else if (id == pfYUV444P14)    m_Info.ColorSpace = VideoInfo::CS_YUV444P14;
        else if (id == pfYUV422P14)    m_Info.ColorSpace = VideoInfo::CS_YUV422P14;
        else if (id == pfYUV420P14)    m_Info.ColorSpace = VideoInfo::CS_YUV420P14;
        else if (id == pfYUV444P16)    m_Info.ColorSpace = VideoInfo::CS_YUV444P16;
        else if (id == pfYUV422P16)    m_Info.ColorSpace = VideoInfo::CS_YUV422P16;
        else if (id == pfYUV420P16)    m_Info.ColorSpace = VideoInfo::CS_YUV420P16;
        else if (id == pfYUV444PS)     m_Info.ColorSpace = VideoInfo::CS_YUV444PS;
        else if (id == pfGray8)        m_Info.ColorSpace = VideoInfo::CS_Y8;
        else if (id == pfGray16)       m_Info.ColorSpace = VideoInfo::CS_Y16;
        else if (id == pfGrayS)        m_Info.ColorSpace = VideoInfo::CS_Y32;
        else if (id == pfRGB24)        m_Info.ColorSpace = VideoInfo::CS_RGBP;
        else if (id == pfRGB30)        m_Info.ColorSpace = VideoInfo::CS_RGBP10;
        else if (id == pfRGB48)        m_Info.ColorSpace = VideoInfo::CS_RGBP16;
        else if (format.bitsPerSample == 12 && format.colorFamily == cfRGB)
            m_Info.ColorSpace = VideoInfo::CS_RGBP12;
        else if (format.bitsPerSample == 14 && format.colorFamily == cfRGB)
            m_Info.ColorSpace = VideoInfo::CS_RGBP14;
        else if (format.bitsPerSample == 32 && format.colorFamily == cfYUV && format.sampleType == stFloat && format.subSamplingH == 0 && format.subSamplingW == 1)
            m_Info.ColorSpace = VideoInfo::CS_YUV422PS;
        else if (format.bitsPerSample == 32 && format.colorFamily == cfYUV && format.sampleType == stFloat && format.subSamplingH == 1 && format.subSamplingW == 1)
            m_Info.ColorSpace = VideoInfo::CS_YUV420PS;
        else if (format.bitsPerSample == 12 && format.colorFamily == cfGray && format.sampleType == stInteger)
            m_Info.ColorSpace = VideoInfo::CS_Y12;
        else if (format.bitsPerSample == 10 && format.colorFamily == cfGray && format.sampleType == stInteger)
            m_Info.ColorSpace = VideoInfo::CS_Y10;
        else if (format.bitsPerSample == 14 && format.colorFamily == cfGray && format.sampleType == stInteger)
            m_Info.ColorSpace = VideoInfo::CS_Y14;

        return S_OK;
    }
    catch (std::exception& e)
    {
        m_Error = ConvertAnsiToWide(e.what());
    }

    Free();
    return E_FAIL;
}


HRESULT __stdcall VapourSynthServer::GetFrame(int position, void** data, int& pitch)
{
    if (!data)
        return E_POINTER;

    if (!m_vsAPI || !m_vsNode)
        return E_FAIL;

    const VSFrame* frame = m_vsAPI->getFrame(position, m_vsNode, m_vsErrorMessage, sizeof(m_vsErrorMessage));

    if (!frame)
    {
        m_Error = ConvertUtf8ToWide(m_vsErrorMessage);
        return E_FAIL;
    }

    *data = (void*)m_vsAPI->getReadPtr(frame, 0);

    if (!(*data))
    {
        m_Error = L"VapourSynthServer m_vsAPI->getReadPtr returned NULL";
        return E_FAIL;
    }
    
    pitch = m_vsAPI->getStride(frame, 0);

    if (m_vsFrame)
        m_vsAPI->freeFrame(m_vsFrame);

    m_vsFrame = frame;
    return S_OK;
}


ServerInfo* __stdcall VapourSynthServer::GetInfo()
{
    if (!m_vsInfo)
        return nullptr;

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

void __stdcall VapourSynthServer::setError(const char* msg)
{
    m_Error = ConvertUtf8ToWide(msg);
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
        m_vsScriptAPI->freeScript(m_vsScript);
        m_vsScript = NULL;
    }

    if (m_vssDLL)
    {
        FreeLibrary(m_vssDLL);
        m_vssDLL = nullptr;
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