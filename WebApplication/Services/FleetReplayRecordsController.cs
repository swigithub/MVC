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
    public class FleetReplayRecordsController : ApiController
    {
        FM_VehicleBL model = new FM_VehicleBL();

        [Route("swi/Fleet/Get_FleetIdleEngineReportReplay"), HttpPost]
        public HttpResponseMessage FleetIdleEngineReportReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }

        [Route("swi/Fleet/Get_FleetParkingReportReplay"), HttpPost]
        public HttpResponseMessage FleetParkingReportReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetDrivingReportReplay"), HttpPost]
        public HttpResponseMessage FleetDrivingReportReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetMileageReportReplay"), HttpPost]
        public HttpResponseMessage FleetMileageReportReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetFatiguetDrivingReplay"), HttpPost]
        public HttpResponseMessage FleeFatiguetDrivingReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetAlarmsReplay"), HttpPost]
        public HttpResponseMessage FleeAlarmsReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetDetailTrackReplay"), HttpPost]
        public HttpResponseMessage FleeDetailTrackReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_FleetReplay_VM> modelResult = new List<FM_FleetReplay_VM>();

            modelResult = model.GetFleetReplayRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
        [Route("swi/Fleet/Get_FleetTripReplay"), HttpPost]
        public HttpResponseMessage FleeTripReplay(FleetReplayCriteria fleetreplaymodel)
        {
            //List<FM_PositioningReport> LstPositionReport = new List<FM_PositioningReport>();
            List<FM_TrackerTrip> modelResult = new List<FM_TrackerTrip>();

            modelResult = model.GetFleetReplayTripRecord(fleetreplaymodel);
            //LstPositionReport.Add(modelResult);                        
            // in your case this will be result of some service method and then
            if (modelResult == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, modelResult);
        }
    }
}
