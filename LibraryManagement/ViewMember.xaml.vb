Imports System.Drawing
Imports Newtonsoft.Json
Public Class ViewMember
    Friend Sub UpdateView()
        Dim x As DashBoard
        x = Application.Current.Windows(0)
        Dim data = New ArrayList
        data = MemberService.PrintLastAdded()
        LblName.Content = data(0) + " " + data(1)
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

        x.MemberView.Content = Me
        x.ViewMemberDialog.IsOpen = True
    End Sub
    Public Function ImageSourceForBitmap(bmp As Bitmap)

        Dim handle = bmp.GetHbitmap()
        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
        End Function
End Class
