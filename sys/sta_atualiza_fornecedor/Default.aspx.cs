using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
	private string BuscaLetra
	{
		get
		{
			if (Request["BuscarLetra"] != null && Request["BuscarLetra"].ToString() != "" && Request["BuscarLetra"].ToString() == "true")
			{
				return Request["BuscarLetra"];
			}
			else
			{
				return "";
			}
		}
	}

	private string Letra
	{
		get
		{
			if (Request["letra"] != null && Request["letra"].ToString() != "")
			{
				return Request["letra"];
			}
			else
			{
				return "";
			}
		}
	}

	private string CarregarFornecedor
	{
		get
		{
			if (Request["CarregarFornecedor"] != null && Request["CarregarFornecedor"].ToString() != "" && Request["CarregarFornecedor"].ToString() == "true")
			{
				return Request["CarregarFornecedor"];
			}
			else
			{
				return "";
			}
		}
	}
	private string ID_fornecedor
	{
		get
		{
			if (Request["ID"] != null && Request["ID"].ToString() != "")
			{
				return Request["ID"];
			}
			else
			{
				return "";
			}
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{

		if (BuscaLetra == "true" && Letra != "")
			pesquisaLetra(Letra);
		if (CarregarFornecedor == "true" && ID_fornecedor != "")
			carregaFornecedor(ID_fornecedor);




	}

	void pesquisaLetra(string letra)
	{
		Fornecedor objfornecedor = new Fornecedor();
		string xml = string.Empty;
		DataSet ds = new DataSet();


		ds = objfornecedor.Get_fornecedores_by_letra(letra);


		xml += "     <table border='0' cellpadding='0' cellspacing='5' width='511px' >";
		xml += "         <tr>";
		xml += "             <td id='cod' style='width: 100px'class='TituloAuto ColorCinzaClaro' align='center'  >";
		xml += "               Código</td>";
		xml += "             <td style='width: 275px' class='TituloAuto ColorCinzaClaro'>";
		xml += "                 Nome</td>";

		if (ds.Tables[0].Rows.Count > 0)
		{
			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{

				xml += "             <tr id='" + ds.Tables[0].Rows[i]["ANUM_SEQU_FORNECEDOR"].ToString() + "' class='BoxAutoCompletarLink' onmouseover='getBGColorAutoCompletar(this.id);' onmouseout='outBGColorAutoCompletar(this.id);' onclick='CarregarFornecedor(this.id);'>";
				xml += "             <td align='center'>" + ds.Tables[0].Rows[i]["ANUM_SEQU_FORNECEDOR"].ToString() + "</td>";
				xml += "             <td>" + ds.Tables[0].Rows[i]["ANOM_FORNECEDOR"].ToString() + " </td>";
				xml += "         </tr>";

			}
		}
		else
		{
			xml += "     <table border='0' cellpadding='0' cellspacing='5' width='511px' >";
			xml += "         <tr>";
			xml += "             <td class='TituloAuto ColorCinzaClaro' align='center' >";
			xml += "             Sem Resultados...</td>";
			xml += "         </tr>";
		}


		Response.Write(xml);
		Response.End();

	}

	void carregaFornecedor(string id)
	{
		Fornecedor objfornecedor = new Fornecedor();
		string myXML = string.Empty;
		DataSet ds = new DataSet();
		ds = objfornecedor.Get_Fornecedor_by_id(Convert.ToInt32(id));


		if (ds.Tables[0].Rows.Count > 0)
		{

			myXML += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
			myXML += "<FUNCIONARIOS>";
			myXML += "<FUNCIONARIO>";
			myXML += "<ANUM_SEQU_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ANUM_SEQU_FORNECEDOR"].ToString()) + "</ANUM_SEQU_FORNECEDOR>";
			myXML += "<ACOD_FORNECEDOR_BAAN>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ACOD_FORNECEDOR_BAAN"].ToString()) + "</ACOD_FORNECEDOR_BAAN>";
			myXML += "<ANOM_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ANOM_FORNECEDOR"].ToString().Replace("&", "||e||")) + "</ANOM_FORNECEDOR>";
			myXML += "<ANUM_CNPJ_CPF>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ANUM_CNPJ_CPF"].ToString()) + "</ANUM_CNPJ_CPF>";
			myXML += "<ANUM_TEL_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ANUM_TEL_FORNECEDOR"].ToString()) + "</ANUM_TEL_FORNECEDOR>";
			myXML += "<ADES_EMAIL_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ADES_EMAIL_FORNECEDOR"].ToString()) + "</ADES_EMAIL_FORNECEDOR>";
			myXML += "<ANOM_RESPONSAVEL_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ANOM_RESPONSAVEL_FORNECEDOR"].ToString()) + "</ANOM_RESPONSAVEL_FORNECEDOR>";
			myXML += "<ADES_DIRETORIO_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ADES_DIRETORIO_FORNECEDOR"].ToString()) + "</ADES_DIRETORIO_FORNECEDOR>";
			myXML += "<ADES_EMAIL_RESPONSAVEL>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ADES_EMAIL_RESPONSAVEL"].ToString()) + "</ADES_EMAIL_RESPONSAVEL>";
			myXML += "<ADES_DIRETORIO_FORNECEDOR>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["ADES_DIRETORIO_FORNECEDOR"].ToString()) + "</ADES_DIRETORIO_FORNECEDOR>";
			myXML += "<AFTP_EXT_ARQU>" + objfornecedor.TrataCampoNulo(ds.Tables[0].Rows[0]["AFTP_EXT_ARQU"].ToString()) + "</AFTP_EXT_ARQU>";

			myXML += "</FUNCIONARIO>";

		}// fim do if.

		else
		{
			Response.Write("ERRO!");
		}
		myXML += "</FUNCIONARIOS>";
		Response.ContentType = "text/xml";
		Response.Write(myXML);
		Response.End();

	}

	protected void btnGravar_ServerClick(object sender, EventArgs e)
	{
		bool resultado = false;
		Fornecedor objfornecedor = new Fornecedor();
		resultado = objfornecedor.Atualiza_fornecedor(txt_Nome.Value, txt_tel.Value, txt_emailFornecedor.Value, txt_Responsavel.Value, txt_emailResponsavel.Value, HiddenID.Value, txt_cpf.Value, txt_cod_fornecedor.Value, txt_Diretorio_fornecedor.Value, txt_extensao.Value);
		if (resultado == true)
		{
			Painel_informacoes.Attributes.CssStyle.Add("display", "none");
			hideDIv.Attributes.CssStyle.Add("display", "none");
			Resultado.Attributes.CssStyle.Add("display", "block");
			lbl_mensagem.Text = "Fornecedor atualizado com successo!";
			lbl_mensagem.Visible = true;

		}


	}
	protected void btn_delete_ServerClick(object sender, EventArgs e)
	{

		Fornecedor objfornecedor = new Fornecedor();

		if (objfornecedor.Deleta_fornecedor(Convert.ToInt32(HiddenID.Value)))
		{
			lbl_mensagem.Text = "Fornecedor deletado com sucesso";
			lbl_mensagem.Visible = true;
			Painel_informacoes.Attributes.CssStyle.Add("display", "none");
			hideDIv.Attributes.CssStyle.Add("display", "none");
			Resultado.Attributes.CssStyle.Add("display", "block");
		}// fim do if.
		else
		{
			lbl_mensagem.Text = "Ocorreu um erro no processo de deleção!Não foi possível deletar o fornecedor.";
			lbl_mensagem.Visible = true;
			Painel_informacoes.Attributes.CssStyle.Add("display", "none");
			hideDIv.Attributes.CssStyle.Add("display", "none");
			Resultado.Attributes.CssStyle.Add("display", "block");
		}// fim do else.

	}
}
