using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
  public  class AD_SurveyBL
    {
        public List<AD_Survey> GetSurvey()
        {
           try
            {
                AD_SurveyDL DB = new AD_SurveyDL();
                DataTable data = DB.GetSurvey();
                List<AD_Survey> rec = data.ToList<AD_Survey>();
                return rec;
            }
            catch
            {
               return null;
            }
        }



    }
}
