using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/

    public class AV_SiteTestLogBL
    {
        AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
       
        public bool Save(List<AV_SiteTestLog> rec, string Status)
        {
            try
            {
                DataTable SiteTestLog = new DataTable();

                #region DataTable
                SiteTestLog.Columns.AddRange(new DataColumn[135]
                {
                                    new DataColumn("TestType", typeof (string)),
                                    new DataColumn("TimeStamp", typeof (string)),
                                    new DataColumn("Latitude", typeof (double)),
                                    new DataColumn("Longitude", typeof (double)),
                                    new DataColumn("MccId", typeof (string)),
                                    new DataColumn("MncId", typeof (string)),
                                    new DataColumn("Site", typeof (string)),
                                    new DataColumn("Sector", typeof (string)),
                                      new DataColumn("SubNetworkMode", typeof (string)),
                                    new DataColumn("ActualBand", typeof (string)),
                                    new DataColumn("ActualCarrier", typeof (string)),
                                    new DataColumn("Scope", typeof (string)),

                                    new DataColumn("Band", typeof (string)),
                                    new DataColumn("Carrier", typeof (string)),
                                    new DataColumn("NetworkMode", typeof (string)),
                                    new DataColumn("CellId", typeof (string)),
                                    new DataColumn("LacId", typeof (string)),
                                    new DataColumn("PciId", typeof (string)),
                                    new DataColumn("GsmRssi", typeof (float)),
                                    new DataColumn("GsmRxQual", typeof (float)),
                                     new DataColumn("WcdmaRscp", typeof (float)),
                                    new DataColumn("WcdmaEcio", typeof (float)),
                                    new DataColumn("LteRssi", typeof (float)),

                                    new DataColumn("LteRsrp", typeof (float)),
                                    new DataColumn("LteRsrq", typeof (float)),
                                    new DataColumn("LteRsnr", typeof (float)),
                                    new DataColumn("LteCqi", typeof (float)),
                                    
                                    new DataColumn("DistanceFromSite", typeof (float)),
                                    new DataColumn("AngleToSite", typeof (float)),
                                    new DataColumn("StackTrace", typeof (string)),
                                    new DataColumn("TestResult", typeof (float)),
                                    new DataColumn("FtpStatus", typeof (string)),
                                    new DataColumn("IsHandover", typeof (int)),

                                    new DataColumn("SiteRefId", typeof (int)),
                                    new DataColumn("SectorRefId", typeof (int)),
                                    new DataColumn("NetworkModeId", typeof (int)),
                                    new DataColumn("BandId", typeof (int)),
                                    new DataColumn("CarrierId", typeof (int)),
                                    new DataColumn("ScopeId", typeof (int)),
                                    new DataColumn("WoRefId", typeof (string)),
                                     new DataColumn("FloorId",typeof (long)),
                                    new DataColumn("RRCState",typeof (String)),

                                    new DataColumn("NetMode1",typeof (String)),
                                    new DataColumn("Band1",typeof (String)),
                                    new DataColumn("CH1",typeof (String)),
                                    new DataColumn("PCI1",typeof (int)),
                                    new DataColumn("CI1",typeof (double)),
                                    new DataColumn("SS1",typeof (double)),
                                    new DataColumn("SP1",typeof (double)),
                                    new DataColumn("SQ1",typeof (double)),
                                    new DataColumn("SNR1",typeof (double)),

                                     new DataColumn("NetMode2",typeof (String)),
                                    new DataColumn("Band2",typeof (String)),
                                    new DataColumn("CH2",typeof (String)),
                                    new DataColumn("PCI2",typeof (int)),
                                    new DataColumn("CI2",typeof (double)),
                                    new DataColumn("SS2",typeof (double)),
                                    new DataColumn("SP2",typeof (double)),
                                    new DataColumn("SQ2",typeof (double)),
                                    new DataColumn("SNR2",typeof (double)),

                                     new DataColumn("NetMode3",typeof (String)),
                                    new DataColumn("Band3",typeof (String)),
                                    new DataColumn("CH3",typeof (String)),
                                    new DataColumn("PCI3",typeof (int)),
                                    new DataColumn("CI3",typeof (double)),
                                    new DataColumn("SS3",typeof (double)),
                                    new DataColumn("SP3",typeof (double)),
                                    new DataColumn("SQ3",typeof (double)),
                                    new DataColumn("SNR3",typeof (double)),

                                     new DataColumn("NetMode4",typeof (String)),
                                    new DataColumn("Band4",typeof (String)),
                                    new DataColumn("CH4",typeof (String)),
                                    new DataColumn("PCI4",typeof (int)),
                                    new DataColumn("CI4",typeof (double)),
                                    new DataColumn("SS4",typeof (double)),
                                    new DataColumn("SP4",typeof (double)),
                                    new DataColumn("SQ4",typeof (double)),
                                    new DataColumn("SNR4",typeof (double)),

                                     new DataColumn("NetMode5",typeof (String)),
                                    new DataColumn("Band5",typeof (String)),
                                    new DataColumn("CH5",typeof (String)),
                                    new DataColumn("PCI5",typeof (int)),
                                    new DataColumn("CI5",typeof (double)),
                                    new DataColumn("SS5",typeof (double)),
                                    new DataColumn("SP5",typeof (double)),
                                    new DataColumn("SQ5",typeof (double)),
                                    new DataColumn("SNR5",typeof (double)),

                                     new DataColumn("NetMode6",typeof (String)),
                                    new DataColumn("Band6",typeof (String)),
                                    new DataColumn("CH6",typeof (String)),
                                    new DataColumn("PCI6",typeof (int)),
                                    new DataColumn("CI6",typeof (double)),
                                    new DataColumn("SS6",typeof (double)),
                                    new DataColumn("SP6",typeof (double)),
                                    new DataColumn("SQ6",typeof (double)),
                                    new DataColumn("SNR6",typeof (double)),

                                     new DataColumn("NetMode7",typeof (String)),
                                    new DataColumn("Band7",typeof (String)),
                                    new DataColumn("CH7",typeof (String)),
                                    new DataColumn("PCI7",typeof (int)),
                                    new DataColumn("CI7",typeof (double)),
                                    new DataColumn("SS7",typeof (double)),
                                    new DataColumn("SP7",typeof (double)),
                                    new DataColumn("SQ7",typeof (double)),
                                    new DataColumn("SNR7",typeof (double)),

                                     new DataColumn("NetMode8",typeof (String)),
                                    new DataColumn("Band8",typeof (String)),
                                    new DataColumn("CH8",typeof (String)),
                                    new DataColumn("PCI8",typeof (int)),
                                    new DataColumn("CI8",typeof (double)),
                                    new DataColumn("SS8",typeof (double)),
                                    new DataColumn("SP8",typeof (double)),
                                    new DataColumn("SQ8",typeof (double)),
                                    new DataColumn("SNR8",typeof (double)),
                                    new DataColumn("TCH",typeof (string)),
                                    new DataColumn("FromPCI",typeof (string)),
                                    new DataColumn("ToPCI",typeof (string)),

                                    new DataColumn("PRBPercent",typeof(float)),
                                    new DataColumn("MCS",typeof(String)),
                                    new DataColumn("RB",typeof(int)),
                                    new DataColumn("Modulation",typeof(String)),
                                    new DataColumn("ModPercent",typeof(String)),
                                    new DataColumn("TM",typeof(String)),
                                    new DataColumn("RI",typeof(int)),
                                    new DataColumn("PCPDSCH",typeof(float)),
                                    new DataColumn("PCPUSCH",typeof(float)),
                                    new DataColumn("SCPDSCH",typeof(float)),
                                    new DataColumn("SCPUSCH",typeof(float)),
                                    new DataColumn("NRBand", typeof (string)),
                                    new DataColumn("NRCarrier", typeof (string)),
                                    new DataColumn("NRRsrp", typeof (float)),
                                    new DataColumn("NRRsrq", typeof (float)),
                                    new DataColumn("NRRsnr", typeof (float)),
                                    new DataColumn("NRCqi", typeof (float)),
                                    new DataColumn("NRPci", typeof (float)),

    });
                /*
                 
               
                 */
                #endregion

                #region Add Rows In DataTable
                foreach (var item in rec)
                {
                    DataRow row;
                    row = SiteTestLog.NewRow();

                    row["TestType"] = item.TestType;
                    row["TimeStamp"] = item.TimeStamp;
                    row["Latitude"] = item.Latitude;
                    row["Longitude"] = item.Longitude;
                    row["MccId"] = item.MccId;
                    row["MncId"] = item.MncId;
                    row["Site"] = item.Site;
                    row["Sector"] = item.Sector;
                    row["Band"] = item.Band;
                    row["Carrier"] = item.Carrier;
                    row["NetworkMode"] = item.NetworkMode;
                    row["CellId"] = item.CellId;
                    row["LacId"] = item.LacId;
                    row["PciId"] = item.PciId;
                    row["GsmRssi"] = item.GsmRssi;
                    row["GsmRxQual"] = item.GsmRxQual;
                    row["LteRsrp"] = item.LteRsrp;
                    row["LteRsrq"] = item.LteRsrq;
                    row["LteRsnr"] = item.LteRsnr;
                    row["LteCqi"] = item.LteCqi;
                    row["NRBand"] = item.NRBand;
                    row["NRCarrier"] = item.NRCarrier;
                    row["NRRsrp"] = item.NRRsrp;
                    row["NRRsrq"] = item.NRRsrq;
                    row["NRRsnr"] = item.NRRsnr;
                    row["NRCqi"] = item.NRCqi;
                    row["NRPci"] = item.NRPci;
                    row["DistanceFromSite"] = item.DistanceFromSite;
                    row["AngleToSite"] = item.AngleToSite;
                    row["StackTrace"] = item.StackTrace;
                    row["TestResult"] = item.TestResult;
                    row["FtpStatus"] =( item.FtpStatus=="Running")?1:0;

                   
                    row["WcdmaRscp"] = item.WcdmaRscp;
                    row["WcdmaEcio"] = item.WcdmaEcio;

                    row["SubNetworkMode"] = item.SubNetworkMode;
                    row["ActualBand"] = item.ActualBand;
                    row["ActualCarrier"] = item.ActualCarrier;
                    row["IsHandover"] = item.IsHandover;


                    row["LteRssi"] = item.LteRssi;
                    row["Scope"] = item.Scope;


                    row["SiteRefId"] = item.SiteRefId;
                    row["SectorRefId"] = item.SectorRefId;
                    row["NetworkModeId"] = item.NetworkModeId;
                    row["CarrierId"] = item.CarrierId;
                    row["BandId"] = item.BandId;
                    row["ScopeId"] = item.ScopeId;
                    row["WoRefId"] = item.WoRefId;
                    row["FloorId"] = item.FloorId;
                    row["RRCState"] = item.RRCState;


                    row["NetMode1"] = item.NetMode1;
                    row["Band1"] = item.Band1;
                    row["CH1"] = item.CH1;
                    row["PCI1"] = item.PCI1;
                    row["CI1"] = item.CI1;
                    row["SS1"] = item.SS1;
                    row["SP1"] = item.SP1;
                    row["SQ1"] = item.SQ1;
                    row["SNR1"] = item.SNR1;

                    row["NetMode2"] = item.NetMode2;
                    row["Band2"] = item.Band2;
                    row["CH2"] = item.CH2;
                    row["PCI2"] = item.PCI2;
                    row["CI2"] = item.CI2;
                    row["SS2"] = item.SS2;
                    row["SP2"] = item.SP2;
                    row["SQ2"] = item.SQ2;
                    row["SNR2"] = item.SNR2;

                    row["NetMode3"] = item.NetMode3;
                    row["Band3"] = item.Band3;
                    row["CH3"] = item.CH3;
                    row["PCI3"] = item.PCI3;
                    row["CI3"] = item.CI3;
                    row["SS3"] = item.SS3;
                    row["SP3"] = item.SP3;
                    row["SQ3"] = item.SQ3;
                    row["SNR3"] = item.SNR3;

                    row["NetMode4"] = item.NetMode4;
                    row["Band4"] = item.Band4;
                    row["CH4"] = item.CH4;
                    row["PCI4"] = item.PCI4;
                    row["CI4"] = item.CI4;
                    row["SS4"] = item.SS4;
                    row["SP4"] = item.SP4;
                    row["SQ4"] = item.SQ4;
                    row["SNR4"] = item.SNR4;

                    row["NetMode5"] = item.NetMode5;
                    row["Band5"] = item.Band5;
                    row["CH5"] = item.CH5;
                    row["PCI5"] = item.PCI5;
                    row["CI5"] = item.CI5;
                    row["SS5"] = item.SS5;
                    row["SP5"] = item.SP5;
                    row["SQ5"] = item.SQ5;
                    row["SNR5"] = item.SNR5;

                    row["NetMode6"] = item.NetMode6;
                    row["Band6"] = item.Band6;
                    row["CH6"] = item.CH6;
                    row["PCI6"] = item.PCI6;
                    row["CI6"] = item.CI6;
                    row["SS6"] = item.SS6;
                    row["SP6"] = item.SP6;
                    row["SQ6"] = item.SQ6;
                    row["SNR6"] = item.SNR6;

                    row["NetMode7"] = item.NetMode7;
                    row["Band7"] = item.Band7;
                    row["CH7"] = item.CH7;
                    row["PCI7"] = item.PCI7;
                    row["CI7"] = item.CI7;
                    row["SS7"] = item.SS7;
                    row["SP7"] = item.SP7;
                    row["SQ7"] = item.SQ7;
                    row["SNR7"] = item.SNR7;

                    row["NetMode8"] = item.NetMode8;
                    row["Band8"] = item.Band8;
                    row["CH8"] = item.CH8;
                    row["PCI8"] = item.PCI8;
                    row["CI8"] = item.CI8;
                    row["SS8"] = item.SS8;
                    row["SP8"] = item.SP8;
                    row["SQ8"] = item.SQ8;
                    row["SNR8"] = item.SNR8;
                    row["TCH"] = item.TCH;
                    row["FromPCI"] = item.FromPCI;
                    row["ToPCI"] = item.ToPCI;

                    row["PRBPercent"] = item.PRBPercent;
                    row["MCS"] = item.MCS;
                    row["RB"] = item.RB;
                    row["Modulation"] = item.Modulation;
                    row["ModPercent"] = item.ModPercent;
                    row["TM"] = item.TM;
                    row["RI"] = item.RI;
                    row["PCPDSCH"] = item.PCPDSCH;
                    row["PCPUSCH"] = item.PCPUSCH;
                    row["SCPDSCH"] = item.SCPDSCH;
                    row["SCPUSCH"] = item.SCPUSCH;
                    SiteTestLog.Rows.Add(row);
                }
                #endregion


                AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
                stld.Insert(SiteTestLog, Status);

                return true;
            }
            catch 
            {

                throw;
            }
        }

        public bool SaveBeamTest(List<AV_BeamTestLog> rec, string Status=null)
        {
            try
            {
                DataTable BeamTestLog = new DataTable();

                #region DataTable
                BeamTestLog.Columns.AddRange(new DataColumn[28]
                {
                                    new DataColumn("TimeStamp",typeof(DateTime)),
                                    new DataColumn("SiteId",typeof(Int64)),
                                    new DataColumn("SectorId",typeof(Int64)),
                                    new DataColumn("NetworkModeId",typeof(Int64)),
                                    new DataColumn("ScopeId",typeof(Int64)),
                                    new DataColumn("BandId",typeof(Int64)),
                                    new DataColumn("CarrierId",typeof(Int64)),
                                    new DataColumn("LayerStatusId",typeof(Int64)),
                                    new DataColumn("BeamGroupId",typeof(int)),
                                    new DataColumn("BeamId",typeof(int)),
                                    new DataColumn("BMGColor",typeof(string)),
                                    new DataColumn("BMColor",typeof(string)),
                                    new DataColumn("Latitude",typeof(float)),
                                    new DataColumn("Longitude",typeof(float)),
                                    new DataColumn("PCIId",typeof(int)),
                                    new DataColumn("SSBIndex",typeof(int)),
                                    new DataColumn("NRRSRP0",typeof(float)),
                                    new DataColumn("NRRSRP1",typeof(float)),
                                    new DataColumn("NRRSRP2",typeof(float)),
                                    new DataColumn("NRRSRP3",typeof(float)),
                                    new DataColumn("NRRSRQ0",typeof(float)),
                                    new DataColumn("NRRSRQ1",typeof(float)),
                                    new DataColumn("NRRSRQ2",typeof(float)),
                                    new DataColumn("NRRSRQ3",typeof(float)),
                                    new DataColumn("NRRSNR0",typeof(float)),
                                    new DataColumn("NRRSNR1",typeof(float)),
                                    new DataColumn("NRRSNR2",typeof(float)),
                                    new DataColumn("NRRSNR3",typeof(float)),


    }
                );
                #endregion

                #region Add Rows In DataTable
                foreach (var item in rec)
                {
                    DataRow row;

                    row = BeamTestLog.NewRow();

                    row["TimeStamp"] = item.TimeStamp;
                    row["SiteId"] = item.SiteId;
                    row["SectorId"] = item.SectorId;
                    row["NetworkModeId"] = item.NetworkModeId;
                    row["ScopeId"] = item.ScopeId;
                    row["BandId"] = item.BandId;
                    row["CarrierId"] = item.CarrierId;
                    row["LayerStatusId"] = item.LayerStatusId;
                    row["BeamGroupId"] = item.BeamGroupId;
                    row["BeamId"] = item.BeamId;
                    row["BMGColor"] = item.BMGColor;
                    row["BMColor"] = item.BMColor;
                    row["Latitude"] = item.Latitude;
                    row["Longitude"] = item.Longitude;
                    row["PCIId"] = item.PCIId;
                    row["SSBIndex"] = item.SSBIndex;
                    row["NRRSRP0"] = item.NRRSRP0;
                    row["NRRSRP1"] = item.NRRSRP1;
                    row["NRRSRP2"] = item.NRRSRP2;
                    row["NRRSRP3"] = item.NRRSRP3;
                    row["NRRSRQ0"] = item.NRRSRQ0;
                    row["NRRSRQ1"] = item.NRRSRQ1;
                    row["NRRSRQ2"] = item.NRRSRQ2;
                    row["NRRSRQ3"] = item.NRRSRQ3;
                    row["NRRSNR0"] = item.NRRSNR0;
                    row["NRRSNR1"] = item.NRRSNR1;
                    row["NRRSNR2"] = item.NRRSNR2;
                    row["NRRSNR3"] = item.NRRSNR3;
                    

                    BeamTestLog.Rows.Add(row);
                }
                #endregion


                AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
                stld.InsertBeamTest(BeamTestLog, Status);

                return true;
            }
            catch
            {

                throw;
            }
        }


        public List<AV_SiteTestLog> ToList(string Filter, Int64 SiteId, Int64 NetworkmodeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId)
        {
            DataTable dt = stld.Get(Filter, SiteId, NetworkmodeId, BandId, CarrierId, ScopeId);
            return dt.ToList<AV_SiteTestLog>();
        }

        public bool SaveFromDT(DataTable rec,string TestType,bool Status)
        {
            try
            {
                DataTable SiteTestLog = new DataTable();

                #region DataTable
                SiteTestLog.Columns.AddRange(new DataColumn[135]
                {
                                    new DataColumn("TestType", typeof (string)),
                                    new DataColumn("TimeStamp", typeof (string)),
                                    new DataColumn("Latitude", typeof (double)),
                                    new DataColumn("Longitude", typeof (double)),
                                    new DataColumn("MccId", typeof (string)),
                                    new DataColumn("MncId", typeof (string)),
                                    new DataColumn("Site", typeof (string)),
                                    new DataColumn("Sector", typeof (string)),
                                      new DataColumn("SubNetworkMode", typeof (string)),
                                    new DataColumn("ActualBand", typeof (string)),
                                    new DataColumn("ActualCarrier", typeof (string)),
                                    new DataColumn("Scope", typeof (string)),

                                    new DataColumn("Band", typeof (string)),
                                    new DataColumn("Carrier", typeof (string)),
                                    new DataColumn("NetworkMode", typeof (string)),
                                    new DataColumn("CellId", typeof (string)),
                                    new DataColumn("LacId", typeof (string)),
                                    new DataColumn("PciId", typeof (string)),
                                    new DataColumn("GsmRssi", typeof (float)),
                                    new DataColumn("GsmRxQual", typeof (float)),
                                     new DataColumn("WcdmaRscp", typeof (float)),
                                    new DataColumn("WcdmaEcio", typeof (float)),
                                    new DataColumn("LteRssi", typeof (float)),

                                    new DataColumn("LteRsrp", typeof (float)),
                                    new DataColumn("LteRsrq", typeof (float)),
                                    new DataColumn("LteRsnr", typeof (float)),
                                    new DataColumn("LteCqi", typeof (float)),

                                    new DataColumn("DistanceFromSite", typeof (float)),
                                    new DataColumn("AngleToSite", typeof (float)),
                                    new DataColumn("StackTrace", typeof (string)),
                                    new DataColumn("TestResult", typeof (float)),
                                    new DataColumn("FtpStatus", typeof (string)),
                                    new DataColumn("IsHandover", typeof (int)),

                                    new DataColumn("SiteRefId", typeof (int)),
                                    new DataColumn("SectorRefId", typeof (int)),
                                    new DataColumn("NetworkModeId", typeof (int)),
                                    new DataColumn("BandId", typeof (int)),
                                    new DataColumn("CarrierId", typeof (int)),
                                    new DataColumn("ScopeId", typeof (int)),
                                    new DataColumn("WoRefId", typeof (string)),
                                     new DataColumn("FloorId",typeof (long)),
                                    new DataColumn("RRCState",typeof (String)),

                                    new DataColumn("NetMode1",typeof (String)),
                                    new DataColumn("Band1",typeof (String)),
                                    new DataColumn("CH1",typeof (String)),
                                    new DataColumn("PCI1",typeof (int)),
                                    new DataColumn("CI1",typeof (double)),
                                    new DataColumn("SS1",typeof (double)),
                                    new DataColumn("SP1",typeof (double)),
                                    new DataColumn("SQ1",typeof (double)),
                                    new DataColumn("SNR1",typeof (double)),

                                     new DataColumn("NetMode2",typeof (String)),
                                    new DataColumn("Band2",typeof (String)),
                                    new DataColumn("CH2",typeof (String)),
                                    new DataColumn("PCI2",typeof (int)),
                                    new DataColumn("CI2",typeof (double)),
                                    new DataColumn("SS2",typeof (double)),
                                    new DataColumn("SP2",typeof (double)),
                                    new DataColumn("SQ2",typeof (double)),
                                    new DataColumn("SNR2",typeof (double)),

                                     new DataColumn("NetMode3",typeof (String)),
                                    new DataColumn("Band3",typeof (String)),
                                    new DataColumn("CH3",typeof (String)),
                                    new DataColumn("PCI3",typeof (int)),
                                    new DataColumn("CI3",typeof (double)),
                                    new DataColumn("SS3",typeof (double)),
                                    new DataColumn("SP3",typeof (double)),
                                    new DataColumn("SQ3",typeof (double)),
                                    new DataColumn("SNR3",typeof (double)),

                                     new DataColumn("NetMode4",typeof (String)),
                                    new DataColumn("Band4",typeof (String)),
                                    new DataColumn("CH4",typeof (String)),
                                    new DataColumn("PCI4",typeof (int)),
                                    new DataColumn("CI4",typeof (double)),
                                    new DataColumn("SS4",typeof (double)),
                                    new DataColumn("SP4",typeof (double)),
                                    new DataColumn("SQ4",typeof (double)),
                                    new DataColumn("SNR4",typeof (double)),

                                     new DataColumn("NetMode5",typeof (String)),
                                    new DataColumn("Band5",typeof (String)),
                                    new DataColumn("CH5",typeof (String)),
                                    new DataColumn("PCI5",typeof (int)),
                                    new DataColumn("CI5",typeof (double)),
                                    new DataColumn("SS5",typeof (double)),
                                    new DataColumn("SP5",typeof (double)),
                                    new DataColumn("SQ5",typeof (double)),
                                    new DataColumn("SNR5",typeof (double)),

                                     new DataColumn("NetMode6",typeof (String)),
                                    new DataColumn("Band6",typeof (String)),
                                    new DataColumn("CH6",typeof (String)),
                                    new DataColumn("PCI6",typeof (int)),
                                    new DataColumn("CI6",typeof (double)),
                                    new DataColumn("SS6",typeof (double)),
                                    new DataColumn("SP6",typeof (double)),
                                    new DataColumn("SQ6",typeof (double)),
                                    new DataColumn("SNR6",typeof (double)),

                                     new DataColumn("NetMode7",typeof (String)),
                                    new DataColumn("Band7",typeof (String)),
                                    new DataColumn("CH7",typeof (String)),
                                    new DataColumn("PCI7",typeof (int)),
                                    new DataColumn("CI7",typeof (double)),
                                    new DataColumn("SS7",typeof (double)),
                                    new DataColumn("SP7",typeof (double)),
                                    new DataColumn("SQ7",typeof (double)),
                                    new DataColumn("SNR7",typeof (double)),

                                     new DataColumn("NetMode8",typeof (String)),
                                    new DataColumn("Band8",typeof (String)),
                                    new DataColumn("CH8",typeof (String)),
                                    new DataColumn("PCI8",typeof (int)),
                                    new DataColumn("CI8",typeof (double)),
                                    new DataColumn("SS8",typeof (double)),
                                    new DataColumn("SP8",typeof (double)),
                                    new DataColumn("SQ8",typeof (double)),
                                    new DataColumn("SNR8",typeof (double)),
                                    new DataColumn("TCH",typeof (string)),
                                    new DataColumn("FromPCI",typeof (string)),
                                    new DataColumn("ToPCI",typeof (string)),

                                    new DataColumn("PRBPercent",typeof(float)),
                                    new DataColumn("MCS",typeof(String)),
                                    new DataColumn("RB",typeof(int)),
                                    new DataColumn("Modulation",typeof(String)),
                                    new DataColumn("ModPercent",typeof(String)),
                                    new DataColumn("TM",typeof(String)),
                                    new DataColumn("RI",typeof(int)),
                                    new DataColumn("PCPDSCH",typeof(float)),
                                    new DataColumn("PCPUSCH",typeof(float)),
                                    new DataColumn("SCPDSCH",typeof(float)),
                                    new DataColumn("SCPUSCH",typeof(float)),
                                    new DataColumn("NRBand", typeof (string)),
                                    new DataColumn("NRCarrier", typeof (string)),
                                    new DataColumn("NRRsrp", typeof (float)),
                                    new DataColumn("NRRsrq", typeof (float)),
                                    new DataColumn("NRRsnr", typeof (float)),
                                    new DataColumn("NRCqi", typeof (float)),
                                    new DataColumn("NRPci", typeof (float)),

    });
                /*
                 
               
                 */
                #endregion


                #region Add Rows In DataTable
                foreach (DataRow item in rec.Rows)
                {
                    DataRow row;
                    row = SiteTestLog.NewRow();

                    row["TestType"] = TestType;
                    row["TimeStamp"] = item["time_stamp"];
                    row["Latitude"] = item["latitude"];
                    row["Longitude"] = item["longitude"];
                    row["MccId"] = item["mcc"];
                    row["MncId"] = item["mnc"];
                    row["Site"] = item["site_id"];
                    row["Sector"] = item["sector_id"];
                    row["Band"] = item["band"];
                    row["Carrier"] = item["carrier"];
                    row["NetworkMode"] = item["network_mode"];
                    row["CellId"] = item["cell_id"];
                    row["LacId"] = item["lac_id"];
                    row["PciId"] = item["pci_id"];
                    row["GsmRssi"] = item["gsm_rssi"];
                    row["GsmRxQual"] = item["gsm_rxqual"];
                    row["LteRsrp"] = item["lte_rsrp"];
                    row["LteRsrq"] = item["lte_rsrq"];
                    row["LteRsnr"] = item["lte_rsnr"];
                    row["LteCqi"] = item["lte_cqi"];
                    row["NRBand"] = item["nr_band"];
                    row["NRCarrier"] = item["nr_carrier"];
                    row["NRPci"] = item["nr_pci"];
                    row["NRRsrp"] = item["nr_rsrp"]; 
                    row["NRRsrq"] = item["nr_rsrq"];
                    row["NRRsnr"] = item["nr_snr"];
                    row["NRCqi"] = item["nr_cqi"];
                    row["DistanceFromSite"] = item["distance_from_site"];
                    row["AngleToSite"] = item["angle_to_site"];
                    row["StackTrace"] = item["stack_trace"];
                    row["TestResult"] = item["test_result"];
                    row["FtpStatus"] = (item["ftp_status"].ToString() == "Running") ? 1 : 0;
                    row["WcdmaRscp"] = item["wcdma_rscp"];
                    row["WcdmaEcio"] = item["wcdma_ecio"];
                    row["subnetworkmode"] = item["sector_netmode"];
                    row["actualband"] = item["sector_band"];
                    row["actualcarrier"] = item["sector_carrier"];
                    row["ishandover"] = Status;
                    row["LteRssi"] = item["lte_rssi"];
                    row["Scope"] = item["sector_scope"];

                    row["SiteRefId"] = item["SiteRefId"];
                    row["SectorRefId"] = item["SectorRefId"];
                    row["NetworkModeId"] = item["NetworkModeId"];
                    row["BandId"] = item["BandId"];
                    row["CarrierId"] = item["CarrierId"];
                    row["ScopeId"] = item["ScopeId"];
                    row["WoRefId"] = item["WoRefId"];
                    row["FloorId"] = item["FloorId"];
                    row["RRCState"] = item["RRCState"];


                    row["NetMode1"] = item["NetMode1"];
                    row["Band1"] = item["Band1"];
                    row["CH1"] = item["CH1"];
                    row["PCI1"] = item["PCI1"];
                    row["CI1"] = item["CI1"];
                    row["SS1"] = item["SS1"];
                    row["SP1"] = item["SP1"];
                    row["SQ1"] = item["SQ1"];
                    row["SNR1"] = item["SNR1"];

                    row["NetMode2"] = item["NetMode2"];
                    row["Band2"] = item["Band2"];
                    row["CH2"] = item["CH2"];
                    row["PCI2"] = item["PCI2"];
                    row["CI2"] = item["CI2"];
                    row["SS2"] = item["SS2"];
                    row["SP2"] = item["SP2"];
                    row["SQ2"] = item["SQ2"];
                    row["SNR2"] = item["SNR2"];

                    row["NetMode3"] = item["NetMode3"];
                    row["Band3"] = item["Band3"];
                    row["CH3"] = item["CH3"];
                    row["PCI3"] = item["PCI3"];
                    row["CI3"] = item["CI3"];
                    row["SS3"] = item["SS3"];
                    row["SP3"] = item["SP3"];
                    row["SQ3"] = item["SQ3"];
                    row["SNR3"] = item["SNR3"];

                    row["NetMode4"] = item["NetMode4"];
                    row["Band4"] = item["Band4"];
                    row["CH4"] = item["CH4"];
                    row["PCI4"] = item["PCI4"];
                    row["CI4"] = item["CI4"];
                    row["SS4"] = item["SS4"];
                    row["SP4"] = item["SP4"];
                    row["SQ4"] = item["SQ4"];
                    row["SNR4"] = item["SNR4"];

                    row["NetMode5"] = item["NetMode5"];
                    row["Band5"] = item["Band5"];
                    row["CH5"] = item["CH5"];
                    row["PCI5"] = item["PCI5"];
                    row["CI5"] = item["CI5"];
                    row["SS5"] = item["SS5"];
                    row["SP5"] = item["SP5"];
                    row["SQ5"] = item["SQ5"];
                    row["SNR5"] = item["SNR5"];

                    row["NetMode6"] = item["NetMode6"];
                    row["Band6"] = item["Band6"];
                    row["CH6"] = item["CH6"];
                    row["PCI6"] = item["PCI6"];
                    row["CI6"] = item["CI6"];
                    row["SS6"] = item["SS6"];
                    row["SP6"] = item["SP6"];
                    row["SQ6"] = item["SQ6"];
                    row["SNR6"] = item["SNR6"];

                    row["NetMode7"] = item["NetMode7"];
                    row["Band7"] = item["Band7"];
                    row["CH7"] = item["CH7"];
                    row["PCI7"] = item["PCI7"];
                    row["CI7"] = item["CI7"];
                    row["SS7"] = item["SS7"];
                    row["SP7"] = item["SP7"];
                    row["SQ7"] = item["SQ7"];
                    row["SNR7"] = item["SNR7"];

                    row["NetMode8"] = item["NetMode8"];
                    row["Band8"] = item["Band8"];
                    row["CH8"] = item["CH8"];
                    row["PCI8"] = item["PCI8"];
                    row["CI8"] = item["CI8"];
                    row["SS8"] = item["SS8"];
                    row["SP8"] = item["SP8"];
                    row["SQ8"] = item["SQ8"];
                    row["SNR8"] = item["SNR8"];
                    row["TCH"] = item["TCH"];
                    row["FromPCI"] = item["from_pci"];
                    row["ToPCI"] = item["to_pci"];
                    row["PRBPercent"] = item["PRBPercent"];
                    row["MCS"] = item["MCS"];
                    row["RB"] = item["RB"];
                    row["Modulation"] = item["Modulation"];
                    row["ModPercent"] = item["ModPercent"];
                    row["TM"] = item["TM"];
                    row["RI"] = item["RI"];
                    row["PCPDSCH"] = item["PCPDSCH"];
                    row["PCPUSCH"] = item["PCPUSCH"];
                    row["SCPDSCH"] = item["SCPDSCH"];
                    row["SCPUSCH"] = item["SCPUSCH"];
                    SiteTestLog.Rows.Add(row);
                }

               
                #endregion


                AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
                stld.Insert(SiteTestLog, Status.ToString());

                return true;
            }
            catch
            {

                throw;
            }
        }

        public bool RemoveSiteTestLogs(string Filter, string SiteId,bool IsActive, DataTable dt)
        {
            return stld.RemoveSiteTestLogs(Filter, SiteId, IsActive, dt);
        }
        public bool ChangeFolderName( long networklayerid, string SiteCode)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                int x = 0;
                string ClientPrefix = "";
                string networkmode = "";
                string carriors = "";
                string bands = "";
                var Newtworkmode = db.SelectedList("NetworkModes", null, "-NetworkMode-");
                var Bands = db.ToList("Bands");
                var Carriers = db.ToList("Carriers");
                string oldpath = "";
                string newpath = "";
                AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
                
                var rec = nlsb.ToSingle("Get_byLayerStatusId", 0, 0, 0, 0, networklayerid.ToString());
               
                AV_SitesBL sb = new AV_SitesBL();
                AV_Site oldSite = new AV_Site();
               oldSite= sb.Single("SingleSitebyId", rec.SiteId.ToString(),null,null,null,null); 
                AV_Site newSite = new AV_Site();
                newSite = sb.Single("SingleSitebySiteCode", SiteCode.ToString(), null, null, null, null); 
                var Newrec = nlsb.ToSingle("Get_byLayerStatusId", 0, 0, 0, 0, networklayerid.ToString());
                var oldSiteRecord = sb.ToList("BySiteCodeWithLayer",oldSite.SiteCode, null, null, null, null, null);
                var newSiteRecord = sb.ToList("BySiteCodeWithLayer", newSite.SiteCode, null, null, null, null, null);

                foreach (var netmod in Newtworkmode)
                            {
                                if (netmod.Value == rec.NetworkModeId.ToString())
                                {
                                    networkmode = netmod.Text;
                                }
                            }
                            foreach (var b in Bands)
                            {
                                if (b.DefinationId == rec.BandId)
                                {
                                    bands = b.DefinationName;
                                }
                            }
                            foreach (var c in Carriers)
                            {
                                if (c.DefinationId == rec.CarrierId)
                                {
                                    carriors = c.DefinationName;
                                }
                            }
                            oldpath = "/Content/AirViewLogs/" + oldSiteRecord[0].ClientPrefix + "/" + oldSite.SiteCode + "/"  + networkmode + "_" + bands + "_" + carriors + "";

                            foreach (var netmod in Newtworkmode)
                            {
                                if (netmod.Value == Newrec.NetworkModeId.ToString())
                                {
                                    networkmode = netmod.Text;
                                }
                            }
                            foreach (var b in Bands)
                            {
                                if (b.DefinationId == Newrec.BandId)
                                {
                                    bands = b.DefinationName;
                                }
                            }
                            foreach (var c in Carriers)
                            {
                                if (c.DefinationId == Newrec.CarrierId)
                                {
                                    carriors = c.DefinationName;
                                }
                            }


                            newpath = "/Content/AirViewLogs/"+ newSiteRecord[0].ClientPrefix + "/" + newSite.SiteCode + '/' + networkmode + "_" + bands + "_" + carriors + "";

                           var newPath = HttpContext.Current.Server.MapPath(newpath);
                        var oldPath = HttpContext.Current.Server.MapPath(oldpath);
                        WorkOrderBL wo = new WorkOrderBL();
                       wo.DirectoryCopy(oldPath, newPath, true);
 
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public bool TransferSiteLogs(string Filter, Int64 SiteId, Int64 LayerStatusId, string Sectors, string Tests, string SiteCode, string Task)
        {
            TransferSiteLogsDL ts = new TransferSiteLogsDL();

            return ts.Transfer( Filter, SiteId, LayerStatusId, Sectors, Tests,  SiteCode,  Task);
        }
    }
}
