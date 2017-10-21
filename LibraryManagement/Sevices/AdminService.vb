
Imports System.Configuration
Imports System.Data.SqlClient
Imports Newtonsoft.Json

Public Class AdminService
    Public Function MatchPin(uid As String, pin As Integer) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and PIN=@PIN", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@PIN", PIN))
        Try
            connection.Open()
            MatchPIN = command.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function

    Public Function CheckUser(uid As String, name As String) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and FName=@Name", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@Name", Name))
        Try
            connection.Open()
            CheckUser = command.ExecuteScalar()
        Catch ex As Exception
            Msgbox(ex.toString)
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function
    Friend Shared Function GetAdmin(uid As String)
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT * FROM ADMIN WHERE UID = @UID",connection)
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
            data.Add(reader("Pin").ToString)
            data.Add(reader("SecurityQuestion").ToString)
            data.Add(reader("Answer").ToString)
            Return data
        Catch ex As exception
            throw
        Finally
            query.connection.Close
        End Try
    End Function

    Public Async Shared Function AddAdmin(admin As Object) As Task(of Integer)
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('ADM',@JSONString)", connection)
        Try
            query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(admin)))
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    Friend Shared Async Function EditAdmin( admin As Object,uid As String) As Task(Of Integer)
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("UPDATE Admin 
                                    SET FNAME = @FName, LNAME = @LName, PHONE = @Phone, PIN = @Pin, SECURITYQUESTION = @SecurityQ,  ANSWER = @Answer
                                    WHERE UID = @UID", connection)
        query.Parameters.Add(New SqlParameter("@FName", admin("FName").ToString))
        query.Parameters.Add(New SqlParameter("@LName", admin("LName").ToString))
        query.Parameters.Add(New SqlParameter("@Phone", admin("Phone").ToString))
        query.Parameters.Add(New SqlParameter("@Pin", admin("Pin").ToString))
        query.Parameters.Add(New SqlParameter("@SecurityQ", admin("SecurityQuestion").ToString))
        query.Parameters.Add(New SqlParameter("@Answer", admin("Answer").ToString))
        query.Parameters.Add(New SqlParameter("@UID", UID))
        Try
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Finally
            query.connection.Close()
        End Try
    End Function
    Friend  Shared Function GetLastUid() As String
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT UID FROM ADMIN WHERE ID = (SELECT MAX(ID) FROM ADMIN)", connection)
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
End Class
