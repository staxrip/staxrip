Imports System.Reflection
Imports System.ComponentModel
Imports System.Text
Imports System.Drawing.Design
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Namespace UI
    Class FormBase
        Inherits Form

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ScaleFactor As SizeF = New SizeF(1, 1)

        Public Sub New()
            AutoScaleMode = AutoScaleMode.None
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            KeyPreview = True
            SetTabIndexes(Me)

            If AutoScaleDimensions.IsEmpty Then AutoScaleDimensions = New SizeF(144.0!, 144.0!)

            If AutoScaleDimensions <> CurrentDPIDimension Then
                ScaleFactor = New SizeF(CurrentDPIDimension.Width / AutoScaleDimensions.Width,
                                       CurrentDPIDimension.Height / AutoScaleDimensions.Height)

                AutoScaleDimensions = CurrentDPIDimension
                Scale(ScaleFactor)
            End If

            If Not DesignMode AndAlso Not s.WindowPositions Is Nothing Then
                s.WindowPositions.RestorePosition(Me)
            End If

            MyBase.OnLoad(e)
        End Sub

        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            If Not s.WindowPositions Is Nothing Then s.WindowPositions.Save(Me)
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

        Private CurrentDPIDimensionValue As SizeF?

        ReadOnly Property CurrentDPIDimension As SizeF
            Get
                If Not CurrentDPIDimensionValue.HasValue Then
                    Using g = CreateGraphics()
                        CurrentDPIDimensionValue = New SizeF(g.DpiX, g.DpiY)
                    End Using
                End If

                Return CurrentDPIDimensionValue.Value
            End Get
        End Property

        Protected Overrides Sub WndProc(ByRef m As Message)
            Snap(m)
            MyBase.WndProc(m)
        End Sub

        Private IsResizing As Boolean

        Sub Snap(ByRef m As Message)
            Const WM_SIZING = &H214, WM_EXITSIZEMOVE = &H232, WM_WINDOWPOSCHANGING = &H46

            Select Case m.Msg
                Case WM_SIZING
                    IsResizing = True
                Case WM_EXITSIZEMOVE
                    IsResizing = False
                Case WM_WINDOWPOSCHANGING
                    If Not IsResizing Then Snap(m.LParam)
            End Select
        End Sub

        Sub Snap(handle As IntPtr)
            Dim workingArea = Screen.FromControl(Me).WorkingArea
            Dim newPos = DirectCast(Marshal.PtrToStructure(handle, GetType(WindowPos)), WindowPos)
            Dim snapMargin = CInt(Control.DefaultFont.Height * 1.5)
            Dim border As Integer

            If OSVersion.Current >= OSVersion.Windows8 Then
                border = (Width - ClientSize.Width) \ 2 - 1
            End If

            If Math.Abs(newPos.Y - workingArea.Y) < snapMargin Then
                newPos.Y = workingArea.Y
            ElseIf Math.Abs(newPos.Y + Height - (workingArea.Bottom + border)) < snapMargin Then
                newPos.Y = (workingArea.Bottom + border) - Height
            End If

            If Math.Abs(newPos.X - (workingArea.X - border)) < snapMargin Then
                newPos.X = workingArea.X - border
            ElseIf Math.Abs(newPos.X + Width - (workingArea.Right + border)) < snapMargin Then
                newPos.X = (workingArea.Right + border) - Width
            End If

            Marshal.StructureToPtr(newPos, handle, True)
        End Sub

        <StructLayout(LayoutKind.Sequential)>
        Structure WindowPos
            Public Hwnd As IntPtr
            Public HwndInsertAfter As IntPtr
            Public X As Integer
            Public Y As Integer
            Public Width As Integer
            Public Height As Integer
            Public Flags As Integer
        End Structure
    End Class

    Class DialogBase
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

    Class ListBag(Of T)
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
    Class WindowPositions
        Private Positions As New Dictionary(Of String, Point)
        Private WindowStates As New Dictionary(Of String, FormWindowState)

        Sub Save(f As Form)
            SavePosition(f)
            SaveWindowState(f)
        End Sub

        Private Sub SavePosition(f As Form)
            If f.WindowState = FormWindowState.Normal Then
                Positions(GetKey(f)) = f.Location
            End If
        End Sub

        Private Sub SaveWindowState(f As Form)
            WindowStates(GetKey(f)) = f.WindowState
        End Sub

        Private Sub RestorePositionInternal(f As Form)
            f.StartPosition = FormStartPosition.Manual

            If Positions.ContainsKey(GetKey(f)) Then
                Dim p = Positions(GetKey(f))
                Dim workingArea = Screen.FromControl(f).WorkingArea

                If workingArea.Contains(p) AndAlso workingArea.Contains(p.X + f.Width, p.Y + f.Height) Then
                    f.Location = p
                Else
                    f.StartPosition = FormStartPosition.CenterParent
                End If
            Else
                f.StartPosition = FormStartPosition.CenterParent
            End If
        End Sub

        Private Sub RestoreWindowState(f As Form)
            If WindowStates.ContainsKey(GetKey(f)) Then
                f.WindowState = WindowStates(GetKey(f))
            End If
        End Sub

        Sub RestorePosition(f As Form)
            Dim v = GetText(f)

            If Not s.WindowPositionsCenterScreen.ContainsNothingOrEmpty AndAlso
                Not TypeOf f Is InputBoxForm Then

                For Each i In s.WindowPositionsCenterScreen
                    If v.StartsWith(i) OrElse i = "all" Then
                        f.StartPosition = FormStartPosition.Manual
                        Dim b = Screen.FromControl(f).WorkingArea
                        f.Left = (b.Width - f.Width) \ 2
                        f.Top = (b.Height - f.Height) \ 2
                        Exit For
                    End If
                Next
            End If

            If Not s.WindowPositionsRemembered.ContainsNothingOrEmpty AndAlso
                Not TypeOf f Is InputBoxForm Then

                For Each i In s.WindowPositionsRemembered
                    If v.StartsWith(i) OrElse i = "all" Then
                        RestorePositionInternal(f)
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Function GetKey(f As Form) As String
            Return f.Name + f.GetType().FullName + GetText(f)
        End Function

        Function GetText(f As Form) As String
            Return If(TypeOf f Is HelpForm, "Help", f.Text)
        End Function
    End Class

    Class OpenFileDialogEditor
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

    Class StringEditor
        Inherits UITypeEditor

        Sub New()
        End Sub

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim f As New StringEditorForm
            f.tb.Text = DirectCast(value, String)

            If f.ShowDialog() = DialogResult.OK Then
                Return f.tb.Text
            Else
                Return value
            End If
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class
End Namespace