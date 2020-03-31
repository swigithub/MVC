using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;


namespace SWI.Libraries.Common
{
    /*----MoB!----*/
    public static class myDataTable 
    {
        /*Converts List To DataTable*/
        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        /*Converts DataTable To List*/
        //public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        //{
        //    var dataList = new List<TSource>();

        //    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        //    var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
        //                         select new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType }).ToList();
        //    var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
        //                             select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
        //    var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

        //    foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
        //    {
        //        var aTSource = new TSource();
        //        foreach (var aField in commonFields) 
        //        {
        //            PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
        //            var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name]; //if database field is nullable
        //            propertyInfos.SetValue(aTSource, value, null);
        //        }
        //        dataList.Add(aTSource);
        //    }
        //    return dataList;
        //}

        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            if (dataTable!=null)
            {
                var dataList = new List<TSource>();

                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                     select new { Name = aProp.Name }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                         select new { Name = aHeader.ColumnName }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
                foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
                {
                    var aTSource = new TSource();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name]; //if database field is nullable
                        var PropertyType = propertyInfos.PropertyType;
                        try
                        {

                            var GetValueType = (value != null) ? value.GetType() : typeof(string);
                            value = (value == null) ? "" : value;
                            if (GetValueType.Name == PropertyType.Name && !string.IsNullOrEmpty(value.ToString()))
                            {
                                propertyInfos.SetValue(aTSource, value, null);
                            }
                            else
                            {
                                if (PropertyType.Name == "Int32")
                                {
                                    value = (value != null) ? DataType.ToInt32(value.ToString()) : 0;

                                }
                                else if (PropertyType.Name == "Int64")
                                {
                                    value = (value != null) ? DataType.ToInt64(value.ToString()) : 0;
                                }
                                else if (PropertyType.Name == "Single")
                                {
                                    value = (value != null) ? DataType.ToFloat(value.ToString()) : 0;
                                }
                                //
                                else if (PropertyType.Name == "Double")
                                {
                                    value = (value != null) ? DataType.ToDouble(value.ToString()) : 0;
                                }
                                else if (PropertyType.Name == "String")
                                {
                                    value = value.ToString();
                                }
                                else if (PropertyType.Name == "DateTime")
                                {
                                    //value = DateTime.Now.ToString(value.ToString());
                                    value = Convert.ToDateTime(value.ToString());
                                }
                                else if (PropertyType.Name == "Boolean")
                                {
                                    if (value != null && value!="")
                                    {
                                        value = Convert.ToBoolean(Convert.ToInt16(value.ToString()));
                                    }
                                }
                                else if (PropertyType.Name == "Decimal")
                                {
                                    value = (value != null) ? DataType.ToDecimal(value.ToString()) : 0;
                                }

                                propertyInfos.SetValue(aTSource, value, null);
                            }

                        }
                        catch(Exception er) { }

                    }
                    dataList.Add(aTSource);
                }
                return dataList;
            }
            return null;
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static List<TSource> ToImportList<TSource>(this DataTable dataTable) where TSource : new()
        {
            if (dataTable != null)
            {
                var dataList = new List<TSource>();

                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                     select new { Name = aProp.Name }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                         select new { Name = aHeader.ColumnName }).ToList();
                //.Replace(" ", "").Replace("-", "").Replace("_", "")
                //var commonFields = objFieldNames.Union(dataTblFieldNames).ToList();
                var commonFields = dataTblFieldNames.ToList();
                foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
                {
                    var aTSource = new TSource();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name.Replace(" ", "").Replace("-", "").Replace("_", ""));
                        var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name]; //if database field is nullable
                        var PropertyType = propertyInfos.PropertyType;
                        try
                        {

                            var GetValueType = (value != null) ? value.GetType() : typeof(string);
                            value = (value == null) ? "" : value;
                            if (GetValueType.Name == PropertyType.Name && !string.IsNullOrEmpty(value.ToString()))
                            {
                                propertyInfos.SetValue(aTSource, value, null);
                            }
                            else
                            {
                                if (PropertyType.Name == "Int32")
                                {
                                    value = (value != null) ? DataType.ToInt32(value.ToString()) : 0;

                                }
                                else if (PropertyType.Name == "Int64")
                                {
                                    value = (value != null) ? DataType.ToInt64(value.ToString()) : 0;
                                }
                                else if (PropertyType.Name == "Single")
                                {
                                    value = (value != null) ? DataType.ToFloat(value.ToString()) : 0;
                                }
                                //
                                else if (PropertyType.Name == "Double")
                                {
                                    value = (value != null) ? DataType.ToDouble(value.ToString()) : 0;
                                }
                                else if (PropertyType.Name == "String")
                                {
                                    value = value.ToString();
                                }
                                else if (PropertyType.Name == "Boolean")
                                {
                                    if (value != null)
                                    {
                                        value = DataType.ToBoolean(value.ToString());
                                    }
                                }
                                else if (PropertyType.Name == "Decimal")
                                {
                                    value = (value != null) ? DataType.ToDecimal(value.ToString()) : 0;
                                }

                                propertyInfos.SetValue(aTSource, value, null);
                            }

                        }
                        catch(Exception ex) { throw;  }

                    }
                    dataList.Add(aTSource);
                }
                return dataList;
            }
            return null;
        }
        
        public static DataTable Create(params object[] columns) {
            try
            {
                DataTable tempTable = new DataTable();
                for (int i = 0; i < columns.Length; i++)
                {
                    string ColumnName = columns[i].ToString();
                    tempTable.Columns.Add(ColumnName);
                }
                return tempTable;
            }
            catch (Exception er)
            {

                return null;
            }
        }

        public static DataTable AddRow(DataTable Table, params object[] Row)
        {
            try
            {
                DataRow tempRow = Table.NewRow();
                for (int liX = 0, liY = 1; liY < Row.Length; liX = liX + 2, liY = liY + 2)
                {
                    tempRow[Row[liX].ToString()] = Row[liY];
                }
                Table.Rows.Add(tempRow);
                return Table;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
        //public static DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    //Get all the properties
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        //Setting column names as Property names
        //        dataTable.Columns.Add(prop.Name);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}






        //  DataTable Table = ToDataTable(wo);
        //private DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    //Get all the properties
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        //Setting column names as Property names
        //        dataTable.Columns.Add(prop.Name);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}
    }
}
