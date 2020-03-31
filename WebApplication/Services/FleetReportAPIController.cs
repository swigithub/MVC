using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AirView.DBLayer.Fleet.Model;
using AirView.DBLayer.Fleet.BLL;

namespace WebApplication.Services
{
    public class FleetReportAPIController : ApiController
    {
        FM_FleetReportBL model = new FM_FleetReportBL();

        [Route("swi/Fleet/GetDevice"), HttpGet]
        public HttpResponseMessage GetDevice(string Filter = "Get_Device")
        {
            
            List<string> modelResult = new List<string>();

            modelResult = model.GetDevice(Filter);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_PositioningStatement"),HttpPost]
        public HttpResponseMessage PositioningStatement(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetPositioningReports(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_SpeedReports"), HttpPost]
        public HttpResponseMessage SpeedReports(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetSpeedReports(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_AssetStatus"), HttpPost]
        public HttpResponseMessage AssetStatus(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetAssetStatus(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_MileageReport"), HttpPost]
        public HttpResponseMessage MileageReport(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetMileageReport(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FuelReport"), HttpPost]
        public HttpResponseMessage FuelReport(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetFuelReport(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_TempratureReport"), HttpPost]
        public HttpResponseMessage TempratureReport(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetTempratureReport(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_DetailedAlarmReport"), HttpPost]
        public HttpResponseMessage DetailedAlarmReport(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetDetailedAlarmReport(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_GeoFenceReport"), HttpPost]
        public HttpResponseMessage GeoFenceReport(FleetRptCriteria frcmodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_PositioningReport> modelResult = new List<FM_PositioningReport>();

            modelResult = model.GetGeoFenceReport(frcmodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);

        }
    }
    }
