/************************************************
*	ARQUIVO : funcoes.js                        *
*	OBJETIVO: Codificação de funcões Javascript *
*	AUTOR: Raphael Jorge						*
*	EMPRESA: Aim Informática Ltda.              *
*************************************************/

//Variáveis Globais a serem utilizadas

	var janTop = (screen.height/2); 
	var janLeft = ((screen.width/2)-50);
	var nameApp = "Itambé - Transferência de arquivos";
	var ultimaLinhaSelecionada = "";

// Objetivo: Carregar métodos ao carregar a página (comum a todas as páginas)
// Uso: onload="javascript:focoCampoPadrao();"

	function focoCampoPadrao()
	{
		if(document.forms.length)
		{
			for(var i = 0; i < document.forms[0].length; i++)
			{
				if((!document.forms[0].elements[i].type == 'hidden') && (!document.forms[0].elements[i].readonly) && (!document.forms[0].elements[i].disabled))
				{
					document.forms[0].elements[i].focus();
					if(document.forms[0].elements[i].type == 'text') document.forms[0].elements[i].select();
					break;
				}
			}
		}
	}

// Objetivo: Navegar entre as paginas de iframe e escrever o título na janela principal
// Uso: onclick="javascript:navegacao('pagina.asp', 'titulo');"

	function navegacao(pagina, titulo)
	{
		window.parent.document.title = nameApp + " - " + titulo;
		window.iframeNavegacao.navigate(pagina)	;
	}

// Objetivo: Redimensiona a altura do iframe de acordo com a resolucao
// Uso: <body onload="javascript:tamanhoIframe();" onresize="javascript:tamanhoIframe();">

	function tamanhoIframe()
	{
		var scrnY = (document.body.clientHeight - 135);
		document.all('iframeNavegacao').style.height=scrnY;
	}

// Objetivo: Selecionar todos os checkboxes de exclusao
// Uso: onclick="javascript:marcaTodosRegistros();"

	function marcaTodosRegistros()
	{
		if(document.forms.length)
		{
			if(document.all('chkExclusao'))
			{
				if(document.forms[0].chkExclusao.length)
				{
					for(var i = 0; i < document.forms[0].chkExclusao.length; i++)
					{
						document.forms[0].chkExclusao[i].checked = !document.forms[0].chkExclusao[i].checked;
					}
				}
				else
				{
					document.forms[0].chkExclusao.checked = !document.forms[0].chkExclusao.checked;
				}
			}
		}
	}
	
// Objetivo: Validar se checkboxes de exclusao foram selecionados antes de enviar exclusao
// Uso: onclick="javascript:excluirRegistro();"

	function excluirRegistro()
	{
		return (confirm('Confirma a exclusão do registro ?'))
	}

// Objetivo: Abrir janela já centralizada
// Uso: onclick="javascript:abrirJanela('arquivo.asp', largura, altura, modal);"

	function abrirJanela(arquivo, largura, altura, modal)
	{
		if(!modal)
		{
			auxTop   = (janTop - (largura/2));
			auxLeft  = (janLeft - (altura/2));
			window.open(arquivo,'janSec','scrollbars=yes,width='+largura+',height='+altura+',left='+auxLeft+',top='+auxTop);
		}
	}

//Objetivo: Abrir janela de pesquisa no Active Directory
//Uso: onclick="javascript:abrirJanela('nomeDoCampo', largura, altura);"
	function abrirJanelaAD(strCampo, largura, altura)
	{
		auxTop   = (janTop - (largura/2));
		auxLeft  = (janLeft - (altura/2));
		window.open('ad.aspx?' + strCampo,'janSec','scrollbars=yes,width='+largura+',height='+altura+',left='+auxLeft+',top='+auxTop);
	}

// Objetivo: Copiar campos de um combo para outro
// Uso: onclick="javascript:copiaDePara(combo1, combo2);"

	function copiaDePara(de,para)
	{
		if(document.forms.length < 1)
		{
			alert('ERRO:\n\nO formulário padrão: \'forms[0]\' não foi encontrado.');
			return;
		}
		
		listaDe = eval('document.forms[0].' + de);
		listaPara = eval('document.forms[0].' + para);
		
		if (listaPara.options.length > 0 && listaPara.options[0].value == 'temp')
		{
			listaPara.options.length = 0;
		}

		var sel = false;

		for (var i=0; i < listaDe.options.length; i++)
		{
			var current = listaDe.options[i];
			if (current.selected)
			{
				sel = true;
				txt = current.text;
				val = current.value;
				if(document.all('nomeSistemas')) //alteracao pra gravar apenas sistemas incluidos
				{
					document.forms[0].nomeSistemas.value += current.text + '||';
				}
				listaPara.options[listaPara.length] = new Option(txt,val);
				listaDe.options[i] = null;
				i--;
			}
		}

		if (!sel) alert ('Selecione os registros que deseja copiar.');
	}
	
