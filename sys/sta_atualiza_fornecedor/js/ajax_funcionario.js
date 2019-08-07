//Cria efeito de cor para os movimentos do mouse
 function getBGColorAutoCompletar(myID) {       
           document.getElementById(myID).style.backgroundColor='#fafae7';
           }
 function outBGColorAutoCompletar(myID) {
            document.getElementById(myID).style.backgroundColor='#fff';
             }

 

//Função manda letra a ser pesquisada
function BuscaLetra(letra){ 
   
        if(letra)
        {
            createXMLHttpRequest();
            xmlHttp.onreadystatechange = hscMontaResultado;
            xmlHttp.open("GET","?BuscarLetra=true&letra="+letra, true);
            xmlHttp.send(null);
        }
  
     else
     {
     alert("Digite uma letra para fazer a busca de fornecedor!");
     }
        
}
//Função mostra pesquisa feita pela letra e tipo.
function hscMontaResultado()
{
        if(xmlHttp.readyState == 4) 
        {
            if(xmlHttp.status == 200) 
            {
                document.getElementById("loader").style.display="none";
                document.getElementById("BoxAutoCompletar").style.display="block";
                document.getElementById("BoxAutoCompletar").innerHTML=xmlHttp.responseText;
            }
        }
    else
        {
           document.getElementById("loader").style.display="block";
           document.getElementById("BoxAutoCompletar").style.display="block";
           document.getElementById("BoxAutoCompletar").innerHTML="<img src='imagens/indicator.gif' alt='Carregando...' />";
        }
}



//funçaõ esconde div.
function HideDiv(x)
{
 document.getElementById("BoxAutoCompletar").style.display="none";
}

function CarregarFornecedor(ID){
        createXMLHttpRequest();
        xmlHttp.onreadystatechange = hscCarregarFornecedor;
        xmlHttp.open("GET","?CarregarFornecedor=true&ID="+ID, true);
        xmlHttp.send(null);
}


function hscCarregarFornecedor()
{
       if(xmlHttp.readyState == 4) 
        {
            if(xmlHttp.status == 200) 
            {
            
             // escreve componentes html;
                var xmlDoc=xmlHttp.responseXML.documentElement;
                if(xmlDoc)
                {
                  
                    //Atribuir Valores nos componentes HTML
                    document.getElementById("HiddenID").value=xmlDoc.getElementsByTagName("ANUM_SEQU_FORNECEDOR")[0].childNodes[0].nodeValue;
                    //Obs.: A tabela de Clientes não contempla o ID do Endereco Basico do Cliente                 
                    document.getElementById("txt_Nome").value=xmlDoc.getElementsByTagName("ANOM_FORNECEDOR")[0].childNodes[0].nodeValue.replace("||e||", "&");
                    document.getElementById("txt_cpf").value=xmlDoc.getElementsByTagName("ANUM_CNPJ_CPF")[0].childNodes[0].nodeValue; 
                    document.getElementById("txt_tel").value=xmlDoc.getElementsByTagName("ANUM_TEL_FORNECEDOR")[0].childNodes[0].nodeValue; 
                    document.getElementById("txt_emailFornecedor").value=xmlDoc.getElementsByTagName("ADES_EMAIL_FORNECEDOR")[0].childNodes[0].nodeValue;
                    document.getElementById("txt_Responsavel").value=xmlDoc.getElementsByTagName("ANOM_RESPONSAVEL_FORNECEDOR")[0].childNodes[0].nodeValue;
                    document.getElementById("txt_emailResponsavel").value=xmlDoc.getElementsByTagName("ADES_EMAIL_RESPONSAVEL")[0].childNodes[0].nodeValue;
                    document.getElementById("txt_cod_fornecedor").value=xmlDoc.getElementsByTagName("ACOD_FORNECEDOR_BAAN")[0].childNodes[0].nodeValue;
                    document.getElementById("txt_Diretorio_fornecedor").value=xmlDoc.getElementsByTagName("ADES_DIRETORIO_FORNECEDOR")[0].childNodes[0].nodeValue;
                    document.getElementById("txt_extensao").value=xmlDoc.getElementsByTagName("AFTP_EXT_ARQU")[0].childNodes[0].nodeValue;

                    document.getElementById("loader").style.display="none";
                    document.getElementById("Painel_informacoes").style.display="block";
                    document.getElementById("Resultado").style.display="none";
                    document.getElementById("hideDIv").style.display="block";
                    
                              
                    
                }
            }
        }
        else
        {
        document.getElementById("loader").style.display="block";
        }
}


function LimparWebForm(){
// limapa os campos do form
    document.getElementById("txt_Nome").value="";
    document.getElementById("txt_cpf").value="";
    document.getElementById("txt_tel").value="";
    document.getElementById("txt_emailFornecedor").value="";
    document.getElementById("txt_Responsavel").value="";
    document.getElementById("txt_emailResponsavel").value="";
    document.getElementById("txt_cod_fornecedor").value="";
    document.getElementById("txt_Diretorio_fornecedor").value="";
    document.getElementById("txt_extensao").value="";
    document.getElementById("Painel_informacoes").style.display="none";
    document.getElementById("hideDIv").style.display="none";

}

