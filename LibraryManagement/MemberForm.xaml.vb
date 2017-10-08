Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json
Public Class MemberForm
    Private Async Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click

        Dim member As Object = New Linq.JObject()
                member.FName = TxtFirstName.Text
                member.LName = TxtLastName.Text
                member.Phone = TxtPhone.Text
                member.Dept = TxtDepartment.Text
                member.Sem = TxtSemester.Text
        If Await MemberService.AddMember(member) Then
            DashBoard.SnackBarMessageQueue.Enqueue("Registered "+ TxtFirstName.Text+ " as Member.", "UNDO", Sub()
                MsgBox("Undo Clicked")
            End Sub)
        Else
            DashBoard.SnackBarMessageQueue.Enqueue("Failed registering "+ TxtFirstName.Text) 
        End If
    End Sub
End Class
