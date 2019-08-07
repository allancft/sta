/*
HENRIQUE PIMENTEL
23/10/2003
V. 1.01
*/
function verifica_data (dt) {
		//retorna verdadeiro se a data for valida
		var situacao = "";
		if(dt.length>=10){
			dia = (dt.substring(0,2));
			mes = (dt.substring(3,5));
			ano = (dt.substring(6,10));	
	
			
			// verifica o dia valido para cada mes
			if ((dia < 01)||(dia < 01 || dia > 30) && (  mes == 04 || mes == 06 || mes == 09 || mes == 11 ) || dia > 31) {
				situacao = "falsa";
				return false;
			}
			
			// verifica se o mes e valido
			if (mes < 01 || mes > 12 ) {
				situacao = "falsa";
				return false;
			}
			
			// verifica se e ano bissexto			
			//(!((ano%400==0) || ((ano%100!=0) && (ano%4==0))))			
			if (mes == 02 && ( dia < 01 || dia > 29 || ( dia > 28 && ((ano%400!=0) && ((ano%100==0)||(ano%4!=0)))))) {
				situacao = "falsa";
				return false;
			}
	}else{
		situacao = "falsa";
	}
			
			if (situacao == "falsa") {
				return false;
			}else{
				return true;
			}	
}