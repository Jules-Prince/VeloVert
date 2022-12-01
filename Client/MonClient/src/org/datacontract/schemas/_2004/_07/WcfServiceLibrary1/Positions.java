/**
 * Positions.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package org.datacontract.schemas._2004._07.WcfServiceLibrary1;

public class Positions  implements java.io.Serializable {
    private org.datacontract.schemas._2004._07.WcfServiceLibrary1.Position[] step;

    public Positions() {
    }

    public Positions(
           org.datacontract.schemas._2004._07.WcfServiceLibrary1.Position[] step) {
           this.step = step;
    }


    /**
     * Gets the step value for this Positions.
     * 
     * @return step
     */
    public org.datacontract.schemas._2004._07.WcfServiceLibrary1.Position[] getStep() {
        return step;
    }


    /**
     * Sets the step value for this Positions.
     * 
     * @param step
     */
    public void setStep(org.datacontract.schemas._2004._07.WcfServiceLibrary1.Position[] step) {
        this.step = step;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof Positions)) return false;
        Positions other = (Positions) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.step==null && other.getStep()==null) || 
             (this.step!=null &&
              java.util.Arrays.equals(this.step, other.getStep())));
        __equalsCalc = null;
        return _equals;
    }

    private boolean __hashCodeCalc = false;
    public synchronized int hashCode() {
        if (__hashCodeCalc) {
            return 0;
        }
        __hashCodeCalc = true;
        int _hashCode = 1;
        if (getStep() != null) {
            for (int i=0;
                 i<java.lang.reflect.Array.getLength(getStep());
                 i++) {
                java.lang.Object obj = java.lang.reflect.Array.get(getStep(), i);
                if (obj != null &&
                    !obj.getClass().isArray()) {
                    _hashCode += obj.hashCode();
                }
            }
        }
        __hashCodeCalc = false;
        return _hashCode;
    }

    // Type metadata
    private static org.apache.axis.description.TypeDesc typeDesc =
        new org.apache.axis.description.TypeDesc(Positions.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://schemas.datacontract.org/2004/07/WcfServiceLibrary1", "Positions"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("step");
        elemField.setXmlName(new javax.xml.namespace.QName("http://schemas.datacontract.org/2004/07/WcfServiceLibrary1", "step"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://schemas.datacontract.org/2004/07/WcfServiceLibrary1", "Position"));
        elemField.setMinOccurs(0);
        elemField.setNillable(true);
        elemField.setItemQName(new javax.xml.namespace.QName("http://schemas.datacontract.org/2004/07/WcfServiceLibrary1", "Position"));
        typeDesc.addFieldDesc(elemField);
    }

    /**
     * Return type metadata object
     */
    public static org.apache.axis.description.TypeDesc getTypeDesc() {
        return typeDesc;
    }

    /**
     * Get Custom Serializer
     */
    public static org.apache.axis.encoding.Serializer getSerializer(
           java.lang.String mechType, 
           java.lang.Class _javaType,  
           javax.xml.namespace.QName _xmlType) {
        return 
          new  org.apache.axis.encoding.ser.BeanSerializer(
            _javaType, _xmlType, typeDesc);
    }

    /**
     * Get Custom Deserializer
     */
    public static org.apache.axis.encoding.Deserializer getDeserializer(
           java.lang.String mechType, 
           java.lang.Class _javaType,  
           javax.xml.namespace.QName _xmlType) {
        return 
          new  org.apache.axis.encoding.ser.BeanDeserializer(
            _javaType, _xmlType, typeDesc);
    }

}
