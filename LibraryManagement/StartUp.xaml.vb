Class StartUp

    Private Async Sub Me_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Await Task.Delay(8000)
        Dim login as new Login
        login.Show()
        me.Close()
    End Sub
End Class