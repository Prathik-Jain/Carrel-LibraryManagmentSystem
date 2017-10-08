Imports System.Data.SqlClient
Imports Newtonsoft.Json

Public Class BookService
    Friend Shared Function AddBook(Book As Object, number As Integer) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON VALUES ('BOOK',@JSONString,@Number)", connection)
        query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(Book)))
        query.Parameters.Add(New SqlParameter("@Number", number))
        connection.Open()
        Return query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Function

End Class
