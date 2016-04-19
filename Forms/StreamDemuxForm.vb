Imports System.Globalization
Imports StaxRip.UI

Class StreamDemuxForm
    Property AudioStreams As List(Of AudioStream)
    Property Subtitles As List(Of Subtitle)

    Sub New(sourceFile As String)
        InitializeComponent()

        StartPosition = FormStartPosition.CenterParent

        lvAudio.View = View.Details
        lvAudio.Columns.Add("")
        lvAudio.CheckBoxes = True
        lvAudio.HeaderStyle = ColumnHeaderStyle.None
        lvAudio.ShowItemToolTips = True
        lvAudio.FullRowSelect = True
        lvAudio.MultiSelect = False
        lvAudio.SendMessageHideFocus()
        lvAudio.AutoCheckMode = AutoCheckMode.SingleClick

        lvSubtitles.View = View.SmallIcon
        lvSubtitles.CheckBoxes = True
        lvSubtitles.HeaderStyle = ColumnHeaderStyle.None
        lvSubtitles.AutoCheckMode = AutoCheckMode.SingleClick

        AudioStreams = MediaInfo.GetAudioStreams(sourceFile)
        Subtitles = MediaInfo.GetSubtitles(sourceFile)

        gbAudio.Enabled = AudioStreams.Count > 0
        gbSubtitles.Enabled = Subtitles.Count > 0

        bnAudioEnglish.Enabled = AudioStreams.Where(Function(stream) stream.Language.TwoLetterCode = "en").Count > 0
        bnAudioNative.Visible = CultureInfo.CurrentCulture.TwoLetterISOLanguageName <> "en"
        bnAudioNative.Text = CultureInfo.CurrentCulture.NeutralCulture.EnglishName
        bnAudioNative.Enabled = AudioStreams.Where(Function(stream) stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Count > 0

        bnSubtitleEnglish.Enabled = Subtitles.Where(Function(stream) stream.Language.TwoLetterCode = "en").Count > 0
        bnSubtitleNative.Visible = CultureInfo.CurrentCulture.TwoLetterISOLanguageName <> "en"
        bnSubtitleNative.Text = CultureInfo.CurrentCulture.NeutralCulture.EnglishName
        bnSubtitleNative.Enabled = Subtitles.Where(Function(stream) stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Count > 0

        For Each i In AudioStreams
            i.Enabled = False

            Dim item = lvAudio.Items.Add(i.Name)
            item.Tag = i

            If i.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName OrElse
                i.Language.TwoLetterCode = "en" OrElse i.Language.TwoLetterCode = "iv" Then

                i.Enabled = p.DemuxAudio
                item.Checked = p.DemuxAudio
            End If
        Next

        For Each i In Subtitles
            Dim text = i.Language.ToString
            If Subtitles.Count <= 12 Then text += " (" + i.TypeName + ")"
            Dim item = lvSubtitles.Items.Add(text)
            item.Tag = i
            item.Checked = i.Enabled AndAlso p.DemuxSubtitles
        Next
    End Sub

    Private Sub StreamDemuxForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        lvAudio.Columns(0).Width = lvAudio.ClientSize.Width
    End Sub

    Private Sub lvAudio_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvAudio.ItemChecked
        If Visible Then DirectCast(e.Item.Tag, AudioStream).Enabled = e.Item.Checked
    End Sub

    Private Sub lvSubtitles_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvSubtitles.ItemChecked
        If Visible Then DirectCast(e.Item.Tag, Subtitle).Enabled = e.Item.Checked
    End Sub

    Private Sub bnAudioAll_Click(sender As Object, e As EventArgs) Handles bnAudioAll.Click
        For Each i As ListViewItem In lvAudio.Items
            i.Checked = True
        Next
    End Sub

    Private Sub bnAudioNone_Click(sender As Object, e As EventArgs) Handles bnAudioNone.Click
        For Each i As ListViewItem In lvAudio.Items
            i.Checked = False
        Next
    End Sub

    Private Sub bnAudioEnglish_Click(sender As Object, e As EventArgs) Handles bnAudioEnglish.Click
        For Each i As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(i.Tag, AudioStream)

            If stream.Language.TwoLetterCode = "en" Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnAudioNative_Click(sender As Object, e As EventArgs) Handles bnAudioNative.Click
        For Each i As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(i.Tag, AudioStream)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnSubtitleAll_Click(sender As Object, e As EventArgs) Handles bnSubtitleAll.Click
        For Each i As ListViewItem In lvSubtitles.Items
            i.Checked = True
        Next
    End Sub

    Private Sub bnSubtitleNone_Click(sender As Object, e As EventArgs) Handles bnSubtitleNone.Click
        For Each i As ListViewItem In lvSubtitles.Items
            i.Checked = False
        Next
    End Sub

    Private Sub bnSubtitleEnglish_Click(sender As Object, e As EventArgs) Handles bnSubtitleEnglish.Click
        For Each i As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(i.Tag, Subtitle)

            If stream.Language.TwoLetterCode = "en" Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnSubtitleNative_Click(sender As Object, e As EventArgs) Handles bnSubtitleNative.Click
        For Each i As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(i.Tag, Subtitle)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                i.Checked = True
            End If
        Next
    End Sub
End Class