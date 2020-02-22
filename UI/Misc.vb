Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    Public Class FormBase
        Inherits Form

        Event FilesDropped(files As String())

        Private FileDropValue As Boolean

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property FontHeight As Integer

        Public Sub New()
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
            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
                If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
            End If

            MyBase.OnDragEnter(e)
        End Sub

        Protected Overrides Sub OnDragDrop(e As DragEventArgs)
            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
                If Not files.NothingOrEmpty Then RaiseEvent FilesDropped(files)
            End If

            MyBase.OnDragDrop(e)
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            KeyPreview = True
            SetTabIndexes(Me)

            If AutoScaleMode = AutoScaleMode.Dpi OrElse AutoScaleMode = AutoScaleMode.Font Then
                If s.UIScaleFactor <> 1 Then
                    Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
                    Scale(New SizeF(1 * s.UIScaleFactor, 1 * s.UIScaleFactor))
                End If
            Else
                Dim designDimension = 144
                If s.UIScaleFactor <> 1 Then Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

                If designDimension <> DeviceDpi OrElse s.UIScaleFactor <> 1 Then
                    Scale(New SizeF(CSng(DeviceDpi / designDimension * s.UIScaleFactor),
                                    CSng(DeviceDpi / designDimension * s.UIScaleFactor)))
                End If
            End If

            MyBase.OnLoad(e)

            If Not DesignHelp.IsDesignMode Then
                s.WindowPositions?.RestorePosition(Me)
            End If
        End Sub

        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            If Not s.WindowPositions Is Nothing Then
                s.WindowPositions.Save(Me)
            End If

            MyBase.OnFormClosing(e)
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

        Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
            e.Cancel = True
            MyBase.OnHelpButtonClicked(e)
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

        Sub Save(f As Form)
            SavePosition(f)
            SaveWindowState(f)
        End Sub

        Private Sub SavePosition(f As Form)
            If f.WindowState = FormWindowState.Normal Then Positions(GetKey(f)) = f.Location
        End Sub

        Private Sub SaveWindowState(f As Form)
            WindowStates(GetKey(f)) = f.WindowState
        End Sub

        Private Sub RestorePositionInternal(form As Form)
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

        Private Sub RestoreWindowState(f As Form)
            If WindowStates.ContainsKey(GetKey(f)) Then f.WindowState = WindowStates(GetKey(f))
        End Sub

        Shared Sub CenterScreen(form As Form)
            form.StartPosition = FormStartPosition.Manual
            Dim wa = Screen.FromControl(form).WorkingArea
            form.Left = (wa.Width - form.Width) \ 2
            form.Top = (wa.Height - form.Height) \ 2
        End Sub

        Sub RestorePosition(form As Form)
            Dim v = GetText(form)

            If Not s.WindowPositionsCenterScreen.NothingOrEmpty AndAlso Not TypeOf form Is InputBoxForm Then
                For Each i In s.WindowPositionsCenterScreen
                    If v.StartsWith(i) OrElse i = "all" Then
                        CenterScreen(form)
                        Exit For
                    End If
                Next
            End If

            If Not s.WindowPositionsRemembered.NothingOrEmpty AndAlso Not TypeOf form Is InputBoxForm Then
                For Each i In s.WindowPositionsRemembered
                    If v.StartsWith(i) OrElse i = "all" Then
                        RestorePositionInternal(form)
                        Exit For
                    End If
                Next
            End If
        End Sub

        Function GetKey(f As Form) As String
            Return f.Name + f.GetType().FullName + GetText(f)
        End Function

        Function GetText(f As Form) As String
            If TypeOf f Is HelpForm Then Return "Help"
            If TypeOf f Is PreviewForm Then Return "Preview"
            Return f.Text
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
                If Not IsDesignModeValue.HasValue Then IsDesignModeValue = Process.GetCurrentProcess.ProcessName = "devenv"
                Return IsDesignModeValue.Value
            End Get
        End Property
    End Class
End Namespace