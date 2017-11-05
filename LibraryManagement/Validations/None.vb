Imports System.Globalization
Namespace Validation
    ''' <summary>
    ''' </summary>
Public Class None
    Inherits ValidationRule
        ''' <summary>
        ''' This function is a dummy validation - returns true always.
        ''' </summary>
        ''' <param name="value">Value in the textfield.</param>
        ''' <returns> True.</returns>
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
           Return New ValidationResult(True,"")
    End Function
End Class
    End Namespace
