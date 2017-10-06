Imports MaterialDesignThemes.Wpf

Public Class MemberForm
    Private Async Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click

        Dim member as New Member With {
                .FName = TxtFirstName.Text,
                .LName = TxtLastName.Text,
                .Phone = TxtPhone.Text,
                .Dept = TxtDepartment.Text,
                .Sem = TxtSemester.Text
                }
        If Await MemberService.AddMember(member) Then
            DashBoard.SnackBarMessageQueue.Enqueue("Registered "+ TxtFirstName.Text+ " as Member.", "CARD",Sub ()
                                                                                                              Dim card As New Card
                                                                                                               card.UpdateCard
                                                                                                           End Sub)
        Else
            DashBoard.SnackBarMessageQueue.Enqueue("Failed registering "+ TxtFirstName.Text) 
        End If
    End Sub
End Class
