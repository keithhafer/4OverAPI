using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4OverAPI
{
    public class FourOverAddress
    {
        public class ShipAddress
        {
            public string company { get; set; }
            /// <summary>
            /// Ship_to First Name. (Required if company is not provided)
            /// </summary>
            public string firstname { get; set; }
            /// <summary>
            /// Ship_to Last Name. (Required if company is not provided)
            /// </summary>
            public string lastname { get; set; }
            /// <summary>
            /// {string} Ship_to Email. (optional)
            /// </summary>
            public string email { get; set; }
            /// <summary>
            /// Ship_to Phone Number. (Required)
            /// </summary>
            public string phone { get; set; }
            /// <summary>
            /// Ship_to address line. (Required)
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// Ship_to address2 line. (Optional)
            /// </summary>
            public string address2 { get; set; }
            /// <summary>
            /// Ship_to city. (Required)
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// Ship_to state. (Required)
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// Ship-to zipcode. (Required)
            /// </summary>
            public string zipcode { get; set; }
            /// <summary>
            /// Ship_to country. (Required)
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 
            /// </summary>
            /// 

            public ShipAddress()
            {

            }
        }

        public FourOverAddress()
        {

        }
    }
}