
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions

Imports StaxRip.UI

Public Module ShortcutModule
    Public g As New GlobalClass
    Public p As New Project
    Public s As New ApplicationSettings
End Module

<Serializable()>
Public Class Range
    Implements IComparable(Of Range)

    Public Start As Integer
    Public [End] As Integer

    Sub New(startPosition As Integer, endPosition As Integer)
        Me.Start = startPosition
        Me.End = endPosition
    End Sub

    Function GetLenght() As Integer
        Return Me.End - Start
    End Function

    Function CompareTo(other As Range) As Integer Implements System.IComparable(Of Range).CompareTo
        Return Start.CompareTo(other.Start)
    End Function
End Class

Public Class Calc
    Shared Function IsValidFrameRate(value As Double) As Boolean
        Return Not (Double.IsNaN(value) OrElse Double.IsInfinity(value) OrElse
            value < 1 OrElse value > 500)
    End Function

    Shared Function GetYFromTwoPointForm(x1 As Single, y1 As Single, x2 As Single, y2 As Single, x As Single) As Integer
        Return CInt((((y2 - y1) / (x2 - x1)) * (x - x1)) + y1)
    End Function

    Shared Function GetPercent() As Double
        If p.Compressibility = 0 Then
            Return 0
        End If

        Return (GetBPF() / p.Compressibility) * 100
    End Function

    Shared Function GetBPF() As Double
        Dim framerate = p.Script.GetFramerate

        If framerate = 0 Then Return 0
        If p.TargetWidth = 0 Then Return 0
        If p.TargetHeight = 0 Then Return 0

        Return p.VideoBitrate * 1024L / (p.TargetWidth * p.TargetHeight * CLng(framerate))
    End Function

    Shared Function GetSize() As Double
        Dim ret = (Calc.GetVideoKBytes() + Calc.GetAudioKBytes() +
            GetSubtitleKBytes() + Calc.GetOverheadKBytes()) / 1024

        If ret < 1 Then ret = 1

        Return ret
    End Function

    Shared Function GetVideoBitrate() As Double
        If p.TargetSeconds = 0 Then
            Return 0
        End If

        Dim kbytes = p.TargetSize * 1024 - GetAudioKBytes() - GetSubtitleKBytes() - GetOverheadKBytes()
        Dim ret = kbytes * 8 * 1.024 / p.TargetSeconds

        If ret < 1 Then
            ret = 1
        End If

        Return ret
    End Function

    Shared Function GetVideoKBytes() As Double
        Return ((p.VideoBitrate * p.TargetSeconds) / 8) / 1.024
    End Function

    Shared Function GetSubtitleKBytes() As Double
        Return Aggregate i In p.VideoEncoder.Muxer.Subtitles Into Sum(If(i.Enabled, i.Size / 1024 / 3, 0))
    End Function

    Shared Function GetOverheadKBytes() As Double
        Dim ret As Double
        Dim frames = p.Script.GetFrameCount

        If {"avi", "divx"}.Contains(p.VideoEncoder.Muxer.OutputExt) Then
            ret += frames * 0.024
            If p.Audio0.File <> "" Then ret += frames * 0.04
            If p.Audio1.File <> "" Then ret += frames * 0.04
        ElseIf p.VideoEncoder.Muxer.OutputExt = "mp4" Then
            ret += 10.4 / 1024 * frames
        ElseIf p.VideoEncoder.Muxer.OutputExt = "mkv" Then
            ret += frames * 0.013
        End If

        Return ret
    End Function

    Shared Function GetAudioKBytes() As Double
        Return ((Calc.GetAudioBitrate() * p.TargetSeconds) / 8) / 1.024
    End Function

    Shared Function GetAudioBitrate() As Double
        Dim b0, b1 As Double

        If p.Audio0.File <> "" Then b0 = p.Audio0.Bitrate
        If p.Audio1.File <> "" Then b1 = p.Audio1.Bitrate

        Return b0 + b1 + p.AudioTracks.Sum(Function(arg) arg.Bitrate)
    End Function

    Shared Function GetBitrateFromFile(path As String, seconds As Integer) As Double
        Try
            If path = "" OrElse seconds = 0 Then Return 0
            Dim kBits = New FileInfo(path).Length * 8 / 1000
            Return kBits / seconds
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Function

    Shared Function IsARSignalingRequired() As Boolean
        If Not p.Script Is Nothing Then
            Dim par = GetTargetPAR()

            If par.X <> par.Y Then
                If p.Script.IsFilterActive("Resize") Then
                    Return Math.Abs(GetAspectRatioError()) > p.MaxAspectRatioError
                Else
                    Return True
                End If
            End If
        End If
    End Function

    Private Shared Function GetSimplePar(par As Point) As Point
        If par.Y > 0 Then
            For Each i In New Point() {New Point(12, 11), New Point(10, 11), New Point(16, 11), New Point(40, 33)}
                If Math.Abs((par.X / par.Y) / (i.X / i.Y) * 100 - 100) < 1 Then Return i
            Next
        End If

        If par.X > 255 OrElse par.Y > 255 Then
            Dim x = par.X / 255
            Dim y = par.Y / 255

            If x > y Then
                par.X = CInt(par.X / x)
                par.Y = CInt(par.Y / x)
            Else
                par.X = CInt(par.X / y)
                par.Y = CInt(par.Y / y)
            End If
        End If

        If par.X = par.Y Then
            par.X = 1
            par.Y = 1
        End If

        Return par
    End Function

    Shared Function ParseCustomAR(value As String, defaultX As Integer, defaultY As Integer) As Point
        If value <> "" AndAlso (value.Contains(":") OrElse value.Contains("/")) Then
            Dim a = value.Split(":/".ToCharArray)

            If a.Length = 2 AndAlso a(0).IsInt AndAlso a(1).IsInt Then
                Return Reduce(New Point(a(0).ToInt, a(1).ToInt))
            End If
        ElseIf value.IsDouble Then
            Dim val = value.ToDouble

            If val > 0 Then
                Return Reduce(New Point(CInt(val * 1000000), 1000000))
            End If
        End If

        Return New Point(defaultX, defaultY)
    End Function

    Shared Function GetSourceDAR() As Double
        Try
            Dim par = GetSourcePAR()
            Return (par.X * p.SourceWidth) / (par.Y * p.SourceHeight)
        Catch ex As Exception
            Return 4 / 3
        End Try
    End Function

    Shared Function GetSimpleSourcePAR() As Point
        Return GetSimplePar(GetSourcePAR)
    End Function

    Shared Function GetSourcePAR() As Point
        If p.CustomSourcePAR <> "" Then
            Dim val = ParseCustomAR(p.CustomSourcePAR, 0, 0)

            If val.X <> 0 Then
                Return Reduce(val)
            End If
        End If

        If p.CustomSourceDAR <> "" Then
            Dim val = ParseCustomAR(p.CustomSourceDAR, 0, 0)

            If val.X <> 0 Then
                Return Reduce(New Point(p.SourceHeight * val.X, p.SourceWidth * val.Y))
            End If
        End If

        Dim par As New Point(1, 1)
        Dim w = p.SourceWidth, h = p.SourceHeight

        If (h = 576 OrElse h = 480) AndAlso w <= 768 Then
            Dim f As VideoFormat

            For Each i In Formats
                If i.Width = p.SourceWidth AndAlso i.Height = p.SourceHeight Then
                    f = i
                    Exit For
                End If
            Next

            If f.Width > 0 Then
                Dim samplingWidth = 52.0

                If Not p.ITU Then
                    samplingWidth = f.Width / f.SamplingRate
                End If

                Dim dar = (p.SourcePAR.X * p.SourceWidth) / (p.SourcePAR.Y * p.SourceHeight)
                par.X = CInt(If(p.SourceAnamorphic OrElse dar > 1.7, 16 / 9, 4 / 3) * f.Height)
                par.Y = CInt(f.SamplingRate * samplingWidth)
                Return Reduce(par)
            Else
                Dim dar = If(p.SourceAnamorphic, 16 / 9, 4 / 3)
                If p.ITU Then dar *= 1.0255
                par.X = CInt(dar * h)
                par.Y = CInt(w)
                Return Reduce(par)
            End If
        End If

        If h = 720 OrElse h = 1080 OrElse h = 1088 Then
            If p.SourcePAR.X = 1364 Then
                p.SourcePAR.X = 4
                p.SourcePAR.Y = 3
            End If
        End If

        Return Reduce(p.SourcePAR)
    End Function

    Shared Function GetTargetPAR() As Point
        Try
            If p.CustomTargetPAR <> "" Then
                Dim val = ParseCustomAR(p.CustomTargetPAR, 0, 0)

                If val.X <> 0 Then
                    Return Reduce(val)
                End If
            End If

            Dim par = GetSourcePAR()
            Dim croppedWidth = p.SourceWidth
            Dim croppedHeight = p.SourceHeight

            If p.Script.IsFilterActive("Crop") Then
                croppedWidth -= p.CropLeft + p.CropRight
                croppedHeight -= p.CropTop + p.CropBottom
            End If

            If p.CustomTargetDAR <> "" Then
                Dim val = ParseCustomAR(p.CustomTargetDAR, 0, 0)

                If val.X <> 0 Then
                    Return Reduce(New Point(CInt(val.X * croppedHeight), CInt(val.Y * croppedWidth)))
                End If
            End If

            If p.TargetWidth <> croppedWidth OrElse p.TargetHeight <> croppedHeight Then
                Dim par2 = Reduce(New Point(croppedWidth * p.TargetHeight, croppedHeight * p.TargetWidth))
                par.X = par.X * par2.X
                par.Y = par.Y * par2.Y
                par = Reduce(par)
            End If

            Return GetSimplePar(par)
        Catch ex As Exception
            Return New Point(1, 1)
        End Try
    End Function

    Shared Function GetTargetDAR() As Double
        If p.CustomTargetDAR <> "" Then
            Dim val = ParseCustomAR(p.CustomTargetDAR, 0, 0)

            If val.X <> 0 AndAlso val.Y <> 0 Then
                Return val.X / val.Y
            End If
        End If

        Dim w = p.SourceWidth, h = p.SourceHeight
        Dim cropw = w, croph = h

        If p.Script.IsFilterActive("Crop") Then
            cropw = w - p.CropLeft - p.CropRight
            croph = h - p.CropTop - p.CropBottom
        End If

        If p.CustomTargetPAR <> "" Then
            Dim val = ParseCustomAR(p.CustomTargetPAR, 0, 0)
            If val.X <> 0 Then Return (val.X * cropw) / (val.Y * croph)
        End If

        Return ((cropw / w) / (croph / h)) * GetSourceDAR()
    End Function

    Shared Function GetAspectRatioError() As Double
        Return ((p.TargetWidth / p.TargetHeight) / Calc.GetTargetDAR) * 100 - 100
    End Function

    Shared Function Reduce(p As Point) As Point
        If p.X <> 0 AndAlso p.Y <> 0 Then
            Dim gcd = GetGCD(p.X, p.Y)
            p.X \= gcd
            p.Y \= gcd
        End If

        While p.X > 1000000
            p.X = CInt(p.X / 10)
            p.Y = CInt(p.Y / 10)
        End While

        Return p
    End Function

    Shared Function GetGCD(a As Integer, b As Integer) As Integer
        If b = 0 Then
            Return a
        Else
            Return GetGCD(b, a Mod b)
        End If
    End Function

    Shared Function FixMod16(value As Integer) As Integer
        Return CInt(value / 16) * 16
    End Function

    Shared Function FixMod(value As Integer, modValue As Integer) As Integer
        Return CInt(value / modValue) * modValue
    End Function

    Shared Function GetMod(
        w As Integer,
        h As Integer,
        Optional skip16 As Boolean = True) As String

        Dim wmod, hmod As Integer

        For Each x In {1, 2, 4, 8, 16}
            If w Mod x = 0 Then wmod = x
            If h Mod x = 0 Then hmod = x
        Next

        If wmod = 16 AndAlso hmod = 16 Then
            If skip16 Then
                Return ""
            Else
                Return "16/16"
            End If
        Else
            Dim x = w - FixMod16(w)
            Dim xval As String

            If x = 8 OrElse x = -8 Then
                xval = "8"
            ElseIf x > 0 Then
                xval = "+" & x
            Else
                xval = x.ToString
            End If

            Dim y = h - FixMod16(h)
            Dim yval As String

            If y = 8 OrElse y = -8 Then
                yval = "8"
            ElseIf y > 0 Then
                yval = "+" & y
            Else
                yval = y.ToString
            End If

            Return wmod & "/" & hmod & " (" & xval & "/" & yval & ")"
        End If
    End Function

    Shared Function GetNextMod(val As Integer, step1 As Integer) As Integer
        Do
            val += 1
        Loop Until val Mod step1 = 0

        Return val
    End Function

    Shared Function GetPreviousMod(val As Integer, step1 As Integer) As Integer
        Do
            val -= 1
        Loop Until val Mod step1 = 0

        Return val
    End Function

    Private Shared FormatsValue As VideoFormat()

    Shared ReadOnly Property Formats() As VideoFormat()
        Get
            If FormatsValue Is Nothing Then
                FormatsValue = {
                    New VideoFormat(768, 576, 14.75),
                    New VideoFormat(768, 560, 14.75),
                    New VideoFormat(720, 576, 13.5),
                    New VideoFormat(704, 576, 13.5),
                    New VideoFormat(702, 576, 13.5),
                    New VideoFormat(544, 576, 10.125),
                    New VideoFormat(480, 576, 9.0),
                    New VideoFormat(384, 288, 7.375),
                    New VideoFormat(384, 280, 7.375),
                    New VideoFormat(352, 576, 6.75),
                    New VideoFormat(352, 288, 6.75),
                    New VideoFormat(176, 144, 3.375),
                    New VideoFormat(720, 486, 13.5),
                    New VideoFormat(720, 480, 13.5),
                    New VideoFormat(711, 486, 13.5),
                    New VideoFormat(704, 486, 13.5),
                    New VideoFormat(704, 480, 13.5),
                    New VideoFormat(640, 480, 12.27272),
                    New VideoFormat(480, 480, 9.0),
                    New VideoFormat(352, 480, 6.75),
                    New VideoFormat(352, 240, 6.75),
                    New VideoFormat(320, 240, 6.13636)
                }
            End If

            Return FormatsValue
        End Get
    End Property
End Class

Public Structure VideoFormat
    Sub New(width As Integer, height As Integer, samplingRate As Double)
        Me.Width = width
        Me.Height = height
        Me.SamplingRate = samplingRate
    End Sub

    Public Width As Integer
    Public Height As Integer
    Public SamplingRate As Double
End Structure

