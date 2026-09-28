# [Documentation](../README.md) / [Support](README.md) / Bug Reports

A useful bug report identifies the failing stage and gives another person enough information to distinguish a StaxRip problem from a source, setting, plugin, or external-tool problem.

## Before opening an issue

1. Reproduce the problem with the latest release when practical.
2. Search the issue tracker and changelog for the same symptom.
3. Try a clean built-in template when that will not risk your source or output.
4. Record whether the problem affects one source or several sources.
5. Keep the smallest sample that still reproduces the problem, but share it only when you have permission.

## Create the support report

Open `Help > Support Report Preview...`. The report captures the current project and selected stream, so create it while the affected project is loaded. Complete the reporter prompts, review the text, and select `Copy Report`.

The command option is off by default. Enable it only when the generated arguments help explain the failure. Sanitized commands are still a best-effort export and require manual review.

Older StaxRip releases may not provide the support report. If a maintainer requests a log, use `Project > Log File > Save Obfuscated As...` and review the saved file before attaching it. Do not attach the raw log by default.

## Details StaxRip cannot supply

Add these facts yourself:

- what you saw or heard, not only that the output was "wrong";
- what you expected;
- whether the failure is repeatable and how often it occurs;
- the first failing and last known working StaxRip versions;
- whether a clean template and a different source reproduce it;
- the first affected timestamp or processing stage;
- whether a manual command used the same executable version and input method.

## Details by problem type

### Audio corruption or synchronization

Describe whether the result contains noise, silence, clicks, missing channels, drift, a constant offset, or a duration mismatch. State the first affected timestamp. Compare the input, intermediate, and final files when the workflow creates them.

If a manual FFmpeg command works, state whether it used the same FFmpeg executable, stream mapping, input file or pipe, decoder, sample format, channel layout, and output options. A command that looks similar but uses a different executable or input method is not an exact comparison.

### Encoder or external-tool failure

Include the failing stage, decimal and hexadecimal exit code, tool version, and whether the same source fails every time. If the tool crashes, state whether other sources and a simpler profile also crash.

### AviSynth+ or VapourSynth

Select the frame server that was active. Include the relevant runtime and plugin versions. State whether previewing the script fails before encoding begins. Do not paste a script until you have removed paths, media names, comments, credentials, and custom code you do not want to publish.

### GUI, jobs, projects, or settings

List the exact controls used and the order of actions. State whether restarting StaxRip or using a clean settings directory changes the result. Attach a screenshot only after checking window titles, paths, job names, and media names.

### Updates and included tools

Identify the component, previous version, new version, and observed failure. State whether restoring the previous component changes the result. Do not publish download URLs that contain credentials or signed query parameters.

## Samples and attachments

Use synthetic or freely redistributable media when possible. A small sample is better than a complete source. Check samples for private audio, subtitles, chapters, cover art, titles, tags, and location or device metadata.

GitHub issues are public. Remove personal paths, names, network locations, credentials, private media details, and copyrighted material you cannot redistribute.

[Log File Viewer and report details](../Usage/User-Interface/Log-File-Viewer.md)
