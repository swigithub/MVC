using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SWI.AirView.Common
{
    public class WebConfig
    {

        public string AppSettings(string key)
        {
            try
            {
              return  WebConfigurationManager.AppSettings[key];
            }
            catch 
            {

                return null;
            }
        }
    }
}