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
    Public Class FormBase
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

            If Math.Abs(newPos.Y - workingArea.Y) < snapMargin Then
                newPos.Y = workingArea.Y
            ElseIf Math.Abs(newPos.Y + Height - workingArea.Bottom) < snapMargin Then
                newPos.Y = workingArea.Bottom - Height
            End If

            If Math.Abs(newPos.X - workingArea.X) < snapMargin Then
                newPos.X = workingArea.X
            ElseIf Math.Abs(newPos.X + Width - workingArea.Right) < snapMargin Then
                newPos.X = workingArea.Right - Width
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
        Sub New(text As String, value As T)
            Me.Text = text
            Me.Value = value
        End Sub

        Private TextValue As String

        Property Text() As String
            Get
                Return TextValue
            End Get
            Set(Value As String)
                TextValue = Value
            End Set
        End Property

        Private ValueValue As T

        Property Value() As T
            Get
                Return ValueValue
            End Get
            Set(Value As T)
                ValueValue = Value
            End Set
        End Property

        Shared Sub SelectItem(cb As ComboBox, value As T)
            Dim selectItem As Object = Nothing

            For Each i As ListBag(Of T) In cb.Items
                If i.Value.Equals(value) Then
                    selectItem = i
                End If
            Next

            If Not selectItem Is Nothing Then
                cb.SelectedItem = selectItem
            End If
        End Sub

        Shared Function GetValue(cb As ComboBox) As T
            Return DirectCast(DirectCast(cb.SelectedItem, ListBag(Of T)).Value, T)
        End Function

        Shared Function GetBagsForEnumType() As ListBag(Of T)()
            Dim l As New List(Of ListBag(Of T))

            For Each i As T In System.Enum.GetValues(GetType(T))
                l.Add(New ListBag(Of T)(DispNameAttribute.GetValueForEnum(i), i))
            Next

            Return l.ToArray
        End Function

        Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    <Serializable()>
    Public Class WindowPositions
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

            If OK(s.WindowPositionsCenterScreen) AndAlso Not TypeOf f Is InputBoxForm Then
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

            If OK(s.WindowPositionsRemembered) AndAlso Not TypeOf f Is InputBoxForm Then
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

    ''' <summary>
    ''' Specifies ellipsis format and alignment.
    ''' </summary>
    <Flags>
    Public Enum EllipsisFormat
        ''' <summary>
        ''' Text is not modified.
        ''' </summary>
        None = 0
        ''' <summary>
        ''' Text is trimmed at the end of the string. An ellipsis (...) is drawn in place of remaining text.
        ''' </summary>
        [End] = 1
        ''' <summary>
        ''' Text is trimmed at the begining of the string. An ellipsis (...) is drawn in place of remaining text. 
        ''' </summary>
        Start = 2
        ''' <summary>
        ''' Text is trimmed in the middle of the string. An ellipsis (...) is drawn in place of remaining text.
        ''' </summary>
        Middle = 3
        ''' <summary>
        ''' Preserve as much as possible of the drive and filename information. Must be combined with alignment information.
        ''' </summary>
        Path = 4
        ''' <summary>
        ''' Text is trimmed at a word boundary. Must be combined with alignment information.
        ''' </summary>
        Word = 8
    End Enum

    Public Class Ellipsis
        ''' <summary>
        ''' String used as a place holder for trimmed text.
        ''' </summary>
        Public Shared ReadOnly EllipsisChars As String = "..."

        Private Shared prevWord As New Regex("\W*\w*$")
        Private Shared nextWord As New Regex("\w*\W*")

        ''' <summary>
        ''' Truncates a text string to fit within a given control width by replacing trimmed text with ellipses. 
        ''' </summary>
        ''' <param name="text">String to be trimmed.</param>
        ''' <param name="ctrl">text must fit within ctrl width.
        '''	The ctrl's Font is used to measure the text string.</param>
        ''' <param name="options">Format and alignment of ellipsis.</param>
        ''' <returns>This function returns text trimmed to the specified witdh.</returns>
        Public Shared Function Compact(text As String, ctrl As Control, options As EllipsisFormat) As String
            If String.IsNullOrEmpty(text) Then
                Return text
            End If

            ' no aligment information
            If (EllipsisFormat.Middle And options) = 0 Then
                Return text
            End If

            If ctrl Is Nothing Then
                Throw New ArgumentNullException("ctrl")
            End If

            Using dc As Graphics = ctrl.CreateGraphics()
                Dim s As Size = TextRenderer.MeasureText(dc, text, ctrl.Font)

                ' control is large enough to display the whole text
                If s.Width <= ctrl.Width Then
                    Return text
                End If

                Dim pre As String = ""
                Dim mid As String = text
                Dim post As String = ""

                Dim isPath As Boolean = (EllipsisFormat.Path And options) <> 0

                ' split path string into <drive><directory><filename>
                If isPath Then
                    pre = Path.GetPathRoot(text)
                    mid = Path.GetDirectoryName(text).Substring(pre.Length)
                    post = Path.GetFileName(text)
                End If

                Dim len As Integer = 0
                Dim seg As Integer = mid.Length
                Dim fit As String = ""

                ' find the longest string that fits into 
                ' the control boundaries using bisection method
                While seg > 1
                    seg -= seg \ 2

                    Dim left As Integer = len + seg
                    Dim right As Integer = mid.Length

                    If left > right Then
                        Continue While
                    End If

                    If (EllipsisFormat.Middle And options) = EllipsisFormat.Middle Then
                        right -= left \ 2
                        left -= left \ 2
                    ElseIf (EllipsisFormat.Start And options) <> 0 Then
                        right -= left
                        left = 0
                    End If

                    ' trim at a word boundary using regular expressions
                    If (EllipsisFormat.Word And options) <> 0 Then
                        If (EllipsisFormat.[End] And options) <> 0 Then
                            left -= prevWord.Match(mid, 0, left).Length
                        End If
                        If (EllipsisFormat.Start And options) <> 0 Then
                            right += nextWord.Match(mid, right).Length
                        End If
                    End If

                    ' build and measure a candidate string with ellipsis
                    Dim tst As String = mid.Substring(0, left) & EllipsisChars & mid.Substring(right)

                    ' restore path with <drive> and <filename>
                    If isPath Then
                        tst = Path.Combine(Path.Combine(pre, tst), post)
                    End If
                    s = TextRenderer.MeasureText(dc, tst, ctrl.Font)

                    ' candidate string fits into control boundaries, try a longer string
                    ' stop when seg <= 1
                    If s.Width <= ctrl.Width Then
                        len += seg
                        fit = tst
                    End If
                End While

                If len = 0 Then
                    ' string can't fit into control
                    ' "path" mode is off, just return ellipsis characters
                    If Not isPath Then
                        Return EllipsisChars
                    End If

                    ' <drive> and <directory> are empty, return <filename>
                    If pre.Length = 0 AndAlso mid.Length = 0 Then
                        Return post
                    End If

                    ' measure "C:\...\filename.ext"
                    fit = Path.Combine(Path.Combine(pre, EllipsisChars), post)

                    s = TextRenderer.MeasureText(dc, fit, ctrl.Font)

                    ' if still not fit then return "...\filename.ext"
                    If s.Width > ctrl.Width Then
                        fit = Path.Combine(EllipsisChars, post)
                    End If
                End If

                Return fit
            End Using
        End Function
    End Class

End Namespace