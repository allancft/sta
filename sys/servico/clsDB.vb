Imports System
Imports System.Data
Imports System.Collections
Imports System.Configuration
Imports System.Data.SqlClient
'Classe para Realizar Operações no banco de Dados e Tratamentos de Erro
Public NotInheritable Class clsDB
    'Declaração de Variáveis Locais para Armazenamento Temporário das Propriedades da Classe Base de Dados
    Private str_auxConexao As String
    Private obj_auxConexao As New SqlClient.SqlConnection
    Private str_auxmensagemerroDb As String
    Private str_auxSql As String
    Private bol_auxmostraMensagem As Boolean
    Private obj_auxsqlException As SqlClient.SqlException
    Private obj_auxParametros As Object

#Region "Propriedades"
    'Propriedade que Recebe / armazena comando SQL
    Public Property SQL() As String
        Get
            Return str_auxSql
        End Get
        Set(ByVal str_value As String)
            str_auxSql = str_value
        End Set
    End Property
    'Propriedade que seta e retorna conexao
    Public Property Conexao() As SqlClient.SqlConnection
        Get
            Return obj_auxConexao
        End Get
        Set(ByVal Value As SqlClient.SqlConnection)
            obj_auxConexao = Value
        End Set
    End Property
    'Propriedade que Retorna Mensagem de Erro
    Public ReadOnly Property MensagemErroDB() As String
        Get
            Return str_auxmensagemerroDb
        End Get
    End Property
    'Propriedade que Permite indicar se a mensagem de erro deve ou não ser mostrada na console
    Public Property MostrarMensagemErroConsole() As Boolean
        Get
            Return bol_auxmostraMensagem
        End Get
        Set(ByVal bol_value As Boolean)
            bol_auxmostraMensagem = bol_value
        End Set
    End Property
    'Propriedade que Contem o SQL Exception
    Public ReadOnly Property RetornaSQLException() As SqlClient.SqlException
        Get
            Return obj_auxsqlException
        End Get
    End Property
    'Propriedade que Contem Parametros
    Public Property Parametros() As Object
        Get
            Return obj_auxParametros
        End Get
        Set(ByVal obj_value As Object)
            obj_auxParametros = obj_value
        End Set
    End Property

#End Region

