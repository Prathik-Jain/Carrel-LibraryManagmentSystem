Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json

Public Class BookService
    Friend Shared Async Function AddBook(book As Object, number As Integer) As Task(Of Integer)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON VALUES ('BOOK',@JSONString,@Number)", connection)
        Try
            query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(book)))
            query.Parameters.Add(New SqlParameter("@Number", number))
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Friend Shared function GetBooksBorrowed(memberId As String) 
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("SELECT UID, TITLE, BORROWEDON FROM BOOK WHERE BORROWEDBY = @memberID", connection)
        Try
            query.Parameters.Add(New SqlParameter("@MemberID", memberId))
            connection.Open()
            dim da = new SqlDataAdapter(query)
            Dim dt = new DataTable("Students")
            da.Fill(dt)
            Return dt.DefaultView
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End function

    Friend Shared Function GetLastAdded() As ArrayList
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("SELECT * FROM BOOK WHERE ID = (SELECT MAX(ID) FROM BOOK)", connection)
        Try
            connection.Open()
            Dim reader As SqlDataReader
            reader = query.ExecuteReader
            Dim data As New ArrayList
            reader.Read()
            data.Add(reader("ISBN").ToString)
            data.Add(reader("TITLE").ToString)
            data.Add(reader("AUTHOR").ToString)
            data.Add(reader("PUBLISHER").ToString)
            data.Add(reader("EDITION").ToString)
            data.Add(reader("PRICE").ToString)
            data.Add(reader("RACK").ToString)
            Return data
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Friend Shared Async Function Returned(bookId As String) As Task(Of Integer)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("UPDATE Book SET BORROWEDBY = '', BORROWEDON = '', AVAILABLE = 1  WHERE UID = @BookID",
                               connection)

        Try
            query.Parameters.Add(New SqlParameter("@BookID", bookId))
            connection.Open()
            Return Await query.ExecuteNonQueryAsync
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Friend Shared Async Function Borrowed(bookId As String, memberId As String) as Task(Of Integer)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("UPDATE Book SET BORROWEDBY = @MemberID, BORROWEDON = @NOW, AVAILABLE = 0 WHERE UID = @BooKID",
                    connection)
        connection.Open()

        Try
            query.Parameters.Add(New SqlParameter("@MemberID", memberId))
            query.Parameters.Add(New SqlParameter("@BooKID", bookId))
            query.Parameters.Add(New SqlParameter("@NOW", Format(DateTime.Now, "dd MMMM yyyy")))
            Return Await query.ExecuteNonQueryAsync
        Catch e As Exception
            MsgBox(e.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Public Shared Function GetBookById(bookId As String) As Object
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT * FROM BOOK WHERE UID = @BookID", connection)

        Try
            query.Parameters.Add(New SqlParameter("BookID", bookId))
            connection.Open()
            Dim reader As SqlDataReader
            reader = query.ExecuteReader
            Dim data As New ArrayList
            reader.Read()
            data.Add(reader("ISBN").ToString)
            data.Add(reader("TITLE").ToString)
            data.Add(reader("AUTHOR").ToString)
            data.Add(reader("PUBLISHER").ToString)
            data.Add(reader("EDITION").ToString)
            data.Add(reader("PRICE").ToString)
            data.Add(reader("RACK").ToString)
            data.Add(reader("AVAILABLE").ToString())
            data.Add(reader("BORROWEDBY").ToString())
            data.Add(reader("BORROWEDON").ToString())
            data.Add(reader("ADDEDON").ToString())
            Return data
        Catch ex As Exception
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Public Shared Function GetNumber(isbn As String) As String
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT COUNT(ISBN) FROM BOOK WHERE ISBN = @ISBN", connection)
        query.Parameters.Add(New SqlParameter("@ISBN", isbn))

        Try
            connection.Open()
            Return query.ExecuteScalar
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Public Shared Function GetAvailable(isbn As String) As String
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("SELECT COUNT(ISBN) FROM BOOK WHERE ISBN = @ISBN AND AVAILABLE = 1", connection)
        query.Parameters.Add(New SqlParameter("@ISBN", isbn))

        Try
            connection.Open()
            Return query.ExecuteScalar
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function

    Public async Shared Function EditBooks(book As Object) As Task(Of Integer)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("UPDATE Book SET Title = @Title, Author = @Author, Publisher = @Publisher, Edition = @edition , Price = @Price , Rack = @Rack WHERE ISBN= @ISBN", connection)
        Try
            query.Parameters.Add(New SqlParameter("@Title", book("Title")))
            query.Parameters.Add(New SqlParameter("@Author", book("Author")))
            query.Parameters.Add(New SqlParameter("@Publisher", book("Publisher")))
            query.Parameters.Add(New SqlParameter("@Edition", book("Edition")))
            query.Parameters.Add(New SqlParameter("@Price", book("Price")))
            query.Parameters.Add(New SqlParameter("@Rack", book("Rack")))
            query.Parameters.Add(New SqlParameter("@ISBN", book("ISBN")))
            connection.Open()
            return await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            Throw
        Finally
           query.Connection.Close
        End Try
    End Function

    Public Async Shared Function Delete(uid As String) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("DELETE FROM Book WHERE UID = @UID",connection)
        query.Parameters.Add(new SqlParameter("@UID",uid))
        Try
            Connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch  ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
End Class
