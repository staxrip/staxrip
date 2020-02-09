
#pragma once

#define WIN32_LEAN_AND_MEAN

#include <Windows.h>

#include <string>

///////////////////// convert strings

std::string  ConvertWideToANSI(const std::wstring& wstr);
std::wstring ConvertAnsiToWide(const std::string& str);

std::string  ConvertWideToUtf8(const std::wstring& wstr);
std::wstring ConvertUtf8ToWide(const std::string& str);
