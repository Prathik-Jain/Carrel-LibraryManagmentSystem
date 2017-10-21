Imports MaterialDesignThemes.Wpf

Public Class AdminPopup
    Dim data As arraylist
    Dim _dashBoard As DashBoard
    Public Shared SnackBarMessageQueue As SnackbarMessageQueue

    Public sub New 

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        For Each window As Window In Application.Current.Windows
            If window.GetType() = GetType(DashBoard)
                _dashBoard = window
            End If
        Next

    End Sub
    Public Sub GetData(UID As String)
        try
            data = AdminService.GetAdmin(UID)
            LblUID.Content = UID
            LblName.Content = data(0) + " " + data(1)
            LblPhone.Content = data(2)
            ImgQr.source = qrGenerator.generate(lblUID.content)
            _dashBoard.AdminPopup.Content = Me.Content
            _dashBoard.AdminPopupDialog.IsOpen = True
        Catch ex As Exception
            msgbox("Admin not found")
        End Try
    End Sub

    Private Sub AddAdmin_Click(sender As Object, e As RoutedEventArgs) Handles AddAdmin.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        AdminFormDialog.isOpen=true
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        AdminForm.LblUID.Content = LblUID.Content
        Adminform.TxtFName.Text = data(0)
        AdminForm.TxtLName.Text = data(1)
        AdminForm.TxtPhone.Text = LblPhone.Content
        AdminForm.CmbSecurityQ.Text = data(4)
        AdminForm.TxtPin.Text = data(3)
        AdminForm.TxtAnswer.Text = data(5)
        AdminForm.BtnAdd.Content = "UPDATE"
        AdminFormDialog.IsOpen = True
    End Sub
    Friend Sub Print(admin As Object)
        dim uid = AdminService.GetLastUid()
        PrintAdmin.ImgQR.Source = QrGenerator.Generate(uid)
        PrintAdmin.LblDepartment.Content = ""
        PrintAdmin.LblPhone.Content = admin("Phone")
        PrintAdmin.LblName.Content = admin("Fname").ToString() + " " + admin("LName").ToString()
        PrintAdmin.LblUID.content = uid
        PrintAdmin.LblCat.Content = "ADMIN"
        Dim printdlg As new PrintDialog
        if printDlg.ShowDialog() = true
            printDlg.PrintVisual(PrintAdmin, "Printing Admin Card")
        End If
    End Sub
End Class