#Region "Tratamento de Erro"
    'Função Interna para Tratamento de Erros
    Private Sub TrataErro(ByVal e As SqlException)
        Dim int_contador As Integer
        str_auxmensagemerroDb = ""
        obj_auxsqlException = e
        For int_contador = 0 To e.Errors.Count - 1
            Select Case e.Errors(int_contador).Number
                Case 17 'Falha de Conexao - Servidor Invalido
                    str_auxmensagemerroDb = "Não foi possível abrir o banco de dados por motivo de falha de login. O Servidor SQL Server especificado na string de conexão não existe, não está disponível, ou seu acesso a ele foi recusado. Verifique se os parâmetros da conexão estão corretos banco de dados."

                Case 4060, 18546 'Falha de Conexao - Servidor Senha Invalida
                    str_auxmensagemerroDb = "Não foi possível abrir o banco de dados por motivo de falha de login. Verifique se os parâmetros da conexão do banco de dados estão corretos."

                Case 18456 'Falha de Conexao - Usuario Inválido
                    str_auxmensagemerroDb = "Não foi possível abrir o banco de dados por motivo de falha de login. Verifique se os parâmetros da conexão do banco de dados estão corretos."

                Case 2627 'Falha de Inserção - Chave Duplicada
                    Dim str_aux As String
                    str_aux = e.Errors(0).Message
                    str_aux = Mid(str_aux, InStr(str_aux, "Cannot insert") + 40)
                    str_aux = Left(str_aux, Len(str_aux) - 2)
                    str_auxmensagemerroDb = "Não foi possível inserir registro na tabela '" & str_aux & "' do Banco de Dados por tentativa de inserção de Chave Duplicada."

                Case 547 'Violacao de constraint - Tentativa de Inserir Registro Filho sem pai correspondente ou violacao de constraint de campos
                    Dim str_aux As String
                    Dim str_tabela As String, str_banco As String, str_coluna As String, bol_insert As Boolean
                    str_aux = e.Errors(0).Message
                    Dim int_posicao As Integer
                    Dim bol_colunaCheck As Boolean
                    Dim bol_fk As Boolean
                    Dim bol_ColunaReferencia As Boolean
                    Dim bol_TabelaReferencia As Boolean

                    bol_fk = IIf(InStr(str_aux, "COLUMN FOREIGN KEY") > 0, True, False)
                    bol_insert = IIf(InStr(str_aux, "INSERT") > 0, True, False)
                    bol_colunaCheck = IIf(InStr(str_aux, "COLUMN CHECK") > 0, True, False)
                    bol_ColunaReferencia = IIf(InStr(str_aux, "COLUMN REFERENCE") > 0, True, False)
                    bol_TabelaReferencia = IIf(InStr(str_aux, "TABLE REFERENCE") > 0, True, False)

                    'Banco
                    int_posicao = InStr(str_aux, "in database")
                    str_aux = Mid(str_aux, int_posicao + 13)
                    int_posicao = InStr(str_aux, ",")
                    str_banco = Mid(str_aux, 1, int_posicao - 2)

                    'Tabela
                    int_posicao = InStr(str_aux, "table")
                    str_aux = Mid(str_aux, int_posicao + 7)
                    int_posicao = InStr(str_aux, ",")
                    If int_posicao = 0 Then
                        int_posicao = InStr(str_aux, ".")
                    End If
                    str_tabela = Mid(str_aux, 1, int_posicao - 2)

                    'Coluna
                    int_posicao = InStr(str_aux, "column")
                    If int_posicao <> 0 Then
                        str_aux = Mid(str_aux, int_posicao + 8)
                        int_posicao = InStr(str_aux, ".")
                        str_coluna = Mid(str_aux, 1, int_posicao - 2)
                    End If

                    If bol_insert And bol_fk Then
                        str_auxmensagemerroDb = "Não foi possível inserir registro na tabela do Banco de Dados '" & str_banco & "' por tentativa de inserção de registro 'filho' sem correspondente na tabela 'pai' " & str_tabela & ". Restrição referente a coluna " & str_coluna & "."
                    ElseIf Not bol_insert And bol_fk Then
                        str_auxmensagemerroDb = "Não foi possível excluir registro na tabela do Banco de Dados '" & str_banco & "' por tentativa de exclusão de registro 'pai' com correspondente na tabela 'filha' " & str_tabela & ". Restrição referente a coluna " & str_coluna & "."
                    End If
                    If bol_colunaCheck Then
                        str_auxmensagemerroDb = "Não foi possível inserir registro na tabela '" & str_tabela & "' do banco '" & str_banco & "' por tentativa de inserção de valores inválidos na coluna '" & str_coluna & "'. A coluna '" & str_coluna & "' não suporta o valor que a ela foi tentado atribuir, por regras definidas no banco de dados."
                    End If
                    If bol_ColunaReferencia Then
                        str_auxmensagemerroDb = "Não foi possível excluir registro por integridade com a tabela '" & str_tabela & "' do banco '" & str_banco & "' na coluna " & str_coluna & "."
                    End If
                    If bol_TabelaReferencia Then
                        str_auxmensagemerroDb = "Não foi possível excluir registro por integridade com a tabela '" & str_tabela & "' do banco '" & str_banco & "."
                    End If

                    If (bol_insert Or bol_colunaCheck Or bol_ColunaReferencia Or bol_fk Or bol_TabelaReferencia) = False Then
                        str_auxmensagemerroDb = "Erro 547 -Integridade Referencial- não tratado pela classe de Banco de Dados. A operação não será realizada."
                    End If
                Case 515 'Tratamento de inserção de valores nulos
                    Dim str_aux As String
                    Dim str_tabela As String, str_coluna As String
                    str_aux = e.Errors(0).Message
                    Dim int_posicao As Integer
                    Dim bol_nulo As Boolean

                    'Nulo
                    bol_nulo = IIf(InStr(str_aux, "column does not allow nulls") > 0, True, False)

                    'Coluna
                    int_posicao = InStr(str_aux, "column")
                    str_aux = Mid(str_aux, int_posicao + 8)
                    int_posicao = InStr(str_aux, ",")
                    str_coluna = Mid(str_aux, 1, int_posicao - 2)

                    'Tabela
                    int_posicao = InStr(str_aux, "table")
                    str_aux = Mid(str_aux, int_posicao + 7)
                    int_posicao = InStr(str_aux, ";")
                    str_tabela = Mid(str_aux, 1, int_posicao - 2)

                    If bol_nulo Then
                        str_auxmensagemerroDb = "Não foi possível inserir registro na tabela '" & str_tabela & "' por tentativa de inserção de valores nulos na coluna '" & str_coluna & "'. Esta coluna não suporta valores nulos."
                    End If

                Case 170 'incorrect syntax near ,
                    str_auxmensagemerroDb = "Sintaxe incorreta do Comando SQL próximo ao comando ','. Verifique a sintaxe do comando submetido ao servidor."

                Case 207 ' Coluna invalida
                    str_auxmensagemerroDb = "Coluna inválida no Comando SQL. Verifique a sintaxe da query submetida ao servidor."

                Case 208 ' Tabela invalida
                    str_auxmensagemerroDb = "Objeto (tabela) inválida no comando SQL. Verifique a sintaxe da query submetida ao servidor."
                Case Else
                    If str_auxmensagemerroDb = "" Then
                        str_auxmensagemerroDb = "Mensagem de Erro Tratada Genericamente." & ControlChars.NewLine
                        str_auxmensagemerroDb += "Indice #" & int_contador.ToString() & ControlChars.NewLine _
                                       & "Mensagem: " & e.Errors(int_contador).Message & ControlChars.NewLine _
                                       & "Código: " & e.Errors(int_contador).Number & ControlChars.NewLine _
                                       & "Número da Linha: " & e.Errors(int_contador).LineNumber.ToString & ControlChars.NewLine _
                                       & "Origem: " & e.Errors(int_contador).Source & ControlChars.NewLine _
                                       & "Procedimento: " & e.Errors(int_contador).Procedure.ToString & ControlChars.NewLine _
                                       & "Classe: " & e.Errors(int_contador).Class.ToString() & ControlChars.NewLine _
                                       & "Servidor: " & e.Errors(int_contador).Server.ToString & ControlChars.NewLine
                    End If
            End Select
        Next int_contador

        If bol_auxmostraMensagem = True Then
            Console.WriteLine("Ocorreu um erro na Tentativa de Executar uma operação com o Banco de dados. Comunique com o Administrador do Sistema, caso não seja possível corrigir o problema. Mensagem de Erro:" & str_auxmensagemerroDb)
        End If
    End Sub
#End Region

