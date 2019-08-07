/*
HENRIQUE PIMENTEL
29/10/2003
V. 1.06
*/
function mascara(obj)
{
	var mask = obj.getAttribute("mask")
	
	if(mask)
	{	
		for(u=0;u<=obj.value.length;u++)
		{
		/*
		===============================================================================
		DEFINICAO TIPOS DE MASCARA PARA VALIDACAO DO CAMPO
		X - UM CARACTER
		0 - UM NUMERO
		% - SOMENTE CARACTERES ( A-Z , a-z)
		# - SOMENTE NUMEROS (0-9)
		
		_ - PESQUISA CARACTERES ALPHANUMERICOS MAIS OS SIMBOLOS * ? % : | < > = 
		_# - PESQUISA NUMEROS MAIS OS SIMBOLOS * ? % : | < > = 
		_% - PESQUISA CARACTERES ALPHA MAIS OS SIMBOLOS * ? % : | < > = 
		
		~regEXp - EXPRESSAO REGULAR PADRAO DE JAVASCRIPT

		'N' - CARACTER OBRIGATORIO ONDE 'N' E O CARACTER

		* - CORINGA (qualquer caracter)
		EX:
		00/00/0000 - DOIS NUMEROS, "/", DOIS NUMEROS, "/" , QUATRO NUMEROS
		0* - QUALQUER COISA COMECADO POR NUMERO
		A* - QUALQUER COISA COMECADO COM "A"
		XXX0 - TRES CARACTERES E UM NUMERO: O CAMPO TERA SEMPRE 4 DIGITOS
		XXX0* - QUALQUER COISA COMECADO POR TRES CARACTERES E UM NUMERO: O CAMPO NAO TERA TAMANHO DEFINIDO
		===============================================================================
		*/
		var msk = "";
		var Xpr = "";
		var vf = "";
		var tam = "";
		if(mask.charAt(mask.length-1)=="*")
		{
			tam = obj.value.length;
		}
		else
		{
			tam = mask.length;
		}
		
		
		
		if(mask.charAt(0)=="~")
		{
				
				ex_reg = new RegExp(mask.substr(1,(mask.length-1)), "g");

				obj.value = obj.value.replace(/\n/, "")
				obj.value = obj.value.replace(/\r/, "")



				var v = obj.value.search(ex_reg)
				if(v>-1){
					for(x=0;x<=obj.value.length;x++){
						obj.value = obj.value.replace(ex_reg, "")
					}
				}

		}else{
			if(mask.charAt(0)=="_"){
				
				switch(mask)
				{
						case "_":
							Xpr = /[^A-Za-z0-9\*\?\%\_\:\|\<\>\=\s\r\n]/;
						break;
						case "_#":
							
							Xpr = /[^0-9\*\?\%\_\:\|\<\>\=\s\r\n]/;
						break;
						case "_%":							
							Xpr = /[^A-Za-z\*\?\%\_\:\|\<\>\=\s\r\n]/;
						break;				
				}				
				var v = obj.value.search(Xpr)
				if(v>-1){
					for(x=0;x<=obj.value.length;x++){
						obj.value = obj.value.replace(Xpr, "")
					}
				}
				
				
				}else{
					a=0;
					for(a=0;a<=tam-1;a++)
					{
						if(mask == "%")
						{
							vx = "X";
							tam = obj.value.length;
						}
						else if(mask == "#")
						{
							vx = "0";
							tam = obj.value.length;
						}
						else
						{
							if(a>mask.length-1)
								vx = "*";
							else
								vx = mask.charAt(a);
						}
							vl =  obj.value.charAt(a)
	
						switch (vx)
						{
			   				case "X" :
			   					Xpr = /[A-Za-z\r\n]/;
			   					if(vl.search(Xpr)==0)
			   						vf += vl;
			      			break;
	
			      			case "0" :
			      				Xpr = /[0-9]/;
			   					if(vl.search(Xpr)==0)
			   						vf += vl;
			      			break;
	
			      			case "*" :
			      				//Xpr = /[A-Za-z0-9À-ÖÙ-Ýà-öù-ý]/;
			      				Xpr = /[A-Za-z0-9\s]/;
			      				//Xpr = "";
			   					if(vl.search(Xpr)==0)
			   						vf += vl;
			      			break;
	
			      			default :
			      				Xpr = new RegExp(vx,"g");
			   					if(vl.search(Xpr)==0)
			   						vf += vl;
	
			      		}
					}
					obj.value = vf;
				}
			
			}
		}
		//alert("obj.value.length"+obj.value.length+"\ntam"+tam);
	}
else
	{	
	/*	obj.value = obj.value.replace(/\n/, "")
		obj.value = obj.value.replace(/\r/, "")

		eXREG =/[^A-Za-z0-9 \r\n]/;
		var v = obj.value.search(eXREG)
		if(v>-1)
		{	for(x=0;x<=obj.value.length;x++)
			{
				obj.value = obj.value.replace(eXREG, "")
			}
		}*/
	}

}