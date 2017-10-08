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

End Class
