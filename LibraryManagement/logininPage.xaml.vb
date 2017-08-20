Imports System.Drawing
Imports System.Windows.Media.Animation
Imports ZXing
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports System.Windows.Threading

Class Page1
    Dim camera As VideoCaptureDevice
    Dim bmp As Bitmap
    Dim mw As New MainWindow
    WithEvents timer As DispatcherTimer
    Dim movetxt As Storyboard
    Dim re As Result
    Dim scr As ImageSource
    Private Sub qrScanned(str)
        camera.Stop()
        timer.Stop()
        BeginStoryboard(movetxt)
        txtWelcome.Text = "Welcome, " + str
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        movetxt = Me.Resources("moveText")
        selectCamera()
        timer = New DispatcherTimer()
        timer.Interval = New TimeSpan(0, 0, 1)
        timer.Start()
    End Sub


    Private Sub selectCamera()
        Dim cameras As VideoCaptureDeviceForm = New VideoCaptureDeviceForm
        If cameras.ShowDialog() = Windows.Forms.DialogResult.OK Then
            camera = cameras.VideoDevice
            camera.Start()
            AddHandler camera.NewFrame, New NewFrameEventHandler(AddressOf capturer)
        End If
    End Sub

    Public Sub capturer(sender As Object, eventArgs As NewFrameEventArgs)
        bmp = DirectCast(eventArgs.Frame.Clone(), Bitmap)
    End Sub

    Private Sub Close_Click_1(sender As Object, e As RoutedEventArgs) Handles Close.Click
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Application.Current.Shutdown()
    End Sub

    Private Sub timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        Dim reader As New BarcodeReader
        Dim tryAgain = True
        If (tryAgain) Then
            Try
                re = reader.Decode(bmp)
                Dim qrStr = re.ToString.Trim
                qrScanned(qrStr)
            Catch ex As System.ArgumentNullException
                tryAgain = True
            Catch a As System.NullReferenceException
                tryAgain = True
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

End Class
