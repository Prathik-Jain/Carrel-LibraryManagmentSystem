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

    Friend Shared Function GetBooks(MemberID As String) As List(Of Book)
        Dim list As New List(Of Book)
        Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("SELECT UID, TITLE, BORROWEDON FROM BOOK WHERE BORROWEDBY = @MemberID" ,connection)
        Dim reader As SqlDataReader
        Dim data As New ArrayList
        Try
            query.Parameters.Add(New SqlParameter("@MemberID", MemberID))
            connection.Open
            reader= query.ExecuteReader
            Dim count =0
            While reader.Read
                list.Add(New Book With {.Sl = count, .BookID = reader("UID").ToString, .BorrowedOn = reader("BORROWEDON").ToString, .Title = reader("TITLE").ToString})
                count += 1
            End While
            Return list
        Finally
            query.Connection.Close
        End Try
    End Function

    Friend Shared Function PrintLastAdded()
         Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("SELECT * FROM BOOK WHERE ID = (SELECT MAX(ID) FROM BOOK)" ,connection)
        connection.Open()
        Dim reader As SqlDataReader
        reader = query.ExecuteReader
        Dim data As New ArrayList
        reader.Read

            data.add(reader("ISBN").ToString)
            data.Add(reader("TITLE").ToString)
            data.Add(reader("AUTHOR").ToString)
            data.Add(reader("PUBLISHER").ToString)
            data.Add(reader("EDITION").ToString)
            data.Add(reader("PRICE").ToString)
            data.Add(reader("RACK").ToString)
      Return data
    
    End Function

    Friend Shared Async function [Return](bookId As String) As Task(Of Integer)
        Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("UPDATE Book
                                    SET BORROWEDBY = ''
                                    WHERE UID = @BookID" ,connection)
        
        Try
            query.Parameters.Add(New SqlParameter("@BookID",bookId))
            connection.Open
            Return await query.ExecuteNonQueryAsync
            
        Finally
            query.Connection.Close
        End Try
    End Function

    Friend Async Shared Function AddBorrower(BookID  As String,MemberID As String) As Task(Of Integer)
         Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("UPDATE Book
                                    SET BORROWEDBY = @MemberID, BORROWEDON = @NOW
                                    WHERE UID = @BooKID" ,connection)
            Connection.Open()
        
        Try
            query.Parameters.Add(New SqlParameter("@MemberID",MemberID))
            query.Parameters.Add(New SqlParameter("@BooKID",BookId))
            query.Parameters.Add(New SqlParameter("@NOW",Now.Date()))
            Dim x= Await query.ExecuteNonQueryAsync
            Return x
            Catch e As Exception
            MsgBox(e.ToString())
            Throw
            Finally
            query.Connection.Close
        End Try
    End Function
End Class
