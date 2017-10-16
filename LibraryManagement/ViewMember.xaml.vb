Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Windows.Controls.Primitives
Imports Newtonsoft.Json
Public Class ViewMember
    Dim _dashBoard As DashBoard = Application.Current.Windows(0)
    Dim _data = New ArrayList
Public Sub New

    ' This call is required by the designer.
    InitializeComponent()

End Sub
    Friend Sub UpdateView()
        _data = MemberService.PrintLastAdded()
        LblFName.Content = _data(0).ToString.ToUpper
        LblLName.Content =_data(1)
        LblPhone.Content = _data(2)
        LblDepartment.Content = _data(3)
        LblSemester.Content = _data(4)
        LblUID.Content = _data(5)
        Dim qrContent As Object = New Linq.JObject
        qrContent.UID = LblUID.Content
        Dim qrString = JsonConvert.SerializeObject(qrContent)
        ImgQR.Source = QRGenerator.Generate(QRString)
        _dashBoard.MemberView.Content = Me
        _dashBoard.ViewMemberDialog.IsOpen = True
        DeleteMember.SetData("MEM",_data(5))
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        _dashBoard.ViewMemberDialog.IsOpen = False
        _dashBoard.MemberForm.TxtFirstName.Text = LblFName.Content
        _dashBoard.MemberForm.TxtLastName.Text = LblLName.Content
        _dashBoard.MemberForm.TxtPhone.Text = LblPhone.Content
        _dashBoard.MemberForm.CmbDept.Text = LblDepartment.Content
        _dashBoard.MemberForm.TxtSemester.Text = LblSemester.Content
        _dashBoard.MemberForm.LblUID.Content = LblUID.Content
        _dashBoard.MemberFormDialog.IsOpen = True
    End Sub


    Private Sub OpenDeleteDialog(sender As Object, e As RoutedEventArgs)
        DeleteMember.LblPrompt.Content = "Delete Member?"
        DeleteMemberDialog.IsOpen= True
    End Sub
End Class
Public  class QrGenerator
    public Shared Function Generate(jsonString As String) As ImageSource
        Dim qr = New ZXing.BarcodeWriter()
        qr.Format = ZXing.BarcodeFormat.QR_CODE
        qr.Options = New ZXing.Common.EncodingOptions
        qr.Options.Width = 150
        qr.Options.Height = 150
        Return  ImageSourceForBitmap(New Bitmap(qr.Write(JSONString)))
    End Function

    Private Shared Function ImageSourceForBitmap(bmp As Bitmap)
        Dim handle = bmp.GetHbitmap()
        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
    End Function
End Class
