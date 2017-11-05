Imports System.Globalization
Namespace Validation
    ''' <summary>
    ''' </summary>
Public Class None
    Inherits ValidationRule
        ''' <summary>
        ''' This function is used to check if the textfield is empty
        ''' </summary>
        ''' <param name="value">Value in the textfield.</param>
        ''' <returns> As this function is called when the content of textfield is  changed; it would return true as the field is not empty.</returns>
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
           Return New ValidationResult(True,"")
    End Function
End Class
    End Namespace
