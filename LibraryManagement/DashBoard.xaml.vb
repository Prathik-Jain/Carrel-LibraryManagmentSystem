Imports System.ComponentModel
Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json

Public Class DashBoard
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue
    WithEvents _sendImage As DispatcherTimer
    Dim _camera As Camera
    Public memberAccount as new MemberPopup

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
                If jsonString.Contains("BOOK") Then
                    if await memberAccount.CheckBookInList(jsonString) 'Checks and returns book if found
                        memberAccount.GetData(memberAccount.LblUID.Content)
                        StartCameraAndTimer()
                    ElseIf await BookService.Borrowed(jsonString, memberaccount.LblUID.Content)
                        memberAccount.getData(memberAccount.LblUID.Content)
                        StartCameraAndTimer()
                    Else
                        MsgBox("Failed to Update database")
                    End If
                elseif jsonString.Contains("MEM")
                    MemberPopupDialog.IsOpen = False
                else
                    MsgBox("Please scan a book to add or return")
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            End Try
        ElseIf AdminPopupDialog.IsOpen
            Try
                If jsonString.Contains("ADM")
                    StopCameraAndTimer()
                    me.Close()
                End If
            Catch ex As Exception
                MsgBox(ex.Message.ToString())
            End Try
        ElseIf viewbookdialog.IsOpen
            Try
                If jsonString.Contains("BOOK")
                    ViewBookDialog.IsOpen = False
                End If
            Catch ex As Exception

            End Try
        Else
            If jsonString.Contains("MEM") Then
                'stopcameraandtimer
                memberAccount.GetData(jsonString)
                'StartCameraAndTimer()

            ElseIf jsonString.Contains("ADM") Then
                dim _admin = JsonConvert.DeserializeObject(Of Admin)(jsonString)
                'StopCameraAndTimer()
                AdminPopup.GetData(_admin.Uid)
                'StartCameraAndTimer()

            ElseIf jsonString.Contains("BOOK") Then
                Dim viewBook as new ViewBook
                viewBook.ViewBookById(jsonString)
                'StartCameraAndTimer()

            End If
        End If

    End Sub



    Public Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        BookFormDialog.IsOpen = True
        BookForm.ClearAll()
        FAB.IsEnabled = False
        stopCameraAndTimer()
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        MemberFormDialog.IsOpen = True
        MemberForm.clearAll()
        FAB.IsEnabled = False
        stopCameraAndTimer()
    End Sub

    Private Sub Dialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing,
                BookFormDialog.DialogClosing
        if BookFormDialog.IsOpen = False OR MemberFormDialog.IsOpen = False
            FAB.IsEnabled = True
        End If
        StopCameraAndTimer()
        startCameraAndTimer()
    End Sub

    Private Sub Dialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) _
        Handles MemberPopupDialog.DialogOpened, AdminPopupDialog.DialogOpened, ViewBookDialog.DialogOpened
        SnackBarMessageQueue = Snackbar.MessageQueue
        FAB.IsEnabled = False
        StartCameraAndTimer()
    End Sub


    Public Async Sub Dialog_PopupClosing() Handles ViewBookDialog.DialogClosing, MemberPopupDialog.DialogClosing, AdminPopupDialog.DialogClosing, MemberPopup.Unloaded, BookView.Unloaded, AdminPopup.Unloaded
        StopCameraAndTimer()
        Await Task.Delay(200)
        StartCameraAndTimer()
        FAB.IsEnabled = true
    End Sub

    Private Sub ViewBookDialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) Handles Me.Unloaded
        StopCameraAndTimer()
        FAB.IsEnabled = true
    End Sub

    Private Sub DashBoard_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        StopCameraAndTimer()
        FAB.IsEnabled = true
    End Sub

    Private Async Sub DashBoard_GotFocus(sender As Object, e As RoutedEventArgs) Handles Me.Loaded,MemberPopupDialog.DialogClosing,MemberFormDialog.DialogClosing,BookFormDialog.DialogClosing,ViewBookDialog.DialogClosing,MemberForm.Unloaded,BookForm.Unloaded,MemberPopup.Unloaded
        Me.lblTotalMembers.Text = Await MemberService.TotalMembers()
        Me.lblTotalIssued.Text = Await BookService.TotalBooksIssued()
        Me.lblTotalBooks.Text = Await BookService.TotalBooks()

    End Sub
End Class