
#include "Common.h"

#include <limits>

namespace
{
    int GetWindowsStringLength(std::size_t length)
    {
        if (length > static_cast<std::size_t>((std::numeric_limits<int>::max)()))
            throw std::length_error("String exceeds the Windows conversion limit");

        return static_cast<int>(length);
    }

    std::string ConvertWideToMultiByte(const std::wstring& source, UINT codePage)
    {
        if (source.empty())
            return {};

        const int sourceLength = GetWindowsStringLength(source.length());
        const int outputLength = WideCharToMultiByte(
            codePage, 0, source.data(), sourceLength, nullptr, 0, nullptr, nullptr);

        if (outputLength == 0)
            throw std::runtime_error("WideCharToMultiByte failed");

        std::string output(outputLength, 0);
        const int convertedLength = WideCharToMultiByte(
            codePage, 0, source.data(), sourceLength, output.data(), outputLength, nullptr, nullptr);

        if (convertedLength != outputLength)
            throw std::runtime_error("WideCharToMultiByte returned an unexpected length");

        return output;
    }

    std::wstring ConvertMultiByteToWide(const std::string& source, UINT codePage)
    {
        if (source.empty())
            return {};

        const int sourceLength = GetWindowsStringLength(source.length());
        const int outputLength = MultiByteToWideChar(
            codePage, 0, source.data(), sourceLength, nullptr, 0);

        if (outputLength == 0)
            throw std::runtime_error("MultiByteToWideChar failed");

        std::wstring output(outputLength, 0);
        const int convertedLength = MultiByteToWideChar(
            codePage, 0, source.data(), sourceLength, output.data(), outputLength);

        if (convertedLength != outputLength)
            throw std::runtime_error("MultiByteToWideChar returned an unexpected length");

        return output;
    }
}

///////////////////// convert strings

std::string ConvertWideToANSI(const std::wstring& wstr)
{
    return ConvertWideToMultiByte(wstr, CP_ACP);
}

std::wstring ConvertAnsiToWide(const std::string& str)
{
    return ConvertMultiByteToWide(str, CP_ACP);
}

std::string ConvertWideToUtf8(const std::wstring& wstr)
{
    return ConvertWideToMultiByte(wstr, CP_UTF8);
}

std::wstring ConvertUtf8ToWide(const std::string& str)
{
    return ConvertMultiByteToWide(str, CP_UTF8);
}

//////////////////// Misc

std::string GetWinErrorMessage(int id)
{
    std::string ret(2048, 0);

    const DWORD length = FormatMessageA(
        FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        nullptr, static_cast<DWORD>(id), MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        ret.data(), static_cast<DWORD>(ret.size()), nullptr);

    ret.resize(length);

    return ret;
}

bool FileExists(LPCWSTR szPath)
{
    DWORD dwAttrib = GetFileAttributes(szPath);

    return (dwAttrib != INVALID_FILE_ATTRIBUTES &&
        !(dwAttrib & FILE_ATTRIBUTE_DIRECTORY));
}
