
package prince.yvars.velovert_client.wsdl;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour anonymous complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="Depart" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *         &lt;element name="Arrivee" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "depart",
    "arrivee"
})
@XmlRootElement(name = "getCheminAVelo")
public class GetCheminAVelo {

    @XmlElementRef(name = "Depart", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<String> depart;
    @XmlElementRef(name = "Arrivee", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<String> arrivee;

    /**
     * Obtient la valeur de la propriété depart.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getDepart() {
        return depart;
    }

    /**
     * Définit la valeur de la propriété depart.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setDepart(JAXBElement<String> value) {
        this.depart = value;
    }

    /**
     * Obtient la valeur de la propriété arrivee.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getArrivee() {
        return arrivee;
    }

    /**
     * Définit la valeur de la propriété arrivee.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setArrivee(JAXBElement<String> value) {
        this.arrivee = value;
    }

}
