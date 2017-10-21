Imports System.Drawing

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