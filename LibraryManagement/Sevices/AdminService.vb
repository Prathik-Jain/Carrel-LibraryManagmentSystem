
Imports System.Configuration
Imports System.Data.SqlClient

Public Class AdminService
    Public Function MatchPin(uid As String, pin As Integer) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("CarrelConnectionString").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and PIN=@PIN", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@PIN", PIN))
        Try
            MatchPIN = command.ExecuteScalar()
        Catch e As Exception
            MsgBox("Database error")
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function

    Public Function CheckUser(uid As String, name As String) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("CarrelConnectionString").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and Name=@Name", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@Name", Name))
        Try
            connection.Open()
            CheckUser = command.ExecuteScalar()
        Catch e As Exception
            MsgBox("Database error")
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function
End Class
