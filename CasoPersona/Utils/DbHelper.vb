Imports System.Data.SqlClient

Public Class DbHelper
    Private ReadOnly _connectionString As String

    ' Cadena de conexión a la base de datos, se puede configurar en el archivo web.config o app.config
    Public Sub New()
        _connectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    End Sub

    ' Obtiene una nueva conexión a la base de datos
    Public Function GetConnection() As SqlConnection
        Return New SqlConnection(_connectionString)
    End Function

    ' Ejecuta una consulta SQL que no devuelve resultados (INSERT, UPDATE, DELETE)
    Public Function ExecuteNonQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As Integer
        Using conn As SqlConnection = GetConnection() ' Obtiene la conexión a la base de datos
            Using cmd As New SqlCommand(query, conn) ' Crea un comando SQL con la consulta y la conexión
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If
                conn.Open()
                Return cmd.ExecuteNonQuery() ' Ejecuta la consulta y devuelve el número de filas afectadas
            End Using
        End Using
    End Function

    ' Ejecuta una consulta SQL que devuelve resultados (SELECT)
    Public Function ExecuteQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As DataTable
        Dim dt As New DataTable()
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function
End Class
