using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.BLL
{
    public class AD_UserEquipmentsBL
    {
        AD_UserEquipmentDL ued = new AD_UserEquipmentDL();
        public AD_UserEquipment ToSingle(string filter, string value = null, string value2 = null, string value3 = null)
        {
            try
            {
                DataTable dt = ued.Get(filter, value, value2, value3);
                var ue = dt.ToList<AD_UserEquipment>();
                if (ue.Count > 0)
                {
                    return ue.FirstOrDefault();
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<AD_UserEquipment> Paging(int Skip, int Take , string Search ,ref Int64 TotalCount)
        {
            try
            {

                DataSet ds = ued.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search);
                if (ds.Tables.Count>0)
                {
                    DataTable Count = ds.Tables[1];
                    if (Count!=null && Count.Rows.Count > 0)
                    {
                        TotalCount =Convert.ToInt64 (Count.Rows[0]["TotalRecord"].ToString());
                    }

                    return ds.Tables[0].ToList<AD_UserEquipment>();
                }
                
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<AD_UserEquipment> ToList(string filter, string value = null, string value2 = null, string value3 = null)
        {
            try
            {
                DataTable dt = ued.Get(filter, value, value2, value3);
               return dt.ToList<AD_UserEquipment>();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<SelectedList> SelectedList(string filter, string value = null, string Message = null)
        {
            SelectedList sl = new SelectedList();

            var rec = ToList(filter, value).Select(m => new SelectedList { Text = m.Model + " "+m.SerialNo, Value = m.UEId.ToString() }).ToList();
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }

            return rec;
        }

        public bool Manage(string filter, AD_UserEquipment ue)
        {
            return ued.Manage(filter,ue.UEId,ue.UETypeId,ue.Manufacturer,ue.Model,ue.SerialNo,ue.MAC,ue.UENumber,ue.IsActive,ue.Token,ue.UEOwnerId, ue.UERefNo);
        }
    }
}
