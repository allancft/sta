Imports System.IO
Imports System.Configuration

Public Class clsArquivo
    Sub Copiar(ByVal CaminhoFonte As String, ByVal CaminhoDestino As String, Optional ByVal Sobrepor As Boolean = False)
        Dim DiretorioFonte As DirectoryInfo = New DirectoryInfo(CaminhoFonte)
        Dim DiretorioDestino As DirectoryInfo = New DirectoryInfo(CaminhoDestino)

        If DiretorioFonte.Exists Then
            If Not DiretorioDestino.Parent.Exists Then
                Throw New DirectoryNotFoundException(" O diretório de destino não existe : " + DiretorioDestino.FullName)
            End If

            'Dim teste As Directory
            'teste.CreateDirectory(

            'teste.CreateDirectory(CaminhoDestino)

            If Not DiretorioDestino.Exists Then
                'MsgBox("O diretorio destino não existe , vou criá-lo", MsgBoxStyle.Critical, "Copia")

                DiretorioDestino.Create()
            End If

            'copia todos os arquivos do diretório
            Dim arquivos As FileInfo

            For Each arquivos In DiretorioFonte.GetFiles()
                If Sobrepor Then
                    arquivos.CopyTo(Path.Combine(DiretorioDestino.FullName, arquivos.Name), True)
                Else
                    If Not File.Exists(Path.Combine(DiretorioDestino.FullName, arquivos.Name)) Then
                        arquivos.CopyTo(Path.Combine(DiretorioDestino.FullName, arquivos.Name), False)
                    End If
                End If
            Next

            'copia todos os subdiretorios usando recursao
            Dim subdir As DirectoryInfo

            For Each subdir In DiretorioFonte.GetDirectories()
                Copiar(subdir.FullName, Path.Combine(DiretorioDestino.FullName, subdir.Name), Sobrepor)
            Next
        Else
            Throw New DirectoryNotFoundException("Diretório origem não existe " + DiretorioFonte.FullName)
        End If
    End Sub

    '		public Arquivo[] Listar()
    '		{
    '			//System.Security.AccessControl.DirectorySecurity clsDirectorySecurity = new System.Security.AccessControl.DirectorySecurity();
    '			//clsDirectorySecurity.
    '			//Directory.
    '			string[] Arquivos = Directory.s.GetFiles(Local, Nome);
    '			Arquivo[] arrArquivo = new Arquivo[Arquivos.Length];
    '			int Count = 0;

    '			foreach (string arquivo in Arquivos)
    '			{
    '				arrArquivo[Count] = new Arquivo();
    '				arrArquivo[Count]._Nome = arquivo.Replace(Local, "");
    '				arrArquivo[Count]._Local = _Local;
    '				Count += 1;
    '			}

    '			return arrArquivo;
    '}

    '****** Robson Firmino 18/12/09 ******
    'INICIO COMENTARIO
    'Este metodo mapeia a unidade de rede, com informações vindas do web.config e também formata campos com metodos mais abaixo. 
    'FIM COMENTARIO
    Public Function mapearUnidade(ByVal unidadeMapeada As String)

        Dim caminhoDoExecutavel As String
        Dim executavel As String
        Dim executarComando As String
        Dim comandoFechar As String

        'caminhoDoExecutavel = caminhoExe(System.Environment.GetEnvironmentVariable("ComSpec"))
        'executavel = ConfigurationSettings.AppSettings("executarPrograma")
        'comandoFechar = ConfigurationSettings.AppSettings("comandoFechar")
        'executarComando = formarLinhaComando(unidadeMapeada)

        'Dim dirInfor As DirectoryInfo = New DirectoryInfo(caminhoDoExecutavel)
        'Dim proInfo As System.Diagnostics.Process = New System.Diagnostics.Process
        'Dim start As System.Diagnostics.Process
        ''proInfo.StartInfo.FileName = dirInfor.FullName & executavel '& " " & comandoFechar
        'proInfo.StartInfo.FileName = dirInfor.FullName & "mapear.bat"
        ''proInfo.StartInfo.Arguments = executarComando
        'proInfo.StartInfo.CreateNoWindow = False
        'proInfo.StartInfo.ErrorDialog = True

        ''.Start = Inicia o processo e abaixo
        ''.WaitForExit = faz com que a aplicação aguarde até o fim da execução do processo.
        'proInfo.Start()
        'proInfo.WaitForExit()
        '' = System.Diagnostics.Process.Start(proInfo)

    End Function

    Private Function caminhoExe(ByVal caminhoCompleto As String) As String
        Dim caminho As String

        If (caminhoCompleto.Length > 0) Then
            caminho = caminhoCompleto
            caminho = caminho.Replace("cmd.exe", "")
        End If

        Return caminho
    End Function

    '****** Robson Firmino 18/12/09 ******
    'INICIO COMENTARIO
    'Este metodo forma uma linha de comando a ser executada para o mapeamento.
    'usa informações do web.config
    'retorna um comando (string) para ser executado dentro do aplicativo CMD.EXE do windows. 
    'FIM COMENTARIO
    'Private Function formarLinhaComando(ByVal unidadeMapeada As String) As String
    '    Dim comando As String
    '    Dim usuario As String = ConfigurationSettings.AppSettings("usuarioMapeamento")
    '    Dim senhaUsuario As String = ConfigurationSettings.AppSettings("senhaUsuarioMapeamento")
    '    Dim diretorioMapear As String = ConfigurationSettings.AppSettings("diretorioArquivos")
    '    'exclui a ultima "\" do caminho
    '    diretorioMapear = diretorioMapear.Substring(0, diretorioMapear.Length - 1)

    '    comando = "net use {0} {1} /USER:{2} {3}"
    '    comando = String.Format(comando, unidadeMapeada, diretorioMapear, usuario, senhaUsuario)

    '    Return comando
    'End Function

    Public Function isMapeado(ByVal unidadeMapeada As String) As Boolean

        'If Not unidadeMapeada.Equals(String.Empty) Then
        '    Dim unidade As DirectoryInfo = New DirectoryInfo(unidadeMapeada)
        '    Return unidade.Exists
        'End If

        Return False

    End Function

End Class
