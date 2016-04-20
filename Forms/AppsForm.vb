Imports System.Globalization
Imports StaxRip.UI

Class AppsForm
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
    Friend WithEvents bnClose As StaxRip.UI.ButtonEx
    Friend WithEvents flp As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SearchTextBox As StaxRip.SearchTextBox
    Friend WithEvents tsbWebsite As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbOpenDir As System.Windows.Forms.ToolStripButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.tsbLaunch = New System.Windows.Forms.ToolStripButton()
        Me.tsbOpenDir = New System.Windows.Forms.ToolStripButton()
        Me.tsbWebsite = New System.Windows.Forms.ToolStripButton()
        Me.tsbHelp = New System.Windows.Forms.ToolStripButton()
        Me.bnClose = New StaxRip.UI.ButtonEx()
        Me.flp = New System.Windows.Forms.FlowLayoutPanel()
        Me.SearchTextBox = New StaxRip.SearchTextBox()
        Me.ToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'tv
        '
        Me.tv.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tv.AutoCollaps = True
        Me.tv.ExpandMode = StaxRip.UI.TreeNodeExpandMode.InclusiveChilds
        Me.tv.FullRowSelect = True
        Me.tv.HideSelection = False
        Me.tv.Location = New System.Drawing.Point(12, 60)
        Me.tv.Name = "tv"
        Me.tv.Scrollable = False
        Me.tv.SelectOnMouseDown = True
        Me.tv.ShowLines = False
        Me.tv.ShowPlusMinus = False
        Me.tv.Size = New System.Drawing.Size(232, 542)
        Me.tv.Sorted = True
        Me.tv.TabIndex = 0
        '
        'ToolStrip
        '
        Me.ToolStrip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip.AutoSize = False
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbLaunch, Me.tsbOpenDir, Me.tsbWebsite, Me.tsbHelp})
        Me.ToolStrip.Location = New System.Drawing.Point(250, 10)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New System.Windows.Forms.Padding(3, 1, 1, 0)
        Me.ToolStrip.Size = New System.Drawing.Size(740, 40)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'tsbLaunch
        '
        Me.tsbLaunch.AutoToolTip = False
        Me.tsbLaunch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLaunch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLaunch.Name = "tsbLaunch"
        Me.tsbLaunch.Size = New System.Drawing.Size(76, 36)
        Me.tsbLaunch.Text = "Launch "
        Me.tsbLaunch.ToolTipText = "Launches the application"
        '
        'tsbOpenDir
        '
        Me.tsbOpenDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbOpenDir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOpenDir.Name = "tsbOpenDir"
        Me.tsbOpenDir.Size = New System.Drawing.Size(93, 36)
        Me.tsbOpenDir.Text = "Directory "
        Me.tsbOpenDir.ToolTipText = "Opens the directory containing the application"
        '
        'tsbWebsite
        '
        Me.tsbWebsite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebsite.Name = "tsbWebsite"
        Me.tsbWebsite.Size = New System.Drawing.Size(84, 36)
        Me.tsbWebsite.Text = "Website "
        Me.tsbWebsite.ToolTipText = "Opens the application's website"
        '
        'tsbHelp
        '
        Me.tsbHelp.AutoToolTip = False
        Me.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHelp.Name = "tsbHelp"
        Me.tsbHelp.Size = New System.Drawing.Size(58, 36)
        Me.tsbHelp.Text = "Help "
        Me.tsbHelp.ToolTipText = "Opens the application's help"
        '
        'bnClose
        '
        Me.bnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnClose.Location = New System.Drawing.Point(890, 608)
        Me.bnClose.Size = New System.Drawing.Size(100, 34)
        Me.bnClose.Text = "Close"
        '
        'flp
        '
        Me.flp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flp.BackColor = System.Drawing.Color.White
        Me.flp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flp.Location = New System.Drawing.Point(250, 60)
        Me.flp.Margin = New System.Windows.Forms.Padding(3, 10, 3, 3)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(740, 542)
        Me.flp.TabIndex = 2
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchTextBox.Location = New System.Drawing.Point(12, 16)
        Me.SearchTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(232, 31)
        Me.SearchTextBox.TabIndex = 4
        '
        'ApplicationsForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnClose
        Me.ClientSize = New System.Drawing.Size(1002, 654)
        Me.Controls.Add(Me.SearchTextBox)
        Me.Controls.Add(Me.flp)
        Me.Controls.Add(Me.bnClose)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.tv)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "ApplicationsForm"
        Me.Text = "Apps"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private ActivePackage As Package
    Private Directories As New Hashtable
    Private Nodes As New List(Of TreeNode)
    Private ControlDic As New Dictionary(Of String, Control)
    Private Headers As New Dictionary(Of String, Label)
    Private Contents As New Dictionary(Of String, Label)
    Private SetupButton As New ButtonEx
    Private DownloadButton As New ButtonEx

    Sub New()
        MyBase.New()
        InitializeComponent()

        SearchTextBox_TextChanged()

        tv.Scrollable = True
        tv.ItemHeight = CInt(FontHeight * 1.5)

        AddHandler SetupButton.Click, Sub() RunSetup()
        SetupButton.ForeColor = Color.Red
        SetupButton.AutoSize = True
        SetupButton.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        SetupButton.TextImageRelation = TextImageRelation.ImageBeforeText
        SetupButton.Image = StockIcon.GetSmallImage(StockIconIdentifier.Shield)

        AddHandler DownloadButton.Click, Sub() g.ShellExecute(ActivePackage.DownloadURL)
        DownloadButton.AutoSize = True
        DownloadButton.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink

        Dim title = New Label With {.Font = New Font(flp.Font.FontFamily, 14, FontStyle.Bold),
                                    .AutoSize = True,
                                    .Margin = New Padding(6, 6, 0, 0)}
        Headers("Title") = title
        flp.Controls.Add(title)
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
        Dim path = ActivePackage.GetPath

        Headers("Title").Text = ActivePackage.Name

        SetupButton.Text = "Install " + ActivePackage.Name
        SetupButton.Visible = Not ActivePackage.SetupAction Is Nothing AndAlso (ActivePackage.IsStatusCritical OrElse (Not ActivePackage.IsCorrectVersion AndAlso ActivePackage.Version <> ""))

        DownloadButton.Text = "Download " + ActivePackage.Name
        DownloadButton.Visible = ActivePackage.DownloadURL <> "" AndAlso (ActivePackage.IsStatusCritical OrElse (Not ActivePackage.IsCorrectVersion AndAlso ActivePackage.Version <> ""))

        tsbOpenDir.Enabled = path <> ""
        tsbLaunch.Enabled = Not ActivePackage.LaunchAction Is Nothing AndAlso Not ActivePackage.IsStatusCritical
        tsbWebsite.Enabled = ActivePackage.WebURL <> ""
        tsbHelp.Enabled = ActivePackage.GetHelpPath <> ""

        s.StringDictionary("RecentExternalApplicationControl") = ActivePackage.Name

        flp.SuspendLayout()

        Contents("Status").Text = ActivePackage.GetStatusDisplay()
        Contents("Location").Text = path
        Contents("Version").Text = ActivePackage.Version
        Contents("Description").Text = ActivePackage.Description

        Contents("Location").Visible = path <> ""
        Headers("Location").Visible = path <> ""

        Contents("Version").Visible = ActivePackage.Version <> ""
        Headers("Version").Visible = ActivePackage.Version <> ""

        Headers("AviSynth Filters").Visible = False
        Contents("AviSynth Filters").Visible = False

        Headers("VapourSynth Filters").Visible = False
        Contents("VapourSynth Filters").Visible = False

        Headers("Filters").Visible = False
        Contents("Filters").Visible = False

        If TypeOf ActivePackage Is PluginPackage Then
            Dim plugin = DirectCast(ActivePackage, PluginPackage)

            If Not plugin.AviSynthFilterNames Is Nothing AndAlso
                Not plugin.VapourSynthFilterNames Is Nothing Then

                Headers("AviSynth Filters").Visible = True
                Contents("AviSynth Filters").Text = plugin.AviSynthFilterNames.Join(", ")
                Contents("AviSynth Filters").Visible = True

                Headers("VapourSynth Filters").Visible = True
                Contents("VapourSynth Filters").Text = plugin.VapourSynthFilterNames.Join(", ")
                Contents("VapourSynth Filters").Visible = True
            ElseIf Not plugin.AviSynthFilterNames Is Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.AviSynthFilterNames.Join(", ")
                Contents("Filters").Visible = True
            ElseIf Not plugin.VapourSynthFilterNames Is Nothing Then
                Headers("Filters").Visible = True
                Contents("Filters").Text = plugin.VapourSynthFilterNames.Join(", ")
                Contents("Filters").Visible = True
            End If
        End If

        flp.ResumeLayout()
    End Sub

    Sub AddSection(title As String)
        Dim headerLabel = New Label With {.Text = title,
                                          .Font = New Font(flp.Font.FontFamily, 9, FontStyle.Bold),
                                          .AutoSize = True,
                                          .Margin = New Padding(8, 10, 0, 1)}

        Dim contentLabel = New Label With {.AutoSize = True,
                                           .Margin = New Padding(8, 2, 0, 0)}

        Headers(title) = headerLabel
        Contents(title) = contentLabel

        flp.Controls.Add(headerLabel)
        flp.Controls.Add(contentLabel)
    End Sub

    Sub RunSetup()
        ActivePackage.SetupAction.Invoke()
        ShowActivePackage()
    End Sub

    Sub ShowPackage(package As Package)
        For Each i As TreeNode In tv.Nodes
            If i.IsExpanded Then
                i.Collapse()
            End If
        Next

        For Each i In tv.GetNodes
            If package Is i.Tag Then
                tv.SelectedNode = i
            End If
        Next
    End Sub

    Private Sub ShowPackage(tn As TreeNode)
        If Not tn Is Nothing AndAlso Not tn.Tag Is Nothing Then
            Dim newPackage = DirectCast(tn.Tag, Package)

            If Not newPackage Is ActivePackage Then
                ActivePackage = newPackage
                ShowActivePackage()
            End If
        End If
    End Sub

    Private Sub PathsForm_Activated() Handles MyBase.Activated
        ShowActivePackage()
    End Sub

    Private Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
        If e.Node.Tag Is Nothing AndAlso e.Node.Nodes.Count > 0 Then
            tv.SelectedNode = e.Node.Nodes(0)
        End If

        If Not e.Node.Tag Is Nothing Then ShowPackage(e.Node)
    End Sub

    Private Sub ApplicationsForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        MsgInfo("Shorcuts", "F11: Edit location" + CrLf + "F12: Edit version")
    End Sub

    Private Sub ApplicationsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.F11
                Using d As New OpenFileDialog
                    d.SetInitDir(s.Storage.GetString(ActivePackage.Name + "custom path"))

                    If Not ActivePackage.Filenames.NothingOrEmpty Then
                        d.Filter = "|" + ActivePackage.Filenames.Join(";") + "|All Files|*.*"
                    Else
                        d.Filter = "|" + ActivePackage.Filename + "|All Files|*.*"
                    End If

                    If d.ShowDialog = DialogResult.OK Then
                        s.Storage.SetString(ActivePackage.Name + "custom path", d.FileName)
                    End If
                End Using
            Case Keys.F12
                If Not File.Exists(ActivePackage.GetPath) Then Exit Sub

                Dim input = InputBox.Show("What's the name of this version?", "StaxRip", ActivePackage.Version)

                If input <> "" Then
                    input = input.Replace(";", "_")
                    ActivePackage.Version = input
                    ActivePackage.VersionDate = File.GetLastWriteTimeUtc(ActivePackage.GetPath)

                    Dim textContent As String

                    For Each i In Packs.Packages
                        If i.Version <> "" Then
                            textContent += i.ID + " = " + i.Version + "; " +
                                i.VersionDate.ToString("yyyy-MM-dd",
                                                       CultureInfo.InvariantCulture) + CrLf 'persian calendar
                        End If
                    Next

                    File.WriteAllText(CommonDirs.Startup + "Apps\Versions.txt", textContent.FormatColumn("="))
                End If
        End Select

        ShowActivePackage()
    End Sub

    Private Sub SearchTextBox_TextChanged() Handles SearchTextBox.TextChanged
        tv.Nodes.Clear()

        For Each i In Packs.Packages
            Dim plugin = TryCast(i, PluginPackage)

            Dim searchString = i.Name + i.Description + i.Version +
                plugin?.VapourSynthFilterNames.Join(" ") +
                plugin?.AviSynthFilterNames.Join(" ")

            If searchString?.ToLower.Contains(SearchTextBox.Text?.ToLower) Then
                If plugin Is Nothing Then
                    If i Is Packs.Java OrElse
                        i Is Packs.Python OrElse
                        i Is Packs.vscpp2013 OrElse
                        i Is Packs.vscpp2015 Then

                        Dim n = tv.AddNode("Runtimes|" + i.Name)
                        Nodes.Add(n)
                        n.Tag = i
                    Else
                        Dim n = tv.AddNode("Apps|" + i.Name)
                        Nodes.Add(n)
                        n.Tag = i
                    End If
                Else
                    If plugin.AviSynthFilterNames?.Length > 0 Then
                        Dim n = tv.AddNode("Plugins|AviSynth|" + i.Name)
                        Nodes.Add(n)
                        n.Tag = i
                    End If

                    If plugin.VapourSynthFilterNames?.Length > 0 Then
                        Dim n = tv.AddNode("Plugins|VapourSynth|" + i.Name)
                        Nodes.Add(n)
                        n.Tag = i
                    End If
                End If
            End If
        Next

        If tv.Nodes.Count > 0 Then tv.SelectedNode = tv.Nodes(0)
        ToolStrip.Enabled = tv.Nodes.Count > 0
        flp.Enabled = tv.Nodes.Count > 0
        If SearchTextBox.Text <> "" Then tv.ExpandAll()
    End Sub

    Private Sub tsbLaunch_Click(sender As Object, e As EventArgs) Handles tsbLaunch.Click
        ActivePackage.LaunchAction.Invoke()
    End Sub

    <DebuggerNonUserCode()>
    Private Sub tsbOpenDir_Click(sender As Object, e As EventArgs) Handles tsbOpenDir.Click
        g.OpenDirAndSelectFile(ActivePackage.GetPath, Handle)
    End Sub

    Private Sub tsbHelp_Click(sender As Object, e As EventArgs) Handles tsbHelp.Click
        g.ShellExecute(ActivePackage.GetHelpPath)
    End Sub

    Private Sub tsbWebsite_Click(sender As Object, e As EventArgs) Handles tsbWebsite.Click
        g.ShellExecute(ActivePackage.WebURL)
    End Sub
End Class