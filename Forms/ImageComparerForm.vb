Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Public Class ImageComparerForm
    Shared Property Pos As Integer

    Public Sub New()
        InitializeComponent()
        UpdateControls()
    End Sub

    Private Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Using f As New OpenFileDialog
            f.SetFilter(FileTypes.Video)
            f.Multiselect = True

            If f.ShowDialog() = DialogResult.OK Then
                For Each i In f.FileNames
                    Add(i)
                Next
            End If
        End Using

        UpdateControls()
    End Sub

    Private Sub bnRemove_Click(sender As Object, e As EventArgs) Handles bnRemove.Click
        If Not TabControl.SelectedTab Is Nothing Then
            Dim tab = TabControl.SelectedTab
            TabControl.TabPages.Remove(tab)
            tab.Dispose()
        End If

        UpdateControls()
    End Sub

    Private Sub bnSave_Click(sender As Object, e As EventArgs) Handles bnSave.Click
        For Each i As VideoTab In TabControl.TabPages
            i.AVI.Position = Pos
            Dim outputPath = Filepath.GetDir(i.SourceFile) & Pos & " " + Filepath.GetBase(i.SourceFile) + ".png"

            Using b = i.AVI.GetBitmap
                b.Save(outputPath, ImageFormat.Png)
            End Using
        Next
    End Sub

    Sub Add(sourePath As String)
        Dim tab = New VideoTab()
        tab.Form = Me
        tab.Open(sourePath)
        TabControl.TabPages.Add(tab)
        tab.TrackBarScroll()
    End Sub

    Private Sub TrackBar_Scroll(sender As Object, e As EventArgs) Handles TrackBar.Scroll
        DirectCast(TabControl.SelectedTab, VideoTab).TrackBarScroll()
        Pos = TrackBar.Value
    End Sub

    Private Sub TabControl_Selected(sender As Object, e As TabControlEventArgs) Handles TabControl.Selected
        DirectCast(TabControl.SelectedTab, VideoTab).TrackBarScroll()
    End Sub

    Private Sub TabControl_Deselecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControl.Deselecting
        For Each i As VideoTab In TabControl.TabPages
            i.AVI.Position = Pos
        Next
    End Sub

    Sub UpdateControls()
        bnRemove.Enabled = TabControl.TabPages.Count > 0
        bnSave.Enabled = TabControl.TabPages.Count > 0
        TrackBar.Enabled = TabControl.TabPages.Count > 0
    End Sub

    Class VideoTab
        Inherits TabPage

        Property AVI As AVIFile
        Property Form As ImageComparerForm
        Property SourceFile As String

        Private FrameInfo As String()

        Sub New()
            SetStyle(ControlStyles.Opaque, True)
        End Sub

        Sub Open(sourePath As String)
            Text = Filepath.GetBase(sourePath)
            SourceFile = sourePath

            Dim doc As New AviSynthDocument
            doc.Filters.Add(New AviSynthFilter("FFVideoSource(""" + sourePath + """)"))
            doc.Path = sourePath + ".avs"

            If p.SourceHeight > 576 Then
                doc.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec709"")"))
            Else
                doc.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec601"")"))
            End If

            doc.Synchronize()
            AVI = New AVIFile(doc.Path)
            Form.TrackBar.Maximum = AVI.FrameCount - 1

            Dim csvFile = Filepath.GetDirAndBase(sourePath) + ".csv"

            If File.Exists(csvFile) Then
                Dim len = Form.TrackBar.Maximum
                Dim lines = File.ReadAllLines(csvFile)

                If lines.Length > len Then
                    FrameInfo = New String(len) {}
                    Dim headers = lines(0).Split({","c})

                    For x = 1 To len + 1
                        Dim values = lines(x).Split({","c})

                        For x2 = 0 To headers.Length - 1
                            FrameInfo(x - 1) += headers(x2).Trim + ": " + values(x2).Trim + ", "
                        Next

                        FrameInfo(x - 1) = FrameInfo(x - 1).TrimEnd(" ,".ToCharArray)
                    Next
                End If
            End If
        End Sub

        Sub Draw()
            Dim padding As Padding
            Dim sizeToFit = New Size(AVI.FrameSize.Width, AVI.FrameSize.Height)

            Dim rect As New Rectangle(padding.Left, padding.Top,
                                      Width - padding.Horizontal,
                                      Height - padding.Vertical)
            Dim targetPoint As Point
            Dim targetSize As Size
            Dim ar1 = rect.Width / rect.Height
            Dim ar2 = sizeToFit.Width / sizeToFit.Height

            If ar2 < ar1 Then
                targetSize.Height = rect.Height
                targetSize.Width = CInt(sizeToFit.Width / (sizeToFit.Height / rect.Height))
                targetPoint.X = CInt((rect.Width - targetSize.Width) / 2) + padding.Left
                targetPoint.Y = padding.Top
            Else
                targetSize.Width = rect.Width
                targetSize.Height = CInt(sizeToFit.Height / (sizeToFit.Width / rect.Width))
                targetPoint.Y = CInt((rect.Height - targetSize.Height) / 2) + padding.Top
                targetPoint.X = padding.Left
            End If

            Dim targetRect = New Rectangle(targetPoint, targetSize)
            Dim reg As New Region(ClientRectangle)
            reg.Exclude(targetRect)

            Using g = CreateGraphics()
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.FillRegion(Brushes.Black, reg)
                g.DrawImage(AVI.GetBitmap, targetRect)
            End Using
        End Sub

        Sub TrackBarScroll()
            AVI.Position = Form.TrackBar.Value
            Draw()

            If Not FrameInfo Is Nothing Then
                Form.lInfo.Text = FrameInfo(Form.TrackBar.Value)
                Form.lInfo.Refresh()
            Else
                Form.lInfo.Text = ""
            End If
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Draw()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            AVI.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Sub bnHelp_Click(sender As Object, e As EventArgs) Handles bnHelp.Click
        MsgInfo("In the x265 dialog in the statistic tab choose Log Level Frame and check writing a CSV log file, this will create a CSV file with identical file path except extension to the target file which the comparison tool uses to display frame info.")
    End Sub
End Class