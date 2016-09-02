using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4OverAPI
{
    public class FourOverJob
    {
        public FourOverJob()
        {
            this.files = new FilesDict();
            this.ship_to = new FourOverAddress.ShipAddress();
            this.ship_from = new FourOverAddress.ShipAddress();
            this.shipper = new Shipper();
            this.option_uuids = new List<object>();
        }
        public string product_uuid { get; set; }
        public string runsize_uuid { get; set; }
        public List<object> option_uuids { get; set; }
        public string turnaroundtime_uuid { get; set; }
        public string colorspec_uuid { get; set; }
        public bool dropship { get; set; }
        public int sets { get; set; }

        /// <summary>
        /// Keys must be named as follows: set_001, set_002, set_003...
        /// </summary>
        public FilesDict files { get; set; }
        public FourOverAddress.ShipAddress ship_to { get; set; }
        public FourOverAddress.ShipAddress ship_from { get; set; }
        public Shipper shipper { get; set; }
        public string ship_from_facility { get; set; }

        public class FilesDict : Dictionary<string, FileSet>
        {
            public FilesDict()
            {
            }
            public void AddFile (FileSet F)
            {
                int index = this.Count + 1;
                string indexStr = index.ToString().PadLeft(3, '0');
                this.Add("set_" + indexStr, F);
            }  
        }
        public class Files
        {
            public string fr { get; set; }
            public string bk { get; set; }
        }

        public class FileSet
        {
            public string job_name { get; set; }
            public Files files { get; set; }

            public FileSet()
            {
                files = new Files();
            }

            public FileSet(string FrontFileUUID, string BackFileUUID)
            {
                files = new Files()
                {
                    fr = FrontFileUUID,
                    bk = BackFileUUID
                };
            }
        }



        public class Shipper
        {
            public string shipping_method { get; set; }
            public string shipping_code { get; set; }
        }


    }
}