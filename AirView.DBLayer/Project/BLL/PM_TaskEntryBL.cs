using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.DAL;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using Library.SWI.Template.Model;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_TaskEntryBL
    {
        private PM_TaskEntryDL ted = new PM_TaskEntryDL();

        public dynamic Manage(string Filter, PM_TaskEntry t)
        {
            return ted.Manage(Filter, t.EntryId, t.ProjectId, t.ProjectSiteId, t.TaskId, t.FormId, t.FormValue, t.CreatedById, t.CreatedOn);
        }

        public dynamic ManageResources(string Filter, Int64 ResourceId, Int64 GroupId, Int64 RACIId, Int64 ProjectId, Int64 TaskId, Int64 RatePerHour, bool IsDeleted = false)
        {
            return ted.ManageResources(Filter, ResourceId, GroupId, RACIId, ProjectId, TaskId, RatePerHour, IsDeleted);
        }

        public dynamic ManageResources(string Filter, Int64 ResourceId, Int64 GroupId, Int64 RACIId, Int64 ProjectId, Int64 TaskId, Int64 PMTRId, Int64 RatePerHour, bool IsDeleted = false)
        {
            return ted.ManageResources(Filter, ResourceId, GroupId, RACIId, ProjectId, TaskId, PMTRId, RatePerHour, IsDeleted);
        }

        public dynamic ManageResources(string Filter, Int64 PMTRId)
        {
            return ted.ManageResources(Filter, PMTRId);
        }

        public List<PM_TaskEntry> GET_Group_LIST(string Filter, int ProjectId, int TaskId)
        {
            try
            {
                DataTable dataTableModel = ted.GET_Group_LIST(Filter, ProjectId, TaskId);

                List<PM_TaskEntry> ListModel = dataTableModel.ToList<PM_TaskEntry>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PM_TaskEntry> GET_Group_LIST(string Filter, int ProjectId)
        {
            try
            {
                DataTable dataTableModel = ted.GET_Group_LIST(Filter, ProjectId);

                List<PM_TaskEntry> ListModel = dataTableModel.ToList<PM_TaskEntry>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PM_TaskEntry> GET_Group_LIST(string Filter)
        {
            try
            {
                DataTable dataTableModel = ted.GET_Group_LIST(Filter);

                List<PM_TaskEntry> ListModel = dataTableModel.ToList<PM_TaskEntry>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<PM_TaskEntry> ToList(string Filter, long TaskId = 0, long SiteId = 0)
        {
            DataTable dt = ted.Get(Filter, TaskId, SiteId);
            return dt.ToList<PM_TaskEntry>();
        }
        public List<PM_UETypes> Get_UE_Type_List(string Filter)
        {
            try
            {
                DataTable dataTableModel = ted.Get_UE_Type_List(Filter);

                List<PM_UETypes> ListModel = dataTableModel.ToList<PM_UETypes>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PM_UEItems> Get_UE_Items(string Filter,long UETypeId)
        {
            try
            {
                DataTable dataTableModel = ted.Get_UE_Item_List(Filter,UETypeId);

                List<PM_UEItems> ListModel = dataTableModel.ToList<PM_UEItems>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PM_SiteTaskInventory> Get_SiteTask_Inventory(string Filter, long SiteTaskId,long SiteId)
        {
            try
            {
                DataTable dataTableModel = ted.Get_SiteTask_Inventory(Filter, SiteTaskId,SiteId);

                List<PM_SiteTaskInventory> ListModel = dataTableModel.ToList<PM_SiteTaskInventory>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PM_SiteTaskInventoryHeader> Get_SiteTask_InventoryHeader(string Filter, long SiteTaskId, long SiteId)
        {
            try
            {
                DataTable dataTableModel = ted.Get_SiteTask_Inventory(Filter, SiteTaskId, SiteId);

                List<PM_SiteTaskInventoryHeader> ListModel = dataTableModel.ToList<PM_SiteTaskInventoryHeader>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<PM_SiteTaskInventoryAttachments> Get_SiteTask_Inventory_Attachments(string Filter, long SiteTaskInventoryId)
        {
            try
            {
                DataTable dataTableModel = ted.Get_SiteTask_Inventory_Attachments(Filter, SiteTaskInventoryId);

                List<PM_SiteTaskInventoryAttachments> ListModel = dataTableModel.ToList<PM_SiteTaskInventoryAttachments>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DataTable Manage_SiteTask_Inventory(string Filter, DataTable dt = null, long SiteTaskId = 0, long SiteId = 0, long UserId = 0, long SiteTaskInventoryId = 0, string FileName = null, string SubDirectory = "",long ContentLength=0)
        {
            try
            {

               return ted.Manage_SiteTask_Inventory(Filter,dt,SiteTaskId,SiteId,UserId,SiteTaskInventoryId, FileName,SubDirectory, ContentLength);
            }
            catch
            {
                return null;
            }
        }
       
    }
}