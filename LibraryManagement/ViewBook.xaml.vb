Imports Newtonsoft.Json

Public Class ViewBook
    Public sub New
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    Dim _dashboard As DashBoard
    Dim _data = New ArrayList

     Friend Sub ViewLastRecord(number As Integer)
        _dashboard = Application.Current.Windows(0)
        _data = BookService.GetLastAdded()
        UpdateView(_data)
        LblNoofBooks.Content = number
        _dashboard.BookView.Content = Me
        _dashboard.ViewBookDialog.IsOpen = True
    End Sub
    Friend Sub ViewBookById(bookId As String)
        _data = bookservice.GetBookByID(bookID)
        UpdateView(_data)
        LblNoofBooks.Content = BookService.GetNumber(_data(0))
        'LblAvailable.Content = BookService.GetAvailable(data(0))
        'LblUID.Content = bookID
        Dim qrContent As Object = New Linq.JObject
        'qrContent.UID = LblUID.Content
        Dim qrString = JsonConvert.SerializeObject(qrContent)
        'ImgQR.source = QRGenerator.Generate(QRString)
        if(_data(7)) 'Available
            'LblBorrowedBy.Content = "BOOK IS NOT ISSUED"
            'LblBorrowedBy.color =green
            Else 
            'LblBorrowedBy.Content = MemberService.GetMemberByID(data(8)) + "[" + data(8) + "]"
            'LblBorrrowedOn.Content = data(9)
        End If
        _dashboard.BookView.Content = me
        _dashboard.ViewBookDialog.IsOpen =True
    End Sub

    Sub UpdateView(data)
        LblISBN.Content = data(0)
        LblTitle.Content = data(1)
        LblAuthors.Content = data(2)
        LblPublisher.Content =data(3)
        LblEdition.Content = data(4)
        LblPrice.Content = data(5)
        LblRack.Content = data(6)
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        _dashBoard.ViewBookDialog.IsOpen = False
        _dashBoard.BookForm.TxtISBN.Text = LblISBN.Content
        _dashBoard.BookForm.TxtTitle.Text = LblTitle.Content
        _dashBoard.BookForm.LstAuthor.ItemsSource = LblAuthors.Content
        _dashBoard.BookForm.TxtPublisher.Text = LblPublisher.Content
        _dashBoard.BookForm.CmbEdition.Text = LblEdition.Content
        '_dashBoard.BookForm.LblUID.Content = LblUID.Content
        _dashBoard.BookForm.BtnAccept.Content = "UPDATE"
        _dashBoard.MemberFormDialog.IsOpen = True
    End Sub
End Class