#Region "Tratamento Conexão"
    'Funcao que le parametros de conexão com banco de Dados e constroi string de conexao
    Private Function RetornaStringConexao() As String

        Dim strUsuarioSQL As String = ConfigurationSettings.AppSettings.Get("SQLUsuario").Trim
        Dim strServidorSQL As String = ConfigurationSettings.AppSettings.Get("SQLServidor").Trim
        Dim strBancoSQL As String = ConfigurationSettings.AppSettings.Get("SQLBanco").Trim
        Dim strSenhaSQL As String = ConfigurationSettings.AppSettings.Get("SQLSenha").Trim

        'Concatena variável formando string de conexao
        str_auxConexao = "Persist Security Info=False;" & _
                    "Initial Catalog=" & strBancoSQL & _
                    ";Data Source=" & strServidorSQL & _
                    ";pwd=" & strSenhaSQL & _
                    ";User ID=" & strUsuarioSQL
        Return str_auxConexao
    End Function
    'Função que AbreConexao retornando objeto de Conexao como parametro e variável booleana indicando sucesso da operacao
    Public Function AbreConexao(Optional ByVal str_conexao As String = "", Optional ByRef obj_conexao As SqlClient.SqlConnection = Nothing, Optional ByVal str_mensagemerroDb As String = "") As Boolean

        Try
            obj_auxConexao.ConnectionString = RetornaStringConexao()
            obj_auxConexao.Open()

        Catch e As SqlException
            TrataErro(e)
            str_mensagemerroDb = str_auxmensagemerroDb
            Return False

        End Try

        Return True

    End Function
    'Função que Fecha Conexao   
    Public Function FechaConexao() As Boolean

        Try
            obj_auxConexao.Close()
            obj_auxConexao.Dispose()
        Catch e As SqlException
            TrataErro(e)
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "Tratamento de Parametros"
    'Função para Atachar Parâmetros
    ' Este método é utilizado para atachar array de SqlParameters para um SqlCommand.
    ' Este método vai Atribuir um valor de DbNull para qualquer parâmetro com direção de entrada e saída.
    ' Esta chamada vai previnir valores default de serem utilizados, mas
    ' vai ser um caso menos comum  que o pretendido para parâmetros de saída (derivados como InputOutput)
    ' onde o usuário não provê valor de input 
    ' Parâmetros:
    ' -command - O command que vai receber os parametros 
    ' -commandParameters - Um array de SqlParameters que vai ser adicionados ao command
    Private Shared Sub AtacharParametros(ByVal cmd_command As SqlCommand, ByVal par_commandParameters() As SqlParameter)
        Dim par_parametros As SqlParameter
        For Each par_parametros In par_commandParameters
            'check for derived output value with no value assigned
            If par_parametros.Direction = ParameterDirection.InputOutput And par_parametros.Value Is Nothing Then
                par_parametros.Value = Nothing
            End If
            cmd_command.Parameters.Add(par_parametros)
        Next par_parametros
    End Sub

    ' Este método associa um array de valores para um array de SqlParameters
    ' Parameters:
    ' - commandParameters - array de SqlParameters para serem associados valores 
    ' - Array de objetos guardando os valores que serão associados 
    Private Shared Sub AssociarValoresParametros(ByVal par_commandParameters() As SqlParameter, ByVal obj_parameterValues() As Object)

        Dim sht_i As Short
        Dim sht_j As Short

        If (par_commandParameters Is Nothing) And (obj_parameterValues Is Nothing) Then
            'Nada para fazer.
            Return
        End If

        'Se os parametros forem passados por um array sqlparameter
        If (par_commandParameters.Length = obj_parameterValues.Length) Then
            'array de Valores
            sht_j = par_commandParameters.Length - 1
            For sht_i = 0 To sht_j
                par_commandParameters(sht_i).Value = obj_parameterValues(sht_i)
            Next
            'Se os parâmetros forem passados por command
        ElseIf (obj_parameterValues.Length = 1) Then
            If (obj_parameterValues(0).count = par_commandParameters.Length) Then

                'array de Valores
                sht_j = par_commandParameters.Length - 1
                For sht_i = 0 To sht_j
                    par_commandParameters(sht_i).Value = obj_parameterValues(0)(sht_i).value
                Next
            Else
                Throw New ArgumentException("Quantidade de Parametros do command não confere com a quantidade de parâmetros de valores.")
            End If
        Else
            Throw New ArgumentException("Quantidade de Parametros do command não confere com a quantidade de parâmetros de valores.")
        End If
    End Sub
#End Region

#Region "Tratamento de Command"
    ' Este método associa uma conexão , transação , tipo comando e parâmetros
    ' para ser disponibilizado para o command.
    ' Parameters:
    ' -command - the SqlCommand to be prepared
    ' -connection - a valid SqlConnection, on which to execute this command
    ' -transaction - a valid SqlTransaction, or 'null'
    ' -commandType - the CommandType (stored procedure, text, etc.)
    ' -commandText - the stored procedure name or T-SQL command
    ' -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required
    Private Sub PrepareCommand(ByVal cmd_command As SqlCommand, _
                               ByVal con_conexao As SqlConnection, _
                               ByVal trs_transacao As SqlTransaction, _
                               ByVal cmt_commandType As CommandType, _
                               ByVal str_commandText As String, _
                               ByVal par_commandParameters() As SqlParameter)

        If con_conexao.State <> ConnectionState.Open Then
            If obj_auxConexao.State <> ConnectionState.Open Then
                Throw New ArgumentException("Comando não pode ser processado porque a conexão está fechada.")
            End If
            cmd_command.Connection = obj_auxConexao
        Else
            cmd_command.Connection = Conexao
        End If

        If str_commandText <> "" Then
            str_auxSql = str_commandText
        End If

        If str_auxSql = "" Then
            str_auxmensagemerroDb = "Commando SQL não foi fornecido."
            Exit Sub
        End If

        cmd_command.CommandText = str_auxSql

        'Se houver transação associe o commando a transação
        If Not (trs_transacao Is Nothing) Then
            cmd_command.Transaction = trs_transacao
        End If

        'Setar o tipo de comando
        cmd_command.CommandType = cmt_commandType

        'attachar os parâmetros do commando se forem fornecidos
        If Not (par_commandParameters Is Nothing) Then
            AtacharParametros(cmd_command, par_commandParameters)
        End If
        Return
    End Sub

