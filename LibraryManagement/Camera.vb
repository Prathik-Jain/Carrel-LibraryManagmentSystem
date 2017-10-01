Imports System.Drawing
Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class Camera
    Public camera As VideoCaptureDevice
    Public videoDevices As FilterInfoCollection
    Public frame As Bitmap

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
End Class
