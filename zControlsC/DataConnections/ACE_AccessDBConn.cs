using System.ComponentModel;
using System.Windows.Forms;
using System;
using OleDb = System.Data.OleDb;
using System.Data;

namespace zControlsC.DataConnection
{
    public class ACE_AccessDBConn : IDisposable
    {
        #region "Declarations"
        private string zFilename = "zControls.dll version 1.0.0.2";
        private string zClass = "AccessDBConn";

        private string strConnectionString;
        private string strUserPassword;
        private OleDb.OleDbCommandBuilder CommandBuilder;
        private DataTable dt = new DataTable();
        private OleDb.OleDbDataAdapter da;
        private OleDb.OleDbConnection db = new OleDb.OleDbConnection();
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

        private string strSQLString;
        public string SQLString
        {
            get { return strSQLString; }
        }

        private string strDatabase;
        public string Database
        {
            get { return strDatabase; }
        }

        public string ConnectionString
        {
            get { return strConnectionString; }
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
            public OleDb.OleDbDataAdapter DataAdapter;

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
            public string[] SortBy;
        }

        public DataTable TableChanges
        {
            get { return dt.GetChanges(); }
        }
        #endregion

        #region "Public Methods"

        public ACE_AccessDBConn(string Database, string Password)
        {
            strDatabase = Database;
            strUserPassword = Password;
            MakeConnectionString();

            structureSQL.Table = new string[0];
            structureSQL.SelectField = new string[99];
            structureSQL.WhereClause = new string[99];
        }

        public ACE_AccessDBConn(string Database)
        {
            strDatabase = Database;
            strUserPassword = "";
            MakeConnectionString();

            structureSQL.Table = new string[0];
            structureSQL.SelectField = new string[99];
            structureSQL.WhereClause = new string[99];
        }


        [Description("Uses the specified SQL string to run a query")]
        public void RunQuery(string SQL)
        {
        //StartOver:
            strSQLString = SQL;
            ClearQueryResults();
            QueryResults = FillTable();
        }
        //[Description("Uses the specified SQL Structure to run a query. A SQL string will be created from the structure.")]
        //public void RunQuery(SQLStructure SQL)
        //{
        //    if (MakeSQLString(SQL))
        //    {
        //        RunQuery(strSQLString);
        //    }
        //    else
        //    {
        //        MessageBox.Show("There is an error in the SQL Structure.",
        //                        "zControlsC", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}
        //[Description("Uses the classes SQL structure to build the SQL string.")]
        //public void RunQuery()
        //{
        //    RunQuery(structureSQL);
        //}

        public void RefreshData(bool SaveChanges)
        {
            if (SaveChanges) UpdateDatabase();
            RunQuery(strSQLString);
        }

        public void UpdateDatabase()
        {
            SetSQLCommands();
            try
            {
                QueryResults.DataAdapter.Update(this.TableChanges);
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
                QueryResults.Table.AcceptChanges();
            }


            //Me.RunQuery(strSQLString)
        }
        #endregion

        #region "Private Methods"

        private void SetSQLCommands()
        {
            if (!(QueryResults.DataAdapter.InsertCommand == null))
            {
                QueryResults.DataAdapter.InsertCommand = null;
            }
            if (!(QueryResults.DataAdapter.UpdateCommand == null))
            {
                QueryResults.DataAdapter.UpdateCommand = null;
            }
            if (!(QueryResults.DataAdapter.DeleteCommand == null))
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
            if (!(strUserPassword == ""))
            {
                strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase + ";Persist Security Info=True;ACE OLEDB:Database Password=" + strUserPassword;
            }
            else
            {
                strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase;
            }
        }

        public void ClearQueryResults()
        {
            if ((this.QueryResults.Table != null))
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


        }

        private StructureFillTable FillTable()
        {
            StructureFillTable StructureTable = new StructureFillTable();
            db.ConnectionString = strConnectionString;
            try
            {
                //db.Open()
                da = new OleDb.OleDbDataAdapter(strSQLString, db);
                da.Fill(dt);
                StructureTable.DataAdapter = da;
                StructureTable.Table = dt;
                StructureTable.Rows = dt.Rows.Count;
                StructureTable.Errors = "";
                CommandBuilder = new OleDb.OleDbCommandBuilder(da);
                return StructureTable;
            }
            //db.Close()
            catch (Exception ex)
            {
                //All exceptions are stored in a variable without displaying a message box to the user.
                //This allows the error to be deciphered so it can be determined in code if a message box is necessary.
                //This will be determined outside of this function.

                StructureTable.Errors = ex.Message;
                return StructureTable;
            }
            //db.Close()
            finally
            {
                db.Close();
            }
        }
        #endregion



        void System.IDisposable.Dispose()
        {
            db.Dispose();
            db = null;
            strConnectionString = null;
            strUserPassword = null;
            CommandBuilder = null;
            dt = null;
            da = null;

            GC.Collect();

        }
    }
}

