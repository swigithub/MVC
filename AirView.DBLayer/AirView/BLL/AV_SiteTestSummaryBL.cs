using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AirView.DBLayer.AirView.BLL
{
    /*----MoB!----*/
    public class AV_SiteTestSummaryBL
    {
        AV_SiteTestSummaryDL sd = new AV_SiteTestSummaryDL();

        Conversion cnvtr = new Conversion();

        public string ExtractHexDigits(string input)
        {
            // remove any characters that are not digits (like #)
            Regex isHexDigit
               = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            string newnum = "";
            foreach (char c in input)
            {
                if (isHexDigit.IsMatch(c.ToString()))
                    newnum += c.ToString();
            }
            return newnum;
        }
        public List<AV_SiteTestSummary> ToList(int SiteId, int BandId, int Carrier, int NetworkMode, Int64 UserId,ref List<AV_SiteTestLog> siteTestLog, ref List<SiteReportPlotVM> ReportPlot, ref List<AD_ReportConfiguration> rptConf, ref List<AV_MarketSites> MarketSites, DateTime plotDate,ref bool AfterDate, string kmlFilePath,ref List<TempData> ServerTimeStamp, ref List<AV_RFPlotLegends> RFPlotLegends , ref List<AV_FloorPlan> FloorPlans, ref List<AV_BeamTestLog> BTLog,ref List<BeamTestLegend> BTLegend ,ref List<RDACounts> RDACounts,ref List<OoklaDLSiteLevels> OoklaDLSiteLevels)
        {
            DataSet ds = sd.Get(SiteId, BandId, Carrier, NetworkMode, UserId, plotDate);

            List<AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataTable Plots = ds.Tables[2];
                //  DataTable ccwPlots = ds.Tables[2];
                string SiteStatus = string.Empty;
                SiteReportPlotVM plot;
                DateTime SiteScheduleDate = DateTime.Now;
                string NetLayer = string.Empty;
                bool MakeKml = true;
                string ClientPrefix = string.Empty;
                CustomDataTable cdt = new CustomDataTable();

               
              
                //lst = dt.ToList<AV_SiteTestSummary>();// DataTableToList(dt);
                lst = DataTableToList(dt, ds.Tables[8]);
                // Beam Test Log 
                if (ds.Tables[13].Rows.Count > 0)
                {
                    DataTable dtBTLog = ds.Tables[13];
                    BTLog = dtBTLog.ToList<AV_BeamTestLog>();
                    lst[0].BeamTestLog = BTLog;
                }
                // Beam Test Legend 
                if (ds.Tables[14].Rows.Count > 0)
                {
                    DataTable dtBTLegend = ds.Tables[14];
                    BTLegend = dtBTLegend.ToList<BeamTestLegend>();
                    lst[0].BTLegend = BTLegend;
                }
                if (ds.Tables[15].Rows.Count > 0)
                {
                    DataTable dtsiteTestLog = ds.Tables[15];
                    siteTestLog = dtsiteTestLog.ToList<AV_SiteTestLog>();
                    lst[0].SiteTestLog = siteTestLog;
                }
                if (ds.Tables[16].Rows.Count > 0)
                {
                    DataTable SectorsOokla = ds.Tables[16];
                    lst[0].SectorsOoklaTest = SectorsOokla.ToList<AV_SectorsOoklaTest>();
                    
                }
                if (ds.Tables[17].Rows.Count > 0)
                {
                    DataTable dtRDACounts = ds.Tables[17];
                    RDACounts = dtRDACounts.ToList<RDACounts>();
                    lst[0].RDACounts = RDACounts;
                }
                if (ds.Tables[18].Rows.Count > 0)
                {
                    DataTable dtOoklaDLSiteLevels = ds.Tables[18];
                    OoklaDLSiteLevels = dtOoklaDLSiteLevels.ToList<OoklaDLSiteLevels>();
                    lst[0].OoklaDLSiteLevels = OoklaDLSiteLevels;
                }
                if (ds.Tables[13].Rows.Count > 0)
                {
                    DataTable dtBTLog = ds.Tables[13];
                    BTLog = dtBTLog.ToList<AV_BeamTestLog>();
                    lst[0].BeamTestLog = BTLog;
                }
                List<TestLocations> tL = ds.Tables[1].ToList<TestLocations>();
                if(tL.Count > 0) { 
                lst[0].Locations.AddRange(tL);
                }
                List<AV_NetLayerObservation> lstObservation = ds.Tables[12].ToList<AV_NetLayerObservation>();
                if (lstObservation.Count > 0)
                {
                    lst[0].Observations.AddRange(lstObservation);
                }
                List<TestWiseSiteData> tst = ds.Tables[8].ToList<TestWiseSiteData>();
                    lst[0].TestWiseSiteData.AddRange(tst);
                    List<MOMTCallPlot> momt = ds.Tables[9].ToList<MOMTCallPlot>();
                    lst[0].MOMTCallPlot.AddRange(momt);
                    List<CW_CCWHOPlot> cwccw = ds.Tables[10].ToList<CW_CCWHOPlot>();
                    lst[0].CW_CCWHOPlot.AddRange(cwccw);    
                string Scope = "";
                if (ds.Tables[0].Columns.Contains("Scope") && ds.Tables[0].Rows.Count>0   )
                {
                    Scope = "IND";

                       DataTable dtt = ds.Tables[0];
                    Scope = (from DataRow dr in dtt.Rows
                              where (string)dr["Scope"] == Scope
                              select (string)dr["Scope"]).FirstOrDefault();

                    if(Scope == "IND") { 
                    DataTable dtfloorPlan = ds.Tables[11];
                    if (dtfloorPlan.Columns.Contains("PlanFile"))
                    {
                        FloorPlans = dtfloorPlan.ToList<AV_FloorPlan>();
                        lst[0].FloorPlans = FloorPlans;
                    }
                    }

                }

                if (lst.Count>0)
                {
                    var FirstRow = lst.FirstOrDefault();
                    SiteScheduleDate = FirstRow.SiteUploadDate;

                    if (!string.IsNullOrEmpty(FirstRow.SiteUploadDate.ToString()))
                    {
                       NetLayer = FirstRow.ActualSiteCode + "\\" + FirstRow.NetworkMode + "_" + FirstRow.Band + "_" + FirstRow.Carrier;

                    }

                    ClientPrefix = FirstRow.ClientPrefix;

                    if (FirstRow.LayerStatus == "REPORT_SUBMITTED" || FirstRow.LayerStatus == "COMPLETED")
                    {
                          MakeKml = false;
                    }

                }
                #region ReportPlot

                string TempNetworkMode = string.Empty;
               
                List<TempData> tempdata = new List<TempData>();
                TempData tempd;

                //KML km = new KML();
                //string kml = string.Empty;
                //kml = km.MarkerOpen();


                KML km = new KML();
                string kml = string.Empty;
                string PciKml = string.Empty;
                string HoKml = string.Empty;
                string CwKml = string.Empty;
                string CcwKml = string.Empty;
                string RsrpKml = string.Empty;
                string RsrqKml = string.Empty;
                string CinrKml = string.Empty;
                string ChKml = string.Empty;
               
                string color = string.Empty;

                

                if (SiteScheduleDate >= plotDate)
                {

                    if (Scope == "IND")
                    {
                        if (!ds.Tables[0].Columns.Contains("Scope"))
                        {
                            
                        }
                        else
                        {

                            List<string> TempColor = new List<string>();
                            for (int i = 0; i < Plots.Rows.Count; i++)
                            {
                                plot = new SiteReportPlotVM();
                                plot.Latitude = Convert.ToDouble(Plots.Rows[i]["Latitude"]);
                                plot.Longitude = Convert.ToDouble(Plots.Rows[i]["Longitude"]);
                                string Sector = Plots.Rows[i]["Sector"].ToString();
                                //   string PciId = Plots.Rows[i]["PciId"].ToString();// cnvtr.Int32();
                                TempNetworkMode = Plots.Rows[i]["SubNetworkMode"].ToString();
                                plot.NetworkMode = Plots.Rows[i]["NetworkMode"].ToString();

                                if (!string.IsNullOrEmpty(Plots.Rows[i]["serverTimestamp"].ToString()))
                                {
                                    plot.serverTimestamp = Convert.ToDateTime(Plots.Rows[i]["serverTimestamp"].ToString()).ToString("yyy-MM-dd HH:mm:ss");
                                }
                                plot.Band = Plots.Rows[i]["Band"].ToString();
                                plot.Carrier = Plots.Rows[i]["Carrier"].ToString();
                                plot.PCI = Plots.Rows[i]["PciId"].ToString();
                                plot.TestType = Plots.Rows[i]["TestType"].ToString();
                                plot.IsHandover = Convert.ToBoolean(Plots.Rows[i]["IsHandover"]);
                                plot.plotColorName = Plots.Rows[i]["pciColor"].ToString();
                                plot.rsrqColorName = Plots.Rows[i]["rsrqColor"].ToString();
                                plot.rsrpColorName = Plots.Rows[i]["rsrpColor"].ToString();
                                plot.PlanFile = FloorPlans.Where(x => x.FloorId == Convert.ToInt64(Plots.Rows[i]["FloorId"])).Select(y => y.PlanFile).FirstOrDefault();
                                plot.FloorId = Convert.ToInt64(Plots.Rows[i]["FloorId"]);
                                //kml += km.MarkerCoordinates("", "des", "Id"+i, "http://dagik.org/kml_intro/E/ball.png", plot.Longitude+","+ plot.Latitude);
                                plot.ChColorName= Plots.Rows[i]["ChColor"].ToString();


                                #region  ch plot
                                AD_DefinationBL db = new AD_DefinationBL();

                                if (plot.TestType == "CCW" || plot.TestType == "CW")
                                {
                                    var Colors = db.ToList("Colors");

                                    var temp = tempdata.Where(m => m.value1 == plot.Carrier).FirstOrDefault();
                                    if (temp != null)
                                    {
                                     //  plot.ChColorName = temp.value2;
                                    }
                                    else
                                    {
                                        tempd = new TempData();
                                        tempd.value1 = plot.Carrier;
                                        Again:
                                        var rand = new Random();
                                        var Color = Colors[rand.Next(Colors.Count)];

                                        if (TempColor.Contains(Color.ColorCode))
                                        {
                                            goto Again;
                                        }

                                        tempd.value2 = Color.ColorCode;
                                     //   plot.ChColorName = tempd.value2;

                                        TempColor.Add(Color.ColorCode);

                                        tempdata.Add(tempd);


                                        //Random randomGen = new Random();
                                        //KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                                        //KnownColor randomColorName = names[randomGen.Next(names.Length)];
                                        //Color randomColor = Color.FromKnownColor(randomColorName);


                                    }
                                }
                                #endregion

                                #region LTE Color Collection

                                var Sectors = db.ToList("Sectors");

                                var secRec = Sectors.Where(m => m.DefinationName.ToLower() == Sector.ToLower()).FirstOrDefault();
                                if (secRec != null)
                                {
                                   // plot.plotColorName = secRec.ColorCode;
                                }
                                else
                                {
                                    plot.plotColorName = Color.Gray.Name;
                                }


                                //RF Parameters Plot
                                if (TempNetworkMode == "LTE")
                                {
                                    int LteRsrp = Plots.Rows[i]["LteRsrp"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsrp"]);
                                    if (LteRsrp > -75 && LteRsrp <= -40)
                                    {
                                        plot.rsrpColor = Color.DarkGreen;
                                    }
                                    else if (LteRsrp > -85 && LteRsrp <= -75)
                                    {
                                        plot.rsrpColor = Color.LightGreen;
                                    }
                                    else if (LteRsrp > -95 && LteRsrp <= -85)
                                    {
                                        plot.rsrpColor = Color.Yellow;
                                    }
                                    else if (LteRsrp > -105 && LteRsrp <= -95)
                                    {
                                        plot.rsrpColor = Color.Orange;
                                    }
                                    else if (LteRsrp > -120 && LteRsrp <= -105)
                                    {
                                        plot.rsrpColor = Color.Red;
                                    }
                                    else if (LteRsrp >= -130 && LteRsrp <= -120)
                                    {
                                        plot.rsrpColor = Color.DarkRed;
                                    }
                                    //else
                                    //    plot.rsrpColor = ColorTranslator.FromHtml("#87898A");

                                    int LteRsrq = Plots.Rows[i]["LteRsrq"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsrq"]);
                                    if (LteRsrq > -7 && LteRsrq <= 0)
                                    {
                                        plot.rsrqColor = Color.DarkGreen;
                                    }
                                    else if (LteRsrq > -12 && LteRsrq <= -7)
                                    {
                                        plot.rsrqColor = Color.LightGreen;
                                    }
                                    else if (LteRsrq > -16 && LteRsrq <= -12)
                                    {
                                        plot.rsrqColor = Color.Yellow;
                                    }
                                    else if (LteRsrq >= -20 && LteRsrq <= -16)
                                    {
                                        plot.rsrqColor = Color.Red;
                                    }



                                    int Ltesinr = Plots.Rows[i]["LteRsnr"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsnr"]);
                                    if (Ltesinr > 25 && Ltesinr <= 30)
                                    {
                                        plot.rsnrColor = Color.DarkGreen;
                                    }
                                    else if (Ltesinr > 15 && Ltesinr <= 25)
                                    {
                                        plot.rsnrColor = Color.LightGreen;
                                    }
                                    else if (Ltesinr > 5 && Ltesinr <= 15)
                                    {
                                        plot.rsnrColor = Color.Yellow;
                                    }
                                    else if (Ltesinr >= -15 && Ltesinr <= 5)
                                    {
                                        plot.rsnrColor = Color.Orange;
                                    }
                                }
                                #endregion

                                #region WCDMA
                                else if (TempNetworkMode == "WCDMA")
                                {
                                    int WcdmaEcio = cnvtr.Int32(Plots.Rows[i]["WcdmaEcio"].ToString());

                                    if (WcdmaEcio > -6 && WcdmaEcio <= 0)
                                    {
                                        plot.rsrqColor = Color.DarkGreen;
                                    }
                                    else if (WcdmaEcio > -10 && WcdmaEcio <= -6)
                                    {
                                        plot.rsrqColor = Color.LightGreen;
                                    }
                                    else if (WcdmaEcio > -14 && WcdmaEcio <= -10)
                                    {
                                        plot.rsrqColor = Color.Yellow;
                                    }
                                    else if (WcdmaEcio > -16 && WcdmaEcio <= -14)
                                    {
                                        plot.rsrqColor = Color.Orange;
                                    }
                                    else if (WcdmaEcio >= -40 && WcdmaEcio <= -16)
                                    {
                                        plot.rsrqColor = Color.Red;
                                    }

                                    //WcdmaRscp
                                    int WcdmaRscp = Plots.Rows[i]["WcdmaRscp"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["WcdmaRscp"]);
                                    if (WcdmaRscp > -75 && WcdmaRscp < -40)
                                    {
                                        plot.rsrpColor = Color.DarkGreen;
                                    }
                                    else if (WcdmaRscp > -85 && WcdmaRscp <= -75)
                                    {
                                        plot.rsrpColor = Color.LightGreen;
                                    }
                                    else if (WcdmaRscp > -95 && WcdmaRscp <= -85)
                                    {
                                        plot.rsrpColor = Color.Yellow;
                                    }
                                    else if (WcdmaRscp > -105 && WcdmaRscp <= -95)
                                    {
                                        plot.rsrpColor = Color.Orange;
                                    }
                                    else if (WcdmaRscp >= -130 && WcdmaRscp <= -105)
                                    {
                                        plot.rsrpColor = Color.Red;
                                    }
                                }
                                #endregion

                                #region GSM
                                else if (TempNetworkMode == "GSM")
                                {
                                    int GsmRxQual = Plots.Rows[i]["GsmRxQual"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["GsmRxQual"]);
                                    if (GsmRxQual >= 0 && GsmRxQual < 2)
                                    {
                                        plot.rsrqColor = Color.DarkGreen;
                                    }
                                    else if (GsmRxQual >= 2 && GsmRxQual < 4)
                                    {
                                        plot.rsrqColor = Color.LightGreen;
                                    }
                                    else if (GsmRxQual >= 4 && GsmRxQual < 6)
                                    {
                                        plot.rsrqColor = Color.Yellow;
                                    }
                                    else if (GsmRxQual >= 6 && GsmRxQual <= 7)
                                    {
                                        plot.rsrqColor = Color.Orange;
                                    }


                                    int GsmRssi = Plots.Rows[i]["GsmRssi"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["GsmRssi"]);
                                    if (GsmRssi > -75 && GsmRssi <= -40)
                                    {
                                        plot.rsrpColor = Color.DarkGreen;
                                    }
                                    else if (GsmRssi > -85 && GsmRssi <= -75)
                                    {
                                        plot.rsrpColor = Color.LightGreen;
                                    }
                                    else if (GsmRssi > -95 && GsmRssi <= -85)
                                    {
                                        plot.rsrpColor = Color.Yellow;
                                    }
                                    else if (GsmRssi > -105 && GsmRssi <= -95)
                                    {
                                        plot.rsrpColor = Color.Orange;
                                    }
                                    else if (GsmRssi >= -130 && GsmRssi <= -105)
                                    {
                                        plot.rsrpColor = Color.Red;
                                    }
                                }
                                #endregion

                                ReportPlot.Add(plot);
                            }
                            #endregion


                        }
                        DataTable Lagend = ds.Tables[4];
                        for (int i = 0; i < Lagend.Rows.Count; i++)
                        {
                            plot = new SiteReportPlotVM();
                            plot.PCI = Lagend.Rows[i]["PciId"].ToString();
                            plot.plotColorName = Lagend.Rows[i]["pciColor"].ToString();
                            plot.TestType = Lagend.Rows[i]["TestType"].ToString();
                            plot.PlanFile = Lagend.Rows[i]["PlanFile"].ToString(); 
                            plot.NetworkMode = Lagend.Rows[i]["NetworkMode"].ToString();
                            ReportPlot.Add(plot);

                        }
                    }
                    else {
                        if (MakeKml)
                        {
                            for (int i = 0; i < Plots.Rows.Count; i++)
                            {
                                km.SaveKml(Plots.Rows[i]["pciPlot"].ToString(), "pci", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["pciPlotLTE"].ToString(), "pciLTE", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["hoPlot"].ToString(), "ho", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["cwPlot"].ToString(), "cw", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["ccwPlot"].ToString(), "ccw", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["rsrpPlot"].ToString(), "Rsrp", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["rsrqPlot"].ToString(), "Rsrq", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["sinrPlot"].ToString(), "Cinr", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["chPlot"].ToString(), "ch", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["beamPlot"].ToString(), "bm", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["beamGroupPlot"].ToString(), "bmg", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["brRelPlot"].ToString(), "brrel", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["brDropPlot"].ToString(), "brdrop", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                km.SaveKml(Plots.Rows[i]["dlPlot"].ToString(), "dl", kmlFilePath + ClientPrefix + "\\" + NetLayer);
                                //
                            }
                        }
                        DataTable Lagend = ds.Tables[4];
                    for (int i = 0; i < Lagend.Rows.Count; i++)
                    {
                        plot = new SiteReportPlotVM();
                        plot.PCI = Lagend.Rows[i]["PciId"].ToString();
                        plot.plotColorName = Lagend.Rows[i]["pciColor"].ToString();
                        plot.TestType = Lagend.Rows[i]["TestType"].ToString();
                        plot.NetworkMode = Lagend.Rows[i]["NetworkMode"].ToString();
                            ReportPlot.Add(plot);

                    }

                    }
                }
                else
                {
                  

                    List<string> TempColor = new List<string>();
                    for (int i = 0; i < Plots.Rows.Count; i++)
                    {
                        plot = new SiteReportPlotVM();
                        plot.Latitude = Convert.ToDouble(Plots.Rows[i]["Latitude"]);
                        plot.Longitude = Convert.ToDouble(Plots.Rows[i]["Longitude"]);
                        string Sector = Plots.Rows[i]["Sector"].ToString();
                        //   string PciId = Plots.Rows[i]["PciId"].ToString();// cnvtr.Int32();
                        TempNetworkMode = Plots.Rows[i]["SubNetworkMode"].ToString();
                        plot.NetworkMode = Plots.Rows[i]["NetworkMode"].ToString();

                        if (!string.IsNullOrEmpty(Plots.Rows[i]["serverTimestamp"].ToString()))
                        {
                            plot.serverTimestamp = Convert.ToDateTime(Plots.Rows[i]["serverTimestamp"].ToString()).ToString("yyy-MM-dd HH:mm:ss");
                        }
                        plot.Band = Plots.Rows[i]["Band"].ToString();
                        plot.Carrier = Plots.Rows[i]["Carrier"].ToString();
                        plot.PCI = Plots.Rows[i]["PciId"].ToString();
                        plot.TestType = Plots.Rows[i]["TestType"].ToString();
                        plot.IsHandover = Convert.ToBoolean(Plots.Rows[i]["IsHandover"]);
                        plot.plotColorName = Plots.Rows[i]["pciColor"].ToString();
                        plot.rsrqColorName = Plots.Rows[i]["rsrqColor"].ToString();
                        plot.rsrpColorName = Plots.Rows[i]["rsrpColor"].ToString();
                        plot.PlanFile = FloorPlans.Where(x => x.FloorId == Convert.ToInt64(Plots.Rows[i]["FloorId"])).Select(y=>y.PlanFile).FirstOrDefault();

                        //kml += km.MarkerCoordinates("", "des", "Id"+i, "http://dagik.org/kml_intro/E/ball.png", plot.Longitude+","+ plot.Latitude);



                      
                        AD_DefinationBL db = new AD_DefinationBL();

                        if (plot.TestType == "CCW" || plot.TestType == "CW")
                        {
                            var Colors = db.ToList("Colors");

                            var temp = tempdata.Where(m => m.value1 == plot.Carrier).FirstOrDefault();
                            if (temp != null)
                            {
                                plot.ChColorName = temp.value2;
                            }
                            else
                            {
                                tempd = new TempData();
                                tempd.value1 = plot.Carrier;
                                Again:
                                var rand = new Random();
                                var Color = Colors[rand.Next(Colors.Count)];

                                if (TempColor.Contains(Color.ColorCode))
                                {
                                    goto Again;
                                }

                                tempd.value2 = Color.ColorCode;
                                plot.ChColorName = tempd.value2;

                                TempColor.Add(Color.ColorCode);

                                tempdata.Add(tempd);
                               

                                //Random randomGen = new Random();
                                //KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                                //KnownColor randomColorName = names[randomGen.Next(names.Length)];
                                //Color randomColor = Color.FromKnownColor(randomColorName);


                            }
                        }
                      

                        #region LTE Color Collection

                        var Sectors = db.ToList("Sectors");

                        var secRec = Sectors.Where(m => m.DefinationName.ToLower() == Sector.ToLower()).FirstOrDefault();
                        if (secRec != null)
                        {
                            plot.plotColorName = secRec.ColorCode;
                        }
                        else
                        {
                            plot.plotColorName = Color.Gray.Name;
                        }


                        //RF Parameters Plot
                        if (TempNetworkMode == "LTE")
                        {
                            int LteRsrp = Plots.Rows[i]["LteRsrp"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsrp"]);
                            if (LteRsrp > -75 && LteRsrp <= -40)
                            {
                                plot.rsrpColor = Color.DarkGreen;
                            }
                            else if (LteRsrp > -85 && LteRsrp <= -75)
                            {
                                plot.rsrpColor = Color.LightGreen;
                            }
                            else if (LteRsrp > -95 && LteRsrp <= -85)
                            {
                                plot.rsrpColor = Color.Yellow;
                            }
                            else if (LteRsrp > -105 && LteRsrp <= -95)
                            {
                                plot.rsrpColor = Color.Orange;
                            }
                            else if (LteRsrp > -120 && LteRsrp <= -105)
                            {
                                plot.rsrpColor = Color.Red;
                            }
                            else if (LteRsrp >= -130 && LteRsrp <= -120)
                            {
                                plot.rsrpColor = Color.DarkRed;
                            }
                            //else
                            //    plot.rsrpColor = ColorTranslator.FromHtml("#87898A");

                            int LteRsrq = Plots.Rows[i]["LteRsrq"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsrq"]);
                            if (LteRsrq > -7 && LteRsrq <= 0)
                            {
                                plot.rsrqColor = Color.DarkGreen;
                            }
                            else if (LteRsrq > -12 && LteRsrq <= -7)
                            {
                                plot.rsrqColor = Color.LightGreen;
                            }
                            else if (LteRsrq > -16 && LteRsrq <= -12)
                            {
                                plot.rsrqColor = Color.Yellow;
                            }
                            else if (LteRsrq >= -20 && LteRsrq <= -16)
                            {
                                plot.rsrqColor = Color.Red;
                            }



                            int Ltesinr = Plots.Rows[i]["LteRsnr"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["LteRsnr"]);
                            if (Ltesinr > 25 && Ltesinr <= 30)
                            {
                                plot.rsnrColor = Color.DarkGreen;
                            }
                            else if (Ltesinr > 15 && Ltesinr <= 25)
                            {
                                plot.rsnrColor = Color.LightGreen;
                            }
                            else if (Ltesinr > 5 && Ltesinr <= 15)
                            {
                                plot.rsnrColor = Color.Yellow;
                            }
                            else if (Ltesinr >= -15 && Ltesinr <= 5)
                            {
                                plot.rsnrColor = Color.Orange;
                            }
                        }
                        #endregion

                        #region WCDMA
                        else if (TempNetworkMode == "WCDMA")
                        {
                            int WcdmaEcio = cnvtr.Int32(Plots.Rows[i]["WcdmaEcio"].ToString());

                            if (WcdmaEcio > -6 && WcdmaEcio <= 0)
                            {
                                plot.rsrqColor = Color.DarkGreen;
                            }
                            else if (WcdmaEcio > -10 && WcdmaEcio <= -6)
                            {
                                plot.rsrqColor = Color.LightGreen;
                            }
                            else if (WcdmaEcio > -14 && WcdmaEcio <= -10)
                            {
                                plot.rsrqColor = Color.Yellow;
                            }
                            else if (WcdmaEcio > -16 && WcdmaEcio <= -14)
                            {
                                plot.rsrqColor = Color.Orange;
                            }
                            else if (WcdmaEcio >= -40 && WcdmaEcio <= -16)
                            {
                                plot.rsrqColor = Color.Red;
                            }

                            //WcdmaRscp
                            int WcdmaRscp = Plots.Rows[i]["WcdmaRscp"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["WcdmaRscp"]);
                            if (WcdmaRscp > -75 && WcdmaRscp < -40)
                            {
                                plot.rsrpColor = Color.DarkGreen;
                            }
                            else if (WcdmaRscp > -85 && WcdmaRscp <= -75)
                            {
                                plot.rsrpColor = Color.LightGreen;
                            }
                            else if (WcdmaRscp > -95 && WcdmaRscp <= -85)
                            {
                                plot.rsrpColor = Color.Yellow;
                            }
                            else if (WcdmaRscp > -105 && WcdmaRscp <= -95)
                            {
                                plot.rsrpColor = Color.Orange;
                            }
                            else if (WcdmaRscp >= -130 && WcdmaRscp <= -105)
                            {
                                plot.rsrpColor = Color.Red;
                            }
                        }
                        #endregion

                        #region GSM
                        else if (TempNetworkMode == "GSM")
                        {
                            int GsmRxQual = Plots.Rows[i]["GsmRxQual"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["GsmRxQual"]);
                            if (GsmRxQual >= 0 && GsmRxQual < 2)
                            {
                                plot.rsrqColor = Color.DarkGreen;
                            }
                            else if (GsmRxQual >= 2 && GsmRxQual < 4)
                            {
                                plot.rsrqColor = Color.LightGreen;
                            }
                            else if (GsmRxQual >= 4 && GsmRxQual < 6)
                            {
                                plot.rsrqColor = Color.Yellow;
                            }
                            else if (GsmRxQual >= 6 && GsmRxQual <= 7)
                            {
                                plot.rsrqColor = Color.Orange;
                            }


                            int GsmRssi = Plots.Rows[i]["GsmRssi"] is DBNull ? 0 : Convert.ToInt32(Plots.Rows[i]["GsmRssi"]);
                            if (GsmRssi > -75 && GsmRssi <= -40)
                            {
                                plot.rsrpColor = Color.DarkGreen;
                            }
                            else if (GsmRssi > -85 && GsmRssi <= -75)
                            {
                                plot.rsrpColor = Color.LightGreen;
                            }
                            else if (GsmRssi > -95 && GsmRssi <= -85)
                            {
                                plot.rsrpColor = Color.Yellow;
                            }
                            else if (GsmRssi > -105 && GsmRssi <= -95)
                            {
                                plot.rsrpColor = Color.Orange;
                            }
                            else if (GsmRssi >= -130 && GsmRssi <= -105)
                            {
                                plot.rsrpColor = Color.Red;
                            }
                        }
                      

                        ReportPlot.Add(plot);
                    }
                   

                }


                #endregion

                DataTable conf = ds.Tables[3];
                rptConf = conf.ToList<AD_ReportConfiguration>();

                if (ds.Tables.Count>5)
                {
                    DataTable dtMarketSitesOrServerTimeStamp = ds.Tables[5];
                    if (ds.Tables[4].Columns.Contains("serverTimeStamp"))
                    {
                        ServerTimeStamp.AddRange(fnServerTimeStamp(dtMarketSitesOrServerTimeStamp));
                    }
                }


                if (ds.Tables.Count > 6)
                {
                    DataTable dtRFPlotLegends = ds.Tables[6];
                    if (dtRFPlotLegends.Columns.Contains("rangeColor"))
                    {
                       RFPlotLegends = dtRFPlotLegends.ToList<AV_RFPlotLegends>();
                    }
                }
              


                if (ds.Tables.Count >7)
                {
                    DataTable dtMarketSites = ds.Tables[7];

                    if (dtMarketSites.Columns.Contains("SiteCode"))
                    {
                        MarketSites = dtMarketSites.ToList<AV_MarketSites>();
                    }
                }



            }

            return lst;
        }

        private List<TempData> fnServerTimeStamp(DataTable dt) {
            TempData td;
           List< TempData> tdList=new List<TempData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                td = new TempData();
                if (!string.IsNullOrEmpty(dt.Rows[i]["serverTimestamp"].ToString()))
                {
                    td.value1 = Convert.ToDateTime(dt.Rows[i]["serverTimeStamp"].ToString()).ToString("yyy-MM-dd HH:mm:ss");
                    tdList.Add(td);
                }
            }
            return tdList;
        }
    
        
        public List<AV_SiteTestSummary> AV_GetNetworkLayerProcessed(int SiteId, int BandId, string Carrier, string NetworkMode)
        {

            DataSet ds = sd.AV_GetNetworkLayerProcessed(SiteId, BandId, Carrier, NetworkMode);

            List<AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lst = dt.ToList<AV_SiteTestSummary>();
            }

            return lst;
        }


        public List<AV_SiteTestSummary> NetLayerSummary(int SiteId, int BandId, string Carrier, string NetworkMode, int UserId)
        {

            DataTable dt = sd.NetLayerSummary(SiteId, BandId, Carrier, NetworkMode, UserId);

            List<AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            if (dt != null)
            {
                lst = dt.ToList<AV_SiteTestSummary>();
            }

            return lst;
        }


        public List<AV_SiteTestSummary> DataTableToList(DataTable dt) {
      
            List< AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            #region AV_SiteTestSummary
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AV_SiteTestSummary smry = new AV_SiteTestSummary();
                smry.SummaryId = cnvtr.Int32(dt.Rows[i]["SummaryId"].ToString());
                smry.RegionId = cnvtr.Int32(dt.Rows[i]["RegionId"].ToString());
                smry.Region = dt.Rows[i]["Region"].ToString();
                smry.CityId = cnvtr.Int32(dt.Rows[i]["CityId"].ToString());
                smry.City = dt.Rows[i]["City"].ToString();
                smry.ClusterId = cnvtr.Int32(dt.Rows[i]["ClusterId"].ToString());
                smry.Cluster = dt.Rows[i]["Cluster"].ToString();
                smry.SiteId = cnvtr.Int32(dt.Rows[i]["SiteId"].ToString());
                smry.Site = dt.Rows[i]["Site"].ToString();
                smry.ActualSiteCode =(dt.Columns.Contains("ActualSiteCode"))? dt.Rows[i]["ActualSiteCode"].ToString():null;
                smry.BandId = cnvtr.Int32(dt.Rows[i]["BandId"].ToString());
                smry.Band = dt.Rows[i]["Band"].ToString();
                smry.BandNo = (dt.Columns.Contains("BandNo")) ? dt.Rows[i]["BandNo"].ToString():null;
             
                smry.NetworkModeId = cnvtr.Int32(dt.Rows[i]["NetworkModeId"].ToString());
                smry.NetworkMode = dt.Rows[i]["NetworkMode"].ToString();
                smry.CarrierId = cnvtr.Int32(dt.Rows[i]["CarrierId"].ToString());
                smry.Carrier = dt.Rows[i]["Carrier"].ToString();
                smry.Testcount = (dt.Columns.Contains("TestCount")) ? Convert.ToInt64(dt.Rows[i]["TestCount"].ToString()) : 0;
                smry.TestDuration = (dt.Columns.Contains("TestDuration")) ? Convert.ToDecimal(dt.Rows[i]["TestDuration"].ToString()) : 0;
                if (dt.Columns.Contains("SiteScheduleDate") && !string.IsNullOrEmpty(dt.Rows[i]["SiteScheduleDate"].ToString()))
                {
                    smry.SiteScheduleDate = Convert.ToDateTime(dt.Rows[i]["SiteScheduleDate"].ToString());
                }

                if (dt.Columns.Contains("SiteUploadDate") && !string.IsNullOrEmpty(dt.Rows[i]["SiteUploadDate"].ToString()))
                {
                    smry.SiteUploadDate = Convert.ToDateTime(dt.Rows[i]["SiteUploadDate"].ToString());
                    //SiteScheduleDate = (DateTime)smry.SiteUploadDate;

                    //NetLayer = smry.ActualSiteCode + "\\" + smry.NetworkMode + "_" + smry.Band + "_" + smry.Carrier;

                }
                smry.SectorId = cnvtr.Int32(dt.Rows[i]["SectorId"].ToString());
                smry.Sector = dt.Rows[i]["Sector"].ToString();
                smry.ScopeId = cnvtr.Int32(dt.Rows[i]["ScopeId"].ToString());
                smry.Scope = dt.Rows[i]["Scope"].ToString();


                smry.CADLSpeed = cnvtr.Double(dt.Rows[i]["CADLSpeed"].ToString());
                smry.CAULSpeed = cnvtr.Double(dt.Rows[i]["CAULSpeed"].ToString());
                smry.Antenna = dt.Rows[i]["Antenna"].ToString();
                smry.Azimuth = cnvtr.Double(dt.Rows[i]["Azimuth"].ToString());
                smry.PciId = Convert.ToDecimal(dt.Rows[i]["PciId"].ToString());
                smry.BeamWidth = cnvtr.Double(dt.Rows[i]["BeamWidth"].ToString());
                smry.BandWidth = (dt.Columns.Contains("BandWidth")) ? cnvtr.Int32(dt.Rows[i]["BandWidth"].ToString()):0;
                smry.Testcount = (dt.Columns.Contains("Testcount")) ? cnvtr.Int32(dt.Rows[i]["Testcount"].ToString()) : 0;
                smry.TestDuration = Convert.ToDecimal(dt.Rows[i]["TestDuration"].ToString());
                smry.GsmRssi = cnvtr.Int32(dt.Rows[i]["GsmRssi"].ToString());
                smry.GsmRxQual = cnvtr.Int32(dt.Rows[i]["GsmRxQual"].ToString());
                smry.WcdmaRssi = cnvtr.Int32(dt.Rows[i]["WcdmaRssi"].ToString());
                smry.WcdmaRscp = cnvtr.Int32(dt.Rows[i]["WcdmaRscp"].ToString());
                smry.WcdmaEcio = cnvtr.Double(dt.Rows[i]["WcdmaEcio"].ToString());
                smry.LteRssi = cnvtr.Int32(dt.Rows[i]["LteRssi"].ToString());
                smry.LteRsrp = cnvtr.Int32(dt.Rows[i]["LteRsrp"].ToString());
                smry.LteRsrq = cnvtr.Int32(dt.Rows[i]["LteRsrq"].ToString());
                smry.LteRsnr = cnvtr.Double(dt.Rows[i]["LteRsnr"].ToString());
                smry.LteCqi = cnvtr.Double(dt.Rows[i]["LteCqi"].ToString());
                smry.DistanceFromSite = cnvtr.Double(dt.Rows[i]["DistanceFromSite"].ToString());
                smry.AngleToSite = cnvtr.Double(dt.Rows[i]["AngleToSite"].ToString());
                smry.FtpStatus = dt.Rows[i]["FtpStatus"].ToString();
                smry.PingHost = dt.Rows[i]["PingHost"].ToString();
                smry.LatencyRate = cnvtr.Double(dt.Rows[i]["LatencyRate"].ToString());
                smry.PingIterations = cnvtr.Double(dt.Rows[i]["PingIterations"].ToString());
                smry.PingMinResult = cnvtr.Double(dt.Rows[i]["PingMinResult"].ToString());
                smry.PingMaxResult = cnvtr.Double(dt.Rows[i]["PingMaxResult"].ToString());
                smry.PingAverageResult = cnvtr.Double(dt.Rows[i]["PingAverageResult"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["PingStatus"].ToString()))
                {
                    smry.PingStatus = cnvtr.Bool(dt.Rows[i]["PingStatus"].ToString());

                }

                smry.DownlinkRate = cnvtr.Double(dt.Rows[i]["DownlinkRate"].ToString());
                smry.DownlinkMinResult = cnvtr.Double(dt.Rows[i]["DownlinkMinResult"].ToString());
                smry.DownlinkMaxResult = cnvtr.Double(dt.Rows[i]["DownlinkMaxResult"].ToString());
                smry.DownlinkAvgResult = cnvtr.Double(dt.Rows[i]["DownlinkAvgResult"].ToString());
                if (!string.IsNullOrEmpty(dt.Rows[i]["DownlinkStatus"].ToString()))
                {
                    smry.DownlinkStatus = cnvtr.Bool(dt.Rows[i]["DownlinkStatus"].ToString());
                }
                smry.UplinkRate = cnvtr.Double(dt.Rows[i]["UplinkRate"].ToString());
                smry.UplinkMinResult = cnvtr.Double(dt.Rows[i]["UplinkMinResult"].ToString());
                smry.UplinkMaxResult = cnvtr.Double(dt.Rows[i]["UplinkMaxResult"].ToString());
                smry.UplinkAvgResult = cnvtr.Double(dt.Rows[i]["UplinkAvgResult"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["UplinkStatus"].ToString()))
                {
                    smry.UplinkStatus = cnvtr.Bool(dt.Rows[i]["UplinkStatus"].ToString());
                }

                smry.ConnectionSetupTime = cnvtr.Double(dt.Rows[i]["ConnectionSetupTime"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["ConnectionSetupStatus"].ToString()))
                {
                    smry.ConnectionSetupStatus = cnvtr.Bool(dt.Rows[i]["ConnectionSetupStatus"].ToString());
                }
                smry.MoMtCallNo = dt.Rows[i]["MoMtCallNo"].ToString();
                smry.MoMtCallDuration = cnvtr.Int32(dt.Rows[i]["MoMtCallDuration"].ToString());


                if (!string.IsNullOrEmpty(dt.Rows[i]["MoStatus"].ToString()))
                {
                    smry.MoStatus = cnvtr.Bool(dt.Rows[i]["MoStatus"].ToString());
                }

                if (dt.Columns.Contains("ActualSiteCode"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MobHandoverStatus"].ToString()))
                    {
                        smry.MobHandoverStatus = cnvtr.Bool(dt.Rows[i]["MobHandoverStatus"].ToString());
                    }
                }

                if (dt.Columns.Contains("MimoStatus"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MimoStatus"].ToString()))
                    {
                        smry.MtStatus = cnvtr.Bool(dt.Rows[i]["MimoStatus"].ToString());
                    }
                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["MtStatus"].ToString()))
                {
                    smry.MtStatus = cnvtr.Bool(dt.Rows[i]["MtStatus"].ToString());
                }
                smry.VMoMtCallno = dt.Rows[i]["VMoMtCallno"].ToString();
                smry.VMoMtCallDuration = cnvtr.Int32(dt.Rows[i]["VMoMtCallDuration"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["VMoStatus"].ToString()))
                {
                    smry.VMoStatus = cnvtr.Bool(dt.Rows[i]["VMoStatus"].ToString());
                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["VMtStatus"].ToString()))
                {
                    smry.VMtStatus = cnvtr.Bool(dt.Rows[i]["VMtStatus"].ToString());
                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["CwHandoverStatus"].ToString()))
                {
                    smry.CwHandoverStatus = cnvtr.Bool(dt.Rows[i]["CwHandoverStatus"].ToString());
                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["Ccwhandoverstatus"].ToString()))
                {
                    smry.Ccwhandoverstatus = cnvtr.Bool(dt.Rows[i]["Ccwhandoverstatus"].ToString());
                }
                smry.FtpServerIp = dt.Rows[i]["FtpServerIp"].ToString();
                smry.FtpServerPort = dt.Rows[i]["FtpServerPort"].ToString();
                smry.FtpServerPath = dt.Rows[i]["FtpServerPath"].ToString();
                smry.FtpDownlinkFile = dt.Rows[i]["FtpDownlinkFile"].ToString();
                smry.Latitude = dt.Rows[i]["Latitude"].ToString();// cnvtr.Double(dt.Rows[i]["Latitude"].ToString());
                smry.Longitude = dt.Rows[i]["Longitude"].ToString();// cnvtr.Double(dt.Rows[i]["Longitude"].ToString());


                smry.TestLatitude = cnvtr.Double(dt.Rows[i]["TestLatitude"].ToString());
                smry.TestLongitude = cnvtr.Double(dt.Rows[i]["TestLongitude"].ToString());


                smry.OoklaTestFilePath = dt.Rows[i]["OoklaTestFilePath"].ToString();
                smry.MimoTestFilePath = dt.Rows[i]["MimoTestFilePath"].ToString();
                smry.SpeedTestFilePath = dt.Rows[i]["SpeedTestFilePath"].ToString();
                smry.CaActiveTestFilePath = dt.Rows[i]["CaActiveTestFilePath"].ToString();
                smry.CaDeavticeTestFilePath = dt.Rows[i]["CaDeavticeTestFilePath"].ToString();
                smry.CaSpeedTestFilePath = dt.Rows[i]["CaSpeedTestFilePath"].ToString();
                smry.LaaSmTestFilePath = dt.Rows[i]["LaaSmTestFilePath"].ToString();
                smry.LaaSpeedTestFilePath = dt.Rows[i]["LaaSpeedTestFilePath"].ToString();
                smry.OoklaDownlinkResult =(!string.IsNullOrEmpty(dt.Rows[i]["OoklaDownlinkResult"].ToString()))? Convert.ToDouble(dt.Rows[i]["OoklaDownlinkResult"].ToString()):0;
                smry.OoklaPingResult = (!string.IsNullOrEmpty(dt.Rows[i]["OoklaPingResult"].ToString())) ? Convert.ToDouble(dt.Rows[i]["OoklaPingResult"].ToString()) : 0;
                smry.OoklaUplinkResult = (!string.IsNullOrEmpty(dt.Rows[i]["OoklaUplinkResult"].ToString())) ? Convert.ToDouble(dt.Rows[i]["OoklaUplinkResult"].ToString()) : 0;
                smry.ClientLogo = dt.Rows[i]["ClientLogo"].ToString();
                smry.VendorLogo = dt.Rows[i]["VendorLogo"].ToString();


                if (!string.IsNullOrEmpty(dt.Rows[i]["ICwHandoverStatus"].ToString()))
                {
                    smry.ICwHandoverStatus = cnvtr.Bool(dt.Rows[i]["ICwHandoverStatus"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["ICcwhandoverstatus"].ToString()))
                {
                    smry.ICcwhandoverstatus = cnvtr.Bool(dt.Rows[i]["ICcwhandoverstatus"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLStatus"].ToString()))
                {
                    smry.PhyDLStatus = cnvtr.Bool(dt.Rows[i]["PhyDLStatus"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULStatus"].ToString()))
                {
                    smry.PhyULStatus = cnvtr.Bool(dt.Rows[i]["PhyULStatus"].ToString());

                }
                smry.SiteName = dt.Rows[i]["SiteName"].ToString();
                smry.SiteType =(dt.Columns.Contains("SiteType"))? dt.Rows[i]["SiteType"].ToString():null;
                smry.SiteClass = (dt.Columns.Contains("SiteClass")) ? dt.Rows[i]["SiteClass"].ToString() : null;
                smry.LayerStatus = (dt.Columns.Contains("LayerStatus")) ? dt.Rows[i]["LayerStatus"].ToString() : null;
                if (dt.Columns.Contains("RFHeight"))
                {
                    smry.RFHeight = (!string.IsNullOrEmpty(dt.Rows[i]["RFHeight"].ToString())) ? Convert.ToDouble(dt.Rows[i]["RFHeight"].ToString()) : 0; //cdt.ToDouble(dt, i, "RFHeight");//Convert.ToDouble( dt.Rows[i]["RFHeight"].ToString());
                }
                if (dt.Columns.Contains("ETilt"))
                {
                    smry.ETilt = (!string.IsNullOrEmpty(dt.Rows[i]["ETilt"].ToString())) ? Convert.ToInt32(dt.Rows[i]["ETilt"].ToString()) : 0;
                }
                if (dt.Columns.Contains("MTilt"))
                {
                    smry.MTilt = (!string.IsNullOrEmpty(dt.Rows[i]["MTilt"].ToString())) ? Convert.ToInt32(dt.Rows[i]["MTilt"].ToString()) : 0;
                }
                if (dt.Columns.Contains("CellId"))
                {
                    smry.CellId = dt.Rows[i]["CellId"].ToString();
                }
                if (dt.Columns.Contains("Project"))
                {
                    smry.Project = dt.Rows[i]["Project"].ToString();
                }
                
                smry.CwTestFilePath = dt.Rows[i]["CwTestFilePath"].ToString();
                smry.StationaryTestFilePath = dt.Rows[i]["StationaryTestFilePath"].ToString();


                if (dt.Columns.Contains("SMoStatus") && !string.IsNullOrEmpty(dt.Rows[i]["SMoStatus"].ToString()))
                {
                    smry.SMoStatus = bool.Parse(dt.Rows[i]["SMoStatus"].ToString());

                }

                if (dt.Columns.Contains("SMtStatus") && !string.IsNullOrEmpty(dt.Rows[i]["SMtStatus"].ToString()))
                {
                    smry.SMtStatus = bool.Parse(dt.Rows[i]["SMtStatus"].ToString());
                }

                if (dt.Columns.Contains("CarrierAggregationStatus") && !string.IsNullOrEmpty(dt.Rows[i]["CarrierAggregationStatus"].ToString()))
                {
                    smry.CarrierAggregationStatus = bool.Parse(dt.Rows[i]["CarrierAggregationStatus"].ToString());
                }

                if (dt.Columns.Contains("E911Status") && !string.IsNullOrEmpty(dt.Rows[i]["E911Status"].ToString()))
                {
                    smry.E911Status = bool.Parse(dt.Rows[i]["E911Status"].ToString());
                }
              
                smry.ClientPrefix = (dt.Columns.Contains("ClientPrefix"))?dt.Rows[i]["ClientPrefix"].ToString():null;

                if (dt.Columns.Contains("SiteAddress"))
                {
                    smry.SiteAddress = dt.Rows[i]["SiteAddress"].ToString();
                }

                smry.SectorColor = (dt.Columns.Contains("SectorColor"))? dt.Rows[i]["SectorColor"].ToString():"white";

                smry.MRBTS = (dt.Columns.Contains("MRBTS")) ? dt.Rows[i]["MRBTS"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(dt.Rows[i]["IRATHandover"].ToString()))
                {
                    smry.IRATHandover = cnvtr.Bool(dt.Rows[i]["IRATHandover"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["FastReturnStatus"].ToString()))
                {
                    smry.FastReturnStatus = cnvtr.Bool(dt.Rows[i]["FastReturnStatus"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["CoverageDistance"].ToString()))
                {
                    smry.CoverageDistance = Convert.ToDouble(dt.Rows[i]["CoverageDistance"].ToString());

                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["SampleCount"].ToString()))
                {
                    smry.SampleCount = Convert.ToInt64(dt.Rows[i]["SampleCount"].ToString());

                }
                smry.TCH = (dt.Columns.Contains("TCH")) ? dt.Rows[i]["TCH"].ToString() : string.Empty;

                if (dt.Columns.Contains("InterHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["InterHOInteruptTime"].ToString()))
                    {
                        smry.InterHOInteruptTime = cnvtr.Double(dt.Rows[i]["InterHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("IntraHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IntraHOInteruptTime"].ToString()))
                    {
                        smry.IntraHOInteruptTime = cnvtr.Double(dt.Rows[i]["IntraHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("IntreHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IntreHOInteruptTime"].ToString()))
                    {
                        smry.IntreHOInteruptTime = cnvtr.Double(dt.Rows[i]["IntreHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("callSetupTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["callSetupTime"].ToString()))
                    {
                        smry.callSetupTime = cnvtr.Double(dt.Rows[i]["callSetupTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedAvg"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedAvg"].ToString()))
                    {
                        smry.PhyULSpeedAvg = cnvtr.Double(dt.Rows[i]["PhyULSpeedAvg"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedMax"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedMax"].ToString()))
                    {
                        smry.PhyULSpeedMax = cnvtr.Double(dt.Rows[i]["PhyULSpeedMax"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedMin"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedMin"].ToString()))
                    {
                        smry.PhyULSpeedMin = cnvtr.Double(dt.Rows[i]["PhyULSpeedMin"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedAvg"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedAvg"].ToString()))
                    {
                        smry.PhyDLSpeedAvg = cnvtr.Double(dt.Rows[i]["PhyDLSpeedAvg"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedMax"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedMax"].ToString()))
                    {
                        smry.PhyDLSpeedMax = cnvtr.Double(dt.Rows[i]["PhyDLSpeedMax"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedMin"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedMin"].ToString()))
                    {
                        smry.PhyDLSpeedMin = cnvtr.Double(dt.Rows[i]["PhyDLSpeedMin"].ToString());

                    }
                }




                lst.Add(smry);
            }

            #endregion
            return lst;
        }

        public List<AV_SiteTestSummary> DataTableToList(DataTable dt, DataTable dts)
        {
            List<TestWiseSiteData> tst = dts.ToList<TestWiseSiteData>();
            List<AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            #region AV_SiteTestSummary
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AV_SiteTestSummary smry = new AV_SiteTestSummary();
                smry.SummaryId = cnvtr.Int32(dt.Rows[i]["SummaryId"].ToString());
                smry.RegionId = cnvtr.Int32(dt.Rows[i]["RegionId"].ToString());
                smry.Region = dt.Rows[i]["Region"].ToString();
                smry.CityId = cnvtr.Int32(dt.Rows[i]["CityId"].ToString());
                smry.City = dt.Rows[i]["City"].ToString();
                smry.ClusterId = cnvtr.Int32(dt.Rows[i]["ClusterId"].ToString());
                smry.Cluster = dt.Rows[i]["Cluster"].ToString();
                smry.SiteId = cnvtr.Int32(dt.Rows[i]["SiteId"].ToString());
                smry.Site = dt.Rows[i]["Site"].ToString();
                smry.ActualSiteCode = (dt.Columns.Contains("ActualSiteCode")) ? dt.Rows[i]["ActualSiteCode"].ToString() : null;
                smry.BandId = cnvtr.Int32(dt.Rows[i]["BandId"].ToString());
                smry.Band = dt.Rows[i]["Band"].ToString();
                smry.BandNo = (dt.Columns.Contains("BandNo")) ? dt.Rows[i]["BandNo"].ToString() : null;
                smry.TestWiseSiteData.AddRange(tst.Where(x => x.SectorId == cnvtr.Int32(dt.Rows[i]["SectorId"].ToString())));
                smry.NetworkModeId = cnvtr.Int32(dt.Rows[i]["NetworkModeId"].ToString());
                smry.NetworkMode = dt.Rows[i]["NetworkMode"].ToString();
                smry.CarrierId = cnvtr.Int32(dt.Rows[i]["CarrierId"].ToString());
                smry.Carrier = dt.Rows[i]["Carrier"].ToString();
                smry.Testcount = (dt.Columns.Contains("TestCount")) ? Convert.ToInt64(dt.Rows[i]["TestCount"].ToString()):0;
                smry.TestDuration = (dt.Columns.Contains("TestDuration")) ? Convert.ToDecimal(dt.Rows[i]["TestDuration"].ToString()):0;
                if (dt.Columns.Contains("SiteScheduleDate") && !string.IsNullOrEmpty(dt.Rows[i]["SiteScheduleDate"].ToString()))
                {
                    smry.SiteScheduleDate = Convert.ToDateTime(dt.Rows[i]["SiteScheduleDate"].ToString());
                }

                if (dt.Columns.Contains("SiteUploadDate") && !string.IsNullOrEmpty(dt.Rows[i]["SiteUploadDate"].ToString()))
                {
                    smry.SiteUploadDate = Convert.ToDateTime(dt.Rows[i]["SiteUploadDate"].ToString());
                    //SiteScheduleDate = (DateTime)smry.SiteUploadDate;

                    //NetLayer = smry.ActualSiteCode + "\\" + smry.NetworkMode + "_" + smry.Band + "_" + smry.Carrier;

                }
                smry.SectorId = cnvtr.Int32(dt.Rows[i]["SectorId"].ToString());
                smry.Sector = dt.Rows[i]["Sector"].ToString();
                smry.ScopeId = cnvtr.Int32(dt.Rows[i]["ScopeId"].ToString());
                smry.Scope = dt.Rows[i]["Scope"].ToString();



                smry.Antenna = dt.Rows[i]["Antenna"].ToString();
                smry.Azimuth = cnvtr.Double(dt.Rows[i]["Azimuth"].ToString());
                smry.PciId = Convert.ToDecimal(dt.Rows[i]["PciId"].ToString());
                smry.BeamWidth = cnvtr.Double(dt.Rows[i]["BeamWidth"].ToString());
                smry.BandWidth = (dt.Columns.Contains("BandWidth")) ? cnvtr.Int32(dt.Rows[i]["BandWidth"].ToString()) : 0;

                smry.GsmRssi = cnvtr.Int32(dt.Rows[i]["GsmRssi"].ToString());
                smry.GsmRxQual = cnvtr.Int32(dt.Rows[i]["GsmRxQual"].ToString());
                smry.WcdmaRssi = cnvtr.Int32(dt.Rows[i]["WcdmaRssi"].ToString());
                smry.WcdmaRscp = cnvtr.Int32(dt.Rows[i]["WcdmaRscp"].ToString());
                smry.WcdmaEcio = cnvtr.Double(dt.Rows[i]["WcdmaEcio"].ToString());
                smry.LteRssi = cnvtr.Int32(dt.Rows[i]["LteRssi"].ToString());
                smry.LteRsrp = cnvtr.Int32(dt.Rows[i]["LteRsrp"].ToString());
                smry.LteRsrq = cnvtr.Int32(dt.Rows[i]["LteRsrq"].ToString());
                smry.LteRsnr = cnvtr.Double(dt.Rows[i]["LteRsnr"].ToString());
                smry.LteCqi = cnvtr.Double(dt.Rows[i]["LteCqi"].ToString());
                smry.CADLSpeed= cnvtr.Double(dt.Rows[i]["CADLSpeed"].ToString());
                smry.CAULSpeed = cnvtr.Double(dt.Rows[i]["CAULSpeed"].ToString()); 
                smry.DistanceFromSite = cnvtr.Double(dt.Rows[i]["DistanceFromSite"].ToString());
                smry.AngleToSite = cnvtr.Double(dt.Rows[i]["AngleToSite"].ToString());
                smry.FtpStatus = dt.Rows[i]["FtpStatus"].ToString();
                smry.PingHost = dt.Rows[i]["PingHost"].ToString();
                smry.LatencyRate = cnvtr.Double(dt.Rows[i]["LatencyRate"].ToString());
                smry.PingIterations = cnvtr.Double(dt.Rows[i]["PingIterations"].ToString());
                smry.PingMinResult = cnvtr.Double(dt.Rows[i]["PingMinResult"].ToString());
                smry.PingMaxResult = cnvtr.Double(dt.Rows[i]["PingMaxResult"].ToString());
                smry.PingAverageResult = cnvtr.Double(dt.Rows[i]["PingAverageResult"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["PingStatus"].ToString()))
                {
                    smry.PingStatus = cnvtr.Bool(dt.Rows[i]["PingStatus"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLStatus"].ToString()))
                {
                    smry.PhyDLStatus = cnvtr.Bool(dt.Rows[i]["PhyDLStatus"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULStatus"].ToString()))
                {
                    smry.PhyULStatus = cnvtr.Bool(dt.Rows[i]["PhyULStatus"].ToString());

                }


                if (dt.Columns.Contains("AvgConSetupTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["AvgConSetupTime"].ToString()))
                    {
                        smry.AvgConSetupTime = cnvtr.Double(dt.Rows[i]["AvgConSetupTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("MinConSetupTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MinConSetupTime"].ToString()))
                    {
                        smry.MinConSetupTime = cnvtr.Double(dt.Rows[i]["MinConSetupTime"].ToString());
                    }

                }
                if (dt.Columns.Contains("HoInterruptionTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["HoInterruptionTime"].ToString()))
                    {
                        smry.HoInterruptionTime = cnvtr.Double(dt.Rows[i]["HoInterruptionTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("InterHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["InterHOInteruptTime"].ToString()))
                    {
                        smry.InterHOInteruptTime = cnvtr.Double(dt.Rows[i]["InterHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("IntraHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IntraHOInteruptTime"].ToString()))
                    {
                        smry.IntraHOInteruptTime = cnvtr.Double(dt.Rows[i]["IntraHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("IntreHOInteruptTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IntreHOInteruptTime"].ToString()))
                    {
                        smry.IntreHOInteruptTime = cnvtr.Double(dt.Rows[i]["IntreHOInteruptTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("CCWHoInterruptionTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CCWHoInterruptionTime"].ToString()))
                    {
                        smry.CCWHoInterruptionTime = cnvtr.Double(dt.Rows[i]["CCWHoInterruptionTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("callSetupTime"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["callSetupTime"].ToString()))
                    {
                        smry.callSetupTime = cnvtr.Double(dt.Rows[i]["callSetupTime"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedAvg"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedAvg"].ToString()))
                    {
                        smry.PhyULSpeedAvg = cnvtr.Double(dt.Rows[i]["PhyULSpeedAvg"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedMax"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedMax"].ToString()))
                    {
                        smry.PhyULSpeedMax = cnvtr.Double(dt.Rows[i]["PhyULSpeedMax"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyULSpeedMin"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyULSpeedMin"].ToString()))
                    {
                        smry.PhyULSpeedMin = cnvtr.Double(dt.Rows[i]["PhyULSpeedMin"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedAvg"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedAvg"].ToString()))
                    {
                        smry.PhyDLSpeedAvg = cnvtr.Double(dt.Rows[i]["PhyDLSpeedAvg"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedMax"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedMax"].ToString()))
                    {
                        smry.PhyDLSpeedMax = cnvtr.Double(dt.Rows[i]["PhyDLSpeedMax"].ToString());

                    }
                }
                if (dt.Columns.Contains("PhyDLSpeedMin"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["PhyDLSpeedMin"].ToString()))
                    {
                        smry.PhyDLSpeedMin = cnvtr.Double(dt.Rows[i]["PhyDLSpeedMin"].ToString());

                    }
                }

                smry.DownlinkRate = cnvtr.Double(dt.Rows[i]["DownlinkRate"].ToString());
                smry.DownlinkMinResult = cnvtr.Double(dt.Rows[i]["DownlinkMinResult"].ToString());
                smry.DownlinkMaxResult = cnvtr.Double(dt.Rows[i]["DownlinkMaxResult"].ToString());
                smry.DownlinkAvgResult = cnvtr.Double(dt.Rows[i]["DownlinkAvgResult"].ToString());
                if (!string.IsNullOrEmpty(dt.Rows[i]["DownlinkStatus"].ToString()))
                {
                    smry.DownlinkStatus = cnvtr.Bool(dt.Rows[i]["DownlinkStatus"].ToString());
                }
                smry.UplinkRate = cnvtr.Double(dt.Rows[i]["UplinkRate"].ToString());
                smry.UplinkMinResult = cnvtr.Double(dt.Rows[i]["UplinkMinResult"].ToString());
                smry.UplinkMaxResult = cnvtr.Double(dt.Rows[i]["UplinkMaxResult"].ToString());
                smry.UplinkAvgResult = cnvtr.Double(dt.Rows[i]["UplinkAvgResult"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["UplinkStatus"].ToString()))
                {
                    smry.UplinkStatus = cnvtr.Bool(dt.Rows[i]["UplinkStatus"].ToString());
                }

                smry.ConnectionSetupTime = cnvtr.Double(dt.Rows[i]["ConnectionSetupTime"].ToString());
         
        
           
                

                if (!string.IsNullOrEmpty(dt.Rows[i]["ConnectionSetupStatus"].ToString()))
                {
                    smry.ConnectionSetupStatus = cnvtr.Bool(dt.Rows[i]["ConnectionSetupStatus"].ToString());
                }
                smry.MoMtCallNo = dt.Rows[i]["MoMtCallNo"].ToString();
                smry.MoMtCallDuration = cnvtr.Int32(dt.Rows[i]["MoMtCallDuration"].ToString());


                if (!string.IsNullOrEmpty(dt.Rows[i]["MoStatus"].ToString()))
                {
                    smry.MoStatus = cnvtr.Bool(dt.Rows[i]["MoStatus"].ToString());
                }
                if (dt.Columns.Contains("MinConSetupTime"))
                {


                    if (!string.IsNullOrEmpty(dt.Rows[i]["MaxConSetupTime"].ToString()))
                    {
                        smry.MaxConSetupTime = cnvtr.Double(dt.Rows[i]["MaxConSetupTime"].ToString());
                    }
                }

                if (dt.Columns.Contains("ActualSiteCode"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MobHandoverStatus"].ToString()))
                    {
                        smry.MobHandoverStatus = cnvtr.Bool(dt.Rows[i]["MobHandoverStatus"].ToString());
                    }
                }




                if (!string.IsNullOrEmpty(dt.Rows[i]["MtStatus"].ToString()))
                {
                    smry.MtStatus = cnvtr.Bool(dt.Rows[i]["MtStatus"].ToString());
                }
                smry.VMoMtCallno = dt.Rows[i]["VMoMtCallno"].ToString();
                smry.VMoMtCallDuration = cnvtr.Int32(dt.Rows[i]["VMoMtCallDuration"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[i]["VMoStatus"].ToString()))
                {
                    smry.VMoStatus = cnvtr.Bool(dt.Rows[i]["VMoStatus"].ToString());
                }
                if (dt.Columns.Contains("MimoStatus"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MimoStatus"].ToString()))
                    {
                        smry.MimoStatus = cnvtr.Bool(dt.Rows[i]["MimoStatus"].ToString());
                    }
                }
                if (dt.Columns.Contains("LaaSpeedTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["LaaSpeedTestFilePath"].ToString()))
                    {
                        smry.LaaSpeedTestFilePath = dt.Rows[i]["LaaSpeedTestFilePath"].ToString();
                    }
                }
                if (dt.Columns.Contains("LaaSmTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["LaaSmTestFilePath"].ToString()))
                    {
                        smry.LaaSmTestFilePath = dt.Rows[i]["LaaSmTestFilePath"].ToString();
                    }
                }
                if (dt.Columns.Contains("CaSpeedTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CaSpeedTestFilePath"].ToString()))
                    {
                        smry.CaSpeedTestFilePath = dt.Rows[i]["CaSpeedTestFilePath"].ToString();
                    }
                }
                if (dt.Columns.Contains("CaDeavticeTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CaDeavticeTestFilePath"].ToString()))
                    {
                        smry.CaDeavticeTestFilePath = dt.Rows[i]["CaDeavticeTestFilePath"].ToString();
                    }
                }
                if (dt.Columns.Contains("CaActiveTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CaActiveTestFilePath"].ToString()))
                    {
                        smry.CaActiveTestFilePath = dt.Rows[i]["CaActiveTestFilePath"].ToString();
                    }
                }
                if (dt.Columns.Contains("SpeedTestFilePath"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["SpeedTestFilePath"].ToString()))
                    {

                        smry.SpeedTestFilePath = dt.Rows[i]["SpeedTestFilePath"].ToString();
                    }
                }
                

                if (!string.IsNullOrEmpty(dt.Rows[i]["VMtStatus"].ToString()))
                {
                    smry.VMtStatus = cnvtr.Bool(dt.Rows[i]["VMtStatus"].ToString());
                }
              

                if (!string.IsNullOrEmpty(dt.Rows[i]["CwHandoverStatus"].ToString()))
                {
                    smry.CwHandoverStatus = cnvtr.Bool(dt.Rows[i]["CwHandoverStatus"].ToString());
                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["Ccwhandoverstatus"].ToString()))
                {
                    smry.Ccwhandoverstatus = cnvtr.Bool(dt.Rows[i]["Ccwhandoverstatus"].ToString());
                }
                smry.FtpServerIp = dt.Rows[i]["FtpServerIp"].ToString();
                smry.FtpServerPort = dt.Rows[i]["FtpServerPort"].ToString();
                smry.FtpServerPath = dt.Rows[i]["FtpServerPath"].ToString();

                smry.FtpDownlinkFile = dt.Rows[i]["FtpDownlinkFile"].ToString();
                smry.Latitude = dt.Rows[i]["Latitude"].ToString();// cnvtr.Double(dt.Rows[i]["Latitude"].ToString());
                smry.Longitude = dt.Rows[i]["Longitude"].ToString();// cnvtr.Double(dt.Rows[i]["Longitude"].ToString());


                smry.TestLatitude = cnvtr.Double(dt.Rows[i]["TestLatitude"].ToString());
                smry.TestLongitude = cnvtr.Double(dt.Rows[i]["TestLongitude"].ToString());


                smry.OoklaTestFilePath = dt.Rows[i]["OoklaTestFilePath"].ToString();
                smry.OoklaDownlinkResult = (!string.IsNullOrEmpty(dt.Rows[i]["OoklaDownlinkResult"].ToString())) ? Convert.ToDouble(dt.Rows[i]["OoklaDownlinkResult"].ToString()) : 0;
                smry.OoklaPingResult = (!string.IsNullOrEmpty(dt.Rows[i]["OoklaPingResult"].ToString())) ? Convert.ToDouble(dt.Rows[i]["OoklaPingResult"].ToString()) : 0;
                smry.OoklaUplinkResult = (!string.IsNullOrEmpty(dt.Rows[i]["OoklaUplinkResult"].ToString())) ? Convert.ToDouble(dt.Rows[i]["OoklaUplinkResult"].ToString()) : 0;
                smry.ClientLogo = dt.Rows[i]["ClientLogo"].ToString();
                smry.VendorLogo = dt.Rows[i]["VendorLogo"].ToString();


                if (!string.IsNullOrEmpty(dt.Rows[i]["ICwHandoverStatus"].ToString()))
                {
                    smry.ICwHandoverStatus = cnvtr.Bool(dt.Rows[i]["ICwHandoverStatus"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["ICcwhandoverstatus"].ToString()))
                {
                    smry.ICcwhandoverstatus = cnvtr.Bool(dt.Rows[i]["ICcwhandoverstatus"].ToString());
                }

                smry.SiteName = dt.Rows[i]["SiteName"].ToString();
                smry.SiteType = (dt.Columns.Contains("SiteType")) ? dt.Rows[i]["SiteType"].ToString() : null;
                smry.SiteClass = (dt.Columns.Contains("SiteClass")) ? dt.Rows[i]["SiteClass"].ToString() : null;
                smry.LayerStatus = (dt.Columns.Contains("LayerStatus")) ? dt.Rows[i]["LayerStatus"].ToString() : null;
                if (dt.Columns.Contains("RFHeight"))
                {
                    smry.RFHeight = (!string.IsNullOrEmpty(dt.Rows[i]["RFHeight"].ToString())) ? Convert.ToDouble(dt.Rows[i]["RFHeight"].ToString()) : 0; //cdt.ToDouble(dt, i, "RFHeight");//Convert.ToDouble( dt.Rows[i]["RFHeight"].ToString());
                }
                if (dt.Columns.Contains("ETilt"))
                {
                    smry.ETilt = (!string.IsNullOrEmpty(dt.Rows[i]["ETilt"].ToString())) ? Convert.ToInt32(dt.Rows[i]["ETilt"].ToString()) : 0;
                }
                if (dt.Columns.Contains("MTilt"))
                {
                    smry.MTilt = (!string.IsNullOrEmpty(dt.Rows[i]["MTilt"].ToString())) ? Convert.ToInt32(dt.Rows[i]["MTilt"].ToString()) : 0;
                }
                if (dt.Columns.Contains("CellId"))
                {
                    smry.CellId = dt.Rows[i]["CellId"].ToString();
                }
                if (dt.Columns.Contains("Project"))
                {
                    smry.Project = dt.Rows[i]["Project"].ToString();
                }

                smry.CwTestFilePath = dt.Rows[i]["CwTestFilePath"].ToString();

                smry.StationaryTestFilePath = dt.Rows[i]["StationaryTestFilePath"].ToString();


                if (dt.Columns.Contains("SMoStatus") && !string.IsNullOrEmpty(dt.Rows[i]["SMoStatus"].ToString()))
                {
                    smry.SMoStatus = bool.Parse(dt.Rows[i]["SMoStatus"].ToString());
                }

                if (dt.Columns.Contains("SMtStatus") && !string.IsNullOrEmpty(dt.Rows[i]["SMtStatus"].ToString()))
                {
                    smry.SMtStatus = bool.Parse(dt.Rows[i]["SMtStatus"].ToString());
                }

                if (dt.Columns.Contains("CarrierAggregationStatus") && !string.IsNullOrEmpty(dt.Rows[i]["CarrierAggregationStatus"].ToString()))
                {
                    smry.CarrierAggregationStatus = bool.Parse(dt.Rows[i]["CarrierAggregationStatus"].ToString());
                }

                if (dt.Columns.Contains("E911Status") && !string.IsNullOrEmpty(dt.Rows[i]["E911Status"].ToString()))
                {
                    smry.E911Status = bool.Parse(dt.Rows[i]["E911Status"].ToString());
                }

                smry.ClientPrefix = (dt.Columns.Contains("ClientPrefix")) ? dt.Rows[i]["ClientPrefix"].ToString() : null;

                if (dt.Columns.Contains("SiteAddress"))
                {
                    smry.SiteAddress = dt.Rows[i]["SiteAddress"].ToString();
                }

                smry.SectorColor = (dt.Columns.Contains("SectorColor")) ? dt.Rows[i]["SectorColor"].ToString() : "white";

                smry.MRBTS = (dt.Columns.Contains("MRBTS")) ? dt.Rows[i]["MRBTS"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(dt.Rows[i]["IRATHandover"].ToString()))
                {
                    smry.IRATHandover = cnvtr.Bool(dt.Rows[i]["IRATHandover"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["FastReturnStatus"].ToString()))
                {
                    smry.FastReturnStatus = cnvtr.Bool(dt.Rows[i]["FastReturnStatus"].ToString());

                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["CoverageDistance"].ToString()))
                {
                    smry.CoverageDistance = Convert.ToDouble(dt.Rows[i]["CoverageDistance"].ToString());

                }

                if (!string.IsNullOrEmpty(dt.Rows[i]["SampleCount"].ToString()))
                {
                    smry.SampleCount = Convert.ToInt64(dt.Rows[i]["SampleCount"].ToString());

                }
                smry.TCH = (dt.Columns.Contains("TCH")) ? dt.Rows[i]["TCH"].ToString() : string.Empty;


                //---------------Siddique----------
                if (dt.Columns.Contains("DLLat"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DLLat"].ToString()))
                    {
                        smry.DLLat = Convert.ToDouble(dt.Rows[i]["DLLat"].ToString());
                    }
                }
                if (dt.Columns.Contains("DLLng"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DLLng"].ToString()))
                    {
                        smry.DLLng = Convert.ToDouble(dt.Rows[i]["DLLng"].ToString());
                    }
                }
                if (dt.Columns.Contains("DLSignalStrength"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DLSignalStrength"].ToString()))
                    {
                        smry.DLSignalStrength = Convert.ToDouble(dt.Rows[i]["DLSignalStrength"].ToString());
                    }
                }
                if (dt.Columns.Contains("DLSignalQuality"))
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i]["DLSignalQuality"].ToString()))
                    {
                        smry.DLSignalQuality = Convert.ToDouble(dt.Rows[i]["DLSignalQuality"].ToString());
                    }
                }
                if (dt.Columns.Contains("DLSignalNoise"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DLSignalNoise"].ToString()))
                    {
                        smry.DLSignalNoise = Convert.ToDouble(dt.Rows[i]["DLSignalNoise"].ToString());
                    }
                }
                //-------------------------
                if (dt.Columns.Contains("CSLat"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CSLat"].ToString()))
                    {
                        smry.CSLat = Convert.ToDouble(dt.Rows[i]["CSLat"].ToString());
                    }
                }
                if (dt.Columns.Contains("CSLng"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CSLng"].ToString()))
                    {
                        smry.CSLng = Convert.ToDouble(dt.Rows[i]["CSLng"].ToString());
                    }
                }
                if (dt.Columns.Contains("CSSignalStrength"))
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i]["CSSignalStrength"].ToString()))
                {
                    smry.CSSignalStrength = Convert.ToDouble(dt.Rows[i]["CSSignalStrength"].ToString());
                }
                }
                if (dt.Columns.Contains("CSSignalQuality"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CSSignalQuality"].ToString()))
                {
                    smry.CSSignalQuality = Convert.ToDouble(dt.Rows[i]["CSSignalQuality"].ToString());
                }
                }
                if (dt.Columns.Contains("CSSignalNoise"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["CSSignalNoise"].ToString()))
                {
                    smry.CSSignalNoise = Convert.ToDouble(dt.Rows[i]["CSSignalNoise"].ToString());
                    }
                }
                if (dt.Columns.Contains("RttLat"))
                {
                    //-----------------------------
                    if (!string.IsNullOrEmpty(dt.Rows[i]["RttLat"].ToString()))
                {
                    smry.RttLat = Convert.ToDouble(dt.Rows[i]["RttLat"].ToString());
                    }
                }
                if (dt.Columns.Contains("RttLng"))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["RttLng"].ToString()))
                {
                    smry.RttLng = Convert.ToDouble(dt.Rows[i]["RttLng"].ToString());
                    }
                }
                if (dt.Columns.Contains("RttSignalStrength"))
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i]["RttSignalStrength"].ToString()))
                {
                    smry.RttSignalStrength = Convert.ToDouble(dt.Rows[i]["RttSignalStrength"].ToString());
                    }
                }
                if (dt.Columns.Contains("RttSignalQuality"))
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i]["RttSignalQuality"].ToString()))
                {
                    smry.RttSignalQuality = Convert.ToDouble(dt.Rows[i]["RttSignalQuality"].ToString());
                    }
                }
                if (dt.Columns.Contains("RttSignalNoise"))
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i]["RttSignalNoise"].ToString()))
                {
                    smry.RttSignalNoise = Convert.ToDouble(dt.Rows[i]["RttSignalNoise"].ToString());
                    }
                }
               







                    lst.Add(smry);
            }

            #endregion
            return lst;
        }
        public bool Manage(string Filter, string SiteId, string SectorId, string NetworkModeId, string BandId, string CarrierId, string ScopeId, string TestType, string Ping,
                          string Value4, string Value3, string ImagePath, string Value1, string Value2, Int64 UserId,string CWComments=null,string CCWComments=null,string PDSCHComments=null,string PUSCHComments=null)
        {
            AV_SiteTestSummaryDL DL = new AV_SiteTestSummaryDL();
            return DL.Manage(Filter, SiteId, SectorId, NetworkModeId, BandId, CarrierId, ScopeId, TestType, Ping, Value4, Value3, ImagePath, Value1, Value2, UserId, CWComments, CCWComments, PDSCHComments, PUSCHComments);
        }



    }
}
