Imports MaterialDesignThemes.Wpf

Public Class DashBoard
    Public Shared SnackBarMessageQueue as SnackbarMessageQueue

    Private Sub BtnAddBook_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddBook.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        BookFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BtnAddMember_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddMember.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        MemberFormDialog.IsOpen = True
        FAB.IsEnabled = False
    End Sub

    Private Sub BookFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles BookFormDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub


    Private Sub MemberFormDialog_DialogClosing(sender As Object, eventArgs As DialogClosingEventArgs) _
        Handles MemberFormDialog.DialogClosing
        FAB.IsEnabled = True
    End Sub
End Class