<Serializable()>
Public Class Language
    Implements IComparable(Of Language)

    <NonSerialized>
    Public IsCommon As Boolean

    Sub New()
        Me.New("")
    End Sub

    Sub New(ci As CultureInfo, Optional isCommon As Boolean = False)
        Me.IsCommon = isCommon
        CultureInfoValue = ci
    End Sub

    Sub New(twoLetterCode As String, Optional isCommon As Boolean = False)
        Try
            Me.IsCommon = isCommon

            Select Case twoLetterCode
                Case "iw"
                    twoLetterCode = "he"
                Case "jp"
                    twoLetterCode = "ja"
            End Select

            CultureInfoValue = New CultureInfo(twoLetterCode)
        Catch ex As Exception
            CultureInfoValue = CultureInfo.InvariantCulture
        End Try
    End Sub

    Private CultureInfoValue As CultureInfo

    ReadOnly Property CultureInfo() As CultureInfo
        Get
            Return CultureInfoValue
        End Get
    End Property

    ReadOnly Property TwoLetterCode() As String
        Get
            Return CultureInfo.TwoLetterISOLanguageName
        End Get
    End Property

    <NonSerialized()> Private ThreeLetterCodeValue As String

    ReadOnly Property ThreeLetterCode() As String
        Get
            If ThreeLetterCodeValue Is Nothing Then
                If CultureInfo.TwoLetterISOLanguageName = "iv" Then
                    ThreeLetterCodeValue = "und"
                Else
                    Select Case CultureInfo.ThreeLetterISOLanguageName
                        Case "deu"
                            ThreeLetterCodeValue = "ger"
                        Case "ces"
                            ThreeLetterCodeValue = "cze"
                        Case "zho"
                            ThreeLetterCodeValue = "chi"
                        Case "nld"
                            ThreeLetterCodeValue = "dut"
                        Case "ell"
                            ThreeLetterCodeValue = "gre"
                        Case "fra"
                            ThreeLetterCodeValue = "fre"
                        Case "sqi"
                            ThreeLetterCodeValue = "alb"
                        Case "hye"
                            ThreeLetterCodeValue = "arm"
                        Case "eus"
                            ThreeLetterCodeValue = "baq"
                        Case "mya"
                            ThreeLetterCodeValue = "bur"
                        Case "kat"
                            ThreeLetterCodeValue = "geo"
                        Case "isl"
                            ThreeLetterCodeValue = "ice"
                        Case "bng"
                            ThreeLetterCodeValue = "ben"
                        Case Else
                            ThreeLetterCodeValue = CultureInfo.ThreeLetterISOLanguageName
                    End Select
                End If
            End If

            Return ThreeLetterCodeValue
        End Get
    End Property

    ReadOnly Property Name() As String
        Get
            If CultureInfo.TwoLetterISOLanguageName = "iv" Then
                Return "Undetermined"
            Else
                Return CultureInfo.EnglishName
            End If
        End Get
    End Property

    Private Shared LanguagesValue As List(Of Language)

    Shared ReadOnly Property Languages() As List(Of Language)
        Get
            If LanguagesValue Is Nothing Then
                Dim l As New List(Of Language)

                l.Add(New Language("en", True))
                l.Add(New Language("es", True))
                l.Add(New Language("de", True))
                l.Add(New Language("fr", True))
                l.Add(New Language("it", True))
                l.Add(New Language("ru", True))
                l.Add(New Language("zh", True))
                l.Add(New Language("hi", True))
                l.Add(New Language("ja", True))
                l.Add(New Language("pt", True))
                l.Add(New Language("ar", True))
                l.Add(New Language("bn", True))
                l.Add(New Language("pa", True))
                l.Add(New Language("ms", True))
                l.Add(New Language("ko", True))

                l.Add(New Language(CultureInfo.InvariantCulture, True))

                Dim current = l.Where(Function(a) a.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).FirstOrDefault

                If current Is Nothing Then
                    l.Add(CurrentCulture)
                End If

                l.Sort()

                Dim l2 As New List(Of Language)

                For Each i In CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                    l2.Add(New Language(i))
                Next

                l2.Sort()
                l.AddRange(l2)
                LanguagesValue = l
            End If

            Return LanguagesValue
        End Get
    End Property

    Shared ReadOnly Property CurrentCulture As Language
        Get
            Return New Language(CultureInfo.CurrentCulture.NeutralCulture, True)
        End Get
    End Property


    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Language) As Integer Implements System.IComparable(Of Language).CompareTo
        Return Name.CompareTo(other.Name)
    End Function

    Overrides Function Equals(o As Object) As Boolean
        If TypeOf o Is Language Then
            Return CultureInfo.Equals(DirectCast(o, Language).CultureInfo)
        End If
    End Function
End Class

Public Class CommandLineTypeEditor
    Inherits UITypeEditor

    Overloads Overrides Function EditValue(context As ITypeDescriptorContext,
                                           provider As IServiceProvider,
                                           value As Object) As Object
        Using f As New MacroEditorDialog
            f.SetBatchDefaults()
            f.MacroEditorControl.Value = CStr(value)

            If f.ShowDialog = DialogResult.OK Then
                Return f.MacroEditorControl.Value
            Else
                Return value
            End If
        End Using
    End Function

    Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function
End Class

Public Class ScriptTypeEditor
    Inherits UITypeEditor

    Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Using f As New MacroEditorDialog
            f.SetScriptDefaults()
            f.MacroEditorControl.Value = CStr(value)

            If f.ShowDialog = DialogResult.OK Then
                Return f.MacroEditorControl.Value
            Else
                Return value
            End If
        End Using
    End Function

    Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function
End Class

Public Class MacroStringTypeEditor
    Inherits UITypeEditor

    Overloads Overrides Function EditValue(context As ITypeDescriptorContext,
                                           provider As IServiceProvider,
                                           value As Object) As Object
        Using f As New MacroEditorDialog
            f.SetMacroDefaults()
            f.MacroEditorControl.Value = CStr(value)

            If f.ShowDialog = DialogResult.OK Then
                Return f.MacroEditorControl.Value
            Else
                Return value
            End If
        End Using
    End Function

    Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function
End Class

<Serializable()>
Public MustInherit Class Profile
    Implements IComparable(Of Profile)

    Sub New()
    End Sub

    Sub New(name As String)
        Me.Name = name
    End Sub

    Private NameValue As String

    Overridable Property Name() As String
        Get
            If NameValue = "" Then
                Return DefaultName
            Else
                If NameValue = DefaultName Then
                    NameValue = Nothing
                    Return DefaultName
                End If
            End If

            Return NameValue
        End Get
        Set(Value As String)
            If Value = DefaultName Then
                NameValue = Nothing
            Else
                NameValue = Value
            End If
        End Set
    End Property

    Overridable ReadOnly Property DefaultName As String
        Get
            Return "untitled"
        End Get
    End Property

    Protected CanEditValue As Boolean

    Overridable ReadOnly Property CanEdit() As Boolean
        Get
            Return CanEditValue
        End Get
    End Property

    Overridable Function Edit() As DialogResult
    End Function

    Overridable Function CreateEditControl() As Control
        Return Nothing
    End Function

    Overridable Sub Clean()
    End Sub

    Overridable Function GetCopy() As Profile
        Return DirectCast(ObjectHelp.GetCopy(Me), Profile)
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Profile) As Integer Implements System.IComparable(Of Profile).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

<Serializable()>
Public Class ObjectStorage
    Private StringDictionary As New Dictionary(Of String, String)
    Private IntDictionary As New Dictionary(Of String, Integer)
    Private DoubleDictionary As New Dictionary(Of String, Double)
    Private BoolDictionaryValue As Dictionary(Of String, Boolean)

    ReadOnly Property BoolDictionary() As Dictionary(Of String, Boolean)
        Get
            If BoolDictionaryValue Is Nothing Then
                BoolDictionaryValue = New Dictionary(Of String, Boolean)
            End If

            Return BoolDictionaryValue
        End Get
    End Property

    Function GetBool(key As String) As Boolean
        Return GetBool(key, False)
    End Function

    Function GetBool(key As String, defaultValue As Boolean) As Boolean
        If BoolDictionary.ContainsKey(key) Then
            Return BoolDictionary(key)
        End If

        Return defaultValue
    End Function

    Sub SetBool(key As String, Value As Boolean)
        BoolDictionary(key) = Value
    End Sub

    Function GetInt(key As String) As Integer
        Return GetInt(key, 0)
    End Function

    Function GetInt(key As String, defaultValue As Integer) As Integer
        If IntDictionary.ContainsKey(key) Then
            Return IntDictionary(key)
        End If

        Return defaultValue
    End Function

    Sub SetInt(key As String, value As Integer)
        IntDictionary(key) = value
    End Sub

    Function GetDouble(key As String) As Double
        Return GetDouble(key, 0.0)
    End Function

    Function GetDouble(key As String, defaultValue As Double) As Double
        If DoubleDictionary.ContainsKey(key) Then
            Return DoubleDictionary(key)
        End If

        Return defaultValue
    End Function

    Sub SetDouble(key As String, value As Double)
        DoubleDictionary(key) = value
    End Sub

    Function GetString(key As String, Optional defaultValue As String = Nothing) As String
        If StringDictionary.ContainsKey(key) Then
            Return StringDictionary(key)
        End If

        Return defaultValue
    End Function

    Sub SetString(key As String, value As String)
        If value Is Nothing Then
            If StringDictionary.ContainsKey(key) Then
                StringDictionary.Remove(key)
            End If
        Else
            StringDictionary(key) = value
        End If
    End Sub
End Class

Public Enum CompCheckAction
    [Nothing]
    <DispName("image size")> AdjustImageSize
    <DispName("file size")> AdjustFileSize
End Enum

<Serializable()>
Public Class EventCommand
    Property Name As String = "???"
    Property Enabled As Boolean = True
    Property CriteriaList As New List(Of Criteria)
    Property OrOnly As Boolean
    Property CommandParameters As CommandParameters
    Property [Event] As ApplicationEvent

    Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Enum DynamicMenuItemID
    Audio1Profiles
    Audio2Profiles
    EncoderProfiles
    FilterSetupProfiles
    MuxerProfiles
    RecentProjects
    TemplateProjects
    HelpApplications
    Scripts
End Enum

Public Class Startup
    <STAThread()>
    Shared Sub Main()
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf g.OnUnhandledException
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim args = Environment.GetCommandLineArgs

        If args.Count > 2 AndAlso args(1) = "--create-soft-links" Then
            Try
                SoftLink.CreateLinksElevated(args.Skip(2))
            Catch ex As Exception
                MsgError(ex.Message)
                Environment.ExitCode = 1
            End Try

            Exit Sub
        End If

        Application.Run(New MainForm())
    End Sub
End Class

'obsolete since 2017 
<Serializable()>
Public Class Dummy
End Class

Public Class KeyValueList(Of T1, T2)
    Inherits List(Of KeyValuePair(Of T1, T2))

    Overloads Sub Add(key As T1, value As T2)
        Add(New KeyValuePair(Of T1, T2)(key, value))
    End Sub
End Class

Public Class GUIDS
    Shared Property LAVSplitter As String = "{171252A0-8820-4AFE-9DF8-5C92B2D66B04}"
    Shared Property LAVVideoDecoder As String = "{EE30215D-164F-4A92-A4EB-9D4C13390F9F}"
    Shared Property HaaliMuxer As String = "{A28F324B-DDC5-4999-AA25-D3A7E25EF7A8}"
End Class

Public Class M2TSStream
    Property Text As String = "Nothing"
    Property Codec As String = ""
    Property OutputType As String = ""
    Property Options As String = ""
    Property ID As Integer
    Property IsVideo As Boolean
    Property IsAudio As Boolean
    Property IsSubtitle As Boolean
    Property IsChapters As Boolean
    Property Language As New Language
    Property Checked As Boolean
    Property ListViewItem As ListViewItem

    Sub UpdateListViewItem()
        ListViewItem.Text = ToString()
    End Sub

    Public Overrides Function ToString() As String
        Dim ret = Text

        If ret.Contains("TrueHD/AC3") Then ret = ret.Replace("TrueHD/AC3", "THD+AC3")
        If ret.Contains("DTS Master Audio") Then ret = ret.Replace("DTS Master Audio", "DTS-MA")
        If ret.Contains("DTS Hi-Res") Then ret = ret.Replace("DTS Hi-Res", "DTS-HRA")
        If ret.Contains("DTS Express") Then ret = ret.Replace("DTS Express", "DTS-EX")

        If IsAudio Then
            ret += "  ->  " + OutputType

            If Options <> "" Then
                ret += ": " + Options
            End If
        End If

        Return ret
    End Function
End Class

<Serializable>
Public Class AudioStream
    Property BitDepth As Integer
    Property Bitrate As Integer
    Property Bitrate2 As Integer
    Property Channels As Integer
    Property Channels2 As Integer
    Property Delay As Integer
    Property Format As String
    Property FormatString As String
    Property FormatProfile As String
    Property ID As Integer
    Property StreamOrder As Integer
    Property Index As Integer
    Property Language As Language
    Property SamplingRate As Integer
    Property Title As String
    Property Enabled As Boolean = True
    Property [Default] As Boolean
    Property Forced As Boolean
    Property Lossy As Boolean

    ReadOnly Property Name As String
        Get
            Dim ret = "#" & Index + 1

            If FormatProfile.EqualsAny(
                "TrueHD+Atmos / TrueHD",
                "E-AC-3+Atmos / E-AC-3",
                "TrueHD+Atmos / TrueHD / AC-3") Then

                ret += " Atmos"
            ElseIf FormatString = "TrueHD / AC3" Then
                ret += " TrueHD"
            ElseIf FormatString = "MPEG-1 Audio layer 2" Then
                ret += " MP2"
            ElseIf FormatString = "MPEG-1 Audio layer 3" Then
                ret += " MP3"
            ElseIf FormatString = "MPEG Audio" Then
                If FormatProfile = "Layer 2" Then ret += " MP2"
                If FormatProfile = "Layer 3" Then ret += " MP3"
            ElseIf FormatString = "AC3+" OrElse Format = "E-AC-3" Then
                ret += " EAC3"
            ElseIf Format = "MLP FBA" Then
                ret += " TrueHD"
            ElseIf FormatString = "DTS XLL" OrElse FormatProfile.StartsWith("MA /") Then
                ret += " DTSMA"
            ElseIf FormatString = "DTS XLL X" Then
                ret += " DTSX"
            ElseIf FormatProfile.StartsWith("HRA /") Then
                ret += " DTSHRA"
            ElseIf FormatString = "AC-3" Then
                ret += " AC3"
            Else
                ret += " " + FormatString
            End If

            If Not ret.Contains("Atmos") Then
                If Channels <> Channels2 AndAlso Channels > 0 AndAlso Channels2 > 0 Then
                    ret += " " & Channels & "/" & Channels2 & "ch"
                ElseIf Channels > 0 Then
                    ret += " " & Channels & "ch"
                ElseIf Channels2 > 0 Then
                    ret += " " & Channels2 & "ch"
                End If
            End If

            If BitDepth > 0 AndAlso Not Lossy Then ret += " " & BitDepth & "Bit"

            If SamplingRate > 0 Then
                If SamplingRate Mod 1000 = 0 Then
                    ret += " " & SamplingRate / 1000 & "kHz"
                Else
                    ret += " " & SamplingRate & "Hz"
                End If
            End If

            If Bitrate2 > 0 Then
                ret += " " & If(Bitrate = 0, "?", Bitrate.ToString) & "/" & Bitrate2 & "Kbps"
            ElseIf Bitrate > 0 Then
                ret += " " & Bitrate & "Kbps"
            End If

            If Delay <> 0 Then
                ret += " " & Delay & "ms"
            End If

            If Language.TwoLetterCode <> "iv" Then
                ret += " " + Language.Name
            End If

            If Title <> "" AndAlso Title <> " " Then
                ret += " " + Title
            End If

            Return ret
        End Get
    End Property

    ReadOnly Property ExtFull() As String
        Get
            Return "." + Ext
        End Get
    End Property

    ReadOnly Property Ext() As String
        Get
            Select Case FormatString
                Case "AAC LC", "AAC LC-SBR", "AAC LC-SBR-PS", "AAC LC SBR"
                    Return "m4a"
                Case "AC3", "AC-3"
                    Return "ac3"
                Case "DTS"
                    Return "dts"
                Case "DTS-HD", "DTS XLL", "DTS XLL X", "DTS XBR", "DTS ES XLL"
                    Return "dtshd"
                Case "PCM", "ADPCM"
                    Return "wav"
                Case "MPEG-1 Audio layer 2"
                    Return "mp2"
                Case "MPEG-1 Audio layer 3"
                    Return "mp3"
                Case "MPEG Audio"
                    If FormatProfile = "Layer 2" Then Return "mp2"
                    If FormatProfile = "Layer 3" Then Return "mp3"
                Case "TrueHD / AC3"
                    Return "thd"
                Case "FLAC"
                    Return "flac"
                Case "Vorbis"
                    Return "ogg"
                Case "Opus"
                    Return "opus"
                Case "TrueHD", "Atmos / TrueHD", "MLP FBA 16-ch"
                    Return "thd"
                Case "AC3+", "E-AC-3"
                    Return "eac3"
                Case Else
                    Select Case Format
                        Case "MLP FBA"
                            Return "thd"
                        Case "E-AC-3"
                            Return "eac3"
                        Case Else
                            Return "mka"
                    End Select
            End Select
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class VideoStream
    Property Format As String
    Property StreamOrder As Integer
    Property ID As Integer
    Property Index As Integer

    ReadOnly Property Ext() As String
        Get
            Select Case Format
                Case "MPEG Video"
                    Return "mpg"
                Case "AVC"
                    Return "h264"
                Case "MPEG-4 Visual", "JPEG"
                    Return "avi"
                Case "HEVC"
                    Return "h265"
                Case "AV1", "VP8", "VP9"
                    Return "ivf"
                Case Else
                    Return ""
            End Select
        End Get
    End Property

    ReadOnly Property ExtFull() As String
        Get
            Return "." + Ext
        End Get
    End Property
End Class

