
@echo off
SET PATH=%PATH%;C:\Program Files (x86)\Google\Chrome\Application
rmdir /S /Q _build
call make html
chrome.exe %~dp0\_build\html\index.html
pause
