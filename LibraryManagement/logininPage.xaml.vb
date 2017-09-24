Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports System.Data.SqlClient

Class Page1
    Dim camera As New Camera
    WithEvents SendImage As DispatcherTimer
    WithEvents DelayTimer As DispatcherTimer
    Dim movetxt As Storyboard
    Dim JsonConvert As JsonConvert

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Initializing animation
        movetxt = Me.Resources("moveText")
        InitializeComponent()

        camera.StartCamera()
        SendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
        }
        SendImage.Start()

    End Sub

    Public Sub SendImage_Tick(sender As Object, e As EventArgs) Handles SendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQR(camera.frame)
        Try
            If jsonString <> "" Then
                QrScanned(jsonString)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Dim admin = New AdminQR
    Dim loginService = New AdminService
    Public Async Sub QrScanned(str As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If str.Contains("ADM") Then
            admin = JsonConvert.DeserializeObject(Of AdminQR)(str)
            Try
                If loginService.CheckUser(admin.UID, admin.Name) = 1 Then
                    camera.StopCamera()
                    SendImage.Stop()
                    UpdateScreen()
                Else

                    loginPage.Background = Brushes.OrangeRed
                    txtLoginInstruction.Text = "Admin not found"
                    SendImage.Stop()
                    Await Task.Delay(2000)
                    SendImage.Start()
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            MsgBox("Invalid Card! Please scan an Admin card.")
        End If

    End Sub


    Private Sub UpdateScreen()
        'Updates login screen with Admin info.
        BeginStoryboard(movetxt)
        TxtPIN.IsEnabled = True
        TxtPIN.Clear()
        Keyboard.Focus(TxtPIN)
        txtWelcome.Text = "Welcome, " + admin.Name
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles Close.Click
        StopAllServices()
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Windows.Application.Current.Shutdown()
    End Sub

    Private Sub StopAllServices()
        camera.StopCamera()  'Stops the webCam.
        SendImage.Stop() 'Stops sending images to decode.
    End Sub

    Private Sub TxtPIN_PrieviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPIN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtPIN_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TxtPIN.TextChanged
        Dim PIN = TxtPIN.Text.Replace(" ", "")
        Select Case (PIN.Length)
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
                If loginService.MatchPIN(admin.UID, PIN) = 1 Then
                    Dim Dash = New DashBoard
                    Dash.Show()
                    loginPage.Visibility = Visibility.Collapsed

                Else
                    MsgBox("naaaa")
                    TxtPIN.Clear()
                End If
        End Select

    End Sub



    Private Sub ClearDots()
        dot1.Background = New SolidColorBrush(Colors.White)
        dot2.Background = New SolidColorBrush(Colors.White)
        dot3.Background = New SolidColorBrush(Colors.White)
        dot4.Background = New SolidColorBrush(Colors.White)
    End Sub

    Private Sub AddNumberToPIN(number As Integer)
        TxtPIN.Text += number.ToString
        Keyboard.Focus(TxtPIN)
        TxtPIN.CaretIndex = TxtPIN.Text.Length 'Brings the caret to the end.
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
