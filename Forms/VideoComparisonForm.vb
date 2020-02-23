
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Imports StaxRip.UI

Public Class VideoComparisonForm
    Shared Property Pos As Integer

    Public CropLeft, CropTop, CropRight, CropBottom As Integer
    Public Zoom As Integer = 100

    Shadows Menu As ContextMenuStripEx

    Event UpdateMenu()

    Public Sub New()
        InitializeComponent()
        KeyPreview = True
        bnMenu.TabStop = False
        TabControl.AllowDrop = True
        TrackBar.NoMouseWheelEvent = True

        Dim enabledFunc = Function() Not TabControl.SelectedTab Is Nothing
        Menu = New ContextMenuStripEx()
        Menu.Form = Me

        bnMenu.ContextMenuStrip = Menu
        TabControl.ContextMenuStrip = Menu

        Menu.Add("Add files to compare...", AddressOf Add, Keys.O, "Video files to compare, the file browser has multiselect enabled.")
        Menu.Add("Close selected tab", AddressOf Remove, Keys.Delete, enabledFunc)
        Menu.Add("Save PNGs at current position", AddressOf Save, Keys.S, enabledFunc, "Saves a PNG image for every file/tab at the current position in the directory of the source file.")
        Menu.Add("Crop and Zoom...", AddressOf CropZoom, Keys.C, enabledFunc)
        Menu.Add("Go To Frame...", AddressOf GoToFrame, Keys.F, enabledFunc)
        Menu.Add("Go To Time...", AddressOf GoToTime, Keys.T, enabledFunc)
        Menu.Add("Select next tab", AddressOf NextTab, Keys.Space, enabledFunc)
        Menu.Add("Navigate | 1 frame backward", Sub() TrackBar.Value -= 1, Keys.Left, enabledFunc)
        Menu.Add("Navigate | 1 frame forward", Sub() TrackBar.Value += 1, Keys.Right, enabledFunc)
        Menu.Add("Navigate | 100 frame backward", Sub() TrackBar.Value -= 100, Keys.Left Or Keys.Control, enabledFunc)
        Menu.Add("Navigate | 100 frame forward", Sub() TrackBar.Value += 100, Keys.Right Or Keys.Control, enabledFunc)
        Menu.Add("Help", AddressOf Me.Help, Keys.F1)
    End Sub

    Sub Add()
        If Not Package.AviSynth.VerifyOK(True) Then
            Exit Sub
        End If

        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.Storage.GetString("video comparison folder"))

            If dialog.ShowDialog() = DialogResult.OK Then
                s.Storage.SetString("video comparison folder", FilePath.GetDir(dialog.FileName))

                For Each i In dialog.FileNames
                    Add(i)
                Next
            End If
        End Using
    End Sub

    Private Sub Remove()
        If Not TabControl.SelectedTab Is Nothing Then
            Dim tab = TabControl.SelectedTab
            TabControl.TabPages.Remove(tab)
            tab.Dispose()
            RaiseEvent UpdateMenu()
        End If
    End Sub

    Private Sub Save()
        For Each i As VideoTab In TabControl.TabPages
            Dim outputPath = i.SourceFile.Dir & Pos & " " + i.SourceFile.Base + ".png"

            Using b = i.GetBitmap
                b.Save(outputPath, ImageFormat.Png)
            End Using
        Next
    End Sub

    Sub Add(sourePath As String)
        Dim tab = New VideoTab()
        tab.Form = Me

        If tab.Open(sourePath) Then
            TabControl.TabPages.Add(tab)
            DirectCast(TabControl.SelectedTab, VideoTab).TrackBarValueChanged()
            RaiseEvent UpdateMenu()
            Application.DoEvents()
        Else
            tab.Dispose()
        End If
    End Sub

    Private Sub TrackBar_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar.ValueChanged
        TrackBarValueChanged()
    End Sub

    Private Sub CodecComparisonForm_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        Dim value = 100
        If e.Delta < 0 Then value = value * -1
        If s.ReverseVideoScrollDirection Then value = value * -1
        TrackBar.Value += value
    End Sub

    Sub TrackBarValueChanged()
        If Not TabControl.SelectedTab Is Nothing Then
            DirectCast(TabControl.SelectedTab, VideoTab).TrackBarValueChanged()
            Pos = TrackBar.Value
        End If
    End Sub

    Sub Help()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteP("In the statistic tab of the x265 dialog select Log Level Frame and enable CSV log file creation, the video comparison tool can displays containing frame info.")
        form.Doc.WriteTips(Menu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", Menu.GetKeys, False)
        form.Show()
    End Sub

    Private Sub NextTab()
        Dim index = TabControl.SelectedIndex + 1

        If index >= TabControl.TabPages.Count Then
            index = 0
        End If

        If index <> TabControl.SelectedIndex Then
            TabControl.SelectedIndex = index
        End If
    End Sub

    Sub Reload()
        For Each i As VideoTab In TabControl.TabPages
            i.Reload()
        Next
    End Sub

    Private Sub TabControl_Selected(sender As Object, e As TabControlEventArgs) Handles TabControl.Selected
        Dim tab = DirectCast(TabControl.SelectedTab, VideoTab)

        If Not tab Is Nothing Then
            tab.TrackBarValueChanged()
        End If
    End Sub

    Private Sub CodecComparisonForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Dispose()
    End Sub

    Private Sub CropZoom()
        Using f As New SimpleSettingsForm("Crop and Zoom")
            f.Height = CInt(f.Height * 0.6)
            f.Width = CInt(f.Width * 0.6)
            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim nb = ui.AddNum(page)
            nb.Label.Text = "Crop Left:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropLeft
            nb.NumEdit.SaveAction = Sub(value) CropLeft = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Top:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropTop
            nb.NumEdit.SaveAction = Sub(value) CropTop = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Right:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropRight
            nb.NumEdit.SaveAction = Sub(value) CropRight = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Bottom:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropBottom
            nb.NumEdit.SaveAction = Sub(value) CropBottom = CInt(value)

            ui.AddLine(page)

            nb = ui.AddNum(page)
            nb.Label.Text = "Zoom:"
            nb.NumEdit.Config = {0, 1000, 10}
            nb.NumEdit.Value = Zoom
            nb.NumEdit.SaveAction = Sub(value) Zoom = CInt(value)

            page.ResumeLayout()

            If f.ShowDialog() = DialogResult.OK Then
                ui.Save()
                Reload()
                TrackBarValueChanged()
            End If
        End Using
    End Sub

    Private Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", TrackBar.Value.ToString)
        Dim pos As Integer
        If Integer.TryParse(value, pos) Then TrackBar.Value = pos
    End Sub

    Private Sub GoToTime()
        Dim tab = DirectCast(TabControl.SelectedTab, VideoTab)
        Dim d As Date
        d = d.AddSeconds(Pos / tab.Server.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))
        Dim time As TimeSpan

        If value <> "" AndAlso TimeSpan.TryParse(value, time) Then
            TrackBar.Value = CInt((time.TotalMilliseconds / 1000) * tab.Server.FrameRate)
        End If
    End Sub

    Private Sub VideoComparisonForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        WindowState = FormWindowState.Normal
    End Sub

    Public Class VideoTab
        Inherits TabPage

        Property Server As FrameServer
        Property Form As VideoComparisonForm
        Property SourceFile As String

        Private FrameInfo As String()

        Sub New()
            SetStyle(ControlStyles.Opaque, True)
        End Sub

        Sub Reload()
            Server.Dispose()
            Open(SourceFile)
        End Sub

        Function Open(sourePath As String) As Boolean
            Text = FilePath.GetBase(sourePath)
            SourceFile = sourePath

            Dim script As New VideoScript
            script.Engine = ScriptEngine.AviSynth
            script.Path = Folder.Temp + Guid.NewGuid.ToString + ".avs"
            AddHandler Disposed, Sub() FileHelp.Delete(script.Path)

            script.Filters.Add(New VideoFilter("SetMemoryMax(512)"))

            If sourePath.Ext = "png" Then
                script.Filters.Add(New VideoFilter("ImageSource(""" + sourePath + """, end = 0)"))
            Else
                Try
                    Dim cachePath = Folder.Temp + Guid.NewGuid.ToString + ".ffindex"
                    AddHandler Disposed, Sub() FileHelp.Delete(cachePath)
                Catch
                End Try

                If sourePath.EndsWith("mp4") Then
                    script.Filters.Add(New VideoFilter("LSMASHVideoSource(""" + sourePath + "" + """, format = ""YV12"")"))
                Else
                    script.Filters.Add(New VideoFilter("FFVideoSource(""" + sourePath + "" + """, colorspace = ""YV12"")"))
                End If
            End If

            If (Form.CropLeft Or Form.CropTop Or Form.CropRight Or Form.CropBottom) <> 0 Then
                script.Filters.Add(New VideoFilter("Crop(" & Form.CropLeft & ", " & Form.CropTop & ", -" & Form.CropRight & ", -" & Form.CropBottom & ")"))
            End If

            If Form.Zoom <> 100 Then
                script.Filters.Add(New VideoFilter("Spline64Resize(Int(width / 100.0 * " & Form.Zoom & "), Int(height / 100.0 * " & Form.Zoom & "))"))
            End If

            script.Synchronize(True)
            Server = New FrameServer(script.Path)

            Try
                FileHelp.Delete(sourePath + ".ffindex")
            Catch ex As Exception
            End Try

            If Form.TrackBar.Maximum < Server.Info.FrameCount - 1 Then
                Form.TrackBar.Maximum = Server.Info.FrameCount - 1
            End If

            Dim csvFile = sourePath.DirAndBase + ".csv"

            If File.Exists(csvFile) Then
                Dim len = Form.TrackBar.Maximum
                Dim lines = File.ReadAllLines(csvFile)

                If lines.Length > len Then
                    FrameInfo = New String(len) {}
                    Dim headers = lines(0).Split({","c})

                    For x = 1 To len + 1
                        Dim values = lines(x).Split({","c})

                        For x2 = 0 To headers.Length - 1
                            Dim value = values(x2).Trim

                            If value <> "" AndAlso value <> "-" Then
                                FrameInfo(x - 1) += headers(x2).Trim + ": " + value + ", "
                            End If
                        Next

                        FrameInfo(x - 1) = FrameInfo(x - 1).TrimEnd(" ,".ToCharArray)
                    Next
                End If
            End If

            Return True
        End Function

        Sub Draw()
            Dim padding As Padding
            Dim sizeToFit = New Size(Server.Info.Width, Server.Info.Height)

            Dim rect As New Rectangle(
                padding.Left, padding.Top,
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
                g.DrawImage(GetBitmap, targetRect)
            End Using
        End Sub

        Function GetBitmap() As Bitmap
            Dim ret = BitmapUtil.CreateBitmap(Server, Pos)
            ret.RotateFlip(RotateFlipType.RotateNoneFlipY)

            Using g = Graphics.FromImage(ret)
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Dim text = FilePath.GetBase(SourceFile)
                Dim fontSize = ret.Height \ 100

                If fontSize < 10 Then
                    fontSize = 10
                End If

                Dim font = New Font("Arial", fontSize)
                Dim size = g.MeasureString(text, font)
                Dim rect = New RectangleF(font.Height \ 2, font.Height \ 2, size.Width, size.Height)
                g.FillRectangle(Brushes.DarkGray, rect)
                g.DrawString(text, font, Brushes.White, rect)
            End Using

            Return ret
        End Function

        Sub TrackBarValueChanged()
            Pos = Form.TrackBar.Value

            Try
                Draw()
            Catch ex As Exception
                Form.Reload()

                Try
                    Draw()
                Catch ex2 As Exception
                    g.ShowException(ex2)
                End Try
            End Try

            If Not FrameInfo Is Nothing Then
                Form.lInfo.Text = FrameInfo(Form.TrackBar.Value)
            Else
                Dim d As Date
                d = d.AddSeconds(Pos / Server.FrameRate)
                Form.lInfo.Text = "Position: " & Pos & ", Time: " + d.ToString("HH:mm:ss.fff") + ", Size: " & Server.Info.Width & " x " & Server.Info.Height
            End If

            Form.lInfo.Refresh()
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Draw()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Not Server Is Nothing Then
                Server.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub
    End Class
End Class