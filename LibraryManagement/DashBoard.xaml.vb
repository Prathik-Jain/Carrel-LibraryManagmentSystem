Public Class DashBoard
    Private Sub BtnMember_OnClick(sender As Object, e As RoutedEventArgs)
        MemberForm.IsOpen = True
    End Sub

    Private Sub BtnBook_OnClick(sender As Object, e As RoutedEventArgs)
        BookForm.IsOpen = True
    End Sub

End Class
