---
name: Bug report
about: Create a report to help us improve
title: ''
labels: Bug
assignees: stax76

---

**Describe the bug**
A clear and concise description of what the bug is.


**Expected behavior**
A clear and concise description of what you expected to happen.


**How to reproduce the issue**

Test if the bug happens only with a particular source file or source file type, if so then upload a sample file. If you cannot upload the file then please install the application https://github.com/stax76/MediaInfo.NET and post media info about the source file.

If the issue happens only under special conditions then please describe how this conditions can be reproduced:

1. Go to '...'
2. Click on '...'
3. Using '...' encoder.
4. 'AviSynth or VapourSynth' being used.
5. Filter(s) x is being used.
6. If it's tied to a hardware encoder then post your hardware specs.


**Provide information**

If there is a error message than copy the message if possible, if not then make a screenshot. Ater that search for the log file because it should not only contain the error message but also additional useful debug information. The log file is located in the temp folder and the temp folder is located in the same directory as the source file. The log file ends with _staxrip.log. Visit the wesite pastebin.com and paste the log file content there, use the auto expire feature and set it to one year.


**Notes before posting**
- Only open an issue if it's tied to StaxRip, if the issue is tied to an external program or filter then head to that project and open an issue there. 
- If you require help with one of the frame servers, tools or scripting then post in the forums rather then opening a ticket here:
  https://forum.videohelp.com
  https://forum.doom9.org
- Check your CPU specs, many newer filters are AVX2 enabled with no AVX1 support, this is espeically true on the VapourSynth side, which will crash if your CPU don't support it.
- Lastly, only files that are released with StaxRip are fully supported, this is done to make debugging easier. Also since all filters have been tested before the release, newer versions can have changes which have not been added to StaxRip yet.


**Additional context**
Add any other information or additional context that can be helpful.


**Please be as clear and as detailed as possible**
