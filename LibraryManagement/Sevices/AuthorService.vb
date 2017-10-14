Imports System.Data.SqlClient

Public Class AuthorService
    Public Shared Function GetAuthors() As ArrayList
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim dr as SqlDataReader
        Dim query = New SqlCommand("SELECT * FROM AUTHOR", connection)
        Try
            connection.Open()
            DR = query.ExecuteReader()
            dim authors = New ArrayList
            While(DR.Read())
                Authors.Add(DR.GetString(1))
            End While
            DR.Close()
            Return Authors
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.connection.Close()
        End Try
       
    End Function

    Friend Shared Async Sub AddAuthor(author As String)
   Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO AUTHOR VALUES (@Author)" ,connection)
        Try
            query.Parameters.Add(New SqlParameter("@Author",AUTHOR))
            connection.Open()
            await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.connection.Close()
        End Try
    End Sub
End Class
