using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary1
{

    public class Positions
    {

        private Position[] stepField;

        /// <remarks/>
        public Position[] step
        {
            get
            {
                return this.stepField;
            }
            set
            {
                this.stepField = value;
            }
        }
    }

    public class Position
    {

        private double latitudeField;

        private double longitudeField;

        /// <remarks/>
        public double Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        public double Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }
    }

}
