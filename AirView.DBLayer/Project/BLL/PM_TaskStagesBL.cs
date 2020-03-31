using AirView.DBLayer.Common;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_TaskStagesBL
    {
        PM_TaskStagesDL pM_TaskStagesDL = new PM_TaskStagesDL();
        public bool InsertBulk(long? ProjectId, List<PM_TaskStages> pM_TaskStages)
        {
            pM_TaskStages.ForEach(x => x.ProjectId = ProjectId.Value);
            var ModelIntoDataTable = CopyDataTable.CopyToDataTable(pM_TaskStages);
            return pM_TaskStagesDL.Manage("PM_TaskStages", ModelIntoDataTable);
        }

        public bool InsertBulk(long ProjectId, long TaskId, List<PM_TaskStages> pM_TaskStages)
        {
            pM_TaskStages.ForEach(p => {
                p.ProjectId = ProjectId;
                p.TaskId = TaskId;
            });
            var ModelIntoDataTable = CopyDataTable.CopyToDataTable(pM_TaskStages);
            return pM_TaskStagesDL.Manage("PM_TaskStages", ModelIntoDataTable);
        }

        public List<PM_TaskStages> ToList(string filter, long ProjectId)
        {
            try
            {
                DataTable dt = pM_TaskStagesDL.Get(filter, ProjectId);
                List<PM_TaskStages> rec = dt.ToList<PM_TaskStages>();
                return rec;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PM_TaskStages> ToList(string filter, long ProjectId, long TaskId)
        {
            try
            {
                DataTable dt = pM_TaskStagesDL.Get(filter, ProjectId, TaskId);
                List<PM_TaskStages> rec = dt.ToList<PM_TaskStages>();
                return rec;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Single(string filter, int StageId)
        {
            try
            {
                return pM_TaskStagesDL.Get(filter, StageId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateOrAdd(string Filter, long ProjectId, List<PM_TaskStages> TaskStages)
        {
            TaskStages.ForEach(x => x.ProjectId = ProjectId);
            DataTable DT = CopyDataTable.CopyToDataTable(TaskStages);
            return pM_TaskStagesDL.Manage(Filter, ProjectId, DT);
        }

        public bool UpdateOrAdd(string Filter, long ProjectId, long TaskId, List<PM_TaskStages> TaskStages)
        {
            TaskStages.ForEach(p => {
                p.ProjectId = ProjectId;
            });
            DataTable DT = CopyDataTable.CopyToDataTable(TaskStages);
            return pM_TaskStagesDL.Manage(Filter, ProjectId, TaskId, DT);
        }
    }
}
