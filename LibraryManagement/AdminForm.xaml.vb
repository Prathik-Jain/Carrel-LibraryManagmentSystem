Imports System.Text.RegularExpressions
Public Class AdminForm
    Public Sub New
      ' This call is required by the designer.
        InitializeComponent()
    End Sub
    'Private Sub FieldLostFocus(sender As Object, e As RoutedEventArgs)
    '    Dim txt = DirectCast(sender, TextBox)
    '    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource()
    'End Sub

    Private Sub TxtPhone_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPhone.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtFirstName_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtFName.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub

    Private Sub TxtLastName_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtLName.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub

    Private Sub TxtPassword_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPin.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As RoutedEventArgs) Handles BtnAdd.Click
        AddAdmin()
    End Sub
    private async sub AddAdmin()
        dim admin as Object = New Newtonsoft.Json.Linq.JObject
        admin.FName = TxtFName.Text
        admin.LName = TxtLName.Text
        admin.Phone = TxtPhone.Text
        admin.Pin = TxtPin.Text
        admin.SecurityQuestion = TxtSecurityQ.Text
        admin.Answer = TxtAnswer.Text
        if lblUid.content = ""
            if await AdminService.AddAdmin(admin) then
                AdminPopup.SnackBarMessageQueue.Enqueue(TxtFName.Text + " Registered as Admin", "PRINT" , sub()
                                                                                                              dim adminpopup as New AdminPopup
AdminPopup.Print(admin,String.Empty)
                                                                                              End Sub)
            else
                AdminPopup.SnackBarMessageQueue.Enqueue("Failed Registering")
            End If
        else
            if await AdminService.EditAdmin(admin, lbluid.content) then
                AdminPopup.SnackBarMessageQueue.Enqueue(" Updated Admin Details", "PRINT" , sub()
                                                                                                 dim adminpopup as New adminpopup
AdminPopup.Print(admin,LblUID.Content)
                End Sub)
            else
                AdminPopup.SnackBarMessageQueue.Enqueue("Failed Updating Details")
            End If
        End If

    End sub


    Public Sub clearAll()
        LblUID.Content = ""
        TxtPhone.Clear()
        TxtAnswer.Clear()
        TxtFName.Clear()
        TxtLName.Clear
        TxtPin.Clear()
        BtnAdd.Content = "ADD"
        TxtSecurityQ.Clear()
    End Sub
End Class
