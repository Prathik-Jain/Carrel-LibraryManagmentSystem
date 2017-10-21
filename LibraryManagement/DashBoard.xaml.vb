Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf

Public Class DashBoard
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue
    WithEvents _sendImage As DispatcherTimer
    Dim _camera As Camera
    dim memberAccount as new MemberPopup

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        startCameraAndTimer()
    End Sub

    Public Sub StartCameraAndTimer()
        _camera = New Camera
        _camera.StartCamera()
        _sendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
            }
        _sendImage.Start()
    End Sub

    Public Sub StopCameraAndTimer()
        _camera.StopCamera()
        _camera.Camera.WaitForStop()
        _sendImage.Stop()
        _camera.Dispose()
    End Sub

    Public Sub SendImage_Tick(sender As Object, e As EventArgs) Handles _sendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQR(_camera.frame)
        Try
            If jsonString <> "" Then
                stopCameraAndTimer()
                QrScanned(jsonString)
                qrDecoder.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Async Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If MemberPopupDialog.IsOpen
            Try
                If jsonString.Contains("UID") Then
                    if await memberAccount.CheckBookInList 'Checks and returns book if found
                        memberAccount.GetData("MEM-0003")
                        StartCameraAndTimer()
                    ElseIf await BookService.Borrowed("BOOK-2","MEM-0003") 
                        memberAccount.getData("MEM-0003")
                        StartCameraAndTimer()
                    Else 
                        MsgBox("Failed to Update database")
                    End If
                else
                    MsgBox("Please scan a book to add or return")
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            End Try
            Else 
                If jsonString.Contains("MEM") Then
                    MsgBox("Member")
                ElseIf jsonString.Contains("ADM") Then
                    stopcameraandtimer
                    memberAccount.GetData("MEM-0003")
                    FAB.IsEnabled = False
                ElseIf jsonString.Contains("BOOK") Then
                    MsgBox("BOok")
                End If
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

    Private Sub Dialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing,
                BookFormDialog.DialogClosing,
                ViewBookDialog.DialogClosing
        FAB.IsEnabled = True
        startCameraAndTimer()
    End Sub

    Private  Sub Dialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) _
        Handles ViewBookDialog.DialogOpened,
                MemberPopupDialog.DialogOpened,
                Me.Unloaded
        SnackBarMessageQueue = Snackbar.MessageQueue
        FAB.IsEnabled = False
StartCameraAndTimer()
    End Sub


    
End Class