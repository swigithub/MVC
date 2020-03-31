using AirView.DBLayer.Schema.DAL;
using AirView.DBLayer.Schema.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Schema.BAL
{
    public class TablesBL
    {
        public List<Tables> TablesList()
        {
            return new DatabaseSchemaDL().GetSchemaInfo("Tables", null).AsEnumerable().Select(col => new Tables() { Name = col.Field<string>("TABLE_NAME") }).ToList();
        }

        public List<Tables> ViewsList()
        {
            return new DatabaseSchemaDL().GetSchemaInfo("Views", null).AsEnumerable().Select(col => new Tables() { Name = col.Field<string>("Name") }).ToList();
        }

        public List<Columns> TablesColumn(string TableName)
        {
            return new DatabaseSchemaDL().GetSchemaInfo("Table_Columns", TableName).AsEnumerable().Select(col => new Columns() { ColumnName = col.Field<string>("COLUMN_NAME") }).ToList();
        }

        public List<Columns> ViewsColumn(string ViewName)
        {
            return new DatabaseSchemaDL().GetSchemaInfo("Views_Columns", ViewName).AsEnumerable().Select(col => new Columns()
            { ColumnName = col.Field<string>("COLUMN_NAME"), DataType = col.Field<string>("DATA_TYPE"), DefinationType = col.Field<string>("DefinationType") }).ToList();
        }
    }
}
