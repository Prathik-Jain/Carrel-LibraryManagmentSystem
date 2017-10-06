Imports Newtonsoft.Json
Imports System.Windows.Interop
Imports System.Drawing
Public Class Card
    Dim admin As Admin
    Public sub UpdateCard()
         Dim x As  DashBoard
        x = Application.Current.Windows(0)
        Dim data = New ArrayList
         data = MemberService.PrintLastAdded()
        LblName.Content = Data(0) + " " + Data(1)
        LblPhone.Content = Data(2)
        LblDepartment.Content = data(3)
        LblUID.Content = Data(4)
        Dim product As Object = New Linq.JObject
        product.UID = LblUID.Content
        product.Name = LblName.Content
        Dim QRJson = JsonConvert.SerializeObject(product)
        Dim qr = New ZXing.BarcodeWriter()
        qr.Format = ZXing.BarcodeFormat.QR_CODE
        Dim result = New Bitmap(qr.Write(QRJson))
        ImgQR.Source =ImageSourceForBitmap(result)
        
        x.a.card.Content= Me
        x.PrintQRDialog.IsOpen = True

    End sub
        public Function ImageSourceForBitmap(bmp As Bitmap)
    
        Dim handle = bmp.GetHbitmap()
            return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(200,200))
        End Function
End Class
