package com.polytech.velovert;

import java.rmi.RemoteException;

import org.datacontract.schemas._2004._07.WcfServiceLibrary1.Position;
import org.datacontract.schemas._2004._07.WcfServiceLibrary1.Positions;
import org.tempuri.INavigationveloserviceSOAP;
import org.tempuri.INavigationveloserviceSOAPProxy;

public class Client {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		INavigationveloserviceSOAPProxy serviceSOAP = new INavigationveloserviceSOAPProxy();
		
		try {
			Positions p =  serviceSOAP.getCheminAVelo("lala", "lulu");
			Position[] tutu = p.getStep();
			for(int i = 0; i < tutu.length; i++) {
				System.out.println(tutu[i].getLatitude());
				System.out.println(tutu[i].getLongitude());
			}
		} catch (RemoteException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		
	}

}
