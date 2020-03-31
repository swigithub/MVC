using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.AirView.BLL
{
    public class AD_HelpBL
    {
        AD_HelpDL help = new AD_HelpDL();
        public int Create(string filter, int ComponentId, int ModuleId, int FeatureId, string Title, string Description, bool IsActive)
        {
            try
            { 
                return help.Create(filter, ComponentId, ModuleId, FeatureId, Title, Description, IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<AD_Help> Read(string filter)
        {
            try
            {
                DataTable dt = help.Read(filter);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();
                return model;
               
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<AD_Help> Read(string filter, int CID)
        {
            try
            {
                DataTable dt = help.Read(filter, CID);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();
                return model;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<AD_Help> Read(string filter, int CID, int MID)
        {
            try
            {
                DataTable dt = help.Read(filter, CID, MID);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();
                return model;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<AD_Help> List(string filter, bool listFilter)
        {
            try
            {
                DataTable dt = help.Read(filter, listFilter);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> jkj = dt.ToList<AD_Help>();
                return jkj;

            }
            catch (Exception)
            {
                return null;
            }

        }


        public int ListRow(string filter, bool IsActive, int ModuleId)
        {
       
            try
            {
                return help.UpdateRow(filter, IsActive, ModuleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<AD_Help> ReadPost(string filter, int id)
        {
            try
            {
                DataTable dt = help.ReadPost(filter, id);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();


                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        model[i].ModuleId = int.Parse(dt.Rows[i]["ModuleId"].ToString());
                    }
                    return model;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public int EditPost(string filter, int HelpId, string title, string description)
        {
            try
            {
                return help.EditPost(filter, HelpId, title, description);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       
    }
}
