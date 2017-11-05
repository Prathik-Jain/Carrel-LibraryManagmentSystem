Imports System.Text.RegularExpressions

Public Class ChangeSemDialog
    Public Sub New
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    ''' <summary>
    ''' Input constraints to enter only alphabets.
    ''' </summary>
    Private Sub TxtSem_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtSem.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")

    End Sub
    ''' <summary>
    ''' Checks if the Text entered is <c>SEMESTER</c>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TxtSem_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TxtSem.TextChanged
        If TxtSem.Text = "SEMESTER"
            btnok.IsEnabled = True
        Else 
            Btnok.IsEnabled = False
        End If
    End Sub
    ''' <summary>
    ''' Calls <see cref="AdminService.ChangeSem()"/> when ok is clicked.
    ''' </summary>
    Private Sub Btnok_Click(sender As Object, e As RoutedEventArgs) Handles Btnok.Click
        AdminService.ChangeSem()
    End Sub
End Class
