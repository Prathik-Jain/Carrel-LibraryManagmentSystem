Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports System.Text.RegularExpressions
Imports newtonsoft.json

Class Page1
    Dim camera As New Camera
    Dim mw As New MainWindow
    WithEvents SendImage As DispatcherTimer
    Dim movetxt As Storyboard
    Dim JsonConvert As JsonConvert

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Initializing animation
        movetxt = Me.Resources("moveText")
        camera.StartCamera()
        SendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
        }
        SendImage.Start()
    End Sub

    Private Sub SendImage_Tick(sender As Object, e As EventArgs) Handles SendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        While jsonString = ""
            jsonString = qrDecoder.ScanQR(camera.frame)
        End While
        QrScanned(jsonString)
    End Sub

    Private Sub QrScanned(str As String)
        My.Computer.Audio.Play("C:\beep.wav", AudioPlayMode.Background)
        If str.Contains("ADM") Then
            camera.StopCamera()
            SendImage.Stop()
            Dim adnQR = New AdminQR
            adnQR = JsonConvert.DeserializeObject(Of AdminQR)(str)
            BeginStoryboard(movetxt)
            txtPIN.IsEnabled = True
            txtPIN.Clear()
            Keyboard.Focus(txtPIN)
            txtWelcome.Text = "Welcome, " + adnQR.Name
            txtLoginInstruction.Text = "Please enter your PIN"
        Else
            MsgBox("U need to be an admin")
        End If
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles Close.Click
        'TODO make close working on welcome screen
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Windows.Application.Current.Shutdown()
    End Sub

    Private Sub txtPIN_PrieviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtPIN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtPIN_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtPIN.TextChanged
        Dim x = txtPIN.Text.Replace(" ", "")
        Dim i As Integer = 0
        Select Case (x.Length)
            Case 0
                ClearDots()

            Case 1
                ClearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)

            Case 2
                ClearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot2.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
            Case 3
                ClearDots()
                dot1.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot2.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
                dot3.Background = DirectCast(New BrushConverter().ConvertFrom("#000000"), SolidColorBrush)
            Case 4

                ClearDots()
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
            MsgBox("Naaa")
        Else
            MsgBox("Yaay!")
        End If
    End Sub

    Private Sub ClearDots()
        dot1.Background = New SolidColorBrush(Colors.White)
        dot2.Background = New SolidColorBrush(Colors.White)
        dot3.Background = New SolidColorBrush(Colors.White)
        dot4.Background = New SolidColorBrush(Colors.White)
    End Sub

    Private Sub AddNumberToPIN(number As Integer)
        txtPIN.Text += number.ToString
        Keyboard.Focus(txtPIN)
        txtPIN.CaretIndex = txtPIN.Text.Length
    End Sub

#Region "NumberPad Button Click Events"
    Private Sub BtnZero_Click(sender As Object, e As RoutedEventArgs) Handles btnZero.Click
        AddNumberToPIN(0)
    End Sub

    Private Sub BtnOne_Click(sender As Object, e As RoutedEventArgs) Handles btnOne.Click
        AddNumberToPIN(1)
    End Sub

    Private Sub BtnTwo_Click(sender As Object, e As RoutedEventArgs) Handles btnTwo.Click
        AddNumberToPIN(2)
    End Sub

    Private Sub BtnThree_Click(sender As Object, e As RoutedEventArgs) Handles btnThree.Click
        AddNumberToPIN(3)
    End Sub

    Private Sub BtnFour_Click(sender As Object, e As RoutedEventArgs) Handles btnFour.Click
        AddNumberToPIN(4)
    End Sub

    Private Sub BtnFive_Click(sender As Object, e As RoutedEventArgs) Handles btnFive.Click
        AddNumberToPIN(5)
    End Sub

    Private Sub BtnSix_Click(sender As Object, e As RoutedEventArgs) Handles btnSix.Click
        AddNumberToPIN(6)
    End Sub

    Private Sub BtnSeven_Click(sender As Object, e As RoutedEventArgs) Handles btnSeven.Click
        AddNumberToPIN(7)
    End Sub

    Private Sub BtnEight_Click(sender As Object, e As RoutedEventArgs) Handles btnEight.Click
        AddNumberToPIN(8)
    End Sub

    Private Sub BtnNine_Click(sender As Object, e As RoutedEventArgs) Handles btnNine.Click
        AddNumberToPIN(9)
    End Sub
#End Region
End Class
