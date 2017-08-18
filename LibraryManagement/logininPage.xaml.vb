Imports System.Windows.Media.Animation

Class Page1
    Dim movetxt As Storyboard
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        BeginStoryboard(movetxt)
        txtWelcome.Text = "Welcome, John Doe"
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub
    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        movetxt = Me.Resources("moveText")
    End Sub
End Class
