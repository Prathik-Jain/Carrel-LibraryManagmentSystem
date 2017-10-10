Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf

Public Class DashBoard
    Dim ReadOnly camera As New Camera
    WithEvents SendImage As DispatcherTimer
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue

    Private Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        BookFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        MemberFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BookFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles BookFormDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub
    Private Sub MemberFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub

    Private Sub ViewMemberDialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) Handles ViewMemberDialog.DialogOpened
        FAB.IsEnabled = False
    End Sub

    Private Sub ViewMemberDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) Handles ViewMemberDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub

    Private Sub ViewBookDialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) Handles ViewBookDialog.DialogOpened
        FAB.IsEnabled = False
    End Sub

    Private Sub ViewBookDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) Handles ViewBookDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub

    Private Sub DashBoard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
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
     Public Async Sub QrScanned(str As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If str.Contains("ADM") Then
          'Show Admin Panel
        ElseIf str.Contains("MEM") then
            'Show MemberPopUP
            ElseIf str.Contains("BOOK") then
            'Show BookView
        Else
            MsgBox("Invalid Card!")
        End If
    End Sub
End Class
