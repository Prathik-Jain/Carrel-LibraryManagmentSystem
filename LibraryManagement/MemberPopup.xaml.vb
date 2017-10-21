Imports System.Data
Imports System.Windows.Threading
Imports Newtonsoft.Json
Imports  System.Threading
Public Class MemberAccount
    Dim _dashBoard As DashBoard
    Dim data as ArrayList
    WithEvents _sendImage As New DispatcherTimer
    Dim _camera As Camera
    public Shared x as DataView
    Public sub New 

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        For Each window As Window In Application.Current.Windows
            If window.GetType() = GetType(DashBoard)
                _dashBoard = window
            End If
        Next

    End Sub 
    Public sub GetData(uid As String)
        try
            data = MemberService.GetMember(UID)
            x =BookService.GetBooksBorrowed(uid)
            borrowedlist.ItemsSource= x
            LblUID.Content = UID
            LblName.Content = data(0) + " " + data(1)
            LblPhone.Content = data(2)
            LblDepartment.Content = data(3)
            LblSemester.Content = data(4)
            ImgQr.source = qrGenerator.generate(lblUID.content)
            _dashBoard.MemberPopup.Content = Me
            _dashBoard.MemberPopupDialog.IsOpen = True
        Catch ex As Exception
            msgbox("Member not found")
        End Try
    End sub
    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        _dashBoard.MemberPopupDialog.IsOpen = False
        _dashBoard.MemberForm.TxtFirstName.Text = data(0)
        _dashBoard.MemberForm.TxtLastName.Text = data(1)
        _dashBoard.MemberForm.TxtPhone.Text = LblPhone.Content
        _dashBoard.MemberForm.CmbDept.Text = LblDepartment.Content
        _dashBoard.MemberForm.CmbSemister.Text = LblSemester.Content
        _dashBoard.MemberForm.LblUID.Content = LblUID.Content
        _dashBoard.MemberForm.BtnAccept.Content = "UPDATE"
        _dashBoard.MemberFormDialog.IsOpen = True
    End Sub

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
    Private Sub OpenDeleteDialog(sender As Object, e As RoutedEventArgs)
        DeleteMember.SetData("MEM",lbluid.Content)
        DeleteMember.LblPrompt.Content = "Delete Member?"
        DeleteMemberDialog.IsOpen= True
    End Sub
End Class

