using AirView.DBLayer.KPI.VM;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AirView.DBLayer.Project.BLL
{


    public class PM_KPI_BL
    {
        public PM_KPI_DL KPI_DL = new PM_KPI_DL();
        public List<Model.PM_KPI> KPIList(string Filter, Int64 value = 0)
        {
            try
            {
                DataTable dt = KPI_DL.GetDataTable(Filter, Convert.ToString(value));
                return dt.ToList<Model.PM_KPI>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<Model.PM_KPI> KPIList(string Filter, string value = "")
        {
            try
            {
                DataTable dt = KPI_DL.GetDataTable(Filter, Convert.ToString(value));
                return dt.ToList<Model.PM_KPI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Model.PM_Threshold> ThresholdList(string Filter, Int64 value= 0)
        {
            try
            {
                DataTable dt = KPI_DL.GetDataTable(Filter, Convert.ToString(value));
                return dt.ToList<Model.PM_Threshold>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AD_Defination> DefinationList(string Filter, Int64 value = 0)
        {
            try
            {
                DataTable dt = KPI_DL.GetDataTable(Filter, Convert.ToString(value));
                return dt.ToList<AD_Defination>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Insert(string filter, List<PM_KPI> wo, string UserId)
        {
            try
            {
                PM_KPI_DL KPI_DL = new PM_KPI_DL();
                dbDataTable ddt = new dbDataTable();
                DataTable dt = ddt.List();
                if (wo != null)
                {
                    foreach (var item in wo)
                    {
                        if (item.IsActive == true || item.KPI !=0)
                        {
                            myDataTable.AddRow(dt, "Value1", item.KPI, "Value2", item.Kpi_Name, "Value3", item.Kpi_Type, "Value4", item.Level, "Value5", item.Technology, "Value6", item.DataType, "Value7", item.TaskId, "Value8", item.IsActive, "Value9", item.Formula, "Value10", item.BandId, "Value11", item.Weightage);
                        }
                    
                    }
                }
                return KPI_DL.Manage(filter, dt, UserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool Insert(string filter, List<PM_Threshold> th, string UserId)
        {
            try
            {
                PM_KPI_DL KPI_DL = new PM_KPI_DL();
                dbDataTable ddt = new dbDataTable();
                DataTable dt = ddt.List();
                if (th != null)
                {
                    foreach (var item in th)
                    {
                      if(item.IsActive == true || item.Threshold !=0)
                        {
                            myDataTable.AddRow(dt, "Value1", item.KPI, "Value2", item.Condition, "Value3", item.Threshold_Name, "Value4", item.ThresholdTo, "Value5", item.Color, "Value6", item.Action, "Value7", item.IsActive, "Value8", item.Threshold
                   );
                        }
                    }
                }

                return KPI_DL.Manage(filter, dt, UserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region KPI Data
        public bool Insert(string filter, List<KPIDATA_VM> wo)
        {
            try
            {
                PM_KPI_DL KPI_DL = new PM_KPI_DL();
                dbDataTable ddt = new dbDataTable();
                DataTable dt = ddt.List();
                if (wo != null)
                {
                    foreach (var item in wo)
                    {

                        myDataTable.AddRow(dt, "Value1", item.KPIData_Id, "Value2", item.KPI_Id, "Value3", Convert.ToString(item.KPI_Value[0]), "Value4", item.Date, "Value5", item.Site,"Value6",item.Carrier,"Value7",item.Sector);


                    }
                }
                return KPI_DL.Manage(filter, dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<KPIDATA_VM> KPIData(string Filter, Int64 value = 0,string SDate="",string EDate="", Int64 Site = 0, Int64 Carrier = 0, Int64 Sector = 0)
        {
            try
            {
                DataTable dt = KPI_DL.GetDataTable(Filter, Convert.ToString(value), SDate,EDate,Site,Carrier,Sector);
                return dt.ToList<KPIDATA_VM>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
