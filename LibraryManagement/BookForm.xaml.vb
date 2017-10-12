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
        Book.Edition = CmbEdition.Text
        Book.Price = TxtPrice.Text
        Book.Rack = TxtRack.Text
        Dim number As Integer = Convert.ToInt32(TxtNumber.Text)
        If Await BookService.AddBook(Book, number) Then
            DashBoard.SnackBarMessageQueue.Enqueue(TxtNumber.Text + " Book(s) Added", "VIEW", Sub()
                                                                                                  Dim BookView As New ViewBook
                                                                                                  BookView.UpdateView(number)
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
#Region "InputConstraint"
    Private Sub TxtISBN_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtISBN.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtPublisher_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPublisher.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub

    Private Sub TxtPrice_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPrice.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtNumber_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtNumber.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtRack_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtRack.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z0-9]+$")
    End Sub

    Private Sub TxtTitle_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtTitle.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z0-9]+$")
    End Sub
#End Region
    Private Sub RemoveAuthorFromList(sender As Object, e As RoutedEventArgs)
        LstAuthor.Items.Remove(sender.Content)
    End Sub
End Class
