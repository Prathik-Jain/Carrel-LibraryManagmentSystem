Class StartUp
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim login as new Login
        login.Show()
        me.Close()
    End Sub

    Private Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
    End Sub
End Class