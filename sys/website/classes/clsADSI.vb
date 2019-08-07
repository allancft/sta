Imports System.Configuration
Imports System.DirectoryServices
Imports System.Data
Public Class clsADSI

    Private StrNomeServidorAD As String = ConfigurationSettings.AppSettings("nomeServidorAD")
    Private StrNomeContaAD As String = ConfigurationSettings.AppSettings("contaAD")
    Private StrSenhaContaAD As String = ConfigurationSettings.AppSettings("senhaAD")
    Protected entradaAD As DirectoryEntry
    'Retorna DataSet com resultado de pesquisa no Active Directory
    Function pesquisaUsuario(ByVal strUsuario As String, _
                            ByRef strErro As String) As Data.DataSet

        'Descriptografar senha da Conta AD
		Dim objCriptografia As aim.objCriptografia
        objCriptografia = New aim.objCriptografia
        StrSenhaContaAD = objCriptografia.Descriptografar(StrSenhaContaAD)
        objCriptografia = Nothing

        Try
            Dim entradaRoot As New DirectoryEntry( _
                                    "LDAP://" & StrNomeServidorAD, _
                                    StrNomeContaAD, _
                                    StrSenhaContaAD, _
                                    AuthenticationTypes.Secure)

            Dim objPesquisa As New DirectorySearcher(entradaRoot)
            objPesquisa.PropertiesToLoad.Add("cn")
            objPesquisa.PropertiesToLoad.Add("samAccountName")
            objPesquisa.ServerTimeLimit = New TimeSpan(0, 10, 0)

            If strUsuario <> "" Then
                objPesquisa.Filter = String.Format("(&(objectCategory=person)(objectClass=user)(cn={0}))", "*" & strUsuario & "*")
            Else
                objPesquisa.Filter = String.Format("(&(objectCategory=person)(objectClass=user))")
            End If

            Dim pesquisaResultados As SearchResultCollection

            'Depois de pesquisar o Active Directory,
            'faz loops para retornar um dataset para
            'então dar um bind() no datagrid.
            pesquisaResultados = objPesquisa.FindAll()

            Dim dtResultados As New DataTable("Results")
            Dim nomeColuna As String

            For Each nomeColuna In objPesquisa.PropertiesToLoad
                dtResultados.Columns.Add(nomeColuna, GetType(System.String))
            Next
            dtResultados.Columns.Add("diretorio", GetType(System.String))
            dtResultados.Columns.Add("codigo", GetType(System.String))
            dtResultados.Columns.Add("codSeq", GetType(Integer))

            Dim objResultado As SearchResult

            'Comeca a popular o DataSet
            For Each objResultado In pesquisaResultados
                Dim dr As DataRow = dtResultados.NewRow()
                For Each nomeColuna In objPesquisa.PropertiesToLoad
                    If objResultado.Properties.Contains(nomeColuna) Then
                        dr(nomeColuna) = CStr(objResultado.Properties(nomeColuna)(0)).Trim()
                    Else
                        dr(nomeColuna) = ""
                    End If
                    dr("diretorio") = ""
                    dr("codSeq") = 0
                Next
                dtResultados.Rows.Add(dr)
            Next

            Dim dsRes As New DataSet
            dsRes.Tables.Add(dtResultados)

            Return dsRes 'Retorna Dataset com dados

        Catch objErro As Exception
            strErro = objErro.Message.ToString().Trim()
        End Try

    End Function
    Function ValidaUsuario(ByVal strUsuario As String, _
                            ByVal strSenha As String, _
                            ByRef strErro As String) As Boolean
        Try
            Dim entradaRoot As New DirectoryEntry( _
                                    "LDAP://" & StrNomeServidorAD, _
                                    strUsuario, _
                                    strSenha, _
                                    AuthenticationTypes.Secure)

            Dim objPesquisa As New DirectorySearcher(entradaRoot)
            objPesquisa.PropertiesToLoad.Add("cn")
            objPesquisa.FindOne()

            If IsNothing(objPesquisa) Then
                Throw New System.Exception("Usuário ou Senha incorretos")
            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.Trim()
        End Try

        ValidaUsuario = (strErro = "")

    End Function
End Class
