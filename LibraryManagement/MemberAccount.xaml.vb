Imports System.Windows.Threading
Imports Newtonsoft.Json
Imports  System.Threading
Public Class MemberAccount
    Dim _dashBoard As DashBoard = Application.Current.Windows(0)
    WithEvents _sendImage As New DispatcherTimer
    Dim _camera As Camera
    Public sub New 

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub 
    Public sub GetData(uid As String)
        dim data as ArrayList = MemberService.GetMember(UID)
        Dim c = BookService.GetBooksBorrowed(UID)
        me.Dispatcher.Invoke( Sub()
            borrowedlist.ItemsSource= c
            LblUID.Content = UID
            LblName.Content = data(0) + "" + data(1)
            LblPhone.Content = data(2)
            LblDepartment.Content = data(3)
            LblSemester.Content = data(4)
           _dashBoard.MemberPopup.Content = Me
           _dashBoard.MemberPopupDialog.IsOpen = True
                                  _sendImage.Start()
        End Sub) 
    End sub

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
       dim jsonString = qrDecoder.ScanQR(_camera.frame)
        Try
            If jsonString <> "" Then
                'StopCameraAndTimer
                _sendImage.Stop()
                QrScanned(jsonString)
                qrDecoder.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        Try
            If jsonString.Contains("UID") Then
                if CheckBookInList 'Checks and returns book if found
                         GetData("MEM-0001")
                ElseIf BookService.Borrowed("BOOK-2","MEM-0001") 
                        GetData("MEM-0001")
                    '_sendImage.Start()
                Else 
                    MsgBox("Failed to Update database")
                End If
            else
                MsgBox("Please scan a book to add or return")
            End If
        Catch ex As Exception
            msgbox(ex.ToString())
        End Try

        
    End Sub

    'Private async Sub UpdateList(memberId As String)
    '    BorrowedList.ItemsSource = await task.run(Function()
    '                                                        Return BookService.GetBooksBorrowed(memberId)
    '                                        End Function)
    '    StartCameraAndTimer
    'End Sub

    Private Function CheckBookInList() As Boolean
        dim flag = False
        for each item in BorrowedList.Items
            If item.BookID.ToString().Contains("BOOK-2")
               BookService.Returned("BOOK-2")
                FLAG =1
            End If
        Next
       Return FLAG
    End Function

    Private Sub MemberAccount_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        '_sendImage.Start()
        StartCameraAndTimer
    End Sub

End Class
public Class  Book
    public    Property Sl
    Property BookId
    Property Title
    Property BorrowedOn
End Class
