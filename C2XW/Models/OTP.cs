using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2XW.Models
{
   
        public class Data
        {
            
            public string Id { get; set; }
            public string phoneNumber { get; set; }
            public int id_giaodich { get; set; }
            public int dichvu_gia { get; set; }
            public string dichvu_ten { get; set; }
        }

        public class OTPModel
        {
            
            public string Id { get; set; }
            public Data data { get; set; }
            public int stt { get; set; }
            public string msg { get; set; }
        }

        public class ListSm
        {
            
            public string Id { get; set; }
            public string sender { get; set; }
            public string number { get; set; }
            public int id { get; set; }
            public string smsContent { get; set; }
            public string fileRecord { get; set; }
            public string phoneNumber { get; set; }
            public string receiveTimeText { get; set; }
        }

        public class ListSmModel
        {
            
            public string Id { get; set; }
            public int stt { get; set; }
            public object msg { get; set; }
            public ListData data { get; set; }
        }

        public class ListData
        {
            [JsonProperty("$id")]
            public string Id { get; set; }
            public List<ListSm> listSms { get; set; }
            public int giaodich_id { get; set; }
            public int dichvu_dongia { get; set; }
            public string dichvu_ten { get; set; }
            public string phoneNum { get; set; }
            public int totalPrice { get; set; }
            public int number_sms { get; set; }
            public string createDate { get; set; }
            public int status { get; set; }
        }


}
