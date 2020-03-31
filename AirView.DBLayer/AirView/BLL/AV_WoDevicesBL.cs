using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using System;
using System.Collections.Generic;
using System.Data;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/

 public   class AV_WoDevicesBL
    {
        AV_WoDevicesDL wdd = new AV_WoDevicesDL();
        public bool SaveDevices(int SiteId, int TesterId, string Date, int[] NetworkMode, int[] Band, int[] Carrier, int[] Devices, int[] TesterIds)
        {
            try
            {
                string Queries = "";
                int TempTesterId = 0;
                Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
                var UserDevices = udbl.ToList("byUserId", TesterId.ToString());
                for (int i = 0; i < NetworkMode.Length; i++)
                {
                    if (Devices[i] == 0)
                    {
                        foreach (var item in UserDevices)
                        {
                            Queries = Queries + "INSERT INTO [dbo].[AV_WoDevices]([BandId],[CarrierId],[NetworkId],[UserId],[UserDeviceId],[SiteId])VALUES(" + Band[i] + ", " + Carrier[i] + "," + NetworkMode[i] + ", " + TesterId + ", " + item.DeviceId + ", " + SiteId + ");";

                        }
                    }
                    else
                    {
                        if (TesterIds!=null)
                        {
                            TempTesterId = TesterIds[i];
                        }
                        else
                        {
                            TempTesterId = TesterId;
                        }
                        // wodl.Manage(0, Band[i], Carrier[i], false, NetworkMode[i], TesterId, Devices[i], SiteId, 0, null);
                        Queries = Queries + "INSERT INTO [dbo].[AV_WoDevices]([BandId],[CarrierId],[NetworkId],[UserId],[UserDeviceId],[SiteId])VALUES(" + Band[i] + ", " + Carrier[i] + "," + NetworkMode[i] + ", " + TesterId + ", " + Devices[i] + ", " + SiteId + ");";

                    }

                }

                DataContext.SqlQuery(Queries);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AV_WoDevices> ToList(string Filter, Int64 SiteId, Int64 NetworkId, Int64 BandId, Int64 CarrierId, Int64 ScopeId,Int64 UserId)
        {
            try
            {
                DataTable dt = wdd.Get(Filter,SiteId,NetworkId,BandId,CarrierId,ScopeId, UserId);
                return dt.ToList<AV_WoDevices>();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
