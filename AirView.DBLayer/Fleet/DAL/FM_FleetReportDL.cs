using AirView.DBLayer.Fleet.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.DAL
{
    public class FM_FleetReportDL
    {
        public DataTable GetDevice(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetFleetReporting");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_PositioningReports(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetFleetReporting");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate,
                        "@endDate", frcmodel.endDate,"@Device", frcmodel.Device,"@Latitude",frcmodel.Latitude,"@Longitude",frcmodel.Longitude,
                        "@Radius",frcmodel.Radius,"@startWork",frcmodel.startWork,"@endWork",frcmodel.endWork,"@ParkingTime", frcmodel.parkingTime, "@DrivingTime",frcmodel.drivingTime));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_SpeedReports(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetSpeedReporting");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device",
                    frcmodel.Device, "@Speed", frcmodel.Speed, "@DriveOverHour",frcmodel.Driveoverhour, "@NoParkRest",frcmodel.Noparkrest, "@IdleSpeedMoreThan",frcmodel.Idlespeedmorethan));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_AssetStatus(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetAssetStatus");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device ));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_MileageReport(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetMileageReport");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device, "@FuelConsumption", frcmodel.FuelConsumption, "@FuelPrice",frcmodel.FuelPrice));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_FuelReport(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetFuelReport");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_TemperatureReport(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetTempratureReport");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device, "@startTemp",frcmodel.startTemprature, "@endTemp",frcmodel.endTemprature));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_DetailedAlarmReport(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetDetailAlarmReport");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_GeoFenceReport(FleetRptCriteria frcmodel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetGeoFenceReport");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", frcmodel.ReportFilter, "@startDate", frcmodel.startDate, "@endDate", frcmodel.endDate, "@Device", frcmodel.Device));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

    }
}