<Serializable()>
Public Class Subtitle
    Property Title As String = ""
    Property Path As String
    Property CodecString As String
    Property Format As String
    Property ID As Integer
    Property StreamOrder As Integer
    Property Index As Integer
    Property IndexIDX As Integer
    Property Language As Language
    Property [Default] As Boolean
    Property Forced As Boolean
    Property Enabled As Boolean = True
    Property Size As Long

    Sub New()
        Language = New Language
    End Sub

    Sub New(lang As Language)
        Language = lang
    End Sub

    ReadOnly Property Filename As String
        Get
            Dim ret = "ID" & (StreamOrder + 1)
            ret += " " + Language.Name

            If Title <> "" AndAlso Title <> " " AndAlso p.SourceFile <> "" Then
                ret += " {" + Title.Shorten(50).EscapeIllegalFileSysChars + "}"
            End If

            Return ret
        End Get
    End Property

    ReadOnly Property ExtFull As String
        Get
            Return "." + Ext
        End Get
    End Property

    ReadOnly Property Ext As String
        Get
            Select Case CodecString
                Case "VobSub"
                    Return "idx"
                Case "S_HDMV/PGS", "PGS"
                    Return "sup"
                Case "S_TEXT/ASS", "ASS"
                    Return "ass"
                Case "S_TEXT/UTF8", "UTF-8", "Timed", "Timed Text"
                    Return "srt"
                Case "S_TEXT/SSA", "SSA"
                    Return "ssa"
                Case "S_TEXT/USF", "USF"
                    Return "usf"
                Case Else
                    Return Path.Ext
            End Select
        End Get
    End Property

    ReadOnly Property TypeName As String
        Get
            Dim ret = ExtFull
            If ret = "" Then ret = Path.ExtFull
            Return ret.TrimStart("."c).ToUpper.Replace("SUP", "PGS").Replace("IDX", "VobSub")
        End Get
    End Property

    Shared Function Create(path As String) As List(Of Subtitle)
        Dim ret As New List(Of Subtitle)

        If New FileInfo(path).Length = 0 Then
            Return ret
        End If

        If path.Ext = "idx" Then
            Dim indexData As Integer
            Dim st As Subtitle = Nothing

            For Each line In path.ReadAllText.SplitLinesNoEmpty
                If line.StartsWith("id: ") AndAlso line Like "id: ??, index: *" Then
                    st = New Subtitle

                    If path.Contains("forced") Then st.Forced = True

                    Try
                        st.Language = New Language(New CultureInfo(line.Substring(4, 2)))
                    Catch
                        st.Language = New Language(CultureInfo.InvariantCulture)
                    End Try

                    Dim autoCode = p.PreferredSubtitles.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    st.Enabled = autoCode.ContainsAny("all", st.Language.TwoLetterCode, st.Language.ThreeLetterCode)

                    If Not st Is Nothing Then
                        st.IndexIDX = CInt(Regex.Match(line, ", index: (\d+)").Groups(1).Value)
                    End If
                End If

                If Not st Is Nothing AndAlso line.StartsWith("timestamp: ") Then
                    st.StreamOrder = indexData
                    st.Path = path
                    indexData += 1
                    st.Size = CInt(New FileInfo(path).Length / 1024)
                    Dim subFile = path.ChangeExt("sub")
                    If File.Exists(subFile) Then st.Size += New FileInfo(subFile).Length
                    ret.Add(st)
                    st = Nothing
                End If
            Next
        ElseIf path.Ext.EqualsAny("mkv", "mp4", "m2ts", "webm") Then
            For Each i In MediaInfo.GetSubtitles(path)
                If i.Size = 0 Then
                    Select Case i.TypeName
                        Case "SRT"
                            i.Size = 10L * p.TargetSeconds
                        Case "VobSub"
                            i.Size = 1250L * p.TargetSeconds
                        Case "PGS"
                            i.Size = 5000L * p.TargetSeconds
                    End Select
                End If

                i.Path = path
                ret.Add(i)
            Next
        Else
            Dim st As New Subtitle()
            st.Size = New FileInfo(path).Length
            Dim match = Regex.Match(path, " ID(\d+)")

            If match.Success Then
                st.StreamOrder = match.Groups(1).Value.ToInt - 1
            End If

            For Each lng In Language.Languages
                If path.Contains(lng.CultureInfo.EnglishName) Then
                    st.Language = lng
                    Exit For
                End If
            Next

            If path.Contains("{") Then
                Dim title = path.Right("{")
                st.Title = title.Left("}").UnescapeIllegalFileSysChars
            End If

            Dim autoCode = p.PreferredSubtitles.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
            st.Enabled = autoCode.ContainsAny("all", st.Language.TwoLetterCode, st.Language.ThreeLetterCode)

            st.Path = path
            ret.Add(st)
        End If

        Dim enabledSubs = ret.Where(Function(val) val.Enabled)

        Select Case p.DefaultSubtitle
            Case DefaultSubtitleMode.Single
                If enabledSubs.Count = 1 Then
                    enabledSubs(0).Default = True
                End If
            Case DefaultSubtitleMode.First
                If enabledSubs.Count > 0 Then
                    enabledSubs(0).Default = True
                End If
            Case DefaultSubtitleMode.Second
                If enabledSubs.Count > 1 Then
                    enabledSubs(1).Default = True
                End If
        End Select

        For Each st In ret
            If p.SubtitleName <> "" Then
                st.Title = p.SubtitleName
            End If
        Next

        For Each st In ret
            If p.DefaultSubtitle = DefaultSubtitleMode.English Then
                If st.Language.TwoLetterCode = "en" Then
                    st.Default = True
                    Exit For
                End If
            ElseIf p.DefaultSubtitle = DefaultSubtitleMode.Native Then
                If st.Language.TwoLetterCode = Language.CurrentCulture.TwoLetterCode Then
                    st.Default = True
                    Exit For
                End If
            End If
        Next

        Return ret
    End Function

    Shared Sub Cut(subtitles As List(Of Subtitle))
        If p.Ranges.Count = 0 OrElse TypeOf p.VideoEncoder Is NullEncoder Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        Dim emptySubs As New List(Of Subtitle)

        For x = 0 To subtitles.Count - 1
            Dim inSub = subtitles(x)

            If Not inSub.Enabled OrElse Not File.Exists(inSub.Path) OrElse inSub.Path.Contains("_cut_") Then Continue For
            Dim aviPath = p.TempDir + inSub.Path.Base + "_cut_mm.avi"
            Dim d = (p.CutFrameCount / p.CutFrameRate).ToString("f9", CultureInfo.InvariantCulture)
            Dim r = p.CutFrameRate.ToString("f9", CultureInfo.InvariantCulture)
            Dim args = $"-f lavfi -i color=c=black:s=16x16:d={d}:r={r} -y -hide_banner -c:v copy " + aviPath.Escape

            Using proc As New Proc
                proc.Header = "Create avi file for subtitle cutting"
                proc.SkipStrings = {"frame=", "size="}
                proc.WriteLog("mkvmerge cannot cut subtitles without video so a avi file has to be created" + BR2)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.ffmpeg
                proc.Arguments = args
                proc.Start()
            End Using

            If Not File.Exists(aviPath) Then
                Throw New ErrorAbortException("Error", "Output file missing")
            Else
                Log.WriteLine(MediaInfo.GetSummary(aviPath))
            End If

            Dim id = If(FileTypes.SubtitleSingle.Contains(inSub.Path.Ext), 0, inSub.StreamOrder)
            Dim mkvPath = p.TempDir + inSub.Path.Base + " ID" & id & "_cut_sub.mkv"
            args = "-o " + mkvPath.Escape + " " + aviPath.Escape

            If Not FileTypes.SubtitleExludingContainers.Contains(inSub.Path.Ext) Then
                args += " --no-audio --no-video --no-chapters --no-attachments --no-track-tags --no-global-tags"
            End If

            If Not FileTypes.SubtitleSingle.Contains(inSub.Path.Ext) Then args += " --subtitle-tracks " & id
            args += " " + inSub.Path.Escape
            args += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & v.End).Join(",+")
            args += " --ui-language en"

            Using proc As New Proc
                proc.Header = "Cut subtitle"
                proc.SkipString = "Progress: "
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvmerge
                proc.Arguments = args
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using

            If Not File.Exists(mkvPath) Then
                Throw New ErrorAbortException("Error", "Output file missing")
            Else
                Log.WriteLine(MediaInfo.GetSummary(mkvPath))
            End If

            Dim subPath = p.TempDir + inSub.Path.Base + " ID" & id & "_cut_" + inSub.ExtFull
            args = "tracks " + mkvPath.Escape + " 1:" + subPath.Escape

            Using proc As New Proc
                proc.Header = "Demux subtitle"
                proc.SkipString = "Progress: "
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = args + " --ui-language en"
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using

            If Not File.Exists(subPath) Then
                Throw New ErrorAbortException("Error", "Output file missing")
            Else
                Log.WriteLine(MediaInfo.GetSummary(subPath))
            End If

            Dim subs = Subtitle.Create(subPath)

            If Not subs.NothingOrEmpty Then
                Dim outSub = Subtitle.Create(subPath)(0)
                outSub.Language = inSub.Language
                outSub.Forced = inSub.Forced
                outSub.Default = inSub.Default
                outSub.Title = inSub.Title
                outSub.Enabled = True
                subtitles(x) = outSub
            Else
                emptySubs.Add(subtitles(x))
            End If
        Next

        For Each i In emptySubs
            subtitles.Remove(i)
        Next
    End Sub
End Class

<Serializable>
Public Class PrimitiveStore
    Property Bool As New Dictionary(Of String, Boolean)
    Property Int As New Dictionary(Of String, Integer)
    Property [Double] As New Dictionary(Of String, Double)
    Property [String] As New Dictionary(Of String, String)
End Class

Public Enum ContainerStreamType
    Unknown
    Audio
    Video
    Subtitle
    Attachment
    Chapters
End Enum

Public Class FileTypes
    Shared Property AudioRaw As String() = {"thd", "aac", "ec3", "eac3"}
    Shared Property Audio As String() = {"flac", "dtshd", "dtsma", "dtshr", "thd", "thd+ac3", "true-hd", "truehd", "aac", "ac3", "dts", "ec3", "eac3", "m4a", "mka", "mp2", "mp3", "mpa", "opus", "wav", "w64"}
    Shared Property VideoAudio As String() = {"avi", "mp4", "mkv", "divx", "flv", "mov", "mpeg", "mpg", "ts", "m2ts", "vob", "webm", "wmv", "pva", "ogg", "ogm", "m4v", "3gp"}
    Shared Property DGDecNVInput As String() = {"264", "h264", "265", "h265", "avc", "hevc", "hvc", "mkv", "mp4", "m4v", "mpg", "vob", "ts", "m2ts", "mts", "m2t", "mpv", "m2v"}
    Shared Property eac3toInput As String() = {"dts", "dtshd", "dtshr", "dtsma", "evo", "vob", "ts", "m2ts", "wav", "w64", "pcm", "raw", "flac", "ac3", "ec3", "eac3", "thd", "thd+ac3", "mlp", "mp2", "mp3", "mpa"}
    Shared Property NicAudioInput As String() = {"wav", "mp2", "mpa", "mp3", "ac3", "dts"}
    Shared Property SubtitleExludingContainers As String() = {"srt", "ass", "idx", "sup", "ttxt", "ssa", "smi"}
    Shared Property SubtitleSingle As String() = {"srt", "ass", "sup", "ttxt", "ssa", "smi"}
    Shared Property SubtitleIncludingContainers As String() = {"m2ts", "mkv", "mp4", "m4v", "ass", "idx", "smi", "srt", "ssa", "sup", "ttxt"}
    Shared Property TextSub As String() = {"ass", "idx", "smi", "srt", "ssa", "ttxt", "usf", "ssf", "psb", "sub"}
    Shared Property Video As String() = {"264", "265", "avc", "avi", "avs", "d2v", "dgi", "dgim", "divx", "flv", "h264", "h265", "hevc", "hvc", "ivf", "m2t", "m2ts", "m2v", "mkv", "mov", "mp4", "m4v", "mpeg", "mpg", "mpv", "mts", "ogg", "ogm", "pva", "rmvb", "ts", "vdr", "vob", "vpy", "webm", "wmv", "y4m", "3gp"}
    Shared Property VideoIndex As String() = {"d2v", "dgi", "dga", "dgim"}
    Shared Property VideoOnly As String() = {"264", "265", "avc", "gif", "h264", "h265", "hevc", "hvc", "ivf", "m2v", "mpv", "apng", "png", "y4m"}
    Shared Property VideoRaw As String() = {"264", "265", "h264", "h265", "avc", "hevc", "hvc", "ivf"}
    Shared Property VideoText As String() = {"d2v", "dgi", "dga", "dgim", "avs", "vpy"}
    Shared Property VideoDemuxOutput As String() = {"mpg", "h264", "avi", "h265"}
    Shared Property Image As String() = {"bmp", "jpg", "png", "gif", "tif", "jpe", "jpeg", "psd"}

    Shared Function GetFilter(values As IEnumerable(Of String)) As String
        Return "*." + values.Join(";*.") + "|*." + values.Join(";*.") + "|All Files|*.*"
    End Function
End Class

Public Class OSVersion
    Shared Property Windows7 As Single = 6.1
    Shared Property Windows8 As Single = 6.2
    Shared Property Windows10 As Single = 10.0

    Shared ReadOnly Property Current As Single
        Get
            Return CSng(Environment.OSVersion.Version.Major + Environment.OSVersion.Version.Minor / 10)
        End Get
    End Property
End Class

Public Class OS
    Public Shared EnvVars As String() = {"ALLUSERSPROFILE", "APPDATA", "CD", "CMDCMDLINE",
        "CMDEXTVERSION", "CommonProgramFiles", "CommonProgramFiles(x86)", "CommonProgramW6432",
        "COMPUTERNAME", "COMSPEC", "DATE", "ERRORLEVEL", "HOMEDRIVE", "HOMEPATH", "LOCALAPPDATA",
        "LOGONSERVER", "NUMBER_OF_PROCESSORS", "OS", "PATH", "PATHEXT", "PROCESSOR_ARCHITECTURE",
        "PROCESSOR_IDENTIFIER", "PROCESSOR_LEVEL", "PROCESSOR_REVISION", "ProgramData",
        "ProgramFiles", "ProgramFiles(x86)", "ProgramW6432", "PROMPT", "PSModulePath", "PUBLIC",
        "RANDOM", "SessionName", "SystemDrive", "SystemRoot", "TEMP", "TIME", "TMP", "USERDOMAIN",
        "USERDOMAIN_ROAMINGPROFILE", "USERNAME", "USERPROFILE", "WINDIR"}

    Private Shared VideoControllersValue As String()

    Public Shared ReadOnly Property VideoControllers As String()
        Get
            If VideoControllersValue Is Nothing Then
                Try 'bug report received
                    Dim mc As New ManagementClass("Win32_VideoController")
                    VideoControllersValue = mc.GetInstances().OfType(Of ManagementBaseObject)().Select(Function(val) CStr(val("Caption"))).ToArray
                Catch ex As Exception
                    Return {"WMI Error"}
                End Try
            End If

            Return VideoControllersValue
        End Get
    End Property
End Class

<Serializable>
Public Class eac3toProfile
    Property Match As String
    Property Output As String
    Property Options As String

    Sub New()
    End Sub

    Sub New(match As String,
            output As String,
            options As String)

        Me.Match = match
        Me.Output = output
        Me.Options = options
    End Sub
End Class

Public Class BitmapUtil
    Property Data As Byte()
    Property BitmapData As BitmapData

    Function GetPixel(x As Integer, y As Integer) As Color
        Dim pos = y * BitmapData.Stride + x * 4
        Return Color.FromArgb(Data(pos), Data(pos + 1), Data(pos + 2))
    End Function

    Function GetMax(x As Integer, y As Integer) As Integer
        Dim col = GetPixel(x, y)
        Dim max = Math.Max(col.R, col.G)
        Return Math.Max(max, col.B)
    End Function

    Shared Function Create(bmp As Bitmap) As BitmapUtil
        Dim util As New BitmapUtil
        Dim rect As New Rectangle(0, 0, bmp.Width, bmp.Height)
        util.BitmapData = bmp.LockBits(rect, Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat)
        Dim ptr = util.BitmapData.Scan0
        Dim bytesCount = Math.Abs(util.BitmapData.Stride) * bmp.Height
        util.Data = New Byte(bytesCount - 1) {}
        Marshal.Copy(ptr, util.Data, 0, bytesCount)
        bmp.UnlockBits(util.BitmapData)
        Return util
    End Function

    Shared Function CreateBitmap(server As IFrameServer, position As Integer) As Bitmap
        Dim pitch As Integer
        Dim data As IntPtr

        If server.GetFrame(position, data, pitch) = 0 Then
            Return New Bitmap(server.Info.Width, server.Info.Height,
                pitch, PixelFormat.Format32bppArgb, data)
        End If
    End Function
