Imports System.Data
Imports System.Windows.Forms

Public Class BookForm
    Dim authorService as new AuthorService
    Private Sub BookForm_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TxtAuthor.ItemsSource = authorService.GetAuthors()
    End Sub
End Class
