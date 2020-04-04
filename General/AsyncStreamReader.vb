
Imports System.Text
Imports System.Threading

Imports VB6 = Microsoft.VisualBasic

Public Class AsyncStreamReader
    Implements IDisposable

    Const DefaultBufferSize As Integer = 1024
    Private Stream As Stream
    Private Decoder As Decoder
    Private ByteBuffer As Byte()
    Private CharBuffer As Char()
    Private MaxCharsPerBuffer As Integer
    Private CallBack As Action(Of String)
    Private EofEvent As New ManualResetEvent(False)
    Private Queue As New Queue(Of String)
    Private SB As StringBuilder
    Private LastCarriageReturn As Boolean
    Private CurrentLinePos As Integer

    Sub New(stream As Stream, callback As Action(Of String), encoding As Encoding)
        Me.Stream = stream
        Me.CallBack = callback
        Decoder = encoding.GetDecoder()
        ByteBuffer = New Byte(DefaultBufferSize - 1) {}
        MaxCharsPerBuffer = encoding.GetMaxCharCount(DefaultBufferSize)
        CharBuffer = New Char(MaxCharsPerBuffer - 1) {}
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        If Not Stream Is Nothing Then
            Stream.Close()
            EofEvent.Close()
            Stream = Nothing
        End If
    End Sub

    Sub BeginReadLine()
        SB = New StringBuilder(DefaultBufferSize)
        Stream.BeginRead(ByteBuffer, 0, ByteBuffer.Length, New AsyncCallback(AddressOf ReadBuffer), Nothing)
    End Sub

    Sub ReadBuffer(ar As IAsyncResult)
        Dim byteLen As Integer

        Try
            byteLen = Stream.EndRead(ar)
        Catch
            byteLen = 0
        End Try

        If byteLen = 0 Then
            SyncLock Queue
                If SB.Length <> 0 Then
                    Queue.Enqueue(SB.ToString())
                    SB.Length = 0
                End If

                Queue.Enqueue(Nothing)
            End SyncLock

            FlushMessageQueue()
            EofEvent.Set()
        Else
            Dim charLen = Decoder.GetChars(ByteBuffer, 0, byteLen, CharBuffer, 0)
            SB.Append(CharBuffer, 0, charLen)
            GetLinesFromStringBuilder()
            Stream.BeginRead(ByteBuffer, 0, ByteBuffer.Length, New AsyncCallback(AddressOf ReadBuffer), Nothing)
        End If
    End Sub

    Sub GetLinesFromStringBuilder()
        Dim currentIndex = CurrentLinePos
        Dim lineStart As Integer
        Dim len = SB.Length
        Dim BackSpace = VB6.Chr(8)
        Dim LineFeet = VB6.Chr(10)
        Dim CarriageReturn = VB6.Chr(13)

        If LastCarriageReturn AndAlso len > 0 AndAlso SB(0) = LineFeet Then
            currentIndex = 1
            lineStart = 1
            LastCarriageReturn = False
        End If

        While currentIndex < len
            Dim ch = SB(currentIndex)

            If ch = CarriageReturn OrElse ch = LineFeet OrElse ch = BackSpace Then
                Dim str = SB.ToString(lineStart, currentIndex - lineStart)
                lineStart = currentIndex + 1

                If ch = CarriageReturn AndAlso lineStart < len AndAlso SB(lineStart) = LineFeet Then
                    lineStart += 1
                    currentIndex += 1
                End If

                SyncLock Queue
                    Queue.Enqueue(str)
                End SyncLock
            End If

            currentIndex += 1
        End While

        If len > 0 AndAlso SB(len - 1) = CarriageReturn Then
            LastCarriageReturn = True
        End If

        If lineStart < len Then
            If lineStart = 0 Then
                CurrentLinePos = currentIndex
            Else
                SB.Remove(0, lineStart)
                CurrentLinePos = 0
            End If
        Else
            SB.Length = 0
            CurrentLinePos = 0
        End If

        FlushMessageQueue()
    End Sub

    Sub FlushMessageQueue()
        While True
            If Queue.Count > 0 Then
                SyncLock Queue
                    If Queue.Count > 0 Then
                        CallBack(Queue.Dequeue())
                    End If
                End SyncLock
            Else
                Exit While
            End If
        End While
    End Sub

    Sub WaitUtilEOF()
        EofEvent.WaitOne()
    End Sub
End Class
