using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
    public class AD_ApplicationsBL
    {
        private AD_ApplicationsDL pd = new AD_ApplicationsDL();
        public List<AD_Applications> ToList(string filter, string value = null, string value2 = null)
        {

            DataTable dt = pd.Get(filter, value, value2);
            return DataTableToList(dt);


        }

        private List<AD_Applications> DataTableToList(DataTable dt)
        {
            List<AD_Applications> lstApps = new List<AD_Applications>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstApps.Add(DataTableToObject(dt, i));
                }
            }

            return lstApps;
        }

        private AD_Applications DataTableToObject(DataTable dt, int Row)
        {
            AD_Applications app = new AD_Applications();

            app.AppId = DataType.ToInt64(dt.Rows[Row]["AppId"].ToString());
            app.AppName = dt.Rows[Row]["AppName"].ToString();
            app.ModuleId = (dt.Columns.Contains("ModuleId")) ? DataType.ToInt64(dt.Rows[Row]["ModuleId"].ToString()) : 0;

            app.PackageName = dt.Rows[Row]["PackageName"].ToString();
            app.Version = dt.Rows[Row]["Version"].ToString();
            app.AppURL = dt.Rows[Row]["AppURL"].ToString();
           
            return app;
        }
    }
    public static class csvTsvFile
    {
        public static DataTable GetCSVData(string localDestination)
        {
            //Instantiating Data Table
            var dt = new DataTable();

            try
            {
                if (File.Exists(localDestination))
                {
                    using (StreamReader streamReader = new StreamReader(localDestination))
                    {
                        string[] headers = streamReader.ReadLine().Split(',');

                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header);
                        }

                        while (!streamReader.EndOfStream)
                        {
                            string[] rows = streamReader.ReadLine().Split(',');

                            if (rows.Length > 0)
                            {
                                DataRow dr = dt.NewRow();

                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dr[i] = rows[i].Trim();
                                }

                                dt.Rows.Add(dr);
                            }

                        }
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static DataTable GetTsvData(string localDestination)
        {
            //Instantiating Data Table
            var dt = new DataTable();

            try
            {
                if (File.Exists(localDestination))
                {
                    using (StreamReader streamReader = new StreamReader(localDestination))
                    {
                        string[] headers = streamReader.ReadLine().Split('\t');
                        int count = 0;
                        foreach (string header in headers)
                        {
                          var statua=  dt.Columns.Contains(header);
                            if (statua)
                            {
                                count++;
                                dt.Columns.Add(header + count);
                            }
                            else
                            {
                                dt.Columns.Add(header);
                            }
                        }

                        while (!streamReader.EndOfStream)
                        {
                            string[] rows = streamReader.ReadLine().Split('\t');

                            if (rows.Length > 0)
                            {
                                DataRow dr = dt.NewRow();
                                var cnt = rows.Length > headers.Length ? rows.Length-1 : rows.Length;
                                for (int i = 0; i < cnt; i++)
                                {
                                    dr[i] = rows[i].Trim();
                                }

                                dt.Rows.Add(dr);
                            }

                        }
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
