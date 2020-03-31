using System;
using System.Collections.Generic;
using System.Reflection;


namespace SWI.Libraries.Common
{
    /*----MoB!----*/
  public  class ObjectProperties
    {

        public List<string> PropertiesName(object obj) {
            List<string> Names = new List<string>();
            Type myType = obj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                Names.Add(prop.Name);
            }

            return Names;
        }
    }
}
