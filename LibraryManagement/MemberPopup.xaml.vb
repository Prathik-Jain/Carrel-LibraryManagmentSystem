Imports System.Data
Imports System.Windows.Threading
Imports Newtonsoft.Json
Imports System.Threading
Imports MaterialDesignThemes.Wpf

Public Class MemberPopup
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
    ''' <summary>
    ''' Gets the data about a member. Calls <see cref="MemberService.GetMemberById(String)"/>
    ''' </summary>
    ''' <param name="uid">Unique ID of the member</param>
    ''' <seealso cref="MemberService"/>
    Public sub GetData(uid As String)
        try
            data = MemberService.GetMember(uid)
            x = BookService.GetBooksBorrowed(uid)
            borrowedlist.ItemsSource = x
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
            _dashBoard.StartCameraAndTimer()
        End Try
    End sub
    ''' <summary>
    ''' Used to edit a member - Opens the member form - Auto fills all the  fileds - Grabs data from Member Popup.
    ''' </summary>
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
    ''' <summary>
    ''' This function is used to check if the book is already present in the borrowedlist of the member 
    ''' <para>If already present <see cref="BookService.Returned(String)"/>is called to return the book</para>
    ''' </summary>
    ''' <param name="Bookid">Unique ID of the book</param>
    ''' <returns>Returns <c>One</c> if book already present, else returns <c>Zero</c></returns>
    Public Async Function CheckBookInList(Bookid As String) As Task(Of Boolean)
        dim flag = False
        for each item in BorrowedList.Items
            If item.row("UID").ToString().Contains(Bookid)
                Await BookService.Returned(Bookid)
                FLAG = 1
            End If
        Next
        Return FLAG
    End Function
    ''' <summary>
    ''' This function is called when Delete button is clicked. This sets the category and the Unique ID in <see cref="Delete.SetData(String, String)"/> and pops up a confirmation.
    ''' </summary>
    ''' <seealso cref="Delete"/>
    Private Sub OpenDeleteDialog()
        DeleteMember.SetData("MEM", lbluid.Content)
        DeleteMember.LblPrompt.Content = "Delete Member?"
        DeleteMemberDialog.IsOpen = True
    End Sub
    ''' <summary>
    ''' Grabs all the data and sets to <see cref="Card"/>
    ''' <para> Opes the print dialog once done.</para>
    ''' </summary>
    Private Sub OpenPrintDialog()
        PrintMember.ImgQR.Source = ImgQR.Source
        PrintMember.LblDepartment.Content = LblDepartment.Content
        PrintMember.LblPhone.Content = LblPhone.Content
        PrintMember.LblName.Content = LblName.Content
        PrintMember.LblUID.content = LblUID.Content
        PrintMember.LblCat.Content = "MEMBER"
        Dim printdlg As new PrintDialog
        if printDlg.ShowDialog() = true
            printDlg.PrintVisual(PrintMember, "User Control Printing.")
        End If
    End Sub
    ''' <summary>
    ''' Stops the camera and timer in dashboard if <c>DeleteDialog</c> is open.
    ''' </summary>
    ''' <seealso cref="DashBoard"/>
    Private Sub DeleteMemberDialog_DialogOpened() Handles DeleteMemberDialog.DialogOpened
        _dashBoard.StopCameraAndTimer
    End Sub

End Class

