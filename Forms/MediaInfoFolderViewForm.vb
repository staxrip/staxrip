
Imports System.Threading.Tasks
Imports System.Text

Imports StaxRip.UI

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

        For Each columnHeader As ColumnHeader In lv.Columns
            columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        Dim hs As New HashSet(Of String)

        For Each file In Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
            If FileTypes.Audio.Contains(file.Ext) OrElse FileTypes.Video.Contains(file.Ext) Then
                Files.Add(file)
            Else
                If file.Ext <> "ini" Then
                    hs.Add(file.Ext)
                End If
            End If
        Next

        If hs.Count > 0 Then
            MsgWarn("Unknown file type(s): " + BR2 + hs.Join(", "))
        End If

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
            If Abort Then
                Exit For
            End If

            Dim videoFile = Files(x)
            Dim videoFormat = MediaInfo.GetVideoFormat(videoFile)

            If videoFormat = "" Then
                Continue For
            End If

            Using mi As New MediaInfo(videoFile)
                Dim audioCodecs = MediaInfo.GetAudioCodecs(videoFile).Replace(" ", "")

                Dim item As New ListViewItemEx
                item.Text = " " + videoFile + " "
                item.Tag = item.Text
                item.Path = videoFile

                Dim width = mi.GetInfo(MediaInfoStreamKind.Video, "Width")
                Dim height = mi.GetInfo(MediaInfoStreamKind.Video, "Height")

                item.SubItems.Add(GetSubItem(" " + videoFile.Ext + " "))
                item.SubItems.Add(GetSubItem(" " + videoFormat + " "))
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

                                If lv.Items.Count = 9 Then
                                    lv.AutoResizeColumns(False)
                                End If
                            End Sub)
            End Using
        Next

        Invoke(Sub()
                   lv.ListViewItemSorter = New ListViewEx.ColumnSorter
                   lv.AutoResizeColumns(False)
                   Completed = True

                   If Abort Then
                       Close()
                   End If
               End Sub)
    End Sub

    Sub SaveCSV()
        Dim sb As New StringBuilder
        sb.Append(lv.Columns.Cast(Of ColumnHeader).Select(Function(arg) If(arg.Text.Contains(","), """" + arg.Text + """", arg.Text)).Join(",") + BR)

        For Each item As ListViewItem In lv.Items
            sb.Append(item.SubItems.Cast(Of ListViewItem.ListViewSubItem).Select(Function(arg) If(arg.Text.Contains(","), """" + arg.Text + """", arg.Text)).Join(",") + BR)
        Next

        Using dialog As New SaveFileDialog()
            dialog.AddExtension = True
            dialog.DefaultExt = "csv"
            dialog.FileName = "MediaInfo.csv"

            If dialog.ShowDialog = DialogResult.OK Then
                File.WriteAllText(dialog.FileName, sb.ToString)
            End If
        End Using
    End Sub

    Sub ShowMediaInfo()
        If lv.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Using form As New MediaInfoForm(DirectCast(lv.SelectedItems(0), ListViewItemEx).Path)
            form.ShowDialog()
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

        If Not Completed Then
            e.Cancel = True
        End If
    End Sub

    Public Class ListViewItemEx
        Inherits ListViewItem

        Property Path As String
    End Class
End Class
