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
End Class
