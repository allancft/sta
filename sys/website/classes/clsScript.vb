Public Class clsScript
    Private Shared strMsg As String
    Public Shared Function limpaStringMensagem(ByVal strAux As String) As String
        'Remove quebras de linha
        strAux = strAux.Replace("\", "\\")
        strAux = strAux.Replace(Chr(13), "\n")
        strAux = strAux.Replace(Chr(10), "\n")
        strAux = strAux.Replace(vbCr, "\n")
        strAux = strAux.Replace("'", "\'")
        strAux = strAux.Replace("""", "\""")
        limpaStringMensagem = strAux
    End Function
    Public Shared Sub exibirAlerta(ByRef objPagina As System.Web.UI.Page, _
                                    ByVal strChave As String)
        Dim strScript As String

        strScript = "<script type=""text/javascript"" language=""javascript"">" & vbCrLf
        strScript += vbTab & "alert('" & limpaStringMensagem(strMsg) & "');" & vbCrLf
        strScript += "</script>" & vbCrLf

        If (Not objPagina.IsStartupScriptRegistered(strChave)) Then
            objPagina.RegisterStartupScript(strChave, strScript)
        End If

    End Sub

    Public Shared Sub eventoObjetoGenerico(ByRef objPagina As System.Web.UI.Page, _
                                            ByVal strControle As String, _
                                            ByVal strEvento As String, _
                                            ByVal strCodigo As String)

        Dim objHTML As System.Web.UI.HtmlControls.HtmlGenericControl
        objHTML = objPagina.FindControl(strControle)

        If Not objHTML Is Nothing Then objHTML.Attributes.Add(strEvento, strCodigo)

        objHTML = Nothing

    End Sub

End Class
