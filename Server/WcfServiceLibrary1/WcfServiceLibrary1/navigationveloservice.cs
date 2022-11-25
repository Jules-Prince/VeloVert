using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WcfServiceLibrary1
{
    public class navigationveloservice : INavigationveloserviceSOAP
    {
        [return: XmlElement("Chemin", Form = XmlSchemaForm.Unqualified)]
        public Positions getCheminAVelo([XmlElement(Form = XmlSchemaForm.Unqualified)] string Depart, [XmlElement(Form = XmlSchemaForm.Unqualified)] string Arrivee)
        {
            Position poss1 = new Position();
            Position poss2 = new Position();

            poss1.Latitude = 1;
            poss1.Longitude = 2;
            poss2.Longitude = 3;
            poss2.Latitude = 4;
            //throw new NotImplementedException();
            Positions p = new Positions();
            p.step = new Position[2];
            p.step[0] = poss1;
            p.step[1] = poss2;

            Console.WriteLine("Depart : " + Depart);
            Console.WriteLine("Arrivee : " + Arrivee);

            return p;
        }
    }
}
