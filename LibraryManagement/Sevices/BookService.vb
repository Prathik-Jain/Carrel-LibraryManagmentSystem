Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json
''' <summary>
''' Handles all the functions related to books in the library.
''' </summary>

Public Class BookService
    ''' <summary>
    ''' This function adds a new book to the <c>JSON</c> table which calls a trigger to add it to the <c>Book</c> table.
    ''' </summary>
    ''' <param name="book">Dynamic object of book which contains all the details about the book.</param>
    ''' <param name="number">Total number of books to be added.</param>
    ''' <returns>Number of rows affected</returns>
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
    ''' <summary>
    ''' Gets the list of books borrowed by a particular member.
    ''' </summary>
    ''' <param name="memberId">UID of a member.</param>
    ''' <returns>The a table of books with Unique ID, Title, and the date on which it was borrowed.</returns>
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
    ''' <summary>
    ''' Gets the details of the book which was last added.
    ''' </summary>
    ''' <returns>An arrary list of books with all the basic fields.</returns>
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
    ''' <summary>
    ''' This function is responsible to handle the books returned by a particular member. 
    ''' <para>This is basically done by setting <C>BORRROWEDBY</C> and <c>BORROWEDON</c> to null and setting <c>AVAILABLE</c> to 1 in the <c>Book</c> table.</para>
    ''' </summary>
    ''' <param name="bookId">Unique ID of the book to be returned.</param>
    ''' <returns>Number of rows affected i.e, 1 as true.</returns>
    Friend Shared Async Function Returned(bookId As String) As Task(Of Integer)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query =
                New SqlCommand("UPDATE Book SET BORROWEDBY = @null , BORROWEDON = @null, AVAILABLE = 1  WHERE UID = @BookID",
                               connection)

        Try
            query.Parameters.Add(New SqlParameter("@null", DBNull.Value))
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
    ''' <summary>
    ''' Add a book to the borrowed list.
        ''' <para>This is basically done by setting <C>BORRROWEDBY</C> to the Unique ID of the member and <c>BORROWEDON</c> to todays date. and setting <c>AVAILABLE</c> to 0 in the <c>Book</c> table.</para>
    ''' </summary>
    ''' <param name="bookId">Unique ID of the book which is borrowed.</param>
    ''' <param name="memberId">Unique ID of the member who borrows the book.</param>
    ''' <returns>The number of rows affected.</returns>
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
    ''' <summary>
    ''' This function returns the details of the books from the <c>Book</c> table and used to update the view of the Book.
    ''' </summary>
    ''' <param name="bookId">Unique ID of the book.</param>
    ''' <returns>A dynamic object which contains the details about the book.</returns>
    ''' <seealso cref="Carrel.ViewBook"/>
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
    ''' <summary>
    ''' This function is used to get the number of books available with the same ISBN number.
    ''' </summary>
    ''' <param name="isbn">ISBN number of any book.</param>
    ''' <returns>Returns the number of books of same ISBN</returns>
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
    ''' <summary>
    ''' Gets the total number of books available in the <c>Book</c> table.
    ''' </summary>
    ''' <returns>Total number of books in <c>Book</c> table.</returns>
    Public Async Shared Function TotalBooks() As Task(Of String)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT COUNT(*) FROM BOOK", connection)
        Try
            connection.Open()
            Return Await query.ExecuteScalarAsync
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function returns the total number of books issued.
    ''' <para>Gets the count form <c>Book</c> table where <c>BORROWEDBY</c> is not <c>NULL</c></para>
    ''' </summary>
    ''' <returns>Total number of books issued.</returns>
    Public Async Shared Function TotalBooksIssued() As Task(Of String)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT COUNT(*) FROM BOOK WHERE BORROWEDBY IS NOT NULL", connection)
        Try
            connection.Open()
            Return Await query.ExecuteScalarAsync
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' Gets the number of books available in the library of a particular ISBN number.
    ''' <para>Used in <see cref="ViewBook"/></para>
    ''' </summary>
    ''' <param name="isbn">ISBN number of the book.</param>
    ''' <returns>The total number of books available of a particular ISBN</returns>
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
    ''' <summary>
    ''' This function is used to edit books with a particualr ISBN.
    ''' <para>The <see cref="BookForm"/> is autofilled with all the details and then the fields are converted to a dynamic object for editing the book in the database.</para>
    ''' </summary>
    ''' <param name="book">Dyanmic object of book</param>
    ''' <returns>The numbers of rows affected. A possitive integer would say books are edited.</returns>
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
    ''' <summary>
    ''' Delete any book with an unique ID
    ''' </summary>
    ''' <param name="uid">Unique ID of the book.</param>
    ''' <returns>The number of rows affected.</returns>
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
