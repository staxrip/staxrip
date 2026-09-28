Imports StaxRip.UI

Public Class SupportReportForm
    Inherits FormBase

    Private ReadOnly IncludeCommandsCheckBox As New CheckBox()
    Private ReadOnly NoticeLabel As New Label()
    Private ReadOnly ReportTextBox As New RichTextBoxEx()
    Private ReadOnly CopyButton As New ButtonEx()
    Private ReadOnly CloseButton As New ButtonEx()
    Private ReadOnly LayoutPanel As New TableLayoutPanel()
    Private ReadOnly ButtonPanel As New FlowLayoutPanel()

    Sub New()
        Text = $"Support Report Preview - {g.DefaultCommands.GetApplicationDetails()}"
        ShowIcon = False
        StartPosition = FormStartPosition.CenterParent
        KeyPreview = True
        RestoreClientSize(58, 42)

        NoticeLabel.AutoSize = True
        NoticeLabel.Dock = DockStyle.Fill
        NoticeLabel.Margin = New Padding(12, 10, 12, 4)
        NoticeLabel.Text = "Choose the command option before editing. Review the report before sharing it. Raw logs, paths, names, titles, scripts, and custom text are excluded by default."

        IncludeCommandsCheckBox.AutoSize = True
        IncludeCommandsCheckBox.Margin = New Padding(12, 4, 12, 8)
        IncludeCommandsCheckBox.Text = "Include sanitized command summaries (manual review required)"

        ReportTextBox.AcceptsTab = True
        ReportTextBox.DetectUrls = False
        ReportTextBox.Dock = DockStyle.Fill
        ReportTextBox.Font = FontManager.GetCodeFont()
        ReportTextBox.Margin = New Padding(12, 0, 12, 8)
        ReportTextBox.WordWrap = False

        CopyButton.AutoSize = True
        CopyButton.Text = "Copy Report"
        CloseButton.AutoSize = True
        CloseButton.DialogResult = DialogResult.Cancel
        CloseButton.Text = "Close"

        ButtonPanel.AutoSize = True
        ButtonPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonPanel.Dock = DockStyle.Fill
        ButtonPanel.FlowDirection = FlowDirection.RightToLeft
        ButtonPanel.Margin = New Padding(12, 0, 12, 10)
        ButtonPanel.Controls.Add(CloseButton)
        ButtonPanel.Controls.Add(CopyButton)

        LayoutPanel.ColumnCount = 1
        LayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        LayoutPanel.Dock = DockStyle.Fill
        LayoutPanel.RowCount = 4
        LayoutPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        LayoutPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        LayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        LayoutPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        LayoutPanel.Controls.Add(NoticeLabel, 0, 0)
        LayoutPanel.Controls.Add(IncludeCommandsCheckBox, 0, 1)
        LayoutPanel.Controls.Add(ReportTextBox, 0, 2)
        LayoutPanel.Controls.Add(ButtonPanel, 0, 3)
        Controls.Add(LayoutPanel)

        AcceptButton = CopyButton
        CancelButton = CloseButton

        AddHandler IncludeCommandsCheckBox.CheckedChanged, AddressOf RefreshReport
        AddHandler CopyButton.Click, AddressOf CopyReport
        AddHandler ReportTextBox.TextChanged, AddressOf ReportChanged
        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged

        RefreshReport()
        ApplyTheme()
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        MyBase.Dispose(disposing)
    End Sub

    Private Sub RefreshReport()
        ReportTextBox.Text = SupportReportBuilder.Build(p, New SupportReportOptions With {
            .IncludeSanitizedCommands = IncludeCommandsCheckBox.Checked
        })
        ReportTextBox.SelectionStart = 0
        ReportTextBox.SelectionLength = 0
    End Sub

    Private Sub RefreshReport(sender As Object, args As EventArgs)
        RefreshReport()
    End Sub

    Private Sub CopyReport(sender As Object, args As EventArgs)
        ReportTextBox.Text.ToClipboard()
        CopyButton.Text = "Copied"
    End Sub

    Private Sub ReportChanged(sender As Object, args As EventArgs)
        CopyButton.Text = "Copy Report"
    End Sub

    Private Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Private Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Private Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then Return

        BackColor = theme.General.BackColor
        ForeColor = theme.General.ForeColor
        LayoutPanel.BackColor = theme.General.BackColor
        ButtonPanel.BackColor = theme.General.BackColor
        NoticeLabel.ForeColor = theme.General.ForeColor
        IncludeCommandsCheckBox.ForeColor = theme.General.ForeColor
    End Sub
End Class
