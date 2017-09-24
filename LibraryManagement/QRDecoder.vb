﻿Imports System.Drawing
Imports ZXing
Public Class QRDecoder
    Dim camera As Camera
    Dim result As Result
    Dim jsonString
    Dim reader As New BarcodeReader
    Public Function ScanQR(frame As Bitmap)
        Try
            result = reader.Decode(frame)
            jsonString = result.ToString.Trim
        Catch ex As System.ArgumentNullException
        Catch a As System.NullReferenceException
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ScanQR = jsonString
    End Function
End Class
