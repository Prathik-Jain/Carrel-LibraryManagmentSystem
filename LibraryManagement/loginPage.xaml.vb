Imports System.Text.RegularExpressions
Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports Newtonsoft.Json

Class Page1
    Dim ReadOnly _camera As New Camera
    WithEvents _sendImage As DispatcherTimer
    WithEvents _delayTimer As DispatcherTimer
    Dim _movetxt As Storyboard
    Dim _jsonConvert As JsonConvert

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Initializing animation
        _movetxt = Me.Resources("moveText")
        InitializeComponent()

        _camera.StartCamera()
        _sendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
            }
        _sendImage.Start()
    End Sub

    Public Sub SendImage_Tick(sender As Object, e As EventArgs) Handles _sendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQR(_camera.frame)
        Try
            If jsonString <> "" Then
                QrScanned(jsonString)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Dim _admin = New Admin
    Dim ReadOnly _loginService = New AdminService

    Public Async Sub QrScanned(str As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If str.Contains("ADM") Then
            _admin = JsonConvert.DeserializeObject (Of Admin)(str)
            Try
                If _loginService.CheckUser(_admin.UID, _admin.FName) = 1 Then
                    _camera.StopCamera()
                    _sendImage.Stop()
                    UpdateScreen()
                Else

                    loginPage.Background = Brushes.OrangeRed
                    txtLoginInstruction.Text = "Admin not found"
                    _sendImage.Stop()
                    Await Task.Delay(2000)
                    _sendImage.Start()
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
        BeginStoryboard(_movetxt)
        TxtPIN.IsEnabled = True
        TxtPIN.Clear()
        Keyboard.Focus(TxtPIN)
        txtWelcome.Text = "Welcome, " + _admin.FName
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles Close.Click
        StopAllServices()
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Windows.Application.Current.Shutdown()
    End Sub

    Private Sub StopAllServices()
        _camera.StopCamera()  'Stops the webCam.
        _sendImage.Stop() 'Stops sending images to decode.
    End Sub

    Private Sub TxtPIN_PrieviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtPIN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtPIN_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TxtPIN.TextChanged
        Dim pin = TxtPIN.Text.Replace(" ", "")
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
                If _loginService.MatchPIN(_admin.UID, PIN) = 1 Then
                    Dim dash As New DashBoard()
                    dash.Show()
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

    Private Sub AddNumberToPin(number As Integer)
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
Public Class Admin
    Public Uid As String
    Public FName As String
End Class
