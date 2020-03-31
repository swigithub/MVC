using AirView.DBLayer.Schema.DAL;
using AirView.DBLayer.Schema.Models;
using AirView.DBLayer.Template.DAL;
using AirView.DBLayer.Template.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AirView.DBLayer.Template.BLL
{
    public class TMP_ModuleTypeBL
    {
        public List<ModuleType> ModuleTypesList(string ModuleType)
        {
            TMP_ModuleTypesDL _TMP_ModuleTypesDL = new TMP_ModuleTypesDL();
            return _TMP_ModuleTypesDL.GetTb(ModuleType).AsEnumerable().Select(x => new ModuleType()
            {
                Id = x.Field<decimal>("DefinationId"),
                ModuleName = x.Field<string>("DefinationName")
            }).ToList();
        }
    }
}
