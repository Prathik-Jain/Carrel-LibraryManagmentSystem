Imports System.Data
Imports System.Data.SqlClient
Imports MahApps.Metro.Converters

Public Class AuthorService
    Public Shared Function GetAuthors() As ArrayList
        Dim connection =
                new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim DR as SqlDataReader
        Dim query = new SqlCommand("SELECT * FROM AUTHOR", connection)
        connection.Open()
        DR = query.ExecuteReader()
        dim Authors = New ArrayList
        While(DR.Read())
            Authors.Add(DR.GetString(1))
        End While
        DR.Close()
        connection.Close()
        Return Authors
    End Function

    Friend shared Sub AddAuthor(AUTHOR As String)
   Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("INSERT INTO AUTHOR VALUES (@Author)" ,connection)
        query.Parameters.Add(New SqlParameter("@Author",AUTHOR))
        connection.Open()
        query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Sub
End Class
