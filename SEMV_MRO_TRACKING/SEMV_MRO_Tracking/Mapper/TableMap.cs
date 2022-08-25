using CsvHelper.Configuration;
using SEMV_MRO_Tracking.Models;

namespace SEMV_MRO_Tracking.Mapper
{
    public sealed class MaterialMap : ClassMap<MaterialModel>
    {
        public MaterialMap()
        {
            Map(x => x.Part_Number).Name("Part_Number");
            Map(x => x.Barcode).Name("Barcode");
            Map(x => x.Unit).Name("Unit");
            Map(x => x.Material_Name).Name("Material_Name");
            Map(x => x.Material_Desc).Name("Material_Desc");
            Map(x => x.Leadtime).Name("Leadtime");
            Map(x => x.Stock).Name("Stock");
            Map(x => x.Safety_Stock).Name("Safety_Stock");
            Map(x => x.Max_Stock).Name("Max_Stock");
            Map(x => x.Total_Purchase).Name("Total_Purchase");
            Map(x => x.Price).Name("Price");
            Map(x => x.Supplier_Name).Name("Supplier_Name");
            Map(x => x.Supplier_Email).Name("Supplier_Email");
        }
    }
}
