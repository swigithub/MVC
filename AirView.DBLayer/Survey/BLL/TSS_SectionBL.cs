using AirView.DBLayer.Survey.BLL;
using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.BLL
{
    /*----MoB!----*/
    /*----28-08-2017----*/
    public class TSS_SectionBL
    {
        private TSS_SectionDL sd = new TSS_SectionDL();

        public dynamic Manage(string Filter, TSS_Section sec)
        {
            return sd.Manage(Filter, sec.SectionId, sec.PSectionId, sec.SurveyId, sec.SectionTitle,
                sec.PIterationID, sec.IterationID, sec.Description, sec.SortOrder, sec.IsActive, sec.CreatedOn, sec.CreatedBy, sec.IsRepeatable, sec.IsApplicable,sec.IsSignatureRequired);
        }

        public List<TSS_Section> ToList1(string filter, string value = null)
        {
            DataTable dt = sd.Get(filter, value);
            var sections = dt.ToList<TSS_Section>();
            foreach (var generateChildSection in sections)
            {
                if (!string.IsNullOrEmpty(generateChildSection.ChildTitle) || generateChildSection.ChildTitle == "0")
                    generateChildSection.SectionTitle = $"{generateChildSection.SectionTitle} {generateChildSection.ChildTitle}";
                if (generateChildSection.IsRepeatable) continue;
               
            }

          //  List<TSS_Section> conf = dt.ToList<TSS_Section>();


            return sections;
         //   return new TSS_SurveyDocumentBL().FillRecursiveForDashboard(conf, 0);
        }


        public List<TSS_Section> ToList(string filter, string value = null)
        {
            DataTable dt = sd.Get(filter, value);
            var sections = dt.ToList<TSS_Section>();
            //foreach (var generateChildSection in sections)
            //{
            //    if (!string.IsNullOrEmpty(generateChildSection.ChildTitle) || generateChildSection.ChildTitle == "0")
            //        generateChildSection.SectionTitle = $"{generateChildSection.SectionTitle} {generateChildSection.ChildTitle}";
            //    if (generateChildSection.IsRepeatable) continue;

            //}

            List<TSS_Section> conf = dt.ToList<TSS_Section>();


            //return sections;
        return new TSS_SurveyDocumentBL().SurveyDocumentTemplateTree(conf, 0);
        }



        public List<TSS_Section> ToListWithoutTree(string filter, string value = null)
        {
            DataTable dt = sd.Get(filter, value);
            List<TSS_Section> conf = dt.ToList<TSS_Section>();
            return new TSS_SurveyDocumentBL().SurveyDocumentTemplateTreeWithoutTree(conf, 0);
        }
        public void SaveSectionsOrder(List<TSS_Section> sec)
        {
            var faltObjects = new List<TSS_Section>();
            foreach (var pSection in sec)
            {
                faltObjects.Add(pSection);
                var child = pSection.Sections.Flatten(s => s.Sections).Distinct().ToList();
                if (child.Any())
                    faltObjects.AddRange(child);
            }
            var dt = new dbDataTable().List();
            foreach (var res in faltObjects)
            {
                myDataTable.AddRow(dt, "Value1", res.SectionId, "Value2", res.sort);
            }
            sd.Manage("Save_Sections_SortOrder", 0, 0, 0, "", 0, 0, "", 0, false, new DateTime(), 0, false, false,false, dt);
        }
        public List<TSS_Section> GenerateChildSections(int parentId, int count)
        {
            return new TSS_SectionDL().GenerateChildSections("GENERATE_CHILD_SITE_SECTIONS", parentId, count).ToList<TSS_Section>();
        }
        public List<TSS_Section> DeleteSectionsWithChild(int PSectionId, int SectionId)
        {
            return new TSS_SectionDL().DeleteSectionsWithChild("DELETE_SECTIONS_WITH_CHILD", PSectionId,SectionId ).ToList<TSS_Section>();
        }
        public TSS_Section ToSingle(string filter, string value = null)
        {
            DataTable dt = sd.Get(filter, value);
            return dt.ToList<TSS_Section>().FirstOrDefault();
        }
        public TSS_SurveyDocument SurvayBySite(Int64 SiteSurveyId)
        {

            TSS_SurveyDocument sur = new TSS_SurveyDocument();
            TSS_SectionBL sb = new TSS_SectionBL();
            //----------------------------------------

            //var result = myList.GroupBy(test => test.id)
            //.Select(grp => grp.First())
            //.ToList();

            TSS_SectionDL sd = new TSS_SectionDL();
            DataTable dt = sd.Get("By_SiteSurveyId", SiteSurveyId.ToString());
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow item in dt.Rows)
                {

                    sur.SiteId = int.Parse(dt.Rows[0]["SiteId"].ToString());
                    sur.SiteCode = dt.Rows[0]["SiteCode"].ToString();
                    sur.WoRefId = dt.Rows[0]["WoRefId"].ToString();
                    sur.Status = dt.Rows[0]["Status"].ToString();
                    sur.ClientName = dt.Rows[0]["ClientName"].ToString();

                    sur.CityName = dt.Rows[0]["CityName"].ToString();
                    sur.Scope = dt.Rows[0]["Scope"].ToString();

                    sur.Category = dt.Rows[0]["Category"].ToString();
                    sur.SubCategory = dt.Rows[0]["SubCategory"].ToString();

                    sur.Latitude = dt.Rows[0]["Latitude"].ToString();
                    sur.Longitude = dt.Rows[0]["Longitude"].ToString();

                    sur.SurveyId = int.Parse(dt.Rows[0]["SurveyId"].ToString());
                    sur.SiteSurveyId = int.Parse(dt.Rows[0]["SiteSurveyId"].ToString());

                    sur.SurveyTitle = dt.Rows[0]["SurveyTitle"].ToString();
                    sur.Description = dt.Rows[0]["Sur Descp"].ToString();
                    break;
                }


                var temp = dt.AsEnumerable()
                                  .GroupBy(r => new { Col1 = r["SiteSectionId"] })
                                  .Select(g => g.OrderBy(r => r["SiteSectionId"]).First())
                                  .CopyToDataTable();

                List<TSS_Section> sec = temp.ToList<TSS_Section>();
                TSS_QuestionBL qb = new TSS_QuestionBL();

                foreach (var q in sec)
                {
                    var que = qb.ToList("GET_QUESTION_BY_SECTION", q.SiteSectionId.ToString());
                    if (que != null)
                    {
                        q.Questions.AddRange(que);
                    }

                    TSS_ResponseBL rb = new TSS_ResponseBL();
                    RequiredActionsBL reqA = new RequiredActionsBL();

                    foreach (var r in que)
                    {
                        var resp = rb.ToList("GET_RESPONSE_BY_SITEQUESTION", r.QuestionId.ToString());
                        if (resp != null)
                        {
                            r.Responses.AddRange(resp);
                        }
                    }

                    foreach (var r in que)
                    {
                        var rAction = reqA.ToList("GET_REQACTION_BY_SITEQUESTION", r.QuestionId.ToString());
                        if (rAction != null)
                        {
                            r.ReqActions.AddRange(rAction);
                        }
                    }
                }

                sur.Sections = sec;


                //var temp = dt.AsEnumerable()
                //                  .GroupBy(r => new { Col1 = r["SiteSectionId"] })
                //                  .Select(g => g.OrderBy(r => r["SiteSectionId"]).First())
                //                  .CopyToDataTable();
                //List<TSS_Section> sec = temp.ToList<TSS_Section>();

                //foreach (var item in sec)
                //{
                //    dt.DefaultView.RowFilter = "SiteSectionId='" + item.SiteSectionId + "'";
                //    DataTable ques = (dt.DefaultView).ToTable();
                //    item.Questions = ques.ToList<TSS_Question>();

                //    foreach (var res in item.Questions)
                //    {
                //        dt.DefaultView.RowFilter = "QuestionId='" + res.QuestionId + "'";
                //        DataTable quesLogic = (dt.DefaultView).ToTable();
                //        res.QuestionLogics = quesLogic.ToList<TSS_QuestionLogic>();


                //        dt.DefaultView.RowFilter = "QuestionId='" + res.QuestionId + "'";
                //        DataTable response = (dt.DefaultView).ToTable();
                //        res.Responses = response.ToList<TSS_Response>();

                //        dt.DefaultView.RowFilter = "SiteQuestionId='" + res.QuestionId + "'";
                //        DataTable reqAction = (dt.DefaultView).ToTable();
                //        res.ReqActions = reqAction.ToList<RequiredActions>();
                //    }
                //}
                //sur.Sections = sec;

                //-------------------------------
            }
            return sur;
        }

        public TSS_SurveyDocument SurveyBySiteId(Int64 SiteSurveyId)
        {
            var sur = new TSS_SurveyDocument();
            var sd = new TSS_SectionDL();
            var dt = sd.Get("By_SiteSurveyId_Only_Sections", SiteSurveyId.ToString());
            if (dt.Rows.Count == 0) return sur;
            sur.SiteId = int.Parse(dt.Rows[0]["SiteId"].ToString());
            sur.SiteAddress = dt.Rows[0]["SiteAddress"].ToString();
            sur.SiteCode = dt.Rows[0]["SiteCode"].ToString();
            sur.WoRefId = dt.Rows[0]["WoRefId"].ToString();
            sur.Status = dt.Rows[0]["Status"].ToString();
            sur.ClientName = dt.Rows[0]["ClientName"].ToString();
            sur.CityName = dt.Rows[0]["CityName"].ToString();
            sur.Scope = dt.Rows[0]["Scope"].ToString();
            sur.Category = dt.Rows[0]["Category"].ToString();
            sur.SubCategory = dt.Rows[0]["SubCategory"].ToString();
            sur.Latitude = dt.Rows[0]["Latitude"].ToString();
            sur.Longitude = dt.Rows[0]["Longitude"].ToString();
            sur.SurveyId = int.Parse(dt.Rows[0]["SurveyId"].ToString());
            sur.SiteSurveyId = int.Parse(dt.Rows[0]["SiteSurveyId"].ToString());
            sur.SurveyTitle = dt.Rows[0]["SurveyTitle"].ToString();
            sur.Description = dt.Rows[0]["Sur Descp"].ToString();
            sur.ProjectName = dt.Rows[0]["ProjectName"].ToString();
            sur.WODescription = dt.Rows[0]["WODescription"].ToString();
            sur.SiteTpye = dt.Rows[0]["SiteType"].ToString();
            var tempSections = dt.AsEnumerable()
                .GroupBy(r => new { Col1 = r["SiteSectionId"] })
                .Select(g => g.OrderBy(r => r["SiteSectionId"]).First())
                .CopyToDataTable();

            sur.Sections = new TSS_SurveyDocumentBL().GetSections("SURVEY_SECTIONS_BY_SITESURVEYID",
                sur.SiteSurveyId.ToString(), true, 0, 0, true);

         sur.CompletetionSections = new TSS_SurveyDocumentBL().GetSections("SURVEY_SECTIONS_BY_SITESURVEYID_FOR_COMPLETION",
                sur.SiteSurveyId.ToString(), true, 0, 0, true);
            sur.SiteAttendeesList = new TSS_DashboardBL().GetSiteAttendees("Get_Site_Attendees_For_Dashboard",Convert.ToInt32( sur.SiteId), Convert.ToInt32(sur.SiteSurveyId));
            sur.SectorLocationList = new TSS_SurveyDocumentBL().GetSectorLocations(Convert.ToInt64(sur.SiteSurveyId));
            //foreach (var q in sur.Sections)
            //{
            //    if (q.IsRepeatable) continue;
            //    var que = new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_API", 0, q.SiteSectionId.ToString(), true);
            //    if (que.Any())
            //        q.Questions.AddRange(que);
            //}

            ////sur.Sections = sur.Sections.OrderBy(s => s.PSectionId).ThenBy(s => s.SiteSectionId).ToList();
            //sur.Sections = new TSS_SurveyDocumentBL().FillChildRecursive(sur.Sections, 0);
            return sur;
        }

     

        public TSS_SurveyDocument SurveyBySiteSurveyId(Int64 SiteSurveyId)
        {
            var sur = new TSS_SurveyDocument();
            var sd = new TSS_SectionDL();
            var dt = sd.Get("By_SiteSurveyId_Only_Sections", SiteSurveyId.ToString());
            if (dt.Rows.Count == 0) return sur;
            sur.SiteId = int.Parse(dt.Rows[0]["SiteId"].ToString());
            sur.SiteCode = dt.Rows[0]["SiteCode"].ToString();
            sur.WoRefId = dt.Rows[0]["WoRefId"].ToString();
            sur.Status = dt.Rows[0]["Status"].ToString();
            sur.ClientName = dt.Rows[0]["ClientName"].ToString();
            sur.CityName = dt.Rows[0]["CityName"].ToString();
            sur.Scope = dt.Rows[0]["Scope"].ToString();
            sur.Category = dt.Rows[0]["Category"].ToString();
            sur.SubCategory = dt.Rows[0]["SubCategory"].ToString();
            sur.Latitude = dt.Rows[0]["Latitude"].ToString();
            sur.Longitude = dt.Rows[0]["Longitude"].ToString();
            ;
            sur.SurveyId = int.Parse(dt.Rows[0]["SurveyId"].ToString());
            sur.SiteSurveyId = int.Parse(dt.Rows[0]["SiteSurveyId"].ToString());
            sur.SurveyTitle = dt.Rows[0]["SurveyTitle"].ToString();
            sur.Description = dt.Rows[0]["Sur Descp"].ToString();
            var tempSections = dt.AsEnumerable()
                .GroupBy(r => new { Col1 = r["SiteSectionId"] })
                .Select(g => g.OrderBy(r => r["SiteSectionId"]).First())
                .CopyToDataTable();
            sur.Sections = new TSS_SurveyDocumentBL().GetSections("SURVEY_SECTIONS_BY_SITESURVEYID", SiteSurveyId.ToString(), true);
            //sur.Sections = new TSS_SurveyDocumentBL().FillRecursive(sur.Sections, 0);
            return sur;
        }

        public DataTable GetSurveyBySiteId(Int64 SiteId)
        {
            var sd = new TSS_SurveyDocumentBL();
            return sd.GetSurveyBySiteId("Survey_By_SiteId", SiteId);
        }
    }
}
