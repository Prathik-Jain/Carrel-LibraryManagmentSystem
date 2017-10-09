Imports System.Data.SqlClient
Imports Carrel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization

Public Class MemberService
    Friend Shared Function AddMember(member As Object) As Task(Of Integer)
        Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('MEM',@JSONString)" ,connection)
        query.Parameters.Add(New SqlParameter("@JSONString",JsonConvert.SerializeObject(member)))
        connection.Open()
        Return query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Function

    Friend Shared Function PrintLastAdded()
         Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("SELECT * FROM MEMBER WHERE ID = (SELECT MAX(ID) FROM MEMBER)" ,connection)
        connection.Open()
        Dim reader As SqlDataReader
        reader = query.ExecuteReader
        Dim data As New ArrayList
        reader.Read

            data.add(reader("FNAME").ToString)
            data.Add(reader("LNAME").ToString)
            data.Add(reader("PHONE").ToString)
            data.Add(reader("DEPT").ToString)
            data.Add(reader("SEM").ToString)
            data.Add(reader("UID").ToString)
      Return data
    End Function
End Class
