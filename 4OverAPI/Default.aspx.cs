using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _4OverAPI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //FourOverConnect conn = new FourOverConnect();

            //string response = conn.GetOrganizations();

            //Response.Write(response);

            //FourOverOrder order = new FourOverOrder();

            FourOverOrder order = new FourOverOrder();

            order.order_id = "PC123456";
            order.is_test_order = true;
            order.coupon_code = "PRC5PKS";
            order.skip_confirmation = true;

            // JOBS
            order.jobs = new List<FourOverJob>();

            FourOverJob newJob = new FourOverJob()
            {
                colorspec_uuid = "ColorSpec UUID",
                dropship = false,
                option_uuids = {
                    "Option UUIDS"
                    },
                product_uuid = "Product UUID",
                runsize_uuid = "Runsize UUID",
                shipper =
                {
                     shipping_code = "03f",
                      shipping_method = "FREE UPS Ground"
                },
                ship_from =
                {
                    address = "121 Varick Street",
                    city = "New York",
                    company = "1-800 Postcards, Inc.",
                    country = "US",
                    email = "info@1800postcards.com",
                    firstname = "David",
                    lastname = "Moyal",
                    phone = "212-741-1070",
                    state = "NY",
                    zipcode = "10013"
                },
                 ship_to =
                {
                    address = "5 Windsor Terrace",
                    city = "Holmdel",
                    country = "US",
                    email = "keithhafer@me.com",
                    firstname = "Keith",
                    lastname = "Hafer",
                    phone = "917-940-5565",
                    state = "NJ",
                    zipcode = "07733"
                },
                  ship_from_facility = "DAY",
                  turnaroundtime_uuid = "Turnaround time UUID"
            };

            FourOverJob.FileSet set_001 = new FourOverJob.FileSet()
            {
                job_name = "Job One",
                files = { bk = "UUID for back file", fr = "UUID for front file" }
            };

            newJob.files.AddFile(set_001);

            order.jobs.Add(newJob);


            // PAYMENT
            order.payment = new FourOverPayment()
            {
                billing_info =
                {
                     first_name = "Keith",
                     last_name = "Hafer",
                     address1 = "5 Windsor Terrace",
                     city = "Holmdel",
                     state = "NJ",
                     zip = "07733",
                     country = "US"
                },
                comments = "These are some comments",
                credit_card =
                {
                     account_number = "4111111111111111",
                     ccv = "666",
                     month = "11",
                     year = "2020"
                },
                order_id = "PC123456",
                payment_provider =
                {
                     payment_provider_uuid = "Payment provider UUID"
                },
                requested_currency =
                {
                     currency_code = "USD"
                }
            };


            FourOverConnect conn = new FourOverConnect();

            string response = conn.PostToServer("orders", order.ToJSON());

            Response.Write(response);
        }
    }
}