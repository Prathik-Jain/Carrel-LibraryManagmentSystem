Imports System.Windows.Threading
Imports Newtonsoft.Json

Public Class MemberAccount
    Dim dashBoard As DashBoard = Application.Current.Windows(0)
    Dim admin As New Admin
    WithEvents SendImage As DispatcherTimer
    Dim camera As Camera
    Public sub New

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub GetData(UID As String)
        Dim data As New ArrayList
        data = MemberService.GetMember(UID)
        BorrowedList.ItemsSource = BookService.GetBooks(UID)
        LblUID.Content = UID
        LblName.Content = data(0) + "" + data(1)
        LblPhone.Content = data(2)
        LblDepartment.Content = data(3)
        LblSemester.Content = data(4)
        dashBoard.MemberPopup.Content = Me
        dashBoard.MemberPopupDialog.IsOpen = True
    End Sub
    Public Sub StartCameraAndTimer()
        camera = New Camera
        camera.StartCamera()
        SendImage = New DispatcherTimer With {
            .Interval = New TimeSpan(0, 0, 0.2)
            }
        SendImage.Start()
    End Sub

    Public Sub StopCameraAndTimer()
        camera.StopCamera()
        SendImage.Stop()
    End Sub

    Public Sub SendImage_Tick(sender As Object, e As EventArgs) Handles SendImage.Tick
        Dim qrDecoder As New QRDecoder
        Dim jsonString = ""
        jsonString = qrDecoder.ScanQR(camera.frame)
        Try
            If jsonString <> "" Then
                StopCameraAndTimer
                QrScanned(jsonString:=jsonString)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Async Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        Try
            If jsonString.Contains("UID") Then
                if AWAIT CheckBookInList 'Checks and returns book if found
                    updateList("MEM-0001")
                ElseIf await BookService.AddBorrower("BOOK-2","MEM-0001") 
                    updateList("MEM-0001")
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

    Private async Sub updateList(memberID As String)
        BorrowedList.ItemsSource =BookService.GetBooks("MEM-0001")
        Await Task.Delay(50)
        StartCameraAndTimer
    End Sub

    Private Async Function CheckBookInList() As Task(Of Boolean)
        dim FLAG = False
        for each item in BorrowedList.Items
            If item.BookID.ToString().Contains("BOOK-2")
               Await BookService.Return("BOOK-2")
                FLAG =1
            End If
        Next
       Return FLAG
    End Function

    Private Sub MemberAccount_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        StartCameraAndTimer

    End Sub
End Class
Class Book
    Property Sl
    Property BookID
    Property Title
    Property BorrowedOn
End Class