#End Region

#Region "ExecuteNonQuery"


    ' Executa um SQLCommand (que retorna nenhum resultset) 
    ' e.g.:  
    ' Dim result as Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -conexao - uma conexao valida 
    ' -commandType - Tipo de command (stored procedure, text, etc.)
    ' -commandText - O nome da storedprocedure ou comando T-SQL 
    ' -commandParameters - um array de SqlParamters utilizado para executar o comando
    ' Retorno: um int representando o numero de linhas afetadas pelo command 
    Public Overloads Function ExecuteNonQuery(ByVal con_conexao As SqlClient.SqlConnection, _
                                              ByVal cmt_tipoComando As CommandType, _
                                              ByVal str_textoComando As String, _
                                              ByVal ParamArray par_commandParameters() As SqlParameter) As Integer

        If con_conexao.State = ConnectionState.Open Then
            obj_auxConexao = con_conexao
        Else
            If obj_auxConexao.State <> ConnectionState.Open Then
                str_auxmensagemerroDb = "Conexão não está aberta."
                Return 0
            End If
        End If
        If str_textoComando = "" Then
            If str_auxSql <> "" Then
                str_textoComando = str_auxSql
            Else
                str_auxmensagemerroDb = "Comando SQL não foi fornecido."
                Return 0
            End If
        End If

        Try
            Return ExecuteNonQuery(obj_auxConexao, cmt_tipoComando, str_textoComando, par_commandParameters)
        Finally
        End Try
    End Function 'ExecuteNonQuery

    ' Executa uma stored procedure via um SQLCommand (que não retorna resultset) 
    ' Este método vai descobrir os parâmetros para a stored procedure 
    ' , e associa os valores baseados na ordem dos parâmetros. 

    ' Este método não prove acesso para para obter valores dos parametros de saida ou para retornar valores da stored procedure 
    ' e.g.:  
    '  Dim result as Integer = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
    ' Parameters:
    ' -conexao - uma conexao valida
    ' -spName - o nome da stored procedure
    ' -parameterValues - um array de objetos para serem associados como os valores de entrada da stored procedure 
    ' Returns: um inteiro representando o número de registros afetados pelo command 
    Public Overloads Function ExecuteNonQuery(ByVal con_conexao As SqlClient.SqlConnection, _
                                              ByVal str_spName As String, _
                                              ByVal ParamArray obj_parameterValues() As Object) As Integer
        Dim par_commandParameters As SqlParameter()

        If Conexao.State = ConnectionState.Open Then
            obj_auxConexao = Conexao
        Else
            If obj_auxConexao.State <> ConnectionState.Open Then
                str_auxmensagemerroDb = "Conexão não está aberta."
                Return 0
            End If
        End If

        If str_spName = "" Then
            If str_auxSql <> "" Then
                str_spName = str_auxSql
            Else
                str_auxmensagemerroDb = "Comando SQL não foi fornecido."
                Return 0
            End If
        End If

        'Se forem recebidos valores de parâmetros, nós precisaríamos confirmá-los para chamada da stored procedure
        If Not (obj_parameterValues Is Nothing) And obj_parameterValues.Length > 0 Then
            'associar os parâmetros para a stored procedure do cache (ou descobri-los e popular o cache)
            par_commandParameters = SqlHelperParameterCache.ObterConjuntoParametrosSP(Conexao, str_spName)

            'Associar os valores fornecidos para os parametros baseados na ordem dos parametros parametro 
            AssociarValoresParametros(par_commandParameters, obj_parameterValues)

            'Chamar a funcao overload que pecebe um array de SqlParameters
            Return ExecuteNonQuery(Conexao, CommandType.StoredProcedure, str_spName, par_commandParameters)

            'Caso contrario invoca stored procedures sem parametros
        Else
            Return ExecuteNonQuery(Conexao, CommandType.StoredProcedure, str_spName)
        End If
    End Function 'ExecuteNonQuery

    ' Executa a SqlCommand (que nao retorna resultset e nao tem parametros) contra a conexao fornecida 
    ' e.g.:  
    ' Dim result as Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
    ' Parameters:
    ' -connection - uma conexao valida
    ' -commandType - o tipo de command (stored procedure, text, etc.)
    ' -commandText - o nome da stored procedure ou comando T-SQL 
    ' Retorno: um int representando o numero de linhas afetadas pelo command 
    Public Overloads Function ExecuteNonQuery(ByVal con_connection As SqlClient.SqlConnection, _
                                              ByVal cmt_commandType As CommandType, _
                                              ByVal str_commandText As String) As Integer

        If con_connection.State = ConnectionState.Open Then
            obj_auxConexao = Conexao
        Else
            If obj_auxConexao.State <> ConnectionState.Open Then
                str_auxmensagemerroDb = "Conexão não está aberta."
                Return 0
            End If
        End If

        If str_commandText = "" Then
            If str_auxSql <> "" Then
                str_commandText = str_auxSql
            Else
                str_auxmensagemerroDb = "Comando SQL não foi fornecido."
                Return 0
            End If
        End If

        'chama a funcao fornecendo nul para o conjunto de SqlParameters
        Return ExecuteNonQuery(con_connection, cmt_commandType, str_commandText, CType(Nothing, SqlParameter()))

    End Function 'ExecuteNonQuery

    ' Executa um SqlCommand (que nao retorna nenhum resultset e nao recebe parametros) contra uma SqlTransaction fornecida.
    ' e.g.:  
    '  Dim result as Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
    ' Parametros:
    ' -transaction - uma SqlTransaction valida associada com a conexão
    ' -commandType - o CommandType (stored procedure, text, etc.) 
    ' -commandText - o nome stored procedure ou comando T-SQL 
    ' Retorno:  um int representando o numero de linhas afetadas pelo comand 
    Public Overloads Function ExecuteNonQuery(ByVal trs_transaction As SqlTransaction, _
                                              ByVal cmt_commandType As CommandType, _
                                              ByVal str_commandText As String) As Integer

        'passa a chamada provendo null para o conjunto de sqlparameters

        Return ExecuteNonQuery(trs_transaction, cmt_commandType, str_commandText, CType(Nothing, SqlParameter()))
    End Function 'ExecuteNonQuery

    ' Executa um SqlCommand (que não retorna resultset) contra uma SqlTransaction especificada
    ' de acordo com os parâmetros recebidos
    ' e.g.:  
    ' Dim result as Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parametros:
    ' -transaction - uma transação válida
    ' -commandType - um CommandType (stored procedure, text, etc.) 
    ' -commandText - um nome de stored procedure ou comando T-SQL 
    ' -commandParameters - um array de SqlParamters utilizado para executar um command 
    ' Retorno: um int representando o numero de linhas afetadas pelo command
    Public Overloads Function ExecuteNonQuery(ByVal str_transaction As SqlTransaction, _
                                              ByVal cmt_commandType As CommandType, _
                                              ByVal str_commandText As String, _
                                              ByVal ParamArray par_commandParameters() As SqlParameter) As Integer
        'cria um command e o prepara para sua execução 
        Dim cmd_command As New SqlCommand
        Dim int_retVal As Integer

        If str_transaction.Connection.State = ConnectionState.Open Then
            obj_auxConexao = Conexao
        Else
            If obj_auxConexao.State <> ConnectionState.Open Then
                str_auxmensagemerroDb = "Conexão não está aberta."
                Return 0
            End If
        End If

        If str_commandText = "" Then
            If str_auxSql <> "" Then
                str_commandText = str_auxSql
            Else
                str_auxmensagemerroDb = "Comando SQL não foi fornecido."
                Return 0
            End If
        End If

        PrepareCommand(cmd_command, str_transaction.Connection, str_transaction, cmt_commandType, str_commandText, par_commandParameters)

        'executa o commando
        int_retVal = cmd_command.ExecuteNonQuery()

        'limpa os parâmetros do command
        cmd_command.Parameters.Clear()

        Return int_retVal

    End Function 'ExecuteNonQuery

    ' Executa uma stored procedure através de um SqlCommand (que não retorna resultset) contra uma SqlTransaction especificada
    ' utilizando os valores de parâmetros fornecidos.  Este método vai descobrir os parâmetros para a stored procedure
    ' e associa os valores baseados na ordem dos parâmetros.
    ' Este método não provê acesso a parâmetros de saida ou valores de retornos de stored procedures.
    ' e.g.:  
    ' Dim result As Integer = SqlHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
    ' Parametros:
    ' -transaction - um SqlTransaction valido
    ' -spName - o nome da stored procedure
    ' -parameterValues - um array de objetos para serem assoriados aos valores de entrada da procedure
    ' Retorno: um int representando o número de linhas afetadas pelo command 
    Public Overloads Function ExecuteNonQuery(ByVal str_transaction As SqlTransaction, _
                                              ByVal str_spName As String, _
                                              ByVal ParamArray obj_parameterValues() As Object) As Integer
        Dim par_commandParameters As SqlParameter()

        If str_spName = "" Then
            If str_auxSql <> "" Then
                str_spName = str_auxSql
            Else
                str_auxmensagemerroDb = "Comando SQL não foi fornecido."
                Return 0
            End If
        End If


        If Not (obj_parameterValues Is Nothing) And obj_parameterValues.Length > 0 Then
            'Colocar os parametros para a stored procedure do cache de parâmetros (ou descobri-los e populá-los do cache)
            par_commandParameters = SqlHelperParameterCache.ObterConjuntoParametrosSP(Conexao, str_spName)

            'Associar os valores para os parametros baseados na ordem dos parametros 
            AssociarValoresParametros(par_commandParameters, obj_parameterValues)

            'Chama overload que recebe um array de SqlParameters
            Return ExecuteNonQuery(str_transaction, CommandType.StoredProcedure, str_spName, par_commandParameters)
            'Caso contrario, chamar sp sem parametros
        Else
            'Chama sp sem parametros
            Return ExecuteNonQuery(str_transaction, CommandType.StoredProcedure, str_spName)
        End If
    End Function 'ExecuteNonQuery
