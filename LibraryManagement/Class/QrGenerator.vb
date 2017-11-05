Imports System.Drawing
''' <summary>
''' Generates QR code from a string using the ZXing.NET library.
''' </summary>
Public  class QrGenerator
    ''' <summary>
    ''' This function accepts a string to generate a QR Code of bitmap type, which is then converted into an ImageSource for the image control.
    ''' </summary>
    ''' <param name="QRstring">The string which is supposed to be converted into a QR Code</param>
    ''' <returns>The Imagesource of the generated QR bitmap. </returns>
    public Shared Function Generate(QRstring As String) As ImageSource
        Dim qr = New ZXing.BarcodeWriter()
        qr.Format = ZXing.BarcodeFormat.QR_CODE
        qr.Options = New ZXing.Common.EncodingOptions
        qr.Options.Width = 150
        qr.Options.Height = 150
        Return  ImageSourceForBitmap(New Bitmap(qr.Write(QRstring))) 'For converting the Bitmap to ImageSource
    End Function
    ''' <summary>
    ''' This function converts any Bitmap to an ImageSource for Image Control.
    ''' </summary>
    ''' <param name="bmp">The Bitmap picture whose source is to be extracted.</param>
    ''' <returns>Imagesource of the bitmap image.</returns>
    Private Shared Function ImageSourceForBitmap(bmp As Bitmap)
        Dim handle = bmp.GetHbitmap()
        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
    End Function
End Class