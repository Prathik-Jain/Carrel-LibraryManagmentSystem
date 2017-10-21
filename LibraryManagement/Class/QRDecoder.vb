Imports System.Drawing
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports ZXing

Public Class QrDecoder
    Implements  IDisposable
    Dim _camera As Camera
    Dim _result As Result
    Dim _jsonString
    Dim ReadOnly _reader As New BarcodeReader
    Private disposed As Boolean = False
    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

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

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then
            Return
        End If

        If disposing Then
            handle.Dispose()
        End If
        disposed = True
    End Sub
End Class
