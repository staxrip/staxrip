Imports StaxRip.UI
Imports System.Threading.Tasks
Imports System.Text

Public Class MediaInfoFolderViewForm
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
    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents lv As StaxRip.UI.ListViewEx

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'lv
        '
        Me.lv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(1200, 601)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'MediaInfoFolderViewForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1200, 601)
        Me.Controls.Add(Me.lv)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "MediaInfoFolderViewForm"
        Me.ShowIcon = True
        Me.ShowInTaskbar = True
        Me.Text = "MediaInfo Folder View"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim Completed As Boolean
    Dim Abort As Boolean
    Dim Files As New List(Of String)
    Dim Folder As String

    Sub New(folder As String)
        MyBase.New()
        InitializeComponent()

        Icon = g.Icon

        lv.View = View.Details
        lv.FullRowSelect = True
        lv.MultiSelect = False

        AddHandler lv.MouseDoubleClick, Sub() ShowMediaInfo()

        For Each i In {
            " Path ", " Type ", " Codec ", " DAR ",
            " Width ", " Height ", " Bitrate ",
            " Duration ", " Filesize ", " Framerate ",
            " Scan Type ", " Interlacement ", " Colorimetry ",
            " Color Prim ", "Transfer", " Profile ",
            " Audiocodec ", " Subtitle Format "}

            lv.Columns.Add(i)
        Next

        For Each i As ColumnHeader In lv.Columns
            i.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        Me.Folder = folder

        Dim hs As New HashSet(Of String)

        For Each i In Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
            If FileTypes.Audio.Contains(i.Ext) OrElse FileTypes.Video.Contains(i.Ext) Then
                Files.Add(i)
            Else
                If i.Ext <> "ini" Then hs.Add(i.Ext)
            End If
        Next

        If hs.Count > 0 Then MsgWarn("Unknown file type(s): " + BR2 + hs.Join(", "))

        Dim cms As New ContextMenuStripEx()
        cms.Form = Me

        lv.SendMessageHideFocus()
        lv.ContextMenuStrip = cms
        Dim enabledFunc = Function() lv.SelectedItems.Count = 1
        Dim pathFunc = Function() DirectCast(lv.SelectedItems(0), ListViewItemEx).Path

        cms.Add("Show Media Info", AddressOf ShowMediaInfo, Keys.I, enabledFunc)
        cms.Add("Show in File Explorer", Sub() g.SelectFileWithExplorer(pathFunc()), Keys.E, enabledFunc)
        cms.Add("Play", Sub() g.Play(pathFunc()), Keys.P, enabledFunc)
        cms.Add("Copy path to clipboard", Sub() Clipboard.SetText(pathFunc()), Keys.C, enabledFunc)
        cms.Add("Open with StaxRip", Sub() g.MainForm.OpenVideoSourceFile(pathFunc()), Keys.O, enabledFunc)
        cms.Add("Save table as CSV", AddressOf SaveCSV, Keys.S, enabledFunc)
    End Sub

    Sub Populate()
        For x = 0 To Files.Count - 1
            If Abort Then Exit For

            Dim fp = Files(x)
            Dim codec = MediaInfo.GetVideoCodec(fp)
            If codec = "" Then Continue For

            Using mi As New MediaInfo(fp)
                Dim audioCodecs = MediaInfo.GetAudioCodecs(fp).Replace(" ", "")

                Dim item As New ListViewItemEx
                item.Text = " " + fp + " "
                item.Tag = item.Text
                item.Path = fp

                Dim width = mi.GetInfo(MediaInfoStreamKind.Video, "Width")
                Dim height = mi.GetInfo(MediaInfoStreamKind.Video, "Height")

                'If mi.GetAudioCount = 1 Then
                '    item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Audio, "StreamOrder") + " "))
                '    item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Audio, "ID") + " "))
                'ElseIf mi.GetAudioCount > 1 Then
                '    item.SubItems.Add(GetSubItem(" " + mi.GetAudio(0, "StreamOrder") + " " + mi.GetAudio(1, "StreamOrder") + " "))
                '    item.SubItems.Add(GetSubItem(" " + mi.GetAudio(0, "ID") + " " + mi.GetAudio(1, "ID") + " "))
                'Else
                '    item.SubItems.Add(GetSubItem(" "))
                '    item.SubItems.Add(GetSubItem(" "))
                'End If

                item.SubItems.Add(GetSubItem(" " + fp.Ext + " "))
                item.SubItems.Add(GetSubItem(" " + codec + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "DisplayAspectRatio") + " "))
                item.SubItems.Add(GetSubItem(" " + width, width.ToInt))
                item.SubItems.Add(GetSubItem(" " + height, height.ToInt))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "BitRate/String") + " ", mi.GetInfo(MediaInfoStreamKind.Video, "BitRate").ToInt))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.General, "Duration/String") + " ", mi.GetInfo(MediaInfoStreamKind.General, "Duration").ToInt))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.General, "FileSize/String") + " ", CLng(mi.GetInfo(MediaInfoStreamKind.General, "FileSize"))))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "FrameRate")))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "ScanType") + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "Interlacement") + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "Colorimetry") + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "colour_primaries") + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "transfer_characteristics") + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetInfo(MediaInfoStreamKind.Video, "Format_Profile") + " "))
                item.SubItems.Add(GetSubItem(" " + audioCodecs + " "))
                item.SubItems.Add(GetSubItem(" " + mi.GetGeneral("Text_Format_List") + " "))

                BeginInvoke(Sub()
                                lv.Items.Add(item)
                                If lv.Items.Count = 9 Then lv.AutoResizeColumns(False)
                            End Sub)
            End Using
        Next

        Invoke(Sub()
                   lv.ListViewItemSorter = New ListViewEx.ColumnSorter
                   lv.AutoResizeColumns(False)
                   Completed = True
                   If Abort Then Close()
               End Sub)
    End Sub

    Sub SaveCSV()
        Dim sb As New StringBuilder

        sb.Append(lv.Columns.Cast(Of ColumnHeader).Select(Function(arg) If(arg.Text.Contains(","), """" + arg.Text + """", arg.Text)).Join(",") + BR)

        For Each i As ListViewItem In lv.Items
            sb.Append(i.SubItems.Cast(Of ListViewItem.ListViewSubItem).Select(Function(arg) If(arg.Text.Contains(","), """" + arg.Text + """", arg.Text)).Join(",") + BR)
        Next

        Using f As New SaveFileDialog()
            f.AddExtension = True
            f.DefaultExt = "csv"
            f.FileName = "MediaInfo.csv"

            If f.ShowDialog = DialogResult.OK Then
                File.WriteAllText(f.FileName, sb.ToString)
            End If
        End Using
    End Sub

    Sub ShowMediaInfo()
        If lv.SelectedItems.Count = 0 Then Exit Sub

        Using f As New MediaInfoForm(DirectCast(lv.SelectedItems(0), ListViewItemEx).Path)
            f.ShowDialog()
        End Using
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Task.Run(AddressOf Populate)
    End Sub

    Function GetSubItem(text As String, Optional sort As Object = Nothing) As ListViewItem.ListViewSubItem
        Dim ret As New ListViewItem.ListViewSubItem
        ret.Text = text

        If sort Is Nothing Then
            ret.Tag = text
        Else
            ret.Tag = sort
        End If

        Return ret
    End Function

    Private Sub MediaInfoFolderViewForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Abort = True
        If Not Completed Then e.Cancel = True
    End Sub

    Public Class ListViewItemEx
        Inherits ListViewItem

        Property Path As String
    End Class
End Class