Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Public Class MemberForm
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        If DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
            Return
        Else
            'Executes when MemberForm is loaded - Not accessed by designer
            CmbDept.ItemsSource = DepatmentService.GetDept()
        End If
    End Sub
    Private Sub BtnAccept_Click(sender As Object, e As RoutedEventArgs) Handles BtnAccept.Click
        AddMember()
        AddDept()
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
        member.Sem = CmbSemister.Text
        If LblUID.Content.ToString = "" Then
            Try
                If  Await MemberService.AddMember(member) Then
                    DashBoard.SnackBarMessageQueue.Enqueue("Registered " + TxtFirstName.Text + " as Member.", "VIEW", Sub()
                        Dim memberPopup as new MemberAccount
                                                                                                                          memberPopup.GetData(MemberService.GetLastUid())
                    End Sub)
                else
                    DashBoard.SnackBarMessageQueue.Enqueue("Failed registering " + TxtFirstName.Text)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
           
        Else
            Try
                If Not Await MemberService.EditMember(LblUID.Content.ToString, member) Then
                    DashBoard.SnackBarMessageQueue.Enqueue("Edited " + TxtFirstName.Text + ".", "VIEW", Sub()
                        Dim viewMember As New ViewMember
                        viewMember.UpdateView()
                    End Sub)

                Else
                    DashBoard.SnackBarMessageQueue.Enqueue("Failed registering " + TxtFirstName.Text)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
                Throw
            End Try
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
