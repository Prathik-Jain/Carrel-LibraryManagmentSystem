Imports System.Configuration
Imports System.Data.SqlClient

Class StartUp
    Public Sub New
        InitializeComponent()
    End Sub
    ''' <summary>
    ''' Sets up a dummy connections for fast initial transaction.
    ''' </summary>
    ''' <returns><c>True</c> when the connected.</returns>
    Private Async Function InitialConnection() As Task(Of Boolean)
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT 'STARTUP'", connection)
        Try
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' Calls tge <see cref="InitialConnection()"/> function - Add <c>5 s</c> Delay -Shows Login Window.
    ''' </summary>
    ''' <seealso cref="Login"/>
    Private Async Sub Me_Loaded() Handles Me.Loaded
        Await InitialConnection
        Await Task.Delay(5000)
        Dim login As New Login
        login.Show()
        Me.Close()
    End Sub
End Class