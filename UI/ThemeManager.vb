Public NotInheritable Class ThemeManager
    Private Shared _current As Theme
    Private Shared _themes As List(Of Theme)
    Private Shared _lumaCategories() As KeyValuePair(Of String, Single) = {
        New KeyValuePair(Of String, Single)("Black", 0.0),
        New KeyValuePair(Of String, Single)("Darker", 0.05),
        New KeyValuePair(Of String, Single)("Dark", 0.11),
        New KeyValuePair(Of String, Single)("Not So Dark", 0.16),
        New KeyValuePair(Of String, Single)("Almost Dark", 0.21),
        New KeyValuePair(Of String, Single)("Dark Gray", 0.26),
        New KeyValuePair(Of String, Single)("Gray", 0.33),
        New KeyValuePair(Of String, Single)("Light Gray", 0.41)
    }
    Private Shared _colorCategories() As Tuple(Of String, Integer, Integer) = {
        New Tuple(Of String, Integer, Integer)("Red", 358, -1),
        New Tuple(Of String, Integer, Integer)("Orange", 25, 355),
        New Tuple(Of String, Integer, Integer)("Yellow", 48, 355),
        New Tuple(Of String, Integer, Integer)("Yellow Green", 77, 355),
        New Tuple(Of String, Integer, Integer)("Green", 105, 355),
        New Tuple(Of String, Integer, Integer)("Blue Green", 146, 355),
        New Tuple(Of String, Integer, Integer)("Green Blue", 166, 355),
        New Tuple(Of String, Integer, Integer)("Turquoise", 180, 355),
        New Tuple(Of String, Integer, Integer)("Light Blue", 200, 355),
        New Tuple(Of String, Integer, Integer)("Blue", 220, 355),
        New Tuple(Of String, Integer, Integer)("Purple", 276, 355),
        New Tuple(Of String, Integer, Integer)("Purple Pink", 290, 355),
        New Tuple(Of String, Integer, Integer)("Pink", 313, 10),
        New Tuple(Of String, Integer, Integer)("Pink Red", 335, -1)
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

    Public Shared ReadOnly Property Themes As List(Of Theme)
        Get
            _themes = If(_themes, LoadDefaults())
            Return _themes
        End Get
    End Property


    Private Sub ThemeManager()
    End Sub

    Private Shared Function LoadDefaults() As List(Of Theme)
        Dim defaults = New List(Of Theme) From {
            New Theme("Default")
        }

        For Each cc In _colorCategories
            For Each lc In _lumaCategories
                defaults.Add(New DarkTheme($"{cc.Item1} | {lc.Key}", cc.Item2, cc.Item3, lc.Value))
            Next
        Next

        Return defaults
    End Function

    Public Shared Function SetCurrentTheme(Optional name As String = "Default") As Theme
        If String.IsNullOrWhiteSpace(name) Then name = "Default"

        Dim theme = Themes?.Where(Function(x) x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()
        If theme IsNot Nothing Then
            If Not theme.Equals(_current) Then
                _current = theme
                OnCurrentChanged()
            End If
        End If
        Return _current
    End Function

    Public Shared Event CurrentThemeChanged(theme As Theme)

    Public Shared Function OnCurrentChanged() As Theme
        RaiseEvent CurrentThemeChanged(_current)
        Return _current
    End Function

    Private Class DarkTheme
        Inherits Theme

        Private ReadOnly _baseHue As Integer = 0
        Private ReadOnly _highlightHue As Integer = 0
        Private ReadOnly _backLuma As Single = 0
        Private ReadOnly _backLumaDefault As Single = 0.11
        Private ReadOnly _accentSat As Single = 0
        Private ReadOnly _accentSatDefault As Single = 0.6

        Public Sub New(Optional name As String = "DarkMode", Optional hue As Integer = 200, Optional highlightHue As Integer = -1, Optional backLuma As Single = 0.11)
            MyBase.New(name)
            _baseHue = hue
            _highlightHue = Mathf.Clamp(If(highlightHue >= 0, highlightHue, If(_baseHue - 180 < 0, _baseHue + 180, _baseHue - 180)), 0, 359)
            _backLuma = Mathf.Clamp01(backLuma)
            _accentSat = _accentSatDefault - _backLuma / 2

            Dim _backColor As ColorHSL = New ColorHSL(_baseHue, 0.0333, _backLuma, 1)
            Dim _foreColor As ColorHSL = New ColorHSL(_baseHue, 0.0333, 0.7D - (_backLumaDefault - _backLuma), 1)
            Dim _accentColor As ColorHSL = New ColorHSL(_baseHue, _accentSat, 0.525D - (_backLumaDefault - _backLuma / 2), 1)

            Dim _backSelectedColor As ColorHSL = New ColorHSL(_baseHue, _accentColor.S - _backLuma / 4, _accentColor.L - 0.2D, 1)

            Dim _controlBackColor As ColorHSL = _backColor.AddLuminance(0.025)
            Dim _controlBackHighlightColor As ColorHSL = New ColorHSL(_highlightHue, 1, _controlBackColor.L + 0.1D, 1)
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
                        .LinkForeHoverColor = .LinkForeColor.AddSaturation(1).AddLuminance(0.1)
                    },
                    .CheckBox = New ControlsThemeColors.CheckBoxThemeColors() With {
                        .BackColor = Color.Empty,
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackHoverColor = .BackColor.AddLuminance(0.1),
                        .BorderColor = Color.Transparent,
                        .CheckedBackColor = _backColor,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = .ForeColor.AddLuminance(0.15)
                    },
                    .ComboBox = New ControlsThemeColors.ComboBoxThemeColors() With {
                        .BackColor = _backColor.AddLuminance(0.1),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BorderColor = Color.Transparent,
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = .ForeColor.AddLuminance(0.15)
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
                        .BackColor = _backColor,
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
                        .ForeColor = _foreColor,
                        .ProgressColor = _accentColor.AddSaturation(0.25).AddLuminance(-0.375)
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
                        .SelectedItemWithFocusBackColor = _accentColor.AddSaturation(-0.2).AddLuminance(-0.35),
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
                        .BorderHoverColor = .BorderFocusedColor.AddSaturation(0.1).AddLuminance(0.1),
                        .ForeColor = _foreColor,
                        .ForeHighlightColor = _controlForeHighlightColor
                    },
                    .ToolStrip = New ControlsThemeColors.ToolStripThemeColors() With {
                        .MenuStripBackgroundDefaultColor = _controlBackColor.AddLuminance(-0.01),
                        .MenuStripBackgroundSelectedColor = _backSelectedColor,
                        .MenuStripTextDefaultColor = _foreColor.AddLuminance(0.1),
                        .MenuStripTextSelectedColor = _foreColor.AddLuminance(0.2),
                        .BorderColor = .MenuStripBackgroundDefaultColor.AddLuminance(0.05),
                        .DropdownBackgroundDefaultColor = .MenuStripBackgroundDefaultColor,
                        .DropdownBackgroundSelectedColor = .MenuStripBackgroundSelectedColor,
                        .DropdownTextDefaultColor = .MenuStripTextDefaultColor,
                        .DropdownTextSelectedColor = .MenuStripTextSelectedColor,
                        .SymbolImageColor = .DropdownTextDefaultColor
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
                        .BackExpandedColor = _accentColor.AddSaturation(-0.1).AddLuminance(-0.41),
                        .BackHighlightColor = _controlBackHighlightColor,
                        .BackSelectedColor = _backSelectedColor,
                        .ForeColor = _foreColor,
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
                .AttentionForeColor = New ColorHSL(10, 1, 0.5, 1),
                .OkayForeColor = New ColorHSL(130, 1, 0.5, 1)
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
                .laTipBackColor = Color.Empty,
                .laTipBackHighlightColor = Color.Empty,
                .laTipForeColor = _accentColor,
                .laTipForeHighlightColor = _foreHighlightColor
            }

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Class


