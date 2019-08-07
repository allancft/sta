/*************************************
AJAX SCRIPT
pagina:
    script Mestre
Data:
    03/04/2007
Developer: 
    Daniel M. Moreira(dmmoreira@multiservices.com.br)
**************************************/
var xmlHttp;
/*Criando o XmlHttpRequest*/
function createXMLHttpRequest(){
    try
    {    // Firefox, Opera 8.0+, Safari    
        xmlHttp=new XMLHttpRequest();    
    }
    catch (e)
    { // Internet Explorer    
        try
        {
            xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");      
        }
        catch (e)
        {
            try
            {
            xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");        
            }
            catch (e)
            {
            alert("Your browser does not support AJAX!");        
            }
        }
   }    
}
