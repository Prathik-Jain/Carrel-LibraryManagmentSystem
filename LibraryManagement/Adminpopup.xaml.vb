Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json

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
            _dashBoard.StartCameraAndTimer()
        End Try
    End Sub

    Private Sub AddAdmin_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddAdmin.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        AdminForm.clearAll
        AdminFormDialog.isOpen=true
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        SnackBarMessageQueue = Snackbar.MessageQueue
        AdminForm.LblUID.Content = LblUID.Content
        Adminform.TxtFName.Text = data(0)
        AdminForm.TxtLName.Text = data(1)
        AdminForm.TxtPhone.Text = LblPhone.Content
        AdminForm.TxtSecurityQ.Text = data(4)
        AdminForm.TxtPin.Text = data(3)
        AdminForm.TxtAnswer.Text = data(5)
        AdminForm.BtnAdd.Content = "UPDATE"
        AdminFormDialog.IsOpen = True
    End Sub
    Friend Sub Print(admin As Object, uid As String)
        If uid = Nothing
            uid = AdminService.GetLastUid()
        End If
        PrintAdmin.ImgQR.Source = QrGenerator.Generate(UID) ' Need to update image!
        PrintAdmin.LblDepartment.Content = ""
        PrintAdmin.LblPhone.Content = admin("Phone")
        PrintAdmin.LblName.Content = admin("FName") + " " + admin("LName")
        PrintAdmin.DeptIcon.Visibility = Visibility.Collapsed
        PrintAdmin.LblUID.content = uid
        PrintAdmin.LblPhone.Content = admin("Phone")
        PrintAdmin.LblCat.Content = "ADMIN"
        Dim printdlg As new PrintDialog
        if printDlg.ShowDialog() = true
            printDlg.PrintVisual(PrintAdmin, "Printing Admin Card")
        End If
    End Sub

    Private Sub ChangeSem_Click(sender As Object, e As RoutedEventArgs) Handles BtnChangeSem.Click
        ChangeSemDialog.isOpen=True
    End Sub
    Private  sub StopcameraonDialogOpen Handles  ChangeSemDialog.DialogOpened,AdminFormDialog.DialogOpened
        _dashBoard.StopCameraAndTimer()
    End sub
    private sub StartCameraOnDailogClose Handles AdminForm.Unloaded,
            AdminFormDialog.DialogClosing,ChangeSemDialog.DialogClosing,ChangeSem.Unloaded,AdminForm.Unloaded
       _dashBoard.StopCameraAndTimer()
    End sub

End Class
