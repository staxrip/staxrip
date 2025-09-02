Imports System.Drawing.Text
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text

Public NotInheritable Class FontManager
    Private Shared ReadOnly _currentFontFamilies As New Dictionary(Of FontCategory, FontFamily)
    Private Shared ReadOnly _fontCollections As New Dictionary(Of FontCategory, PrivateFontCollection)

    Public Shared Sub Init()
        For Each category As FontCategory In [Enum].GetValues(GetType(FontCategory)).Cast(Of FontCategory)().Skip(1)
            AddCollection(category)
        Next
    End Sub

    Shared Sub Reset()
        _currentFontFamilies.Clear()
    End Sub

    Public Shared Sub AddCollection(category As FontCategory)
        Dim collection As New PrivateFontCollection()
        Dim fontFiles = Directory.GetFiles(Path.Combine(Folder.Fonts, category.ToString()), "*.ttf", SearchOption.AllDirectories)

        For Each fontFile In fontFiles
            collection.AddFontFile(fontFile)
        Next

        If g.SettingsFolderExists Then
            If Folder.UserFonts.DirExists() Then
                Dim subfolderPath = Path.Combine(Folder.UserFonts, category.ToString())
                If subfolderPath.DirExists() Then
                    fontFiles = Directory.GetFiles(subfolderPath, "*.ttf", SearchOption.AllDirectories)
                    For Each fontFile In fontFiles
                        collection.AddFontFile(fontFile)
                    Next
                End If
            End If
        End If

        If collection.Families.Any() Then
            _fontCollections.Add(category, collection)
        End If
    End Sub

    Shared Function GetFontFamilies(category As FontCategory, filtered As Boolean) As List(Of FontFamily)
        If Not _fontCollections.Any() Then Init()

        Dim collections = If(category = FontCategory.All, _fontCollections, _fontCollections.Where(Function(x) x.Key = category))
        Dim fontFamilies = collections.SelectMany(Function(x) x.Value.Families)

        If filtered Then
            fontFamilies = fontFamilies _
                .Where(Function(x) Not x.Name.ContainsAny({"UltraCondensed", "UltraExpanded"})) _
                .Where(Function(x) Not x.Name.EndsWithAny({" Thin"}))
        End If

        Return fontFamilies.ToList()
    End Function

    Shared Function GetFontFamily(category As FontCategory, fontName As String) As FontFamily
        If Not _fontCollections.Any() Then Init()
        Dim collections = If(category = FontCategory.All, _fontCollections, _fontCollections.Where(Function(x) x.Key = category))
        Return collections.SelectMany(Function(s) s.Value.Families.Where(Function(x) x.Name = fontName)).FirstOrDefault(Function(x) x IsNot Nothing)
    End Function

    Shared Function GetFont(category As FontCategory, fontName As String, Optional size As Single = 9.0, Optional fontStyle As FontStyle = FontStyle.Regular, Optional graphicsUnit As GraphicsUnit = GraphicsUnit.Point, Optional gdiCharSet As Byte = 0) As Font
        If Not _fontCollections.Any() Then Init()

        Dim family = GetFontFamily(category, fontName)

        If family IsNot Nothing Then
            Return GetFont(family, size, fontStyle)
        End If

        Return New Font(_fontCollections.First().Value.Families.First(), size * s.UIScaleFactor, fontStyle, graphicsUnit, gdiCharSet)
    End Function

    Shared Function GetFont(fontFamily As FontFamily, Optional size As Single = 9.0, Optional fontStyle As FontStyle = FontStyle.Regular, Optional graphicsUnit As GraphicsUnit = GraphicsUnit.Point, Optional gdiCharSet As Byte = 0) As Font
        If Not _fontCollections.Any() Then Init()

        Return New Font(fontFamily, size * s.UIScaleFactor, fontStyle, graphicsUnit, gdiCharSet)
    End Function

    Shared Function GetCodeFont(Optional size As Single = 10.0, Optional fontStyle As FontStyle = FontStyle.Regular) As Font
        Dim family As FontFamily

        If _currentFontFamilies.TryGetValue(FontCategory.Code, family) Then
            Return GetFont(family, size, fontStyle)
        End If

        Dim font = GetFont(FontCategory.Code, s.Fonts(FontCategory.Code), size, fontStyle)
        _currentFontFamilies.Add(FontCategory.Code, font.FontFamily)
        Return font
    End Function

    Shared Function GetDefaultFont(Optional size As Single = 9.0, Optional fontStyle As FontStyle = FontStyle.Regular) As Font
        Dim family As FontFamily

        If _currentFontFamilies.TryGetValue(FontCategory.Default, family) Then
            Return GetFont(family, size, fontStyle)
        End If

        Dim font = GetFont(FontCategory.Default, s.Fonts(FontCategory.Default), size, fontStyle)
        _currentFontFamilies.Add(FontCategory.Default, font.FontFamily)
        Return font
    End Function

    Shared Function GetThumbnailFont(Optional size As Single = 10.0, Optional fontStyle As FontStyle = FontStyle.Regular) As Font
        Dim family As FontFamily

        If _currentFontFamilies.TryGetValue(FontCategory.Thumbnail, family) Then
            Return GetFont(family, size, fontStyle)
        End If

        Dim font = GetFont(FontCategory.Thumbnail, s.Fonts(FontCategory.Thumbnail), size, fontStyle)
        _currentFontFamilies.Add(FontCategory.Thumbnail, font.FontFamily)
        Return font
    End Function
End Class


Public Enum FontCategory
    All
    Code
    [Default]
    Thumbnail
End Enum