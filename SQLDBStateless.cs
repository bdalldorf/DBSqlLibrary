using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.IO;
using DBSqlLibrary;
using System.Data.SqlClient;

namespace DBSqlLibrary
{
    public class SQLDBStateless
    {
        private string _ConnectionString { get; set; }

        public SQLDBStateless(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        private SqlConnection OpenConnection()
        {
            SqlConnection SqlConnection = new SqlConnection(_ConnectionString);

            SqlConnection.Open();
            return SqlConnection;
        }

        private static SqlConnection OpenConnection(string connectionString)
        {
            SqlConnection SqlConnection = new SqlConnection(connectionString);

            SqlConnection.Open();
            return SqlConnection;
        }

        private SqlTransaction BeginTransaction()
        {
            return OpenConnection().BeginTransaction();
        }

        private static SqlTransaction BeginTransaction(string connectionString)
        {
            return OpenConnection(connectionString).BeginTransaction();
        }

        private static SqlCommand Command(string sql, SqlConnection SqlConnection)
        {
            return new SqlCommand(sql, SqlConnection);
        }

        private static SqlCommand Command(string sql, SqlTransaction SqlTransaction)
        {
            return new SqlCommand(sql, SqlTransaction.Connection, SqlTransaction);
        }

        private static SqlDataAdapter DataAdapter(string sql, SqlConnection SqlConnection)
        {
            return new SqlDataAdapter(sql, SqlConnection);
        }

        public int ExecNonQuery(string sql)
        {
            int Results = -1;
            SqlConnection SqlConnection = null;

            try
            {
                SqlConnection = OpenConnection();

                Results = Command(sql, SqlConnection).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (SqlConnection != null)
                    SqlConnection.Close();
            }

            return Results;
        }

        public static int ExecNonQuery(string sql, string connectionString)
        {
            int Results = -1;
            SqlConnection SqlConnection = null;

            try
            {
                SqlConnection = OpenConnection(connectionString);

                Results = Command(sql, SqlConnection).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (SqlConnection != null)
                    SqlConnection.Close();
            }

            return Results;
        }

        public bool ExecNonQueryTransaction(List<string> sqlStatements)
        {
            SqlTransaction SqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                SqlTransaction = BeginTransaction();

                foreach (string sqlStatement in sqlStatements)
                {
                    Command(sqlStatement, SqlTransaction).ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Rollback();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                    return false;
                }
            }
            finally
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Commit();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }

            return true;
        }

