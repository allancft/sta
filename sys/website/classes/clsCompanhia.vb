Imports System.Data.SqlClient
Public Class clsCompanhia
#Region "Variáveis"
    Private Const strTabela As String = "TCOMPANHIA"
    Private strSQL As String
#End Region
#Region "Objetos"
    Public objConexao As clsDB
#End Region
    Public Function Incluir(ByVal strCod As String, _
                            ByVal strNome As String, _
                            ByRef strErro As String) As Boolean
        Dim objDataReader As SqlDataReader
        Try
            Dim intCodigo As Integer

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_COMP), 0) + 1 AS CODIGO " & _
                    "FROM " & strTabela
            If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objDataReader.Read()

            intCodigo = Convert.ToInt32(objDataReader("CODIGO"))

            objDataReader.Close()

            strSQL = "INSERT INTO " & strTabela & " (ANUM_SEQU_COMP, ANUM_COMP, ANOM_COMP) VALUES(" & _
                    intCodigo & ", " & _
                    "'" & strNome.Trim & "', " & _
                    "'" & strCod.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
    Public Function Editar(ByVal intCod As Integer, _
                            ByVal strCod As String, _
                            ByRef strErro As String) As Boolean
        Try
            strSQL = "UPDATE " & strTabela & " SET " & _
                    "ANUM_COMP = '" & strCod.Trim & "' " & _
                    "WHERE ANUM_SEQU_COMP = " & intCod
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
    Public Function Excluir(ByVal intCodigo As Integer, _
                            ByRef strErro As String) As Boolean
        Try
            strSQL = "DELETE " & strTabela & " WHERE ANUM_SEQU_COMP = " & intCodigo
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
    Public Function Consultar(ByRef objDataSet As Data.DataSet, _
                            ByRef strErro As String, _
                            Optional ByVal intCodigo As Integer = 0) As Boolean
        Try
            strSQL = "SELECT ANUM_COMP, ANUM_SEQU_COMP FROM " & strTabela
            If intCodigo <> 0 Then
                strSQL += " WHERE ANUM_COMP = " & intCodigo
            End If

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
End Class