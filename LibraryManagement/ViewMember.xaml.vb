Imports System.Drawing
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Public Class ViewMember
    Dim dashBoard As DashBoard = Application.Current.Windows(0)
    Dim data = New ArrayList
Public Sub New

    ' This call is required by the designer.
    InitializeComponent()

End Sub
    Friend Sub UpdateView()
        data = MemberService.PrintLastAdded()
        LblFName.Content = data(0).ToString.ToUpper
        LblLName.Content =data(1)
        LblPhone.Content = data(2)
        LblDepartment.Content = data(3)
        LblSemester.Content = data(4)
        LblUID.Content = data(5)
        Dim qrContent As Object = New Linq.JObject
        qrContent.UID = LblUID.Content
        Dim QRJson = JsonConvert.SerializeObject(qrContent)
        Dim qr = New ZXing.BarcodeWriter()
        qr.Format = ZXing.BarcodeFormat.QR_CODE
        qr.Options = New ZXing.Common.EncodingOptions
        qr.Options.Width = 150
        qr.Options.Height = 150
        Dim result = New Bitmap(qr.Write(QRJson))
        ImgQR.Source = ImageSourceForBitmap(result)
        dashBoard.MemberView.Content = Me
        dashBoard.ViewMemberDialog.IsOpen = True
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles BtnEdit.Click
        dashBoard.ViewMemberDialog.IsOpen = False
        dashBoard.MemberForm.TxtFirstName.Text = LblFName.Content
        dashBoard.MemberForm.TxtLastName.Text = LblLName.Content
        dashBoard.MemberForm.TxtPhone.Text = LblPhone.Content
        dashBoard.MemberForm.CmbDept.Text = LblDepartment.Content
        dashBoard.MemberForm.TxtSemester.Text = LblSemester.Content
        dashBoard.MemberForm.LblUID.Content = LblUID.Content
        dashBoard.MemberFormDialog.IsOpen = True
    End Sub

    Public Function ImageSourceForBitmap(bmp As Bitmap)
        Dim handle = bmp.GetHbitmap()
        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
    End Function
End Class
