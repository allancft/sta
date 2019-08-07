Imports System.data
Imports System.Data.SqlClient
Public Class clsConfiguracao
#Region "Variáveis"
    Private Const strTabela As String = "TCONFIGURACAO"
    Private strSQL As String
#End Region
#Region "Objetos"
    Public objConexao As clsDB
#End Region
    Public Function ConfiguracaoConsultar(ByRef objReader As SqlClient.SqlDataReader, _
                                        ByRef strErro As String) As Boolean
        ConfiguracaoConsultar = False

        Try
            strSQL = "SELECT ADAT_EXCLUSAO, AFTP_EXT_ARQU_USER_INTERNO, AFTP_EXT_ARQU_USER_FORNECEDOR, " & _
                    " ANOM_USER_ADMIN" & _
                    " FROM TCONFIGURACAO"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConfiguracaoConsultar = (strErro = "")

    End Function
    Public Overloads Function Salvar(ByVal strExtensoesInt As String, _
                                    ByVal strExtensoesExt As String, _
                                    ByRef strErro As String) As Boolean
        Try
            strSQL = "UPDATE " & strTabela & " SET " & _
                    " AFTP_EXT_ARQU_USER_INTERNO = '" & strExtensoesInt.Trim() & "'" & _
                    " , AFTP_EXT_ARQU_USER_FORNECEDOR = '" & strExtensoesExt.Trim() & "'"

            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.Trim

        End Try

        Return (strErro = "")
    End Function
    Public Overloads Function Salvar(ByVal strUsuarios As String, _
                                    ByRef strErro As String) As Boolean
        Try
            strSQL = "UPDATE " & strTabela & " SET " & _
                    "ANOM_USER_ADMIN = '" & strUsuarios.Trim & "'"

            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.Trim

        End Try

        Return (strErro = "")
    End Function
    Public Overloads Function Salvar(ByVal intDiasExclusao As Integer, _
                                    ByRef strErro As String) As Boolean
        Salvar = False

        Dim objCriptografia As New aim.objCriptografia
        Dim objDataReader As SqlClient.SqlDataReader

        Try

            'Se há existe configuracao, faz apenas a atualizacao dos dados.
            strSQL = "SELECT ADAT_EXCLUSAO FROM TCONFIGURACAO"
            If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If objDataReader.HasRows Then

                strSQL = "UPDATE TCONFIGURACAO SET" & _
                    " ADAT_EXCLUSAO = " & CInt(0 & intDiasExclusao)
            Else

                strSQL = "INSERT INTO TCONFIGURACAO (ADAT_EXCLUSAO, AFTP_DIR_VIRTUAL " & _
                        ", AFTP_SERVIDOR, AFTP_WRITE_USER, AFTP_WRITE_PASS " & _
                        ", AFTP_READ_USER, AFTP_READ_PASS, AFTP_EXT_ARQU_USER_INTERNO " & _
                        ", AFTP_EXT_ARQU_USER_FORNECEDOR)" & _
                        " VALUES(" & CInt(0 & intDiasExclusao) & _
                        ")"
            End If

            objDataReader.Close()

            If Not objConexao.ExecutaSQL(strSQL) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If
        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Salvar = (strErro = "")

    End Function
    Public Function verificaConfiguracao(ByRef blnJaConfigurado As Boolean, _
                                        ByRef strErro As String) As Boolean

        verificaConfiguracao = False

        Dim objReader As SqlClient.SqlDataReader

        Try
            Dim strSQL As String
            strSQL = "SELECT ADAT_EXCLUSAO"
            strSQL += " FROM TCONFIGURACAO"

            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                strErro = objConexao.MensagemErroDB
                Exit Try
            End If

            'Retorna se o sistema já tem parametros de configuracao
            blnJaConfigurado = objReader.HasRows

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim

        End Try

        verificaConfiguracao = (strErro = "")

    End Function
End Class
