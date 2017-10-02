Imports System.Data.SqlClient
Imports Carrel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization

Public Class MemberService
    Friend Shared Function AddMember(member As Member) As Task(Of Integer)
        Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('MEM','"+JsonConvert.SerializeObject(member)+"')" ,connection)
        connection.Open()
        Return query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Function

End Class
