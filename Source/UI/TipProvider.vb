
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    <ProvideProperty("TipText", GetType(Control))>
    Public Class TipProvider
        Inherits Component
        Implements IExtenderProvider

        Private ToolTip As New ToolTip
        Private TipTitles As New Dictionary(Of Control, String)
        Private TipTexts As New Dictionary(Of Control, String)

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property TipsFunc As Func(Of StringPairList)

        Sub New(Optional component As IContainer = Nothing)
            If Not component Is Nothing Then
                component.Add(Me)
            End If

            ToolTip.AutomaticDelay = 1000
            ToolTip.AutoPopDelay = 10000
            ToolTip.InitialDelay = 1000
            ToolTip.ReshowDelay = 1000
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            ToolTip.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Function CanExtend(obj As Object) As Boolean Implements IExtenderProvider.CanExtend
            Return TypeOf obj Is Control
        End Function

        <Category("TipProvider")>
        <DefaultValue("")>
        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        Function GetTipText(ctrl As Control) As String
            If TipTexts.ContainsKey(ctrl) Then
                Return TipTexts(ctrl)
            End If

            Return ""
        End Function

        Sub SetTipText(c As Control, value As String)
            TipTexts(c) = value
            Init(value, c)
        End Sub

        Sub SetTip(tipText As String, tipTitle As String, c As Control)
            TipTitles(c) = tipTitle
            SetTipText(c, tipText)
        End Sub

        Sub SetTip(tipText As String, ParamArray controls As Control())
            If tipText = "" Then
                Exit Sub
            End If

            Dim title = ""

            For Each ctrl In controls
                If TypeOf ctrl Is Label OrElse TypeOf ctrl Is CheckBox Then
                    title = FormatName(ctrl.Text)
                End If
            Next

            For Each ctrl In controls
                TipTexts(ctrl) = tipText

                If title <> "" Then
                    TipTitles(ctrl) = title
                End If

                Init(tipText, ctrl)
            Next
        End Sub

        Sub Init(tipText As String, control As Control)
            If Not DesignMode Then
                AddHandler control.MouseDown, AddressOf TipMouseDown
                tipText = tipText.TrimEnd("."c)

                If tipText.Length > 80 Then
                    If HasContextMenu(control) Then
                        tipText = Nothing
                    Else
                        tipText = "Right-click for help"
                    End If
                ElseIf HelpDocument.MustConvert(tipText) Then
                    tipText = HelpDocument.ConvertMarkup(tipText, True)
                End If

                If tipText <> "" Then
                    AddHandler control.HandleCreated, Sub() ToolTip.SetToolTip(control, tipText)
                End If
            End If
        End Sub

        Sub TipMouseDown(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Right AndAlso
                Not HasContextMenu(DirectCast(sender, Control)) Then

                ShowHelp(DirectCast(sender, Control))
            End If
        End Sub

        Sub ShowHelp(ctrl As Control)
            Dim tip = GetTip(ctrl)
            g.ShowHelp(tip.Name, tip.Value)
        End Sub

        Function GetTip(ctrl As Control) As StringPair
            Dim ret As New StringPair

            If TipTitles.ContainsKey(ctrl) Then
                ret.Name = FormatName(TipTitles(ctrl))
            End If

            If TipTexts.ContainsKey(ctrl) Then
                ret.Value = TipTexts(ctrl)
            End If
            Return ret
        End Function

        Function HasContextMenu(ctrl As Control) As Boolean
            If TypeOf ctrl Is TextBox Then Return True
            If TypeOf ctrl Is RichTextBox Then Return True
            If TypeOf ctrl Is NumericUpDown Then Return True

            If TypeOf ctrl Is ComboBox AndAlso
                DirectCast(ctrl, ComboBox).DropDownStyle = ComboBoxStyle.DropDown Then

                Return True
            End If

            If Not ctrl.ContextMenuStrip Is Nothing Then
                Return True
            End If
        End Function

        Function FormatName(value As String) As String
            If value.Contains(" ") Then
                value = value.Trim
            End If

            If value.Contains("&") AndAlso Not value.Contains(" & ") Then
                value = value.Replace("&", "")
            End If

            If value.EndsWith("...") Then
                value = value.TrimEnd("."c)
            End If

            If value.EndsWith(":") Then
                value = value.TrimEnd(":"c)
            End If

            Return value
        End Function

        Function GetTips() As StringPairList
            Dim ret As New StringPairList
            Dim temp As New List(Of String)

            For Each ctrl In TipTexts.Keys
                If Not ctrl.IsDisposed AndAlso ctrl.Visible Then
                    Dim pair As New StringPair
                    pair.Value = TipTexts(ctrl)

                    If TipTitles.ContainsKey(ctrl) Then
                        pair.Name = FormatName(TipTitles(ctrl))
                    Else
                        pair.Name = FormatName(ctrl.Text)
                    End If

                    If Not temp.Contains(pair.Name) Then
                        temp.Add(pair.Name)
                        pair.Name = FormatName(pair.Name)
                        ret.Add(pair)
                    End If
                End If
            Next

            If Not TipsFunc Is Nothing Then
                ret.AddRange(TipsFunc.Invoke)
            End If

            ret.Sort()

            Return ret
        End Function
    End Class
End Namespace
