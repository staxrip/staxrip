
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    Public Class FormBase
        Inherits Form

        Event FilesDropped(files As String())

        Private FileDropValue As Boolean
        Private DefaultWidthScale As Single
        Private DefaultHeightScale As Single

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property FontHeight As Integer

        Sub New()
            Font = New Font("Segoe UI", 9)
            FontHeight = Font.Height
        End Sub

        <DefaultValue(False)>
        Property FileDrop As Boolean
            Get
                Return FileDropValue
            End Get
            Set(value As Boolean)
                FileDropValue = value
                AllowDrop = value
            End Set
        End Property

        Protected Overrides Sub OnDragEnter(e As DragEventArgs)
            MyBase.OnDragEnter(e)

            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

                If Not files.NothingOrEmpty Then
                    e.Effect = DragDropEffects.Copy
                End If
            End If
        End Sub

        Protected Overrides Sub OnDragDrop(args As DragEventArgs)
            MyBase.OnDragDrop(args)

            If FileDrop Then
                Dim files = TryCast(args.Data.GetData(DataFormats.FileDrop), String())

                If Not files.NothingOrEmpty Then
                    RaiseEvent FilesDropped(files)
                End If
            End If
        End Sub

        Sub SetMinimumSize(w As Integer, h As Integer)
            MinimumSize = New Size(CInt(Font.Height * w), CInt(Font.Height * h))
        End Sub

        Protected Overrides Sub OnLoad(args As EventArgs)
            KeyPreview = True
            SetTabIndexes(Me)

            If s.UIScaleFactor <> 1 Then
                Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
                Scale(New SizeF(1 * s.UIScaleFactor, 1 * s.UIScaleFactor))
            End If

            If DefaultWidthScale <> 0 Then
                Dim defaultWidth = CInt(Font.Height * DefaultWidthScale)
                Dim defaultHeight = CInt(Font.Height * DefaultHeightScale)

                Dim w = s.Storage.GetInt(Me.GetType().Name + "width")
                Dim h = s.Storage.GetInt(Me.GetType().Name + "height")

                Dim workingArea = Screen.FromControl(Me).WorkingArea

                If w = 0 OrElse w < (defaultWidth / 2) OrElse h = 0 OrElse h < (defaultHeight / 2) Then
                    w = defaultWidth
                    h = defaultHeight
                End If

                If w > workingArea.Width OrElse h > workingArea.Height Then
                    w = workingArea.Width
                    h = workingArea.Height
                End If

                Width = w
                Height = h
            End If

            If StartPosition = FormStartPosition.CenterScreen Then
                WindowPositions.CenterScreen(Me)
            End If

            If Not DesignHelp.IsDesignMode Then
                s.WindowPositions?.RestorePosition(Me)
            End If

            MyBase.OnLoad(args)
        End Sub

        Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
            MyBase.OnFormClosing(args)

            If Not s.WindowPositions Is Nothing Then
                s.WindowPositions.Save(Me)
            End If

            If DefaultWidthScale <> 0 Then
                SaveClientSize()
            End If
        End Sub

        Sub SetTabIndexes(c As Control)
            Dim index = 0

            Dim controls = From i In c.Controls.OfType(Of Control)()
                           Order By Math.Sqrt(i.Top * i.Top + i.Left * i.Left)

            For Each i In controls
                i.TabIndex = index
                index += 1
                SetTabIndexes(i)
            Next
        End Sub

        Sub RestoreClientSize(defaultWidthScale As Single, defaultHeightScale As Single)
            Me.DefaultWidthScale = defaultWidthScale
            Me.DefaultHeightScale = defaultHeightScale
        End Sub

        Sub SaveClientSize()
            s.Storage.SetInt(Me.GetType().Name + "width", Width)
            s.Storage.SetInt(Me.GetType().Name + "height", Height)
        End Sub
    End Class

    Public Class DialogBase
        Inherits FormBase

        Sub New()
            FormBorderStyle = FormBorderStyle.FixedDialog
            HelpButton = True
            MaximizeBox = False
            MinimizeBox = False
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterParent
        End Sub

        Protected Overrides Sub OnHelpButtonClicked(args As CancelEventArgs)
            MyBase.OnHelpButtonClicked(args)
            args.Cancel = True
            OnHelpRequested(New HelpEventArgs(MousePosition))
        End Sub
    End Class

    Public Class ListBag(Of T)
        Implements IComparable(Of ListBag(Of T))

        Property Text As String
        Property Value As T

        Sub New(text As String, value As T)
            Me.Text = text
            Me.Value = value
        End Sub

        Shared Sub SelectItem(cb As ComboBox, value As T)
            Dim selectItem As Object = Nothing

            For Each i As ListBag(Of T) In cb.Items
                If i.Value.Equals(value) Then selectItem = i
            Next

            If Not selectItem Is Nothing Then cb.SelectedItem = selectItem
        End Sub

        Shared Function GetValue(cb As ComboBox) As T
            Return DirectCast(DirectCast(cb.SelectedItem, ListBag(Of T)).Value, T)
        End Function

        Shared Function GetBagsForEnumType() As ListBag(Of T)()
            Dim ret As New List(Of ListBag(Of T))

            For Each i As T In System.Enum.GetValues(GetType(T))
                ret.Add(New ListBag(Of T)(DispNameAttribute.GetValueForEnum(i), i))
            Next

            Return ret.ToArray
        End Function

        Overrides Function ToString() As String
            Return Text
        End Function

        Function CompareTo(other As ListBag(Of T)) As Integer Implements IComparable(Of ListBag(Of T)).CompareTo
            Return Text.CompareTo(other.Text)
        End Function
    End Class

    <Serializable()>
    Public Class WindowPositions
        Public Positions As New Dictionary(Of String, Point)
        Private WindowStates As New Dictionary(Of String, FormWindowState)

        Sub Save(form As Form)
            SavePosition(form)
            SaveWindowState(form)
        End Sub

        Sub SavePosition(form As Form)
            If form.WindowState = FormWindowState.Normal Then
                Positions(GetKey(form)) = form.Location
            End If
        End Sub

        Sub SaveWindowState(form As Form)
            WindowStates(GetKey(form)) = form.WindowState
        End Sub

        Sub RestorePositionInternal(form As Form)
            If Positions.ContainsKey(GetKey(form)) Then
                Dim pos = Positions(GetKey(form))
                Dim wa = Screen.FromControl(form).WorkingArea

                If pos.X < 0 OrElse pos.Y < 0 OrElse
                    pos.X + form.Width > wa.Width OrElse
                    pos.Y + form.Height > wa.Height Then

                    CenterScreen(form)
                Else
                    form.StartPosition = FormStartPosition.Manual
                    form.Location = pos
                End If
            End If
        End Sub

        Shared Sub CenterScreen(form As Form)
            form.StartPosition = FormStartPosition.Manual
            Dim wa = Screen.FromControl(form).WorkingArea
            form.Left = (wa.Width - form.Width) \ 2
            form.Top = (wa.Height - form.Height) \ 2
        End Sub

        Sub RestorePosition(form As Form)
            Dim text = GetText(form)

            If Not s.WindowPositionsRemembered.NothingOrEmpty AndAlso Not TypeOf form Is InputBoxForm Then
                For Each i In s.WindowPositionsRemembered
                    If text.StartsWith(i) OrElse i = "all" Then
                        RestorePositionInternal(form)
                        Exit For
                    End If
                Next
            End If
        End Sub

        Function GetKey(form As Form) As String
            Return form.Name + form.GetType().FullName + GetText(form)
        End Function

        Function GetText(form As Form) As String
            If TypeOf form Is HelpForm Then
                Return "Help"
            ElseIf TypeOf form Is PreviewForm Then
                Return "Preview"
            ElseIf TypeOf form Is MainForm Then
                Return "StaxRip"
            End If

            Return form.Text
        End Function
    End Class

    Public Class OpenFileDialogEditor
        Inherits UITypeEditor

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Using f As New OpenFileDialog
                If f.ShowDialog = DialogResult.OK Then
                    Return f.FileName
                Else
                    Return value
                End If
            End Using
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Public Class StringEditor
        Inherits UITypeEditor

        Sub New()
        End Sub

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim form As New StringEditorForm
            form.rtb.Text = DirectCast(value, String)

            If form.ShowDialog() = DialogResult.OK Then
                Return form.rtb.Text
            Else
                Return value
            End If
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Public Class DesignHelp
        Private Shared IsDesignModeValue As Boolean?

        Shared ReadOnly Property IsDesignMode As Boolean
            Get
                If Not IsDesignModeValue.HasValue Then
                    IsDesignModeValue = Process.GetCurrentProcess.ProcessName = "devenv"
                End If

                Return IsDesignModeValue.Value
            End Get
        End Property
    End Class
End Namespace
