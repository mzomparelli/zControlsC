using System.ComponentModel;
using System.Data.SqlClient;
using System;
//using OleDb = System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using zControlsC.Encryption;

namespace zControlsC.DataConnection
{
    public class SQLServerConn : IDisposable
    {
        #region "Declarations"
        private string zFilename = "zControlsC.dll version 1.0.0.0";
        private string zClass = "SQLServerConn";


        private string connectionString;
        private string userPassword;
        private SqlCommandBuilder CommandBuilder;
        private DataTable dt = new DataTable();
        private SqlDataAdapter da;
        private SqlConnection db = new SqlConnection();
        private SqlBulkCopy bulkCopy;

        private bool isStoredProcedure = false;
        private string storedProcedureName = "";
        private List<SqlParameter> storedProcedureParams = new List<SqlParameter>();


        //Private _conn As New SqlConnection
        //Public cmd As New SqlCommand
        #endregion

        #region "Properties and Structures"

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override string ToString()
        {
            return "Michael Zomparelli";
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return object.Equals(this, obj);
        }

        private string userID;
        public string UserID
        {
            get { return userID; }
        }

        private string sqlServer;
        public string SQLServer
        {
            get { return sqlServer; }
        }

        private string sqlString;
        public string SQLString
        {
            get { return sqlString; }
        }

        private string database;
        public string Database
        {
            get { return database; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public bool IsStoredProcedure
        {
            get { return isStoredProcedure; }
            set { isStoredProcedure = value; }
        }

        public string StoredProcedureName
        {

            get { return storedProcedureName; }
            set { storedProcedureName = value; }
        }

        public List<SqlParameter> StoredProcedureParams
        {
            get { return storedProcedureParams; }
            set { storedProcedureParams = value; }
        }

        public StructureFillTable QueryResults = new StructureFillTable();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public struct StructureFillTable
        {
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return object.Equals(this, obj);
            }

            public DataTable Table;
            public SqlDataAdapter DataAdapter;

            public int Rows;
            public string Errors;
        }

        private SQLStructure structureSQL = new SQLStructure();

        public SQLStructure SQL
        {
            get { return structureSQL; }
            set { structureSQL = value; }
        }


        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public struct SQLStructure
        {

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return object.Equals(this, obj);
            }

            public string[] SelectField;
            public string[] Table;
            public string[] WhereClause;
        }

        public DataTable TableChanges
        {
            get { return dt.GetChanges(); }
        }


        private bool useWindowsCredentials;
        public bool UseWindowsCredentials
        {
            get { return useWindowsCredentials; }
            set
            {
                useWindowsCredentials = value;
                if (value == true)
                {
                    db.ConnectionString = "Provider=SQLOLEDB;Data Source=" + sqlServer + ";Integrated Security=True;Pooling=False;Initial Catalog=" + database + ";";
                }

                else
                {
                    MakeConnectionString();
                }
            }
        }

        public SqlConnection SqlConnection
        {
            get { return this.db; }
        }

        #endregion

        #region "Public Methods"

        public SQLServerConn(string Server, string UserID, string UserPassword, string Database)
        {

            database = Database;
            sqlServer = Server;
            userPassword = UserPassword;
            userID = UserID;
            MakeConnectionString();

            structureSQL.Table = new string[0];
            structureSQL.SelectField = new string[99];
            structureSQL.WhereClause = new string[99];

        }

        public SQLServerConn(string EncryptedCredFile)
        {
            if (File.Exists(EncryptedCredFile))
            {
                StreamReader oRead;
                oRead = File.OpenText(EncryptedCredFile);

                if (oRead.Peek() == -1) return;
                sqlServer = EncryptStrings.DecryptString(oRead.ReadLine());

                if (oRead.Peek() == -1) return;
                userID = EncryptStrings.DecryptString(oRead.ReadLine());

                if (oRead.Peek() == -1) return;
                userPassword = EncryptStrings.DecryptString(oRead.ReadLine());

                if (oRead.Peek() == -1) return;
                database = EncryptStrings.DecryptString(oRead.ReadLine());

                oRead.Close();
                oRead.Dispose();

                MakeConnectionString();

                structureSQL.Table = new string[0];
                structureSQL.SelectField = new string[99];
                structureSQL.WhereClause = new string[99];

            }           

        }


        [Description("Uses the specified SQL string to run a query")]
        public void RunQuery(string SQL)
        {
            sqlString = SQL;
            ClearQueryResults();
            QueryResults = FillTable();
        }


        public void BulkInsert(string tableName, ref DataTable dataTable, bool lockTable)
        {
            db.Open();
            if (lockTable)
            {
                bulkCopy = new SqlBulkCopy(db, SqlBulkCopyOptions.TableLock, null);
            }
            else
            {
                bulkCopy = new SqlBulkCopy(db);
            }

            bulkCopy.BulkCopyTimeout = 1200;
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.BatchSize = 20000;
            bulkCopy.WriteToServer(dataTable);
            bulkCopy.Close();
            db.Close();
        }

        


