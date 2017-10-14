Imports System.Globalization
Namespace Validation
Public Class IsbnLength
    		Inherits ValidationRule
		Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        If value.ToString.Trim.Length = 10
                        Return New ValidationResult(True,"")
              Else 
            Return New ValidationResult(False,"Must be a 10 digit number")
        End If              
		End Function
End Class
    End Namespace
