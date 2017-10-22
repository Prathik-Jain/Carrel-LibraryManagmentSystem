Imports System.Text.RegularExpressions

Public Class ChangeSemDialog
    Public Sub New
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub TxtSem_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtSem.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")

    End Sub

    Private Sub TxtSem_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TxtSem.TextChanged
        If TxtSem.Text = "SEMISTER"
            btnok.IsEnabled = True
        Else 
            Btnok.IsEnabled = False
        End If
    End Sub

    Private Sub Btnok_Click(sender As Object, e As RoutedEventArgs) Handles Btnok.Click
        AdminService.ChangeSem()
    End Sub
End Class
