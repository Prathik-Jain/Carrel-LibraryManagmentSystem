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
                stopCameraAndTimer
                QrScanned(jsonString)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub QrScanned(jsonString As String)
        My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
        
        If jsonString.Contains("UID") Then
           if BookService.AddBorrower("BOOK-2",LblUID.Content.ToString) Then
                MsgBox("BookAdded")
                BorrowedList.ItemsSource = BookService.GetBooks("MEM-0001")
                startCameraAndTimer
           End If
            Else
            MsgBox("Scan a book only")
        End If
    End Sub

    Private Sub MemberAccount_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        startCameraAndTimer

    End Sub
End Class
Class Book
    Property Sl
    Property BookID
    Property Title
    Property BorrowedOn
End Class
