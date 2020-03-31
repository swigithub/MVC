using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Common
{
  public  class CustomDataTable
    {

        public  int ToInt32(DataTable dt, int RowNumber, string ColumnName,bool IsColumnExist = false)
        {
            try
            {
                if (IsColumnExist)
                {
                    if (dt.Columns.Contains(ColumnName))
                    {

                        return Convert.ToInt32(dt.Rows[RowNumber][ColumnName].ToString()); 
                    }
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[RowNumber][ColumnName].ToString());
                }
                

                return 0;
            }
            catch
            {

                return 0;
            }



        }

        public  double ToDouble(DataTable dt, int RowNumber, string ColumnName, bool IsColumnExist = false)
        {
            try
            {
                if (IsColumnExist)
                {
                    if (dt.Columns.Contains(ColumnName))
                    {

                        return Convert.ToDouble(dt.Rows[RowNumber][ColumnName].ToString());
                    }
                }
                else
                {
                    return Convert.ToDouble(dt.Rows[RowNumber][ColumnName].ToString());
                }

                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
        }
    }
}
