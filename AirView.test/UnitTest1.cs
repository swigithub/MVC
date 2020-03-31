using System;
using System.Collections.Generic;
using System.Data;
using Library.SWI.Survey.BLL;
using Library.SWI.Survey.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.SWI.Survey.Model;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.Common;

namespace AirView.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateChildSections()
        {
            var result = new TSS_SectionDL().GenerateChildSections("GENERATE_CHILD_SITE_SECTIONS", 70131, 2).ToList<TSS_Section>();
        }
        [TestMethod]
        public void GetSectionQuestions()
        {
            TSS_QuestionBL qb = new TSS_QuestionBL();
            TSS_ResponseBL rb = new TSS_ResponseBL();
            TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
            var que =
                    new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_API", 0, 70293.ToString());
            //var que = qb.ToList("GET_QUESTION_BY_SECTION", Id.ToString());
            //foreach (var q in que)
            //{
            //    //q.Responses = rb.ToList("GET_BY_QUESTIONID", q.QuestionId.ToString());
            //    q.Responses = rb.ToList("GET_RESPONSE_BY_SITEQUESTION", q.QuestionId.ToString());
            //}

            foreach (var q in que)
            {
                q.QuestionLogics = qlb.ToList("GET_BY_QUESTIONID", q.QuestionId.ToString());
            }

        }
        [TestMethod]
        public void UpdateSectionStatus()
        {
            new TSS_SurveyResponseDL().UpdateSectionStatus(70296, 90);
        }
        [TestMethod]
        public void SurveyReportResultData()
        {
            TSS_SectionBL s = new TSS_SectionBL();
            var sur = s.SurveyBySiteId(70398);
        }
        [TestMethod]
        public void Tree()
        {
            var sur = new TSS_SectionBL().SurveyBySiteSurveyId(70418);
            TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
            var result = sb.ToList("SURVEY_BY_WO", 728833.ToString(), true);
            // new TSS_SurveyDocumentBL().GetSections("SURVEY_SECTIONS_BY_SITESURVEYID", 70418.ToString(), true);
        }
    }
}
