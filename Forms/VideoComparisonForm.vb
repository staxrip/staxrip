
Imports System.Drawing.Imaging
Imports System.Reflection
Imports StaxRip.UI

Public Class VideoComparisonForm
    Shared Property Pos As Integer

    Public CropLeft, CropTop, CropRight, CropBottom As Integer

    Shadows Menu As ContextMenuStripEx

    Event UpdateMenu()

    Sub New()
        InitializeComponent()
        RestoreClientSize(53, 36)

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
        Menu.Add("Crop and Zoom...", AddressOf CropZoom, Keys.C)
        Menu.Add("Go To Frame...", AddressOf GoToFrame, Keys.F, enabledFunc)
        Menu.Add("Go To Time...", AddressOf GoToTime, Keys.T, enabledFunc)
        Menu.Add("Select next tab", AddressOf NextTab, Keys.Space, enabledFunc)
        Menu.Add("Navigate | 1 frame backward", Sub() TrackBar.Value -= 1, Keys.Left, enabledFunc)
        Menu.Add("Navigate | 1 frame forward", Sub() TrackBar.Value += 1, Keys.Right, enabledFunc)
        Menu.Add("Navigate | 100 frame backward", Sub() TrackBar.Value -= 100, Keys.Left Or Keys.Control, enabledFunc)
        Menu.Add("Navigate | 100 frame forward", Sub() TrackBar.Value += 100, Keys.Right Or Keys.Control, enabledFunc)
        Menu.Add("Help", AddressOf Me.Help, Keys.F1)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim ret = MyBase.CreateParams
            ret.ExStyle = ret.ExStyle Or &H2000000 'WS_EX_COMPOSITED
            Return ret
        End Get
    End Property

    Sub Add()
        If Not Package.AviSynth.VerifyOK(True) Then
            Exit Sub
        End If

        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.Storage.GetString("video comparison folder"))

            If dialog.ShowDialog() = DialogResult.OK Then
                s.Storage.SetString("video comparison folder", dialog.FileName.Dir)

                For Each file In dialog.FileNames
                    Add(file)
                Next
            End If
        End Using
    End Sub

    Sub Remove()
        If Not TabControl.SelectedTab Is Nothing Then
            Dim tab = TabControl.SelectedTab
            TabControl.TabPages.Remove(tab)
            tab.Dispose()
            RaiseEvent UpdateMenu()
        End If
    End Sub

    Sub Save()
        For Each tab As VideoTab In TabControl.TabPages
            Dim outputPath = tab.SourceFile.Dir & Pos & " " + tab.SourceFile.Base + ".png"

            Using bmp = tab.GetBitmap
                bmp.Save(outputPath, ImageFormat.Png)
            End Using
        Next

        MsgInfo("Images were saved in the source file directory.")
    End Sub

    Sub Add(sourePath As String)
        Dim tab = New VideoTab()
        tab.Form = Me
        tab.VideoPanel.ContextMenuStrip = TabControl.ContextMenuStrip

        If tab.Open(sourePath) Then
            TabControl.TabPages.Add(tab)
            Dim page = DirectCast(TabControl.SelectedTab, VideoTab)
            page.DoLayout()
            page.TrackBarValueChanged()
            RaiseEvent UpdateMenu()
        Else
            tab.Dispose()
        End If
    End Sub

    Sub TrackBar_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar.ValueChanged
        TrackBarValueChanged()
    End Sub

    Sub TrackBarValueChanged()
        If TabControl.TabPages.Count > 0 Then
            DirectCast(TabControl.SelectedTab, VideoTab).TrackBarValueChanged()
        End If
    End Sub

    Sub Help()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteParagraph("In the statistic tab of the x265 dialog select Log Level Frame and enable CSV log file creation, the video comparison tool can displays containing frame info.")
        form.Doc.WriteTips(Menu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", Menu.GetKeys, False)
        form.Show()
    End Sub

    Sub NextTab()
        Dim index = TabControl.SelectedIndex + 1

        If index >= TabControl.TabPages.Count Then
            index = 0
        End If

        If index <> TabControl.SelectedIndex Then
            TabControl.SelectedIndex = index
        End If
    End Sub

    Sub Reload()
        For Each tab As VideoTab In TabControl.TabPages
            tab.Reload()
        Next
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        Dim value = 100

        If e.Delta < 0 Then
            value = value * -1
        End If

        If s.ReverseVideoScrollDirection Then
            value = value * -1
        End If

        TrackBar.Value += value
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        Dispose()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Add()
    End Sub

    Sub CropZoom()
        Using form As New SimpleSettingsForm("Crop and Zoom")
            form.Height = CInt(form.Height * 0.6)
            form.Width = CInt(form.Width * 0.6)
            Dim ui = form.SimpleUI
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

            page.ResumeLayout()

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()
                Reload()
                TrackBarValueChanged()
            End If
        End Using
    End Sub

    Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", TrackBar.Value.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            TrackBar.Value = pos
        End If
    End Sub

    Sub GoToTime()
        Dim tab = DirectCast(TabControl.SelectedTab, VideoTab)
        Dim d As Date
        d = d.AddSeconds(Pos / tab.Server.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))
        Dim time As TimeSpan

        If value <> "" AndAlso TimeSpan.TryParse(value, time) Then
            TrackBar.Value = CInt((time.TotalMilliseconds / 1000) * tab.Server.FrameRate)
        End If
    End Sub

    Public Class VideoTab
        Inherits TabPage

        Property Server As IFrameServer
        Property Form As VideoComparisonForm
        Property SourceFile As String
        Property VideoPanel As Panel

        Private Renderer As VideoRenderer
        Private FrameInfo As String()

        Sub New()
            VideoPanel = New Panel
            AddHandler VideoPanel.Paint, Sub() Draw()
            Controls.Add(VideoPanel)
        End Sub

        Sub Reload()
            Server.Dispose()
            Open(SourceFile)
        End Sub

        Function Open(sourePath As String) As Boolean
            Text = sourePath.Base
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

            script.Synchronize(True, True, True)
            Server = FrameServerFactory.Create(script.Path)
            Renderer = New VideoRenderer(VideoPanel, Server)

            FileHelp.Delete(sourePath + ".ffindex")

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
            Renderer.Position = Pos
            Renderer.Draw()
        End Sub

        Function GetBitmap() As Bitmap
            Return BitmapUtil.CreateBitmap(Server, Pos)
        End Function

        Sub TrackBarValueChanged()
            Try
                Pos = Form.TrackBar.Value
                Draw()

                If Not FrameInfo Is Nothing Then
                    Form.laInfo.Text = FrameInfo(Form.TrackBar.Value)
                Else
                    Dim frameRate = If(Calc.IsValidFrameRate(Server.FrameRate), Server.FrameRate, 25)
                    Dim dt = DateTime.Today.AddSeconds(Pos / frameRate)
                    Form.laInfo.Text = "Position: " & Pos & ", Time: " + dt.ToString("HH:mm:ss.fff") + ", Size: " & Server.Info.Width & " x " & Server.Info.Height
                End If

                Form.laInfo.Refresh()
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End Sub

        Sub DoLayout()
            If Server Is Nothing Then
                Exit Sub
            End If

            Dim sizeToFit = New Size(Server.Info.Width, Server.Info.Height)

            If sizeToFit.IsEmpty Then
                Exit Sub
            End If

            Dim padding As Padding

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

            VideoPanel.Left = targetRect.Left
            VideoPanel.Top = targetRect.Top
            VideoPanel.Width = targetRect.Width
            VideoPanel.Height = targetRect.Height
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Not Server Is Nothing Then
                Server.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

        Sub VideoTab_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            DoLayout()
        End Sub
    End Class
End Class
