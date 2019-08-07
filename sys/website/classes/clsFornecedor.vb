Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public Class clsFornecedor
    Public objConexao As clsDB
    Private strSQL As String
    Private strDirRaiz As String = ConfigurationSettings.AppSettings("diretorioArquivos").Trim()
    Dim objCriptografia As aim.objCriptografia
    Public Function ConsultaFornecedorLista(ByRef objDataSet As Data.DataSet, _
                                    ByRef strErro As String, _
                                    Optional ByVal strSort As String = "") As Boolean
        ConsultaFornecedorLista = False

        Try
            strSQL = "SELECT ANUM_SEQU_FORNECEDOR, ANOM_FORNECEDOR, " & _
                    " ANUM_CNPJ_CPF, ANUM_TEL_FORNECEDOR, ANOM_RESPONSAVEL_FORNECEDOR, ADES_EMAIL_FORNECEDOR " & _
                    " FROM TFORNECEDOR"
            If strSort.Trim <> "" Then
                strSQL += " ORDER BY " & strSort
            End If

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConsultaFornecedorLista = (strErro = "")

    End Function
    Public Function ConsultaFornecedor(ByRef objReaderAux As SqlClient.SqlDataReader, _
                                        ByRef strErro As String, _
                                        Optional ByVal strCodFornecedor As String = "") As Boolean
        ConsultaFornecedor = False

        Try
            strSQL = "SELECT ANUM_SEQU_FORNECEDOR, ANOM_FORNECEDOR, ANUM_CNPJ_CPF, " & _
                    " ANUM_TEL_FORNECEDOR, ADES_EMAIL_FORNECEDOR, ANOM_RESPONSAVEL_FORNECEDOR, " & _
                    " ADES_SENHA_FORNECEDOR, ACOD_FORNECEDOR_BAAN, ADES_EMAIL_RESPONSAVEL, " & _
                    " AFTP_EXT_ARQU, ADES_DIRETORIO_FORNECEDOR" & _
                    " FROM TFORNECEDOR"
            If strCodFornecedor <> "" Then
                strSQL += " WHERE ANUM_SEQU_FORNECEDOR = '" & strCodFornecedor & "'"
            End If

            If Not (objConexao.ExecuteReader(strSQL, objReaderAux)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConsultaFornecedor = (strErro = "")

    End Function
    Public Function ConsultaDestinoFornecedor(ByRef strErro As String, _
                                ByRef objDataSetAux As DataSet, _
                                Optional ByVal strCodFornecedor As String = "") As Boolean
        Try
            strSQL = "SELECT ANOM_FORNECEDOR AS cn, ADES_EMAIL_FORNECEDOR AS samAccountName, " & _
                    "  diretorio = case RTrim(LTrim(ISNULL(ACOD_FORNECEDOR_BAAN, '')))" & _
                    " when '' then ADES_DIRETORIO_FORNECEDOR" & _
                    " else ACOD_FORNECEDOR_BAAN" & _
                    " end " & _
                    ", ANUM_SEQU_FORNECEDOR AS CODSEQ" & _
                    " FROM TFORNECEDOR"
            If strCodFornecedor <> "" Then
                strSQL += " WHERE ANOM_FORNECEDOR LIKE '%" & strCodFornecedor & "%'"
            End If

            If Not (objConexao.ExecuteDataSet(objDataSetAux, "objDataSetAux", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConsultaDestinoFornecedor = (strErro = "")

    End Function
    Public Function Edita(ByVal intCodSeq As Integer, _
                            ByVal strCodFornecedor As String, _
                            ByVal strNome As String, _
                            ByVal strCNPJCPF As String, _
                            ByVal strEmail As String, _
                            ByVal strTelefone As String, _
                            ByVal strReponsavel As String, _
                            ByVal strEmailResponsavel As String, _
                            ByRef strErro As String, _
                            Optional ByVal strSenha As String = "", _
                            Optional ByVal strExtensoes As String = "", _
                            Optional ByVal strDiretorio As String = "", _
                            Optional ByVal strDirAntigo As String = "") As Boolean
        Try
            If (strDiretorio <> strDirAntigo) And (IO.Directory.Exists(strDirRaiz & "externo\" & strDiretorio)) Then
                Throw New System.Exception("O diretório especificado já existe no servidor.")
            End If

            strSQL = "UPDATE TFORNECEDOR SET " & _
                "ACOD_FORNECEDOR_BAAN = '" & strCodFornecedor & "', " & _
                "ANOM_FORNECEDOR = '" & strNome.Trim() & "', " & _
                "ANUM_CNPJ_CPF = '" & strCNPJCPF.Trim() & "', " & _
                "ANUM_TEL_FORNECEDOR = '" & strTelefone.Trim() & "', " & _
                "ADES_EMAIL_FORNECEDOR = '" & strEmail.Trim() & "', " & _
                "ANOM_RESPONSAVEL_FORNECEDOR = '" & strReponsavel.Trim() & "', " & _
                "ADES_EMAIL_RESPONSAVEL = '" & strEmailResponsavel.Trim() & "', " & _
                "ADES_DIRETORIO_FORNECEDOR = '" & strDiretorio.Trim() & "'"

            objCriptografia = New aim.objCriptografia

            If strSenha <> "" Then
                strSQL += ", ADES_SENHA_FORNECEDOR = '" & objCriptografia.Criptografar(strSenha) & "'"
            End If

            strSQL += ", AFTP_EXT_ARQU = '" & strExtensoes.Trim & "'"
            strSQL += "WHERE ANUM_SEQU_FORNECEDOR = " & intCodSeq

            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If (strDiretorio <> strDirAntigo) And (IO.Directory.Exists(strDirRaiz & "externo\" & strDirAntigo)) Then
                IO.Directory.Move(strDirRaiz & "externo\" & strDirAntigo, strDirRaiz & "externo\" & strDiretorio)
            End If

        Catch objErro As Exception
            strErro = objErro.Message.ToString().Trim()
        End Try

        Return (strErro = "")

    End Function
    Public Function Inclui(ByVal strNome As String, _
                            ByVal strCNPJCPF As String, _
                            ByVal strEmail As String, _
                            ByVal strTelefone As String, _
                            ByVal strReponsavel As String, _
                            ByVal strSenha As String, _
                            ByVal strEmailResponsavel As String, _
                            ByVal strExtensoes As String, _
                            ByVal strDiretorio As String, _
                            ByRef strErro As String, _
                            Optional ByVal strCodigo As String = "")
        Try
            'Verifica se o Diretório especificado já existe no servidor.
            Dim strDirAux As String = strDirRaiz & "externo\" & strDiretorio
            If (IO.Directory.Exists(strDirAux)) Then
                Throw New System.Exception("O diretório especificado já existe no servidor.")
            End If

            objCriptografia = New aim.objCriptografia

            'Inclui Fornecedor no Banco de Dados
            Dim objReader As SqlClient.SqlDataReader

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_FORNECEDOR), 0) + 1" & _
                " AS CODIGO FROM TFORNECEDOR"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            Dim intCodSeq As Integer = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TFORNECEDOR (ANUM_SEQU_FORNECEDOR, ACOD_FORNECEDOR_BAAN, " & _
                " ANOM_FORNECEDOR, ANUM_CNPJ_CPF, ANUM_TEL_FORNECEDOR, ADES_EMAIL_FORNECEDOR," & _
                " ANOM_RESPONSAVEL_FORNECEDOR, ADES_SENHA_FORNECEDOR, ADES_EMAIL_RESPONSAVEL," & _
                " ADES_DIRETORIO_FORNECEDOR, AFTP_EXT_ARQU) VALUES(" & _
                intCodSeq & ", " & _
                "'" & strCodigo.Trim & "'," & _
                "'" & strNome.Trim & "'," & _
                "'" & strCNPJCPF.Trim & "'," & _
                "'" & strTelefone.Trim & "'," & _
                "'" & strEmail.Trim & "'," & _
                "'" & strReponsavel.Trim & "'," & _
                "'" & objCriptografia.Criptografar(strSenha.Trim) & "'," & _
                "'" & strEmailResponsavel.Trim & "'," & _
                "'" & strDiretorio.Trim & "'," & _
                "'" & strExtensoes.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As Exception
            strErro = objErro.Message.Trim

        Finally
            If Not IsNothing(objConexao) Then objConexao.FechaConexao()

        End Try

        Return (strErro = "")

    End Function
    Public Function Exclui(ByVal intCodFornecedor As Integer, _
                                    ByRef strErro As String) As Boolean
        Try
            Dim objReader As SqlDataReader
            Dim strDirAux As String

            strSQL = "SELECT ADES_DIRETORIO_FORNECEDOR FROM TFORNECEDOR" & _
                " WHERE ANUM_SEQU_FORNECEDOR = " & intCodFornecedor
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If objReader.HasRows Then
                objReader.Read()
                strDirAux = objReader("ADES_DIRETORIO_FORNECEDOR").ToString.Trim

            End If

            objReader.Close()

            'exclui historicos de transacoes
            strSQL = "DELETE TARQUIVO" & _
                    " WHERE ANUM_SEQU_FORNECEDOR = '" & intCodFornecedor & "'"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            strSQL = "DELETE TREGISTRO_TRANSMISSAO" & _
                    " WHERE ANUM_SEQU_FORNECEDOR = '" & intCodFornecedor & "'"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            'exclui logs de acessos
            strSQL = "DELETE TLOG_ACESSO_FORNECEDOR" & _
                    " WHERE ANUM_SEQU_FORNECEDOR = '" & intCodFornecedor & "'"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            'excluir fornecedor no banco
            strSQL = "DELETE TFORNECEDOR" & _
                    " WHERE ANUM_SEQU_FORNECEDOR = '" & intCodFornecedor & "'"

            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            'exclui diretório fisico
            If Directory.Exists(strDirRaiz & "externo\" & strDirAux) Then
                IO.Directory.Delete(strDirRaiz & "externo\" & strDirAux)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")
    End Function
End Class
