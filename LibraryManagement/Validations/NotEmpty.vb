Imports System.Globalization
Namespace Validation
	Public Class NotEmpty
		Inherits ValidationRule
		Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
			Return If(String.IsNullOrWhiteSpace((If(value, "")).ToString()), New ValidationResult(False, "Field is required."), ValidationResult.ValidResult)
		End Function
	End Class
End Namespace