End Class

Public Class AutoCrop
    Public Top As Integer()
    Public Bottom As Integer()
    Public Left As Integer()
    Public Right As Integer()

    Shared Function Start(bmp As Bitmap, position As Integer) As AutoCrop
        Dim ret As New AutoCrop
        Dim u = BitmapUtil.Create(bmp)
        Dim max = 20
        Dim xCount = 20
        Dim yCount = 20

        Dim xValues(xCount) As Integer

        For x = 0 To xCount
            xValues(x) = CInt(bmp.Width / (xCount + 1) * x)
        Next

        ret.Top = New Integer(xValues.Length - 1) {}
        ret.Bottom = New Integer(xValues.Length - 1) {}

        For xValue = 0 To xValues.Length - 1
            For y = 0 To u.BitmapData.Height \ 4
                If u.GetMax(xValues(xValue), y) < max Then
                    ret.Top(xValue) = y + 1
                Else
                    Exit For
                End If
            Next

            For y = u.BitmapData.Height - 1 To u.BitmapData.Height - u.BitmapData.Height \ 4 Step -1
                If u.GetMax(xValues(xValue), y) < max Then
                    ret.Bottom(xValue) = u.BitmapData.Height - y
                Else
                    Exit For
                End If
            Next
        Next

        Dim yValues(yCount) As Integer

        For x = 0 To yCount
            yValues(x) = CInt(bmp.Height / (yCount + 1) * x)
        Next

        ret.Left = New Integer(yValues.Length - 1) {}
        ret.Right = New Integer(yValues.Length - 1) {}

        For yValue = 0 To yValues.Length - 1
            For x = 0 To u.BitmapData.Width \ 4
                If u.GetMax(x, yValues(yValue)) < max Then
                    ret.Left(yValue) = x + 1
                Else
                    Exit For
                End If
            Next

            For x = u.BitmapData.Width - 1 To u.BitmapData.Width - u.BitmapData.Width \ 4 Step -1
                If u.GetMax(x, yValues(yValue)) < max Then
                    ret.Right(yValue) = u.BitmapData.Width - x
                Else
                    Exit For
                End If
            Next
        Next

        Return ret
    End Function
End Class

Public Enum MsgIcon
    None = MessageBoxIcon.None
    Info = MessageBoxIcon.Information
    [Error] = MessageBoxIcon.Error
    Warning = MessageBoxIcon.Warning
    Question = MessageBoxIcon.Question
End Enum

Public Enum DemuxMode
    <DispName("Show Dialog")> Dialog
    <DispName("Preferred Languages")> Preferred
    All
    None
End Enum

Public Class StringLogicalComparer
    Implements IComparer, IComparer(Of String)

    <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode)>
    Shared Function StrCmpLogical(x As String, y As String) As Integer
    End Function

    Function IComparer_Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return StrCmpLogical(x.ToString(), y.ToString())
    End Function

    Function IComparerOfString_Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
        Return StrCmpLogical(x, y)
    End Function
End Class

Public Class Comparer(Of T)
    Implements IComparer(Of T)

    Property PropName As String
    Property Ascending As Boolean = True

    Sub New(propName As String, Optional ascending As Boolean = True)
        Me.PropName = propName
        Me.Ascending = ascending
    End Sub

    Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        If Not Ascending Then
            Dim x1 = x
            x = y
            y = x1
        End If

        Dim type = x.GetType
        Dim propInfo = type.GetProperty(PropName)

        Return DirectCast(propInfo.GetValue(x), IComparable).CompareTo(propInfo.GetValue(y))
    End Function
End Class

