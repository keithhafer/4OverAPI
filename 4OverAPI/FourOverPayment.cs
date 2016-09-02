using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4OverAPI
{
    public class FourOverPayment
    {
        public PaymentProvider payment_provider { get; set; }
        public RequestedCurrency requested_currency { get; set; }
        public CreditCard credit_card { get; set; }
        public BillingInfo billing_info { get; set; }
        public string order_id { get; set; }
        public string comments { get; set; }

        public class PaymentProvider
        {
            public string payment_provider_uuid { get; set; }
        }

        public class RequestedCurrency
        {
            public string currency_code { get; set; }
        }

        public class CreditCard
        {
            public string account_number { get; set; }
            public string month { get; set; }
            public string year { get; set; }
            public string ccv { get; set; }
        }

        public class BillingInfo
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
        }

        public FourOverPayment()
        {
            this.billing_info = new BillingInfo();
            this.credit_card = new CreditCard();
            this.payment_provider = new PaymentProvider();
            this.requested_currency = new RequestedCurrency();
        }
    }
}