Imports System.Drawing
Imports System.Runtime.InteropServices
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports Microsoft.Win32.SafeHandles
''' <summary>
''' This class is responsible for performing all the actions related to the webcam or an external camera.
''' </summary>
Public Class Camera
    Implements  IDisposable
    Public Camera As VideoCaptureDevice
    Public VideoDevices As FilterInfoCollection
    Public Frame As Bitmap
    Private disposed As Boolean = False
    Private handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)
    ' TODO - Change camera dynamically.
    ''' <summary>
    ''' This function is responsible to start the webcam.
    ''' </summary>
    Public Sub StartCamera()
        Try
            videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
            camera = New VideoCaptureDevice(videoDevices(0).MonikerString) 'Change index to 1 for an external camera. 
            camera.Start()
            AddHandler camera.NewFrame, New NewFrameEventHandler(AddressOf Capturer)
        Catch ex As Exception
            MsgBox("Camera not found!")
        End Try
    End Sub
    ''' <summary>
    ''' This is responsible to convert each frame to Bitmap
    ''' </summary>
    Public Sub Capturer(sender As Object, eventArgs As NewFrameEventArgs)
        frame = DirectCast(eventArgs.Frame.Clone(), Bitmap)
    End Sub
    ''' <summary>
    ''' This function is responsible to stop camera.
    ''' </summary>
    Public Sub StopCamera()
        camera.SignalToStop()
    End Sub
    ''' <summary>
    ''' The Dispose function is used to dispose an object of the <see cref="Carrel.Camera"/>class.
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
