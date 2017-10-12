Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf

Public Class DashBoard
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue
    WithEvents SendImage As DispatcherTimer
    Dim camera As Camera

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        startCameraAndTimer()
    End Sub

    Public Sub startCameraAndTimer()
        camera = New Camera
        camera.StartCamera()
        SendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
            }
        SendImage.Start()
    End Sub

    Public Sub stopCameraAndTimer()
        camera.StopCamera()
        SendImage.Stop()
    End Sub

    Public Sub SendImage_Tick(sender As Object, e As EventArgs) Handles SendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQR(camera.frame)
        Try
            If jsonString <> "" Then
                stopCameraAndTimer()
                QrScanned(jsonString)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If jsonString.Contains("MEM") Then
            MsgBox("Member")
        ElseIf jsonString.Contains("ADM") Then
            Dim memberAccount As New MemberAccount
            memberAccount.GetData("MEM-0001")
            FAB.IsEnabled = False
        ElseIf jsonString.Contains("BOOK") Then
            MsgBox("BOok")

        End If
    End Sub

    Private Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        BookFormDialog.IsOpen = True
        FAB.IsEnabled = False
        stopCameraAndTimer()
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        MemberFormDialog.IsOpen = True
        FAB.IsEnabled = False
        stopCameraAndTimer()
    End Sub

    Private Sub FormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing,
                BookFormDialog.DialogClosing
        FAB.IsEnabled = True
        startCameraAndTimer()
    End Sub

    Private Sub ViewDialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) _
        Handles ViewMemberDialog.DialogOpened,
                ViewBookDialog.DialogOpened,
                Me.Unloaded
        FAB.IsEnabled = False
        stopCameraAndTimer()
    End Sub

    Private Sub ViewDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles ViewMemberDialog.DialogClosing,
                ViewBookDialog.DialogClosing
        FAB.IsEnabled = True
        startCameraAndTimer()
    End Sub

    Private Sub MemberPopupDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberPopupDialog.DialogClosing
        startCameraAndTimer()
    End Sub
End Class