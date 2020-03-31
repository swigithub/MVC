using AirView.DBLayer.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;

namespace WebApplication.Areas.Project.ObjectComparer
{
    public static class _ObjectComparer
     {
        public static T EnumeratePropertyDifferences<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            string changes ="";
            foreach (PropertyInfo pi in properties)
            {
                object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);
                if (value1 != value2 && (value1 == null || !value1.Equals(value2)) && (pi.Name =="PlanDate" || pi.Name == "ForecastStartDate" || pi.Name == "ForecastEndDate" || pi.Name == "TargetDate" || pi.Name == "ActualStartDate" || pi.Name == "ActualEndDate" || pi.Name == "Status" || pi.Name == "Priority" || pi.Name == "Completion") )
                {
                  //  changes.Add(string.Format("Property {0} changed from {1} to {2}", pi.Name, value1, value2));
                    //  obj1.GetProperty(pi.Name)
                    if(changes == "")
                    changes += (string.Format("{0}:{1}={2}", pi.Name, value1, value2));
                    else
                    changes += (string.Format(",{0}", pi.Name));
                    typeof(T).GetProperty(pi.Name).SetValue(obj2, Convert.ChangeType(value2 + "*?^*", pi.PropertyType), null);
                }
                
            }
            typeof(T).GetProperty("_difference").SetValue(obj2, Convert.ChangeType(changes, typeof(T).GetProperty("_difference").PropertyType), null);
            //    _propertyInfo.SetValue(obj2, Convert.ChangeType(changes, _propertyInfo.PropertyType), null);
            return obj2;
        }
        
    }
}