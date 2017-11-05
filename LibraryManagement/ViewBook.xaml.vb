Imports Newtonsoft.Json

Public Class ViewBook
    Public sub New
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    Dim _dashboard As DashBoard
    Dim _data = New ArrayList
''' <summary>
'''  Opens the <c>ViewBookDialog</c> and sets the fields to the data obtained from <see cref="BookService.GetLastAdded()"/> function.
''' </summary>
''' <param name="number">Total number of books last added.</param>
     Friend Sub ViewLastRecord(number As Integer)
        _dashboard = Application.Current.Windows(0)
        _data = BookService.GetLastAdded()
        UpdateView(_data)
        BtnEdit.Visibility = Visibility.Collapsed
        ImgQR.Visibility = Visibility.Collapsed
        _dashboard.BookView.Content = Me
        _dashboard.ViewBookDialog.IsOpen = True
    End Sub
    ''' <summary>
    ''' Shows the details of book with Unique ID
    ''' </summary>
    ''' <param name="bookId">Unique ID of the book</param>
    Friend Sub ViewBookById(bookId As String)


        _dashboard = Application.Current.Windows(0)
        Try
            _data = bookservice.GetBookByID(bookID)
            UpdateView(_data)
            LblNoofBooks.Text = BookService.GetNumber(_data(0))
            LblAvailable.Text = BookService.GetAvailable(_data(0))
            LblUID.Content = bookID
            ImgQR.source = QRGenerator.Generate(LblUID.Content)
            if (_data(7)) 'Available
                LblBorrowed.Text = "BOOK NOT ISSUED"
                Borrowed.Background = New SolidColorBrush(color.FromRgb(127,255,0))
            Else
                LblBorrowed.Text = "Borrowed By " + MemberService.GetMemberById(_data(8)) + " [" + _data(8) + "] On " + _data(9)
                Borrowed.Background = New  SolidColorBrush(Color.FromRgb(227,1,1))
            End If
            _dashboard.BookView.Content = me.content
            _dashboard.ViewBookDialog.IsOpen =True
        Catch ex As Exception
            MsgBox("Book not found!")
            _dashboard.StartCameraAndTimer()
        End Try

    End Sub
    ''' <summary>
    ''' This is the default data that is to be viewed.
    ''' </summary>
    ''' <param name="data">Data obtained of either <see cref="ViewBookById(String)"/> or <see cref="ViewLastRecord(Integer)"/></param>
    Sub UpdateView(data)
        LblISBN.Content = data(0)
        LblTitle.Content = data(1)
        LblAuthors.Content = data(2)
        LblPublisher.Content = data(3)
        LblRack.Content = data(6)
    End Sub
    ''' <summary>
    ''' This function is used to autofill the <c>BookForm</c> with data from <c>BookView</c> and open the BookForm.
    ''' The <c>TxtISBN</c> field is disabled in this case
    ''' </summary>
    Private Sub BtnEdit_Click() Handles BtnEdit.Click
        _dashBoard.ViewBookDialog.IsOpen = False
        _dashBoard.BookForm.TxtISBN.Text = LblISBN.Content
        _dashboard.BookForm.TxtISBN.IsEnabled = False
        _dashBoard.BookForm.TxtTitle.Text = LblTitle.Content
        _dashboard.BookForm.LstAuthor.Items.Clear()
        _dashBoard.BookForm.TxtPublisher.Text = LblPublisher.Content
        _dashboard.BookForm.CmbEdition.Text = _data(4)
        _dashboard.BookForm.LblUID.Content = "UPDATE"
        _dashBoard.BookForm.BtnAccept.Content = "UPDATE"
        _dashboard.BookForm.NumberIcon.Visibility = Visibility.Collapsed
        _dashboard.BookForm.TxtNumber.Visibility = Visibility.Collapsed
        _dashboard.BookForm.TxtNumber.Text = "1"
        _dashBoard.BookFormDialog.IsOpen = True
    End Sub
    ''' <summary>
    ''' Opens up the Delete Dialog - with a prompt.
    ''' </summary>
    Private Sub OpenDeleteDialog()
        DeleteBook.SetData("BOOK",LblUID.Content)
        DeleteBook.LblPrompt.Content = "Delete Books?"
        DeleteBookDialog.IsOpen= True
    End Sub
    ''' <summary>
    ''' Stops the camera and timer when Delete Dialog is opened.
    ''' </summary>
    Private  sub StopcameraonDialogOpen Handles  DeleteBookDialog.DialogOpened
        _dashBoard.StopCameraAndTimer()
    End sub
End Class
