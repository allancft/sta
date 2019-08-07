Imports System.Web.Mail
Imports System.Collections
Imports System.Configuration
Public Class clsEmail
#Region "Vari�veis"
    Private strTextoMensagem As String
    Private intTipoAcesso As Integer = 1
#End Region
#Region "Classes"
    Private objCriptografia As aim.objCriptografia
    Private objMail As System.Web.Mail.SmtpMail
    Private objDB As clsDB
#End Region
    Private Enum enumTipoMensagem
        Interno = 1
        Externo = 2
        Temporario = 3
        EnvioSenha = 4
    End Enum
    Private Enum enumTipoAcesso
        Interno = 1
        Externo = 2
    End Enum
    Public Overloads Function EnviaEmail(ByVal strDestino As String, _
                                        ByVal strRemetente As String, _
                                        ByVal strCodigo As String, _
                                        ByVal strArquivos As String, _
                                        ByVal intTipoMensagem As Integer, _
                                        ByVal intTipoAcesso As Integer, _
                                        ByRef strErro As String, _
                                        Optional ByVal strVirus As String = "", _
                                        Optional ByVal strMensagem As String = "") As Boolean
        Dim objMailMsg As New System.Web.Mail.MailMessage
        Try
            With objMailMsg
                .Priority = MailPriority.Normal
                .From = strRemetente.Trim
                .To = strDestino.Trim
                .Subject = ConfigurationSettings.AppSettings.Get("nomeSistema").Trim
                .Body = TextoMensagem(CType(intTipoMensagem, enumTipoMensagem), _
                                        CType(intTipoAcesso, enumTipoAcesso), _
                                        strCodigo, _
                                        strArquivos, _
                                        strVirus, _
                                        strMensagem)
            End With

            objMail.SmtpServer = ConfigurationSettings.AppSettings.Get("nomeServidorCorreio")
            objMail.Send(objMailMsg)

        Catch ex As Exception
            strErro = ex.Message.Trim

        Finally
            If Not (IsNothing(objMailMsg)) Then objMailMsg = Nothing

        End Try

        Return (strErro = "")
    End Function
    Public Overloads Function EnviaEmail(ByVal strDestino As String, _
                                        ByVal strTexto As String, _
                                        ByRef strErro As String) As Boolean

        Dim objMailMsg As New System.Web.Mail.MailMessage

        Try
            With objMailMsg
                .Priority = MailPriority.Normal
                .From = "sta@" & ConfigurationSettings.AppSettings.Get("dominioCorreio").Trim
                .To = strDestino.Trim
                .Subject = ConfigurationSettings.AppSettings.Get("nomeSistema").Trim
                .Body = strTexto.Trim
            End With

            objMail.SmtpServer = ConfigurationSettings.AppSettings.Get("nomeServidorCorreio")
            objMail.Send(objMailMsg)

        Catch ex As Exception
            strErro = ex.Message.Trim

        Finally
            If Not (IsNothing(objMailMsg)) Then objMailMsg = Nothing

        End Try

        Return (strErro = "")
    End Function
    Private Function TextoMensagem(ByVal tipoMensagem As enumTipoMensagem, _
                                ByVal tipoAcesso As enumTipoAcesso, _
                                Optional ByVal strCodigo As String = "", _
                                Optional ByVal strTextoAux As String = "", _
                                Optional ByVal strVirus As String = "", _
                                Optional ByVal strMensagem As String = "") As String

        strTextoMensagem = "Voc� est� recebendo este e-mail, em notifica��o"
        strTextoMensagem += " ao recebimento de arquivos para c�pia atrav�s do"
        strTextoMensagem += " Sistema de Transfer�ncia de Arquivos da Itamb�." & vbCrLf & vbCrLf

        Select Case tipoMensagem
            Case enumTipoMensagem.Interno, enumTipoMensagem.Externo
                If strCodigo.Trim <> "" Then
                    strTextoMensagem += "O c�digo dessa transfer�ncia �: " & strCodigo & vbCrLf & vbCrLf
                End If

                If tipoMensagem = enumTipoMensagem.Interno Or tipoMensagem = enumTipoMensagem.Externo Then
                    strTextoMensagem += "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
                    If tipoAcesso = enumTipoAcesso.Interno And tipoMensagem = enumTipoMensagem.Interno Then
                        strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteInterno")
                    Else
                        strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                    End If
                ElseIf tipoMensagem = enumTipoMensagem.Temporario Then

                    strTextoMensagem += "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                    strTextoMensagem += "/chave.aspx?chave=" & System.Web.HttpUtility.UrlEncode(strCodigo)
                End If
                strTextoMensagem += vbCrLf & vbCrLf & "Abaixo, segue a lista dos arquivos disponibilizados:" & vbCrLf & vbCrLf
            Case enumTipoMensagem.Temporario
                If tipoAcesso = enumTipoAcesso.Interno Then
                    strTextoMensagem = "Mensagem:" & vbCrLf & strMensagem & vbCrLf
                    strTextoMensagem += "___________________________________________________________________" & vbCrLf & vbCrLf
                    strTextoMensagem += "Voc� est� recebendo este e-mail, em notifica��o"
                    strTextoMensagem += " ao recebimento de uma chave tempor�ria de acesso ao"
                    strTextoMensagem += " Sistema de Transfer�ncia de Arquivos da Itamb�." & vbCrLf & vbCrLf
                    strTextoMensagem += "Atrav�s desta chave tempor�ria, voc� estar� habilitado"
                    strTextoMensagem += " a enviar arquivos para a rede Itamb�." & vbCrLf & vbCrLf
                    strTextoMensagem += "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                    strTextoMensagem += "/chave.aspx?chave=" & System.Web.HttpUtility.UrlEncode(strCodigo) & vbCrLf & vbCrLf & vbCrLf & vbCrLf
                    strTextoMensagem += "Utiliza��o do sistema: ap�s clicar no link acima, use o menu ""Transfer�ncias"" para enviar ou copiar os arquivos de seu interesse."
                Else
                    strTextoMensagem += "Voc� est� recebendo este e-mail, em notifica��o"
                    strTextoMensagem += " ao recebimento de arquivos para c�pia atrav�s do"
                    strTextoMensagem += " Sistema de Transfer�ncia de Arquivos da Itamb�." & vbCrLf & vbCrLf
                    strTextoMensagem += "O c�digo dessa transfer�ncia �: " & strCodigo & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteInterno") & vbCrLf & vbCrLf & vbCrLf & vbCrLf
                    strTextoMensagem += "Utiliza��o do sistema: ap�s clicar no link acima, use o menu ""Transfer�ncias"" para enviar ou copiar os arquivos de seu interesse."
                End If
                If Not (IsNothing(strTextoAux)) Then
                    strTextoMensagem += vbCrLf & vbCrLf & "Abaixo, segue a lista dos arquivos disponibilizados:" & vbCrLf & vbCrLf
                End If
        End Select

        If Not (IsNothing(strTextoAux)) Then
            strTextoMensagem += strTextoAux.Trim
        End If

        If Not (IsNothing(strVirus)) Then
            strTextoMensagem += vbCrLf & vbCrLf & _
                                "Aten��o: Os arquivos a seguir n�o foram aceitos e foram " & _
                                "exclu�dos ap�s terem sido enviados devido a presen�a " & _
                                "de v�rus." & vbCrLf & vbCrLf & _
                                strVirus.Trim
        End If
        strTextoMensagem += vbCrLf & vbCrLf
        strTextoMensagem += ".................................................................." & vbCrLf & vbCrLf
        '       strTextoMensagem += "You are receiving this e-mail as a notification "
        '      strTextoMensagem += " of the availability of files to copy, through the Itamb� File Transfer System" & vbCrLf & vbCrLf

        Select Case tipoMensagem
            Case enumTipoMensagem.Interno, enumTipoMensagem.Externo
                'If strCodigo.Trim <> "" Then
                'strTextoMensagem += "The code of this transfer is: " & strCodigo & vbCrLf & vbCrLf
                'End If

                'If tipoMensagem = enumTipoMensagem.Interno Or tipoMensagem = enumTipoMensagem.Externo Then
                'strTextoMensagem += "Click on the below link to access:" & vbCrLf & vbCrLf
                'If tipoAcesso = enumTipoAcesso.Interno Then
                'strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteInterno")
                'Else
                '   strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                'End If
                If tipoMensagem = enumTipoMensagem.Temporario Then
                    strTextoMensagem += "Click on the under link to access:" & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                    strTextoMensagem += "/chave.aspx?chave=" & System.Web.HttpUtility.UrlEncode(strCodigo)
                End If
                'strTextoMensagem += vbCrLf & vbCrLf & "below is the list of available files:" & vbCrLf & vbCrLf
            Case enumTipoMensagem.Temporario
                If tipoAcesso = enumTipoAcesso.Interno Then
                    strTextoMensagem += "You are receiving this e-mail as a notification "
                    strTextoMensagem += " of the availability of a temporary access key to"
                    strTextoMensagem += " the Itamb� File Transfer System." & vbCrLf & vbCrLf
                    strTextoMensagem += "Using this temporary key, you will be able "
                    strTextoMensagem += " to send files to Itamb�'s internal network." & vbCrLf & vbCrLf
                    strTextoMensagem += "Click on the below link to access:" & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")
                    strTextoMensagem += "/chave.aspx?chave=" & System.Web.HttpUtility.UrlEncode(strCodigo)
                Else
                    strTextoMensagem += "You are receiving this e-mail as a notification"
                    strTextoMensagem += " of the availability of files to copy, through the Itamb� File Transfer System." & vbCrLf & vbCrLf
                    strTextoMensagem += "The code of this transfer is: " & strCodigo & vbCrLf & vbCrLf
                    strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteInterno")
                End If
                If Not (IsNothing(strTextoAux)) Then
                    strTextoMensagem += vbCrLf & vbCrLf & "below is the list of available files:" & vbCrLf & vbCrLf
                End If
        End Select

        If Not (IsNothing(strTextoAux)) Then
            ' strTextoMensagem += strTextoAux.Trim
        End If

        If Not (IsNothing(strVirus)) Then
            strTextoMensagem += vbCrLf & vbCrLf & _
                                "Aten��o: Os arquivos a seguir n�o foram aceitos e foram " & _
                                "exclu�dos ap�s terem sido enviados devido a presen�a " & _
                                "de v�rus." & vbCrLf & vbCrLf & _
                                strVirus.Trim
        End If

        Return strTextoMensagem

    End Function
    Public Function EnviaSenhaAcesso(ByVal strEmail As String, _
                                    ByVal strSenhaCripto As String, _
                                    ByRef strErro As String) As Boolean
        Try
            objCriptografia = New aim.objCriptografia
            Dim strSenhaAux As String
            strSenhaAux = objCriptografia.Descriptografar(strSenhaCripto)

            strTextoMensagem = "Voc� est� recebendo sua senha de acesso conforme solicita��o realizada"
            strTextoMensagem += " em " & FormatDateTime(Now, DateFormat.LongDate) & vbCrLf & vbCrLf
            strTextoMensagem += "Sua senha �: " & strSenhaAux & vbCrLf & vbCrLf
            strTextoMensagem += "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
            strTextoMensagem += ConfigurationSettings.AppSettings.Get("URLSiteExterno")

            If Not (EnviaEmail(strEmail, strTextoMensagem, strErro)) Then
                Throw New System.Exception(strErro)
            End If

        Catch ex As Exception
            strErro = ex.Message.Trim()

        End Try
        Return (strErro = "")
    End Function
End Class
