Imports System.Drawing
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports ZXing
''' <summary>
''' This is used to decode the QR codes frames from the webcam.
''' </summary>
Public Class QrDecoder
    Implements  IDisposable
    Dim _camera As Camera
    Dim _result As Result
    Dim _string
    Dim ReadOnly _reader As New BarcodeReader
    Private disposed As Boolean = False
    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)
    ''' <summary>
    ''' This function takes the frame from the camera and tries to find a QR-Code and decode it.
    ''' </summary>
    ''' <param name="frame">Frame from the camera</param>
    ''' <returns>String which was decoded from the QR.</returns>
    Public Function ScanQr(frame As Bitmap)
        Try
            _result = _reader.Decode(frame)
            _string = _result.ToString.Trim
        Catch ex As ArgumentNullException
        Catch a As NullReferenceException
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ScanQR = _string
    End Function
    ''' <summary>
    ''' Called to dispose the object of this class.
    ''' </summary>
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
