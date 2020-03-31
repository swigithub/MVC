using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.Template.DAL;
using Library.SWI.Template.Model;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.Template.BLL
{
    public class TMP_NodesPropertiesBL
    {
        TMP_NodesPropertiesDL nDL = new TMP_NodesPropertiesDL();
        public int Manage(string filter, TMP_NodesProperties np)
        {
            return nDL.Manage(filter, np.FormId, np.NodeTypeId, np.Title, np.ControlType, np.DataType, np.DefaultValue, np.MaxLength, np.Required,np.IsAttachment, np.SortOrder,np.IsDeleted,np.Comments);
        }

        public List<TMP_NodesProperties> ToList(string Filter, string Value = null)
        {
            try
            {
                DataTable dt = nDL.Get(Filter, Value);
                return dt.ToList<TMP_NodesProperties>();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public List<AV_SiteScriptFormEntry> ToList(string Filter,long Value = 0)
        {
            try
            {
                DataTable dt = nDL.Get(Filter, Value);
                return dt.ToList<AV_SiteScriptFormEntry>();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public int Manage(string filter, AV_SiteScriptFormEntry np)
        {
            return nDL.ManageSiteScriptsForm(filter, np.FormId, np.NodeTypeId, np.Title, np.ControlType, np.DataType, np.DefaultValue, np.MaxLength, np.Required, np.IsAttachment, np.SortOrder, np.IsDeleted, np.SrId);
        }
    }
}
