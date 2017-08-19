Imports System.Windows.Media.Animation

Class Page1
    Dim mw As New MainWindow
    Dim movetxt As Storyboard
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        BeginStoryboard(movetxt)
        txtWelcome.Text = "Welcome, John Doe"
        txtLoginInstruction.Text = "Please enter your PIN"
    End Sub

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        movetxt = Me.Resources("moveText")
    End Sub

    Private Sub Close_Click_1(sender As Object, e As RoutedEventArgs) Handles Close.Click
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown
        Application.Current.Shutdown()
    End Sub
End Class
