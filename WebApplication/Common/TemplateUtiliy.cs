using AirView.DBLayer.Template.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SWI.AirView.Common
{
    public static class TemplateUtiliy
    {
        public static string NodeUrlByNodeType(string NodeType)
        {
            string Nodeurl = string.Empty;
            switch (NodeType.ToUpper())
            {
                case "CHART":  // Chart
                    Nodeurl = "_chartPage.cshtml";
                    break;
                case "TABLE":  // Table
                    Nodeurl = "_siteDataTablePage.cshtml";
                    break;
                case "MAP":  // Map
                    Nodeurl = "_mapPage.cshtml";
                    break;
                case "PAGE":  // Page
                    Nodeurl = "_welcomePage.cshtml";
                    break;
                case "IMAGES":  // Images
                    Nodeurl = "_ooklaImagesPage.cshtml";
                    break;
                case "TABLE WITH MAP":  // Table With Map
                    Nodeurl = "_tableWithMap.cshtml";
                    break;
                default:
                    Nodeurl = null;
                    break;
            }
            return Nodeurl;
        }

        public static string QueryBuilder(string QueryBuilderJson, string QueryWhereClause)
        {
            StringBuilder ColumnWithAliasAndFunction = new StringBuilder();
            StringBuilder ColumnGroupBy = new StringBuilder();
            StringBuilder ColumnSortBy = new StringBuilder();
            StringBuilder WhereClause = new StringBuilder();

            Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(QueryBuilderJson);
            bool IsAggFunctionUsed = DeserializeQuerySetting.QuerySetting.MetaData.Where(x => x.Funtion != "-1")?.Count() == 0 ? false : true;
            bool IsGroupByUsed = DeserializeQuerySetting.QuerySetting.MetaData.Where(x => x.GroupBy == true)?.Count() == 0 ? false : true;

            if (DeserializeQuerySetting.QuerySetting.MetaData.Count > 0)
            {
                foreach(var setting in DeserializeQuerySetting.QuerySetting.MetaData)
                {
                    // CREATE COLUMNS AND THEIR ALIAS AND AGGERIGATE FUNCTIONS STRING
                    if (setting.Funtion != "-1")
                        ColumnWithAliasAndFunction.Append(string.Format(" {0}([{1}]) ", setting.Funtion, setting.ColumnName));
                    else
                        ColumnWithAliasAndFunction.Append(string.Format(" [{0}] ", setting.ColumnName));

                    if (setting.Alias != string.Empty)
                        ColumnWithAliasAndFunction.Append(string.Format(" As '{0}', ", setting.Alias));


                    // CREATE GROUP BY CLAUSE
                    if (setting.GroupBy == true)
                        if (string.IsNullOrEmpty(ColumnGroupBy.ToString()))
                            ColumnGroupBy.Append(string.Format(" GROUP BY [{0}], ", setting.ColumnName));
                        else
                            ColumnGroupBy.Append(string.Format(" [{0}], ", setting.ColumnName));

                    // CREATE ORDER BY STRING
                    if (setting.GroupBy == true && setting.SortBy != "-1")
                        if (string.IsNullOrEmpty(ColumnSortBy.ToString()))
                            ColumnSortBy.Append(string.Format(" ORDER BY [{0}] {1}, ", setting.ColumnName, setting.SortBy));
                        else
                            ColumnSortBy.Append(string.Format(" [{0}] {1}, ", setting.ColumnName, setting.SortBy));
                }
            }

            string Query = string.Empty;
            if(DeserializeQuerySetting.QuerySetting.MetaData.Count == 0)
                Query= string.Format("SELECT {0} FROM {1} {2} {3} {4}", "*", DeserializeQuerySetting.QuerySetting.Table, QueryWhereClause, ColumnGroupBy.ToString().TrimEnd(',', ' '), ColumnSortBy.ToString().TrimEnd(',', ' '));
            else
                Query= string.Format("SELECT {0} FROM {1} {2} {3} {4}", ColumnWithAliasAndFunction.ToString().TrimEnd(',', ' '), DeserializeQuerySetting.QuerySetting.Table, QueryWhereClause, ColumnGroupBy.ToString().TrimEnd(',', ' '), ColumnSortBy.ToString().TrimEnd(',', ' '));
            return Query;
        }

        
    }
}