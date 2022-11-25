package org.tempuri;

public class INavigationveloserviceSOAPProxy implements org.tempuri.INavigationveloserviceSOAP {
  private String _endpoint = null;
  private org.tempuri.INavigationveloserviceSOAP iNavigationveloserviceSOAP = null;
  
  public INavigationveloserviceSOAPProxy() {
    _initINavigationveloserviceSOAPProxy();
  }
  
  public INavigationveloserviceSOAPProxy(String endpoint) {
    _endpoint = endpoint;
    _initINavigationveloserviceSOAPProxy();
  }
  
  private void _initINavigationveloserviceSOAPProxy() {
    try {
      iNavigationveloserviceSOAP = (new org.tempuri.NavigationveloserviceLocator()).getBasicHttpBinding_INavigationveloserviceSOAP();
      if (iNavigationveloserviceSOAP != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iNavigationveloserviceSOAP)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iNavigationveloserviceSOAP)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iNavigationveloserviceSOAP != null)
      ((javax.xml.rpc.Stub)iNavigationveloserviceSOAP)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public org.tempuri.INavigationveloserviceSOAP getINavigationveloserviceSOAP() {
    if (iNavigationveloserviceSOAP == null)
      _initINavigationveloserviceSOAPProxy();
    return iNavigationveloserviceSOAP;
  }
  
  public org.datacontract.schemas._2004._07.WcfServiceLibrary1.Positions getCheminAVelo(java.lang.String depart, java.lang.String arrivee) throws java.rmi.RemoteException{
    if (iNavigationveloserviceSOAP == null)
      _initINavigationveloserviceSOAPProxy();
    return iNavigationveloserviceSOAP.getCheminAVelo(depart, arrivee);
  }
  
  
}