        public static bool ExecNonQueryTransaction(List<string> sqlStatements, string connectionString)
        {
            SqlTransaction SqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                SqlTransaction = BeginTransaction(connectionString);

                foreach (string sqlStatement in sqlStatements)
                {
                    Command(sqlStatement, SqlTransaction).ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Rollback();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                    return false;
                }
            }
            finally
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Commit();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }

            return true;
        }

        public long ExecInsertNonQueryReturnID(string sql)
        {
            long RowID = SQLDBCommon.EmptyLong;
            SqlTransaction SqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                SqlTransaction = BeginTransaction();

                SqlCommand SqlCommand = Command(sql, SqlTransaction);
                SqlCommand.ExecuteNonQuery();

                RowID = SqlCommand.LastInsertedId;
            }
            catch (Exception exception)
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Rollback();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }
            finally
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Commit();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }

            return RowID;
        }

        public static long ExecInsertNonQueryReturnID(string sql, string connectionString)
        {
            long RowID = MySQLDBCommon.EmptyLong;
            SqlTransaction SqlTransaction = null;
            StringBuilder l_Results = new StringBuilder();

            try
            {
                SqlTransaction = BeginTransaction(connectionString);

                SqlCommand SqlCommand = Command(sql, SqlTransaction);
                SqlCommand.ExecuteNonQuery();

                RowID = SqlCommand.LastInsertedId;
            }
            catch (Exception exception)
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Rollback();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }
            finally
            {
                if (SqlTransaction != null)
                {
                    SqlTransaction.Commit();
                    SqlTransaction.Dispose();
                    SqlTransaction = null;
                }
            }

            return RowID;
        }

        public object ExecScalar(string sql)
        {
            {
                SqlConnection SqlConnection = null;
                object ScalarReturnObject = null;

                try
                {
                    SqlConnection = OpenConnection();

                    ScalarReturnObject = Command(sql, SqlConnection).ExecuteScalar();
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (SqlConnection != null)
                        SqlConnection.Close();
                }

                return ScalarReturnObject;
            }
        }

        public static object ExecScalar(string sql, string connectionString)
        {
            {
                SqlConnection SqlConnection = null;
                object ScalarReturnObject = null;

                try
                {
                    SqlConnection = OpenConnection(connectionString);

                    ScalarReturnObject = Command(sql, SqlConnection).ExecuteScalar();
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (SqlConnection != null)
                        SqlConnection.Close();
                }

                return ScalarReturnObject;
            }
        }

        public MySqlDataReader ExecDataReader(string sql)
        {
            SqlConnection SqlConnection = null;
            MySqlDataReader MySqlDataReader = null;

            try
            {
                SqlConnection = OpenConnection();

                MySqlDataReader = Command(sql, SqlConnection).ExecuteReader();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (SqlConnection != null)
                    SqlConnection.Close();
            }
            return MySqlDataReader;
        }

        public static MySqlDataReader ExecDataReader(string sql, string connectionString)
        {
            SqlConnection SqlConnection = null;
            MySqlDataReader MySqlDataReader = null;

            try
            {
                SqlConnection = OpenConnection(connectionString);

                MySqlDataReader = Command(sql, SqlConnection).ExecuteReader();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                if (SqlConnection != null)
                    SqlConnection.Close();
            }

            return MySqlDataReader;
        }

        public DataTable ExecDataTable(string sql)
        {
            {
                SqlConnection SqlConnection = null;
                SqlDataAdapter SqlDataAdapter = null;
                DataTable DataTable = new DataTable();

                try
                {
                    SqlConnection = OpenConnection();

                    SqlDataAdapter = DataAdapter(sql, SqlConnection);
                    SqlDataAdapter.Fill(DataTable);
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (SqlConnection != null)
                        SqlConnection.Close();
                }

                return DataTable;
            }
        }

        public static DataTable ExecDataTable(string sql, string connectionString)
        {
            {
                SqlConnection SqlConnection = null;
                SqlDataAdapter SqlDataAdapter = null;
                DataTable DataTable = new DataTable();

                try
                {
                    SqlConnection = OpenConnection(connectionString);

                    SqlDataAdapter = DataAdapter(sql, SqlConnection);
                    SqlDataAdapter.Fill(DataTable);
                }
                catch (Exception exception)
                {
                }
                finally
                {
                    if (SqlConnection != null)
                        SqlConnection.Close();
                }

                return DataTable;
            }
        }

        public static string GetDatabaseTableFieldName(object model, string fieldName)
        {
            return (string)model.GetType().GetProperty(fieldName)
                .CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute))
                .First()
                .ConstructorArguments
                .First()
                .Value;
        }

        public static string GetDatabaseTableFieldName(FieldInfo fieldInfo)
        {
            return (string)fieldInfo
                .CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute))
                .First()
                .ConstructorArguments
                .First()
                .Value;
        }

        public static List<string> ModelFieldNames(Type model)
        {
            List<String> ModelFieldNames = new List<string>();

            foreach (PropertyInfo property in model.GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                string Value = (string)property.CustomAttributes.Where(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute)).First().ConstructorArguments.First().Value;
                ModelFieldNames.Add(Value);
            }

            return ModelFieldNames;
        }

        public static List<object> ModelFieldValues(object model)
        {
            List<object> ModelFieldValues = new List<object>();

            foreach (var properties in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                object Value = properties.GetValue(model);
                ModelFieldValues.Add(MySQLDBCommon.SetValueForSql(Value));
            }

            return ModelFieldValues;
        }

        public static string GenerateInsertFields(DatabaseModel model)
        {
            StringBuilder StringBuilderFields = new StringBuilder();
            StringBuilder StringBuilderValues = new StringBuilder();

            foreach (PropertyInfo property in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromInsertAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = property.GetValue(model);

                StringBuilderFields.Append(StringBuilderFields.Length == 0 ? $"({FieldName}" : $", {FieldName}");
                StringBuilderValues.Append(StringBuilderValues.Length == 0 ? $"{MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }

            return StringBuilderFields.Append($") VALUES ({StringBuilderValues.ToString()})").ToString();
        }

        public static string GenerateUpdateFields(DatabaseModel model)
        {
            StringBuilder StringBuilder = new StringBuilder();

            foreach (PropertyInfo property in model.GetType().GetProperties(System.Reflection.BindingFlags.Public
                  | System.Reflection.BindingFlags.GetProperty | BindingFlags.Instance))
            {
                CustomAttributeData l_ExcludeFromUpdate = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldExcludeFromUpdateAttribute));

                if (l_ExcludeFromUpdate != null && Convert.ToBoolean(l_ExcludeFromUpdate.ConstructorArguments.First().Value))
                    continue;

                CustomAttributeData l_TableFieldName = property.CustomAttributes.FirstOrDefault(customAttributes => customAttributes.AttributeType == typeof(TableFieldNameAttribute));

                if (l_TableFieldName == null)
                    continue;

                string FieldName = (string)l_TableFieldName.ConstructorArguments.First().Value;
                object FieldValue = property.GetValue(model);
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}" : $", {FieldName} = {MySQLDBCommon.SetValueForSql(FieldValue)}");
            }
            return StringBuilder.ToString();
        }

        public static string GenerateStandardInsertStatement(DatabaseModel model)
        {
            return $"INSERT INTO {model.TableName()} {GenerateInsertFields(model)}";
        }

        public static string GenerateStandardUpdateStatement(DatabaseModel model, string primaryKeyFieldName, object primaryKeyValue)
        {
            return $"UPDATE {model.TableName()} SET {GenerateUpdateFields(model)} WHERE {GetDatabaseTableFieldName(model, primaryKeyFieldName)} = {MySQLDBCommon.SetValueForSql(primaryKeyValue)}";
        }

        public static string GenerateStandardDeleteStatement(DatabaseModel model, string primaryKeyFieldName, object primaryKeyValue)
        {
            return $"DELETE FROM {model.TableName()} WHERE {GetDatabaseTableFieldName(model, primaryKeyFieldName)} = {MySQLDBCommon.SetValueForSql(primaryKeyValue)}";
        }
    }
}