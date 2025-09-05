
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic

Imports StaxRip.UI

Public Class AppsForm
    Inherits SizeSavingDialogBase
    Implements IUpdateUI

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents tv As TreeViewEx
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents tsbLaunch As ToolStripButton
    Friend WithEvents tsbHelp As ToolStripButton
    Friend WithEvents flp As FlowLayoutPanel
    Friend WithEvents SearchTextBox As StaxRip.SearchTextBox
    Friend WithEvents tsbWebsite As ToolStripButton
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tsbDownload As ToolStripButton
    Friend WithEvents tsbVersion As ToolStripButton
    Friend WithEvents ddbTools As ToolStripDropDownButton
    Friend WithEvents miEditPath As MenuItemEx
    Friend WithEvents miClearPaths As MenuItemEx
    Friend WithEvents miFindPath As MenuItemEx
    Friend WithEvents miShowGrid As MenuItemEx
    Friend WithEvents miStatus As MenuItemEx
    Friend WithEvents miStatusRequired As MenuItemEx
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents miAutoUpdate As MenuItemEx
    Friend WithEvents miEditVersion As MenuItemEx
    Friend WithEvents miEditChangelog As MenuItemEx
    Friend WithEvents miDownload As MenuItemEx
    Friend WithEvents miWebsite As MenuItemEx
    Friend WithEvents miExplore As MenuItemEx
    Friend WithEvents miLaunch As MenuItemEx
    Friend WithEvents miHelp As MenuItemEx
    Friend WithEvents miCopyPath As MenuItemEx
    Friend WithEvents miPATHEnvVar As MenuItemEx
    Friend WithEvents miUpdateRequest As MenuItemEx
    Friend WithEvents tsbExplore As ToolStripButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppsForm))
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.ToolStrip = New ToolStrip()
        Me.tsbLaunch = New ToolStripButton()
        Me.tsbExplore = New ToolStripButton()
        Me.tsbWebsite = New ToolStripButton()
        Me.tsbDownload = New ToolStripButton()
        Me.tsbVersion = New ToolStripButton()
        Me.ddbTools = New ToolStripDropDownButton()
        Me.miEditPath = New StaxRip.UI.MenuItemEx()
        Me.miFindPath = New StaxRip.UI.MenuItemEx()
        Me.miClearPaths = New StaxRip.UI.MenuItemEx()
        Me.miCopyPath = New StaxRip.UI.MenuItemEx()
        Me.miPATHEnvVar = New StaxRip.UI.MenuItemEx()
        Me.ToolStripMenuItem1 = New ToolStripSeparator()
        Me.miEditVersion = New StaxRip.UI.MenuItemEx()
        Me.miEditChangelog = New StaxRip.UI.MenuItemEx()
        Me.miShowGrid = New StaxRip.UI.MenuItemEx()
        Me.miStatus = New StaxRip.UI.MenuItemEx()
        Me.miStatusRequired = New StaxRip.UI.MenuItemEx()
        Me.miAutoUpdate = New StaxRip.UI.MenuItemEx()
        Me.miUpdateRequest = New StaxRip.UI.MenuItemEx()
        Me.miDownload = New StaxRip.UI.MenuItemEx()
        Me.miWebsite = New StaxRip.UI.MenuItemEx()
        Me.miExplore = New StaxRip.UI.MenuItemEx()
        Me.miLaunch = New StaxRip.UI.MenuItemEx()
        Me.miHelp = New StaxRip.UI.MenuItemEx()
        Me.tsbHelp = New ToolStripButton()
        Me.flp = New FlowLayoutPanel()
        Me.SearchTextBox = New StaxRip.SearchTextBox()
        Me.tlpMain = New TableLayoutPanel()
        Me.ToolStrip.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tv
        '
        Me.tv.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tv.AutoCollaps = True
        Me.tv.DrawMode = TreeViewDrawMode.OwnerDrawAll
        Me.tv.ExpandMode = StaxRip.UI.TreeNodeExpandMode.InclusiveChilds
        Me.tv.FullRowSelect = True
        Me.tv.HideSelection = False
        Me.tv.Location = New System.Drawing.Point(10, 101)
        Me.tv.Margin = New Padding(10)
        Me.tv.Name = "tv"
        Me.tv.Scrollable = False
        Me.tv.SelectOnMouseDown = True
        Me.tv.ShowLines = False
        Me.tv.ShowPlusMinus = False
        Me.tv.Size = New System.Drawing.Size(453, 933)
        Me.tv.Sorted = True
        Me.tv.TabIndex = 0
        '
        'ToolStrip
        '
        Me.ToolStrip.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.ToolStrip.AutoSize = False
        Me.ToolStrip.Dock = DockStyle.None
        Me.ToolStrip.GripStyle = ToolStripGripStyle.Hidden
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.ToolStrip.Items.AddRange(New ToolStripItem() {Me.tsbLaunch, Me.tsbExplore, Me.tsbWebsite, Me.tsbDownload, Me.tsbVersion, Me.ddbTools, Me.tsbHelp})
        Me.ToolStrip.Location = New System.Drawing.Point(473, 10)
        Me.ToolStrip.Margin = New Padding(0, 10, 10, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New Padding(5, 2, 2, 0)
        Me.ToolStrip.Size = New System.Drawing.Size(1411, 81)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "tsMain"
        '
        'tsbLaunch
        '
        Me.tsbLaunch.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbLaunch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLaunch.Name = "tsbLaunch"
        Me.tsbLaunch.Size = New System.Drawing.Size(156, 70)
        Me.tsbLaunch.Text = " Launch "
        Me.tsbLaunch.ToolTipText = "Launches the app (Ctrl+L)"
        '
        'tsbExplore
        '
        Me.tsbExplore.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbExplore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExplore.Name = "tsbExplore"
        Me.tsbExplore.Size = New System.Drawing.Size(162, 70)
        Me.tsbExplore.Text = " Explore "
        Me.tsbExplore.ToolTipText = "Opens the apps folder in File Explorer (Ctrl+E)"
        '
        'tsbWebsite
        '
        Me.tsbWebsite.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebsite.Name = "tsbWebsite"
        Me.tsbWebsite.Size = New System.Drawing.Size(137, 70)
        Me.tsbWebsite.Text = "  Web  "
        Me.tsbWebsite.ToolTipText = "Opens the apps website (Ctrl+W)"
        '
        'tsbDownload
        '
        Me.tsbDownload.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbDownload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownload.Name = "tsbDownload"
        Me.tsbDownload.Size = New System.Drawing.Size(205, 70)
        Me.tsbDownload.Text = " Download "
        Me.tsbDownload.ToolTipText = "Opens the apps download web page (Ctrl+D)"
        '
        'tsbVersion
        '
        Me.tsbVersion.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbVersion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVersion.Name = "tsbVersion"
        Me.tsbVersion.Size = New System.Drawing.Size(181, 70)
        Me.tsbVersion.Text = "  Version  "
        Me.tsbVersion.ToolTipText = "Edits the apps version (F12)"
        '
        'ddbTools
        '
        Me.ddbTools.AutoToolTip = False
        Me.ddbTools.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.ddbTools.DropDownItems.AddRange(New ToolStripItem() {Me.miEditPath, Me.miFindPath, Me.miClearPaths, Me.miCopyPath, Me.miPATHEnvVar, Me.ToolStripMenuItem1, Me.miEditVersion, Me.miEditChangelog, Me.miShowGrid, Me.miStatus, Me.miStatusRequired, Me.miAutoUpdate, Me.miUpdateRequest, Me.miDownload, Me.miWebsite, Me.miExplore, Me.miLaunch, Me.miHelp})
        Me.ddbTools.Image = CType(resources.GetObject("ddbTools.Image"), System.Drawing.Image)
        Me.ddbTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ddbTools.Name = "ddbTools"
        Me.ddbTools.Size = New System.Drawing.Size(172, 70)
        Me.ddbTools.Text = "  Tools  "
        '
        'miEditPath
        '
        Me.miEditPath.Help = Nothing
        Me.miEditPath.Name = "miEditPath"
        Me.miEditPath.ShortcutKeys = CType((Keys.Control Or Keys.P), Keys)
        Me.miEditPath.Size = New System.Drawing.Size(594, 67)
        Me.miEditPath.Text = "Edit Path..."
        Me.miEditPath.ToolTipText = "Show Open File dialog to customize the path"
        '
        'miFindPath
        '
        Me.miFindPath.Help = Nothing
        Me.miFindPath.Name = "miFindPath"
        Me.miFindPath.ShortcutKeys = CType((Keys.Control Or Keys.F), Keys)
        Me.miFindPath.Size = New System.Drawing.Size(594, 67)
        Me.miFindPath.Text = "Find Path..."
        Me.miFindPath.ToolTipText = "Find path using voidtools Everything"
        '
        'miClearPaths
        '
        Me.miClearPaths.Help = Nothing
        Me.miClearPaths.Name = "miClearPaths"
        Me.miClearPaths.ShortcutKeys = CType((Keys.Control Or Keys.X), Keys)
        Me.miClearPaths.Size = New System.Drawing.Size(594, 67)
        Me.miClearPaths.Text = "Clear Paths..."
        Me.miClearPaths.ToolTipText = "Clear custom paths"
        '
        'miCopyPath
        '
        Me.miCopyPath.Help = Nothing
        Me.miCopyPath.Name = "miCopyPath"
        Me.miCopyPath.Size = New System.Drawing.Size(594, 67)
        Me.miCopyPath.Text = "Copy Path"
        '
        'miPATHEnvVar
        '
        Me.miPATHEnvVar.Help = Nothing
        Me.miPATHEnvVar.Name = "miPATHEnvVar"
        Me.miPATHEnvVar.Size = New System.Drawing.Size(594, 67)
        Me.miPATHEnvVar.Text = "PATH Env Var..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(591, 6)
        '
        'miEditVersion
        '
        Me.miEditVersion.Help = Nothing
        Me.miEditVersion.Name = "miEditVersion"
        Me.miEditVersion.ShortcutKeys = Keys.F12
        Me.miEditVersion.Size = New System.Drawing.Size(594, 67)
        Me.miEditVersion.Text = "Edit Version"
        Me.miEditVersion.ToolTipText = "Edits the apps version"
        '
        'miEditChangelog
        '
        Me.miEditChangelog.Help = Nothing
        Me.miEditChangelog.Name = "miEditChangelog"
        Me.miEditChangelog.ShortcutKeys = Keys.F10
        Me.miEditChangelog.Size = New System.Drawing.Size(594, 67)
        Me.miEditChangelog.Text = "Edit Changelog"
        '
        'miShowGrid
        '
        Me.miShowGrid.Help = Nothing
        Me.miShowGrid.Name = "miShowGrid"
        Me.miShowGrid.ShortcutKeys = CType((Keys.Control Or Keys.G), Keys)
        Me.miShowGrid.Size = New System.Drawing.Size(594, 67)
        Me.miShowGrid.Text = "Show Grid"
        Me.miShowGrid.ToolTipText = "Show tools in grid view"
        '
        'miStatus
        '
        Me.miStatus.Help = Nothing
        Me.miStatus.Name = "miStatus"
        Me.miStatus.ShortcutKeys = CType((Keys.Control Or Keys.S), Keys)
        Me.miStatus.Size = New System.Drawing.Size(594, 67)
        Me.miStatus.Text = "Check All"
        Me.miStatus.ToolTipText = "Check status of all tools"
        '
        'miStatusRequired
        '
        Me.miStatusRequired.Help = Nothing
        Me.miStatusRequired.Name = "miStatusRequired"
        Me.miStatusRequired.ShortcutKeys = CType((Keys.Control Or Keys.Y), Keys)
        Me.miStatusRequired.Size = New System.Drawing.Size(594, 67)
        Me.miStatusRequired.Text = "Check Required Only"
        Me.miStatusRequired.ToolTipText = "Check status of all required tools"
        '
        'miAutoUpdate
        '
        Me.miAutoUpdate.Help = Nothing
        Me.miAutoUpdate.Name = "miAutoUpdate"
        Me.miAutoUpdate.ShortcutKeys = CType((Keys.Control Or Keys.U), Keys)
        Me.miAutoUpdate.Size = New System.Drawing.Size(594, 67)
        Me.miAutoUpdate.Text = "Auto Update"
        Me.miAutoUpdate.ToolTipText = "Full automatic update"
        '
        'miUpdateRequest
        '
        Me.miUpdateRequest.Name = "miUpdateRequest"
        Me.miUpdateRequest.ShortcutKeys = CType((Keys.Control Or Keys.R), Keys)
        Me.miUpdateRequest.Size = New System.Drawing.Size(594, 66)
        Me.miUpdateRequest.Text = "Update Request"
        '
        'miDownload
        '
        Me.miDownload.Help = Nothing
        Me.miDownload.Name = "miDownload"
        Me.miDownload.ShortcutKeys = CType((Keys.Control Or Keys.D), Keys)
        Me.miDownload.Size = New System.Drawing.Size(594, 67)
        Me.miDownload.Text = "Download"
        Me.miDownload.ToolTipText = "Opens the apps download web page"
        '
        'miWebsite
        '
        Me.miWebsite.Help = Nothing
        Me.miWebsite.Name = "miWebsite"
        Me.miWebsite.ShortcutKeys = CType((Keys.Control Or Keys.W), Keys)
        Me.miWebsite.Size = New System.Drawing.Size(594, 67)
        Me.miWebsite.Text = "Website"
        Me.miWebsite.ToolTipText = "Opens the apps website"
        '
        'miExplore
        '
        Me.miExplore.Help = Nothing
        Me.miExplore.Name = "miExplore"
        Me.miExplore.ShortcutKeys = CType((Keys.Control Or Keys.E), Keys)
        Me.miExplore.Size = New System.Drawing.Size(594, 67)
        Me.miExplore.Text = "Explore"
        Me.miExplore.ToolTipText = "Opens the apps folder in File Explorer"
        '
        'miLaunch
        '
        Me.miLaunch.Help = Nothing
        Me.miLaunch.Name = "miLaunch"
        Me.miLaunch.ShortcutKeys = CType((Keys.Control Or Keys.L), Keys)
        Me.miLaunch.Size = New System.Drawing.Size(594, 67)
        Me.miLaunch.Text = "Launch"
        Me.miLaunch.ToolTipText = "Launches the app"
        '
        'miHelp
        '
        Me.miHelp.Help = Nothing
        Me.miHelp.Name = "miHelp"
        Me.miHelp.ShortcutKeys = Keys.F1
        Me.miHelp.Size = New System.Drawing.Size(594, 67)
        Me.miHelp.Text = "Help"
        Me.miHelp.ToolTipText = "Opens the apps help"
        '
        'tsbHelp
        '
        Me.tsbHelp.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHelp.Name = "tsbHelp"
        Me.tsbHelp.Size = New System.Drawing.Size(139, 70)
        Me.tsbHelp.Text = "  Help  "
        Me.tsbHelp.ToolTipText = "Opens the apps help (F1)"
        '
        'flp
        '
        Me.flp.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.flp.BorderStyle = BorderStyle.FixedSingle
        Me.flp.FlowDirection = FlowDirection.TopDown
        Me.flp.Location = New System.Drawing.Point(473, 101)
        Me.flp.Margin = New Padding(0, 10, 10, 10)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(1411, 933)
        Me.flp.TabIndex = 2
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.SearchTextBox.Location = New System.Drawing.Point(10, 15)
        Me.SearchTextBox.Margin = New Padding(10, 10, 10, 0)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(453, 70)
        Me.SearchTextBox.TabIndex = 4
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 75.0!))
        Me.tlpMain.Controls.Add(Me.tv, 0, 1)
        Me.tlpMain.Controls.Add(Me.flp, 1, 1)
        Me.tlpMain.Controls.Add(Me.ToolStrip, 1, 0)
        Me.tlpMain.Controls.Add(Me.SearchTextBox, 0, 0)
        Me.tlpMain.Dock = DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New RowStyle())
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1894, 1044)
        Me.tlpMain.TabIndex = 6
        '
        'AppsForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1894, 1044)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New Padding(10)
        Me.Name = "AppsForm"
        Me.Text = $"Apps - {g.DefaultCommands.GetApplicationDetails()}"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private CurrentPackage As Package
    Private ReadOnly Nodes As New List(Of TreeNode)
    Private ReadOnly Headers As New Dictionary(Of String, LabelEx)
    Private ReadOnly Contents As New Dictionary(Of String, LabelEx)
    Private ReadOnly SetupButton As New ButtonEx
    Private ToolUpdate As ToolUpdate

    Sub New()
        MyBase.New()
        InitializeComponent()
        RestoreClientSize(48, 32)
        tv.ItemHeight = CInt(FontHeight * 1.3)
        tv.Scrollable = True

        SearchTextBox_TextChanged()
        SearchTextBox.Height = ToolStrip.Height - 2

        AddHandler SearchTextBox.Edit.TextBox.KeyDown, AddressOf SearchTextBoxKeyDown
        AddHandler tv.KeyDown, AddressOf TreeViewKeyDown

        ToolStrip.Font = FontManager.GetDefaultFont()
        g.SetRenderer(ToolStrip)

        miEditChangelog.Visible = g.IsDevelopmentPC

        AddHandler SetupButton.Click, Sub()
                                          CurrentPackage.SetupAction.Invoke
                                          ShowActivePackage()
                                      End Sub

        SetupButton.ForeColor = Color.Red
        SetupButton.TextImageRelation = TextImageRelation.ImageBeforeText
        SetupButton.Image = StockIcon.GetSmallImage(StockIconIdentifier.Shield)
        SetupButton.Font = FontManager.GetDefaultFont(10)
        SetupButton.Margin = New Padding(FontHeight \ 3)
        SetupButton.Padding = New Padding(FontHeight \ 5)
        SetupButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
        SetupButton.AutoSize = True

        Dim titleHeaderLabel = New LabelEx With {
            .Font = FontManager.GetDefaultFont(14, FontStyle.Bold),
            .AutoSize = True
        }

        Headers("Title") = titleHeaderLabel
        flp.Controls.Add(titleHeaderLabel)
        AddSection("Status")
        flp.Controls.Add(SetupButton)
        AddSection("Location", Sub() tsbOpenDir_Click(Nothing, Nothing))
        AddSection("Version", Sub() tsbVersion_Click(Nothing, Nothing))
        AddSection("AviSynth Filters")
        AddSection("VapourSynth Filters")
        AddSection("Filters")
        AddSection("Description")
        AddSection("Website", Sub() g.ShellExecute(CurrentPackage.WebURL))
        AddSection("Help", Sub() g.ShellExecute(CurrentPackage.HelpURL))
        AddSection("Download", Sub() g.ShellExecute(CurrentPackage.DownloadURL))

        ApplyTheme()
        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        BackColor = theme.General.BackColor
    End Sub

    Sub SearchTextBoxKeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyData = Keys.Down Then
            tv.MoveSelectionDown()
            e.Handled = True
        ElseIf e.KeyData = Keys.Up Then
            tv.MoveSelectionUp()
            e.Handled = True
        End If
    End Sub

    Sub TreeViewKeyDown(sender As Object, e As KeyEventArgs)
        If Char.IsLetter(Chr(e.KeyCode)) Then
            SearchTextBox.Text = If(e.Shift, ChrW(e.KeyCode), Char.ToLowerInvariant(Chr(e.KeyCode)))
            SearchTextBox.Focus()
            SearchTextBox.Edit.TextBox.Select(1, 0)            
            e.Handled = True
        End If
    End Sub

    Sub ShowActivePackage()
        If Disposing OrElse IsDisposed Then Exit Sub

        Dim path = CurrentPackage.Path

        Headers("Title").Text = CurrentPackage.Name

        SetupButton.Text = "Install " + CurrentPackage.Name
        SetupButton.Visible = CurrentPackage.SetupAction IsNot Nothing AndAlso CurrentPackage.GetStatus <> ""

        tsbExplore.Enabled = path <> ""
        miExplore.Enabled = tsbExplore.Enabled
        miCopyPath.Enabled = path <> ""
        tsbLaunch.Enabled = CurrentPackage.LaunchAction IsNot Nothing AndAlso CurrentPackage.Path <> ""
        miLaunch.Enabled = tsbLaunch.Enabled
        tsbDownload.Enabled = CurrentPackage.DownloadURL <> ""
        miDownload.Enabled = tsbDownload.Enabled
        miAutoUpdate.Enabled = tsbDownload.Enabled AndAlso CurrentPackage.SupportsAutoUpdate
        tsbWebsite.Enabled = CurrentPackage.URL <> ""

        tsbVersion.Enabled = CurrentPackage.Path.FileExists AndAlso
            Not (CurrentPackage.IsVersionOld() AndAlso Not CurrentPackage.VersionAllowOld)

        miEditPath.Enabled = CurrentPackage.IsCustomPathAllowed
        miFindPath.Enabled = miEditPath.Enabled

        s.StringDictionary("RecentExternalApplicationControl") = CurrentPackage.Name + CurrentPackage.Version

        flp.SuspendLayout()

        Contents("Location").Text = If(path = "", "Not found", path)
        Contents("Description").Text = CurrentPackage.Description

        Dim visible = Not String.IsNullOrWhiteSpace(CurrentPackage.WebURL)
        Headers("Website").Visible = visible
        Contents("Website").Text = CurrentPackage.WebURL
        Contents("Website").Visible = visible

        visible = Not String.IsNullOrWhiteSpace(CurrentPackage.HelpURL)
        Headers("Help").Visible = visible
        Contents("Help").Text = CurrentPackage.HelpURL
        Contents("Help").Visible = visible

        visible = Not String.IsNullOrWhiteSpace(CurrentPackage.DownloadURL)
        Headers("Download").Visible = visible
        Contents("Download").Text = CurrentPackage.DownloadURL
        Contents("Download").Visible = visible

        If File.Exists(CurrentPackage.Path) Then
            Contents("Version").Text = If(CurrentPackage.IsVersionCorrect, CurrentPackage.Version, "Unknown")
            Contents("Version").Text += " (" + File.GetLastWriteTimeUtc(CurrentPackage.Path).ToShortDateString() + ")"
        Else
            Contents("Version").Text = "-"
        End If

        Contents("Status").Text = CurrentPackage.GetStatusDisplay()
        Contents("Status").ForeColor = If(CurrentPackage.GetStatus <> "",
                                                If(CurrentPackage.Required, ThemeManager.CurrentTheme.AppsForm.AttentionForeColor, ThemeManager.CurrentTheme.AppsForm.MinorForeColor),
                                                ThemeManager.CurrentTheme.AppsForm.OkayForeColor)
        Contents("Status").Font = FontManager.GetDefaultFont(10)

        Headers("AviSynth Filters").Visible = False
        Contents("AviSynth Filters").Visible = False

        Headers("VapourSynth Filters").Visible = False
        Contents("VapourSynth Filters").Visible = False

        Headers("Filters").Visible = False
        Contents("Filters").Visible = False

        If TypeOf CurrentPackage Is PluginPackage Then
            Dim plugin = DirectCast(CurrentPackage, PluginPackage)

            If plugin.AvsFilterNames IsNot Nothing AndAlso plugin.VsFilterNames IsNot Nothing Then
                Headers("AviSynth Filters").Visible = True
                Contents("AviSynth Filters").Text = plugin.AvsFilterNames.Join(", ")
                Contents("AviSynth Filters").Visible = True

                Headers("VapourSynth Filters").Visible = True
                Contents("VapourSynth Filters").Text = plugin.VsFilterNames.Join(", ")
                Contents("VapourSynth Filters").Visible = True
            ElseIf plugin.AvsFilterNames IsNot Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.AvsFilterNames.Join(", ")
                Contents("Filters").AutoEllipsis = True
                Contents("Filters").MaximumSize = New Size(Contents("Filters").Parent.Width - Contents("Filters").Left - Contents("Filters").Padding.Left, 333)
                Contents("Filters").Visible = True
            ElseIf plugin.VsFilterNames IsNot Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.VsFilterNames.Join(", ")
                Contents("Filters").AutoEllipsis = True
                Contents("Filters").MaximumSize = New Size(Contents("Filters").Parent.Width - Contents("Filters").Left - Contents("Filters").Padding.Left, 333)
                Contents("Filters").Visible = True
            End If
        End If

        flp.ResumeLayout()
    End Sub

    <STAThread()>
    Sub AddSection(title As String, Optional clickAction As Action = Nothing)
        If String.IsNullOrWhiteSpace(title) Then Return

        Dim controlMargin = CInt(FontHeight / 10)
        Dim headerPadding = CInt(FontHeight / 3)

        Dim headerLabel = New LabelEx With {
            .Text = title,
            .Font = FontManager.GetDefaultFont(9, FontStyle.Bold),
            .AutoSize = True,
            .Margin = New Padding(controlMargin, controlMargin, 0, 0),
            .Padding = New Padding(0, headerPadding, 0, 0)}

        Headers(title) = headerLabel
        flp.Controls.Add(headerLabel)

        Dim contentLabel = If(clickAction IsNot Nothing,
            New ButtonLabel With {
                .AutoSize = True,
                .ClickAction = clickAction,
                .Cursor = Cursors.Hand,
                .Margin = New Padding(controlMargin, CInt(controlMargin / 3), 0, 0)},
            New LabelEx With {
                .AutoSize = True,
                .Margin = New Padding(controlMargin, CInt(controlMargin / 3), 0, 0)})

        Contents(title) = contentLabel
        flp.Controls.Add(contentLabel)
    End Sub

    Sub ShowPackage(package As Package)
        For Each node As TreeNode In tv.Nodes
            If node.IsExpanded Then
                node.Collapse()
            End If
        Next

        For Each i In tv.GetNodes
            If package Is i.Tag Then
                tv.SelectedNode = i
            End If
        Next
    End Sub

    Sub ShowPackage(tn As TreeNode)
        If Not tn Is Nothing AndAlso Not tn.Tag Is Nothing Then
            Dim newPackage = DirectCast(tn.Tag, Package)

            If Not newPackage Is CurrentPackage Then
                CurrentPackage = newPackage
                ShowActivePackage()
            End If
        End If
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        AviSynthToolPathAsync()
    End Sub

    Async Sub AviSynthToolPathAsync()
        Await Task.Run(AddressOf FrameServerHelp.AviSynthToolPath)
        ShowActivePackage()
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        ShowActivePackage()
        Refresh()
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        MyBase.OnDragEnter(e)

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Protected Overrides Sub OnDragDrop(args As DragEventArgs)
        MyBase.OnDragDrop(args)

        Dim files = TryCast(args.Data.GetData(DataFormats.FileDrop), String())

        BeginInvoke(Sub()
                        If Not files.NothingOrEmpty Then
                            If files.Length = 1 AndAlso files(0).Ext.EqualsAny("zip", "7z") Then

                                ToolUpdate = New ToolUpdate(CurrentPackage, Me)
                                ToolUpdate.DownloadFile = files(0)
                                ToolUpdate.Extract()
                            Else
                                ToolUpdate = New ToolUpdate(CurrentPackage, Me)
                                ToolUpdate.ExtractDir = Path.Combine(Folder.Temp, Guid.NewGuid.ToString())
                                Directory.CreateDirectory(ToolUpdate.ExtractDir)

                                For Each i In files
                                    FileHelp.Copy(i, Path.Combine(ToolUpdate.ExtractDir, i.FileName))
                                Next

                                ToolUpdate.DeleteOldFiles()
                                FolderHelp.Delete(ToolUpdate.ExtractDir)
                            End If
                        End If
                    End Sub)
    End Sub

    Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
        If e.Node.Tag Is Nothing AndAlso e.Node.Nodes.Count > 0 Then
            tv.SelectedNode = e.Node.Nodes(0)
        End If

        If Not e.Node.Tag Is Nothing Then
            ShowPackage(e.Node)
        End If
    End Sub

    Sub SearchTextBox_TextChanged() Handles SearchTextBox.TextChanged
        Dim current = CurrentPackage

        tv.BeginUpdate()
        tv.Nodes.Clear()

        For Each pack In Package.Items.Values
            Dim plugin = TryCast(pack, PluginPackage)
            Dim searchString = pack.Name + pack.Description + pack.Version + pack.WebURL + plugin?.VsFilterNames.Join(" ") + pack.Path + plugin?.AvsFilterNames.Join(" ")
            Dim newVersion = Not s.AllowToolsWithWrongVersion AndAlso Not pack.IsVersionValid()
            Dim notFound = pack.GetStatusLocation() <> ""

            If searchString?.ToLowerInvariant.Contains(SearchTextBox.Text?.ToLowerInvariant) OrElse
            (newVersion AndAlso SearchTextBox.Text?.ToLowerInvariant.Contains("<newversion>")) OrElse
            (notFound AndAlso SearchTextBox.Text?.ToLowerInvariant.Contains("<notfound>")) Then
                If plugin Is Nothing Then
                    If pack.TreePath <> "" Then
                        Dim n = tv.AddNode(pack.TreePath + "|" + pack.Name)
                        Nodes.Add(n)
                        n.Tag = pack
                    Else
                        Dim n = tv.AddNode("Apps|" + pack.Name)
                        Nodes.Add(n)
                        n.Tag = pack
                    End If
                Else
                    If plugin.AvsFilterNames?.Length >= 0 Then
                        Dim n = tv.AddNode("AviSynth|" + pack.Name)
                        Nodes.Add(n)
                        n.Tag = pack
                    End If

                    If plugin.VsFilterNames?.Length >= 0 Then
                        Dim n = tv.AddNode("VapourSynth|" + pack.Name)
                        Nodes.Add(n)
                        n.Tag = pack
                    End If
                End If
            End If
        Next

        If tv.Nodes.Count > 0 Then
            tv.SelectedNode = tv.Nodes(0)
        End If

        ToolStrip.Enabled = tv.Nodes.Count > 0
        flp.Enabled = tv.Nodes.Count > 0

        If SearchTextBox.Text <> "" Then
            tv.ExpandAll()
        Else
            ShowPackage(current)
        End If

        tv.EndUpdate()
    End Sub

    Sub tsbLaunch_Click(sender As Object, e As EventArgs) Handles tsbLaunch.Click
        CurrentPackage.LaunchAction?.Invoke()
    End Sub

    <DebuggerNonUserCode()>
    Sub tsbOpenDir_Click(sender As Object, e As EventArgs) Handles tsbExplore.Click
        g.SelectFileWithExplorer(CurrentPackage.Path)
    End Sub

    Sub tsbHelp_Click(sender As Object, e As EventArgs) Handles tsbHelp.Click
        CurrentPackage.ShowHelp()
    End Sub

    Sub tsbWebsite_Click(sender As Object, e As EventArgs) Handles tsbWebsite.Click
        g.ShellExecute(CurrentPackage.URL)
    End Sub

    Sub tsbDownload_Click(sender As Object, e As EventArgs) Handles tsbDownload.Click
        If CurrentPackage.DownloadURL <> "" Then
            g.ShellExecute(CurrentPackage.DownloadURL)
        End If
    End Sub

    Sub tsbVersion_Click(sender As Object, e As EventArgs) Handles tsbVersion.Click
        If Not File.Exists(CurrentPackage.Path) Then
            Exit Sub
        End If

        Dim version = FileVersionInfo.GetVersionInfo(CurrentPackage.Path)

        Dim fileVersionString = version.FileMajorPart & "." & version.FileMinorPart & "." &
                                version.FileBuildPart & "." & version.FilePrivatePart

        Dim productVersionString = version.ProductMajorPart & "." & version.ProductMinorPart & "." &
                                   version.ProductBuildPart & "." & version.ProductPrivatePart
        Dim msg = ""

        If fileVersionString <> "0.0.0.0" Then
            msg += BR2 + "File Version: " + fileVersionString + " (often not correct!)"
        End If

        If productVersionString <> "0.0.0.0" Then
            msg += BR2 + "Product Version: " + productVersionString + " (often not correct!)"
        End If

        Dim input = InputBox.Show("What's the name of this version?", CurrentPackage.Version, msg?.Trim)

        If input <> "" Then
            CurrentPackage.SetVersion(input.Replace(";", "_").Trim)
            ShowActivePackage()
            Application.DoEvents()
        End If
    End Sub

    Sub miShowGridView_Click(sender As Object, e As EventArgs) Handles miShowGrid.Click
        Dim rows As New List(Of Object)

        For Each pack In Package.Items.Values.OrderBy(Function(i) i.GetTypeName)
            Dim row = New With {.Name = "", .Type = "", .Filename = "",
                .Version = "", .ModifiedDate = "", .Folder = ""}

            row.Name = pack.Name
            row.Type = pack.GetTypeName
            row.Filename = pack.Filename
            row.Folder = pack.Directory

            If pack.IsVersionCorrect Then
                row.Version = "'" + pack.Version + "'"
            End If

            If File.Exists(pack.Path) Then
                row.ModifiedDate = File.GetLastWriteTime(pack.Path).ToString("yyyy-MM-dd")
            End If

            rows.Add(row)
        Next

        Using td As New TaskDialog(Of String)
            td.Title = "Choose how to show"
            td.AddCommand("Show as CSV file", "csv")
            td.AddCommand("Show using PowerShell", "ogv")

            Select Case td.Show
                Case "csv"
                    Dim csvFile = Path.Combine(Folder.Temp, "staxrip tools.csv")
                    g.ConvertToCSV(";", rows).WriteFileUTF8(csvFile)
                    g.ShellExecute(g.GetAppPathForExtension("csv", "txt"), csvFile.Escape)
                Case "ogv"
                    g.InvokePowerShellCode($"$objects | Out-GridView", "objects", rows)
            End Select
        End Using
    End Sub

    Sub miStatus_Click(sender As Object, e As EventArgs) Handles miStatus.Click
        Dim counter As Integer = 0
        Dim txt = ""

        For Each pair In Package.Items
            Dim pack = pair.Value

            If pack.GetStatus <> "" Then
                txt += pack.Name + ": " + pack.GetStatus + BR2
                counter += 1
            End If
        Next

        If txt = "" Then
            MsgInfo("OK!", "All tools have OK status!")
        Else
            MsgInfo($"{counter} apps found!", txt)
        End If
    End Sub

    Sub miStatusRequired_Click(sender As Object, e As EventArgs) Handles miStatusRequired.Click
        Dim counter As Integer = 0
        Dim txt = ""

        For Each pair In Package.Items
            Dim pack = pair.Value

            If pack.GetStatus <> "" AndAlso pack.Required Then
                txt += pack.Name + ": " + pack.GetStatus + BR2
                counter += 1
            End If
        Next

        If txt = "" Then
            MsgInfo("OK!", "All required tools have OK status!")
        Else
            MsgInfo($"{counter} apps found!", txt)
        End If
    End Sub

    Sub miBrowsePath_Click(sender As Object, e As EventArgs) Handles miEditPath.Click
        Using dialog As New OpenFileDialog
            dialog.SetInitDir(s.Storage.GetString(CurrentPackage.Name + "custom path"))

            If CurrentPackage.Filter <> "" Then
                dialog.Filter = CurrentPackage.Filter
            Else
                dialog.Filter = "|" + CurrentPackage.Filename + "|All Files|*.*"
            End If

            If dialog.ShowDialog = DialogResult.OK Then
                If Not s.AllowCustomPathsInStartupFolder AndAlso
                    dialog.FileName.ToLowerEx.StartsWithEx(Folder.Startup.ToLowerEx + Path.DirectorySeparatorChar) AndAlso
                    Not dialog.FileName.ToLowerEx.StartsWithEx(Folder.Settings.ToLowerEx + Path.DirectorySeparatorChar) Then

                    MsgError("Custom paths within the startup folder are not permitted " +
                             "because it would prevent a simple update process." + BR2 +
                             "Please put the file somewhere else outside the startup folder.")
                    Exit Sub
                End If

                s.Storage.SetString(CurrentPackage.Name + "custom path", dialog.FileName)
                ShowActivePackage()
            End If
        End Using
    End Sub

    Sub miClearCustomPath_Click(sender As Object, e As EventArgs) Handles miClearPaths.Click
        Dim packs = Package.Items.Values.Where(Function(pack) pack.GetStoredPath() <> "")

        If packs.Count > 0 Then
            Using td As New TaskDialog(Of Package)
                td.Title = "Choose a path to be cleared."

                For Each pack In packs
                    td.AddCommand(pack.Name, pack.GetStoredPath, pack)
                Next

                If Not td.Show Is Nothing Then
                    td.SelectedValue.SetStoredPath(Nothing)
                    ShowActivePackage()
                End If
            End Using
        Else
            MsgInfo("No custom paths defined.")
        End If
    End Sub

    Sub miFindPath_Click(sender As Object, e As EventArgs) Handles miFindPath.Click
        Everything()
    End Sub

    Async Sub Everything()
        Dim paths As New List(Of String)

        Dim run = Sub()
                      Dim size = 500
                      Dim sb As New StringBuilder(size)

                      Try
                          Everything_SetSearch(CurrentPackage.Filename)
                          Everything_SetRequestFlags(EVERYTHING_REQUEST_FILE_NAME Or EVERYTHING_REQUEST_PATH)
                          Everything_Query(True)

                          For x = 0 To Everything_GetNumResults() - 1
                              Everything_GetResultFullPathName(CUInt(x), sb, CUInt(size))
                              Dim path = sb.ToString

                              If path.FileName.ToLowerInvariant = CurrentPackage.Filename.ToLowerInvariant Then
                                  paths.Add(path)
                              End If

                              If paths.Count > 9 Then
                                  Exit For
                              End If
                          Next
                      Catch
                          If MsgQuestion("The Find Path feature requires the installation of a tool named voidtools Everything." + BR2 +
                                         "Open the voidtools Everything website?") = DialogResult.OK Then
                              g.ShellExecute("https://www.voidtools.com")
                          End If

                          If MsgQuestion("Would you like to open the website of Everything.NET?",
                                         "Everything.NET is a voidtools Everything frontend that supports dark mode.") = DialogResult.OK Then
                              g.ShellExecute("https://github.com/stax76/Everything.NET")
                          End If
                      End Try
                  End Sub

        Await Task.Run(run)

        If Disposing OrElse IsDisposed Then
            Exit Sub
        End If

        If paths.Count > 0 Then
            Using td As New TaskDialog(Of String)
                td.Title = "Choose Path"

                For Each path In paths
                    td.AddCommand("", path, path)
                Next

                If td.Show.FileExists Then
                    If Not s.AllowCustomPathsInStartupFolder AndAlso
                        td.SelectedValue.ToLowerEx.StartsWithEx(Folder.Startup.ToLowerEx + Path.DirectorySeparatorChar) AndAlso
                        Not td.SelectedValue.ToLowerEx.StartsWithEx(Folder.Settings.ToLowerEx + Path.DirectorySeparatorChar) Then

                        MsgError("Custom paths within the startup folder are not permitted.")
                        Exit Sub
                    End If

                    CurrentPackage.SetStoredPath(td.SelectedValue)
                    ShowActivePackage()
                ElseIf td.SelectedValue <> "" Then
                    MsgError("File not found", td.SelectedValue)
                End If
            End Using
        Else
            MsgInfo("Nothing found.")
        End If
    End Sub

    Const EVERYTHING_REQUEST_FILE_NAME As Integer = 1
    Const EVERYTHING_REQUEST_PATH As Integer = 2

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Shared Function Everything_SetSearch(lpSearchString As String) As Integer
    End Function

    <DllImport("Everything.dll")>
    Shared Sub Everything_SetRequestFlags(dwRequestFlags As UInt32)
    End Sub

    <DllImport("Everything.dll")>
    Shared Sub Everything_SetSort(dwSortType As UInt32)
    End Sub

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Shared Function Everything_Query(bWait As Boolean) As Boolean
    End Function

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Shared Sub Everything_GetResultFullPathName(nIndex As UInt32, lpString As StringBuilder, nMaxCount As UInt32)
    End Sub

    <DllImport("Everything.dll")>
    Shared Function Everything_GetResultSize(nIndex As UInt32, ByRef lpFileSize As Long) As Boolean
    End Function

    <DllImport("Everything.dll")>
    Shared Function Everything_GetNumResults() As UInt32
    End Function

    Sub miEditVersion_Click(sender As Object, e As EventArgs) Handles miEditVersion.Click
        tsbVersion.PerformClick()
    End Sub

    Sub miAutoUpdate_Click(sender As Object, e As EventArgs) Handles miAutoUpdate.Click
        Dim url = CurrentPackage.DownloadURL

        If url = "" Then
            Exit Sub
        End If

        If url.Contains("mediafire") Then
            MsgError("The auto update feature does currently not support MediaFire.")
            Exit Sub
        End If

        If MsgQuestion("Experimental feature not working for all tools, continue?") = DialogResult.OK Then
            ToolUpdate = New ToolUpdate(CurrentPackage, Me)
            ToolUpdate.Update()
        End If
    End Sub

    Sub miEditChangelog_Click(sender As Object, e As EventArgs) Handles miEditChangelog.Click
        Dim path = IO.Path.Combine(Folder.Startup, "..", "Changelog.md")

        If File.Exists(path) Then
            g.ShellExecute(path)
        End If
    End Sub

    Sub UpdateUI() Implements IUpdateUI.UpdateUI
        ShowActivePackage()
        Refresh()
        Application.DoEvents()
    End Sub

    Sub miHelp_Click(sender As Object, e As EventArgs) Handles miHelp.Click
        tsbHelp.PerformClick()
    End Sub

    Sub miDownload_Click(sender As Object, e As EventArgs) Handles miDownload.Click
        tsbDownload.PerformClick()
    End Sub

    Sub miWebsite_Click(sender As Object, e As EventArgs) Handles miWebsite.Click
        tsbWebsite.PerformClick()
    End Sub

    Sub miExplore_Click(sender As Object, e As EventArgs) Handles miExplore.Click
        tsbExplore.PerformClick()
    End Sub

    Sub miLaunch_Click(sender As Object, e As EventArgs) Handles miLaunch.Click
        tsbLaunch.PerformClick()
    End Sub

    Sub miCopyPath_Click(sender As Object, e As EventArgs) Handles miCopyPath.Click
        Clipboard.SetText(CurrentPackage.Path)
        MsgInfo("The path was copied to the clipboard.")
    End Sub

    Sub miPATHEnvVar_Click(sender As Object, e As EventArgs) Handles miPATHEnvVar.Click
        'crash report by user
        Try
            'no EnvironmentVariableTarget.User on unix systems, Machine would require admin permissions. process is not persistet -> conditional check to not break windows behaviour.
            Dim pathTarget = If(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), EnvironmentVariableTarget.User, EnvironmentVariableTarget.Process)
            Dim dir = CurrentPackage.Directory
            Dim pathVar = If(Environment.GetEnvironmentVariable("path", pathTarget), "")
            Dim pathItems = pathVar.Split(New String() {Path.PathSeparator}, StringSplitOptions.RemoveEmptyEntries).ToList()

            Using td As New TaskDialog(Of String)
                td.Title = "Modify the process PATH environment variable"
                td.AddCommand("Add", $"Add {CurrentPackage.Name} to process PATH environment variable", "add")
                td.AddCommand("Remove", $"Remove {CurrentPackage.Name} from process PATH environment variable", "remove")
                td.AddCommand("Editor", "Show environment variable editor", "edit")

                Select Case td.Show
                    Case "add"
                        If pathItems.Contains(dir) Then
                            MsgError("Folder is already in PATH")
                        Else
                            pathItems.Add(dir)
                            Environment.SetEnvironmentVariable("path", String.Join(Path.PathSeparator, pathItems), pathTarget)
                            MsgInfo("Folder was added to PATH")
                        End If
                    Case "remove"
                        If pathItems.Contains(dir) Then
                            pathItems.Remove(dir)
                            Environment.SetEnvironmentVariable("path", String.Join(Path.PathSeparator, pathItems), pathTarget)
                            MsgInfo("Folder was removed from PATH")
                        Else
                            MsgError("Folder is not in PATH")
                        End If
                    Case "edit"
                        g.Execute("rundll32.exe", "sysdm.cpl,EditEnvironmentVariables")
                End Select
            End Using
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Sub miUpdateRequest_Click(sender As Object, e As EventArgs) Handles miUpdateRequest.Click
        g.ShowWikiPage("Tool-Update-Requests")
    End Sub
End Class
