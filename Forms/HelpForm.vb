Imports StaxRip.UI

Public Class HelpForm
    Inherits FormBase

#Region " Designer "
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents Browser As System.Windows.Forms.WebBrowser
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Browser = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'Browser
        '
        Me.Browser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Browser.Location = New System.Drawing.Point(0, 0)
        Me.Browser.Name = "Browser"
        Me.Browser.Size = New System.Drawing.Size(1218, 791)
        Me.Browser.TabIndex = 0
        '
        'HelpForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1218, 791)
        Me.Controls.Add(Me.Browser)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "HelpForm"
        Me.ResumeLayout(False)

    End Sub
#End Region

    Sub New()
        InitializeComponent()
        Icon = My.Resources.MainIcon
    End Sub

    Sub New(path As String)
        Me.New()

        Try
            Browser.Navigate(path)
        Catch ex As Exception
            MsgWarn("Failed to load: " + path)
        End Try

        Icon = My.Resources.MainIcon
    End Sub

    Private DocumentValue As HelpDocument

    Property Doc() As HelpDocument
        Get
            If DocumentValue Is Nothing Then
                Dim path = Folder.Temp + Guid.NewGuid.ToString + ".htm"
                AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(path)
                DocumentValue = New HelpDocument(path)
            End If

            Return DocumentValue
        End Get
        Set(Value As HelpDocument)
            DocumentValue = Value
        End Set
    End Property

    Overloads Shared Sub ShowDialog(heading As String, tips As StringPairList)
        ShowDialog(heading, tips, Nothing)
    End Sub

    Overloads Shared Sub ShowDialog(heading As String, tips As StringPairList, summary As String)
        Dim f As New HelpForm()
        f.Doc.WriteStart(heading)
        If Not summary Is Nothing Then f.Doc.WriteP(summary)
        f.Doc.WriteTips(tips)
        f.Show()
    End Sub

    Private Sub HelpForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Dispose()
    End Sub

    Private Sub Browser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles Browser.DocumentCompleted
        WebBrowserHelp.ResetTextSize(Browser)
    End Sub

    Private Sub Browser_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles Browser.Navigated
        If Browser.DocumentTitle <> "" Then
            Text = Browser.DocumentTitle
        ElseIf File.Exists(e.Url.LocalPath) Then
            Text = FilePath.GetBase(e.Url.LocalPath)
        End If
    End Sub

    Shadows Sub Show()
        MyBase.Show()
        Application.DoEvents()
        If Not DocumentValue Is Nothing Then DocumentValue.WriteDocument(Browser)
    End Sub

    Private Sub Browser_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles Browser.Navigating
        If e.Url.AbsoluteUri.StartsWith("http") Then
            e.Cancel = True
            g.StartProcess(e.Url.ToString)
        End If
    End Sub
End Class