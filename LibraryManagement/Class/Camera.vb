Imports System.Drawing
Imports System.Runtime.InteropServices
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports Microsoft.Win32.SafeHandles

Public Class Camera
    Implements  IDisposable
    Public Camera As VideoCaptureDevice
    Public VideoDevices As FilterInfoCollection
    Public Frame As Bitmap
    Private disposed As Boolean = False
    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    Public Sub StartCamera()
        videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        camera = New VideoCaptureDevice(videoDevices(0).MonikerString)
        camera.Start()
        AddHandler camera.NewFrame, New NewFrameEventHandler(AddressOf Capturer)
    End Sub

    Public Sub Capturer(sender As Object, eventArgs As NewFrameEventArgs)
        frame = DirectCast(eventArgs.Frame.Clone(), Bitmap)
    End Sub

    Public Sub StopCamera()
        camera.SignalToStop()
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then
            Return
        End If

        If disposing Then
            ' Free any other managed objects here.
            '
            handle.Dispose()
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
        End Sub
End Class
