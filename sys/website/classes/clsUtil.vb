Imports System.Text.RegularExpressions
Public Class clsUtil
    'Objetivo: Formatar uma data no formato Brasileiro.
    'Uso: formataData(data)
    Function formataData(ByVal data)
        Dim dataAux As String
        If data = "" Or Not IsDate(data) Then Exit Function
        dataAux = Day(data) & "/" & Month(data) & "/" & Year(data) & " " & completaZero(Hour(data)) & ":" & completaZero(Minute(data))
        formataData = dataAux
    End Function
    'Objetivo: Completar com zero uma string que veio de um Int e não tem zero a esquerda
    'Uso: completaZero(numero)
    Function completaZero(ByVal numero)
        If Not IsNumeric(numero) Then Exit Function
        If Len(numero) < 2 Then
            completaZero = "0" & numero
        Else
            completaZero = numero
        End If
    End Function
    'Objetivo: Validar se uma string tem caracteres válidos (A até Z e 0 até 1)
    '          se a string tem um caracter diferente, retorna False.
    'Uso: chrValidos("string")
    Public Function chrValidos(ByVal strValidar)
        Dim strValidos As String = "abcdefghijlmnopqrstuvxzwyk0123456789 "
        Dim strAux As String
        Dim i As Integer
        strValidar = LCase(strValidar)
        For i = 1 To Len(strValidar)
            If InStr(strValidos, Mid(strValidar, i, 1)) = 0 Then
                strAux = strAux & "_"
            Else
                strAux = strAux & Mid(strValidar, i, 1)
            End If
        Next
        chrValidos = strAux
    End Function
    Public Function ValidaEmail(ByVal strEmail As String) As Boolean
        ValidaEmail = Regex.IsMatch(strEmail, "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
    End Function
End Class
