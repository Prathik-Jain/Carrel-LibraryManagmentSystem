Imports System.Text.RegularExpressions
Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports Newtonsoft.Json

Class Login
    Dim ReadOnly _camera As New Camera
    WithEvents _sendImage As DispatcherTimer
    WithEvents _delayTimer As DispatcherTimer
    Dim _movetxt As Storyboard
    Dim _jsonConvert As JsonConvert
    ''' <summary>
    ''' Starts camera and timer when loaded.
    ''' Initializes animation
    ''' </summary>
    Private Sub Me_Loaded() Handles Me.Loaded
        ' Initializing animation
        _movetxt = Me.Resources("moveText")
        InitializeComponent()
        _camera.StartCamera()
        _sendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
            }
        _sendImage.Start()
    End Sub
    ''' <summary>
    ''' This function is called every time the timer ticks. This is used to decode the QR code and this continues until decoded.
    ''' </summary>
    ''' <seealso cref="QrDecoder"/>
    Public Sub SendImage_Tick() Handles _sendImage.Tick
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
    ''' <summary>
    ''' This functions checks if the decoded string contains a valid Admin Info.
    ''' </summary>
    ''' <param name="str">Decoded String</param>
    ''' <seealso cref="AdminService.CheckUser(String, String)"/>
    ''' <seealso cref="AdminService.MatchPin(String, Integer)"/>
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

                    Me.Background = Brushes.OrangeRed
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

    ''' <summary>
    ''' Updates the screen with Admin Info.
    ''' </summary>
    Private Sub UpdateScreen()
        BeginStoryboard(_movetxt)
        TxtPIN.IsEnabled = True
        TxtPIN.Clear()
        Keyboard.Focus(TxtPIN)
        txtWelcome.Text = "Welcome, " + _admin.FName
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub

    ''' <summary>
    ''' Closes the application when the close button is clicked.
    ''' </summary>
    Private Sub BtnClose_Click() Handles BtnClose.Click
        StopAllServices()
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Windows.Application.Current.Shutdown()
    End Sub
    ''' <summary>
    ''' Stops the camera and the timer.
    ''' </summary>
    Private Sub StopAllServices()
        _camera.StopCamera()  'Stops the webCam.
        _sendImage.Stop() 'Stops sending images to decode.
    End Sub

    Private Sub TxtPIN_PrieviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtPIN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub
       ''' <summary>
       ''' This procedure is used to update the color of the dots on every character typed for the PIN. Once the length of the filed reaches four, <see cref="AdminService.MatchPin(String, Integer)"/> function is called.
       ''' <para>If the function returns true the dashboard is opened.</para>
       ''' </summary>
       ''' <seealso cref="DashBoard"/>
       ''' <seealso cref="AdminService"/>
    Private Sub TxtPIN_TextChanged() Handles TxtPIN.TextChanged
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
                If _loginService.MatchPIN(_admin.UID, PIN) = 1 Then
                    _camera.StopCamera()
                    Dim dash As New DashBoard()

                    dash.Show()

                    Me.Close()
                   
                Else
                    MsgBox("Wrong Pin!")
                    TxtPIN.Clear()
                End If
        End Select
    End Sub

    ''' <summary>
    ''' This function is used to clear the PIN indicaters
    ''' </summary>
    Private Sub ClearDots()
        dot1.Background = New SolidColorBrush(Colors.White)
        dot2.Background = New SolidColorBrush(Colors.White)
        dot3.Background = New SolidColorBrush(Colors.White)
        dot4.Background = New SolidColorBrush(Colors.White)
    End Sub
    ''' <summary>
    ''' Thisprocedure adds the character to the <c>TxtPIN</c> field and forces the keyboard to focus on the same.
    ''' </summary>
    ''' <param name="number"></param>
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
''' <summary>
''' Class Admin with UID and FName fields.
''' </summary>
Public Class Admin
    Public Uid As String
    Public FName As String
End Class
