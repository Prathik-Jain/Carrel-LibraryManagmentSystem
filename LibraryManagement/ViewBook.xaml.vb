Public Class ViewBook
     Friend Sub UpdateView(number As Integer)
        Dim x As DashBoard
        x = Application.Current.Windows(0)
        Dim data = New ArrayList
        data = BookService.PrintLastAdded()
        LblISBN.Content = data(0)
        LblTitle.Content = data(1)
        LblAuthors.Content = data(2)
        LblPublisher.Content =data(3)
        LblEdition.Content = data(4)
        LblPrice.Content = data(5)
        LblRack.Content = data(6)
        LblNoofBooks.Content = number
        x.BookView.Content = Me
        x.ViewBookDialog.IsOpen = True
    End Sub
End Class