#End Region

#Region "ExecuteReader"

    'Função para Retornar DataReader 

    Public Overloads Function ExecuteReader(Optional ByVal str_sql As String = "", Optional ByRef dre_DataReader As SqlClient.SqlDataReader = Nothing, Optional ByVal trs_transaction As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim cmd_commObj As New SqlClient.SqlCommand

        If str_sql = "" Then
            str_sql = str_auxSql
        Else
            str_auxSql = str_sql
        End If
        If str_auxSql = "" Then
            str_auxmensagemerroDb = "Commando SQL não foi fornecido."
            Return False
        End If
        If Not IsNothing(obj_auxConexao) Then
            Try
                cmd_commObj.Connection = obj_auxConexao
                cmd_commObj.CommandText = str_sql
                If Not IsNothing(trs_transaction) Then
                    cmd_commObj.Transaction = trs_transaction
                End If
                dre_DataReader = cmd_commObj.ExecuteReader()
                cmd_commObj.Dispose()

            Catch e As SqlException
                TrataErro(e)
                Return False

            End Try
        End If
        Return True
    End Function
#End Region

#Region "ExecuteScalar"
    'Função para Retornar DataReader Scalar
    Public Function ExecuteScalar(Optional ByVal str_sql As String = "", Optional ByRef str_valor As String = Nothing) As Boolean
        Dim cmd_commObj As New SqlClient.SqlCommand

        If str_sql = "" Then
            str_sql = str_auxSql
        Else
            str_auxSql = str_sql
        End If
        If str_auxSql = "" Then
            str_auxmensagemerroDb = "Commando SQL não foi fornecido."
            Return False
        End If
        If Not IsNothing(obj_auxConexao) Then
            Try
                cmd_commObj.Connection = obj_auxConexao
                cmd_commObj.CommandText = SQL
                str_valor = CStr(cmd_commObj.ExecuteScalar())

            Catch e As SqlException
                TrataErro(e)
                Return False
            End Try

        End If
        Return True
    End Function
#End Region

#Region "ExecuteDataSet"
    'Função para Retornar DataSet
    Public Function ExecuteDataSet(ByRef dst_DS As DataSet, ByVal str_dsName As String, Optional ByVal str_sql As String = "") As Boolean

        If str_sql = "" Then
            str_sql = str_auxSql
        Else
            str_auxSql = str_sql
        End If
        If str_auxSql = "" Then
            str_auxmensagemerroDb = "Commando SQL não foi fornecido."
            Return False
        End If
        Dim adt_adaptSQL As SqlDataAdapter = New SqlDataAdapter(str_auxSql, obj_auxConexao)
        dst_DS = New DataSet
        If Not IsNothing(obj_auxConexao) Then
            Try
                adt_adaptSQL.Fill(dst_DS, str_dsName)

            Catch e As SqlException
                TrataErro(e)
                Return False

            End Try
        End If
        Return True
    End Function
#End Region

#Region "ExecutaSQL"
    'Função para Executar Comando SQL
    Public Function ExecutaSQL(Optional ByVal str_sql As String = "", Optional ByVal trs_transaction As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim cmd_commObj As New SqlClient.SqlCommand

        If str_sql = "" Then
            str_sql = str_auxSql
        Else
            str_auxSql = str_sql
        End If
        If str_auxSql = "" Then
            str_auxmensagemerroDb = "Commando SQL não foi fornecido."
            Return False
        End If

        If Not IsNothing(obj_auxConexao) Then
            Try
                cmd_commObj.Connection = obj_auxConexao
                cmd_commObj.CommandText = SQL
                If Not IsNothing(trs_transaction) Then
                    cmd_commObj.Transaction = trs_transaction
                End If
                cmd_commObj.ExecuteNonQuery()

            Catch e As SqlException
                TrataErro(e)
                Return False
            End Try
        End If
        Return True
    End Function

#End Region

#Region "ExecuteXmlReader"

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' Returns: an XmlReader containing the resultset generated by the command 
    Public Overloads Function ExecuteXmlReader(ByVal con_connection As SqlConnection, _
                                               ByVal cmt_commandType As CommandType, _
                                               ByVal str_commandText As String) As Xml.XmlReader
        'pass through the call providing null for the set of SqlParameters
        Return ExecuteXmlReader(con_connection, cmt_commandType, str_commandText, CType(Nothing, SqlParameter()))
    End Function 'ExecuteXmlReader

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: an XmlReader containing the resultset generated by the command 
    Public Overloads Function ExecuteXmlReader(ByVal con_connection As SqlConnection, _
                                               ByVal cmt_commandType As CommandType, _
                                               ByVal str_commandText As String, _
                                               ByVal ParamArray par_commandParameters() As SqlParameter) As Xml.XmlReader
        'pass through the call using a null transaction value
        'Return ExecuteXmlReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)
        'create a command and prepare it for execution
        Dim cmd_command As New SqlCommand
        Dim xmr_retval As Xml.XmlReader

        PrepareCommand(cmd_command, con_connection, CType(Nothing, SqlTransaction), cmt_commandType, str_commandText, par_commandParameters)

        'create the DataAdapter & DataSet

        Try
            xmr_retval = cmd_command.ExecuteXmlReader()

        Catch e As SqlException
            TrataErro(e)
            Return Nothing
        End Try


        'detach the SqlParameters from the command object, so they can be used again
        cmd_command.Parameters.Clear()

        Return xmr_retval
    End Function 'ExecuteXmlReader

    ' Executa uma stored procedure através de um SqlCommand (que retorna um resultset) contra uma SqlConnection 
    ' usando os parametros fornecidos. Este método vai descobrir os parameters da stored procedure, e associar os valores baseados na ordem dos parametros.
    ' Este método não provê accesso para parâmetros saída ou valroes de retorno das stored procedures.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, "GetOrders", 24, 36)
    ' Parametros:
    ' -connection - uma conexao valida
    ' -spName - o nome da stored proceudre utilizando "FOR XML AUTO" 
    ' -parameterValues - um array de objetos para serem associados como valores de entrada nas stored procedures
    ' Retorno: um XmlReader contendo o resultset gerado pelo command
    Public Overloads Function ExecuteXmlReader(ByVal con_connection As SqlConnection, _
                                               ByVal str_spName As String, _
                                               ByVal ParamArray obj_parameterValues() As Object) As Xml.XmlReader
        Dim par_commandParameters As SqlParameter()

        If Not (obj_parameterValues Is Nothing) And obj_parameterValues.Length > 0 Then
            par_commandParameters = SqlHelperParameterCache.ObterConjuntoParametrosSP(con_connection, str_spName)

            'Associar os valores fornecidos para os parametros baseados na ordem dos parametros
            AssociarValoresParametros(par_commandParameters, obj_parameterValues)

            'chamar função overload que recebe um array de SqlParameters
            Return ExecuteXmlReader(con_connection, CommandType.StoredProcedure, str_spName, par_commandParameters)
            'caso contrário chama uma stored procedures sem parametros
        Else
            Return ExecuteXmlReader(con_connection, CommandType.StoredProcedure, str_spName)
        End If
    End Function 'ExecuteXmlReader


    ' Executa a SqlCommand (que retorna um resultset e nao recebe parametros) contra uma qlTransaction fornecida
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders")
    ' Parametros:
    ' -transaction - uma SqlTransaction valida
    ' -commandType - o tipo de command (stored procedure, text, etc.) 
    ' -commandText - o nome da stored procedure name ou command T-SQL usando "FOR XML AUTO" 
    ' Retorno: um XmlReader contendo o resultset gerado pelo command
    Public Overloads Function ExecuteXmlReader(ByVal trs_transaction As SqlTransaction, _
                                               ByVal cmt_commandType As CommandType, _
                                               ByVal str_commandText As String) As Xml.XmlReader
        Return ExecuteXmlReader(trs_transaction, cmt_commandType, str_commandText, CType(Nothing, SqlParameter()))
    End Function 'ExecuteXmlReader

    ' Executa um SqlCommand (que retorna um resultset) contra uma SqlTransaction especificada
    ' usando os parametros fornecidos.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parametros:
    ' -transaction - uma SqlTransaction valida
    ' -commandType - o tipo de commando (stored procedure, text, etc.) 
    ' -commandText - o nome da stored procedure ou comando T-SQL usando "FOR XML AUTO" 
    ' -commandParameters - um array de SqlParamters usado para executar o commando
    ' Returns: an XmlReader containing the resultset generated by the command
    Public Overloads Function ExecuteXmlReader(ByVal trs_transaction As SqlTransaction, _
                                               ByVal cmt_commandType As CommandType, _
                                               ByVal str_commandText As String, _
                                               ByVal ParamArray par_commandParameters() As SqlParameter) As Xml.XmlReader

        Dim cmd_command As New SqlCommand
        Dim xmr_retval As Xml.XmlReader

        PrepareCommand(cmd_command, trs_transaction.Connection, trs_transaction, cmt_commandType, str_commandText, par_commandParameters)

        Try
            xmr_retval = cmd_command.ExecuteXmlReader()

        Catch e As SqlException
            TrataErro(e)
            Return Nothing
        End Try

        cmd_command.Parameters.Clear()

        Return xmr_retval

    End Function 'ExecuteXmlReader

    ' Execute uma stored procedure através de um SqlCommand (que retorna um resultset) contra uma SqlTransaction especificada
    ' usando os parametros fornecidos.  Este método vai descobrir os parametros da stored procedure
    ' e associar os valores baseados na ordem dos parametros.
    ' Este métood não provê acesso aos parametros de saida ou valores de retorno da stored procedure
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, "GetOrders", 24, 36)
    ' Parametross:
    ' -transaction - uma SqlTransaction valida
    ' -spName - o nome da stored procedure 
    ' -parameterValues - um array de objetos para serem associados como values de entrada das stored procedure
    ' Retorno: um dataset contendo o resultset gerado pelo command 
    Public Overloads Function ExecuteXmlReader(ByVal trs_transaction As SqlTransaction, _
                                               ByVal str_spName As String, _
                                               ByVal ParamArray obj_parameterValues() As Object) As Xml.XmlReader

        Dim par_commandParameters As SqlParameter()

        If Not (obj_parameterValues Is Nothing) And obj_parameterValues.Length > 0 Then
            par_commandParameters = SqlHelperParameterCache.ObterConjuntoParametrosSP(trs_transaction.Connection, str_spName)

            AssociarValoresParametros(par_commandParameters, obj_parameterValues)

            Return ExecuteXmlReader(trs_transaction, CommandType.StoredProcedure, str_spName, par_commandParameters)
        Else
            Return ExecuteXmlReader(trs_transaction, CommandType.StoredProcedure, str_spName)
        End If
    End Function 'ExecuteXmlReader

#End Region

End Class

' SqlHelperParameterCache provê funcoes para criar um cache estatico de parametros de procedures, e a
' abilidade de descobrir parametros para as stored procedures em run time
Public NotInheritable Class SqlHelperParameterCache

#Region "metodos privados, variaveis, and construtores"
    'Esta classe  provê somente metodos staticos, fazendo os construtores default private para previnir instancias
    'de serem criadas com new "sqlhelperparametercache
    Private Sub New()
    End Sub 'New 

    Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable)

    ' resolve em tempo de execucao o conjunto apropriado de parametros para uma 
    ' Parameters:
    ' - conexao - uma conexao valida
    ' - spName - o nome da stored procedure
    ' - includeReturnValueParameter -quando ou nao o valor de retorno vai ser incluido como parametro de retorno
    ' Returns: SqlParameter()
    Private Shared Function DescobrirConjuntoParametrosSP(ByVal con_conexao As SqlClient.SqlConnection, _
                                                          ByVal str_spName As String, _
                                                          ByVal bol_includereturnvalueParameter As Boolean, _
                                                          ByVal ParamArray obj_parameterValues() As Object) As SqlParameter()


        Dim cmd_command As SqlCommand = New SqlCommand(str_spName, con_conexao)
        Dim par_discoveredParameters() As SqlParameter

        Try
            cmd_command.CommandType = CommandType.StoredProcedure
            SqlCommandBuilder.DeriveParameters(cmd_command)
            If Not bol_includereturnvalueParameter Then
                cmd_command.Parameters.RemoveAt(0)
            End If

            par_discoveredParameters = New SqlParameter(cmd_command.Parameters.Count - 1) {}
            cmd_command.Parameters.CopyTo(par_discoveredParameters, 0)
        Finally
            cmd_command.Dispose()
            con_conexao.Dispose()
        End Try

        Return par_discoveredParameters

    End Function 'DiscoverSpParameterSet

    'Manter copia do array de parametros cacheados
    Private Shared Function ClonarParametros(ByVal par_originalParameters() As SqlParameter) As SqlParameter()

        Dim sht_i As Short
        Dim sht_j As Short = par_originalParameters.Length - 1
        Dim par_clonedParameters(sht_j) As SqlParameter

        For sht_i = 0 To sht_j
            par_clonedParameters(sht_i) = CType(CType(par_originalParameters(sht_i), ICloneable).Clone, SqlParameter)
        Next

        Return par_clonedParameters
    End Function 'CloneParameters
