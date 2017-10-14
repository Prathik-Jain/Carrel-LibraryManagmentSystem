Imports Newtonsoft.Json

Public Class ViewBook
    Public sub New
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    Dim _x As DashBoard
    Dim _data = New ArrayList

     Friend Sub ViewLastRecord(number As Integer)
        _x = Application.Current.Windows(0)
        _data = BookService.GetLastAdded()
        UpdateView(_data)
        LblNoofBooks.Content = number
        _x.BookView.Content = Me
        _x.ViewBookDialog.IsOpen = True
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
        _x.BookView.Content = me
        _x.ViewBookDialog.IsOpen =True
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
End Class
