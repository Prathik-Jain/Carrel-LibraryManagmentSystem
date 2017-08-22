Imports System.Drawing
Imports System.Windows.Media.Animation
Imports System.Windows.Media
Imports ZXing
Imports AForge.Video
Imports System
Imports System.Drawing.Color
Imports AForge.Video.DirectShow
Imports System.Windows.Threading
Imports System.Text.RegularExpressions
Imports System.Windows.Media.Brushes
Imports System.Windows.Automation.Peers

Class Page1
    Dim camera As VideoCaptureDevice
    Dim bmp As Bitmap
    Dim mw As New MainWindow
    WithEvents timer As DispatcherTimer
    Dim movetxt As Storyboard
    Dim re As Result
    Dim scr As ImageSource


    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        movetxt = Me.Resources("moveText")
        selectCamera()
        timer = New DispatcherTimer()
        timer.Interval = New TimeSpan(0, 0, 0.5)
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

    Private Sub qrScanned(str)
        My.Computer.Audio.Play("C:\beep.wav", AudioPlayMode.Background)
        camera.Stop()
        timer.Stop()
        BeginStoryboard(movetxt)
        txtPIN.IsEnabled = True
        txtPIN.Clear()
        Keyboard.Focus(txtPIN)
        txtWelcome.Text = "Welcome, " + str
        txtLoginInstruction.Text = "Please enter your PIN"

    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles Close.Click
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Application.Current.Shutdown()
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
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

    Private Sub txtPIN_PrieviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtPIN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtPIN_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtPIN.TextChanged
        Dim x = txtPIN.Text.Replace(" ", "")
        Dim i As Integer = 0
        Select Case (x.Length)
            Case 0
                clearDots()

            Case 1
                clearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)

            Case 2
                clearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot2.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
            Case 3
                clearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot2.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot3.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
            Case 4

                clearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot2.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot3.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot4.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                'HACK to update UI befor matchPIN() executes.
                matchPIN(x)
        End Select

    End Sub

    Private Sub matchPIN(x)
        If x <> "1234" Then
            txtPIN.Clear()
        Else
            MsgBox("Yaay!")
        End If
    End Sub

    Private Sub clearDots()
        dot1.Background = New SolidColorBrush(Colors.White)
        dot2.Background = New SolidColorBrush(Colors.White)
        dot3.Background = New SolidColorBrush(Colors.White)
        dot4.Background = New SolidColorBrush(Colors.White)
    End Sub

    Private Sub addTextToPin(v As Integer)
        txtPIN.Text += v.ToString
        Keyboard.Focus(txtPIN)
        txtPIN.CaretIndex = txtPIN.Text.Length
    End Sub

#Region "NumberPad Button Click Events"
    Private Sub BtnZero_Click(sender As Object, e As RoutedEventArgs) Handles btnZero.Click
        addTextToPin(0)
    End Sub

    Private Sub BtnOne_Click(sender As Object, e As RoutedEventArgs) Handles btnOne.Click
        addTextToPin(1)
    End Sub

    Private Sub BtnTwo_Click(sender As Object, e As RoutedEventArgs) Handles btnTwo.Click
        addTextToPin(2)
    End Sub

    Private Sub BtnThree_Click(sender As Object, e As RoutedEventArgs) Handles btnThree.Click
        addTextToPin(3)
    End Sub

    Private Sub BtnFour_Click(sender As Object, e As RoutedEventArgs) Handles btnFour.Click
        addTextToPin(4)
    End Sub

    Private Sub BtnFive_Click(sender As Object, e As RoutedEventArgs) Handles btnFive.Click
        addTextToPin(5)
    End Sub

    Private Sub BtnSix_Click(sender As Object, e As RoutedEventArgs) Handles btnSix.Click
        addTextToPin(6)
    End Sub

    Private Sub BtnSeven_Click(sender As Object, e As RoutedEventArgs) Handles btnSeven.Click
        addTextToPin(7)
    End Sub

    Private Sub BtnEight_Click(sender As Object, e As RoutedEventArgs) Handles btnEight.Click
        addTextToPin(8)
    End Sub

    Private Sub BtnNine_Click(sender As Object, e As RoutedEventArgs) Handles btnNine.Click
        addTextToPin(9)
    End Sub
#End Region
End Class
