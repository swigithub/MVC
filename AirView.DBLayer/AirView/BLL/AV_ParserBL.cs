using AirView.DBLayer.AirView.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
    public class AV_ParserBL
    {
        AV_ParserDL dl = new AV_ParserDL();

        public bool Insert(string Filter, string ObjectJson,string tempname,string keys,int UserId)
        {
           var result= dl.Insert(Filter, ObjectJson,tempname,keys,UserId);
            return result;
        }
        public DataTable Get(string filter, int TemplateId=0,int UserId=0 )
        {
           var result= dl.Get(filter, TemplateId, UserId);
            return result;
        }



    }
}
