Imports System.Data
Imports System.Data.SqlClient
Public Class clsEnvio
    Public objConexao As clsDB
    Private strSQL As String
    Private blnRet As Boolean
    Public Function IncluiTransmissaoExterna(ByRef strSeqTransacao As String, _
                                            ByVal strDirFornecedor As String, _
                                            ByVal intTipoEnvio As Integer, _
                                            ByVal intCodSeq As Integer, _
                                            ByRef strErro As String) As Boolean

        Dim intSeqReg, intSeqArquivo As Integer

        Try
            Dim objReader As SqlClient.SqlDataReader

            'Busca código do novo Registro de Transmissao
            strSQL = "EXEC SP_NUMERACAO"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception("(1): " & objConexao.MensagemErroDB)
            End If

            objReader.Read()

            strSeqTransacao = objReader("CODIGO").ToString.Trim

            objReader.Close()

            'Busca o sequencial do fornecedor a partir da descricao do diretorio
            strSQL = "SELECT ANUM_SEQU_FORNECEDOR FROM TFORNECEDOR" & _
                    " WHERE ADES_DIRETORIO_FORNECEDOR = '" & strDirFornecedor & "'"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            Dim intSeqFornecedor As Integer
            If objReader.HasRows Then
                objReader.Read()
                intSeqFornecedor = objReader("ANUM_SEQU_FORNECEDOR")
            Else
                intSeqFornecedor = strDirFornecedor
            End If

            objReader.Close()

            strSQL = "INSERT INTO TREGISTRO_TRANSMISSAO (" & _
                    "ANUM_SEQU_FORNECEDOR, ANUM_SEQU_REGISTRO, ATIP_PROTOCOLO, " & _
                    "ABOL_VALIDACAO_PROTOCOLO, ADAT_PROTOCOLO, ABOL_EXCLUSAO" & _
                    ") VALUES(" & _
                    intSeqFornecedor & ", " & _
                    strSeqTransacao.Trim & ", " & _
                    (intTipoEnvio - 1) & ", " & _
                    "0, GETDATE(), 0)"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception("(2): " & objConexao.MensagemErroDB & "<br /><br />" & strSQL)
            End If

        Catch objErro As Exception
            strErro = objErro.Message.Trim()

        End Try

        IncluiTransmissaoExterna = (strErro = "")

    End Function
    Public Function IncluirTransmissaoInterna(ByRef strSeqTransacao As String, _
                                            ByVal strRemetente As String, _
                                            ByVal strDestino As String, _
                                            ByRef strErro As String) As Boolean
        Try
            Dim objReader As SqlClient.SqlDataReader

            'Busca código do novo Registro de Transmissao
            strSQL = "EXEC SP_NUMERACAO"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            strSeqTransacao = objReader("CODIGO").ToString.Trim

            objReader.Close()

            strSQL = "INSERT INTO TREGISTRO_TRANS_INTERNO VALUES(" & _
                    strSeqTransacao & ", '" & strRemetente & "', '" & strDestino & " '," & _
                    "GETDATE())"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As Exception
            strErro = objErro.Message.Trim()

        End Try

        IncluirTransmissaoInterna = (strErro = "")

    End Function
    Public Function IncluirTransmissaoTemporaria(ByRef strErro As String, _
                                                ByRef strChave As String, _
                                                ByVal strUsuario As String) As Boolean
        Try
            Dim objCriptografia As aim.objCriptografia
            objCriptografia = New aim.objCriptografia

            Dim strChaveCriptografada As String
            strChaveCriptografada = objCriptografia.Criptografar(Now)
            strChaveCriptografada = strChaveCriptografada.Replace("/", "")
            strChaveCriptografada = strChaveCriptografada.Replace("\", "")

            strSQL = "INSERT INTO TCHAVE_TEMPORARIA"
            strSQL += " VALUES('" & strChaveCriptografada & "', '"
            strSQL += strUsuario & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            strChave = strChaveCriptografada

        Catch objErro As Exception
            strErro = objErro.Message.Trim()

        End Try

        Return (strErro = "")

    End Function
    Public Function IncluiRegArquivoExterno(ByVal intSeqFornecedor As Integer, _
                                            ByVal intSeqRegistro As Integer, _
                                            ByVal strNomeArquivo As String, _
                                            ByRef strErro As String) As Boolean
        Try

            Dim intSeqArquivo As Integer
            Dim objReader As SqlClient.SqlDataReader

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_ARQUIVO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO" & _
                    " WHERE ANUM_SEQU_FORNECEDOR = " & intSeqFornecedor & _
                    " AND ANUM_SEQU_REGISTRO = " & intSeqRegistro
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO VALUES(" & _
                    intSeqFornecedor & ", " & _
                    intSeqRegistro & ", " & _
                    intSeqArquivo & ", '" & _
                    strNomeArquivo & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.Trim()

        Finally
            IncluiRegArquivoExterno = (strErro = "")

        End Try
    End Function
    Public Function IncluiRegArquivoInterno(ByVal intEnvio As Integer, _
                                            ByVal strNomeArquivo As String, _
                                            ByRef strErro As String) As Boolean
        Try
            Dim intSeqArquivo As Integer
            Dim objReader As SqlClient.SqlDataReader

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_ARQUIVO_INTERNO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO_INTERNO" & _
                    " WHERE ANUM_SEQU_REGISTRO_INTERNO = " & intEnvio
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO_INTERNO (ANUM_SEQU_ARQUIVO_INTERNO," & _
                    " ANUM_SEQU_REGISTRO_INTERNO, ANOM_ARQUIVO) VALUES(" & _
                    intSeqArquivo & ", " & _
                    intEnvio & ", '" & _
                    strNomeArquivo & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB & vbCrLf & strSQL)
            End If

        Catch ex As System.Exception
            strErro = ex.Message.Trim()

        End Try
        Return (strErro = "")
    End Function
End Class
