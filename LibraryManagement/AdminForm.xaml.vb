Imports System.Text.RegularExpressions
Public Class AdminForm
    Public Sub New
      ' This call is required by the designer.
        InitializeComponent()
    End Sub

    #Region "Validation"
    ''' <summary>
    ''' Checks the input according to the rules in <see cref="Validation"/>
    ''' </summary>
    Private Sub TxtPhone_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtPhone.PreviewTextInput,
                TxtPin.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub
    ''' <summary>
    ''' Checks the input according to the rules in <see cref="Validation"/>
    ''' </summary>
    Private Sub TxtLastName_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) _
        Handles TxtFName.PreviewTextInput,
                TxtLName.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub
    #End Region

    
    Private Sub BtnAdd_Click() Handles BtnAdd.Click
        AddAdmin()
    End Sub
    ''' <summary>
    ''' This sub procedure converts all the fileds in the Admin form into a Dynamic Linq object and then <see cref="AdminService.AddAdmin(Object)"/> is called if the hidden <c>lblUid</c> is empty and <see cref="AdminService.EditAdmin(Object, String)"/> is called if <c>lblUid</c> is not empty.
    ''' </summary>
    ''' <seealso cref="AdminService"/>
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

    ''' <summary>
    ''' This sub procedure clears all the textfileds of the form.
    ''' </summary>
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
