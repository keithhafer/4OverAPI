using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace _4OverAPI
{
    public class FourOverConnect
    {
        #region Configuration Values
        private string _PublicKey;
        public string PublicKey
        {
            get
            {
                if (String.IsNullOrEmpty(_PublicKey))
                {
                    string key = "FourOverPublicKey";
                    // Get the public key value from config file
                    string configValue = ConfigurationManager.AppSettings[key];
                    if (String.IsNullOrEmpty(configValue))
                    {
                        throw new Exception("\"" + key + "\" value missing in Web.Config");
                    }
                    _PublicKey = configValue;
                }
                return _PublicKey;
            }
        }

        private string _PrivateKey;
        public string PrivateKey
        {
            get
            {
                if (String.IsNullOrEmpty(_PrivateKey))
                {
                    string key = "FourOverPrivateKey";
                    // Get the public key value from config file
                    string configValue = ConfigurationManager.AppSettings[key];
                    if (String.IsNullOrEmpty(configValue))
                    {
                        throw new Exception("\"" + key + "\" value missing in Web.Config");
                    }
                    _PrivateKey = configValue;
                }
                return _PrivateKey;
            }
        }

        private string _URLBase;
        public string URLBase
        {
            get
            {
                if (String.IsNullOrEmpty(_URLBase))
                {
                    string key = "FourOverURLBase";
                    // Get the public key value from config file
                    string configValue = ConfigurationManager.AppSettings[key];
                    if (String.IsNullOrEmpty(configValue))
                    {
                        throw new Exception("\"" + key + "\" value missing in Web.Config");
                    }
                    _URLBase = configValue;
                }
                return _URLBase;
            }
        }
        #endregion

        public enum HTTP_Method
        {
            GET,
            DELETE,
            POST,
            PUT,
            PATCH
        }

        #region Authentication
        /// <summary>
        /// Gets the signature needed for making requests to 4Over API
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public string GetSignature(HTTP_Method M)
        {
            byte[] privateKeyBytes = Encoding.ASCII.GetBytes(PrivateKey);
            byte[] methodBytes = Encoding.ASCII.GetBytes(M.ToString());

            HashAlgorithm hash = new SHA256CryptoServiceProvider();

            byte[] privateKeyHash = hash.ComputeHash(privateKeyBytes);
            string privateKeyHex = ByteArrayToString(privateKeyHash); // Should be cb63675c0be505870060bda72ba5e0a80ed76613d7f3781eadb3e90bcb401006

            HMACSHA256 hash2 = new HMACSHA256(Encoding.ASCII.GetBytes(privateKeyHex));

            byte[] signatureBytes = hash2.ComputeHash(methodBytes);

            string signature = ByteArrayToString(signatureBytes); // Should be 998d7f498e35c08f682dda8965daa039af4dd44048998ddd05318f44c959f9da

            return signature;
        }



        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion

        public string GetUrl(string Operation, Dictionary<string, string> Query)
        {
            string url = URLBase;
            url += "/" + Operation + "?";

            url += "apikey=" + PublicKey + "&";
            url += "signature=" + GetSignature(HTTP_Method.GET) + "&";

            // 998d7f498e35c08f682dda8965daa039af4dd44048998ddd05318f44c959f9da

            if (Query != null)
            {
                foreach (KeyValuePair<string, string> kvp in Query)
                {
                    url += kvp.Key + "=" + kvp.Value + "&";
                }
            }
            url = url.TrimEnd('&');
            return url;
        }

        public string GetFromServer(string URL)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create(URL);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response;

            response = (HttpWebResponse)request.GetResponse();

            // Display the status.
            //Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        public string PostToServer(string Operation, string JSONData)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = URLBase;
            url += "/" + Operation;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            var data = Encoding.ASCII.GetBytes(SampleJSONData);

            request.Method = "POST";

            // Authorization: API {PUBLIC_KEY}:{SIGNATURE}
            request.Headers.Add("Authorization", "API " + PublicKey + ":" + GetSignature(HTTP_Method.POST));
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public string SampleJSONData = @"
JSON
{
    ""order_id"": ""test001"",
    ""is_test_order"": ""true"",
    ""coupon_code"": ""PRC5PKS"",
    ""skip_conformation"": ""true"",
    ""jobs"": [
        {
            ""product_uuid"": ""91efff4d-de64-410a-8794-fc547022a7f2"",
            ""runsize_uuid"": ""52e3d710-0e8f-4d4d-8560-7d4d8655be69"",
            ""option_uuids"": [],
            ""turnaroundtime_uuid"": ""50979118-b3a2-4556-9f46-d1da268f2354"",
            ""colorspec_uuid"": ""13abbda7-1d64-4f25-8bb2-c179b224825d"",
            ""dropship"": ""true"",
            ""sets"": 2,
            ""files"": {
                ""set_001"": {
                    ""job_name"": ""job001-001"",
                    ""files"": {
                        ""fr"": ""5a2e207c-8a97-42a3-9b17-1e4899eae9ac"",
                        ""bk"": ""d044410f-003e-4b32-8039-f6d590728f5b""
                    }
                },
                ""set_002"": {
                    ""job_name"": ""job001-002"",
                    ""files"": {
                        ""fr"": ""8b0fd851-7f13-4700-a35a-baff123eecfe"",
                        ""bk"": ""3d71bf4a-a799-4dfb-ac90-8fea5f97b608""
                    }
                }
            },
            ""ship_to"": {
                ""company"": ""My Print Company"",
                ""firstname"": ""James"",
                ""lastname"": ""Doe"",
                ""email"": ""james.doe@abcd_company.com"",
                ""phone"": ""(865) 222-5555"",
                ""address"": ""1200 Executive Park Drive"",
                ""address2"": """",
                ""city"": ""Knoxville"",
                ""state"": ""TN"",
                ""zipcode"": ""37923"",
                ""country"": ""US""
            },
            ""ship_from"": {
                ""company"": ""My Print Company"",
                ""firstname"": ""Mike"",
                ""lastname"": ""Doe"",
                ""email"": null,
                ""phone"": null,
                ""address"": ""1900 CUMMINGS CTR"",
                ""address2"": null,
                ""city"": ""BEVERLY"",
                ""state"": ""MA"",
                ""zipcode"": ""01915"",
                ""country"": ""US""
            },
            ""shipper"": {
                ""shipping_method"": ""FREE UPS Ground"",
                ""shipping_code"": ""03f""
            },
            ""ship_from_facility"": ""DAY""
        },
        {
            ""product_uuid"": ""91efff4d-de64-410a-8794-fc547022a7f2"",
            ""runsize_uuid"": ""52e3d710-0e8f-4d4d-8560-7d4d8655be69"",
            ""option_uuids"": [],
            ""turnaroundtime_uuid"": ""50979118-b3a2-4556-9f46-d1da268f2354"",
            ""colorspec_uuid"": ""13abbda7-1d64-4f25-8bb2-c179b224825d"",
            ""dropship"": ""true"",
            ""sets"": 3,
            ""files"": {
                ""set_001"": {
                    ""job_name"": ""job002-001"",
                    ""files"": {
                        ""fr"": ""62a7d70a-3b38-45a8-9327-e10a9c4057df"",
                        ""bk"": ""ef45e3ff-2f3c-415c-8c63-4f5e3f290e8d""
                    }
                },
                ""set_002"": {
                    ""job_name"": ""job002-002"",
                    ""files"": {
                        ""fr"": ""bad3efad7-2e72-4a5e-87ec-d8bad3ef0359"",
                        ""bk"": ""f9ead3ef0-b0de-4033-ad6d-aa15aa8c2555""
                    }
                },
                ""set_003"": {
                    ""job_name"": ""job002-003"",
                    ""files"": {
                        ""fr"": ""3830c3ee-72e4-46d7-8d0d-aad3ef77ccf1"",
                        ""bk"": ""bd380570-135d-47fd-9411-b3ead3ef99d8""
                    }
                }
            },
            ""ship_to"": {
                ""company"": ""My Test Company"",
                ""firstname"": ""Maria"",
                ""lastname"": ""Doe"",
                ""email"": ""maria.doe@abcd_company.com"",
                ""phone"": null,
                ""address"": ""1200 Executive Park Drive"",
                ""address2"": null,
                ""city"": ""Knoxville"",
                ""state"": ""TN"",
                ""zipcode"": ""37923"",
                ""country"": ""US""
            },
            ""ship_from"": {
                ""company"": ""My Print Company"",
                ""firstname"": ""Mike"",
                ""lastname"": ""Doe"",
                ""email"": null,
                ""phone"": null,
                ""address"": ""1900 CUMMINGS CTR"",
                ""address2"": null,
                ""city"": ""BEVERLY"",
                ""state"": ""MA"",
                ""zipcode"": ""01915"",
                ""country"": ""US""
            },
            ""shipper"": {
                ""shipping_method"": ""FREE UPS Ground"",
                ""shipping_code"": ""03f""
            },
            ""ship_from_facility"": ""DAY""
        },
        {
            ""job_name"": ""job003-001"",
            ""product_uuid"": ""91efff4d-de64-410a-8794-fc547022a7f2"",
            ""runsize_uuid"": ""52e3d710-0e8f-4d4d-8560-7d4d8655be69"",
            ""option_uuids"": [],
            ""turnaroundtime_uuid"": ""50979118-b3a2-4556-9f46-d1da268f2354"",
            ""colorspec_uuid"": ""13abbda7-1d64-4f25-8bb2-c179b224825d"",
            ""dropship"": ""True"",
            ""files"": {
                ""fr"": ""a8ad3ef5-e966-4b78-8107-31fc5f52b8d0"",
                ""bk"": ""c9876bdb-2ec2-4d0d-8a83-a0dad3eff27""
            },
            ""ship_to"": {
                ""company"": ""My Test Company"",
                ""firstname"": ""Maria"",
                ""lastname"": ""Doe"",
                ""email"": ""maria.doe@abcd_company.com"",
                ""phone"": null,
                ""address"": ""1200 Executive Park Drive"",
                ""address2"": null,
                ""city"": ""Knoxville"",
                ""state"": ""TN"",
                ""zipcode"": ""37923"",
                ""country"": ""US""
            },
            ""ship_from"": {
                ""company"": ""My Print Company"",
                ""firstname"": ""Mike"",
                ""lastname"": ""Doe"",
                ""email"": null,
                ""phone"": null,
                ""address"": ""1900 CUMMINGS CTR"",
                ""address2"": null,
                ""city"": ""BEVERLY"",
                ""state"": ""MA"",
                ""zipcode"": ""01915"",
                ""country"": ""US""
            },
            ""shipper"": {
                ""shipping_method"": ""FREE UPS Ground"",
                ""shipping_code"": ""03f""
            },
            ""ship_from_facility"": ""DAY""
        },
        {
            ""job_name"": ""job004-001"",
            ""product_uuid"": ""91efff4d-de64-410a-8794-fc547022a7f2"",
            ""runsize_uuid"": ""52e3d710-0e8f-4d4d-8560-7d4d8655be69"",
            ""option_uuids"": [],
            ""turnaroundtime_uuid"": ""50979118-b3a2-4556-9f46-d1da268f2354"",
            ""colorspec_uuid"": ""13abbda7-1d64-4f25-8bb2-c179b224825d"",
            ""dropship"": ""True"",
            ""files"": {
                ""fr"": ""ad3ef786-2349-490d-8c83-129bc462daf3"",
                ""bk"": ""84ad3ef-f58b-47a2-ad32-b04752ed14e2""
            },
            ""ship_to"": {
                ""company"": ""My Test Company"",
                ""firstname"": ""Maria"",
                ""lastname"": ""Doe"",
                ""email"": ""maria.doe@abcd_company.com"",
                ""phone"": null,
                ""address"": ""1200 Executive Park Drive"",
                ""address2"": null,
                ""city"": ""Knoxville"",
                ""state"": ""TN"",
                ""zipcode"": ""37923"",
                ""country"": ""US""
            },
            ""ship_from"": {
                ""company"": ""My Print Company"",
                ""firstname"": ""Mike"",
                ""lastname"": ""Doe"",
                ""email"": null,
                ""phone"": null,
                ""address"": ""1900 CUMMINGS CTR"",
                ""address2"": null,
                ""city"": ""BEVERLY"",
                ""state"": ""MA"",
                ""zipcode"": ""01915"",
                ""country"": ""US""
            },
            ""shipper"": {
                ""shipping_method"": ""FREE UPS Ground"",
                ""shipping_code"": ""03f""
            },
            ""ship_from_facility"": ""DAY""
        }
    ],
    ""payment"": {
        ""payment_provider"": {
            ""payment_provider_uuid"": ""6b0e699e-bdaf-4d1e-af4c-bc422ad21761""
        },
        ""requested_currency"": {
            ""currency_code"": ""USD""
        },
        ""credit_card"": {
            ""account_number"": ""4111111111111111"",
            ""month"": ""09"",
            ""year"": ""2016"",
            ""ccv"": ""123""
        },
        ""billing_info"": {
            ""first_name"": ""Mike"",
            ""last_name"": ""Doe"",
            ""address1"": ""1900 CUMMINGS CTR"",
            ""address2"": """",
            ""city"": ""BEVERLY"",
            ""state"": ""MA"",
            ""zip"": ""01915"",
            ""country"": ""US""
        },
        ""order_id"": ""test001"",
        ""comments"": ""This is a sample payment""
    }
}
";

        /// <summary>
        /// This route will return all Organizations associated with the authenticated user login. Each organization detail contains the resource URI to users, addresses and organization_roles associated with that organization.
        /// </summary>
        /// <returns></returns>
        public string GetOrganizations()
        {
            string operation = "organizations";
            string url = GetUrl(operation, null);

            string response = GetFromServer(url);

            return response;
        }

        //public string CreateOrder()
        //{

        //}

    }

}