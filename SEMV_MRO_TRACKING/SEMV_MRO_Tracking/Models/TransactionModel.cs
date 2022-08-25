using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEMV_MRO_Tracking.Models
{
    public class TransactionModel
    {
        public string Order_ID { get; set; }
        public string Part_Number { get; set; }
        public float Quantity_Req { get; set; }
        public string Request_By { get; set; }
        public string Request_Type { get; set; }
        public string Remark { get; set; }
        public string Timestamp { get; set; }

    }
}
