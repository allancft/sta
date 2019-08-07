Attribute VB_Name = "modMain"
Option Explicit

Public StrConexao As String
Public StrDiretorioBase As String
Public StrFTPServidor As String
Public StrFTPUsuario As String
Public StrFTPSenha As String
Public StrURL As String
Public StrURLSite As String
Public StrDiretorioIIS As String
Public Sub GravarLogErro(ByVal strErro As String)
    
    Dim objFSOAux As FileSystemObject
    Set objFSOAux = New FileSystemObject
    Dim arqAux
    Set arqAux = objFSOAux.OpenTextFile(App.Path & "\logExclusao.log", ForAppending, True)
    
    Call arqAux.WriteLine(Format(Now, "dd/mm/yyyy hh:nn") & " - " & strErro)
    
    arqAux.Close
    Set objFSOAux = Nothing

End Sub
Public Sub CarregaConfiguracao()
    
    On Error GoTo Trata
    
    Dim arquivoTexto, linha
    Dim intLinha As Integer
    Dim strSQLUsuario As String
    Dim strSQLSenha As String
    Dim strSQLServidor As String
    Dim strSQLBanco As String
    
    arquivoTexto = FreeFile()
    
    Open App.Path & "\configuracao.ini" For Input As arquivoTexto
    
    'Agora vamos ver se o usuário logado está no arquivo texto
    Do While Not EOF(arquivoTexto)
        Line Input #arquivoTexto, linha
        
        If intLinha = 1 Then
            StrFTPServidor = Trim(linha)
        ElseIf intLinha = 2 Then
            StrFTPUsuario = Trim(linha)
        ElseIf intLinha = 3 Then
            StrFTPSenha = Trim(linha)
        ElseIf intLinha = 4 Then
            StrDiretorioBase = Trim(linha)
        ElseIf intLinha = 5 Then
            StrDiretorioIIS = Trim(linha)
        ElseIf intLinha = 6 Then
            StrURLSite = Trim(linha)
        ElseIf intLinha = 7 Then
            strSQLUsuario = Trim(linha)
        ElseIf intLinha = 8 Then
            strSQLSenha = Trim(linha)
        ElseIf intLinha = 9 Then
            strSQLServidor = Trim(linha)
        ElseIf intLinha = 10 Then
            strSQLBanco = Trim(linha)
        End If
        
        intLinha = intLinha + 1
    Loop

    
    StrConexao = "Provider=SQLOLEDB.1;User Id=" & strSQLUsuario & _
                ";Pwd=" & strSQLSenha & _
                ";Data Source=" & strSQLServidor & _
                ";Initial Catalog=" & strSQLBanco

    Close #arquivoTexto
    
    Exit Sub
    
Trata:

    GravarLogErro (Err.Number & " - " & Err.Description)
    
End Sub
