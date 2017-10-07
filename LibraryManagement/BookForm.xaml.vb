Imports Newtonsoft.Json
Public Class BookForm
    Dim authorService As New AuthorService
    Private Sub BookForm_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CmbAuthor.ItemsSource = AuthorService.GetAuthors()
    End Sub

    Private Sub CmbAuthor_KeyDown(sender As Object, e As Input.KeyEventArgs) Handles CmbAuthor.KeyDown
        If e.Key = Key.Tab Then
            If Trim(CmbAuthor.Text) <> "" Then
                LstAuthor.Items.Add(CmbAuthor.Text)
                CmbAuthor.Text = ""
                Keyboard.Focus(CmbAuthor)
            End If
        End If
    End Sub

    Private Sub CmbAuthor_DropDownClosed(sender As Object, e As EventArgs) Handles CmbAuthor.DropDownClosed
        If Trim(CmbAuthor.Text) <> "" Then
            LstAuthor.Items.Add(CmbAuthor.Text)
            CmbAuthor.Text = ""
            Keyboard.Focus(CmbAuthor)
        End If
    End Sub

    Private Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click
        AddBook()
        AddAuthor()
    End Sub
    Private Async Sub AddBook()
        Dim Book As Object = New Linq.JObject
        Book.ISBN = TxtISBN.Text
        Book.Title = TxtTitle.Text
        Book.Author = JsonConvert.SerializeObject(LstAuthor.Items)
        Book.Publisher = TxtPublisher.Text
        Book.Edition = TxtEdition.Text
        Book.Price = TxtPrice.Text
        Book.Rack = TxtRack.Text
        Dim number As Integer = Convert.ToInt32(TxtNumber.Text)
        If Await BookService.AddBook(Book, number) Then
            DashBoard.SnackBarMessageQueue.Enqueue("Added  BOOKS", "UNDO", Sub()
                                                                               MsgBox("Undo Clicked")
                                                                           End Sub)
        Else
            DashBoard.SnackBarMessageQueue.Enqueue("Failed registering ")
        End If
    End Sub
    Private Sub AddAuthor()
        Dim i = 0
        For Each item In LstAuthor.Items
            Dim flag = 1
            For i = 0 To CmbAuthor.Items.Count - 1
                If item = CmbAuthor.Items(i) Then
                    flag = 0
                    Exit For
                End If
            Next
            If flag Then
                authorService.AddAuthor(item.ToString)
            End If
        Next
    End Sub

End Class
