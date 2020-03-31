using AirView.DBLayer.Template.DAL;
using Library.SWI.Template.Model;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.Template.BLL
{
    public class TMP_NodeBL
    {
        TMP_NodeDL nDL = new TMP_NodeDL();
        public int Manage(string filter, TMP_Node n)
        {
            return nDL.Manage(filter, n.NodeId, n.TemplateId, n.NodeTitle, n.Height, n.Width, n.x_axis, n.y_axis, n.PageTyppeId, n.NodeUrl, n.NodeSQL, n.IsActive);
        }

        public List<TMP_Node> ToList(string Filter, string Value = null)
        {
            DataTable dt = nDL.Get(Filter, Value);
            return dt.ToList<TMP_Node>();
        }
    }
}
