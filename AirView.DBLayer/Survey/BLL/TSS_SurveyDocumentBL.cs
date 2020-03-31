using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AirView.DBLayer.Survey.Model;

namespace Library.SWI.Survey.BLL
{
    /*----MoB!----*/
    /*----15-08-2017----*/
    public class TSS_SurveyDocumentBL
    {
        private TSS_SurveyDocumentDL add = new TSS_SurveyDocumentDL();

        public dynamic Manage(string Filter, TSS_SurveyDocument sd)
        {
            return add.Manage(Filter, sd.ClientId, sd.CityId, sd.SurveyId, sd.SurveyTitle, sd.Description, sd.CategoryId, sd.SubCategoryId, sd.IsActive, sd.UnitSystemId, sd.CreatedBy,sd.IsPublished,sd.IsGlobal);
        }

        public dynamic CreateNewWOrkOrder(string Filter,int SurveyId)
        {
            return add.CreateWorkOrderDemo(Filter,SurveyId);
        }
        public dynamic GetSurveyBySiteId(string Filter, long SiteId)
        {
            return add.GetDataTable(Filter, SiteId.ToString());
        }

        private List<TSS_Section> GetTreeView(List<TSS_Section> sec, List<TSS_Section> record)
        {
            TSS_SectionBL sb = new TSS_SectionBL();
            foreach (var s in sec)
            {
                var sec2 = record.Where(m => m.PSectionId == s.SectionId).ToList();// sb.ToList("By_SectionId", s.PSectionId.ToString()).ToList();
                if (sec2 != null)
                {
                    s.Sections.AddRange(sec2);
                    GetTreeView(sec2, record);
                }

            }

            return sec;
        }

        

        public List<TSS_Section> GetSectionsForDashboard(string filter, string Value = null, bool SectionTree = false, int pageIndex = 0, int pageSize = 0, bool loadQuestions = false)
        {
            var dt = add.GetDataTable(filter, Value, pageIndex, pageSize);
            var sections = dt.ToList<TSS_Section>();
            //var sectionsJson = JsonConvert.SerializeObject(sections);
            foreach (var generateChildSection in sections)
            {
                if (!string.IsNullOrEmpty(generateChildSection.ChildTitle) || generateChildSection.ChildTitle == "0")
                    generateChildSection.SectionTitle = $"{generateChildSection.SectionTitle} {generateChildSection.ChildTitle}";
                if (generateChildSection.IsRepeatable) continue;
                if (loadQuestions)
                    generateChildSection.Questions = new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_API", 0,
                        generateChildSection.SectionId.ToString(), true);
            }
            return !sections.Any() ? new List<TSS_Section>() : FillRecursiveForDashboard(sections, 0);
        }

        public List<TSS_Section> FillRecursiveForDashboard(List<TSS_Section> flatObjects, long parentId, long repeatableParentId = 0)
        {

            List<TSS_Section> recursiveObjects = new List<TSS_Section>();
            var asQueryable = flatObjects.AsQueryable();
            asQueryable = repeatableParentId > 0
                ? asQueryable.Where(x => x.PSectionId.Equals(parentId) || x.PSectionId == repeatableParentId).AsQueryable()
                : asQueryable.Where(x => x.PSectionId.Equals(parentId));
            var tssSections = asQueryable.ToList();
            foreach (var item in tssSections)
            {
                recursiveObjects.Add(new TSS_Section
                {
                    SectionTitle = item.SectionTitle,

                    SectionId = item.SectionId,
                    SiteSectionId = item.TemplateSectionId,
                    IsRepeatable = item.IsRepeatable,
                    TotalQuestions = item.TotalQuestions,
                    PSectionId = item.PSectionId,
                  
                    ChildTitle = item.ChildTitle,
                    IsInclude = item.IsInclude,
                    Questions = item.Questions,
                    Signature = item.Signature,
                    TotalAnswered = item.TotalAnswered,
                    DefinationName = item.DefinationName,
                    ColorCode = item.ColorCode,
                    IsDeletable = item.IsDeletable,
                    IsApplicable = item.IsApplicable,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    SortOrder = item.SortOrder,
                    StatusId = item.StatusId,
                    Status = item.Status,
                    StatusColor = item.StatusColor,
                    Sections = FillRecursiveForDashboard(flatObjects, item.SectionId, item.TemplateSectionId > 0 ? item.TemplateSectionId : 0).OrderBy(s => s.SortOrder).ToList()
                });
            }
            return recursiveObjects.OrderBy(s => s.SortOrder).ToList();

        }


