Imports System.Globalization
Namespace Validation
Public Class None
    Inherits ValidationRule
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
           Return New ValidationResult(True,"")
    End Function
End Class
    End Namespace
