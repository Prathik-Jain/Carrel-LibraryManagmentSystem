Imports System.Data
Imports System.Windows.Threading
Imports Newtonsoft.Json
Imports  System.Threading
Public Class MemberAccount
    Dim _dashBoard As DashBoard = Application.Current.Windows(0)
    WithEvents _sendImage As New DispatcherTimer
    Dim _camera As Camera
    public Shared x as DataView
    Public sub New 

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub 
    Public sub GetData(uid As String)
        dim data as ArrayList = MemberService.GetMember(UID)
        x =BookService.GetBooksBorrowed(uid)
            borrowedlist.ItemsSource= x
            LblUID.Content = UID
            LblName.Content = data(0) + "" + data(1)
            LblPhone.Content = data(2)
            LblDepartment.Content = data(3)
            LblSemester.Content = data(4)
           _dashBoard.MemberPopup.Content = Me
           _dashBoard.MemberPopupDialog.IsOpen = True
    End sub

    Public Async Function CheckBookInList() As Task(Of Boolean)
        dim flag = False
        for each item in BorrowedList.Items
            If item.row("UID").ToString().Contains("BOOK-2")
               Await BookService.Returned("BOOK-2")
                FLAG =1
            End If
        Next
       Return FLAG
    End Function

End Class

