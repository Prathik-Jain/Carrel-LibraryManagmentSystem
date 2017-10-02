Imports MaterialDesignThemes.Wpf

Public Class DashBoard
    Private Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        BookFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        MemberFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BookFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) Handles BookFormDialog.DialogClosing
        FAB.IsEnabled=True
    End Sub

    Private Sub MemberFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) Handles MemberFormDialog.DialogClosing
        FAB.IsEnabled=True
    End Sub
End Class
