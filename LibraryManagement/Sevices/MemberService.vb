Imports System.Data.SqlClient
Imports Newtonsoft.Json

Public Class MemberService

    Friend Async Shared Function AddMember(member As Object) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('MEM',@JSONString)", connection)

        Try
            connection.Open()
            query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(member)))
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
                query.connection.Close()
        End Try
    End Function

    Friend Async Shared Function Delete(uid As String) As Task(Of Boolean)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("DELETE FROM Member WHERE UID = @UID",connection)
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

Friend  Shared Function GetLastUid() As String
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT UID FROM MEMBER WHERE ID = (SELECT MAX(ID) FROM MEMBER)", connection)
        Try
            connection.Open()
            Return query.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        Finally
                query.Connection.Close()
        End Try
    End Function

    Friend Shared Async Function EditMember(uid As String, member As Object) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("UPDATE Member 
                                    SET FNAME = @FName, LNAME = @LName, PHONE = @Phone, DEPT = @Dept, SEM = @Sem
                                    WHERE UID = @UID", connection)
        query.Parameters.Add(New SqlParameter("@FName", member("FName").ToString))
        query.Parameters.Add(New SqlParameter("@LName", member("LName").ToString))
        query.Parameters.Add(New SqlParameter("@Phone", member("Phone").ToString))
        query.Parameters.Add(New SqlParameter("@Dept", member("Dept").ToString))
        query.Parameters.Add(New SqlParameter("@Sem", member("Sem").ToString))
        query.Parameters.Add(New SqlParameter("@UID", UID))
        Try
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Finally
            query.connection.Close()
        End Try
    End Function

    Friend Shared Function GetMember(uid As String)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT * FROM MEMBER WHERE UID = @UID",connection)
        query.Parameters.Add(New SqlParameter("@UID",UID))
        Try
            connection.Open
            Dim reader As SqlDataReader
            reader = query.ExecuteReader
            Dim data As New ArrayList
            reader.Read
                data.Add(reader("FName").ToString)
                data.Add(reader("LName").ToString)
                data.Add(reader("Phone").ToString)
                data.Add(reader("Dept").ToString)
                data.Add(reader("Sem").ToString)
                data.Add(reader("MaxBooks").ToString)
                data.Add(reader("Dues").ToString)
            Return data
            Catch ex As exception
            throw
            Finally
                query.connection.Close
        End Try
    End Function

    Public Shared Function GetMemberById(memberId As String) As String
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT FNAME FROM MEMBER WHERE UID = @MemberID",connection)
        query.Parameters.Add(New SqlParameter("@MemberID",memberID))
        Try
            connection.Open()
            Return query.ExecuteScalar()
        Catch ex As Exception
            Throw
            Finally
            query.connection.Close()
        End Try

    End Function
End Class
