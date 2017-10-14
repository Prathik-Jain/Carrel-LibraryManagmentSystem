Imports System.Drawing
Imports ZXing

Public Class QrDecoder
    Dim _camera As Camera
    Dim _result As Result
    Dim _jsonString
    Dim ReadOnly _reader As New BarcodeReader

    Public Function ScanQr(frame As Bitmap)
        Try
            _result = _reader.Decode(frame)
            _jsonString = _result.ToString.Trim
        Catch ex As ArgumentNullException
        Catch a As NullReferenceException
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ScanQR = _jsonString
    End Function
End Class
