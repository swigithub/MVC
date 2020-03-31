using AirView.DBLayer.Template.DAL;
using Library.SWI.Template.Model;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AirView.DBLayer.Template.BLL
{
    public class TMP_TemplatesBL
    {
        TMP_TemplatesDL tDL = new TMP_TemplatesDL();
        public int Manage(string filter, TMP_Templates tmp)
        {
            return tDL.Manage(filter, tmp.TemplateId, tmp.TemplateTitle, tmp.ProjectId, tmp.ScopeId, tmp.BackgroundColor, tmp.PageType, tmp.Parameters, tmp.IsActive, tmp.TemplateType, tmp.IsDefault, tmp.ModuleId);
        }

        public List<TMP_Templates> ToList(string Filter, string Value = null, string ProjectId = null)
        {
            DataTable dt = tDL.Get(Filter, Value, ProjectId);
            return dt.ToList<TMP_Templates>();
        }
    }
}
