using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AirViewTracker.DBLayer.Common
{
    public class DataContext
    {
        private static string _CONNECTIONSTRING;
        public static SqlCommand OpenConnection(String ConnString)
        {
            try
            {
                _CONNECTIONSTRING = ConnString;
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

        public static bool CloseConnection(SqlCommand loCommand)
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
            finally
            {
                loCommand.Dispose();
            }
        }

        #region Transaction

        public static SqlCommand StartTransaction(SqlCommand loCommand)
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

        public static SqlCommand EndTransaction(SqlCommand loCommand)
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

        public static SqlCommand CancelTransaction(SqlCommand loCommand)
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
        public static void SqlQuery(string qry, string ConnectionString)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
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
                finally
                {
                    con.Close();
                }
            }

        }



        public static SqlCommand SetStoredProcedure(SqlCommand loCommand, string storedProcedureName)
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

        public static SqlCommand AddParameters(SqlCommand loCommand, params object[] parameters)
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

        public static bool ExecuteNonQuery(SqlCommand loCommand)
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

        public static int ExecuteScalar(SqlCommand loCommand)
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

        public static DataTable Select(SqlCommand loCommand)
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

        public static DataSet SelectMany(SqlCommand loCommand)
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
    }
}



