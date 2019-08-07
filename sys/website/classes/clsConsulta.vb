Imports System.data
Imports System.Data.SqlClient
Public Class clsConsulta
    Public objConexao As clsDB
    Private strSQL As String
    Public Function ConsultaArquivosExternos(ByVal intCodigo As Integer, _
                                            ByRef objDataSet As Data.DataSet, _
                                            ByRef strErro As String) As Boolean
        Try
            strSQL = "SELECT ANOM_ARQUIVO FROM TARQUIVO" & _
                    " WHERE ANUM_SEQU_REGISTRO = " & intCodigo

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")

    End Function
    Public Function ConsultaArquivosInternos(ByVal intCodigo As Integer, _
                                            ByRef objDataSet As Data.DataSet, _
                                            ByRef strErro As String) As Boolean
        Try
            strSQL = "SELECT ANOM_ARQUIVO FROM TARQUIVO_INTERNO" & _
                    "  WHERE ANUM_SEQU_ARQUIVO_INTERNO = " & intCodigo

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        Return (strErro = "")

    End Function
    Public Function ConsultaExterna(ByRef objDataSet As Data.DataSet, _
                            ByRef strErro As String, _
                            Optional ByVal strSort As String = "", _
                            Optional ByVal strEmail As String = "", _
                            Optional ByVal intTipo As Integer = -1, _
                            Optional ByVal intValidados As Integer = -1, _
                            Optional ByVal intExcluidos As Integer = -1, _
                            Optional ByVal intCodSeq As Integer = 0) As Boolean
        Try
            strSQL = "SELECT DISTINCT A.ANUM_SEQU_REGISTRO, B.ANUM_SEQU_FORNECEDOR, B.ANOM_FORNECEDOR, " & _
                " CASE A.ATIP_PROTOCOLO" & _
                "   WHEN 0 THEN 'Envio'" & _
                "   WHEN 1 THEN 'Recebimento' " & _
                " END AS TIPO," & _
                " CASE A.ABOL_VALIDACAO_PROTOCOLO" & _
                "   WHEN 0 THEN 'Não'" & _
                "   WHEN 1 THEN 'Sim'" & _
                " END AS VALIDACAO," & _
                " CONVERT(CHAR(10), A.ADAT_PROTOCOLO, 103) AS DATA," & _
                " CASE A.ABOL_EXCLUSAO" & _
                "   WHEN 0 THEN 'Não'" & _
                "   WHEN 1 THEN 'Sim'" & _
                " END AS EXCLUSAO" & _
                " FROM TREGISTRO_TRANSMISSAO A, TFORNECEDOR B" & _
                " WHERE A.ANUM_SEQU_FORNECEDOR = B.ANUM_SEQU_FORNECEDOR" & _
                " AND A.ANUM_SEQU_REGISTRO IN (SELECT ANUM_SEQU_REGISTRO FROM TARQUIVO)"

            If strEmail.Trim() <> "" Then
                strSQL += " AND B.ADES_EMAIL_FORNECEDOR = '" & strEmail.Trim() & "'"
            End If
            If intTipo <> -1 Then
                strSQL += " AND A.ATIP_PROTOCOLO = " & intTipo
            End If
            If intValidados <> -1 Then
                strSQL += " AND A.ABOL_VALIDACAO_PROTOCOLO = " & intValidados
            End If
            If intExcluidos <> -1 Then
                strSQL += " AND A.ABOL_EXCLUSAO = " & intExcluidos
            End If
            If intCodSeq <> 0 Then
                strSQL += " AND A.ANUM_SEQU_FORNECEDOR = " & intCodSeq
            End If
            If strSort.Trim() <> "" Then
                strSQL += " ORDER BY " & strSort
            End If

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConsultaExterna = (strErro = "")

    End Function
    Public Function ConsultaInterna(ByRef objDataSet As Data.DataSet, _
                                    ByRef strErro As String, _
                                    Optional ByVal strSort As String = "", _
                                    Optional ByVal strDestinatario As String = "", _
                                    Optional ByVal strDataInicial As String = "", _
                                    Optional ByVal strDataFinal As String = "") As Boolean
        Try
            strSQL = "SELECT ANUM_SEQU_REGISTRO_INTERNO, " & _
                    " ADES_DESTINATARIO, CONVERT(CHAR(10), ADAT_ENVIO, 103) AS DATA" & _
                    " FROM TREGISTRO_TRANS_INTERNO" & _
                    " WHERE ANUM_SEQU_REGISTRO_INTERNO > 0"
            If strDestinatario <> "" Then
                strSQL += " AND ADES_DESTINATARIO LIKE '%" & strDestinatario & "%'"
            End If
            If strDataInicial <> "" And strDataFinal <> "" Then
                strSQL += " AND ADAT_ENVIO BETWEEN CONVERT(SMALLDATETIME, '" & strDataInicial & "', 103)"
                strSQL += " AND CONVERT(SMALLDATETIME, '" & strDataFinal & "', 103)"
            End If
            If strSort <> "" Then
                strSQL += " ORDER BY " & strSort
            End If

            If Not (objConexao.ExecuteDataSet(objDataSet, "objDataSet", strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()

        End Try

        ConsultaInterna = (strErro = "")
    End Function
End Class
