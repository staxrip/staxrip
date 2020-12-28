
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks

Imports StaxRip.UI

Public Class AppsForm
    Inherits DialogBase
    Implements IUpdateUI

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
    Friend WithEvents miEditPath As MenuItemEx
    Friend WithEvents miClearPaths As MenuItemEx
    Friend WithEvents miFindPath As MenuItemEx
    Friend WithEvents miShowGrid As MenuItemEx
    Friend WithEvents miStatus As MenuItemEx
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents miAutoUpdate As MenuItemEx
    Friend WithEvents miEditVersion As MenuItemEx
    Friend WithEvents miEditChangelog As MenuItemEx
    Friend WithEvents miDownload As MenuItemEx
    Friend WithEvents miWebsite As MenuItemEx
    Friend WithEvents miExplore As MenuItemEx
    Friend WithEvents miLaunch As MenuItemEx
    Friend WithEvents miHelp As MenuItemEx
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
        Me.ddbTools = New System.Windows.Forms.ToolStripDropDownButton()
        Me.miEditPath = New StaxRip.UI.MenuItemEx()
        Me.miClearPaths = New StaxRip.UI.MenuItemEx()
        Me.miFindPath = New StaxRip.UI.MenuItemEx()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.miEditVersion = New StaxRip.UI.MenuItemEx()
        Me.miEditChangelog = New StaxRip.UI.MenuItemEx()
        Me.miShowGrid = New StaxRip.UI.MenuItemEx()
        Me.miStatus = New StaxRip.UI.MenuItemEx()
        Me.miAutoUpdate = New StaxRip.UI.MenuItemEx()
        Me.miDownload = New StaxRip.UI.MenuItemEx()
        Me.miWebsite = New StaxRip.UI.MenuItemEx()
        Me.miExplore = New StaxRip.UI.MenuItemEx()
        Me.miLaunch = New StaxRip.UI.MenuItemEx()
        Me.miHelp = New StaxRip.UI.MenuItemEx()
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
        Me.tv.Location = New System.Drawing.Point(6, 59)
        Me.tv.Margin = New System.Windows.Forms.Padding(6)
        Me.tv.Name = "tv"
        Me.tv.Scrollable = False
        Me.tv.SelectOnMouseDown = True
        Me.tv.ShowLines = False
        Me.tv.ShowPlusMinus = False
        Me.tv.Size = New System.Drawing.Size(264, 544)
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
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbLaunch, Me.tsbExplore, Me.tsbWebsite, Me.tsbDownload, Me.tsbVersion, Me.ddbTools, Me.tsbHelp})
        Me.ToolStrip.Location = New System.Drawing.Point(276, 6)
        Me.ToolStrip.Margin = New System.Windows.Forms.Padding(0, 6, 6, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New System.Windows.Forms.Padding(3, 1, 1, 0)
        Me.ToolStrip.Size = New System.Drawing.Size(823, 47)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "tsMain"
        '
        'tsbLaunch
        '
        Me.tsbLaunch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLaunch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLaunch.Name = "tsbLaunch"
        Me.tsbLaunch.Size = New System.Drawing.Size(96, 40)
        Me.tsbLaunch.Text = " Launch "
        Me.tsbLaunch.ToolTipText = "Launches the app (Ctrl+L)"
        '
        'tsbExplore
        '
        Me.tsbExplore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbExplore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExplore.Name = "tsbExplore"
        Me.tsbExplore.Size = New System.Drawing.Size(97, 40)
        Me.tsbExplore.Text = " Explore "
        Me.tsbExplore.ToolTipText = "Opens the apps folder in File Explorer (Ctrl+E)"
        '
        'tsbWebsite
        '
        Me.tsbWebsite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebsite.Name = "tsbWebsite"
        Me.tsbWebsite.Size = New System.Drawing.Size(83, 40)
        Me.tsbWebsite.Text = "  Web  "
        Me.tsbWebsite.ToolTipText = "Opens the apps website (Ctrl+W)"
        '
        'tsbDownload
        '
        Me.tsbDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbDownload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownload.Name = "tsbDownload"
        Me.tsbDownload.Size = New System.Drawing.Size(123, 40)
        Me.tsbDownload.Text = " Download "
        Me.tsbDownload.ToolTipText = "Opens the apps download web page (Ctrl+D)"
        '
        'tsbVersion
        '
        Me.tsbVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbVersion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVersion.Name = "tsbVersion"
        Me.tsbVersion.Size = New System.Drawing.Size(109, 40)
        Me.tsbVersion.Text = "  Version  "
        Me.tsbVersion.ToolTipText = "Edits the apps version (F12)"
        '
        'ddbTools
        '
        Me.ddbTools.AutoToolTip = False
        Me.ddbTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ddbTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miEditPath, Me.miClearPaths, Me.miFindPath, Me.ToolStripMenuItem1, Me.miEditVersion, Me.miEditChangelog, Me.miShowGrid, Me.miStatus, Me.miAutoUpdate, Me.miDownload, Me.miWebsite, Me.miExplore, Me.miLaunch, Me.miHelp})
        Me.ddbTools.Image = CType(resources.GetObject("ddbTools.Image"), System.Drawing.Image)
        Me.ddbTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ddbTools.Name = "ddbTools"
        Me.ddbTools.Size = New System.Drawing.Size(105, 40)
        Me.ddbTools.Text = "  Tools  "
        '
        'miEditPath
        '
        Me.miEditPath.Help = Nothing
        Me.miEditPath.Name = "miEditPath"
        Me.miEditPath.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.miEditPath.Size = New System.Drawing.Size(322, 39)
        Me.miEditPath.Text = "Edit Path..."
        Me.miEditPath.ToolTipText = "Show Open File dialog to customize the path"
        '
        'miClearPaths
        '
        Me.miClearPaths.Help = Nothing
        Me.miClearPaths.Name = "miClearPaths"
        Me.miClearPaths.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.miClearPaths.Size = New System.Drawing.Size(322, 39)
        Me.miClearPaths.Text = "Clear Paths..."
        Me.miClearPaths.ToolTipText = "Clear custom paths"
        '
        'miFindPath
        '
        Me.miFindPath.Help = Nothing
        Me.miFindPath.Name = "miFindPath"
        Me.miFindPath.ShortcutKeyDisplayString = ""
        Me.miFindPath.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.miFindPath.Size = New System.Drawing.Size(322, 39)
        Me.miFindPath.Text = "Find Path..."
        Me.miFindPath.ToolTipText = "Find path using voidtools Everything"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(319, 6)
        '
        'miEditVersion
        '
        Me.miEditVersion.Help = Nothing
        Me.miEditVersion.Name = "miEditVersion"
        Me.miEditVersion.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.miEditVersion.Size = New System.Drawing.Size(322, 39)
        Me.miEditVersion.Text = "Edit Version"
        Me.miEditVersion.ToolTipText = "Edits the apps version"
        '
        'miEditChangelog
        '
        Me.miEditChangelog.Help = Nothing
        Me.miEditChangelog.Name = "miEditChangelog"
        Me.miEditChangelog.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.miEditChangelog.Size = New System.Drawing.Size(322, 39)
        Me.miEditChangelog.Text = "Edit Changelog"
        '
        'miShowGrid
        '
        Me.miShowGrid.Help = Nothing
        Me.miShowGrid.Name = "miShowGrid"
        Me.miShowGrid.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.miShowGrid.Size = New System.Drawing.Size(322, 39)
        Me.miShowGrid.Text = "Show Grid"
        Me.miShowGrid.ToolTipText = "Show tools in grid view"
        '
        'miStatus
        '
        Me.miStatus.Help = Nothing
        Me.miStatus.Name = "miStatus"
        Me.miStatus.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.miStatus.Size = New System.Drawing.Size(322, 39)
        Me.miStatus.Text = "Check Status"
        Me.miStatus.ToolTipText = "Check status of all required tools"
        '
        'miAutoUpdate
        '
        Me.miAutoUpdate.Help = Nothing
        Me.miAutoUpdate.Name = "miAutoUpdate"
        Me.miAutoUpdate.ShortcutKeyDisplayString = ""
        Me.miAutoUpdate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.U), System.Windows.Forms.Keys)
        Me.miAutoUpdate.Size = New System.Drawing.Size(322, 39)
        Me.miAutoUpdate.Text = "Auto Update"
        Me.miAutoUpdate.ToolTipText = "Full automatic update"
        '
        'miDownload
        '
        Me.miDownload.Help = Nothing
        Me.miDownload.Name = "miDownload"
        Me.miDownload.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.miDownload.Size = New System.Drawing.Size(322, 39)
        Me.miDownload.Text = "Download"
        Me.miDownload.ToolTipText = "Opens the apps download web page"
        '
        'miWebsite
        '
        Me.miWebsite.Help = Nothing
        Me.miWebsite.Name = "miWebsite"
        Me.miWebsite.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.miWebsite.Size = New System.Drawing.Size(322, 39)
        Me.miWebsite.Text = "Website"
        Me.miWebsite.ToolTipText = "Opens the apps website"
        '
        'miExplore
        '
        Me.miExplore.Help = Nothing
        Me.miExplore.Name = "miExplore"
        Me.miExplore.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.miExplore.Size = New System.Drawing.Size(322, 39)
        Me.miExplore.Text = "Explore"
        Me.miExplore.ToolTipText = "Opens the apps folder in File Explorer"
        '
        'miLaunch
        '
        Me.miLaunch.Help = Nothing
        Me.miLaunch.Name = "miLaunch"
        Me.miLaunch.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.miLaunch.Size = New System.Drawing.Size(322, 39)
        Me.miLaunch.Text = "Launch"
        Me.miLaunch.ToolTipText = "Launches the app"
        '
        'miHelp
        '
        Me.miHelp.Help = Nothing
        Me.miHelp.Name = "miHelp"
        Me.miHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.miHelp.Size = New System.Drawing.Size(322, 39)
        Me.miHelp.Text = "Help"
        Me.miHelp.ToolTipText = "Opens the apps help"
        '
        'tsbHelp
        '
        Me.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHelp.Name = "tsbHelp"
        Me.tsbHelp.Size = New System.Drawing.Size(84, 40)
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
        Me.flp.Location = New System.Drawing.Point(276, 59)
        Me.flp.Margin = New System.Windows.Forms.Padding(0, 6, 6, 6)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(823, 544)
        Me.flp.TabIndex = 2
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchTextBox.Location = New System.Drawing.Point(6, 9)
        Me.SearchTextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 0)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(264, 41)
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
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(2)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1105, 609)
        Me.tlpMain.TabIndex = 6
        '
        'AppsForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(168.0!, 168.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1105, 609)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6)
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
    Private ToolUpdate As ToolUpdate

    Sub New()
        MyBase.New()
        InitializeComponent()
        RestoreClientSize(45, 32)
        tv.ItemHeight = CInt(FontHeight * 1.5)
        tv.Scrollable = True

        SearchTextBox_TextChanged()

        ToolStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        g.SetRenderer(ToolStrip)

        miEditChangelog.Visible = g.IsDevelopmentPC

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

        Dim titleHeaderLabel = New Label With {
            .Font = New Font(flp.Font.FontFamily, 14 * s.UIScaleFactor, FontStyle.Bold),
            .AutoSize = True
        }

        Headers("Title") = titleHeaderLabel
        flp.Controls.Add(titleHeaderLabel)
        AddSection("Status")
        flp.Controls.Add(SetupButton)
        AddSection("Location")
        AddSection("Version")
        AddSection("AviSynth Filters")
        AddSection("VapourSynth Filters")
        AddSection("Filters")
        AddSection("Description")
    End Sub

    Sub ShowActivePackage()
        If Disposing OrElse IsDisposed Then
            Exit Sub
        End If

        Dim path = CurrentPackage.Path

        Headers("Title").Text = CurrentPackage.Name

        SetupButton.Text = "Install " + CurrentPackage.Name
        SetupButton.Visible = Not CurrentPackage.SetupAction Is Nothing AndAlso CurrentPackage.GetStatus <> ""

        tsbExplore.Enabled = path <> ""
        tsbLaunch.Enabled = Not CurrentPackage.LaunchAction Is Nothing AndAlso CurrentPackage.Path <> ""
        miLaunch.Enabled = tsbLaunch.Enabled
        tsbDownload.Enabled = CurrentPackage.DownloadURL <> "" OrElse Not CurrentPackage.DownloadURLs Is Nothing
        miAutoUpdate.Enabled = tsbDownload.Enabled AndAlso CurrentPackage.SupportsAutoUpdate
        tsbWebsite.Enabled = CurrentPackage.URL <> ""

        tsbVersion.Enabled = CurrentPackage.Path.FileExists AndAlso
            Not (CurrentPackage.IsVersionOld() AndAlso Not CurrentPackage.VersionAllowOld)

        miEditPath.Enabled = CurrentPackage.IsCustomPathAllowed
        miFindPath.Enabled = miEditPath.Enabled
        miClearPaths.Enabled = miEditPath.Enabled

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
                                ToolUpdate.ExtractDir = Folder.Temp + Guid.NewGuid.ToString + "\"
                                Directory.CreateDirectory(ToolUpdate.ExtractDir)

                                For Each i In files
                                    FileHelp.Copy(i, ToolUpdate.ExtractDir + i.FileName)
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
        If CurrentPackage.DownloadURL <> "" Then
            g.ShellExecute(CurrentPackage.DownloadURL)
        ElseIf Not CurrentPackage.DownloadURLs Is Nothing Then
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "Choose a download option."

                For Each pair In CurrentPackage.DownloadURLs
                    td.AddCommand(pair.Name, pair.Value)
                Next

                If td.Show <> "" Then
                    g.ShellExecute(td.SelectedValue)
                End If
            End Using
        End If
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
            CurrentPackage.SetVersion(input.Replace(";", "_").Trim)
            ShowActivePackage()
            Application.DoEvents()
            g.DefaultCommands.TestAndDynamicFileCreation()
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
            MsgInfo("OK!", "All tools have OK status!")
        Else
            MsgInfo(txt)
        End If
    End Sub

    Sub miBrowsePath_Click(sender As Object, e As EventArgs) Handles miEditPath.Click
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

    Sub miClearCustomPath_Click(sender As Object, e As EventArgs) Handles miClearPaths.Click
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

    Sub miSearchUsingEverything_Click(sender As Object, e As EventArgs) Handles miFindPath.Click
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
        Dim url = CurrentPackage.GetDownloadURL

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
        Dim path = Folder.Startup + "..\Changelog.md"

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
End Class
