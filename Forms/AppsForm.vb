
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks

Imports StaxRip.UI

Public Class AppsForm
    Inherits DialogBase

#Region " Designer "
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents tv As TreeViewEx
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbLaunch As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbHelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents flp As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SearchTextBox As StaxRip.SearchTextBox
    Friend WithEvents tsbWebsite As System.Windows.Forms.ToolStripButton
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tsbDownload As ToolStripButton
    Friend WithEvents tsbVersion As ToolStripButton
    Friend WithEvents ddbTools As ToolStripDropDownButton
    Friend WithEvents miShowGridView As MenuItemEx
    Friend WithEvents miStatus As MenuItemEx
    Friend WithEvents ddbPath As ToolStripDropDownButton
    Friend WithEvents miBrowsePath As MenuItemEx
    Friend WithEvents miClearCustomPath As MenuItemEx
    Friend WithEvents miSearchUsingEverything As MenuItemEx
    Friend WithEvents tsbExplore As System.Windows.Forms.ToolStripButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppsForm))
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.tsbLaunch = New System.Windows.Forms.ToolStripButton()
        Me.tsbExplore = New System.Windows.Forms.ToolStripButton()
        Me.tsbWebsite = New System.Windows.Forms.ToolStripButton()
        Me.tsbDownload = New System.Windows.Forms.ToolStripButton()
        Me.tsbVersion = New System.Windows.Forms.ToolStripButton()
        Me.ddbPath = New System.Windows.Forms.ToolStripDropDownButton()
        Me.miBrowsePath = New StaxRip.UI.MenuItemEx()
        Me.miClearCustomPath = New StaxRip.UI.MenuItemEx()
        Me.miSearchUsingEverything = New StaxRip.UI.MenuItemEx()
        Me.ddbTools = New System.Windows.Forms.ToolStripDropDownButton()
        Me.miShowGridView = New StaxRip.UI.MenuItemEx()
        Me.miStatus = New StaxRip.UI.MenuItemEx()
        Me.tsbHelp = New System.Windows.Forms.ToolStripButton()
        Me.flp = New System.Windows.Forms.FlowLayoutPanel()
        Me.SearchTextBox = New StaxRip.SearchTextBox()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tv
        '
        Me.tv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tv.AutoCollaps = True
        Me.tv.BackColor = System.Drawing.SystemColors.Control
        Me.tv.ExpandMode = StaxRip.UI.TreeNodeExpandMode.InclusiveChilds
        Me.tv.FullRowSelect = True
        Me.tv.HideSelection = False
        Me.tv.Location = New System.Drawing.Point(10, 100)
        Me.tv.Margin = New System.Windows.Forms.Padding(10)
        Me.tv.Name = "tv"
        Me.tv.Scrollable = False
        Me.tv.SelectOnMouseDown = True
        Me.tv.ShowLines = False
        Me.tv.ShowPlusMinus = False
        Me.tv.Size = New System.Drawing.Size(453, 1018)
        Me.tv.Sorted = True
        Me.tv.TabIndex = 0
        '
        'ToolStrip
        '
        Me.ToolStrip.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip.AutoSize = False
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbLaunch, Me.tsbExplore, Me.tsbWebsite, Me.tsbDownload, Me.tsbVersion, Me.ddbPath, Me.ddbTools, Me.tsbHelp})
        Me.ToolStrip.Location = New System.Drawing.Point(473, 10)
        Me.ToolStrip.Margin = New System.Windows.Forms.Padding(0, 10, 10, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New System.Windows.Forms.Padding(5, 2, 2, 0)
        Me.ToolStrip.Size = New System.Drawing.Size(1411, 80)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "tsMain"
        '
        'tsbLaunch
        '
        Me.tsbLaunch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLaunch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLaunch.Name = "tsbLaunch"
        Me.tsbLaunch.Size = New System.Drawing.Size(156, 69)
        Me.tsbLaunch.Text = " Launch "
        Me.tsbLaunch.ToolTipText = "Launches the app"
        '
        'tsbExplore
        '
        Me.tsbExplore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbExplore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExplore.Name = "tsbExplore"
        Me.tsbExplore.Size = New System.Drawing.Size(162, 69)
        Me.tsbExplore.Text = " Explore "
        Me.tsbExplore.ToolTipText = "Opens the apps folder in File Explorer"
        '
        'tsbWebsite
        '
        Me.tsbWebsite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebsite.Name = "tsbWebsite"
        Me.tsbWebsite.Size = New System.Drawing.Size(137, 69)
        Me.tsbWebsite.Text = "  Web  "
        Me.tsbWebsite.ToolTipText = "Opens the apps website"
        '
        'tsbDownload
        '
        Me.tsbDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbDownload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownload.Name = "tsbDownload"
        Me.tsbDownload.Size = New System.Drawing.Size(205, 69)
        Me.tsbDownload.Text = " Download "
        Me.tsbDownload.ToolTipText = "Opens the apps download web page"
        '
        'tsbVersion
        '
        Me.tsbVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbVersion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVersion.Name = "tsbVersion"
        Me.tsbVersion.Size = New System.Drawing.Size(181, 69)
        Me.tsbVersion.Text = "  Version  "
        Me.tsbVersion.ToolTipText = "Edits the apps version (F12)"
        '
        'ddbPath
        '
        Me.ddbPath.AutoToolTip = False
        Me.ddbPath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ddbPath.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miBrowsePath, Me.miClearCustomPath, Me.miSearchUsingEverything})
        Me.ddbPath.Image = CType(resources.GetObject("ddbPath.Image"), System.Drawing.Image)
        Me.ddbPath.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ddbPath.Name = "ddbPath"
        Me.ddbPath.Size = New System.Drawing.Size(160, 69)
        Me.ddbPath.Text = "  Path  "
        '
        'miBrowsePath
        '
        Me.miBrowsePath.Help = Nothing
        Me.miBrowsePath.Name = "miBrowsePath"
        Me.miBrowsePath.ShortcutKeyDisplayString = "F11"
        Me.miBrowsePath.Size = New System.Drawing.Size(738, 67)
        Me.miBrowsePath.Text = "Browse..."
        Me.miBrowsePath.ToolTipText = "Show Open File dialog to customize the path"
        '
        'miClearCustomPath
        '
        Me.miClearCustomPath.Help = Nothing
        Me.miClearCustomPath.Name = "miClearCustomPath"
        Me.miClearCustomPath.Size = New System.Drawing.Size(738, 67)
        Me.miClearCustomPath.Text = "Clear custom path..."
        '
        'miSearchUsingEverything
        '
        Me.miSearchUsingEverything.Help = Nothing
        Me.miSearchUsingEverything.Name = "miSearchUsingEverything"
        Me.miSearchUsingEverything.ShortcutKeyDisplayString = "Ctrl+F"
        Me.miSearchUsingEverything.Size = New System.Drawing.Size(738, 67)
        Me.miSearchUsingEverything.Text = "Search using Everything..."
        '
        'ddbTools
        '
        Me.ddbTools.AutoToolTip = False
        Me.ddbTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ddbTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miShowGridView, Me.miStatus})
        Me.ddbTools.Image = CType(resources.GetObject("ddbTools.Image"), System.Drawing.Image)
        Me.ddbTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ddbTools.Name = "ddbTools"
        Me.ddbTools.Size = New System.Drawing.Size(172, 69)
        Me.ddbTools.Text = "  Tools  "
        '
        'miShowGridView
        '
        Me.miShowGridView.Help = Nothing
        Me.miShowGridView.Name = "miShowGridView"
        Me.miShowGridView.Size = New System.Drawing.Size(737, 67)
        Me.miShowGridView.Text = "Show all tools in grid view"
        '
        'miStatus
        '
        Me.miStatus.Help = Nothing
        Me.miStatus.Name = "miStatus"
        Me.miStatus.Size = New System.Drawing.Size(737, 67)
        Me.miStatus.Text = "Check status of all required tools"
        '
        'tsbHelp
        '
        Me.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHelp.Name = "tsbHelp"
        Me.tsbHelp.Size = New System.Drawing.Size(139, 69)
        Me.tsbHelp.Text = "  Help  "
        Me.tsbHelp.ToolTipText = "Opens the apps help (F1)"
        '
        'flp
        '
        Me.flp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flp.Location = New System.Drawing.Point(473, 100)
        Me.flp.Margin = New System.Windows.Forms.Padding(0, 10, 10, 10)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(1411, 1018)
        Me.flp.TabIndex = 2
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchTextBox.Location = New System.Drawing.Point(11, 15)
        Me.SearchTextBox.Margin = New System.Windows.Forms.Padding(11, 10, 11, 0)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(451, 70)
        Me.SearchTextBox.TabIndex = 4
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
        Me.tlpMain.Controls.Add(Me.tv, 0, 1)
        Me.tlpMain.Controls.Add(Me.flp, 1, 1)
        Me.tlpMain.Controls.Add(Me.SearchTextBox, 0, 0)
        Me.tlpMain.Controls.Add(Me.ToolStrip, 1, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1894, 1128)
        Me.tlpMain.TabIndex = 6
        '
        'AppsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1894, 1128)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "AppsForm"
        Me.Text = "Apps"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private CurrentPackage As Package
    Private Nodes As New List(Of TreeNode)
    Private Headers As New Dictionary(Of String, Label)
    Private Contents As New Dictionary(Of String, Label)
    Private SetupButton As New ButtonEx
    Private DownloadButton As New ButtonEx

    Sub New()
        MyBase.New()
        InitializeComponent()
        RestoreClientSize(45, 32)
        tv.ItemHeight = CInt(FontHeight * 1.5)

        SearchTextBox_TextChanged()

        tv.Scrollable = True
        ToolStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        g.SetRenderer(ToolStrip)

        AddHandler SetupButton.Click, Sub()
                                          CurrentPackage.SetupAction.Invoke
                                          ShowActivePackage()
                                      End Sub

        SetupButton.ForeColor = Color.Red
        SetupButton.TextImageRelation = TextImageRelation.ImageBeforeText
        SetupButton.Image = StockIcon.GetSmallImage(StockIconIdentifier.Shield)
        SetupButton.Font = New Font("Segoe UI", 10)
        SetupButton.Margin = New Padding(FontHeight \ 3)
        SetupButton.Padding = New Padding(FontHeight \ 5)
        SetupButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
        SetupButton.AutoSize = True

        AddHandler DownloadButton.Click, Sub() g.ShellExecute(CurrentPackage.DownloadURL)
        DownloadButton.Font = New Font("Segoe UI", 10)
        DownloadButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
        DownloadButton.AutoSize = True

        Dim titleHeaderLabel = New Label With {
            .Font = New Font(flp.Font.FontFamily, 14 * s.UIScaleFactor, FontStyle.Bold),
            .AutoSize = True
        }

        Headers("Title") = titleHeaderLabel
        flp.Controls.Add(titleHeaderLabel)
        AddSection("Status")
        flp.Controls.Add(SetupButton)
        flp.Controls.Add(DownloadButton)
        AddSection("Location")
        AddSection("Version")
        AddSection("AviSynth Filters")
        AddSection("VapourSynth Filters")
        AddSection("Filters")
        AddSection("Description")
    End Sub

    Sub ShowActivePackage()
        Dim path = CurrentPackage.Path

        Headers("Title").Text = CurrentPackage.Name

        SetupButton.Text = "Install " + CurrentPackage.Name
        SetupButton.Visible = Not CurrentPackage.SetupAction Is Nothing AndAlso CurrentPackage.GetStatus <> ""

        DownloadButton.Text = "Download and install " + CurrentPackage.Name
        DownloadButton.Visible = CurrentPackage.DownloadURL <> "" AndAlso CurrentPackage.GetStatus <> ""

        tsbExplore.Enabled = path <> ""
        tsbLaunch.Enabled = Not CurrentPackage.LaunchAction Is Nothing AndAlso CurrentPackage.GetStatus = ""
        tsbWebsite.Enabled = CurrentPackage.URL <> ""
        tsbDownload.Enabled = CurrentPackage.DownloadURL <> ""
        tsbHelp.Enabled = CurrentPackage.HelpFileOrURL <> ""

        tsbVersion.Enabled = CurrentPackage.Path.FileExists AndAlso
            Not (CurrentPackage.IsVersionOld() AndAlso Not CurrentPackage.VersionAllowOld)

        miBrowsePath.Enabled = CurrentPackage.IsCustomPathAllowed
        miSearchUsingEverything.Enabled = miBrowsePath.Enabled
        miClearCustomPath.Enabled = miBrowsePath.Enabled

        s.StringDictionary("RecentExternalApplicationControl") = CurrentPackage.Name + CurrentPackage.Version

        flp.SuspendLayout()

        Contents("Location").Text = If(path = "", "Not found", path)
        Contents("Description").Text = CurrentPackage.Description

        If File.Exists(CurrentPackage.Path) Then
            Contents("Version").Text = If(CurrentPackage.IsVersionCorrect, CurrentPackage.Version, "Unknown")
            Contents("Version").Text += " (" + File.GetLastWriteTimeUtc(CurrentPackage.Path).ToShortDateString() + ")"
        Else
            Contents("Version").Text = "-"
        End If

        Contents("Status").Text = CurrentPackage.GetStatusDisplay()

        If CurrentPackage.GetStatus <> "" AndAlso CurrentPackage.Required Then
            Contents("Status").ForeColor = Color.Red
        Else
            Contents("Status").ForeColor = Color.Black
        End If

        Contents("Status").Font = New Font("Segoe UI", 10 * s.UIScaleFactor)

        Headers("AviSynth Filters").Visible = False
        Contents("AviSynth Filters").Visible = False

        Headers("VapourSynth Filters").Visible = False
        Contents("VapourSynth Filters").Visible = False

        Headers("Filters").Visible = False
        Contents("Filters").Visible = False

        If TypeOf CurrentPackage Is PluginPackage Then
            Dim plugin = DirectCast(CurrentPackage, PluginPackage)

            If Not plugin.AvsFilterNames Is Nothing AndAlso
                Not plugin.VSFilterNames Is Nothing Then

                Headers("AviSynth Filters").Visible = True
                Contents("AviSynth Filters").Text = plugin.AvsFilterNames.Join(", ")
                Contents("AviSynth Filters").Visible = True

                Headers("VapourSynth Filters").Visible = True
                Contents("VapourSynth Filters").Text = plugin.VSFilterNames.Join(", ")
                Contents("VapourSynth Filters").Visible = True
            ElseIf Not plugin.AvsFilterNames Is Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.AvsFilterNames.Join(", ")
                Contents("Filters").Visible = True
            ElseIf Not plugin.VSFilterNames Is Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.VSFilterNames.Join(", ")
                Contents("Filters").Visible = True
            End If
        End If

        flp.ResumeLayout()
    End Sub

    Sub AddSection(title As String)
        Dim controlMargin = CInt(FontHeight / 10)

        Dim headerLabel = New Label With {
            .Text = title,
            .Font = New Font(flp.Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold),
            .AutoSize = True,
            .Margin = New Padding(controlMargin, controlMargin, 0, 0)}

        Dim contentLabel = New Label With {
            .AutoSize = True,
            .Margin = New Padding(controlMargin, CInt(controlMargin / 3), 0, 0)}

        Headers(title) = headerLabel
        Contents(title) = contentLabel

        flp.Controls.Add(headerLabel)
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

    Protected Overrides Sub OnActivated(e As EventArgs)
        ShowActivePackage()
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnKeyDown(args As KeyEventArgs)
        MyBase.OnKeyDown(args)

        Select Case args.KeyData
            Case Keys.F1
                tsbHelp.PerformClick()
            Case Keys.F10
                Dim fp = Folder.Startup + "changelog.md"

                If File.Exists(fp) Then
                    g.ShellExecute(fp)
                End If
            Case Keys.F11
                miBrowsePath.PerformClick()
            Case Keys.F12
                tsbVersion.PerformClick()
            Case Keys.Control Or Keys.F
                miSearchUsingEverything.PerformClick()
        End Select
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

            Dim searchString = pack.Name + pack.Description + pack.Version + pack.WebURL +
                plugin?.VSFilterNames.Join(" ") + pack.Path + plugin?.AvsFilterNames.Join(" ")

            If searchString?.ToLower.Contains(SearchTextBox.Text?.ToLower) Then
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
                    If plugin.AvsFilterNames?.Length > 0 Then
                        Dim n = tv.AddNode("AviSynth|" + pack.Name)
                        Nodes.Add(n)
                        n.Tag = pack
                    End If

                    If plugin.VSFilterNames?.Length > 0 Then
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
        g.ShellExecute(CurrentPackage.DownloadURL)
    End Sub

    Sub tsbVersion_Click(sender As Object, e As EventArgs) Handles tsbVersion.Click
        If Not File.Exists(CurrentPackage.Path) Then
            Exit Sub
        End If

        Dim msg = "What's the name of this version?"
        Dim version = FileVersionInfo.GetVersionInfo(CurrentPackage.Path)

        Dim fileVersionString = version.FileMajorPart & "." & version.FileMinorPart & "." &
                                version.FileBuildPart & "." & version.FilePrivatePart

        Dim productVersionString = version.ProductMajorPart & "." & version.ProductMinorPart & "." &
                                   version.ProductBuildPart & "." & version.ProductPrivatePart

        If fileVersionString <> "0.0.0.0" Then
            msg += BR2 + "File Version: " + fileVersionString + " (often not correct!)"
        End If

        If productVersionString <> "0.0.0.0" Then
            msg += BR2 + "Product Version: " + productVersionString + " (often not correct!)"
        End If

        Dim input = InputBox.Show(msg, "StaxRip", CurrentPackage.Version)

        If input <> "" Then
            input = input.Replace(";", "_")

            CurrentPackage.Version = input
            CurrentPackage.VersionDate = File.GetLastWriteTimeUtc(CurrentPackage.Path)

            Dim txt As String

            For Each pack In Package.Items.Values
                If pack.Version <> "" Then
                    txt += pack.ID + " = " + pack.VersionDate.ToString("yyyy-MM-dd",
                        CultureInfo.InvariantCulture) + "; " + pack.Version + BR 'persian calendar
                End If
            Next

            If Directory.Exists(Folder.Apps) Then
                txt.FormatColumn("=").WriteFileUTF8BOM(Folder.Apps + "Versions.txt")
            End If

            ShowActivePackage()
        End If
    End Sub

    Sub miShowGridView_Click(sender As Object, e As EventArgs) Handles miShowGridView.Click
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
            td.MainInstruction = "Choose how to show"
            td.AddCommand("Show as CSV file", "csv")
            td.AddCommand("Show using PowerShell", "ogv")

            Select Case td.Show
                Case "csv"
                    Dim csvFile = Folder.Temp + "staxrip tools.csv"
                    g.ConvertToCSV(";", rows).WriteFileUTF8(csvFile)
                    g.ShellExecute(g.GetAppPathForExtension("csv", "txt"), csvFile.Escape)
                Case "ogv"
                    g.InvokePowerShellCode($"$objects | Out-GridView", "objects", rows)
            End Select
        End Using
    End Sub

    Sub miStatus_Click(sender As Object, e As EventArgs) Handles miStatus.Click
        Dim txt As String

        For Each pair In Package.Items
            Dim pack = pair.Value

            If pack.Required AndAlso pack.GetStatus <> "" Then
                txt += pack.Name + ": " + pack.GetStatus + BR2
            End If
        Next

        If txt = "" Then
            MsgInfo("OK!")
        Else
            MsgInfo(txt)
        End If
    End Sub

    Sub miBrowsePath_Click(sender As Object, e As EventArgs) Handles miBrowsePath.Click
        Using dialog As New OpenFileDialog
            dialog.SetInitDir(s.Storage.GetString(CurrentPackage.Name + "custom path"))
            dialog.Filter = "|" + CurrentPackage.Filename + "|All Files|*.*"

            If dialog.ShowDialog = DialogResult.OK Then
                If Not s.AllowCustomPathsInStartupFolder AndAlso
                    dialog.FileName.ToLowerEx.StartsWithEx(Folder.Startup.ToLowerEx) AndAlso
                    Not dialog.FileName.ToLowerEx.StartsWithEx(Folder.Settings.ToLowerEx) Then

                    MsgError("Custom paths within the startup folder are not permitted.")
                    Exit Sub
                End If

                s.Storage.SetString(CurrentPackage.Name + "custom path", dialog.FileName)
                ShowActivePackage()
            End If
        End Using
    End Sub

    Sub miClearCustomPath_Click(sender As Object, e As EventArgs) Handles miClearCustomPath.Click
        Dim packs = Package.Items.Values.Where(Function(pack) pack.GetStoredPath() <> "")

        If packs.Count > 0 Then
            Using td As New TaskDialog(Of Package)
                td.MainInstruction = "Choose a path to be cleared."

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

    Sub miSearchUsingEverything_Click(sender As Object, e As EventArgs) Handles miSearchUsingEverything.Click
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

                              If path.FileName.ToLower = CurrentPackage.Filename.ToLower Then
                                  paths.Add(path)
                              End If

                              If paths.Count > 5 Then
                                  Exit For
                              End If
                          Next
                      Catch
                          g.ShellExecute("https://www.voidtools.com")
                      End Try
                  End Sub

        Await Task.Run(run)

        If Disposing OrElse IsDisposed Then
            Exit Sub
        End If

        If paths.Count > 0 Then
            Using form As New SimpleSettingsForm("Choose Path")
                form.ScaleClientSize(35, 20)
                form.bnOK.Visible = False
                form.bnCancel.Visible = False
                form.LineControl.Visible = False
                form.HelpButton = False
                form.Height = CInt(form.FontHeight * 3.6 * paths.Count + form.FontHeight * 2.5)

                Dim ui = form.SimpleUI

                For Each path In paths
                    Dim bn = ui.AddButton
                    bn.Button.Text = path
                    bn.Button.Expand = True
                    bn.Button.TextAlign = ContentAlignment.MiddleLeft
                    bn.Button.Height = CInt(form.FontHeight * 3)
                    bn.Button.Padding = New Padding(form.FontHeight \ 3)
                    AddHandler bn.Button.Click, Sub(sender As Object, e As EventArgs)
                                                    Dim fp = DirectCast(sender, Button).Text

                                                    If Not s.AllowCustomPathsInStartupFolder AndAlso
                                                        fp.ToLowerEx.StartsWithEx(Folder.Startup.ToLowerEx) AndAlso
                                                        Not fp.ToLowerEx.StartsWithEx(Folder.Settings.ToLowerEx) Then

                                                        MsgError("Custom paths within the startup folder are not permitted.")
                                                        Exit Sub
                                                    End If

                                                    CurrentPackage.SetStoredPath(fp)
                                                    ShowActivePackage()
                                                    form.Close()
                                                End Sub
                Next

                form.ShowDialog()
            End Using
        Else
            MsgInfo("Nothing found.")
        End If
    End Sub

    Const EVERYTHING_REQUEST_FILE_NAME As Integer = 1
    Const EVERYTHING_REQUEST_PATH As Integer = 2

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Public Shared Function Everything_SetSearch(lpSearchString As String) As Integer
    End Function

    <DllImport("Everything.dll")>
    Shared Sub Everything_SetRequestFlags(dwRequestFlags As UInt32)
    End Sub

    <DllImport("Everything.dll")>
    Shared Sub Everything_SetSort(dwSortType As UInt32)
    End Sub

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Public Shared Function Everything_Query(bWait As Boolean) As Boolean
    End Function

    <DllImport("Everything.dll", CharSet:=CharSet.Unicode)>
    Shared Sub Everything_GetResultFullPathName(nIndex As UInt32, lpString As StringBuilder, nMaxCount As UInt32)
    End Sub

    <DllImport("Everything.dll")>
    Public Shared Function Everything_GetResultSize(nIndex As UInt32, ByRef lpFileSize As Long) As Boolean
    End Function

    <DllImport("Everything.dll")>
    Public Shared Function Everything_GetNumResults() As UInt32
    End Function
End Class
