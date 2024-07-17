using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace DBSqlLibrary.Models
{
    [TableName("usrUser_usr")]
    public partial class UserModel : DatabaseModel
    {
        #region private properties

        #endregion

        #region public properties

        [TableFieldName("usrID")]
        [TableFieldExcludeFromUpdate(true)]
        [TableFieldExcludeFromInsert(true)]
        public int ID { get; set; }
        [TableFieldName("usrUID")]
        public string UID { get; set; }
        [TableFieldName("usrFirstName")]
        public string FirstName { get; set; }
        [TableFieldName("usrLastName")]
        public string LastName { get; set; }
        [TableFieldName("usrEmailAddress")]
        public string EmailAddress { get; set; }

        #endregion

        #region Constructors

        public UserModel() : base(SQLConnectionString.ConnectionString) { }

        public UserModel(int id) : base(SQLConnectionString.ConnectionString)
        {
            DataTable DataTable = _SQLDBStateless.ExecDataTable($"SELECT * FROM {this.TableName()} WHERE {SQLDBStateless.GetDatabaseTableFieldName(this, nameof(this.ID))} = {SQLDBCommon.SetValueForSql(id)}");

            if (DataTable.Rows.Count == 1)
            {
                LoadByUserModelDataRow(DataTable.Rows[0]);
            }
        }

        public UserModel(DataRow dataRow) : base(SQLConnectionString.ConnectionString)
        {
            LoadByUserModelDataRow(dataRow);
        }

        private void LoadByUserModelDataRow(DataRow dataRow)
        {
            this.ID = SQLDBCommon.GetValueIntFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.ID)));
            this.UID = SQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.UID)));
            this.FirstName = SQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.FirstName)));
            this.LastName = SQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.LastName)));
            this.EmailAddress = SQLDBCommon.GetValueStringFromSql(GetDatabaseTableFieldName(dataRow, nameof(this.EmailAddress)));
        }

        private object GetDatabaseTableFieldName(DataRow dataRow, string fieldName) => dataRow[SQLDBStateless.GetDatabaseTableFieldName(this, fieldName)];
        

        #endregion

        #region Additional Methods

        private List<string> ModelFields
        {
            get { return SQLDBStateless.ModelFieldNames(typeof(UserModel)); }
        }

        private List<object> ModelValues
        {
            get { return SQLDBStateless.ModelFieldValues(this); }        
        }

        #endregion

        #region CRUD Methods

        public void Save()
        {
            if (this.ID.IsEmpty())
                Insert();
            else
                Update();

        }

        private void Insert()
        {
            this.ID = (int)_SQLDBStateless.ExecInsertNonQueryReturnID(SQLDBStateless.GenerateStandardInsertStatement(this));
        }

        private void Update()
        {
            _SQLDBStateless.ExecNonQuery(SQLDBStateless.GenerateStandardUpdateStatement(this, nameof(this.ID), this.ID));
        }

        public void Delete()
        {
            _SQLDBStateless.ExecNonQuery(SQLDBStateless.GenerateStandardDeleteStatement(this, nameof(this.ID), this.ID));
        }

        #endregion
    }
}
