using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.Common
{
    /*----MoB!----*/
    internal class DataContext
    {
        private static string _CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;

        internal static SqlCommand OpenConnection()
        {
            try
            {
                

                SqlConnection loConnection = new SqlConnection(_CONNECTIONSTRING);
                SqlCommand loCommand = new SqlCommand();
                loCommand.CommandTimeout = 0;
                loCommand.CommandType = CommandType.StoredProcedure;
                loCommand.Connection = loConnection;
                loCommand.Connection.Open();
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool CloseConnection(SqlCommand loCommand)
        {
            try
            {
                loCommand.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally {
                loCommand.Dispose();
            }
        }

        #region Transaction

        internal static SqlCommand StartTransaction(SqlCommand loCommand)
        {
            try
            {
                SqlTransaction loTransaction = loCommand.Connection.BeginTransaction();
                loCommand.Transaction = loTransaction;
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static SqlCommand EndTransaction(SqlCommand loCommand)
        {
            try
            {
                loCommand.Transaction.Commit();
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static SqlCommand CancelTransaction(SqlCommand loCommand)
        {
            try
            {
                loCommand.Transaction.Rollback();
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        public static void SqlQuery(string qry) {
            using (SqlConnection con = new SqlConnection(_CONNECTIONSTRING))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    throw;
                }
                finally {
                    con.Close();
                }
            }

        }

      

        internal static SqlCommand SetStoredProcedure(SqlCommand loCommand, string storedProcedureName)
        {
            try
            {
                loCommand.CommandText = storedProcedureName;
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static SqlCommand AddParameters(SqlCommand loCommand, params object[] parameters)
        {
            try
            {
                //loCommand.Parameters.Clear();

                for (int liX = 0, liY = 1; liY < parameters.Length; liX = liX + 2, liY = liY + 2)
                {
                    loCommand.Parameters.AddWithValue(parameters[liX].ToString(), parameters[liY]);
                }
                return loCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool ExecuteNonQuery(SqlCommand loCommand)
        {
            try
            {
                return (loCommand.ExecuteNonQuery() > 0) ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static int ExecuteScalar(SqlCommand loCommand)
        {
            try
            {
                return Convert.ToInt32(loCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static DataTable Select(SqlCommand loCommand)
        {
            try
            {
                DataTable loDataTable = new DataTable();
                SqlDataAdapter loAdapter = new SqlDataAdapter(loCommand);
                loAdapter.Fill(loDataTable);
                return loDataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static DataSet SelectMany(SqlCommand loCommand)
        {
            try
            {
                DataSet loTables = new DataSet();
                SqlDataAdapter loAdapter = new SqlDataAdapter(loCommand);
                loAdapter.Fill(loTables);
                return loTables;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool InsertBulkIntoSQL(string DestinationTableName, DataTable Table)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_CONNECTIONSTRING))
                {
                    cn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                    {
                        bulkCopy.DestinationTableName = DestinationTableName;
                        bulkCopy.WriteToServer(Table);
                    }
                    cn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
