using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEMV_MRO_Tracking.Models
{
    public class MaterialModel
    {
        public int Id { get; set; }
        public string Part_Number { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public string Material_Name { get; set; }
        public string Material_Desc { get; set; }
        public float Stock { get; set; }
        public float Safety_Stock { get; set; }
        public string Leadtime { get; set; }
        public string Price { get; set; }
        public string TotalValue  { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Email { get; set; }
        public string Stock_Status { get; set; }
        public float Max_Stock { get; set; }
        public float Total_Purchase { get; set; }
        public string Timestamp { get; set; }
    }
}
