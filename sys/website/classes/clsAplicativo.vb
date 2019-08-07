Imports System.Data.SqlClient
Public Class clsAplicativo
#Region "Variáveis"
    Private Const strTabela As String = "TAPLICATIVO"
    Private strSQL As String
#End Region
#Region "Objetos"
    Public objConexao As clsDB
#End Region
    Public Function Incluir(ByVal strDir As String, _
                            ByVal strNome As String, _
                            ByRef strErro As String) As Boolean
        Dim objDataReader As SqlDataReader
        Try
            Dim intCodigo As Integer

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_APLIC), 0) + 1 AS CODIGO " & _
                    "FROM " & strTabela
            If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objDataReader.Read()

            intCodigo = Convert.ToInt32(objDataReader("CODIGO"))

            objDataReader.Close()

            strSQL = "INSERT INTO " & strTabela & " (ANUM_SEQU_APLIC, ANOM_APLIC, ANOM_DIR_RAIZ) VALUES(" & _
                    intCodigo & ", '" & strNome.Trim & "', '" & strDir.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
    Public Function Editar(ByVal strDir As String, _
                            ByVal strNome As String, _
                            ByVal intCod As Integer, _
                            ByRef strErro As String) As Boolean
        Try
            strSQL = "UPDATE " & strTabela & " SET " & _
                    "ANOM_APLIC = '" & strNome.Trim & "', " & _
                    "ANOM_DIR_RAIZ = '" & strDir.Trim & "' " & _
                    "WHERE ANUM_SEQU_APLIC = " & intCod
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
    Public Function Excluir(ByVal intCod As Integer, _
                            ByRef strErro As String) As Boolean
        Try
            strSQL = "DELETE " & strTabela & " WHERE ANUM_SEQU_APLIC = " & intCod
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
                            Optional ByVal strCod As String = "") As Boolean
        Try
            strSQL = "SELECT ANUM_SEQU_APLIC, ANOM_APLIC, ANOM_DIR_RAIZ FROM " & strTabela
            If strCod.Trim <> "" Then
                strSQL += " WHERE ANUM_COMP = '" & strCod & "'"
            End If
            strSQL += " ORDER BY ANOM_APLIC"

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
End Class
