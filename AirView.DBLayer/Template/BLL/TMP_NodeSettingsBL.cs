using AirView.DBLayer.Template.DAL;
using AirView.DBLayer.Template.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Template.BLL
{
    public class TMP_NodeSettingsBL
    {
        TMP_NodeSettingsDL nsDL = new TMP_NodeSettingsDL();
        public int Manage(string filter, TMP_NodeSettings n)
        {
            return nsDL.Manage(filter, n.NodeSettingsId, n.NodeId, n.DefinationId, n.KeyName, n.MappedId, n.Value, n.Settings, n.SortOrder.ToString(), n.QueryWhereClause);
        }

        public List<TMP_NodeSettings> ToList(string Filter, string Value = null)
        {
            DataTable dt = nsDL.Get(Filter, Value);
            return dt.ToList<TMP_NodeSettings>();
        }
    }
}
