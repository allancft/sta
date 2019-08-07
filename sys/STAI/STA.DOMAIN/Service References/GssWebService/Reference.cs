﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1022
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STA.DOMAIN.GssWebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://microsoft.com/webservices/", ConfigurationName="GssWebService.ServiceSoap")]
    public interface ServiceSoap {
        
        // CODEGEN: Generating message contract since element name vLogin from namespace http://microsoft.com/webservices/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/webservices/GetCredencial", ReplyAction="*")]
        STA.DOMAIN.GssWebService.GetCredencialResponse GetCredencial(STA.DOMAIN.GssWebService.GetCredencialRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetCredencialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCredencial", Namespace="http://microsoft.com/webservices/", Order=0)]
        public STA.DOMAIN.GssWebService.GetCredencialRequestBody Body;
        
        public GetCredencialRequest() {
        }
        
        public GetCredencialRequest(STA.DOMAIN.GssWebService.GetCredencialRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://microsoft.com/webservices/")]
    public partial class GetCredencialRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string vLogin;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int vAplicacao;
        
        public GetCredencialRequestBody() {
        }
        
        public GetCredencialRequestBody(string vLogin, int vAplicacao) {
            this.vLogin = vLogin;
            this.vAplicacao = vAplicacao;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetCredencialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCredencialResponse", Namespace="http://microsoft.com/webservices/", Order=0)]
        public STA.DOMAIN.GssWebService.GetCredencialResponseBody Body;
        
        public GetCredencialResponse() {
        }
        
        public GetCredencialResponse(STA.DOMAIN.GssWebService.GetCredencialResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://microsoft.com/webservices/")]
    public partial class GetCredencialResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetCredencialResult;
        
        public GetCredencialResponseBody() {
        }
        
        public GetCredencialResponseBody(string GetCredencialResult) {
            this.GetCredencialResult = GetCredencialResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceSoapChannel : STA.DOMAIN.GssWebService.ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<STA.DOMAIN.GssWebService.ServiceSoap>, STA.DOMAIN.GssWebService.ServiceSoap {
        
        public ServiceSoapClient() {
        }
        
        public ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        STA.DOMAIN.GssWebService.GetCredencialResponse STA.DOMAIN.GssWebService.ServiceSoap.GetCredencial(STA.DOMAIN.GssWebService.GetCredencialRequest request) {
            return base.Channel.GetCredencial(request);
        }
        
        public string GetCredencial(string vLogin, int vAplicacao) {
            STA.DOMAIN.GssWebService.GetCredencialRequest inValue = new STA.DOMAIN.GssWebService.GetCredencialRequest();
            inValue.Body = new STA.DOMAIN.GssWebService.GetCredencialRequestBody();
            inValue.Body.vLogin = vLogin;
            inValue.Body.vAplicacao = vAplicacao;
            STA.DOMAIN.GssWebService.GetCredencialResponse retVal = ((STA.DOMAIN.GssWebService.ServiceSoap)(this)).GetCredencial(inValue);
            return retVal.Body.GetCredencialResult;
        }
    }
}
