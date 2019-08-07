Imports System.data
Public Class clsDataUtil
    Private NomeTabela As String
    Private ArrayColunas, ArrayLinhas As Array
    Public Property PropNomeTabela() As String
        Get
            Return NomeTabela
        End Get
        Set(ByVal Value As String)
            NomeTabela = Value
        End Set
    End Property
    Public Property PropArrayColunas() As Array
        Get
            Return ArrayColunas
        End Get
        Set(ByVal Value As Array)
            ArrayColunas = Value
        End Set
    End Property
    Public Property PropArrayLinhas() As Array
        Get
            Return ArrayLinhas
        End Get
        Set(ByVal Value As Array)
            ArrayLinhas = Value
        End Set
    End Property
    Private Function VerificaArrays() As Boolean

    End Function
    Public Function CriaNovaTabela(ByRef strErro, _
                                    ByRef objDataTable) As Boolean
        Try
            ' Cria Tabela com o nome da Propriedade PropNomeTabela
            Dim objDataTableAux As DataTable = New DataTable(PropNomeTabela)

            ' Cria colunas conforme Array
            Dim int_Contador As Integer
            For int_Contador = 0 To ArrayColunas.GetLength(0)
                Dim objColuna As DataColumn = New DataColumn
                objColuna.DataType = GetType(System.String)
                objColuna.ColumnName = ArrayColunas(int_Contador)
                objDataTable.Columns.Add(objColuna)
            Next

            ' Cria Linhas conforme Array
            Dim objLinha As DataRow
            objLinha = objDataTable.NewRow()

            ' Adiciona Linhas nas colunas
            For int_Contador = 0 To ArrayColunas.GetLength(0)
                objLinha(ArrayColunas(int_Contador)) = ArrayLinhas(int_Contador)
                objDataTable.Rows.Add(objLinha)
            Next

            'Retorna DataTable
            objDataTable = objDataTableAux

        Catch objErro As System.Exception
            strErro = objErro.Message.Trim()

        End Try

        CriaNovaTabela = (strErro = "")

    End Function

End Class