Public Enum Symbol
    None = 0
    [Error] = &HE783
    [Like] = &HE8E1
    [Next] = &HE893
    [Stop] = &HE71A
    Accept = &HE8FB
    AcceptLegacy = &HE10B
    Accident = &HE81F
    AccidentSolid = &HEA8E
    Accounts = &HE910
    AccountsLegacy = &HE168
    ActionCenter = &HE91C
    ActionCenterAsterisk = &HEA21
    ActionCenterMirrored = &HED0D
    ActionCenterNotification = &HE7E7
    ActionCenterNotificationMirrored = &HED0C
    ActionCenterQuiet = &HEE79
    ActionCenterQuietNotification = &HEE7A
    Add = &HE710
    AddFriend = &HE8FA
    AddFriendLegacy = &HE1E2
    AddLegacy = &HE109
    AddRemoteDevice = &HE836
    AddSurfaceHub = &HECC4
    AddTo = &HECC8
    AdjustHologram = &HEBD2
    Admin = &HE7EF
    AdminLegacy = &HE1A7
    Airplane = &HE709
    AirplaneSolid = &HEB4C
    AlignCenter = &HE8E3
    AlignCenterLegacy = &HE1A1
    AlignLeft = &HE8E4
    AlignLeftLegacy = &HE1A2
    AlignRight = &HE8E2
    AlignRightLegacy = &HE1A0
    AllApps = &HE71D
    AllAppsLegacy = &HE179
    AllAppsLegacyMirrored = &HE1EC
    AllAppsMirrored = &HEA40
    Annotation = &HE924
    AppIconDefault = &HECAA
    ArrowHTMLLegacy = &HED5
    ArrowHTMLLegacyMirrored = &HEAE
    AspectRatio = &HE799
    Asterisk = &HEA38
    AsteriskBadge12 = &HEDAD
    Attach = &HE723
    AttachCamera = &HE8A2
    AttachCameraLegacy = &HE12D
    AttachLegacy = &HE16C
    Audio = &HE8D6
    AudioLegacy = &HE189
    Back = &HE72B
    BackBttnArrow20Legacy = &HEC4
    BackBttnArrow42Legacy = &HEA6
    BackBttnMirroredArrow20Legacy = &HEAD
    BackBttnMirroredArrow42Legacy = &HEAB
    BackgroundToggle = &HEF1F
    BackLegacy = &HE112
    BackSpaceQWERTY = &HE750
    BackSpaceQWERTYLg = &HEB96
    BackSpaceQWERTYMd = &HE926
    BackSpaceQWERTYSm = &HE925
    BackToWindow = &HE73F
    BackToWindowLegacy = &HE1D8
    Badge = &HEC1B
    Bank = &HE825
    BarcodeScanner = &HEC5A
    Battery0 = &HE850
    Battery1 = &HE851
    Battery10 = &HE83F
    Battery2 = &HE852
    Battery3 = &HE853
    Battery4 = &HE854
    Battery5 = &HE855
    Battery6 = &HE856
    Battery7 = &HE857
    Battery8 = &HE858
    Battery9 = &HE859
    BatteryCharging0 = &HE85A
    BatteryCharging1 = &HE85B
    BatteryCharging10 = &HEA93
    BatteryCharging2 = &HE85C
    BatteryCharging3 = &HE85D
    BatteryCharging4 = &HE85E
    BatteryCharging5 = &HE85F
    BatteryCharging6 = &HE860
    BatteryCharging7 = &HE861
    BatteryCharging8 = &HE862
    BatteryCharging9 = &HE83E
    BatterySaver0 = &HE863
    BatterySaver1 = &HE864
    BatterySaver10 = &HEA95
    BatterySaver2 = &HE865
    BatterySaver3 = &HE866
    BatterySaver4 = &HE867
    BatterySaver5 = &HE868
    BatterySaver6 = &HE869
    BatterySaver7 = &HE86A
    BatterySaver8 = &HE86B
    BatterySaver9 = &HEA94
    BatteryUnknown = &HE996
    Beta = &HEA24
    BidiLtr = &HE9AA
    BidiRtl = &HE9AB
    BlockContact = &HE8F8
    BlockContactLegacy = &HE1E0
    BlockedLegacy = &HE25B
    Bluetooth = &HE702
    BodyCam = &HEC80
    Bold = &HE8DD
    BoldFLegacy = &HE1B3
    BoldGLegacy = &HE1B1
    BoldKoreanLegacy = &HE1BD
    BoldLegacy = &HE19B
    BoldNLegacy = &HE1B7
    BoldRussionLegacy = &HE1B9
    Bookmarks = &HE8A4
    BookmarksLegacy = &HE12F
    BookmarksLegacyMirrored = &HE1EE
    BookmarksMirrored = &HEA41
    Brightness = &HE706
    Broom = &HEA99
    BrowsePhotos = &HE7C5
    BrowsePhotosLegacy = &HE155
    BrushSize = &HEDA8
    BuildingEnergy = &HEC0B
    BulletedList = &HE8FD
    BulletedListLegacy = &HE292
    BulletedListLegacyMirrored = &HE299
    BulletedListMirrored = &HEA42
    Bus = &HE806
    BusSolid = &HEB47
    Calculator = &HE8EF
    CalculatorAddition = &HE948
    CalculatorBackspace = &HE94F
    CalculatorDivide = &HE94A
    CalculatorEqualTo = &HE94E
    CalculatorLegacy = &HE1D0
    CalculatorMultiply = &HE947
    CalculatorNegate = &HE94D
    CalculatorPercentage = &HE94C
    CalculatorSquareroot = &HE94B
    CalculatorSubtract = &HE949
    Calendar = &HE787
    CalendarDay = &HE8BF
    CalendarDayLegacy = &HE161
    CalendarLegacy = &HE163
    CalendarLegacyMirrored = &HE1DC
    CalendarMirrored = &HED28
    CalendarReply = &HE8F5
    CalendarReplyLegacy = &HE1DB
    CalendarSolid = &HEA89
    CalendarWeek = &HE8C0
    CalendarWeekLegacy = &HE162
    CaligraphyPen = &HEDFB
    CallForwarding = &HE7F2
    CallForwardingMirrored = &HEA97
    CallForwardInternational = &HE87A
    CallForwardInternationalMirrored = &HEA43
    CallForwardRoaming = &HE87B
    CallForwardRoamingMirrored = &HEA44
    Camera = &HE722
    CameraLegacy = &HE114
    Cancel = &HE711
    CancelLegacy = &HE10A
    Caption = &HE8BA
    CaptionLegacy = &HE15A
    Car = &HE804
    CashDrawer = &HEC59
    CC = &HE7F0
    CCEuroLegacy = &HE18F
    CCJapanLegacy = &HE18E
    CCLegacy = &HE190
    CellPhone = &HE8EA
    CellPhoneLegacy = &HE1C9
    Certificate = &HEB95
    Characters = &HE8C1
    CharactersLegacy = &HE164
    ChatBubbles = &HE8F2
    Checkbox = &HE739
    CheckboxComposite = &HE73A
    CheckboxCompositeLegacy = &HEA2
    CheckboxCompositeReversed = &HE73D
    CheckboxCompositeReversedLegacy = &HE5
    CheckboxFill = &HE73B
    CheckboxFillLegacy = &HE2
    CheckboxFillZeroWidthLegacy = &HE9
    CheckboxIndeterminate = &HE73C
    CheckboxIndeterminateLegacy = &HE4
    CheckboxLegacy = &HE3
    CheckMark = &HE73E
    CheckMarkLegacy = &HE1
    CheckmarkListviewLegacy = &HE81
    CheckmarkMenuLegacy = &HEE7
    CheckMarkZeroWidthLegacy = &HE8
    ChevronDown = &HE70D
    ChevronDown1Legacy = &HE99
    ChevronDown2Legacy = &HE9D
    ChevronDown3Legacy = &HE15
    ChevronDown4Legacy = &HEA1
    ChevronDownMed = &HE972
    ChevronDownSmall = &HE96E
    ChevronDownSmLegacy = &HE228
    ChevronFlipDownLegacy = &HEE5
    ChevronFlipLeftLegacy = &HEE2
    ChevronFlipRightLegacy = &HEE3
    ChevronFlipUpLegacy = &HEE4
    ChevronLeft = &HE76B
    ChevronLeft1Legacy = &HE96
    ChevronLeft2Legacy = &HE9A
    ChevronLeft3Legacy = &HE12
    ChevronLeft4Legacy = &HE9E
    ChevronLeftMed = &HE973
    ChevronLeftSmall = &HE96F
    ChevronLeftSmLegacy = &HE26C
    ChevronRight = &HE76C
    ChevronRight1Legacy = &HE97
    ChevronRight2Legacy = &HE9B
    ChevronRight3Legacy = &HE13
    ChevronRight4Legacy = &HE9F
    ChevronRightMed = &HE974
    ChevronRightSmall = &HE970
    ChevronRightSmLegacy = &HE26B
    ChevronUp = &HE70E
    ChevronUp1Legacy = &HE98
    ChevronUp2Legacy = &HE9C
    ChevronUp3Legacy = &HE14
    ChevronUp4Legacy = &HEA0
    ChevronUpMed = &HE971
    ChevronUpSmall = &HE96D
    ChineseBoPoMoFo = &HE989
    ChineseChangjie = &HE981
    ChinesePinyin = &HE98A
    ChineseQuick = &HE984
    ChromeAnnotate = &HE931
    ChromeBack = &HE830
    ChromeBackMirrored = &HEA47
    ChromeBackToWindow = &HE92C
    ChromeClose = &HE8BB
    ChromeFullScreen = &HE92D
    ChromeMaximize = &HE922
    ChromeMinimize = &HE921
    ChromeRestore = &HE923
    CircleFill = &HEA3B
    CircleFillBadge12 = &HEDB0
    CircleRing = &HEA3A
    CircleRingBadge12 = &HEDAF
    CityNext = &HEC06
    CityNext2 = &HEC07
    Clear = &HE894
    ClearAllInk = &HED62
    ClearAllInkMirrored = &HEF19
    ClearLegacy = &HE106
    ClearSelection = &HE8E6
    ClearSelectionLegacy = &HE1C5
    ClearSelectionLegacyMirrored = &HE1F4
    ClearSelectionMirrored = &HEA48
    Click = &HE8B0
    ClockLegacy = &HE121
    ClosePane = &HE89F
    ClosePaneLegacy = &HE126
    ClosePaneLegacyMirrored = &HE1BF
    ClosePaneMirrored = &HEA49
    Cloud = &HE753
    CloudPrinter = &HEDA6
    Code = &HE943
    Color = &HE790
    ColorLegacy = &HE2B1
    CommaKey = &HE9AD
    CommandPrompt = &HE756
    Comment = &HE90A
    CommentInlineLegacy = &HE206
    CommentLegacy = &HE134
    Communications = &HE95A
    CompanionApp = &HEC64
    CompanionDeviceFramework = &HED5D
    Completed = &HE930
    CompletedSolid = &HEC61
    Component = &HE950
    Connect = &HE703
    ConnectApp = &HED5C
    Construction = &HE822
    ConstructionCone = &HE98F
    ConstructionSolid = &HEA8D
    Contact = &HE77B
    Contact2 = &HE8D4
    Contact2Legacy = &HE187
    Contact3Legacy = &HE2AF
    ContactInfo = &HE779
    ContactInfoLegacy = &HE136
    ContactInfoMirrored = &HEA4A
    ContactLegacy = &HE13D
    ContactPresence = &HE8CF
    ContactPresenceLegacy = &HE181
    ContactSolid = &HEA8C
    Copy = &HE8C8
    CopyLegacy = &HE16F
    Courthouse = &HEC08
    Crop = &HE7A8
    CropLegacy = &HE123
    Cut = &HE8C6
    CutLegacy = &HE16B
    DashKey = &HE9AE
    DataSense = &HE791
    DataSenseBar = &HE7A5
    DateTime = &HEC92
    DateTimeMirrored = &HEE93
    DecreaseIndentLegacy = &HE290
    DecreaseIndentLegacyMirrored = &HE297
    DefenderApp = &HE83D
    Delete = &HE74D
    DeleteLegacy = &HE107
    DeveloperTools = &HEC7A
    DeviceDiscovery = &HEBDE
    DeviceLaptopNoPic = &HE7F8
    DeviceLaptopPic = &HE7F7
    DeviceMonitorLeftPic = &HE7FA
    DeviceMonitorNoPic = &HE7FB
    DeviceMonitorRightPic = &HE7F9
    Devices = &HE772
    Devices2 = &HE975
    Devices3 = &HEA6C
    Devices4 = &HEB66
    DevicesLegacy = &HE212
    DevUpdate = &HECC5
    Diagnostic = &HE9D9
    Dialpad = &HE75F
    DialUp = &HE83C
    Dictionary = &HE82D
    DictionaryAdd = &HE82E
    DictionaryCloud = &HEBC3
    DirectAccess = &HE83B
    Directions = &HE8F0
    DirectionsLegacy = &HE1D1
    DisableUpdates = &HE8D8
    DisableUpdatesLegacy = &HE194
    DisconnectDisplay = &HEA14
    DisconnectDrive = &HE8CD
    DisconnectDriveLegacy = &HE17A
    Dislike = &HE8E0
    DislikeLegacy = &HE19E
    DMC = &HE951
    Dock = &HE952
    DockBottom = &HE90E
    DockBottomLegacy = &HE147
    DockLeft = &HE90C
    DockLeftLegacy = &HE145
    DockLeftLegacyMirrored = &HE1AB
    DockLeftMirrored = &HEA4C
    DockRight = &HE90D
    DockRightLegacy = &HE146
    DockRightLegacyMirrored = &HE1AC
    DockRightMirrored = &HEA4B
    Document = &HE8A5
    DocumentLegacy = &HE130
    Down = &HE74B
    DownLegacy = &HE1FD
    Download = &HE896
    DownloadLegacy = &HE118
    DownloadMap = &HE826
    DownShiftKey = &HE84A
    Draw = &HEC87
    DrawSolid = &HEC88
    DrivingMode = &HE7EC
    Drop = &HEB42
    DullSound = &HE911
    DullSoundKey = &HE9AF
    EaseOfAccess = &HE776
    EaseOfAccessLegacy = &HE7F
    Edit = &HE70F
    EditLegacy = &HE104
    EditLegacyMirrored = &HE1C2
    EditMirrored = &HEB7E
    Education = &HE7BE
    Emoji = &HE899
    Emoji2 = &HE76E
    Emoji2Legacy = &HE170
    EmojiLegacy = &HE11D
    EmojiSwatch = &HED5B
    EmojiTabCelebrationObjects = &HED55
    EmojiTabFavorites = &HED5A
    EmojiTabFoodPlants = &HED56
    EmojiTabPeople = &HED53
    EmojiTabSmilesAnimals = &HED54
    EmojiTabSymbols = &HED58
    EmojiTabTextSmiles = &HED59
    EmojiTabTransitPlaces = &HED57
    EndPoint = &HE81B
    EndPointSolid = &HEB4B
    EraseTool = &HE75C
    EraseToolFill = &HE82B
    EraseToolFill2 = &HE82C
    ErrorBadge = &HEA39
    ErrorBadge12 = &HEDAE
    eSIM = &HED2A
    eSIMBusy = &HED2D
    eSIMLocked = &HED2C
    eSIMNoProfile = &HED2B
    Ethernet = &HE839
    EthernetError = &HEB55
    EthernetWarning = &HEB56
    ExpandTile = &HE976
    ExpandTileLegacy = &HE13F
    ExpandTileLegacyMirrored = &HE176
    ExpandTileMirrored = &HEA4E
    ExploreContent = &HECCD
    Export = &HEDE1
    ExportMirrored = &HEDE2
    FastForward = &HEB9D
    Favorite2Legacy = &HE249
    FavoriteInlineLegacy = &HE208
    FavoriteLegacy = &HE113
    FavoriteList = &HE728
    FavoriteStar = &HE734
    FavoriteStarFill = &HE735
    Feedback = &HED15
    FeedbackApp = &HE939
    Ferry = &HE7E3
    FerrySolid = &HEB48
    FileExplorer = &HEC50
    FileExplorerApp = &HEC51
    Filter = &HE71C
    FilterLegacy = &HE16E
    FindLegacy = &HE11A
    FingerInking = &HED5F
    Fingerprint = &HE928
    Flag = &HE7C1
    FlagLegacy = &HE129
    Flashlight = &HE754
    FlickDown = &HE935
    FlickLeft = &HE937
    FlickRight = &HE938
    FlickUp = &HE936
    Folder = &HE8B7
    FolderFill = &HE8D5
    FolderLegacy = &HE188
    Font = &HE8D2
    FontColor = &HE8D3
    FontColorKoreanLegacy = &HE1BE
    FontColorLegacy = &HE186
    FontDecrease = &HE8E7
    FontDecreaseLegacy = &HE1C6
    FontIncrease = &HE8E8
    FontIncreaseLegacy = &HE1C7
    FontLegacy = &HE185
    FontSize = &HE8E9
    FontSizeLegacy = &HE1C8
    FontStyleKoreanLegacy = &HE1BA
    Forward = &HE72A
    ForwardLegacy = &HE111
    ForwardSm = &HE9AC
    FourBars = &HE908
    FourBarsLegacy = &HE1E9
    Frigid = &HE9CA
    FullAlpha = &HE97F
    FullCircleMask = &HE91F
    FullHiragana = &HE986
    FullKatakana = &HE987
    FullScreen = &HE740
    FullScreenLegacy = &HE1D9
    Game = &HE7FC
    GameConsole = &HE967
    GlobalNavButton = &HE700
    Globe = &HE774
    GlobeLegacy = &HE12B
    Go = &HE8AD
    GoLegacy = &HE143
    GoLegacyMirrored = &HE1AA
    GoMirrored = &HEA4F
    GoToStart = &HE8FC
    GoToStartLegacy = &HE1E4
    GotoToday = &HE8D1
    GotoTodayLegacy = &HE184
    GripperBarHorizontal = &HE76F
    GripperBarVertical = &HE784
    GripperResize = &HE788
    GripperResizeMirrored = &HEA50
    GripperTool = &HE75E
    Groceries = &HEC09
    GuestUser = &HEE57
    HalfAlpha = &HE97E
    HalfDullSound = &HE9B0
    HalfKatakana = &HE988
    HalfStarLeft = &HE7C6
    HalfStarRight = &HE7C7
    Handwriting = &HE929
    HangUp = &HE778
    HangUpLegacy = &HE137
    HardDrive = &HEDA2
    Headphone = &HE7F6
    Headphone0 = &HED30
    Headphone1 = &HED31
    Headphone2 = &HED32
    Headphone3 = &HED33
    Headset = &HE95B
    Health = &HE95E
    Heart = &HEB51
    HeartBroken = &HEA92
    HeartBrokenLegacy = &HE7
    HeartBrokenZeroWidthLegacy = &HEC
    HeartFill = &HEB52
    HeartFillLegacy = &HEA5
    HeartFillZeroWidthLegacy = &HEB
    HeartLegacy = &HE6
    Help = &HE897
    HelpLegacy = &HE11B
    HelpLegacyMirrored = &HE1F3
    HelpMirrored = &HEA51
    HideBcc = &HE8C5
    HideBccLegacy = &HE16A
    Highlight = &HE7E6
    HighlightFill = &HE891
    HighlightFill2 = &HE82A
    HighlightLegacy = &HE193
    History = &HE81C
    Home = &HE80F
    HomeGroup = &HEC26
    HomeLegacy = &HE10F
    HomeSolid = &HEA8A
    HorizontalTabKey = &HE7FD
    IBeam = &HE933
    IBeamOutline = &HE934
    ImageExport = &HEE71
    Import = &HE8B5
    ImportAll = &HE8B6
    ImportAllLegacy = &HE151
    ImportAllLegacyMirrored = &HE1AE
    ImportAllMirrored = &HEA53
    Important = &HE8C9
    ImportantBadge12 = &HEDB1
    ImportantLegacy = &HE171
    ImportLegacy = &HE150
    ImportLegacyMirrored = &HE1AD
    ImportMirrored = &HEA52
    IncidentTriangle = &HE814
    IncreaseIndentLegacy = &HE291
    IncreaseIndentLegacyMirrored = &HE298
    Info = &HE946
    Info2 = &HEA1F
    InkingCaret = &HED65
    InkingColorFill = &HED67
    InkingColorOutline = &HED66
    InkingTool = &HE76D
    InkingToolFill = &HE88F
    InkingToolFill2 = &HE829
    InPrivate = &HE727
    Input = &HE961
    InsiderHubApp = &HEC24
    InternetSharing = &HE704
    Italic = &HE8DB
    ItalicCLegacy = &HE1B0
    ItalicILegacy = &HE1B6
    ItalicKLegacy = &HE1B4
    ItalicKoreanLegacy = &HE1BC
    ItalicLegacy = &HE199
    ItalicRussianLegacy = &HE1EA
    Japanese = &HE985
    JpnRomanji = &HE87C
    JpnRomanjiLock = &HE87D
    JpnRomanjiShift = &HE87E
    JpnRomanjiShiftLock = &HE87F
    Key12On = &HE980
    KeyboardBrightness = &HED39
    KeyboardClassic = &HE765
    KeyboardDismiss = &HE92F
    KeyboardFull = &HEC31
    KeyboardLeftHanded = &HE763
    KeyBoardLegacy = &HE144
    KeyboardLowerBrightness = &HED3A
    KeyboardOneHanded = &HED4C
    KeyboardRightHanded = &HE764
    KeyboardShortcut = &HEDA7
    KeyboardSplit = &HE766
    KeyboardSplitLegacy = &HE8F
    KeyboardStandard = &HE92E
    KeyboardStandardLegacy = &HE87
    Korean = &HE97D
    Label = &HE932
    LangJPN = &HE7DE
    LanguageChs = &HE88D
    LanguageCht = &HE88C
    LanguageJpn = &HEC45
    LanguageKor = &HE88B
    LaptopSelected = &HEC76
    LayoutLegacy = &HE2AE
    Leaf = &HE8BE
    LeaveChat = &HE89B
    LeaveChatLegacy = &HE11F
    LeaveChatMirrored = &HEA54
    LEDLight = &HE781
    LeftArrowKeyTime0 = &HEC52
    LeftDoubleQuote = &HE9B2
    LeftQuote = &HE848
    LengthLegacy = &HE2AD
    Library = &HE8F1
    LibraryLegacy = &HE1D3
    Light = &HE793
    Lightbulb = &HEA80
    LightningBolt = &HE945
    LikeDislike = &HE8DF
    LikeDislikeLegacy = &HE19D
    LikeInlineLegacy = &HE209
    LikeLegacy = &HE19F
    Link = &HE71B
    LinkLegacy = &HE167
    List = &HEA37
    ListLegacy = &HE14C
    ListLegacyMirrored = &HE175
    ListMirrored = &HEA55
    Location = &HE81D
    LocationLegacy = &HE1D2
    Lock = &HE72E
    LockLegacy = &HE1F6
    LockscreenDesktop = &HEE3F
    LockScreenGlance = &HEE65
    LowerBrightness = &HEC8A
    MagStripeReader = &HEC5C
    Mail = &HE715
    MailBadge12 = &HEDB3
    MailFill = &HE8A8
    MailFillLegacy = &HE135
    MailForward = &HE89C
    MailForwardLegacy = &HE120
    MailForwardLegacyMirrored = &HE1A8
    MailForwardMirrored = &HEA56
    MailLegacy = &HE119
    MailMessageLegacy = &HE20B
    MailReply = &HE8CA
    MailReplyAll = &HE8C2
    MailReplyAllLegacy = &HE165
    MailReplyAllLegacyMirrored = &HE1F2
    MailReplyAllMirrored = &HEA58
    MailReplyLegacy = &HE172
    MailReplyLegacyMirrored = &HE1AF
    MailReplyMirrored = &HEA57
    Manage = &HE912
    ManageLegacy = &HE178
    MapCompassBottom = &HE813
    MapCompassTop = &HE812
    MapDirections = &HE816
    MapDrive = &HE8CE
    MapDriveLegacy = &HE17B
    MapLayers = &HE81E
    MapLegacy = &HE1C4
    MapPin = &HE707
    MapPin2 = &HE7B7
    MapPinLegacy = &HE139
    Marker = &HED64
    Marquee = &HEF20
    Media = &HEA69
    MediaStorageTower = &HE965
    Megaphone = &HE789
    Memo = &HE77C
    MemoLegacy = &HE1D5
    Message = &HE8BD
    MessageLegacy = &HE15F
    MicClipping = &HEC72
    MicError = &HEC56
    MicOff = &HEC54
    MicOn = &HEC71
    Microphone = &HE720
    MicrophoneLegacy = &HE1D6
    MicSleep = &HEC55
    MiracastLogoLarge = &HEC16
    MiracastLogoSmall = &HEC15
    MobActionCenter = &HEC42
    MobAirplane = &HEC40
    MobBattery0 = &HEBA0
    MobBattery1 = &HEBA1
    MobBattery10 = &HEBAA
    MobBattery2 = &HEBA2
    MobBattery3 = &HEBA3
    MobBattery4 = &HEBA4
    MobBattery5 = &HEBA5
    MobBattery6 = &HEBA6
    MobBattery7 = &HEBA7
    MobBattery8 = &HEBA8
    MobBattery9 = &HEBA9
    MobBatteryCharging0 = &HEBAB
    MobBatteryCharging1 = &HEBAC
    MobBatteryCharging10 = &HEBB5
    MobBatteryCharging2 = &HEBAD
    MobBatteryCharging3 = &HEBAE
    MobBatteryCharging4 = &HEBAF
    MobBatteryCharging5 = &HEBB0
    MobBatteryCharging6 = &HEBB1
    MobBatteryCharging7 = &HEBB2
    MobBatteryCharging8 = &HEBB3
    MobBatteryCharging9 = &HEBB4
    MobBatterySaver0 = &HEBB6
    MobBatterySaver1 = &HEBB7
    MobBatterySaver10 = &HEBC0
    MobBatterySaver2 = &HEBB8
    MobBatterySaver3 = &HEBB9
    MobBatterySaver4 = &HEBBA
    MobBatterySaver5 = &HEBBB
    MobBatterySaver6 = &HEBBC
    MobBatterySaver7 = &HEBBD
    MobBatterySaver8 = &HEBBE
    MobBatterySaver9 = &HEBBF
    MobBatteryUnknown = &HEC02
    MobBluetooth = &HEC41
    MobCallForwarding = &HEC7E
    MobCallForwardingMirrored = &HEC7F
    MobDrivingMode = &HEC47
    MobileContactLegacy = &HE25A
    MobileLocked = &HEC20
    MobileSelected = &HEC75
    MobileTablet = &HE8CC
    MobLocation = &HEC43
    MobQuietHours = &HEC46
    MobSignal1 = &HEC37
    MobSignal2 = &HEC38
    MobSignal3 = &HEC39
    MobSignal4 = &HEC3A
    MobSignal5 = &HEC3B
    MobWifi1 = &HEC3C
    MobWifi2 = &HEC3D
    MobWifi3 = &HEC3E
    MobWifi4 = &HEC3F
    MobWifiHotspot = &HEC44
    More = &HE712
    MoreLegacy = &HE10C
    Mouse = &HE962
    MoveToFolder = &HE8DE
    MoveToFolderLegacy = &HE19C
    Movies = &HE8B2
    MultimediaDMP = &HED47
    MultimediaDMS = &HE953
    MultimediaDVR = &HE954
    MultimediaPMP = &HE955
    MultiSelect = &HE762
    MultiSelectLegacy = &HE133
    MultiSelectLegacyMirrored = &HE1EF
    MultiSelectMirrored = &HEA98
    Multitask = &HE7C4
    Multitask16 = &HEE40
    MultitaskExpanded = &HEB91
    MusicAlbum = &HE93C
    MusicInfo = &HE90B
    MusicInfoLegacy = &HE142
    MusicNote = &HEC4F
    Mute = &HE74F
    MuteLegacy = &HE198
    MyNetwork = &HEC27
    Narrator = &HED4D
    NarratorForward = &HEDA9
    NarratorForwardMirrored = &HEDAA
    Network = &HE968
    NetworkAdapter = &HEDA3
    NetworkPrinter = &HEDA5
    NetworkTower = &HEC05
    NewFolder = &HE8F4
    NewFolderLegacy = &HE1DA
    NewWindow = &HE78B
    NewWindowLegacy = &HE17C
    NextLegacy = &HE101
    NUIFace = &HEB68
    NUIFPContinueSlideAction = &HEB85
    NUIFPContinueSlideHand = &HEB84
    NUIFPPressAction = &HEB8B
    NUIFPPressHand = &HEB8A
    NUIFPPressRepeatAction = &HEB8D
    NUIFPPressRepeatHand = &HEB8C
    NUIFPRollLeftAction = &HEB89
    NUIFPRollLeftHand = &HEB88
    NUIFPRollRightHand = &HEB86
    NUIFPRollRightHandAction = &HEB87
    NUIFPStartSlideAction = &HEB83
    NUIFPStartSlideHand = &HEB82
    NUIIris = &HEB67
    OEM = &HE74C
    OneBar = &HE905
    OneBarLegacy = &HE1E6
    OpenFile = &HE8E5
    OpenFileLegacy = &HE1A5
    OpenInNewWindow = &HE8A7
    OpenInNewWindowLegacy = &HE2B4
    OpenLocal = &HE8DA
    OpenLocalLegacy = &HE197
    OpenPane = &HE8A0
    OpenPaneLegacy = &HE127
    OpenPaneLegacyMirrored = &HE1C0
    OpenPaneMirrored = &HEA5B
    OpenWith = &HE7AC
    OpenWithLegacy = &HE17D
    OpenWithLegacyMirrored = &HE1ED
    OpenWithMirrored = &HEA5C
    Orientation = &HE8B4
    OrientationLegacy = &HE14F
    OtherUser = &HE7EE
    OtherUserLegacy = &HE1A6
    OutlineStarLegacy = &HE1CE
    Package = &HE7B8
    Page = &HE7C3
    PageFillLegacy = &HE132
    PageLeft = &HE760
    PageLegacy = &HE160
    PageRight = &HE761
    PageSolid = &HE729
    PanMode = &HECE9
    ParkingLocation = &HE811
    ParkingLocationMirrored = &HEA5E
    ParkingLocationSolid = &HEA8B
    PartyLeader = &HECA7
    PasswordKeyHide = &HE9A9
    PasswordKeyShow = &HE9A8
    Paste = &HE77F
    PasteLegacy = &HE16D
    Pause = &HE769
    PauseBadge12 = &HEDB4
    PauseLegacy = &HE103
    PC1 = &HE977
    PC1Legacy = &HE211
    Pencil = &HED63
    PenPalette = &HEE56
    PenPaletteMirrored = &HEF16
    PenWorkspace = &HEDC6
    PenWorkspaceMirrored = &HEF15
    People = &HE716
    PeopleLegacy = &HE125
    PeriodKey = &HE843
    Permissions = &HE8D7
    PermissionsLegacy = &HE192
    PersonalFolder = &HEC25
    Personalize = &HE771
    Phone = &HE717
    PhoneBook = &HE780
    PhoneBookLegacy = &HE1D4
    PhoneLegacy = &HE13A
    Photo = &HE91B
    Photo2 = &HEB9F
    Picture = &HE8B9
    PictureLegacy = &HE158
    PieSingle = &HEB05
    Pin = &HE718
    PinFill = &HE841
    PinLegacy = &HE141
    Pinned = &HE840
    PinnedFill = &HE842
    PlaceFolderLegacy = &HE18A
    PLAP = &HEC19
    Play = &HE768
    Play36 = &HEE4A
    PlaybackRate1x = &HEC57
    PlaybackRateOther = &HEC58
    PlayBadge12 = &HEDB5
    PlayLegacy = &HE102
    PlayOnLegacy = &HE29B
    PointErase = &HED61
    PointEraseMirrored = &HEF18
    PoliceCar = &HEC81
    PostUpdate = &HE8F3
    PostUpdateLegacy = &HE1D7
    PowerButton = &HE7E8
    PresenceChicklet = &HE978
    PresenceChickletLegacy = &HE25E
    PresenceChickletVideo = &HE979
    PresenceChickletVideoLegacy = &HE25D
    Preview = &HE8FF
    PreviewLegacy = &HE295
    PreviewLink = &HE8A1
    PreviewLinkLegacy = &HE12A
    Previous = &HE892
    PreviousLegacy = &HE100
    Print = &HE749
    Printer3D = &HE914
    Printer3DLegacy = &HE2F7
    PrintfaxPrinterFile = &HE956
    PrintLegacy = &HE2F6
    Priority = &HE8D0
    PriorityLegacy = &HE182
    Process = &HE9F3
    Project = &HEBC6
    Projector = &HE95D
    ProtectedDocument = &HE8A6
    ProtectedDocumentLegacy = &HE131
    ProvisioningPackage = &HE835
    PuncKey = &HE844
    PuncKey0 = &HE84C
    PuncKey1 = &HE9B4
    PuncKey2 = &HE9B5
    PuncKey3 = &HE9B6
    PuncKey4 = &HE9B7
    PuncKey5 = &HE9B8
    PuncKey6 = &HE9B9
    PuncKey7 = &HE9BB
    PuncKey8 = &HE9BC
    PuncKey9 = &HE9BA
    PuncKeyLeftBottom = &HE84D
    PuncKeyRightBottom = &HE9B3
    Puzzle = &HEA86
    QuickNote = &HE70B
    QuietHours = &HE708
    QWERTYOff = &HE983
    QWERTYOn = &HE982
    RadioBtnOff = &HECCA
    RadioBtnOn = &HECCB
    RadioBullet = &HE915
    RadioBullet2 = &HECCC
    RatingStarFillLegacy = &HEB4
    RatingStarFillReducedPaddingHTMLLegacy = &HE82
    RatingStarFillSmallLegacy = &HEB5
    RatingStarFillZeroWidthLegacy = &HEA
    RatingStarLegacy = &HE224
    Read = &HE8C3
    ReadingList = &HE7BC
    ReadLegacy = &HE166
    ReceiptPrinter = &HEC5B
    Recent = &HE823
    Record = &HE7C8
    RecordLegacy = &HE1F5
    Redo = &HE7A6
    RedoLegacy = &HE10D
    ReduceTileLegacy = &HE140
    ReduceTileLegacyMirrored = &HE177
    Refresh = &HE72C
    RefreshLegacy = &HE149
    RememberedDevice = &HE70C
    Reminder = &HEB50
    ReminderFill = &HEB4F
    Remote = &HE8AF
    RemoteLegacy = &HE148
    Remove = &HE738
    RemoveFrom = &HECC9
    RemoveLegacy = &HE108
    Rename = &HE8AC
    RenameLegacy = &HE13E
    Repair = &HE90F
    RepairLegacy = &HE15E
    RepeatAll = &HE8EE
    RepeatAllLegacy = &HE1CD
    RepeatOne = &HE8ED
    RepeatOneLegacy = &HE1CC
    Reply = &HE97A
    ReplyLegacy = &HE248
    ReplyMirrored = &HEE35
    ReportHacked = &HE730
    ReportHackedLegacy = &HE1DE
    ResetDevice = &HED10
    ResetDrive = &HEBC4
    Reshare = &HE8EB
    ReshareLegacy = &HE1CA
    ResizeMouseLarge = &HE747
    ResizeMouseMedium = &HE744
    ResizeMouseMediumMirrored = &HEA5F
    ResizeMouseSmall = &HE743
    ResizeMouseSmallMirrored = &HEA60
    ResizeMouseTall = &HE746
    ResizeMouseTallMirrored = &HEA61
    ResizeMouseWide = &HE745
    ResizeTouchLarger = &HE741
    ResizeTouchNarrower = &HE7EA
    ResizeTouchNarrowerMirrored = &HEA62
    ResizeTouchShorter = &HE7EB
    ResizeTouchSmaller = &HE742
    ResolutionLegacy = &HE2AC
    ReturnKey = &HE751
    ReturnKeyLg = &HEB97
    ReturnKeySm = &HE966
    ReturnToWindow = &HE944
    ReturnToWindowLegacy = &HE2B3
    RevealPasswordLegacy = &HE52
    RevToggleKey = &HE845
    Rewind = &HEB9E
    RightArrowKeyTime0 = &HEBE7
    RightArrowKeyTime1 = &HE846
    RightArrowKeyTime2 = &HE847
    RightArrowKeyTime3 = &HE84E
    RightArrowKeyTime4 = &HE84F
    RightDoubleQuote = &HE9B1
    RightQuote = &HE849
    Ringer = &HEA8F
    RingerBadge12 = &HEDAC
    RingerSilent = &HE7ED
    RoamingDomestic = &HE879
    RoamingInternational = &HE878
    Robot = &HE99A
    Rotate = &HE7AD
    RotateCamera = &HE89E
    RotateCameraLegacy = &HE124
    RotateLegacy = &HE14A
    RotateMapLeft = &HE80D
    RotateMapRight = &HE80C
    RotationLock = &HE755
    Ruler = &HED5E
    Save = &HE74E
    SaveAs = &HE792
    SaveAsLegacy = &HE28F
    SaveCopy = &HEA35
    SaveLegacy = &HE105
    SaveLocal = &HE78C
    SaveLocalLegacy = &HE159
    Scan = &HE8FE
    ScanLegacy = &HE294
    ScrollChevronDownBoldLegacy = &HE19
    ScrollChevronDownLegacy = &HE11
    ScrollChevronLeftBoldLegacy = &HE16
    ScrollChevronLeftLegacy = &HEE
    ScrollChevronRightBoldLegacy = &HE17
    ScrollChevronRightLegacy = &HEF
    ScrollChevronUpBoldLegacy = &HE18
    ScrollChevronUpLegacy = &HE10
    ScrollMode = &HECE7
    ScrollUpDown = &HEC8F
    SDCard = &HE7F1
    Search = &HE721
    SearchAndApps = &HE773
    SearchboxLegacy = &HE94
    SelectAll = &HE8B3
    SelectAllLegacy = &HE14E
    SemanticZoomLegacy = &HEB8
    Send = &HE724
    SendFill = &HE725
    SendFillMirrored = &HEA64
    SendLegacy = &HE122
    SendMirrored = &HEA63
    Sensor = &HE957
    SetlockScreen = &HE7B5
    SetlockScreenLegacy = &HE18C
    SetTile = &HE97B
    SetTileLegacy = &HE18D
    Settings = &HE713
    SettingsBattery = &HEE63
    SettingsDisplaySound = &HE7F3
    SettingsLegacy = &HE115
    Share = &HE72D
    ShareBroadband = &HE83A
    Shop = &HE719
    ShopLegacy = &HE14D
    ShoppingCart = &HE7BF
    ShowAllFiles1Legacy = &HE153
    ShowAllFiles3Legacy = &HE152
    ShowAllFilesLegacy = &HE154
    ShowBcc = &HE8C4
    ShowBccLegacy = &HE169
    ShowResults = &HE8BC
    ShowResultsLegacy = &HE15C
    ShowResultsLegacyMirrored = &HE1F1
    ShowResultsMirrored = &HEA65
    Shuffle = &HE8B1
    ShuffleLegacy = &HE14B
    SignalBars1 = &HE86C
    SignalBars2 = &HE86D
    SignalBars3 = &HE86E
    SignalBars4 = &HE86F
    SignalBars5 = &HE870
    SignalError = &HED2E
    SignalNotConnected = &HE871
    SignalRoaming = &HEC1E
    SIMLock = &HE875
    SIMMissing = &HE876
    SIPMove = &HE759
    SIPRedock = &HE75B
    SIPUndock = &HE75A
    SizeLegacy = &HE2B2
    SkipBack10 = &HED3C
    SkipForward30 = &HED3D
    SliderThumb = &HEC13
    Slideshow = &HE786
    SlideshowLegacy = &HE173
    SlowMotionOn = &HEA79
    Smartcard = &HE963
    SmartcardVirtual = &HE964
    SolidStarLegacy = &HE1CF
    Sort = &HE8CB
    SortLegacy = &HE174
    Speakers = &HE7F5
    SpeedHigh = &HEC4A
    SpeedMedium = &HEC49
    SpeedOff = &HEC48
    StartPoint = &HE819
    StartPointSolid = &HEB49
    StatusCircle = &HEA81
    StatusCircleLeft = &HEBFD
    StatusConnecting1 = &HEB57
    StatusConnecting2 = &HEB58
    StatusDataTransfer = &HE880
    StatusDataTransferVPN = &HE881
    StatusDualSIM1 = &HE884
    StatusDualSIM1VPN = &HE885
    StatusDualSIM2 = &HE882
    StatusDualSIM2VPN = &HE883
    StatusError = &HEA83
    StatusErrorFull = &HEB90
    StatusErrorLeft = &HEBFF
    StatusSGLTE = &HE886
    StatusSGLTECell = &HE887
    StatusSGLTEDataVPN = &HE888
    StatusTriangle = &HEA82
    StatusTriangleLeft = &HEBFE
    StatusUnsecure = &HEB59
    StatusVPN = &HE889
    StatusWarning = &HEA84
    StatusWarningLeft = &HEC00
    StockDown = &HEB0F
    StockUp = &HEB11
    StopLegacy = &HE15B
    StopPoint = &HE81A
    StopPointSolid = &HEB4A
    StopSlideshowLegacy = &HE191
    Stopwatch = &HE916
    StorageNetworkWireless = &HE969
    StorageOptical = &HE958
    StorageTape = &HE96A
    Streaming = &HE93E
    StreamingEnterprise = &HED2F
    Street = &HE913
    StreetLegacy = &HE1C3
    StreetsideSplitExpand = &HE803
    StreetsideSplitMinimize = &HE802
    StrokeErase = &HED60
    StrokeEraseMirrored = &HEF17
    Subtitles = &HED1E
    SubtitlesAudio = &HED1F
    SurfaceHub = &HE8AE
    Sustainable = &HEC0A
    Swipe = &HE927
    SwipeRevealArt = &HEC6D
    Switch = &HE8AB
    SwitchApps = &HE8F9
    SwitchAppsLegacy = &HE1E1
    SwitchLegacy = &HE13C
    SwitchUser = &HE748
    Sync = &HE895
    SyncBadge12 = &HEDAB
    SyncError = &HEA6A
    SyncFolder = &HE8F7
    SyncFolderLegacy = &HE1DF
    SyncLegacy = &HE117
    System = &HE770
    Tablet = &HE70A
    TabletMode = &HEBFC
    TabletSelected = &HEC74
    Tag = &HE8EC
    TagLegacy = &HE1CB
    TapAndSend = &HE9A1
    TaskbarPhone = &HEE64
    ThisPC = &HEC4E
    ThoughtBubble = &HEA91
    ThreeBars = &HE907
    ThreeBarsLegacy = &HE1E8
    Tiles = &HECA5
    TiltDown = &HE80A
    TiltUp = &HE809
    TimeLanguage = &HE775
    ToggleBorder = &HEC12
    ToggleFilled = &HEC11
    ToggleThumb = &HEC14
    ToolTip = &HE82F
    Touch = &HE815
    TouchPointer = &HE7C9
    TouchPointerLegacy = &HE1E3
    Touchscreen = &HEDA4
    Trackers = &HEADF
    TrackersMirrored = &HEE92
    Train = &HE7C0
    TrainSolid = &HEB4D
    TreeFolderFolder = &HED41
    TreeFolderFolderFill = &HED42
    TreeFolderFolderOpen = &HED43
    TreeFolderFolderOpenFill = &HED44
    Trim = &HE78A
    TrimLegacy = &HE12C
    TVMonitor = &HE7F4
    TVMonitorSelected = &HEC77
    TwoBars = &HE906
    TwoBarsLegacy = &HE1E7
    TwoPage = &HE89A
    TwoPageLegacy = &HE11E
    Type = &HE97C
    TypeLegacy = &HE2B0
    TypingIndicatorLegacy = &HE25C
    Underline = &HE8DC
    UnderlineLegacy = &HE19A
    UnderlineLKoreanLegacy = &HE1BB
    UnderlineRussianLegacy = &HE1B8
    UnderlineSLegacy = &HE1B2
    UnderlineULegacy = &HE1B5
    UnderscoreSpace = &HE75D
    Undo = &HE7A7
    UndoLegacy = &HE10E
    Unfavorite = &HE8D9
    Unfavorite2Legacy = &HE24A
    UnfavoriteLegacy = &HE195
    Unit = &HECC6
    Unlock = &HE785
    UnlockLegacy = &HE1F7
    Unpin = &HE77A
    UnpinLegacy = &HE196
    UnsyncFolder = &HE8F6
    UnsyncFolderLegacy = &HE1DD
    Up = &HE74A
    UpArrowShiftKey = &HE752
    UpdateRestore = &HE777
    UpLegacy = &HE110
    Upload = &HE898
    UploadLegacy = &HE11C
    UploadSkyDriveLegacy = &HE183
    UpShiftKey = &HE84B
    USB = &HE88E
    USBSafeConnect = &HECF3
    Vibrate = &HE877
    Video = &HE714
    VideoChat = &HE8AA
    VideoChatLegacy = &HE13B
    VideoInlineLegacy = &HE20A
    VideoLegacy = &HE116
    View = &HE890
    ViewAll = &HE8A9
    ViewAllLegacy = &HE138
    ViewLegacy = &HE18B
    Volume = &HE767
    Volume0 = &HE992
    Volume1 = &HE993
    Volume2 = &HE994
    Volume3 = &HE995
    VolumeBars = &HEBC5
    VolumeLegacy = &HE15D
    VPN = &HE705
    Walk = &HE805
    WalkSolid = &HE726
    Warning = &HE7BA
    Webcam = &HE8B8
    Webcam2 = &HE960
    WebcamLegacy = &HE156
    Wheel = &HEE94
    Wifi = &HE701
    Wifi1 = &HE872
    Wifi2 = &HE873
    Wifi3 = &HE874
    WifiAttentionOverlay = &HE998
    WifiCall0 = &HEBD5
    WifiCall1 = &HEBD6
    WifiCall2 = &HEBD7
    WifiCall3 = &HEBD8
    WifiCall4 = &HEBD9
    WifiCallBars = &HEBD4
    WifiError0 = &HEB5A
    WifiError1 = &HEB5B
    WifiError2 = &HEB5C
    WifiError3 = &HEB5D
    WifiError4 = &HEB5E
    WifiEthernet = &HEE77
    WifiHotspot = &HE88A
    WifiWarning0 = &HEB5F
    WifiWarning1 = &HEB60
    WifiWarning2 = &HEB61
    WifiWarning3 = &HEB62
    WifiWarning4 = &HEB63
    WindDirection = &HEBE6
    WiredUSB = &HECF0
    WirelessUSB = &HECF1
    Work = &HE821
    WorkSolid = &HEB4E
    World = &HE909
    WorldLegacy = &HE128
    XboxOneConsole = &HE990
    ZeroBars = &HE904
    ZeroBarsLegacy = &HE1E5
    Zoom = &HE71E
    ZoomIn = &HE8A3
    ZoomInLegacy = &HE12E
    ZoomLegacy = &HE1A3
    ZoomMode = &HECE8
    ZoomOut = &HE71F
    ZoomOutLegacy = &HE1A4
    fa_500px = &HF26E
    fa_address_book = &HF2B9
    fa_address_book_o = &HF2BA
    fa_address_card = &HF2BB
    fa_address_card_o = &HF2BC
    fa_adjust = &HF042
    fa_adn = &HF170
    fa_align_center = &HF037
    fa_align_justify = &HF039
    fa_align_left = &HF036
    fa_align_right = &HF038
    fa_amazon = &HF270
    fa_ambulance = &HF0F9
    fa_american_sign_language_interpreting = &HF2A3
    fa_anchor = &HF13D
    fa_android = &HF17B
    fa_angellist = &HF209
    fa_angle_double_down = &HF103
    fa_angle_double_left = &HF100
    fa_angle_double_right = &HF101
    fa_angle_double_up = &HF102
    fa_angle_down = &HF107
    fa_angle_left = &HF104
    fa_angle_right = &HF105
    fa_angle_up = &HF106
    fa_apple = &HF179
    fa_archive = &HF187
    fa_area_chart = &HF1FE
    fa_arrow_circle_down = &HF0AB
    fa_arrow_circle_left = &HF0A8
    fa_arrow_circle_o_down = &HF01A
    fa_arrow_circle_o_left = &HF190
    fa_arrow_circle_o_right = &HF18E
    fa_arrow_circle_o_up = &HF01B
    fa_arrow_circle_right = &HF0A9
    fa_arrow_circle_up = &HF0AA
    fa_arrow_down = &HF063
    fa_arrow_left = &HF060
    fa_arrow_right = &HF061
    fa_arrow_up = &HF062
    fa_arrows = &HF047
    fa_arrows_alt = &HF0B2
    fa_arrows_h = &HF07E
    fa_arrows_v = &HF07D
    fa_asl_interpreting = &HF2A3
    fa_assistive_listening_systems = &HF2A2
    fa_asterisk = &HF069
    fa_at = &HF1FA
    fa_audio_description = &HF29E
    fa_automobile = &HF1B9
    fa_backward = &HF04A
    fa_balance_scale = &HF24E
    fa_ban = &HF05E
    fa_bandcamp = &HF2D5
    fa_bank = &HF19C
    fa_bar_chart = &HF080
    fa_bar_chart_o = &HF080
    fa_barcode = &HF02A
    fa_bars = &HF0C9
    fa_bath = &HF2CD
    fa_bathtub = &HF2CD
    fa_battery = &HF240
    fa_battery_0 = &HF244
    fa_battery_1 = &HF243
    fa_battery_2 = &HF242
    fa_battery_3 = &HF241
    fa_battery_4 = &HF240
    fa_battery_empty = &HF244
    fa_battery_full = &HF240
    fa_battery_half = &HF242
    fa_battery_quarter = &HF243
    fa_battery_three_quarters = &HF241
    fa_bed = &HF236
    fa_beer = &HF0FC
    fa_behance = &HF1B4
    fa_behance_square = &HF1B5
    fa_bell = &HF0F3
    fa_bell_o = &HF0A2
    fa_bell_slash = &HF1F6
    fa_bell_slash_o = &HF1F7
    fa_bicycle = &HF206
    fa_binoculars = &HF1E5
    fa_birthday_cake = &HF1FD
    fa_bitbucket = &HF171
    fa_bitbucket_square = &HF172
    fa_bitcoin = &HF15A
    fa_black_tie = &HF27E
    fa_blind = &HF29D
    fa_bluetooth = &HF293
    fa_bluetooth_b = &HF294
    fa_bold = &HF032
    fa_bolt = &HF0E7
    fa_bomb = &HF1E2
    fa_book = &HF02D
    fa_bookmark = &HF02E
    fa_bookmark_o = &HF097
    fa_braille = &HF2A1
    fa_briefcase = &HF0B1
    fa_btc = &HF15A
    fa_bug = &HF188
    fa_building = &HF1AD
    fa_building_o = &HF0F7
    fa_bullhorn = &HF0A1
    fa_bullseye = &HF140
    fa_bus = &HF207
    fa_buysellads = &HF20D
    fa_cab = &HF1BA
    fa_calculator = &HF1EC
    fa_calendar = &HF073
    fa_calendar_check_o = &HF274
    fa_calendar_minus_o = &HF272
    fa_calendar_o = &HF133
    fa_calendar_plus_o = &HF271
    fa_calendar_times_o = &HF273
    fa_camera = &HF030
    fa_camera_retro = &HF083
    fa_car = &HF1B9
    fa_caret_down = &HF0D7
    fa_caret_left = &HF0D9
    fa_caret_right = &HF0DA
    fa_caret_square_o_down = &HF150
    fa_caret_square_o_left = &HF191
    fa_caret_square_o_right = &HF152
    fa_caret_square_o_up = &HF151
    fa_caret_up = &HF0D8
    fa_cart_arrow_down = &HF218
    fa_cart_plus = &HF217
    fa_cc = &HF20A
    fa_cc_amex = &HF1F3
    fa_cc_diners_club = &HF24C
    fa_cc_discover = &HF1F2
    fa_cc_jcb = &HF24B
    fa_cc_mastercard = &HF1F1
    fa_cc_paypal = &HF1F4
    fa_cc_stripe = &HF1F5
    fa_cc_visa = &HF1F0
    fa_certificate = &HF0A3
    fa_chain = &HF0C1
    fa_chain_broken = &HF127
    fa_check = &HF00C
    fa_check_circle = &HF058
    fa_check_circle_o = &HF05D
    fa_check_square = &HF14A
    fa_check_square_o = &HF046
    fa_chevron_circle_down = &HF13A
    fa_chevron_circle_left = &HF137
    fa_chevron_circle_right = &HF138
    fa_chevron_circle_up = &HF139
    fa_chevron_down = &HF078
    fa_chevron_left = &HF053
    fa_chevron_right = &HF054
    fa_chevron_up = &HF077
    fa_child = &HF1AE
    fa_chrome = &HF268
    fa_circle = &HF111
    fa_circle_o = &HF10C
    fa_circle_o_notch = &HF1CE
    fa_circle_thin = &HF1DB
    fa_clipboard = &HF0EA
    fa_clock_o = &HF017
    fa_clone = &HF24D
    fa_close = &HF00D
    fa_cloud = &HF0C2
    fa_cloud_download = &HF0ED
    fa_cloud_upload = &HF0EE
    fa_cny = &HF157
    fa_code = &HF121
    fa_code_fork = &HF126
    fa_codepen = &HF1CB
    fa_codiepie = &HF284
    fa_coffee = &HF0F4
    fa_cog = &HF013
    fa_cogs = &HF085
    fa_columns = &HF0DB
    fa_comment = &HF075
    fa_comment_o = &HF0E5
    fa_commenting = &HF27A
    fa_commenting_o = &HF27B
    fa_comments = &HF086
    fa_comments_o = &HF0E6
    fa_compass = &HF14E
    fa_compress = &HF066
    fa_connectdevelop = &HF20E
    fa_contao = &HF26D
    fa_copy = &HF0C5
    fa_copyright = &HF1F9
    fa_creative_commons = &HF25E
    fa_credit_card = &HF09D
    fa_credit_card_alt = &HF283
    fa_crop = &HF125
    fa_crosshairs = &HF05B
    fa_css3 = &HF13C
    fa_cube = &HF1B2
    fa_cubes = &HF1B3
    fa_cut = &HF0C4
    fa_cutlery = &HF0F5
    fa_dashboard = &HF0E4
    fa_dashcube = &HF210
    fa_database = &HF1C0
    fa_deaf = &HF2A4
    fa_deafness = &HF2A4
    fa_dedent = &HF03B
    fa_delicious = &HF1A5
    fa_desktop = &HF108
    fa_deviantart = &HF1BD
    fa_diamond = &HF219
    fa_digg = &HF1A6
    fa_dollar = &HF155
    fa_dot_circle_o = &HF192
    fa_download = &HF019
    fa_dribbble = &HF17D
    fa_drivers_license = &HF2C2
    fa_drivers_license_o = &HF2C3
    fa_dropbox = &HF16B
    fa_drupal = &HF1A9
    fa_edge = &HF282
    fa_edit = &HF044
    fa_eercast = &HF2DA
    fa_eject = &HF052
    fa_ellipsis_h = &HF141
    fa_ellipsis_v = &HF142
    fa_empire = &HF1D1
    fa_envelope = &HF0E0
    fa_envelope_o = &HF003
    fa_envelope_open = &HF2B6
    fa_envelope_open_o = &HF2B7
    fa_envelope_square = &HF199
    fa_envira = &HF299
    fa_eraser = &HF12D
    fa_etsy = &HF2D7
    fa_eur = &HF153
    fa_euro = &HF153
    fa_exchange = &HF0EC
    fa_exclamation = &HF12A
    fa_exclamation_circle = &HF06A
    fa_exclamation_triangle = &HF071
    fa_expand = &HF065
    fa_expeditedssl = &HF23E
    fa_external_link = &HF08E
    fa_external_link_square = &HF14C
    fa_eye = &HF06E
    fa_eye_slash = &HF070
    fa_eyedropper = &HF1FB
    fa_fa = &HF2B4
    fa_facebook = &HF09A
    fa_facebook_f = &HF09A
    fa_facebook_official = &HF230
    fa_facebook_square = &HF082
    fa_fast_backward = &HF049
    fa_fast_forward = &HF050
    fa_fax = &HF1AC
    fa_feed = &HF09E
    fa_female = &HF182
    fa_fighter_jet = &HF0FB
    fa_file = &HF15B
    fa_file_archive_o = &HF1C6
    fa_file_audio_o = &HF1C7
    fa_file_code_o = &HF1C9
    fa_file_excel_o = &HF1C3
    fa_file_image_o = &HF1C5
    fa_file_movie_o = &HF1C8
    fa_file_o = &HF016
    fa_file_pdf_o = &HF1C1
    fa_file_photo_o = &HF1C5
    fa_file_picture_o = &HF1C5
    fa_file_powerpoint_o = &HF1C4
    fa_file_sound_o = &HF1C7
    fa_file_text = &HF15C
    fa_file_text_o = &HF0F6
    fa_file_video_o = &HF1C8
    fa_file_word_o = &HF1C2
    fa_file_zip_o = &HF1C6
    fa_files_o = &HF0C5
    fa_film = &HF008
    fa_filter = &HF0B0
    fa_fire = &HF06D
    fa_fire_extinguisher = &HF134
    fa_firefox = &HF269
    fa_first_order = &HF2B0
    fa_flag = &HF024
    fa_flag_checkered = &HF11E
    fa_flag_o = &HF11D
    fa_flash = &HF0E7
    fa_flask = &HF0C3
    fa_flickr = &HF16E
    fa_floppy_o = &HF0C7
    fa_folder = &HF07B
    fa_folder_o = &HF114
    fa_folder_open = &HF07C
    fa_folder_open_o = &HF115
    fa_font = &HF031
    fa_font_awesome = &HF2B4
    fa_fonticons = &HF280
    fa_fort_awesome = &HF286
    fa_forumbee = &HF211
    fa_forward = &HF04E
    fa_foursquare = &HF180
    fa_free_code_camp = &HF2C5
    fa_frown_o = &HF119
    fa_futbol_o = &HF1E3
    fa_gamepad = &HF11B
    fa_gavel = &HF0E3
    fa_gbp = &HF154
    fa_ge = &HF1D1
    fa_gear = &HF013
    fa_gears = &HF085
    fa_genderless = &HF22D
    fa_get_pocket = &HF265
    fa_gg = &HF260
    fa_gg_circle = &HF261
    fa_gift = &HF06B
    fa_git = &HF1D3
    fa_git_square = &HF1D2
    fa_github = &HF09B
    fa_github_alt = &HF113
    fa_github_square = &HF092
    fa_gitlab = &HF296
    fa_gittip = &HF184
    fa_glass = &HF000
    fa_glide = &HF2A5
    fa_glide_g = &HF2A6
    fa_globe = &HF0AC
    fa_google = &HF1A0
    fa_google_plus = &HF0D5
    fa_google_plus_circle = &HF2B3
    fa_google_plus_official = &HF2B3
    fa_google_plus_square = &HF0D4
    fa_google_wallet = &HF1EE
    fa_graduation_cap = &HF19D
    fa_gratipay = &HF184
    fa_grav = &HF2D6
    fa_group = &HF0C0
    fa_h_square = &HF0FD
    fa_hacker_news = &HF1D4
    fa_hand_grab_o = &HF255
    fa_hand_lizard_o = &HF258
    fa_hand_o_down = &HF0A7
    fa_hand_o_left = &HF0A5
    fa_hand_o_right = &HF0A4
    fa_hand_o_up = &HF0A6
    fa_hand_paper_o = &HF256
    fa_hand_peace_o = &HF25B
    fa_hand_pointer_o = &HF25A
    fa_hand_rock_o = &HF255
    fa_hand_scissors_o = &HF257
    fa_hand_spock_o = &HF259
    fa_hand_stop_o = &HF256
    fa_handshake_o = &HF2B5
    fa_hard_of_hearing = &HF2A4
    fa_hashtag = &HF292
    fa_hdd_o = &HF0A0
    fa_header = &HF1DC
    fa_headphones = &HF025
    fa_heart = &HF004
    fa_heart_o = &HF08A
    fa_heartbeat = &HF21E
    fa_history = &HF1DA
    fa_home = &HF015
    fa_hospital_o = &HF0F8
    fa_hotel = &HF236
    fa_hourglass = &HF254
    fa_hourglass_1 = &HF251
    fa_hourglass_2 = &HF252
    fa_hourglass_3 = &HF253
    fa_hourglass_end = &HF253
    fa_hourglass_half = &HF252
    fa_hourglass_o = &HF250
    fa_hourglass_start = &HF251
    fa_houzz = &HF27C
    fa_html5 = &HF13B
    fa_i_cursor = &HF246
    fa_id_badge = &HF2C1
    fa_id_card = &HF2C2
    fa_id_card_o = &HF2C3
    fa_ils = &HF20B
    fa_image = &HF03E
    fa_imdb = &HF2D8
    fa_inbox = &HF01C
    fa_indent = &HF03C
    fa_industry = &HF275
    fa_info = &HF129
    fa_info_circle = &HF05A
    fa_inr = &HF156
    fa_instagram = &HF16D
    fa_institution = &HF19C
    fa_internet_explorer = &HF26B
    fa_intersex = &HF224
    fa_ioxhost = &HF208
    fa_italic = &HF033
    fa_joomla = &HF1AA
    fa_jpy = &HF157
    fa_jsfiddle = &HF1CC
    fa_key = &HF084
    fa_keyboard_o = &HF11C
    fa_krw = &HF159
    fa_language = &HF1AB
    fa_laptop = &HF109
    fa_lastfm = &HF202
    fa_lastfm_square = &HF203
    fa_leaf = &HF06C
    fa_leanpub = &HF212
    fa_legal = &HF0E3
    fa_lemon_o = &HF094
    fa_level_down = &HF149
    fa_level_up = &HF148
    fa_life_bouy = &HF1CD
    fa_life_buoy = &HF1CD
    fa_life_ring = &HF1CD
    fa_life_saver = &HF1CD
    fa_lightbulb_o = &HF0EB
    fa_line_chart = &HF201
    fa_link = &HF0C1
    fa_linkedin = &HF0E1
    fa_linkedin_square = &HF08C
    fa_linode = &HF2B8
    fa_linux = &HF17C
    fa_list = &HF03A
    fa_list_alt = &HF022
    fa_list_ol = &HF0CB
    fa_list_ul = &HF0CA
    fa_location_arrow = &HF124
    fa_lock = &HF023
    fa_long_arrow_down = &HF175
    fa_long_arrow_left = &HF177
    fa_long_arrow_right = &HF178
    fa_long_arrow_up = &HF176
    fa_low_vision = &HF2A8
    fa_magic = &HF0D0
    fa_magnet = &HF076
    fa_mail_forward = &HF064
    fa_mail_reply = &HF112
    fa_mail_reply_all = &HF122
    fa_male = &HF183
    fa_map = &HF279
    fa_map_marker = &HF041
    fa_map_o = &HF278
    fa_map_pin = &HF276
    fa_map_signs = &HF277
    fa_mars = &HF222
    fa_mars_double = &HF227
    fa_mars_stroke = &HF229
    fa_mars_stroke_h = &HF22B
    fa_mars_stroke_v = &HF22A
    fa_maxcdn = &HF136
    fa_meanpath = &HF20C
    fa_medium = &HF23A
    fa_medkit = &HF0FA
    fa_meetup = &HF2E0
    fa_meh_o = &HF11A
    fa_mercury = &HF223
    fa_microchip = &HF2DB
    fa_microphone = &HF130
    fa_microphone_slash = &HF131
    fa_minus = &HF068
    fa_minus_circle = &HF056
    fa_minus_square = &HF146
    fa_minus_square_o = &HF147
    fa_mixcloud = &HF289
    fa_mobile = &HF10B
    fa_mobile_phone = &HF10B
    fa_modx = &HF285
    fa_money = &HF0D6
    fa_moon_o = &HF186
    fa_mortar_board = &HF19D
    fa_motorcycle = &HF21C
    fa_mouse_pointer = &HF245
    fa_music = &HF001
    fa_navicon = &HF0C9
    fa_neuter = &HF22C
    fa_newspaper_o = &HF1EA
    fa_object_group = &HF247
    fa_object_ungroup = &HF248
    fa_odnoklassniki = &HF263
    fa_odnoklassniki_square = &HF264
    fa_opencart = &HF23D
    fa_openid = &HF19B
    fa_opera = &HF26A
    fa_optin_monster = &HF23C
    fa_outdent = &HF03B
    fa_pagelines = &HF18C
    fa_paint_brush = &HF1FC
    fa_paper_plane = &HF1D8
    fa_paper_plane_o = &HF1D9
    fa_paperclip = &HF0C6
    fa_paragraph = &HF1DD
    fa_paste = &HF0EA
    fa_pause = &HF04C
    fa_pause_circle = &HF28B
    fa_pause_circle_o = &HF28C
    fa_paw = &HF1B0
    fa_paypal = &HF1ED
    fa_pencil = &HF040
    fa_pencil_square = &HF14B
    fa_pencil_square_o = &HF044
    fa_percent = &HF295
    fa_phone = &HF095
    fa_phone_square = &HF098
    fa_photo = &HF03E
    fa_picture_o = &HF03E
    fa_pie_chart = &HF200
    fa_pied_piper = &HF2AE
    fa_pied_piper_alt = &HF1A8
    fa_pied_piper_pp = &HF1A7
    fa_pinterest = &HF0D2
    fa_pinterest_p = &HF231
    fa_pinterest_square = &HF0D3
    fa_plane = &HF072
    fa_play = &HF04B
    fa_play_circle = &HF144
    fa_play_circle_o = &HF01D
    fa_plug = &HF1E6
    fa_plus = &HF067
    fa_plus_circle = &HF055
    fa_plus_square = &HF0FE
    fa_plus_square_o = &HF196
    fa_podcast = &HF2CE
    fa_power_off = &HF011
    fa_print = &HF02F
    fa_product_hunt = &HF288
    fa_puzzle_piece = &HF12E
    fa_qq = &HF1D6
    fa_qrcode = &HF029
    fa_question = &HF128
    fa_question_circle = &HF059
    fa_question_circle_o = &HF29C
    fa_quora = &HF2C4
    fa_quote_left = &HF10D
    fa_quote_right = &HF10E
    fa_ra = &HF1D0
    fa_random = &HF074
    fa_ravelry = &HF2D9
    fa_rebel = &HF1D0
    fa_recycle = &HF1B8
    fa_reddit = &HF1A1
    fa_reddit_alien = &HF281
    fa_reddit_square = &HF1A2
    fa_refresh = &HF021
    fa_registered = &HF25D
    fa_remove = &HF00D
    fa_renren = &HF18B
    fa_reorder = &HF0C9
    fa_repeat = &HF01E
    fa_reply = &HF112
    fa_reply_all = &HF122
    fa_resistance = &HF1D0
    fa_retweet = &HF079
    fa_rmb = &HF157
    fa_road = &HF018
    fa_rocket = &HF135
    fa_rotate_left = &HF0E2
    fa_rotate_right = &HF01E
    fa_rouble = &HF158
    fa_rss = &HF09E
    fa_rss_square = &HF143
    fa_rub = &HF158
    fa_ruble = &HF158
    fa_rupee = &HF156
    fa_s15 = &HF2CD
    fa_safari = &HF267
    fa_save = &HF0C7
    fa_scissors = &HF0C4
    fa_scribd = &HF28A
    fa_search = &HF002
    fa_search_minus = &HF010
    fa_search_plus = &HF00E
    fa_sellsy = &HF213
    fa_send = &HF1D8
    fa_send_o = &HF1D9
    fa_server = &HF233
    fa_share = &HF064
    fa_share_alt = &HF1E0
    fa_share_alt_square = &HF1E1
    fa_share_square = &HF14D
    fa_share_square_o = &HF045
    fa_shekel = &HF20B
    fa_sheqel = &HF20B
    fa_shield = &HF132
    fa_ship = &HF21A
    fa_shirtsinbulk = &HF214
    fa_shopping_bag = &HF290
    fa_shopping_basket = &HF291
    fa_shopping_cart = &HF07A
    fa_shower = &HF2CC
    fa_sign_in = &HF090
    fa_sign_language = &HF2A7
    fa_sign_out = &HF08B
    fa_signal = &HF012
    fa_signing = &HF2A7
    fa_simplybuilt = &HF215
    fa_sitemap = &HF0E8
    fa_skyatlas = &HF216
    fa_skype = &HF17E
    fa_slack = &HF198
    fa_sliders = &HF1DE
    fa_slideshare = &HF1E7
    fa_smile_o = &HF118
    fa_snapchat = &HF2AB
    fa_snapchat_ghost = &HF2AC
    fa_snapchat_square = &HF2AD
    fa_snowflake_o = &HF2DC
    fa_soccer_ball_o = &HF1E3
    fa_sort = &HF0DC
    fa_sort_alpha_asc = &HF15D
    fa_sort_alpha_desc = &HF15E
    fa_sort_amount_asc = &HF160
    fa_sort_amount_desc = &HF161
    fa_sort_asc = &HF0DE
    fa_sort_desc = &HF0DD
    fa_sort_down = &HF0DD
    fa_sort_numeric_asc = &HF162
    fa_sort_numeric_desc = &HF163
    fa_sort_up = &HF0DE
    fa_soundcloud = &HF1BE
    fa_space_shuttle = &HF197
    fa_spinner = &HF110
    fa_spoon = &HF1B1
    fa_spotify = &HF1BC
    fa_square = &HF0C8
    fa_square_o = &HF096
    fa_stack_exchange = &HF18D
    fa_stack_overflow = &HF16C
    fa_star = &HF005
    fa_star_half = &HF089
    fa_star_half_empty = &HF123
    fa_star_half_full = &HF123
    fa_star_half_o = &HF123
    fa_star_o = &HF006
    fa_steam = &HF1B6
    fa_steam_square = &HF1B7
    fa_step_backward = &HF048
    fa_step_forward = &HF051
    fa_stethoscope = &HF0F1
    fa_sticky_note = &HF249
    fa_sticky_note_o = &HF24A
    fa_stop = &HF04D
    fa_stop_circle = &HF28D
    fa_stop_circle_o = &HF28E
    fa_street_view = &HF21D
    fa_strikethrough = &HF0CC
    fa_stumbleupon = &HF1A4
    fa_stumbleupon_circle = &HF1A3
    fa_subscript = &HF12C
    fa_subway = &HF239
    fa_suitcase = &HF0F2
    fa_sun_o = &HF185
    fa_superpowers = &HF2DD
    fa_superscript = &HF12B
    fa_support = &HF1CD
    fa_table = &HF0CE
    fa_tablet = &HF10A
    fa_tachometer = &HF0E4
    fa_tag = &HF02B
    fa_tags = &HF02C
    fa_tasks = &HF0AE
    fa_taxi = &HF1BA
    fa_telegram = &HF2C6
    fa_television = &HF26C
    fa_tencent_weibo = &HF1D5
    fa_terminal = &HF120
    fa_text_height = &HF034
    fa_text_width = &HF035
    fa_th = &HF00A
    fa_th_large = &HF009
    fa_th_list = &HF00B
    fa_themeisle = &HF2B2
    fa_thermometer = &HF2C7
    fa_thermometer_0 = &HF2CB
    fa_thermometer_1 = &HF2CA
    fa_thermometer_2 = &HF2C9
    fa_thermometer_3 = &HF2C8
    fa_thermometer_4 = &HF2C7
    fa_thermometer_empty = &HF2CB
    fa_thermometer_full = &HF2C7
    fa_thermometer_half = &HF2C9
    fa_thermometer_quarter = &HF2CA
    fa_thermometer_three_quarters = &HF2C8
    fa_thumb_tack = &HF08D
    fa_thumbs_down = &HF165
    fa_thumbs_o_down = &HF088
    fa_thumbs_o_up = &HF087
    fa_thumbs_up = &HF164
    fa_ticket = &HF145
    fa_times = &HF00D
    fa_times_circle = &HF057
    fa_times_circle_o = &HF05C
    fa_times_rectangle = &HF2D3
    fa_times_rectangle_o = &HF2D4
    fa_tint = &HF043
    fa_toggle_down = &HF150
    fa_toggle_left = &HF191
    fa_toggle_off = &HF204
    fa_toggle_on = &HF205
    fa_toggle_right = &HF152
    fa_toggle_up = &HF151
    fa_trademark = &HF25C
    fa_train = &HF238
    fa_transgender = &HF224
    fa_transgender_alt = &HF225
    fa_trash = &HF1F8
    fa_trash_o = &HF014
    fa_tree = &HF1BB
    fa_trello = &HF181
    fa_tripadvisor = &HF262
    fa_trophy = &HF091
    fa_truck = &HF0D1
    fa_try = &HF195
    fa_tty = &HF1E4
    fa_tumblr = &HF173
    fa_tumblr_square = &HF174
    fa_turkish_lira = &HF195
    fa_tv = &HF26C
    fa_twitch = &HF1E8
    fa_twitter = &HF099
    fa_twitter_square = &HF081
    fa_umbrella = &HF0E9
    fa_underline = &HF0CD
    fa_undo = &HF0E2
    fa_universal_access = &HF29A
    fa_university = &HF19C
    fa_unlink = &HF127
    fa_unlock = &HF09C
    fa_unlock_alt = &HF13E
    fa_unsorted = &HF0DC
    fa_upload = &HF093
    fa_usb = &HF287
    fa_usd = &HF155
    fa_user = &HF007
    fa_user_circle = &HF2BD
    fa_user_circle_o = &HF2BE
    fa_user_md = &HF0F0
    fa_user_o = &HF2C0
    fa_user_plus = &HF234
    fa_user_secret = &HF21B
    fa_user_times = &HF235
    fa_users = &HF0C0
    fa_vcard = &HF2BB
    fa_vcard_o = &HF2BC
    fa_venus = &HF221
    fa_venus_double = &HF226
    fa_venus_mars = &HF228
    fa_viacoin = &HF237
    fa_viadeo = &HF2A9
    fa_viadeo_square = &HF2AA
    fa_video_camera = &HF03D
    fa_vimeo = &HF27D
    fa_vimeo_square = &HF194
    fa_vine = &HF1CA
    fa_vk = &HF189
    fa_volume_control_phone = &HF2A0
    fa_volume_down = &HF027
    fa_volume_off = &HF026
    fa_volume_up = &HF028
    fa_warning = &HF071
    fa_wechat = &HF1D7
    fa_weibo = &HF18A
    fa_weixin = &HF1D7
    fa_whatsapp = &HF232
    fa_wheelchair = &HF193
    fa_wheelchair_alt = &HF29B
    fa_wifi = &HF1EB
    fa_wikipedia_w = &HF266
    fa_window_close = &HF2D3
    fa_window_close_o = &HF2D4
    fa_window_maximize = &HF2D0
    fa_window_minimize = &HF2D1
    fa_window_restore = &HF2D2
    fa_windows = &HF17A
    fa_won = &HF159
    fa_wordpress = &HF19A
    fa_wpbeginner = &HF297
    fa_wpexplorer = &HF2DE
    fa_wpforms = &HF298
    fa_wrench = &HF0AD
    fa_xing = &HF168
    fa_xing_square = &HF169
    fa_y_combinator = &HF23B
    fa_y_combinator_square = &HF1D4
    fa_yahoo = &HF19E
    fa_yc = &HF23B
    fa_yc_square = &HF1D4
    fa_yelp = &HF1E9
    fa_yen = &HF157
    fa_yoast = &HF2B1
    fa_youtube = &HF167
    fa_youtube_play = &HF16A
    fa_youtube_square = &HF166
End Enum

Public Enum DefaultSubtitleMode
    None
    [Single]
    First
    Second
    English
    Native
End Enum

Public Class Attachment
    Property ID As Integer
    Property Name As String
    Property Enabled As Boolean = True
End Class

Public Enum FileExistMode
    Ask
    Overwrite
    Skip
End Enum

Public Enum DeleteMode
    Disabled
    <DispName("Recycle Bin")> RecycleBin
    Permanent
End Enum

Public Enum QuotesMode
    Auto
    Always
    Never
End Enum

Public Enum ApplicationEvent
    <DispName("After Job Muxed")> JobMuxed
    <DispName("After Job Processed")> JobProcessed
    <DispName("After Jobs Processed")> JobsProcessed
    <DispName("After Project Loaded")> ProjectLoaded
    <DispName("After Project Or Source Loaded")> ProjectOrSourceLoaded
    <DispName("After Source Loaded")> AfterSourceLoaded
    <DispName("After Video Encoded")> VideoEncoded
    <DispName("Application Exit")> ApplicationExit
    <DispName("Before Job Processed")> BeforeJobProcessed
    <DispName("Before Processing")> BeforeProcessing
End Enum
