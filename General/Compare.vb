'(c) Vasian Cepa http://madebits.com
'http://codeconverter.sharpdevelop.net/SnippetConverter.aspx

Public NotInheritable Class StringLogicalComparer
    Implements IComparer, IComparer(Of String)

    Private ZeroesFirst As Boolean = False

    Private Sub New()
    End Sub

    Private Sub New(zeroesFirst As Boolean)
        Me.ZeroesFirst = zeroesFirst
    End Sub

    Private Shared DefaultValue As StringLogicalComparer

    Public Shared ReadOnly Property [Default] As StringLogicalComparer
        Get
            If DefaultValue Is Nothing Then DefaultValue = New StringLogicalComparer
            Return DefaultValue
        End Get
    End Property

    Private Shared DefaultZeroesFirstValue As StringLogicalComparer

    Public Shared ReadOnly Property DefaultZeroesFirst() As StringLogicalComparer
        Get
            If DefaultZeroesFirstValue Is Nothing Then DefaultZeroesFirstValue = New StringLogicalComparer(True)
            Return DefaultZeroesFirstValue
        End Get
    End Property

    Public Function IComparer_Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        If x Is Nothing AndAlso y Is Nothing Then Return 0
        If x Is Nothing Then Return -1
        If y Is Nothing Then Return 1

        If TypeOf x Is String AndAlso TypeOf y Is String Then
            Return Compare(DirectCast(x, String), DirectCast(y, String), ZeroesFirst)
        End If

        Return Comparer.Default.Compare(x, y)
    End Function

    Private Function IComparerOfString_Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
        Return IComparer_Compare(x, y)
    End Function

    Public Shared Function Compare(s1 As String,
                                   s2 As String,
                                   Optional zeroesFirst As Boolean = False) As Integer
        'get rid of special cases
        If (s1 Is Nothing) AndAlso (s2 Is Nothing) Then
            Return 0
        ElseIf s1 Is Nothing Then
            Return -1
        ElseIf s2 Is Nothing Then
            Return 1
        End If

        If (s1.Length <= 0) AndAlso (s2.Length <= 0) Then
            Return 0
        ElseIf s1.Length <= 0 Then
            Return -1
        ElseIf s2.Length <= 0 Then
            Return 1
        End If

        'special case
        Dim sp1 = Char.IsLetterOrDigit(s1(0))
        Dim sp2 = Char.IsLetterOrDigit(s2(0))

        If sp1 AndAlso Not sp2 Then Return 1
        If Not sp1 AndAlso sp2 Then Return -1

        Dim i1, i2 As Integer
        'current index
        Dim r As Integer
        'temp result
        Dim c1, c2 As Char
        Dim letter1, letter2 As Boolean

        While True
            c1 = s1(i1)
            c2 = s2(i2)
            sp1 = Char.IsDigit(c1)
            sp2 = Char.IsDigit(c2)

            If Not sp1 AndAlso Not sp2 Then
                letter1 = Char.IsLetter(c1)
                letter2 = Char.IsLetter(c2)

                If letter1 AndAlso letter2 Then
                    r = Char.ToUpper(c1).ToString().CompareTo(Char.ToUpper(c2).ToString())
                    If r <> 0 Then Return r
                ElseIf Not letter1 AndAlso Not letter2 Then
                    r = c1.CompareTo(c2)
                    If r <> 0 Then Return r
                ElseIf Not letter1 AndAlso letter2 Then
                    Return -1
                ElseIf letter1 AndAlso Not letter2 Then
                    Return 1
                End If
            ElseIf sp1 AndAlso sp2 Then
                r = CompareNum(s1, i1, s2, i2, zeroesFirst)
                If r <> 0 Then Return r
            ElseIf sp1 Then
                Return -1
            ElseIf sp2 Then
                Return 1
            End If

            i1 += 1
            i2 += 1

            If (i1 >= s1.Length) AndAlso (i2 >= s2.Length) Then
                Return 0
            ElseIf i1 >= s1.Length Then
                Return -1
            ElseIf i2 >= s2.Length Then
                Return 1
            End If
        End While
    End Function

    Private Shared Function CompareNum(s1 As String,
                                       ByRef i1 As Integer,
                                       s2 As String,
                                       ByRef i2 As Integer,
                                       zeroesFirst As Boolean) As Integer

        Dim nzStart1 = i1, nzStart2 = i2
        'nz = non zero
        Dim end1 = i1, end2 = i2
        ScanNumEnd(s1, i1, end1, nzStart1)
        ScanNumEnd(s2, i2, end2, nzStart2)
        Dim start1 = i1
        i1 = end1 - 1
        Dim start2 = i2
        i2 = end2 - 1

        If zeroesFirst Then
            Dim zl1 = nzStart1 - start1
            Dim zl2 = nzStart2 - start2

            If zl1 > zl2 Then Return -1
            If zl1 < zl2 Then Return 1
        End If

        Dim nzLength1 = end1 - nzStart1
        Dim nzLength2 = end2 - nzStart2

        If nzLength1 < nzLength2 Then
            Return -1
        ElseIf nzLength1 > nzLength2 Then
            Return 1
        End If

        Dim j1 = nzStart1, j2 = nzStart2

        While j1 <= i1
            Dim r = s1(j1).CompareTo(s2(j2))
            If r <> 0 Then Return r
            j1 += 1
            j2 += 1
        End While

        'the nz parts are equal
        Dim length1 = end1 - start1
        Dim length2 = end2 - start2
        If length1 = length2 Then Return 0
        If length1 > length2 Then Return -1

        Return 1
    End Function

    'lookahead
    Private Shared Sub ScanNumEnd(s As String,
                                  startPos As Integer,
                                  ByRef endPos As Integer,
                                  ByRef nzStart As Integer)
        nzStart = startPos
        endPos = startPos
        Dim countZeros = True

        While Char.IsDigit(s, endPos)
            If countZeros AndAlso s(endPos).Equals("0"c) Then
                nzStart += 1
            Else
                countZeros = False
            End If

            endPos += 1
            If endPos >= s.Length Then Exit While
        End While
    End Sub
End Class