// Objetivo: Selecionar todas opcoes de um <select multiple>
// Uso: onclick="javascript:selecionaTodosCombo(this);"

	function selecionaTodosCombo(obj)
	{
		if(document.forms.length < 1)
		{
			alert('ERRO:\n\nO formulário padrão: \'forms[0]\' não foi encontrado.');
			return;
		}
		
		combo = eval('document.forms[0].' + obj);

		for(var i=0; i < combo.options.length; i++)
		{
			combo.options[i].selected = true;
		}
	}
	
// Objetivo: Passar todos valores de TEXTO de um combo para um campo hidden
// Uso: onclick="javascript:passaValores(de, para);"

	function passaValores(de, para)
	{
		combo = eval('document.forms[0].' + de);
		campo = eval('document.forms[0].' + para);
		
		for (var i=0; i < combo.options.length; i++)
		{
			campo.value += combo.options[i].text + '||';
		}
	}
	
//Objetivo: Validar todos os camposde formulário com valor vazio iniciados por "obr"
//Uso: <form onsubmit="return validaForm();">

	function validaForm()
	{
		for (i=0; i < document.forms[0].elements.length; i++)
		{			
			if (!document.forms[0].elements[i].disabled)
			{
				if((document.forms[0].elements[i].name.substr(0, 3) == "obr") && (document.forms[0].elements[i].value == ""))
				{
					alert("Atenção:\n\nCampo de Preenchimento Obrigatório!");
					document.forms[0].elements[i].focus();
					return false;
				}
			}
		}
		return true;
	}

//Objetivo: Controle botões e combo de navegação de registros.
//Uso: onclick="javascript:navegaRegistros(pagina)"

	function navegaRegistros(pagina)
	{
		for(var i=0; i < document.forms[0].chkExclusao.length; i++)
		{
			if(document.forms[0].chkExclusao[i].checked)
			{
				if(confirm('ATENÇÃO:\n\nMudando a página você irá perder a seleção do(s) registro(s) atual(is).\n\nDeseja continuar ?'))
				{
					for(var i=0; i < document.forms[0].chkExclusao.length; i++)
					{
						document.forms[0].chkExclusao[i].checked = false;
					}
					document.forms[0].action = self.location.pathname;
					document.forms[0].pagina.value = pagina;
					document.forms[0].submit();
					return;
				}
				else
				{
					return;
				}
			}
		}
		document.forms[0].action = self.location.pathname;
		document.forms[0].pagina.value = pagina;
		document.forms[0].submit();
	}

//Objetivo: Formatar um campo para o formato hora 'xx:xx'
//Uso: onblur="javascript:formataCampoHora(this);"

	function formataCampoHora(obj)
	{
		if((obj.value=='') || (obj.value.length == 5) || (!campoNumerico(obj))) return;
		
		if(obj.value.length == 4)
		{
			obj.value = obj.value.substr(0,2) + ':' + obj.value.substr(2,2);
		}
		else
		{
			alert('Atenção:\n\nO campo deve ter o formato: hhmm');
			obj.focus();
			obj.select();
		}
	}
	
//Objetivo: Impedir digititação de campos não numéricos
//Uso: onkeyup="javascript:campoNumerico(this);"

	function campoNumerico(obj)
	{
		if(obj.value=='') return;
		
		if(isNaN(obj.value))
		{
			alert('Atenção:\n\nO campo deve ser preenchido com valores numéricos.');
			obj.value = "";
			obj.focus();
		}
	}
	
//Objetivo: Limitar tamanho de campos <textarea>
//Uso: onkeypress="return tamanhoMaximo(this, tamanomaximo);"

	function tamanhoMaximo(obj, tamanho)
	{
		if(isNaN(tamanho)) return;
		
		if(obj.value.length >= tamanho) return false;
	}
	
//Objetivo: Trocar a cor (Classe CSS) de uma linha de tabela, quando clicada.
//Uso: onclick="linhaSelecionada(trID);"

	function linhaSelecionada(codLinha)
	{
		ultimaLinhaSelecionada.className = 'tdLinhaRegistro';
		codLinha.className='tdLinhaRegistroSelecionada';
		ultimaLinhaSelecionada = codLinha;
	}
	
	function montaMenus()
	{
		return true;
		
		/*
		var arrMenus = new Array("Externos", "Internos");
		var arrLinks = new Array("consultaExterna.asp", "consultaInterna.asp");
		
		document.innerHTML('<div id=\'menuConsulta\' style=\'visibility:hidden;position:absolute;top:30px;\'>');
		document.innerHTML('<table>');

		for(var i = 0; i < arrMenus.length; i++)
		{
			document.innerHTML('<tr>');
			document.innerHTML('<td>');
			document.innerHTML(arrMenus[i]);
			document.innerHTML('</td>');
			document.innerHTML('</tr>');
		}

		document.innerHTML('</table>');
		document.innerHTML('</div>');
		
		alert('entrou');
		
		document.getElementById('btnConsulta').attachEvent("onmouseover", "document.getElementById('menuConsulta').style.visibility=visible");
		*/
	}