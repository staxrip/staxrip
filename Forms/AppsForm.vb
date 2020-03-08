
Imports System.Globalization
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
    Friend WithEvents tsbPath As ToolStripButton
    Friend WithEvents tsbVersion As ToolStripButton
    Friend WithEvents ddTools As ToolStripDropDownButton
    Friend WithEvents miCSV As ToolStripMenuItem
    Friend WithEvents tsbExplore As System.Windows.Forms.ToolStripButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppsForm))
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.tsbLaunch = New System.Windows.Forms.ToolStripButton()
        Me.tsbExplore = New System.Windows.Forms.ToolStripButton()
        Me.tsbWebsite = New System.Windows.Forms.ToolStripButton()
        Me.tsbDownload = New System.Windows.Forms.ToolStripButton()
        Me.tsbPath = New System.Windows.Forms.ToolStripButton()
        Me.tsbVersion = New System.Windows.Forms.ToolStripButton()
        Me.ddTools = New System.Windows.Forms.ToolStripDropDownButton()
        Me.miCSV = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.tv.Size = New System.Drawing.Size(401, 1018)
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
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbLaunch, Me.tsbExplore, Me.tsbWebsite, Me.tsbDownload, Me.tsbPath, Me.tsbVersion, Me.ddTools, Me.tsbHelp})
        Me.ToolStrip.Location = New System.Drawing.Point(421, 10)
        Me.ToolStrip.Margin = New System.Windows.Forms.Padding(0, 10, 10, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Padding = New System.Windows.Forms.Padding(5, 2, 2, 0)
        Me.ToolStrip.Size = New System.Drawing.Size(1256, 80)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "tsMain"
        '
        'tsbLaunch
        '
        Me.tsbLaunch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbLaunch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLaunch.Name = "tsbLaunch"
        Me.tsbLaunch.Size = New System.Drawing.Size(136, 69)
        Me.tsbLaunch.Text = "Launch"
        Me.tsbLaunch.ToolTipText = "Launches the app"
        '
        'tsbExplore
        '
        Me.tsbExplore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbExplore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExplore.Name = "tsbExplore"
        Me.tsbExplore.Size = New System.Drawing.Size(142, 69)
        Me.tsbExplore.Text = "Explore"
        Me.tsbExplore.ToolTipText = "Opens the apps folder in windows file explorer"
        '
        'tsbWebsite
        '
        Me.tsbWebsite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebsite.Name = "tsbWebsite"
        Me.tsbWebsite.Size = New System.Drawing.Size(117, 69)
        Me.tsbWebsite.Text = " Web "
        Me.tsbWebsite.ToolTipText = "Opens the apps website"
        '
        'tsbDownload
        '
        Me.tsbDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbDownload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownload.Name = "tsbDownload"
        Me.tsbDownload.Size = New System.Drawing.Size(185, 69)
        Me.tsbDownload.Text = "Download"
        Me.tsbDownload.ToolTipText = "Opens the apps download web page"
        '
        'tsbPath
        '
        Me.tsbPath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbPath.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPath.Name = "tsbPath"
        Me.tsbPath.Size = New System.Drawing.Size(113, 69)
        Me.tsbPath.Text = " Path "
        Me.tsbPath.ToolTipText = "Edits the apps path (F11)"
        '
        'tsbVersion
        '
        Me.tsbVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbVersion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVersion.Name = "tsbVersion"
        Me.tsbVersion.Size = New System.Drawing.Size(141, 69)
        Me.tsbVersion.Text = "Version"
        Me.tsbVersion.ToolTipText = "Edits the apps version (F12)"
        '
        'ddTools
        '
        Me.ddTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ddTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miCSV})
        Me.ddTools.Image = CType(resources.GetObject("ddTools.Image"), System.Drawing.Image)
        Me.ddTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ddTools.Name = "ddTools"
        Me.ddTools.Size = New System.Drawing.Size(152, 69)
        Me.ddTools.Text = " Tools "
        '
        'miCSV
        '
        Me.miCSV.Name = "miCSV"
        Me.miCSV.Size = New System.Drawing.Size(538, 66)
        Me.miCSV.Text = "Create CSV file"
        Me.miCSV.ToolTipText = "Generates a CSV file listing all tools"
        '
        'tsbHelp
        '
        Me.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHelp.Name = "tsbHelp"
        Me.tsbHelp.Size = New System.Drawing.Size(119, 69)
        Me.tsbHelp.Text = " Help "
        Me.tsbHelp.ToolTipText = "Opens the apps help (F1)"
        '
        'flp
        '
        Me.flp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flp.BackColor = System.Drawing.Color.White
        Me.flp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flp.Location = New System.Drawing.Point(421, 100)
        Me.flp.Margin = New System.Windows.Forms.Padding(0, 10, 10, 10)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(1256, 1018)
        Me.flp.TabIndex = 2
        '
        'SearchTextBox
        '
        Me.SearchTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchTextBox.Location = New System.Drawing.Point(11, 15)
        Me.SearchTextBox.Margin = New System.Windows.Forms.Padding(11, 10, 11, 0)
        Me.SearchTextBox.Name = "SearchTextBox"
        Me.SearchTextBox.Size = New System.Drawing.Size(399, 70)
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
        Me.tlpMain.Size = New System.Drawing.Size(1687, 1128)
        Me.tlpMain.TabIndex = 6
        '
        'AppsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1687, 1128)
        Me.Controls.Add(Me.tlpMain)
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

        ScaleClientSize(45, 30)
        tv.ItemHeight = CInt(FontHeight * 1.5)

        For Each mi As ToolStripMenuItem In ddTools.DropDownItems
            mi.AutoSize = False
            mi.Height = CInt(FontHeight * 1.5)
        Next

        Text = $"{Package.Items.Count} tools"
        SearchTextBox_TextChanged()

        tv.Scrollable = True
        ToolStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        g.SetRenderer(ToolStrip)

        AddHandler SetupButton.Click, Sub()
                                          CurrentPackage.SetupAction.Invoke
                                          ShowActivePackage()
                                      End Sub

        SetupButton.ForeColor = Color.Red
        SetupButton.Font = New Font("Segoe UI", 10)
        SetupButton.Margin = New Padding(FontHeight \ 3)
        SetupButton.Padding = New Padding(FontHeight \ 5)
        SetupButton.AutoSize = True
        SetupButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
        SetupButton.TextImageRelation = TextImageRelation.ImageBeforeText
        SetupButton.Image = StockIcon.GetSmallImage(StockIconIdentifier.Shield)

        AddHandler DownloadButton.Click, Sub() g.StartProcess(CurrentPackage.DownloadURL)
        DownloadButton.AutoSize = True
        DownloadButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
        DownloadButton.Font = New Font("Segoe UI", 10)
        DownloadButton.Margin = New Padding(FontHeight \ 3)
        DownloadButton.Padding = New Padding(FontHeight \ 5)

        Dim title = New Label With {
            .Font = New Font(flp.Font.FontFamily, 14 * s.UIScaleFactor, FontStyle.Bold),
            .AutoSize = True
        }

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
        Dim path = CurrentPackage.Path

        Headers("Title").Text = CurrentPackage.Name

        SetupButton.Text = "Install " + CurrentPackage.Name
        SetupButton.Visible = Not CurrentPackage.SetupAction Is Nothing AndAlso
            (CurrentPackage.IsStatusCritical OrElse (Not CurrentPackage.IsCorrectVersion AndAlso
            CurrentPackage.Version <> ""))

        DownloadButton.Text = "Download " + CurrentPackage.Name
        DownloadButton.Visible = CurrentPackage.DownloadURL <> "" AndAlso (CurrentPackage.IsStatusCritical OrElse (Not CurrentPackage.IsCorrectVersion AndAlso CurrentPackage.Version <> ""))

        tsbExplore.Enabled = path <> ""
        tsbLaunch.Enabled = Not CurrentPackage.LaunchAction Is Nothing AndAlso Not CurrentPackage.IsStatusCritical
        tsbWebsite.Enabled = CurrentPackage.WebURL <> ""
        tsbDownload.Enabled = CurrentPackage.DownloadURL <> ""
        tsbHelp.Enabled = CurrentPackage.HelpFileOrURL <> ""

        tsbVersion.Enabled = Not CurrentPackage.IgnoreVersion
        tsbPath.Enabled = CurrentPackage.FixedDir = ""

        s.StringDictionary("RecentExternalApplicationControl") = CurrentPackage.Name + CurrentPackage.Version

        flp.SuspendLayout()

        Contents("Status").Text = CurrentPackage.GetStatusDisplay()
        Contents("Location").Text = path

        If File.Exists(CurrentPackage.Path) Then
            Contents("Version").Text = CurrentPackage.Version + " (" + File.GetLastWriteTimeUtc(CurrentPackage.Path).ToShortDateString() + ")"
        Else
            Contents("Version").Text = CurrentPackage.Version
        End If

        Contents("Status").Font = New Font("Segoe UI", 10)
        Contents("Description").Text = CurrentPackage.Description

        Headers("AviSynth Filters").Visible = False
        Contents("AviSynth Filters").Visible = False

        Headers("VapourSynth Filters").Visible = False
        Contents("VapourSynth Filters").Visible = False

        Headers("Version").Visible = Not CurrentPackage.IgnoreVersion
        Contents("Version").Visible = Not CurrentPackage.IgnoreVersion

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
            If package Is i.Tag Then tv.SelectedNode = i
        Next
    End Sub

    Private Sub ShowPackage(tn As TreeNode)
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

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyData
            Case Keys.F1
                tsbHelp.PerformClick()
            Case Keys.F10
                Dim fp = Folder.Startup + "changelog.md"
                If File.Exists(fp) Then g.StartProcess(fp)
            Case Keys.F11
                tsbPath.PerformClick()
            Case Keys.F12
                tsbVersion.PerformClick()
        End Select

        MyBase.OnKeyDown(e)
    End Sub

    Private Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
        If e.Node.Tag Is Nothing AndAlso e.Node.Nodes.Count > 0 Then
            tv.SelectedNode = e.Node.Nodes(0)
        End If

        If Not e.Node.Tag Is Nothing Then ShowPackage(e.Node)
    End Sub

    Private Sub SearchTextBox_TextChanged() Handles SearchTextBox.TextChanged
        tv.BeginUpdate()
        tv.Nodes.Clear()

        For Each pack In Package.Items.Values
            Dim plugin = TryCast(pack, PluginPackage)

            Dim searchString = pack.Name + pack.Description + pack.Version +
                plugin?.VSFilterNames.Join(" ") + pack.Path +
                plugin?.AvsFilterNames.Join(" ")

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

        If tv.Nodes.Count > 0 Then tv.SelectedNode = tv.Nodes(0)
        ToolStrip.Enabled = tv.Nodes.Count > 0
        flp.Enabled = tv.Nodes.Count > 0
        If SearchTextBox.Text <> "" Then tv.ExpandAll()
        tv.EndUpdate()
    End Sub

    Private Sub tsbLaunch_Click(sender As Object, e As EventArgs) Handles tsbLaunch.Click
        CurrentPackage.LaunchAction?.Invoke()
    End Sub

    <DebuggerNonUserCode()>
    Private Sub tsbOpenDir_Click(sender As Object, e As EventArgs) Handles tsbExplore.Click
        g.SelectFileWithExplorer(CurrentPackage.Path)
    End Sub

    Private Sub tsbHelp_Click(sender As Object, e As EventArgs) Handles tsbHelp.Click
        CurrentPackage.ShowHelp()
    End Sub

    Private Sub tsbWebsite_Click(sender As Object, e As EventArgs) Handles tsbWebsite.Click
        g.StartProcess(CurrentPackage.WebURL)
    End Sub

    Private Sub tsbDownload_Click(sender As Object, e As EventArgs) Handles tsbDownload.Click
        g.StartProcess(CurrentPackage.DownloadURL)
    End Sub

    Private Sub tsbPath_Click(sender As Object, e As EventArgs) Handles tsbPath.Click
        Using d As New OpenFileDialog
            d.SetInitDir(s.Storage.GetString(CurrentPackage.Name + "custom path"))
            d.Filter = "|" + CurrentPackage.Filename + "|All Files|*.*"

            If d.ShowDialog = DialogResult.OK Then
                s.Storage.SetString(CurrentPackage.Name + "custom path", d.FileName)
                ShowActivePackage()
            End If
        End Using
    End Sub

    Private Sub tsbVersion_Click(sender As Object, e As EventArgs) Handles tsbVersion.Click
        If Not File.Exists(CurrentPackage.Path) Then
            Exit Sub
        End If

        Dim input = InputBox.Show("What's the name of this version?", "StaxRip", CurrentPackage.Version)

        If input <> "" Then
            input = input.Replace(";", "_")

            CurrentPackage.Version = input
            CurrentPackage.VersionDate = File.GetLastWriteTimeUtc(CurrentPackage.Path)

            Dim txt = Application.ProductVersion + BR2

            For Each pack In Package.Items.Values
                If pack.Version <> "" Then
                    txt += pack.ID + " = " + pack.VersionDate.ToString("yyyy-MM-dd",
                        CultureInfo.InvariantCulture) + "; " + pack.Version + BR 'persian calendar
                End If
            Next

            If Not Directory.Exists(Folder.Apps) Then
                Directory.CreateDirectory(Folder.Apps)
            End If

            txt.FormatColumn("=").WriteUTF8File(Folder.Apps + "Versions.txt")
            txt.FormatColumn("=").WriteUTF8File(Folder.Settings + "Versions.txt")

            ShowActivePackage()
        End If
    End Sub

    Private Sub miCSV_Click(sender As Object, e As EventArgs) Handles miCSV.Click
        Dim rows As New List(Of List(Of String))
        Dim headings = {"Name", "Type", "Filename", "Version", "Modified Date", "Folder"}
        rows.Add(New List(Of String)(headings))

        Dim add = Function(value As String) As String
                      If value Is Nothing Then
                          value = ""
                      End If

                      If value.Contains("""") Then
                          value = value.Replace("""", """""")
                      End If

                      If value.Contains(";") OrElse value.Contains("""") Then
                          value = """" + value + """"
                      End If

                      Return value
                  End Function

        For Each pair In Package.Items.OrderBy(Function(val) val.Value.Path)
            Dim row As New List(Of String)
            Dim pack = pair.Value
            Dim path = pack.Path

            'Name
            row.Add(add(pack.Name))

            'Type
            If Not pack.HelpSwitch Is Nothing Then
                row.Add("Console App")
            ElseIf pack.IsGUI Then
                row.Add("GUI App")
            ElseIf TypeOf pack Is PluginPackage Then
                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.AvsFilterNames.NothingOrEmpty Then
                    If plugin.Filename.Ext = "dll" Then
                        row.Add("AviSynth Plugin")
                    ElseIf plugin.Filename.Ext.EqualsAny("avs", "avsi") Then
                        row.Add("AviSynth Script")
                    Else
                        Throw New Exception()
                    End If
                ElseIf Not plugin.VSFilterNames.NothingOrEmpty Then
                    If plugin.Filename.Ext = "dll" Then
                        row.Add("VapourSynth Plugin")
                    ElseIf plugin.Filename.Ext = "py" Then
                        row.Add("VapourSynth Script")
                    Else
                        row.Add("")
                    End If
                Else
                    row.Add("")
                End If
            ElseIf pack.Filename.Ext = "dll" Then
                row.Add("Library")
            Else
                row.Add("Misc")
            End If

            'Filename
            row.Add(add(pack.Filename))

            'Version
            If pack.IsCorrectVersion Then
                row.Add(add("'" + pack.Version + "'"))
            Else
                row.Add("")
            End If

            'Modified Date
            If File.Exists(path) Then
                row.Add(File.GetLastWriteTime(path).ToShortDateString())
            Else
                row.Add("")
            End If

            'Folder
            row.Add(add(pack.GetDir))

            rows.Add(row)
        Next

        Dim text As String

        For Each rowList In rows
            text += String.Join(";", rowList) + BR
        Next

        Dim csvFile = Folder.Temp + "staxrip tools.csv"
        text.WriteUTF8File(csvFile)
        g.StartProcess(g.GetAppPathForExtension("csv", "txt"), csvFile.Escape)
    End Sub
End Class
