'Imports Newtonsoft.Json
'Imports System.Drawing
'Public Class Card
'    Dim admin As Admin
'    Public Sub UpdateCard()
'        Dim x As DashBoard
'        x = Application.Current.Windows(0)
'        Dim data = New ArrayList
'        data = MemberService.PrintLastAdded()
'        LblName.Content = data(0) + " " + data(1)
'        LblPhone.Content = data(2)
'        LblDepartment.Content = data(3)
'        LblUID.Content = data(4)
'        Dim product As Object = New Linq.JObject
'        product.UID = LblUID.Content
'        product.Name = LblName.Content
'        Dim QRJson = JsonConvert.SerializeObject(product)
'        Dim qr = New ZXing.BarcodeWriter()
'        qr.Format = ZXing.BarcodeFormat.QR_CODE
'        qr.Options = New ZXing.Common.EncodingOptions
'        qr.Options.Width = 150
'        qr.Options.Height = 150
'        Dim result = New Bitmap(qr.Write(QRJson))
'        ImgQR.Source = ImageSourceForBitmap(result)

'        x.a.card.Content = Me
'        x.PrintQRDialog.IsOpen = True

'    End Sub
'    Public Function ImageSourceForBitmap(bmp As Bitmap)

'        Dim handle = bmp.GetHbitmap()
'        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
'    End Function
'End Class