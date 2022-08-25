using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using SEMV_MRO_Tracking.Mapper;
using SEMV_MRO_Tracking.Models;

namespace SEMV_MRO_Tracking.Function
{
    public class CSVService
    {
        public List<MaterialModel> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.RegisterClassMap<MaterialMap>();
                    var records = csv.GetRecords<MaterialModel>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        //Write Out a CSV file
        public void WriteCSVFile(string path, List<MaterialModel> student)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw))
            {
                cw.WriteHeader<MaterialModel>();
                cw.NextRecord();
                foreach (MaterialModel stu in student)
                {
                    cw.WriteRecord<MaterialModel>(stu);
                    cw.NextRecord();
                }
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

    }
}