#End Region

#Region "Funcoes de Cache"

    ' adiciona um parâmetro para o cache 
    '    
    ' Parametros
    ' -Conexao - uma conexao valida
    ' -commandText - o nome de uma stored procedure ou comando T-SQL 
    ' -commandParameters - um array de SqlParamters para ser armazenado no cached 
    Public Shared Sub CacheConjuntoParametros(ByVal con_conexao As SqlClient.SqlConnection, _
                                        ByVal str_commandText As String, _
                                        ByVal ParamArray par_commandParameters() As SqlParameter)
        Dim hashKey As String = con_conexao.ConnectionString + ":" + str_commandText

        paramCache(hashKey) = par_commandParameters
    End Sub 'CacheParameterSet

    ' Obtem um parametro do cache
    ' Parameters:
    ' -connectionString - uma conexao valida 
    ' -commandText - uma procedure name ou comando T-SQL 
    ' Returns: um array de SqlParamters 
    Public Shared Function GetCachedParameterSet(ByVal con_conexao As SqlClient.SqlConnection, ByVal commandText As String) As SqlParameter()
        Dim str_hashKey As String = con_conexao.ConnectionString + ":" + commandText
        Dim par_cachedParameters As SqlParameter() = CType(paramCache(str_hashKey), SqlParameter())

        If par_cachedParameters Is Nothing Then
            Return Nothing
        Else
            Return ClonarParametros(par_cachedParameters)
        End If
    End Function 'GetCachedParameterSet