        public List<TSS_SurveyDocument> ToList(string filter, string Value = null, bool SectionTree = false, int pageIndex = 0, int pageSize = 0,string IMEI=null,long UserId=0)
        {
            DataTable dt = add.GetDataTable(filter, Value, pageIndex, pageSize,IMEI,UserId);
            List<TSS_SurveyDocument> survey = (List<TSS_SurveyDocument>)dt.ToList<TSS_SurveyDocument>();//.ToList();
            if (SectionTree)
            {
                List<TSS_Section> section = dt.ToList<TSS_Section>();
                survey = survey.GroupBy(test => test.SurveyId).Select(grp => grp.First()).ToList();
                foreach (var item in survey)
                {
                    item.Sections = FillRecursive(section.Where(x => x.SurveyId == item.SurveyId).ToList(), 0); //section.Where(x => x.SurveyId == item.SurveyId).ToList();
                }
            }

            return survey;
        }

        public List<TSS_Section> GetSectionBySurveyId(string filter, string SurveyId = null)
        {
            DataTable dt = add.GetDataTable(filter, SurveyId, 0, 0);
            List<TSS_Section> Sections = dt.ToList<TSS_Section>();
            Sections = FillRecursive(Sections, 0);
            return Sections;
        }

        public List<TSS_Section> GetSections(string filter, string Value = null, bool SectionTree = false, int pageIndex = 0, int pageSize = 0, bool loadQuestions = false)
        {
            var dt = add.GetDataTable(filter, Value, pageIndex, pageSize);
            var sections = dt.ToList<TSS_Section>();
            //var sectionsJson = JsonConvert.SerializeObject(sections);
            foreach (var generateChildSection in sections)
            {
                if (!string.IsNullOrEmpty(generateChildSection.ChildTitle) || generateChildSection.ChildTitle == "0")
                    generateChildSection.SectionTitle = $"{generateChildSection.SectionTitle} {generateChildSection.ChildTitle}";
                if (generateChildSection.IsRepeatable) continue;
                if (loadQuestions)
                    generateChildSection.Questions = new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_API", 0,
                        generateChildSection.SectionId.ToString(), true).OrderBy(s => s.SortOrder).ToList();
            }
            return !sections.Any() ? new List<TSS_Section>() : FillRecursive(sections, 0);
        }

        public List<TSS_Section> FillRecursive(List<TSS_Section> flatObjects, long parentId, long repeatableParentId = 0)
        {
            List<TSS_Section> recursiveObjects = new List<TSS_Section>();
            var asQueryable = flatObjects.AsQueryable();
            asQueryable = repeatableParentId > 0
                ? asQueryable.Where(x => x.PSectionId.Equals(parentId) || x.PSectionId == repeatableParentId).AsQueryable()
                : asQueryable.Where(x => x.PSectionId.Equals(parentId));
            var tssSections = asQueryable.ToList();
            foreach (var item in tssSections)
            {
                recursiveObjects.Add(new TSS_Section
                {
                    SectionTitle = item.SectionTitle,

                    SectionId = item.SectionId,
                    SiteSectionId = item.SiteSectionId,
                    IsRepeatable = item.IsRepeatable,
                    TotalQuestions = item.TotalQuestions,
                    PSectionId = item.PSectionId,
                    ChildTitle = item.ChildTitle,
                    IsInclude = item.IsInclude,
                    Questions = item.Questions,
                    Signature = item.Signature,
                    TotalAnswered = item.TotalAnswered,
                    DefinationName = item.DefinationName,
                    ColorCode = item.ColorCode,
                    IsDeletable = item.IsDeletable,
                    IsApplicable = item.IsApplicable,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    SortOrder = item.SortOrder,
                    Status = item.Status,
                    StatusColor = item.StatusColor,
                    SurveyTitle=item.SurveyTitle,
                    SiteCode=item.SiteCode,
                    Longitude=item.Longitude,
                    Latitude=item.Latitude,
                    IsSignatureRequired=item.IsSignatureRequired,
                    RepeatCount=item.RepeatCount,
                    Sections = FillRecursive(flatObjects, item.SectionId, item.TemplateSectionId > 0 ? item.TemplateSectionId : 0).OrderBy(s => s.SortOrder).ToList()
                });
            }
            return recursiveObjects.OrderBy(s => s.SortOrder).ToList();
        }
        public List<TSS_Section> SurveyDocumentTemplateTree(List<TSS_Section> flatObjects, long parentId, long repeatableParentId = 0)
        {
            List<TSS_Section> recursiveObjects = new List<TSS_Section>();
            var asQueryable = flatObjects.AsQueryable();
            asQueryable = repeatableParentId > 0
                ? asQueryable.Where(x => x.PSectionId.Equals(parentId) || x.PSectionId == repeatableParentId).AsQueryable()
                : asQueryable.Where(x => x.PSectionId.Equals(parentId));
            var tssSections = asQueryable.ToList();
            foreach (var item in tssSections)
            {
                recursiveObjects.Add(new TSS_Section
                {
                    SectionTitle = item.SectionTitle,
                    SectionId = item.SectionId,
                    SiteSectionId = item.SiteSectionId,
                    IsRepeatable = item.IsRepeatable,
                    TotalQuestions = item.TotalQuestions,
                    PSectionId = item.PSectionId,
                    ChildTitle = item.ChildTitle,
                    Questions = item.Questions,
                    Signature = item.Signature,
                    IsActive = item.IsActive,
                    SortOrder = item.SortOrder,
                    IsApplicable = item.IsApplicable,
                    IsInclude = item.IsInclude,
                    Description = item.Description,
                    TotalAnswered = item.TotalAnswered,
                    Status=item.Status,
                    StatusColor=item.StatusColor,
                    IsSignatureRequired=item.IsSignatureRequired,
                    IsLogicExists = item.IsLogicExists,
                    Sections = SurveyDocumentTemplateTree(flatObjects, item.SectionId).OrderBy(s => s.SortOrder).ToList()
                });
            }
            return recursiveObjects.OrderBy(s => s.SortOrder).ToList();
         }

        public List<TSS_Section> SurveyDocumentTemplateTreeWithoutTree(List<TSS_Section> flatObjects, long parentId, long repeatableParentId = 0)
        {
            List<TSS_Section> recursiveObjects = new List<TSS_Section>();
            var asQueryable = flatObjects.AsQueryable();
            //asQueryable = repeatableParentId > 0
            //    ? asQueryable.Where(x => x.PSectionId.Equals(parentId) || x.PSectionId == repeatableParentId).AsQueryable()
            //    : asQueryable.Where(x => x.PSectionId.Equals(parentId));
            var tssSections = asQueryable.ToList();
            foreach (var item in tssSections)
            {
                recursiveObjects.Add(new TSS_Section
                {
                    SectionTitle = item.SectionTitle,
                    SectionId = item.SectionId,
                    SiteSectionId = item.SiteSectionId,
                    IsRepeatable = item.IsRepeatable,
                    TotalQuestions = item.TotalQuestions,
                    PSectionId = item.PSectionId,
                    ChildTitle = item.ChildTitle,
                    Questions = item.Questions,
                    Signature = item.Signature,
                    IsActive = item.IsActive,
                    SortOrder = item.SortOrder,
                    IsApplicable = item.IsApplicable,
                    IsInclude = item.IsInclude,
                    Description = item.Description,
                    TotalAnswered = item.TotalAnswered,
                    IsLogicExists=item.IsLogicExists,
                    //  Sections = SurveyDocumentTemplateTree(flatObjects, item.SectionId).OrderBy(s => s.SortOrder).ToList()
                });
            }
            return recursiveObjects.OrderBy(s => s.SortOrder).ToList();
        }

        public List<TSS_Section> FillChildRecursive(List<TSS_Section> flatObjects, long parentId, long repeatableParentId = 0)
        {
            List<TSS_Section> recursiveObjects = new List<TSS_Section>();
            var asQueryable = flatObjects.AsQueryable();
            asQueryable = repeatableParentId > 0
                ? asQueryable.Where(x => x.PSectionId.Equals(parentId) || x.PSectionId == repeatableParentId).AsQueryable()
                : asQueryable.Where(x => x.PSectionId.Equals(parentId));
            var tssSections = asQueryable.ToList();
            foreach (var item in tssSections)
            {
                recursiveObjects.Add(new TSS_Section
                {
                    SectionTitle = item.SectionTitle,

                    SectionId = item.SectionId,
                    SiteSectionId = item.SiteSectionId,
                    IsRepeatable = item.IsRepeatable,
                    TotalQuestions = item.TotalQuestions,
                    PSectionId = item.PSectionId,

                    ChildTitle = item.ChildTitle,
                    Questions = item.Questions,
                    Sections = FillRecursive(flatObjects, item.SiteSectionId, item.PSectionId > 0 ? item.PSectionId : 0).OrderBy(s => s.SortOrder).ToList()
                });
            }
            return recursiveObjects;
        }

        public TSS_SurveyDocument ToSingle(string filter, string Value = null)
        {
            DataTable dt = add.GetDataTable(filter, Value);
            return dt.ToList<TSS_SurveyDocument>().FirstOrDefault();
        }
        public List<TSS_SectorLocations> GetSectorLocations(Int64 SiteSurveyId)
        {
            DataTable dt = add.GetSectorLocation(SiteSurveyId);
            return dt.ToList<TSS_SectorLocations>();
        }

        public List<TSS_COPList> GetAllSurveys(string filter, string Value = null, bool SectionTree = false, int pageIndex = 0, int pageSize = 0)
        {
            DataTable dt = add.GetDataTable(filter, Value, pageIndex, pageSize);
            List<TSS_COPList> survey = (List<TSS_COPList>)dt.ToList<TSS_COPList>();
            return survey;
        }

    }
    public static class IEnumerableExtensions
    { 
        /// <summary>
        /// Flattens an object hierarchy.
        /// </summary>
        /// <param name="rootLevel">The root level in the hierarchy.</param>
        /// <param name="nextLevel">A function that returns the next level below a given item.</param>
        /// <returns><![CDATA[An IEnumerable<T> containing every item from every level in the hierarchy.]]></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> rootLevel, Func<T, IEnumerable<T>> nextLevel)
        {
            List<T> accumulation = new List<T>();
            accumulation.AddRange(rootLevel);
            flattenLevel<T>(accumulation, rootLevel, nextLevel);
            return accumulation;
        }

        /// <summary>
        /// Recursive helper method that traverses a hierarchy, accumulating items along the way.
        /// </summary>
        /// <param name="accumulation">A collection in which to accumulate items.</param>
        /// <param name="currentLevel">The current level we are traversing.</param>
        /// <param name="nextLevel">A function that returns the next level below a given item.</param>
        private static void flattenLevel<T>(List<T> accumulation, IEnumerable<T> currentLevel, Func<T, IEnumerable<T>> nextLevel)
        {
            foreach (T item in currentLevel)
            {
                accumulation.AddRange(currentLevel);
                flattenLevel<T>(accumulation, nextLevel(item), nextLevel);
            }
        }
    }
}
