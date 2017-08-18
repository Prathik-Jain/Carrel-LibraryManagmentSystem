Imports System.Windows.Media.Animation
'Commented things for personal use do not DELETE
Class MainWindow
    Dim showWin As Storyboard
    Dim hideWin As Storyboard
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'Me.Background = New SolidColorBrush(Colors.Red)
        'BeginStoryboard(showWin)
        'grabs contents of page1
        Dim page1 As New Page1()
        Me.Content = page1
    End Sub
    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        'showWin = Me.Resources("showWin")
        'hideWin = Me.Resources("hideWin")
    End Sub
End Class
