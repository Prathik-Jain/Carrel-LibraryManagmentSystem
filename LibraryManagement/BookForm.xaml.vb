Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Public Class BookForm
    Public Sub New
        ' This call is required by the designer.
        InitializeComponent()
        If DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
            Return
        Else
            'Executes when BookForm is loaded - Not accessed by designer
            CmbAuthor.ItemsSource = AuthorService.GetAuthors()
        End If

    End Sub
    ''' <summary>
    ''' This sub procedure adds the selected Author in the combobox to Author List when <c>Tab</c> is pressed, and clears the combobox list later.
    ''' </summary>
    Private Sub CmbAuthor_KeyDown(sender As Object, e As Input.KeyEventArgs) Handles CmbAuthor.KeyDown
        If e.Key = Key.Tab Then
            If Trim(CmbAuthor.Text) <> "" Then
                LstAuthor.Items.Add(CmbAuthor.Text)
                CmbAuthor.Text = ""
                Keyboard.Focus(CmbAuthor)
            End If
        End If
    End Sub
    ''' <summary>
    ''' This sub procedure adds the selected Author in the combobox to Author List when an item is selected and DropDown is closed, alse clears the combobox list later.
    ''' </summary>
    Private Sub CmbAuthor_DropDownClosed() Handles CmbAuthor.DropDownClosed
        If Trim(CmbAuthor.Text) <> "" Then
            LstAuthor.Items.Add(CmbAuthor.Text)
            CmbAuthor.Text = ""
            Keyboard.Focus(CmbAuthor)
        End If
    End Sub

    Private Sub BtnAccept_Click() Handles BtnAccept.Click
        AddBook()
        AddAuthor()
    End Sub
    ''' <summary>
    ''' Converts the author list into JSON and creates a dynamic object of book, which is then passed to <see cref="BookService.AddBook(Object, Integer)"/> if the hidden label is empty else calls <see cref="BookService.EditBooks(Object)"/>
    ''' </summary>
    ''' <seealso cref="BookService"/>
    Private Async Sub AddBook()
        Dim book As Object = New Linq.JObject
        Book.ISBN = TxtISBN.Text
        Book.Title = TxtTitle.Text
        Book.Author = JsonConvert.SerializeObject(LstAuthor.Items)
        Book.Publisher = TxtPublisher.Text
        Book.Edition = CmbEdition.Text
        Book.Price = TxtPrice.Text
        Book.Rack = TxtRack.Text
        Dim number As Integer = Convert.ToInt32(TxtNumber.Text)
        If lblUid.Content = ""
            If Await BookService.AddBook(Book, number) Then
                DashBoard.SnackBarMessageQueue.Enqueue(TxtNumber.Text + " Book(s) Added", "PRINT QR", Sub()
                     MsgBox("This is an upcoming feature")
                End Sub)
            Else
                DashBoard.SnackBarMessageQueue.Enqueue("Failed registering ")
            End If
        Else
            ' TODO  Work on EditBooks
            If Await BookService.EditBooks(Book) Then
                DashBoard.SnackBarMessageQueue.Enqueue(TxtNumber.Text + " Book(s) Added", "VIEW", Sub()
                    Dim bookView As New ViewBook
                    BookView.ViewLastRecord(number)
                End Sub)
            Else
                DashBoard.SnackBarMessageQueue.Enqueue("Failed updating ")
            End If 
        End If
      
    End Sub
    ''' <summary>
    ''' This sub procedure is used to add Author to the database from list - if not already present in the combobox dropdown. Calls <see cref="AuthorService.AddAuthor(String)"/> to add author.
    ''' </summary>
    ''' <seealso cref="AuthorService"/>
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
                AuthorService.AddAuthor(item.ToString)
            End If
        Next
    End Sub
#Region "ValidationOnLostFocus"
    Private Sub FieldLostFocus(sender As Object, e As RoutedEventArgs)
        Dim txt = DirectCast(sender, TextBox)
        txt.GetBindingExpression(TextBox.TextProperty).UpdateSource()
    End Sub
#End Region
#Region "Input Constrations"
    Private Sub Numeric_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtISBN.PreviewTextInput,
                TxtPrice.PreviewTextInput,
                TxtNumber.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub AlphaNumberic_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtRack.PreviewTextInput,
                TxtTitle.PreviewTextInput,
                TxtPublisher.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z0-9]+$")
    End Sub


#End Region
    ''' <summary>
    ''' Removes the author from selected list when the cross button (X) is pressed.
    ''' </summary>
    Private Sub RemoveAuthorFromList(sender As Object, e As RoutedEventArgs)
        LstAuthor.Items.Remove(sender.Content)
    End Sub
''' <summary>
''' Clears all the fields in BookForm.
''' </summary>
    Public Sub ClearAll()
        LblUID.Content = ""
        TxtPublisher.Clear()
        TxtISBN.Clear
        TxtNumber.Clear()
        TxtPrice.Clear()
        TxtTitle.Clear()
        TxtRack.Clear()
        LstAuthor.items.Clear()
        CmbAuthor.Text = ""
        CmbEdition.Text =""
        BtnAccept.Content = "ADD"
    End Sub
End Class
