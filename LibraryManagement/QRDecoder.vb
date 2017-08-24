Imports System.Drawing
Imports ZXing
Public Class QRDecoder
    Dim camera As Camera
    Dim result As Result
    Dim jsonString
    Dim reader As New BarcodeReader
    Public Function ScanQR(frame As Bitmap)
        Dim tryAgain = True
        If (tryAgain) Then
            Try
                result = reader.Decode(frame)
                jsonString = result.ToString.Trim
            Catch ex As System.ArgumentNullException
                tryAgain = True
                Catch a As System.NullReferenceException
                    tryAgain = True
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
        ScanQR = jsonString
    End Function

End Class
