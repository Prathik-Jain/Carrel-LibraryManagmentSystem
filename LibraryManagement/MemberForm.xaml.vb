Public Class MemberForm
    Private Async Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click
        Dim member as New Member With {
                .FName = TxtFirstName.Text,
                .Lname = TxtLastName.Text,
                .Phone = TxtPhone.Text,
                .Dept = TxtDepartment.Text,
                .Sem = TxtSemester.Text
                }
        If (Await MemberService.AddMember(member))
            MsgBox("Added")
        Else
            MsgBox("Not Added")
        End If
    End Sub
End Class
