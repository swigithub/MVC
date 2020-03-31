using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
   public class AV_NetLayerReportExportBL
    {
        AV_NetLayerReportExportDL dl = new AV_NetLayerReportExportDL();

        public List<AV_NetLayerReportExport> Get(string DateFilter, DateTime fromDate, DateTime toDate, string woStatusstring, string Panel1Filter, string Panel1Value, string Panel2Filter, string Panel2Value, string ReportFilter,Int64 UserId)
        {
            DataTable dtbl = new DataTable();
            dtbl = dl.Get(DateFilter, fromDate,  toDate,  woStatusstring,  Panel1Filter,  Panel1Value,  Panel2Filter,  Panel2Value, ReportFilter, UserId);
            List<AV_NetLayerReportExport> Rec = dtbl.ToList<AV_NetLayerReportExport>();
            return Rec;

        }

        }
}
