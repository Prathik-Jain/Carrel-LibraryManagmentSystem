Imports System.Data.SqlClient
''' <summary>
''' Contains all the functions related to an Author.
''' </summary>
Public Class AuthorService
    ''' <summary>
    ''' Gets all the authors from the <c>Author</c> table.
    ''' </summary>
    ''' <returns>All the list of authors from the Database.</returns>
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
''' <summary>
''' This function is used to add new Authors to the Database if not already present in the database. 
''' <para>Grabs the name from Add Book form.</para>
''' </summary>
''' <param name="author">Name of the author to be added.</param>
''' <seealso cref="Carrel.BookForm"/>
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
