using System;

namespace SWI.Libraries.Common
{
  public  class DataType
    {
        public dynamic GetType(string value)
        {
            bool boolValue;
            Int32 intValue;
            Int64 bigintValue;
            double doubleValue;
            DateTime dateValue;

            if (bool.TryParse(value, out boolValue))
                return boolValue;
            else if (Int32.TryParse(value, out intValue))
                return intValue;
            else if (Int64.TryParse(value, out bigintValue))
                return bigintValue;
            else if (double.TryParse(value, out doubleValue))
                return doubleValue;
            else if (DateTime.TryParse(value, out dateValue))
                return dateValue;
            else return value;
        }

        public static Int32 ToInt32(string value)
        {
            Int32 temp;
            try
            {
                if (Int32.TryParse(value, out temp))
                    return temp;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }

        public static Int64 ToInt64(string value)
        {
            Int64 temp;
            try
            {
                if (Int64.TryParse(value, out temp))
                    return temp;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }

       

        public static double ToDouble(string value)
        {
            double Temp;
            try
            {
                if (double.TryParse(value, out Temp))
                    return Temp;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }

        public static Boolean ToBoolean(string value)
        {
            Boolean Temp;
            if (bool.TryParse(value, out Temp))
                return Temp;
            else
                return false;
        }


        public static DateTime? ToDateTime(string value)
        {
            try
            {
                DateTime Temp;
                if (DateTime.TryParse(value, out Temp))
                    return Temp;
                else
                    return null;
            }
            catch
            {

                return null;
            }
           
        }

        public static Decimal ToDecimal(string value)
        {
            Decimal Temp;
            try
            {
                if (Decimal.TryParse(value, out Temp))
                    return Temp;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }

        public static float ToFloat(string value)
        {
            float Temp;
            try
            {
                if (float.TryParse(value, out Temp))
                    return Temp;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }
        
    }
}