#End Region

#Region "Funções para Descobrir Parâmetros "
    ' Obtem um conjunto de SqlParameter apropriado para a stored procedure 
    ' Este método vai buscar no banco de dados esta informação, e então armazena-lo em um cache para futuros requests
    ' 
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -spName - the name of the stored procedure 
    ' Retorno: um Array de SqlParameters
    Public Overloads Shared Function ObterConjuntoParametrosSP(ByVal con_conexao As SqlClient.SqlConnection, ByVal str_spName As String) As SqlParameter()
        Return ObterConjuntoParametrosSP(con_conexao, str_spName, False)
    End Function 'GetSpParameterSet 

    ' Obtem um conjunto de SqlParameter apropriado para a stored procedrue
    ' 
    ' Este método vai buscar no banco de dados as informações de parametros para esta informação, e armazená-la em um cache para futuras chamadas
    ' Parameters:
    ' -Conexao - uma conexao valida
    ' -spName - o nome da stored procedure 
    ' -includeReturnValueParameter - um valor booleano indicando quando o valor de retorno parameter poderia ser incluso nos retornos 
    ' Returns: um array de sqlParameters
    Public Overloads Shared Function ObterConjuntoParametrosSP(ByVal con_conexao As SqlClient.SqlConnection, _
                                                       ByVal str_spName As String, _
                                                       ByVal bol_includereturnvalueparameter As Boolean) As SqlParameter()

        Dim par_cachedParameters() As SqlParameter
        Dim str_hashKey As String

        str_hashKey = con_conexao.ConnectionString + ":" + str_spName + IIf(bol_includereturnvalueparameter = True, ":include ReturnValue Parameter", "")

        par_cachedParameters = CType(paramCache(str_hashKey), SqlParameter())

        If (par_cachedParameters Is Nothing) Then
            paramCache(str_hashKey) = DescobrirConjuntoParametrosSP(con_conexao, str_spName, bol_includereturnvalueparameter)
            par_cachedParameters = CType(paramCache(str_hashKey), SqlParameter())

        End If

        Return ClonarParametros(par_cachedParameters)

    End Function 'GetSpParameterSet
#End Region

End Class 'SqlHelperParameterCache 
