/*
HENRIQUE PIMENTEL
27/08/2003
V. 1.0
*/
function ver_dt(obj){
	var mask = obj.mask;
	if((mask == "00/00/0000")||(mask == "00/00/0000*")){	
		if(obj.value!=""){
			if(obj.value.length<10){
				alert("Data incorreta!");
				obj.value = "";		
			}
			else{
				if(!(verifica_data(obj.value))){
					alert("Data incorreta!");
					obj.value = "";
				}
			}
		}
	}
}