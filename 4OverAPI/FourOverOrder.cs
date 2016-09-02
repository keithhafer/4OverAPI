using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4OverAPI
{
    public class FourOverOrder
    {
        /// <summary>
        /// order_id specified by the API user. (Required)
        /// </summary>
        public string order_id { get; set; }
        /// <summary>
        /// allows the user to submit test orders. Orders with is_test_order set to TRUE will not go to production. (Optional and default is set to FALSE)
        /// </summary>
        public bool is_test_order { get; set; } = true;
        /// <summary>
        /// allows user to pass a coupon-code that will apply to any resources qualified in the order based on coupon rules. Only one coupon per order is allowed (Optional)
        /// </summary>
        public string coupon_code { get; set; }
        /// <summary>
        /// By setting this flag to TRUE the system will bypass conformation. If the flag is set to FALSE, an e-mail containing a HTML preview link is emailed to the authenticated user.  (Optional and default set to FALSE)
        /// </summary>
        public bool skip_confirmation { get; set; }
        /// <summary>
        /// Jobs (Required)
        /// </summary>
        public List<FourOverJob> jobs { get; set; }
     
        public FourOverPayment payment { get; set; }

        public string ToJSON()
        {
            string json = JsonConvert.SerializeObject(this);

            return json;
        }
        
    }
}
 