Imports System.Data.SqlClient

Public Class DepatmentService

    Public Shared Function GetDept() As ArrayList
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim dr as SqlDataReader
        Dim query = New SqlCommand("SELECT * FROM DEPT", connection)
        Try
            connection.Open()
            DR = query.ExecuteReader()
            dim dept = New ArrayList
            While(DR.Read())
                Dept.Add(DR.GetString(1))
            End While
            DR.Close()
            Return Dept
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.connection.Close()
        End Try
        
        
    End Function

     Friend Shared Async Sub AddDepatment(dept As String)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO DEPT VALUES (@DEPT)", connection)
        Try
            query.Parameters.Add(New SqlParameter("@DEPT",DEPT))
            connection.Open()
            Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.connection.Close()
        End Try
       
    End Sub
End Class
