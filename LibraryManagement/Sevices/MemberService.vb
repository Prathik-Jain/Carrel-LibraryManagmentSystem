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

    Friend Shared Function EditMember(UID As String, member As Object) As Task(Of Integer)
                Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("UPDATE Member 
                                    SET FNAME = @FName, LNAME = @LName, PHONE = @Phone, DEPT = @Dept, SEM = @Sem
                                    WHERE UID = @UID" ,connection)
        query.Parameters.Add(New SqlParameter("@FName",member("FName").ToString))
        query.Parameters.Add(New SqlParameter("@LName", member("LName").ToString))
        query.Parameters.Add(New SqlParameter("@Phone", member("Phone").ToString))
        query.Parameters.Add(New SqlParameter("@Dept", member("Dept").ToString))
        query.Parameters.Add(New SqlParameter("@Sem",member("Sem").ToString))
        query.Parameters.Add(New SqlParameter("@UID", UID))
        connection.Open()
        Return query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Function
End Class
