Public Class CardHolder
    Private Sub BtnPrint_Click(sender As Object, e As RoutedEventArgs) Handles BtnPrint.Click
        card.Measure(new Size(206,365))
        card.Arrange(new Rect(new Size(816, 1056)))
        card.UpdateLayout
        Dim printDlg As New PrintDialog
        printDlg.PrintVisual(card,"Card" )
    End Sub
End Class
