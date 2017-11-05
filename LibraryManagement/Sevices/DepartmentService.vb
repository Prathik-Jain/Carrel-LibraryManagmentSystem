Imports System.Data.SqlClient
''' <summary>
''' This class is responsible managing the departments in the library, works with the <c>Depatment</c> table.
''' </summary>
Public Class DepartmentService
    ''' <summary>
    ''' Grabs all the departments from the <c>Department</c> table in the Database.
    ''' </summary>
    ''' <returns>An ArrayList of Departments.</returns>
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
''' <summary>
''' Adds New Department if not already present - Grabs the new name from <see cref="MemberForm"/>
''' </summary>
''' <param name="dept" > Name  of the Department to be added.</param>
''' <seealso cref="Carrel.MemberForm"/>
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