        public void RefreshData(bool SaveChanges)
        {
            if (SaveChanges) UpdateDatabase();
            RunQuery(sqlString);
        }

        public void CustomUpdateDatabase()
        {
              

            try
            {
                QueryResults.DataAdapter.Update(this.TableChanges);
                QueryResults.Table.AcceptChanges();
            }
            catch (ArgumentNullException)
            {
            }
            //this means no changes have been made
            catch (Exception ex)
            {
                MessageBox.Show("File: " +
                                this.zFilename +
                                Environment.NewLine +
                                "Class: " + this.zClass +
                                Environment.NewLine +
                                "Method: UpdateDatabase()" +
                                Environment.NewLine +
                                Environment.NewLine +
                                ex.Message +
                                Environment.NewLine +
                                Environment.NewLine +
                                "This error may prevent the data from being updated in the database.",
                                "zControlsC", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

            }


        }

        public void UpdateDatabase()
        {

            SetSQLCommands();


            try
            {
                QueryResults.DataAdapter.Update(this.TableChanges);
                QueryResults.Table.AcceptChanges();
            }
            catch (ArgumentNullException)
            {
            }
            //this means no changes have been made
            catch (Exception ex)
            {
                MessageBox.Show("File: " +
                                this.zFilename +
                                Environment.NewLine +
                                "Class: " + this.zClass +
                                Environment.NewLine +
                                "Method: UpdateDatabase()" +
                                Environment.NewLine +
                                Environment.NewLine +
                                ex.Message +
                                Environment.NewLine +
                                Environment.NewLine +
                                "This error may prevent the data from being updated in the database.",
                                "zControlsC", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

            }

            
        }

        

        #endregion

        #region "Private Methods"

        private void SetSQLCommands()
        {
            if (QueryResults.DataAdapter.InsertCommand != null)
            {
                QueryResults.DataAdapter.InsertCommand = null;
            }
            if (QueryResults.DataAdapter.UpdateCommand != null)
            {
                QueryResults.DataAdapter.UpdateCommand = null;
            }
            if (QueryResults.DataAdapter.DeleteCommand != null)
            {
                QueryResults.DataAdapter.DeleteCommand = null;
            }
            //CommandBuilder.SetAllValues = False
            QueryResults.DataAdapter.InsertCommand = CommandBuilder.GetInsertCommand();
            QueryResults.DataAdapter.UpdateCommand = CommandBuilder.GetUpdateCommand();
            QueryResults.DataAdapter.DeleteCommand = CommandBuilder.GetDeleteCommand();
        }

        private void MakeConnectionString()
        {
            connectionString = String.Format("Data Source={0};Initial Catalog={1};User id={2};Password={3}", this.sqlServer, this.database, this.userID, this.userPassword);
            db.ConnectionString = connectionString;
            //_conn.ConnectionString = strConnectionString
            //cmd.CommandType = CommandType.StoredProcedure
        }

        public void ClearQueryResults()
        {
            try
            {
                this.QueryResults.Table.Dispose();
                this.QueryResults.Rows = 0;
                this.QueryResults.Errors = "";
            }
            catch (Exception)
            {

            }

        }

        private StructureFillTable FillTable()
        {
            StructureFillTable StructureTable = new StructureFillTable();

            try
            {
                db.Open();
                if (isStoredProcedure)
                {
                    if (sqlString == "")
                    {
                        StructureTable.Errors = "The stored procedure has not been identified.";
                        return StructureTable;
                    }

                    SqlCommand cmd = new SqlCommand(sqlString, db);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in storedProcedureParams)
                    {
                        cmd.Parameters.Add(param);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.UpdateBatchSize = 20000;
                    da.SelectCommand.CommandTimeout = 0;
                    da.Fill(dt);
                    StructureTable.DataAdapter = da;
                    StructureTable.Table = dt;
                    StructureTable.Rows = dt.Rows.Count;
                    StructureTable.Errors = "";
                    CommandBuilder = new SqlCommandBuilder(da);
                    return StructureTable;
                    

                }
                else
                {
                    da = new SqlDataAdapter(sqlString, db);
                    da.SelectCommand.CommandTimeout = 0;
                    da.Fill(dt);
                    StructureTable.DataAdapter = da;
                    StructureTable.Table = dt;
                    StructureTable.Rows = dt.Rows.Count;
                    StructureTable.Errors = "";
                    CommandBuilder = new SqlCommandBuilder(da);
                    return StructureTable;
                }

                
                
            }
            catch (Exception ex)
            {
                //All exceptions are stored in a variable without displaying a message box to the user.
                //This allows the error to be deciphered so it can be determined in code if a message box is necessary.
                //This will be determined outside of this function.

                StructureTable.Errors = ex.Message;
                return StructureTable;
            }
            finally
            {
                db.Close();
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            db.Dispose();
            db = null;
            connectionString = null;
            CommandBuilder = null;
            dt = null;
            da = null;

            GC.Collect();
        }

        #endregion
    }
}
