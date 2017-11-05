Imports System.Globalization
Namespace Validation
	Public Class NotEmpty
		Inherits ValidationRule
		''' <summary>
		''' This function is used to check if the textfield is empty.
		''' </summary>
		''' <param name="value">Text present in textfield</param>
		''' <returns>False with a message if value is null or whitespace..</returns>
		Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
			Return If(String.IsNullOrWhiteSpace((If(value, "")).ToString()), New ValidationResult(False, "Field is required."), ValidationResult.ValidResult)
		End Function
	End Class
End Namespace
