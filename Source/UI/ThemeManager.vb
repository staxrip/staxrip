﻿Imports Microsoft.Win32
Imports StaxRip.SimpleUI

Public NotInheritable Class ThemeManager
    Private Const _defaultThemeName = "Darker | Seagreen"
    Private Const _defaultProgressHighlightingColorName = "Teal"
    Private Const _windowsAccentColorName = "Windows Accent Color"

    Private Shared _current As Theme
    Private Shared _themes As List(Of Theme)
    Private Shared _windowsAccentColor As ColorHSL?

    Public Shared ReadOnly Property ColorCategories As Tuple(Of String, Integer, Integer)()
        Get
            Dim accentColors = If(WindowsAccentColor IsNot Nothing, {New Tuple(Of String, Integer, Integer)(_windowsAccentColorName, WindowsAccentColor.Value.H, -1)}, Nothing)
            Dim defaultColors = {
                New Tuple(Of String, Integer, Integer)("Red", 358, 50),
                New Tuple(Of String, Integer, Integer)("Orange", 25, 355),
                New Tuple(Of String, Integer, Integer)("Yellow", 48, 355),
                New Tuple(Of String, Integer, Integer)("YellowGreen", 80, 355),
                New Tuple(Of String, Integer, Integer)("Green", 100, 355),
                New Tuple(Of String, Integer, Integer)("Lime", 120, 355),
                New Tuple(Of String, Integer, Integer)("Seagreen", 146, 355),
                New Tuple(Of String, Integer, Integer)("TurquoiseGreen", 163, 355),
                New Tuple(Of String, Integer, Integer)("Turquoise", 174, 355),
                New Tuple(Of String, Integer, Integer)("Teal", 181, 355),
                New Tuple(Of String, Integer, Integer)("TealBlue", 194, 355),
                New Tuple(Of String, Integer, Integer)("Blue", 205, 355),
                New Tuple(Of String, Integer, Integer)("DarkBlue", 220, 355),
                New Tuple(Of String, Integer, Integer)("Indigo", 260, 355),
                New Tuple(Of String, Integer, Integer)("Violet", 271, 355),
                New Tuple(Of String, Integer, Integer)("Purple", 281, 355),
                New Tuple(Of String, Integer, Integer)("Magenta", 292, 355),
                New Tuple(Of String, Integer, Integer)("Pink", 313, 10),
                New Tuple(Of String, Integer, Integer)("Rose", 340, 40)
            }

            Return accentColors.Concat(defaultColors).ToArray()
        End Get
    End Property


    Public Shared ReadOnly Property LumaCategories As KeyValuePair(Of String, Single)() = {
        New KeyValuePair(Of String, Single)("Black", 0.01),
        New KeyValuePair(Of String, Single)("Almost Black", 0.04),
        New KeyValuePair(Of String, Single)("Darker", 0.065),
        New KeyValuePair(Of String, Single)("Dark", 0.09),
        New KeyValuePair(Of String, Single)("Not So Dark", 0.11),
        New KeyValuePair(Of String, Single)("Almost Dark", 0.16),
        New KeyValuePair(Of String, Single)("Dark Gray", 0.21),
        New KeyValuePair(Of String, Single)("Gray", 0.26),
        New KeyValuePair(Of String, Single)("Light Gray", 0.33)
    }

    Public Shared ReadOnly Property CurrentTheme As Theme
        Get
            If _current Is Nothing Then
                If s.ThemeName IsNot Nothing AndAlso Themes.Any(Function(theme) String.Equals(theme.Name, s.ThemeName, StringComparison.InvariantCultureIgnoreCase)) Then
                    SetCurrentTheme(s.ThemeName)
                Else
                    Dim newTheme = SetCurrentTheme()
                    s.ThemeName = newTheme.Name
                End If
            End If

            Return _current
        End Get
    End Property

    Public Shared ReadOnly Property DefaultThemeName As String
        Get
            Return _defaultThemeName
        End Get
    End Property

    Public Shared ReadOnly Property DefaultProgressHighlightingColorName As String
        Get
            Return _defaultProgressHighlightingColorName
        End Get
    End Property

    Public Shared ReadOnly Property Themes As List(Of Theme)
        Get
            _themes = If(_themes, LoadDefaults())
            Return _themes
        End Get
    End Property

    Public Shared ReadOnly Property WindowsAccentColor As ColorHSL?
        Get
            If _windowsAccentColor Is Nothing Then
                Try
                    Dim value = Registry.CurrentUser.GetLong("SOFTWARE\Microsoft\Windows\DWM", "AccentColor")
                    Dim r = CInt((value >> 0) And &HFF)
                    Dim g = CInt((value >> 8) And &HFF)
                    Dim b = CInt((value >> 16) And &HFF)
                    Dim a = CInt((value >> 24) And &HFF)
                    _windowsAccentColor = Color.FromArgb(a, r, g, b)
                Catch ex As Exception
                    _windowsAccentColor = Nothing
                End Try
            End If
            Return _windowsAccentColor
        End Get
    End Property

    <CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification:="<Pending>")>
    Private Sub ThemeManager()
    End Sub


    Private Shared Function LoadDefaults() As List(Of Theme)
        Dim defaults = New List(Of Theme) From {
            New Theme("System Colors")
        }

        For Each lc In LumaCategories
            For Each cc In ColorCategories
                defaults.Add(New DarkTheme($"{lc.Key} | {cc.Item1}", cc.Item2, cc.Item3, lc.Value))
            Next
        Next

        Return defaults
    End Function

    Public Shared Function SetCurrentTheme(Optional name As String = _defaultThemeName) As Theme
        If String.IsNullOrWhiteSpace(name) Then name = _defaultThemeName

        Dim theme = Themes?.Where(Function(x) x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()
        If theme IsNot Nothing Then
            If Not theme.Equals(_current) Then
                _current = theme
                OnCurrentThemeChanged()
            End If
        End If
        Return _current
    End Function

    Public Shared Event CurrentThemeChanged(theme As Theme)

    Public Shared Function OnCurrentThemeChanged() As Theme
        RaiseEvent CurrentThemeChanged(_current)
        Return _current
    End Function


    '   -----
    '   #####   #####   #####   #####   #####   #####   #####
    '   -----   -----   -----   -----   -----   -----   -----
    '   #####   #####   #####   #####   #####   #####   #####
    '   -----


    Private Class DarkTheme
        Inherits Theme

        Private ReadOnly _baseHue As Integer = 0
        Private ReadOnly _highlightHue As Integer = 0
        Private ReadOnly _backLuma As Single = 0
        Private ReadOnly _backLumaDefault As Single = 0.11
        Private ReadOnly _accentSat As Single = 0
        Private ReadOnly _accentSatDefault As Single = 0.55

        Public Sub New(name As String, Optional hue As Integer = 200, Optional highlightHue As Integer = -1, Optional backLuma As Single = 0.11)
            MyBase.New(name)
            MyBase._usesSystemColors = False

            _baseHue = hue
            _highlightHue = Mathf.Clamp(If(highlightHue >= 0, highlightHue, If(_baseHue - 180 < 0, _baseHue + 180, _baseHue - 180)), 0, 359)
            _backLuma = Mathf.Clamp01(backLuma)
            _accentSat = _accentSatDefault

            Dim _backColor As ColorHSL = New ColorHSL(_baseHue, 0.01, _backLuma, 1)
            Dim _foreColor As ColorHSL = New ColorHSL(_baseHue, 0.03, 0.7D - (_backLumaDefault - _backLuma), 1)
            Dim _accentColor As ColorHSL = New ColorHSL(_baseHue, _accentSat, 0.5025D - (_backLumaDefault - _backLuma / 2), 1)

            Dim _backSelectedColor As ColorHSL = New ColorHSL(_baseHue, _accentColor.S - _backLuma / 1.5D, _accentColor.L - _backLumaDefault * 1.75D, 1)

            Dim _controlBackColor As ColorHSL = _backColor.AddLuminance(0.025)
            Dim _controlBackHighlightColor As ColorHSL = New ColorHSL(_highlightHue, 1 - _backLuma * 1.5D, _controlBackColor.L + 0.125D, 1)
            Dim _controlBackSelectedColor As ColorHSL = _controlBackColor.AddSaturation(0.25).AddLuminance(0.25)
            Dim _controlBackReadonlyColor As ColorHSL = _backColor.AddLuminance(-0.1)
            Dim _controlForeHighlightColor As ColorHSL = _foreColor.AddLuminance(0.2)
            Dim _controlForeSelectedColor As ColorHSL = _foreColor.AddLuminance(0.2)

            Dim _foreHighlightColor As ColorHSL = _accentColor.SetHue(_highlightHue).AddSaturation(0.5)
            Dim _foreReadonlyColor As ColorHSL = _foreColor.AddLuminance(-0.2)

            Dim _borderColor As ColorHSL = _controlBackColor.AddLuminance(0.15)
            Dim _borderFocusedColor As ColorHSL = _borderColor.AddSaturation(0.5).AddLuminance(0.33)
            Dim _borderHoverColor As ColorHSL = _borderFocusedColor.AddSaturation(0.1).AddLuminance(0.1)

            Dim _dangerBackColor As ColorHSL = _accentColor.AddHue(150).AddLuminance(-0.25)
            Dim _dangerForeColor As ColorHSL = _foreHighlightColor

            Dim _outputHighlightingBackColor As ColorHSL = New ColorHSL(_baseHue, _backSelectedColor.S - _backLuma / 4, 0.255D - _backLumaDefault + _backLuma / 1.75D, 1)
            Dim _outputHighlightingForeColor As ColorHSL = New ColorHSL(_baseHue, 0.66D - _backLumaDefault + _backLuma * 1.5D, 0.94D - _backLumaDefault)
            Dim _outputHighlightingStrongForeColor As ColorHSL = New ColorHSL(_baseHue, 0.66D + _backLuma / 2, 0.6D - _backLumaDefault + _backLuma / 2, 1)

            General = New GeneralThemeColors() With {
                .BackColor = _backColor,
                .ControlBackColor = _controlBackColor,
                .DangerBackColor = Color.Transparent,
                .DangerForeColor = _dangerForeColor,
                .ForeColor = _foreColor,
                .Controls = New ControlsThemeColors() With {
                    .Button = New ControlsThemeColors.ButtonThemeColors() With {
                        .BackColor = _controlBackColor.AddLuminance(0.1),
                        .BackDisabledColor = .BackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BorderColor = .BackColor.AddLuminance(0.05),
                        .ForeColor = _foreColor.AddLuminance(0.1),
                        .ForeDisabledColor = .ForeColor.AddLuminance(-0.33),
                        .ForeHighlightColor = .ForeColor.AddLuminance(0.15)
                    },
                    .ButtonLabel = New ControlsThemeColors.ButtonLabelThemeColors() With {
                        .BackColor = _backColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _accentColor.AddLuminance(0.1),
                        .ForeHighlightColor = _controlForeHighlightColor,
                        .LinkForeColor = .ForeColor,
                        .LinkForeHoverColor = .LinkForeColor.SetSaturation(1).AddLuminance(0.1)
                    },
                    .CheckBox = New ControlsThemeColors.CheckBoxThemeColors() With {
                        .BackColor = Color.Empty,
                        .BackCheckedColor = .BackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackHoverColor = .BackColor.AddLuminance(0.1),
                        .BorderColor = _foreColor.AddLuminance(-0.2),
                        .BorderCheckedColor = .BorderColor,
                        .BoxColor = _foreColor,
                        .BoxCheckedColor = .BoxColor,
                        .CheckedBackColor = _backColor,
                        .CheckmarkColor = _controlBackColor,
                        .ForeColor = _foreColor,
                        .ForeCheckedColor = .ForeColor.AddLuminance(0.1F),
                        .ForeDisabledColor = .ForeColor.AddLuminance(-0.2F),
                        .ForeHighlightColor = _controlForeHighlightColor.AddLuminance(0.15)
                    },
                    .ComboBox = New ControlsThemeColors.ComboBoxThemeColors() With {
                        .BackColor = _backColor.AddLuminance(0.1),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BorderColor = Color.Transparent,
                        .FlatStyle = FlatStyle.Flat,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = .ForeColor.AddLuminance(0.15)
                    },
                    .CommandLineRichTextBox = New ControlsThemeColors.CommandLineRichTextBoxThemeColors() With {
                        .ParameterBackColor = _controlBackColor,
                        .ParameterForeColor = _accentColor.SetHue(180).AddSaturation(-0.2).AddLuminance(0.2),
                        .ParameterFontStyles = {},
                        .ParameterValueBackColor = .ParameterBackColor,
                        .ParameterValueForeColor = .ParameterForeColor.SetHue(48),
                        .ParameterValueFontStyles = {}
                    },
                    .CriteriaControl = New ControlsThemeColors.CriteriaControlThemeColors() With {
                        .BackColor = _controlBackColor,
                        .ForeColor = _foreColor
                    },
                    .FlowLayoutPanel = New ControlsThemeColors.FlowLayoutPanelThemeColors() With {
                        .BackColor = Color.Transparent,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .FlowPage = New ControlsThemeColors.FlowPageThemeColors() With {
                        .BackColor = Color.Transparent,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .GridView = New ControlsThemeColors.GridviewThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor.AddLuminance(-1),
                        .ForeHighlightColor = _controlForeHighlightColor,
                        .GridColor = _foreColor
                    },
                    .GroupBox = New ControlsThemeColors.GroupBoxThemeColors() With {
                        .BackColor = Color.Transparent,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .Label = New ControlsThemeColors.LabelThemeColors() With {
                        .BackColor = Color.Transparent,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .LabelProgressBar = New ControlsThemeColors.LabelProgressBarThemeColors() With {
                        .BackColor = _controlBackColor,
                        .ForeColor = _foreColor.AddLuminance(0.1),
                        .ProgressColor = _backSelectedColor.AddSaturation(0.25).AddLuminance(-0.125)
                    },
                    .Line = New ControlsThemeColors.LineThemeColors() With {
                        .BackColor = Color.Transparent,
                        .ForeColor = _foreColor,
                        .LineColor1 = .ForeColor,
                        .LineColor2 = .ForeColor.AddLuminance(-0.2)
                    },
                    .ListBox = New ControlsThemeColors.ListBoxThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackAlternateColor = .BackColor.AddLuminance(0.025),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackSelectedColor = _backSelectedColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor,
                        .ForeSelectedColor = _foreColor.AddLuminance(0.25),
                        .SymbolImageColor = .ForeColor
                    },
                    .ListView = New ControlsThemeColors.ListViewThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackAlternateColor = .BackColor.AddLuminance(0.025),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackSelectedColor = _backSelectedColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor,
                        .ForeSelectedColor = _foreColor.AddLuminance(0.25),
                        .SeparatorColor = .BackColor.AddLuminance(0.1),
                        .SymbolImageColor = .ForeColor
                    },
                    .NumEdit = New ControlsThemeColors.NumEditThemeColors() With {
                        .BackColor = _controlBackColor.AddLuminance(0.1),
                        .BorderColor = .BackColor,
                        .BorderSelectedColor = .BackColor,
                        .UpDownButton = New ControlsThemeColors.NumEditThemeColors.UpDownButtonThemeColors() With {
                            .BackColor = _backSelectedColor,
                            .BackHotColor = .BackColor.AddLuminance(0.05),
                            .BackPressedColor = .BackColor.AddLuminance(0.15),
                            .BorderColor = _backColor,
                            .ForeColor = _foreColor.AddLuminance(0.2),
                            .ForeDisabledColor = .ForeColor.AddLuminance(-0.15)
                        }
                    },
                    .Panel = New ControlsThemeColors.PanelThemeColors() With {
                        .BackColor = Color.Transparent,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .PropertyGrid = New ControlsThemeColors.PropertyGridThemeColors() With {
                        .BackColor = _controlBackColor,
                        .CategoryForeColor = _foreColor,
                        .CategorySplitterColor = _foreColor,
                        .CommandsActiveLinkColor = _accentColor,
                        .CommandsBackColor = _controlBackColor,
                        .CommandsBorderColor = _foreColor,
                        .CommandsDisabledLinkColor = _controlBackReadonlyColor,
                        .CommandsForeColor = _foreColor,
                        .CommandsLinkColor = _accentColor,
                        .DisabledItemForeColor = _controlBackReadonlyColor,
                        .ForeColor = _foreColor,
                        .HelpBackColor = _controlBackColor,
                        .HelpBorderColor = _foreColor,
                        .HelpForeColor = _foreColor,
                        .LineColor = _controlBackColor,
                        .SelectedItemWithFocusBackColor = _backSelectedColor,
                        .SelectedItemWithFocusForeColor = _foreColor,
                        .ViewBackColor = _backColor,
                        .ViewBorderColor = _foreColor,
                        .ViewForeColor = _foreColor
                    },
                    .RichTextBox = New ControlsThemeColors.RichTextBoxThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackReadonlyColor = _controlBackReadonlyColor,
                        .BorderColor = _borderColor,
                        .BorderFocusedColor = _borderFocusedColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .TableLayoutPanel = New ControlsThemeColors.TableLayoutPanelThemeColors() With {
                        .BackColor = _backColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .TabControl = New ControlsThemeColors.TabControlThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BorderColor = _controlBackColor.AddLuminance(-0.1),
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .TabPage = New ControlsThemeColors.TabPageThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .TextBox = New ControlsThemeColors.TextBoxThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackReadonlyColor = Color.Yellow,
                        .BorderColor = _borderColor,
                        .BorderFocusedColor = .BorderColor.AddSaturation(0.2).AddLuminance(0.1),
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .TextEdit = New ControlsThemeColors.TextEditThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BorderColor = _borderColor,
                        .BorderFocusedColor = .BorderColor.AddSaturation(0.3).AddLuminance(0.15),
                        .BorderHoverColor = .BorderFocusedColor.AddSaturation(0.15).AddLuminance(0.075),
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .ToolStrip = New ControlsThemeColors.ToolStripThemeColors() With {
                        .MenuStripBackgroundDefaultColor = _controlBackColor.AddLuminance(-0.01),
                        .MenuStripBackgroundSelectedColor = _backSelectedColor,
                        .MenuStripTextDefaultColor = _foreColor.AddLuminance(0.1),
                        .MenuStripTextSelectedColor = _foreColor.AddLuminance(0.2),
                        .DropdownBackgroundDefaultColor = .MenuStripBackgroundDefaultColor.AddLuminance(0.025),
                        .DropdownBackgroundSelectedColor = .MenuStripBackgroundSelectedColor,
                        .DropdownTextDefaultColor = .MenuStripTextDefaultColor,
                        .DropdownTextSelectedColor = .MenuStripTextSelectedColor,
                        .SymbolImageColor = .DropdownTextDefaultColor,
                        .BoxColor = .DropdownBackgroundDefaultColor.SetSaturation(0.1).AddLuminance(0.05),
                        .BoxSelectedColor = .DropdownBackgroundSelectedColor,
                        .CheckmarkColor = .DropdownTextSelectedColor.SetSaturation(0.1).AddLuminance(-0.3),
                        .CheckmarkSelectedColor = .DropdownTextSelectedColor,
                        .BorderInnerColor = .DropdownBackgroundDefaultColor.AddLuminance(0.3),
                        .BorderOuterColor = .DropdownBackgroundDefaultColor.AddLuminance(0.15)
                    },
                    .TrackBar = New ControlsThemeColors.TrackBarThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = .ForeColor.AddLuminance(0.15)
                    },
                    .TreeView = New ControlsThemeColors.TreeViewThemeColors() With {
                        .BackColor = _controlBackColor,
                        .BackAlternateColor = .BackColor.AddLuminance(0.025),
                        .BackExpandableColor = _backSelectedColor.AddSaturation(-0.33).AddLuminance(-0.125),
                        .BackExpandedColor = _backSelectedColor.AddSaturation(-0.2).AddLuminance(-0.1),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackSelectedColor = _backSelectedColor,
                        .ForeColor = _foreColor,
                        .ForeExpandableColor = _foreColor.AddLuminance(0),
                        .ForeExpandedColor = _foreColor.AddLuminance(0),
                        .ForeHighlightColor = _controlForeHighlightColor,
                        .ForeSelectedColor = _foreColor.AddLuminance(0.25)
                    },
                    .VideoRenderer = New ControlsThemeColors.VideoRendererThemeColors() With {
                        .BackColor = _accentColor.SetSaturation(0.75).SetLuminance(0.15)
                    }
                }
            }

            AppsForm = New AppsFormThemeColors() With {
                .AttentionForeColor = _foreHighlightColor,
                .MinorForeColor = _foreHighlightColor.AddHue(30),
                .OkayForeColor = _accentColor
            }

            CodeEditor = New CodeEditorThemeColors() With {
                .BackAccentColor = _accentColor.AddSaturation(-0.2).AddLuminance(-0.41),
                .ForeColor = _foreColor.AddLuminance(-0.1),
                .ForeAccentColor = _accentColor,
                .RichTextBoxForeColor = General.Controls.RichTextBox.ForeColor.AddLuminance(-0.3),
                .RichTextBoxForeAccentColor = _accentColor,
                .RichTextBoxBorderColor = _foreColor,
                .RichTextBoxBorderAccentColor = _accentColor.AddLuminance(-0.25)
            }

            CropForm = New CropFormThemeColors() With {
                .BackColor = _backColor,
                .BorderColor = .BackColor,
                .BorderSelectedColor = _accentColor
            }

            MainForm = New MainFormThemeColors() With {
                .laTipBackColor = Color.Transparent,
                .laTipBackHighlightColor = Color.Transparent,
                .laTipForeColor = _accentColor.AddSaturation(0.1).AddLuminance(0.1),
                .laTipForeHighlightColor = _foreHighlightColor
            }

            ProcessingForm = New ProcessingFormThemeColors() With {
                .BackColor = _backColor,
                .ProcessButtonBackColor = .BackColor,
                .ProcessButtonBackSelectedColor = _backSelectedColor,
                .ProcessButtonForeColor = _foreColor,
                .ProcessButtonForeSelectedColor = _foreColor.AddLuminance(0.25),
                .OutputHighlighting = New ProcessingFormThemeColors.OutputHighlightingThemeColors() With {
                    .HeaderBackColor = General.Controls.RichTextBox.BackColor,
                    .HeaderForeColor = General.Controls.RichTextBox.ForeColor.AddLuminance(0.05),
                    .HeaderFontStyles = {FontStyle.Bold},
                                                         _
                    .EncoderTitleBackColor = General.Controls.RichTextBox.BackColor,
                    .EncoderTitleForeColor = _accentColor.AddLuminance(0.1),
                    .EncoderTitleFontStyles = {},
                                                 _
                    .ParameterBackColor = General.Controls.RichTextBox.BackColor,
                    .ParameterForeColor = _outputHighlightingForeColor.SetHue(85).AddSaturation(-0.15D + _backLuma / 4).AddLuminance(-0.1D - _backLuma / 12),
                    .ParameterFontStyles = {FontStyle.Bold},
                    .ParameterValueBackColor = .ParameterBackColor,
                    .ParameterValueForeColor = _outputHighlightingForeColor.SetHue(210).AddSaturation(0.1).AddLuminance(-0.05),
                    .ParameterValueFontStyles = {},
                                                   _
                    .ExeFileBackColor = General.Controls.RichTextBox.BackColor,
                    .ExeFileForeColor = _outputHighlightingForeColor.SetHue(20),
                    .ExeFileFontStyles = {},
                                            _
                    .PipeBackColor = General.Controls.RichTextBox.BackColor,
                    .PipeForeColor = _outputHighlightingForeColor.SetHue(0).AddLuminance(-0.2),
                    .PipeFontStyles = {FontStyle.Bold},
                                                       _
                    .MediaFileBackColor = General.Controls.RichTextBox.BackColor,
                    .MediaFileForeColor = _outputHighlightingForeColor.SetHue(160).AddLuminance(-0.15),
                    .MediaFileFontStyles = {},
                                              _
                    .MetadataFileBackColor = General.Controls.RichTextBox.BackColor,
                    .MetadataFileForeColor = _outputHighlightingForeColor.SetHue(270).AddLuminance(-0.15),
                    .MetadataFileFontStyles = {},
                                                 _
                    .ScriptFileBackColor = General.Controls.RichTextBox.BackColor,
                    .ScriptFileForeColor = _outputHighlightingForeColor.SetHue(300).AddSaturation(-0.33).AddLuminance(-0.25D + _backLuma / 2),
                    .ScriptFileFontStyles = {},
                                               _
                    .AlternateBackColor = General.Controls.RichTextBox.BackColor.AddLuminance(0.05),
                    .AlternateForeColor = _foreColor.AddLuminance(0.15),
                    .AlternateFontStyles = {},
                                              _
                    .SourceBackColor = _outputHighlightingBackColor.AddSaturation(-_backLuma / 4),
                    .SourceForeColor = _foreColor,
                                                  _
                    .InfoLabelBackColor = _outputHighlightingBackColor.SetHue(145),
                    .InfoLabelForeColor = _foreColor,
                    .InfoTextBackColor = General.Controls.RichTextBox.BackColor,
                    .InfoTextForeColor = Color.Purple,
                                                      _
                    .WarningLabelBackColor = _outputHighlightingBackColor.SetHue(350),
                    .WarningLabelForeColor = _outputHighlightingStrongForeColor.SetHue(.WarningLabelBackColor.H).AddLuminance(_backLuma / 2),
                    .WarningLabelFontStyles = {},
                    .WarningTextBackColor = General.Controls.RichTextBox.BackColor,
                    .WarningTextForeColor = _outputHighlightingStrongForeColor.SetHue(.WarningLabelBackColor.H).AddLuminance(-_backLuma / 10),
                    .WarningTextFontStyles = {FontStyle.Bold},
                                                              _
                    .FramesBackColor = General.Controls.RichTextBox.BackColor,
                    .FramesForeColor = _outputHighlightingForeColor.SetHue(140).AddLuminance(-0.05),
                    .FramesFontStyles = {},
                    .FramesCuttedBackColor = General.Controls.RichTextBox.BackColor,
                    .FramesCuttedForeColor = _outputHighlightingForeColor.SetHue(50),
                    .FramesCuttedFontStyles = {},
                    .FramesCuttedNumberBackColor = General.Controls.RichTextBox.BackColor,
                    .FramesCuttedNumberForeColor = New ColorHSL(.FramesCuttedForeColor.H - 20, .FramesCuttedForeColor.S + 0.33D, .FramesCuttedForeColor.L - 0.075D),
.FramesCuttedNumberFontStyles = {FontStyle.Bold},
                                                 _
                    .FrameServerBackColor = General.Controls.RichTextBox.BackColor,
                    .FrameServerForeColor = _outputHighlightingStrongForeColor.SetHue(300).AddSaturation(-0.2),
.FrameServerFontStyles = {FontStyle.Italic, FontStyle.Bold},
                                                            _
                    .EncoderBackColor = General.Controls.RichTextBox.BackColor,
                    .EncoderForeColor = _outputHighlightingStrongForeColor.SetHue(205).AddLuminance(0.05),
                    .EncoderFontStyles = {FontStyle.Italic, FontStyle.Bold}
                }
            }

            TaskDialog = New TaskDialogThemeColors() With {
                .BackColor = _controlBackColor.AddLuminance(-0.005),
                .ForeColor = _foreColor.AddLuminance(0.1),
                .CommandButton = New TaskDialogThemeColors.CommandButtonThemeColors() With {
                    .BackColor = _controlBackColor.AddLuminance(0.005),
                    .BorderColor = .BackColor.AddLuminance(0.05),
                    .ForeColor = _foreColor.AddLuminance(0.1)
                }
            }

            VideoComparisonForm = New VideoComparisonFormThemeColors() With {
                .BackColor = _backColor,
                .TabBackColor = .BackColor.AddLuminance(-0.05)
            }

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Class


