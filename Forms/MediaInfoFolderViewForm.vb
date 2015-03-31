Imports StaxRip.UI
Imports System.Threading

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
        Me.lv.Size = New System.Drawing.Size(914, 601)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'MediaInfoFolderViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.ClientSize = New System.Drawing.Size(914, 601)
        Me.Controls.Add(Me.lv)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
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
    Dim Files As String()
    Dim Folder As String

    Sub New(folder As String)
        MyBase.New()
        InitializeComponent()

        Icon = My.Resources.MainIcon

        lv.View = View.Details
        lv.FullRowSelect = True

        For Each i In {"Filename", "Filetype", "Codec", "Aspect Ratio", "Dimension", "Bitrate",
                       "Duration", "Filesize", "Framerate", "Audiocodec"}

            Dim ch = lv.Columns.Add(i)
        Next

        For Each i As ColumnHeader In lv.Columns
            i.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        Me.Folder = folder
        Files = Directory.GetFiles(folder)

        Dim t As New Thread(AddressOf Populate)
        t.Start()
    End Sub

    Sub Populate()
        Dim kind = MediaInfoStreamKind.Video

        For x = 0 To Files.Length - 1
            If Abort Then
                Exit For
            End If

            Dim fp = Files(x)

            Using mi As New MediaInfo(fp)
                Dim codec = mi.GetInfo(kind, "Format")

                If codec = "" Then
                    Continue For
                End If

                Select Case codec
                    Case "MPEG-4 Visual"
                        codec = mi.GetInfo(kind, "CodecID/Hint")
                    Case "MPEG Video"
                        If mi.GetInfo(kind, "Format_Version") = "Version 1" Then
                            codec = "MPEG-1"
                        ElseIf mi.GetInfo(kind, "Format_Version") = "Version 2" Then
                            codec = "MPEG-2"
                        End If
                End Select

                Dim audioFormat = mi.GetInfo(MediaInfoStreamKind.Audio, "Format")

                If audioFormat = "MPEG Audio" Then
                    audioFormat = mi.GetInfo(MediaInfoStreamKind.Audio, "CodecID/Hint")
                End If

                Dim item As New ListViewItem
                item.Text = Path.GetFileName(fp)
                item.Tag = item.Text

                item.SubItems.Add(GetSubItem(Path.GetExtension(fp)))
                item.SubItems.Add(GetSubItem(codec))
                item.SubItems.Add(GetSubItem(mi.GetInfo(kind, "DisplayAspectRatio")))
                item.SubItems.Add(GetSubItem(mi.GetInfo(kind, "Width") + " x " + mi.GetInfo(kind, "Height")))
                item.SubItems.Add(GetSubItem(mi.GetInfo(kind, "BitRate/String"), mi.GetInfo(kind, "BitRate").ToInt))
                item.SubItems.Add(GetSubItem(mi.GetInfo(kind, "Duration/String"), mi.GetInfo(kind, "Duration").ToInt))
                item.SubItems.Add(GetSubItem(mi.GetInfo(MediaInfoStreamKind.General, "FileSize/String"), CLng(mi.GetInfo(MediaInfoStreamKind.General, "FileSize"))))
                item.SubItems.Add(GetSubItem(mi.GetInfo(kind, "FrameRate/String"), mi.GetInfo(kind, "FrameRate").ToSingle))
                item.SubItems.Add(GetSubItem(audioFormat))

                Invoke(Sub()
                           lv.Items.Add(item)

                           If lv.Items.Count = 0 OrElse lv.Items.Count = 10 Then
                               For Each i As ColumnHeader In lv.Columns
                                   i.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                               Next
                           End If
                       End Sub)
            End Using
        Next

        Invoke(Sub() AfterPopulate())
    End Sub

    Sub AfterPopulate()
        lv.ListViewItemSorter = New ListViewEx.ColumnSorter

        For Each i As ColumnHeader In lv.Columns
            i.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        Completed = True

        If Abort Then
            Close()
        End If
    End Sub

    Function GetSubItem(text As String, Optional sort As Object = Nothing) As ListViewItem.ListViewSubItem
        Dim r As New ListViewItem.ListViewSubItem
        r.Text = text

        If sort Is Nothing Then
            r.Tag = text
        Else
            r.Tag = sort
        End If

        Return r
    End Function

    Private Sub MediaInfoFolderViewForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Abort = True

        If Not Completed Then
            e.Cancel = True
        End If
    End Sub

    Private Sub lv_DoubleClick() Handles lv.DoubleClick
        If lv.SelectedItems.Count = 1 Then
            g.OpenDirAndSelectFile(Folder + lv.SelectedItems(0).Text, Handle)
        End If
    End Sub
End Class