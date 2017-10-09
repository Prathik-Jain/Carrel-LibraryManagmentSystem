Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Public Class MemberForm
    Private Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click
        AddMember()
        AddDept()
    End Sub
    Private Sub MemberForm_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CmbDept.ItemsSource = DepatmentService.GetDept()
    End Sub
    Private Sub AddDept()
        For Each item In CmbDept.Items
            If CmbDept.Text = item
                Exit Sub
            End If
        Next
        DepatmentService.AddDepatment(CmbDept.Text)
    End Sub
    Private Async Sub AddMember()
        Dim member As Object = New Linq.JObject()
        member.FName = TxtFirstName.Text
        member.LName = TxtLastName.Text
        member.Phone = TxtPhone.Text
        member.Dept = CmbDept.Text
        member.Sem = TxtSemester.Text
        If Await MemberService.AddMember(member) Then
            DashBoard.SnackBarMessageQueue.Enqueue("Registered "+ TxtFirstName.Text+ " as Member.", "CARD",Sub ()
                                                                                                              Dim card As New Card
                                                                                                               card.UpdateCard
                                                                                                           End Sub)
        Else
            DashBoard.SnackBarMessageQueue.Enqueue("Failed registering " + TxtFirstName.Text)
        End If
    End Sub

    Private Sub FieldLostFocus(sender As Object, e As RoutedEventArgs)
        Dim txt = DirectCast(sender, TextBox)
        txt.GetBindingExpression(TextBox.TextProperty).UpdateSource()
    End Sub

    Private Sub TxtPhone_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtPhone.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]")
    End Sub

    Private Sub TxtFirstName_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtFirstName.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub

    Private Sub TxtLastName_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TxtLastName.PreviewTextInput
        e.Handled = Regex.IsMatch(e.Text, "[^a-zA-Z]+$")
    End Sub
End Class
