using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;



public class Fornecedor
    {
    /// <summary>
    /// Variavel contém id do fornecedor
    /// </summary>
    private int _fornecedor_id;
    /// <summary>
    /// Variavel contem letra queserá usada para busca de forncedores
    /// </summary>
    private string _forncedor_letra;
    /// <summary>
    /// Variavel contem nome do fornecedor
    /// </summary>
    private string _nome;
    /// <summary>
    /// Variavel contem telefone do fornecedor
    /// </summary>
    private string _tel;
    /// <summary>
    /// Variavel contem o email do fornecedor
    /// </summary>
    private string _email;
    /// <summary>
    /// Variavel contem o nome do responsavel do fornecedor
    /// </summary>
    private string _responsavel;
    /// <summary>
    /// Variavel contem o email do respnsavel do fornecedor
    /// </summary>
    private string _responsavel_email;
    /// <summary>
    /// Variavel contem cnpj/cpf do fornecedor
    /// </summary>
    private string _cnpj;
    /// <summary>
    /// Variavel contem codigo do fornecedor
    /// </summary>
    private string _cod_fornecedor;
    /// <summary>
    /// Variavel contem diretorio do fornedor
    /// </summary>
    private string _diretorio_fornecedor;
    /// <summary>
    /// Variavel contem a senha do fornecedor
    /// </summary>
    private string _senha;
    /// <summary>
    /// Variavel contem as extensão do fornecedor
    /// </summary>
    private string _extensao;
    /// <summary>
    /// Variavel que contera o campo a ser verificado.
    /// </summary>
    private string _variavel;
    /// <summary>
    /// Variavel contém id do fornecedor
    /// </summary>
    public int Fornecedor_id
        {
        get
            {
            return _fornecedor_id;
            }
        set
            {
            _fornecedor_id = value;
            }
        }

    /// <summary>
    /// Variavel contem letra queserá usada para busca de forncedores
    /// </summary>
    public string Fornecedor_letra
        {
        get
            {
            return _forncedor_letra;
            }
        set
            {
            _forncedor_letra = value;
            }
        }

    /// <summary>
    /// Variavel contem nome do fornecedor
    /// </summary>
    public string Nome
        {
        get
            {
            return _nome;

            }
        set
            {
            _nome = value;
            }
        }

    /// <summary>
    /// Variavel contem telefone do fornecedor
    /// </summary>
    public string Tel
        {
        get
            {
            return _tel;
            }
        set
            {
            _tel = value;
            }
        }

    /// <summary>
    /// Variavel contem o email do fornecedor
    /// </summary>
    public string Email
        {
        get
            {
            return _email;
            }
        set
            {
            _email = value;
            }
        }

    /// <summary>
    /// Variavel contem o nome do responsavel do fornecedor
    /// </summary>
    public string Responsavel
        {
        get
            {
            return _responsavel;
            }
        set
            {
            _responsavel = value;
            }
        }

    /// <summary>
    /// Variavel contem o email do respnsavel do fornecedor
    /// </summary>
    public string Responsavel_email
        {
        get
            {
            return _responsavel_email;
            }
        set
            {
            _responsavel_email = value;
            }
        }

    /// <summary>
    /// Variavel contem cnpj/cpf do fornecedor
    /// </summary>
    public string CNPJ
        {
        get
            {
            return _cnpj;
            }
        set
            {
            _cnpj = value;
            }

        }

    /// <summary>
    /// Variavel contem codigo do fornecedor
    /// </summary>
    public string COD_fornecedor
        {
        get
            {
            return _cod_fornecedor;
            }
        set
            {
            _cod_fornecedor = value;
            }
        }

    /// <summary>
    /// Variavel contem diretorio do fornedor
    /// </summary>
    public string Diretorio_fornecedor
        {
        get
            {
            return _diretorio_fornecedor;
            }
        set
            {
            _diretorio_fornecedor = value;
            }
        }

    /// <summary>
    /// Variavel contem a senha do fornecedor
    /// </summary>
    public string Senha
        {
        get
            {
            return _senha;
            }
        set
            {
            _senha = value;
            }
        }

    /// <summary>
    /// Variavel contem as extensão do fornecedor
    /// </summary>
    public string extensao
        {
        get
            {
            return _extensao;
            }
        set
            {
            _extensao = value;
            }
        }

    /// <summary>
    /// Variavel que contera o campo a ser verificado.
    /// </summary>
    public string Variavel
        {
        get
            {
            return _variavel;
            }
        set
            {
            _variavel = value;
            }
        }

    /// <summary>
    /// Metodo pega fornecedor pelo id.
    /// </summary>
    /// <param name="Fornecedor_id">Variavel contém id do fornecedor</param>
    public DataSet Get_Fornecedor_by_id(int Fornecedor_id)
        {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["STA_conn"].ConnectionString);
        SqlCommand cmd = new SqlCommand("Proc_fornecedor_by_id", conn);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
            {
            cmd.Parameters.AddWithValue("@FORNECEDOR",Fornecedor_id);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            adp.Fill(ds);
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            }// fim do try.
        catch(Exception)
            {
            Console.WriteLine("ERRO!");
            }//catch
        return ds;


        }// fim do metodo Get_Fornecedor_by_id

    /// <summary>
    /// Metodo retorna data sete através do parametro informado.
    /// </summary>
    /// <param name="fornecedor_letra">Variavel contem letra queserá usada para busca de forncedores</param>
    public DataSet Get_fornecedores_by_letra(string Fornecedor_letra)
        {
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["STA_conn"].ConnectionString);
        SqlCommand cmd = new SqlCommand("Proc_Fornecedor_by_autocompletar", conn);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
            {
            cmd.Parameters.AddWithValue("@Fornecedor_letra", Fornecedor_letra);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            adp.Fill(ds);
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            }// fim do try.
        catch (Exception err)
            {
            Console.WriteLine("ERRO!");
            }//catch
        return ds;

        }

    /// <summary>
    /// Metodo atulaiza fornecedor
    /// </summary>
    /// <param name="Nome">Variavel contem nome do fornecedor</param>
    /// <param name="Tel">Variavel contem telefone do fornecedor</param>
    /// <param name="Email">Variavel contem o email do fornecedor</param>
    /// <param name="Responsavel">Variavel contem o nome do responsavel do fornecedor</param>
    /// <param name="Responsavel_email">Variavel contem o email do respnsavel do fornecedor</param>
    /// <param name="Fornecedor_id">Variavel contém id do fornecedor</param>
    /// <param name="CNPJ">Variavel contem cnpj/cpf do fornecedor</param>
    /// <param name="COD_fornecedor">Variavel contem codigo do fornecedor</param>
    /// <param name="Diretorio_fornecedor">Variavel contem diretorio do fornedor</param>
    /// <param name="Senha">Variavel contem a senha do fornecedor</param>
    /// <param name="extensao">Variavel contem as extensão do fornecedor</param>
    /// <param name="tel">Variavel contem telefone do fornecedor</param>
    /// <param name="email">Variavel contem o email do fornecedor</param>
    /// <param name="responsavel">Variavel contem o nome do responsavel do fornecedor</param>
    /// <param name="responsavel_email">Variavel contem o email do respnsavel do fornecedor</param>
    public bool Atualiza_fornecedor(string Nome, string Tel, string Email, string Responsavel, string Responsavel_email, string Fornecedor_id, string CNPJ, string COD_fornecedor, string Diretorio_fornecedor,string extensao)
        {
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["STA_conn"].ConnectionString);
        SqlCommand cmd = new SqlCommand("Proc_fornecedor_update", conn);
     

        try
            {
            cmd.Parameters.AddWithValue("@FORNECEDOR_NOME", Nome);
            cmd.Parameters.AddWithValue("@TEL", Tel);
            cmd.Parameters.AddWithValue("@EMAIL_FORNECEDOR", Email);
            cmd.Parameters.AddWithValue("@FORNECEDOR_RESPONSAVEL", Responsavel);
            cmd.Parameters.AddWithValue("@EMAIL_FORNECEDOR_RESPONSAVEL", Responsavel_email);
            cmd.Parameters.AddWithValue("@ID", Fornecedor_id);
            cmd.Parameters.AddWithValue("@CNPJ", CNPJ);
            cmd.Parameters.AddWithValue("@COD_FORNECEDOR", COD_fornecedor);
            cmd.Parameters.AddWithValue("@FORNECEDOR_DIRETORIO", Diretorio_fornecedor);
            cmd.Parameters.AddWithValue("@EXTENSAO", extensao);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            }// fim do try.
        catch (Exception)
            {
            Console.WriteLine("ERRO!");
            }//catch
        return true;
        }

    /// <summary>
    /// Função trta campo nulo
    /// </summary>
    /// <param name="Variavel">Variavel que contera o campo a ser verificado.</param>
    public string TrataCampoNulo(string Variavel)
        {
        if (Variavel.Trim() == "")
            return "-";
        else
            return Variavel;
        }

    /// <summary>
    /// Metodo deleta fornecedor
    /// </summary>
    /// <param name="Fornecedor_id">Fornecedor_id</param>
    public bool Deleta_fornecedor(int Fornecedor_id)
        {


        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["STA_conn"].ConnectionString);
        SqlCommand cmd = new SqlCommand("Proc_Fornecedor_delete", conn);


        try
            {
            cmd.Parameters.AddWithValue("@IdFornecedor", Fornecedor_id);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            return true;
            }// fim do try.
        catch (Exception )
            {
            Console.WriteLine("ERRO!");
            return false;
            }//catch

        }// fim do Get_fornecedores_by_letra

}// fim da classe.
