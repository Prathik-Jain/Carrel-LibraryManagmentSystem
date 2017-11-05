Imports System.Globalization
Namespace Validation
''' <summary>
''' This class is used to check the length of the ISBN number - i.e., 10.
''' </summary>
Public Class IsbnLength
			Inherits ValidationRule
		''' <summary>
		''' This function validates if the length of the entered ISBN is 10
		''' <para>The same function is used to check the size of phone number in <see cref="MemberForm"/></para>
		''' </summary>
		''' <param name="value">The value in the textfiled.</param>
		''' <returns>A validationResult either True or False with a message.</returns>
		Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
		If value.ToString.Trim.Length = 10
						Return New ValidationResult(True,"")
			  Else 
			Return New ValidationResult(False,"Must be a 10 digit number")
		End If              
		End Function
End Class
	End Namespace
