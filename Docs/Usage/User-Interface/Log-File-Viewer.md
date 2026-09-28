# [Documentation](../../README.md) / [Usage](../README.md) / [User Interface](README.md) / Log File Viewer

The Log File Viewer shows the current job log. Open it with `Project > Log File` or press `F8`.

Job logs are useful for troubleshooting, but they can contain file paths, file names, media metadata, scripts, command lines, tool output, and system details. Do not post a raw log without reviewing it.

## Support report preview

Open `Help > Support Report Preview...` to create an editable Markdown report for a GitHub issue or support discussion. You can also open the Log File Viewer, right-click the log, and select `Preview Support Report...`.

The report is generated locally from a fixed allowlist. StaxRip does not upload it. The report includes:

- the StaxRip, settings, Windows, and process-architecture versions;
- the configured frame server, encoder, muxer, cutting, and audio path;
- technical video and selected audio-stream properties;
- versions of core and configured encoders, plus versioned tools named in the current log;
- numeric failing exit codes found in the current log;
- prompts for the observations that StaxRip cannot determine automatically.

The default report does not include:

- paths, file names, source names, or target names;
- media titles, stream titles, template names, or profile names;
- scripts, custom switches, event commands, or other free-form text;
- raw tool output, exception text, or the complete job log;
- user names, machine names, URLs, credentials, or file hashes.

Technical properties such as codec, dimensions, duration, and stream layout can still describe the source. Review the preview and remove anything you do not want to publish.

### Command summaries

The command option is off by default. Enable `Include sanitized command summaries` before editing the report when a maintainer needs the generated encoder arguments.

StaxRip omits standard paths and replaces detected custom or text-valued arguments. It also removes Windows paths, URLs, email addresses, and common credential arguments. This is a best-effort safeguard because command syntax is extensible. Review every command before sharing it.

Muxer commands are not included because muxer profiles can contain complete free-form command templates.

## Copying a report

1. Reproduce the problem or load the affected project.
2. Open `Help > Support Report Preview...`.
3. Decide whether sanitized commands are necessary.
4. Complete the `Reporter notes` section in the preview.
5. Remove any technical detail you do not want to publish.
6. Select `Copy Report` and paste the result into the issue form.

For intermittent problems, state how often the failure occurs. For media corruption, describe the symptom, the first affected timestamp, and how you checked the output. If a manual command works, say whether it used the same executable version and input method as StaxRip.

## Other Log File Viewer actions

Right-click the log to access these actions:

- `Save As...` saves the raw displayed log.
- `Save Obfuscated As...` replaces the current source path and source name before saving.
- `Preview Support Report...` creates the structured report described above.
- `Open in Text Editor` opens the current raw log in the configured editor.
- `Show in File Explorer` selects the current raw log.
- `Show History` opens the saved log folder.

`Save Obfuscated As...` is not a complete privacy filter. Unrelated target paths, scripts, command lines, tool output, and system details can remain. Use the support report preview for a public issue. Attach an obfuscated log only when a maintainer needs it and only after reviewing the saved file.

[Back to User Interface](README.md)
