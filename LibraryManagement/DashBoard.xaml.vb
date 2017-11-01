Imports System.ComponentModel
Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json

Public Class DashBoard
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue
    WithEvents _sendImage As DispatcherTimer
    Dim _camera As Camera
    Public memberAccount As New MemberPopup

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        StartCameraAndTimer()
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
        Dim qrDecoder As New QrDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQr(_camera.Frame)
        Try
            If jsonString <> "" Then
                StopCameraAndTimer()
                QrScanned(jsonString)
                qrDecoder.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Async Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        If MemberPopupDialog.IsOpen Then

            Try
                If jsonString.Contains("BOOK") Then
                    If Await memberAccount.CheckBookInList(jsonString) Then 'Checks and returns book if found
                        memberAccount.GetData(memberAccount.LblUID.Content)
                        StartCameraAndTimer()
                    ElseIf Await BookService.Borrowed(jsonString, memberAccount.LblUID.Content) Then
                        memberAccount.GetData(memberAccount.LblUID.Content)
                        StartCameraAndTimer()
                    Else
                        MsgBox("Failed to Update database")
                    End If
                ElseIf jsonString.Contains("MEM") Then
                    MemberPopupDialog.IsOpen = False
                Else
                    MsgBox("Please scan a book to add or return")
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            End Try
        ElseIf AdminPopupDialog.IsOpen Then

            Try
                If jsonString.Contains("ADM") Then
                    StopCameraAndTimer()
                    Me.Close()
                End If
            Catch ex As Exception
                MsgBox(ex.Message.ToString())
            End Try
        ElseIf ViewBookDialog.IsOpen Then

            Try
                If jsonString.Contains("BOOK") Then
                    ViewBookDialog.IsOpen = False
                End If
            Catch ex As Exception

            End Try
        Else
            If jsonString.Contains("MEM") Then
                memberAccount.GetData(jsonString)
            ElseIf jsonString.Contains("ADM") Then
                Dim _admin = JsonConvert.DeserializeObject(Of Admin)(jsonString)
                AdminPopup.GetData(_admin.Uid)
            ElseIf jsonString.Contains("BOOK") Then
                Dim viewBook As New ViewBook
                viewBook.ViewBookById(jsonString)
            End If
        End If

    End Sub



    Public Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        BookFormDialog.IsOpen = True
        BookForm.ClearAll()
        FAB.IsEnabled = False
        StopCameraAndTimer()
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        MemberFormDialog.IsOpen = True
        MemberForm.clearAll()
        FAB.IsEnabled = False
        StopCameraAndTimer()
    End Sub

    Private Sub Dialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing,
                BookFormDialog.DialogClosing
        If BookFormDialog.IsOpen = False Or MemberFormDialog.IsOpen = False Then
            FAB.IsEnabled = True
        End If
        StopCameraAndTimer()
        StartCameraAndTimer()
    End Sub

    Private Sub Dialog_DialogOpened(sender As Object, eventArgs As DialogOpenedEventArgs) _
        Handles MemberPopupDialog.DialogOpened,
                AdminPopupDialog.DialogOpened,
                ViewBookDialog.DialogOpened
        SnackBarMessageQueue = Snackbar.MessageQueue
        FAB.IsEnabled = False
        StartCameraAndTimer()
    End Sub


    Public Async Sub Dialog_DialogClosing() _
        Handles ViewBookDialog.DialogClosing,
                BookView.Unloaded,
                MemberPopupDialog.DialogClosing,
                MemberPopup.Unloaded,
                AdminPopupDialog.DialogClosing,
                AdminPopup.Unloaded
        StopCameraAndTimer()
        Await Task.Delay(200)
        StartCameraAndTimer()
        FAB.IsEnabled = True
    End Sub

    Private Sub CloseEvent () _
        Handles Me.Unloaded,
                Me.Closing
        StopCameraAndTimer()
        FAB.IsEnabled = True
    End Sub

    Private Async Sub DashBoard_GotFocus(sender As Object, e As RoutedEventArgs) _
        Handles Me.Loaded,
                MemberPopupDialog.DialogClosing,
                MemberPopup.Unloaded,
                MemberFormDialog.DialogClosing,
                MemberForm.Unloaded,
                BookFormDialog.DialogClosing,
                BookForm.Unloaded,
                ViewBookDialog.DialogClosing,
                BookView.Unloaded
        Me.LblTotalMembers.Text = Await MemberService.TotalMembers()
        Me.LblTotalIssued.Text = Await BookService.TotalBooksIssued()
        Me.LblTotalBooks.Text = Await BookService.TotalBooks()
    End Sub
End Class