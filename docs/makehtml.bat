
SET PATH=%PATH%;C:\Program Files (x86)\Google\Chrome\Application
rmdir _build
call make html
chrome.exe D:\Projekte\VB\StaxRip\docs\_build\html\index.html
pause
