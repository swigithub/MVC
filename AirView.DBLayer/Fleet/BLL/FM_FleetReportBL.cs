using AirView.DBLayer.Fleet.DAL;
using AirView.DBLayer.Fleet.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.BLL
{
   
    public class FM_FleetReportBL
    {
        FM_FleetReportDL model = new FM_FleetReportDL();
        public List<String> GetDevice(string Filter)
        {
            try
            {
                List<string> str = new List<string>();
                DataTable dataTableModel = model.GetDevice(Filter);
                foreach(DataRow row in dataTableModel.Rows)
                {
                    foreach (DataColumn Col in dataTableModel.Columns)
                    {
                        str.Add(row[Col].ToString());
                    }
                }
                return str;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetPositioningReports(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_PositioningReports(frcmodel);
                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                var date = ListModel.Where(o=>o.Date!=null).Select(i => i.Date).ToList();
                var assetStatus = ListModel.Where(u => u.AssetStatus != null).Select(a => a.AssetStatus).ToList();
                if (date.Count > 0)
                {
                    var x = ListModel.Select(y => { y.Date = DateTime.Parse(y.Date).ToString("MM-dd-yyyy"); return y; });
                    return x.ToList();
                }
                else if (assetStatus.Count > 0)
                {
                    foreach(var item in ListModel)
                    {
                        if(item.AssetStatus == "True")
                        {
                            item.AssetStatus = "On";
                        }
                        else
                        {
                            item.AssetStatus = "Off";
                        }
                    }
                    return ListModel.ToList();
                }
                else
                {
                    return ListModel.ToList();
                }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetSpeedReports(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_SpeedReports(frcmodel);
                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                

                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetAssetStatus(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_AssetStatus(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                var assetStatus = ListModel.Where(u => u.EngineStatus != null).Select(a => a.AssetStatus).ToList();
                var doorStatus = ListModel.Where(u => u.DoorStatus != null).Select(a => a.DoorStatus).ToList();
                var vibrationSensorState = ListModel.Where(u => u.VibrationSensorState != null).Select(a => a.VibrationSensorState).ToList();

                if (assetStatus.Count > 0)
                {
                    foreach (var item in ListModel)
                    {
                        if (item.EngineStatus == "True")
                        {
                            item.EngineStatus = "On";
                        }
                        else
                        {
                            item.EngineStatus = "Off";
                        }
                    }
                    return ListModel.ToList();
                }
                else if (doorStatus.Count > 0)
                {
                    foreach (var item in ListModel)
                    {
                        if (item.DoorStatus == "True")
                        {
                            item.DoorStatus = "On";
                        }
                        else
                        {
                            item.DoorStatus = "Off";
                        }
                    }
                    return ListModel.ToList();
                }
                else if (vibrationSensorState.Count > 0)
                {
                    foreach (var item in ListModel)
                    {
                        if (item.VibrationSensorState == "True")
                        {
                            item.VibrationSensorState = "SensorOn";
                        }
                        else
                        {
                            item.VibrationSensorState = "SensorOff";
                        }
                    }
                    return ListModel.ToList();
                }
                else
                {

                }
                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetMileageReport(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_MileageReport(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetFuelReport(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_FuelReport(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetTempratureReport(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_TemperatureReport(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetDetailedAlarmReport(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_DetailedAlarmReport(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                var assetStatus = ListModel.Where(u => u.AssetStatus != null).Select(a => a.AssetStatus).ToList();
                if (assetStatus.Count > 0)
                {
                    foreach (var item in ListModel)
                    {
                        if (item.AssetStatus == "True")
                        {
                            item.AssetStatus = "On";
                        }
                        else
                        {
                            item.AssetStatus = "Off";
                        }
                    }
                    return ListModel.ToList();
                }
                else
                {
                    return ListModel.ToList();
                }

                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_PositioningReport> GetGeoFenceReport(FleetRptCriteria frcmodel)
        {
            try
            {
                DataTable dataTableModel = model.Get_GeoFenceReport(frcmodel);

                List<FM_PositioningReport> ListModel = dataTableModel.ToList<FM_PositioningReport>();
                return ListModel.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
