using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Common
{
    /*----MoB!----*/
    public class dbDataTable
    {
        public DataTable Script()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "Value20", "Value21", "Value22", "Value23", "Value24", "Value25");
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DataTable List()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15");
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DataTable Survey_List()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15",
                                          "Value16", "Value17", "Value18", "Value19", "Value20");
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DataTable TaskList()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15",
                                          "Value16", "Value17", "Value18", "Value19", "Value20", "Value21", "Value22", "Value23", "Value24",
                                          "Value25", "Value26", "Value27", "Value28", "Value29", "Value30", "Value31", "Value32", "Value33"
                                          );
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DataTable PM_ImportList()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "Value20");
            }
            catch (Exception)
            {

                return null;
            }
        }

        public DataTable PM_ImportLists()
        {
            try
            {
                return myDataTable.Create("Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10",
                                          "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "Value20",
                                          "Value21", "Value22", "Value23", "Value24", "Value25", "Value26", "Value27", "Value28", "Value29", "Value30",
                                          "Value31", "Value32", "Value33", "Value34", "Value35", "Value36", "Value37", "Value38", "Value39", "Value40",
                                          "Value41", "Value42", "Value43", "Value44", "Value45");
            }
            catch (Exception)
            {

                return null;
            }
        }

        public DataTable ListProjectSites()
        {
            try
            {
                return myDataTable.Create("WoRefId", "ProjectId", "SiteCode", "SiteName", "SiteDate", "SiteTypeId", "SiteClassId", "Latitude", "Longitude", "RevisionId",
                    "PMCode", "PMRefId", "ClusterId", "ClusterCode", "CityId", "StatusId", "MSWindowId", "PriorityId", "ColorId", "CreatedOn",
                    "CreatedBy", "IsActive", "BudgetCost", "ActualCost", "Description", "ClientId", "ScopeId", "ReceivedOn", "Address", "PlannedDate",
                    "TargetDate", "ActualStartDate", "ActualEndDate", "EstimatedStartDate", "EstimatedEndDate", "MilestoneId", "StageId", "FACode",
                    "USID", "CommonId", "MarketId", "SubMarketId", "CountyId", "vMME", "ControlledIntro", "SuperBowl", "isDASInBuild", "FirstNetRAN",
                    "IPlanJob", "PaceNo", "IPlanIssueDate", "SubMarket", "County", "AlarmId", "Value1", "Value2", "Value3", "Value4", "Value5",
                    "Value6", "Value7", "Value8", "Value9", "Value10");
            }
            catch (Exception)
            {

                return null;
            }
        }

        public DataTable Tb_AV_Workorder()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[46]
                {
                    new DataColumn("clusterCode",  typeof (string)),
                    new DataColumn("Region", typeof (string)),
                    new DataColumn("Market", typeof (string)),
                    new DataColumn("Client", typeof (string)),
                    new DataColumn("siteCode", typeof (string)),
                    new DataColumn("siteLatitude", typeof (string)),
                    new DataColumn("siteLongitude", typeof (string)),
                    new DataColumn("Description", typeof (string)),
                    new DataColumn("sectorCode", typeof (string)),
                    new DataColumn("networkMode", typeof (string)),
                    new DataColumn("Scope", typeof (string)),
                    new DataColumn("Band", typeof (string)),
                    new DataColumn("Carrier", typeof (string)),
                    new DataColumn("Antenna", typeof (string)),
                    new DataColumn("BeamWidth", typeof (string)),
                    new DataColumn("Azimuth", typeof (string)),
                    new DataColumn("PCI", typeof (string)),
                    new DataColumn("ReceivedOn", typeof (DateTime)),
                    new DataColumn("BandWidth", typeof (string)),
                    new DataColumn("CellId", typeof (string)),
                    new DataColumn("RFHeight", typeof (string)),
                    new DataColumn("MTilt", typeof (string)),             
                    new DataColumn("ETilt", typeof (string)),
                    new DataColumn("SiteName", typeof (string)),
                    new DataColumn("SiteTypeId", typeof (string)),
                    new DataColumn("SiteClassId", typeof (string)),
                    new DataColumn("SiteAddress", typeof (string)),
                    new DataColumn("MRBTS", typeof (string)),
                    new DataColumn("RevisionId", typeof (int)),
                    new DataColumn("RedriveCount", typeof (int)),
                    new DataColumn("SiteId", typeof (Int64)),
                    new DataColumn("SectorLatitude", typeof (string)),
                    new DataColumn("SectorLongitude", typeof (string)),
                    new DataColumn("clusterName", typeof (string)),
                    new DataColumn("CellFilePath", typeof (string)),
                    new DataColumn("SurveyId", typeof (string)),
                    new DataColumn("ProjectId", typeof (Int64)),
                    new DataColumn("SiteSurveyId", typeof (Int64)),
                    new DataColumn("SectorId", typeof (Int64)),
                    new DataColumn("SiteClusterId", typeof (Int64)),
                    new DataColumn("VerticalBeamWidth", typeof (Int64)),
                    new DataColumn("AntennaDowntilt", typeof (string)),     
                    // new Added
                    new DataColumn("Checklist",  typeof (string)),
                    new DataColumn("Project",  typeof (string)),
                    new DataColumn("SiteClass",  typeof (string)),
                    new DataColumn("SiteType",  typeof (string))

            });

                #endregion
                return wodt;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public DataTable Tb_PM_Target()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[6]
                {
                    new DataColumn("Project",  typeof (string)),
                    new DataColumn("MileStone", typeof (string)),
                    new DataColumn("Stage", typeof (string)),
                    new DataColumn("TargetType", typeof (string)),
                    new DataColumn("TargetValue", typeof (string)),
                    new DataColumn("SiteCount", typeof (string))

            });

                #endregion
                return wodt;
            }
            catch (Exception)
            {

                return null;
            }
        }


        // Import Project Plan Data:: Phase 1
        public DataTable Tb_ImportProjectPlant()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[8]
                {
                    new DataColumn("FACode",  typeof (string)),
                    new DataColumn("Milestone", typeof (string)),
                    new DataColumn("Stage", typeof (string)),
                    new DataColumn("Plan", typeof (DateTime)),
                    new DataColumn("Forecast", typeof (DateTime)),
                    new DataColumn("Target", typeof (DateTime)),
                    new DataColumn("Actual", typeof (DateTime)),
                    new DataColumn("Status", typeof (string))
                    //new DataColumn("OldPlan", typeof (string)),
                    //new DataColumn("OldForecast", typeof (string)),
                    //new DataColumn("OldTarget", typeof (string)),
                    //new DataColumn("OldActual", typeof (string)),
                    //new DataColumn("OldStatus", typeof (string))
                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Tb_ImportWR_Issues()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[15]
                {
                    new DataColumn("FACode",  typeof (string)),
                    new DataColumn("eNB", typeof (string)),
                    new DataColumn("othereNB", typeof (string)),
                    new DataColumn("Schedule", typeof (string)),
                    new DataColumn("Actual", typeof (string)),
                    new DataColumn("MW", typeof (string)),
                    new DataColumn("Status", typeof (string)),
                    new DataColumn("Alarm", typeof (string)),
                    new DataColumn("Issues", typeof (string)),
                    new DataColumn("WhoFix", typeof (string)),
                    new DataColumn("Notes", typeof (string)),
                    new DataColumn("ContentType", typeof (string)),
                    new DataColumn("Attachments", typeof (string)),
                    new DataColumn("Created", typeof (string)),
                    new DataColumn("CreatedBy", typeof (string))

                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Tb_ImportWR_Site()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[17]
                {
                    new DataColumn("FACode",  typeof (string)),
                    new DataColumn("eNB", typeof (string)),
                    new DataColumn("othereNB", typeof (string)),
                    new DataColumn("Reasons", typeof (string)),
                    new DataColumn("Schedule", typeof (string)),
                    new DataColumn("Actual", typeof (string)),
                    new DataColumn("MW", typeof (string)),
                    new DataColumn("Status", typeof (string)),
                    new DataColumn("Alarm", typeof (string)),
                    new DataColumn("Issues", typeof (string)),
                    new DataColumn("Notes", typeof (string)),
                    new DataColumn("AddlNotes", typeof (string)),
                    new DataColumn("NetAct", typeof (string)),
                    new DataColumn("USID", typeof (string)),
                    new DataColumn("ContentType", typeof (string)),
                    new DataColumn("Created", typeof (string)),
                    new DataColumn("CreatedBy", typeof (string))

                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }



        // Import Project Plan Data Phase 2

        //FA_Code USID    Common ID   REGION MARKET  SUB Market  Site Name   Street_Address CITY    State ZIP COUNTY Latitude    Longitude vMME   
        //Controlled Introduction Super Bowl  Site_Type DAS_or_Inbuilding   FirstNet RAN    iPlan Job   iPlan Status    iPlan Issue Date PACE Number TSS_Plan  
        //TSS_Forecast TSS_Submitted   Site_Specific_Material_Available_Forecast Site_Specific_Material_Available_Actual Pre_Install_Planned Pre_Install_Fcst   
        //Pre_Install_Actual Mig_Date_Planned    Mig_Date_Forecast Migration Date EPL Ordered EPL Called Out  EPL Delivered   EPL Status
        public DataTable Tb_MasterEx()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[10]
                {
                    new DataColumn("FACode",  typeof (string)),
                    new DataColumn("Task",  typeof (string)),
                     new DataColumn("PlanDate",  typeof (string)),
                      new DataColumn("ForecastStartDate",  typeof (string)),
                       new DataColumn("ForecastEndDate",  typeof (string)),
                        new DataColumn("TargetDate",  typeof (string)),
                         new DataColumn("ActualStartDate",  typeof (string)),
                          new DataColumn("ActualEndDate",  typeof (string)),
                           new DataColumn("Status",  typeof (string)),
                            new DataColumn("Resources",  typeof (string))
                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Tb_ImportWR_Ex()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
            
                wodt.Columns.AddRange(new DataColumn[18]
                {
                    new DataColumn("FA Code",  typeof (string)),
                    new DataColumn("Activity Type", typeof (string)),
                    new DataColumn("Alarms", typeof (string)),
                    new DataColumn("GNG", typeof (string)),
                    new DataColumn("Scheduled", typeof (string)),
                    new DataColumn("Attachment", typeof (string)),
                    new DataColumn("MW", typeof (string)),
                    new DataColumn("Attachment Type", typeof (string)),
                    new DataColumn("Notes", typeof (string)),
                    new DataColumn("eNB", typeof (string)),
                    new DataColumn("ExtendedENB", typeof (string)),
                    new DataColumn("EquipmentId", typeof (string)),
                    new DataColumn("AOTSCR", typeof (string)),
                    new DataColumn("USID", typeof (string)),
                    new DataColumn("Status", typeof (string)),
                    new DataColumn("IsAdditional", typeof (string)),
                    new DataColumn("Created", typeof (string)),
                    new DataColumn("Created By", typeof (string))


                    //FACode eNB othereNB   SiteName   CC  Market
                    //Schedule    Actual  MW  Status  Alarm
                    //Issues  WhoFix Notes   AddlNotes  NetAct
                    //Effort  Migrated    TicketRequest  TechName   TechNumber
                    //TechE-mail USID    ExpirationDate Notification    ContentType
                    //AppCreatedBy  AppModifiedBy WorkflowInstanceID    FileType   ModifiedOn
                    //PMO Created CreatedBy



                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Tb_ImportWR_Fail()
        {
            try
            {
                DataTable wodt = new DataTable();
                #region DataTable Columns
                wodt.Columns.AddRange(new DataColumn[27]
                {
                     new DataColumn("FACode",  typeof (string)),
                     new DataColumn("Task", typeof (string)),
                    new DataColumn("eNB", typeof (string)),
                   
                    new DataColumn("ExtendedENB", typeof (string)),
                    new DataColumn("EquipmentId", typeof (string)),
                    new DataColumn("AOTSCR", typeof (string)),

                    new DataColumn("Category", typeof (string)),
                    new DataColumn("Type", typeof (string)),
                    new DataColumn("WhoFix", typeof (string)),
                    new DataColumn("IsUnavoidable", typeof (string)),
                    new DataColumn("ActivityType", typeof (string)),

                    new DataColumn("Alarms", typeof (string)),
                    new DataColumn("Severity", typeof (string)),
                    new DataColumn("MW", typeof (string)),
                    new DataColumn("AttachmentType", typeof (string)),
                    new DataColumn("Attachment", typeof (string)),

                    new DataColumn("Description",  typeof (string)),
                    new DataColumn("Status", typeof (string)),
                    new DataColumn("AssignedTo", typeof (string)),
                    new DataColumn("Priority", typeof (string)),
                    new DataColumn("ScheduleDate", typeof (string)),

                    new DataColumn("ActualDate", typeof (string)),
                    new DataColumn("TargetDate", typeof (string)),
                    new DataColumn("RequestedBy", typeof (string)),
                    new DataColumn("RequestDate", typeof (string)),
                     new DataColumn("CreateDate", typeof (string)),
                    new DataColumn("CreatedBy", typeof (string)),
      
      
                    //eNB FACode othereNB   SiteName   CC  
                    //Market  Schedule    Actual  MW  Status  Alarm
                    //Issues  WhoFix Notes   ContentType    AppCreatedBy 
                    //AppModifiedBy Attachments WorkflowInstanceID
                    //FileType   PMO Modified

                });
                #endregion
                return wodt;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public DataTable Tbl_AV_SiteTestSummary()
        {
            try
            {
                DataTable Summery = new DataTable();
                #region DataTable
                Summery.Columns.AddRange(new DataColumn[56]
                {
                                    new DataColumn("SummaryId", typeof (int)),
                                    new DataColumn("LatencyRate", typeof (float)),
                                    new DataColumn("DownlinkRate", typeof (float)),
                                    new DataColumn("UplinkRate", typeof (int)),
                                    new DataColumn("DownlinkMaxResult", typeof (double)),
                                    new DataColumn("UplinkMaxResult", typeof (double)),
                                    new DataColumn("PingAverageResult", typeof (double)),
                                    new DataColumn("OoklaDownlinkResult", typeof (double)),
                                    new DataColumn("OoklaUplinkResult", typeof (double)),
                                    new DataColumn("OoklaPingResult", typeof (double)),
                                    new DataColumn("MoStatus"),
                                    new DataColumn("MtStatus"),
                                    new DataColumn("VMoStatus"),
                                    new DataColumn("VMtStatus"),
                                    
                                    new DataColumn("CwHandoverStatus"),
                                    new DataColumn("Ccwhandoverstatus"),
                                    new DataColumn("ICwHandoverStatus"),
                                    new DataColumn("ICcwhandoverstatus"),
                                    new DataColumn("OoklaTestFilePath", typeof (string)),
                                    new DataColumn("OoklaRssi", typeof (float)),
                                    new DataColumn("OoklaSinr", typeof (float)),
                                    new DataColumn("TestLatitude", typeof (float)),
                                    new DataColumn("TestLongitude", typeof (float)),
                                    new DataColumn("TestRssi", typeof (float)),
                                    new DataColumn("TestSinr", typeof (float)),
                                    new DataColumn("StationaryTestFilePath", typeof (string)),
                                    new DataColumn("CwTestFilePath", typeof (string)),
                                    new DataColumn("CcwTestFilePath", typeof (string)),
                                    new DataColumn("PingStatus"),
                                    new DataColumn("DownlinkStatus"),
                                    new DataColumn("UplinkStatus"),
                                    new DataColumn("E911Status"),
                                    new DataColumn("IsE911Performed"),
                                    new DataColumn("MimoStatus"),
                                     new DataColumn("SMoStatus"),
                                     new DataColumn("SMtStatus"),
                                     new DataColumn("MimoTestFilePath", typeof (string)),
                                     new DataColumn("SpeedTestFilePath", typeof (string)),
                                     new DataColumn("CaActiveTestFilePath", typeof (string)),
                                     new DataColumn("CaDeavticeTestFilePath", typeof (string)),
                                     new DataColumn("CaSpeedTestFilePath", typeof (string)),
                                     new DataColumn("LaaSpeedTestFilePath", typeof (string)),
                                     new DataColumn("LaaSmTestFilePath", typeof (string)),
                                    new DataColumn("PhyDLSpeedMin", typeof (float)),
                                    new DataColumn("PhyDLSpeedMax", typeof (float)),
                                    new DataColumn("PhyDLSpeedAvg", typeof (float)),
                                    new DataColumn("PhyULSpeedMin", typeof (float)),
                                    new DataColumn("PhyULSpeedMax", typeof (float)),
                                    new DataColumn("PhyULSpeedAvg", typeof (float)),
                                    new DataColumn("IntraHOInteruptTime", typeof (float)),
                                    new DataColumn("callSetupTime", typeof (float)),
                                    new DataColumn("IntreHOInteruptTime", typeof (float)),
                                   new DataColumn("PhyDLStatus"),
                                   new DataColumn("PhyULStatus"),
                                      new DataColumn("CADLSpeed"),
                                   new DataColumn("CAULSpeed"),


                });

                #endregion
                return Summery;
            }
            catch (Exception)
            {

                return null;
            }
        }



    }
}
