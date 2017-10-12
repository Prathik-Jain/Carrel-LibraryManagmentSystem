Imports Newtonsoft.Json

Public Class MemberAccount
    Dim dashBoard As DashBoard = Application.Current.Windows(0)
    Dim admin As New Admin
    Public Sub GetData(UID As String)
        Dim data As New ArrayList
        data = MemberService.GetMember(UID)
        LblUID.Content = UID
        LblName.Content = data(0) + "" + data(1)
        LblPhone.Content = data(2)
        LblDepartment.Content = data(3)
        LblSemester.Content = data(4)
        Dim Borrowed = JsonConvert.DeserializeObject(data(6))
        Dim BookList As New List(Of Book)
        Dim count = 1
        For Each item As Linq.JObject In Borrowed
            BookList.Add(New Book With {.Sl=count, .BookID = item.GetValue("UID"), .BorrowedOn = item.GetValue("On"), .Title = item.GetValue("Title")})
            count += 1
        Next
        BorrowedList.ItemsSource = BookList
        dashBoard.MemberPopup.Content = Me
        dashBoard.MemberPopupDialog.IsOpen = True
    End Sub

End Class
Class Book
    Property Sl
    Property BookID
    Property Title
    Property BorrowedOn
End Class
