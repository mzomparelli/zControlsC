using System.ComponentModel;
using System.Windows.Forms;
using System;
using OleDb = System.Data.OleDb;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace zControlsC.DataConnection
{
    [Serializable()]
    public class ACE_ExcelConn : IDisposable
    {
        #region "Declarations"
        private string zFilename = "zControls.dll version 1.0.0.2";
        private string zClass = "AccessDBConn";

        private string strConnectionString;
        [NonSerialized()]
        private OleDb.OleDbCommandBuilder CommandBuilder;
        private DataTable dt = new DataTable();
        [NonSerialized()]
        private OleDb.OleDbDataAdapter da;
        [NonSerialized()]
        private OleDb.OleDbConnection db = new OleDb.OleDbConnection();

        private string _HasHeader = "YES";
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

        public ACE_ExcelConn(string Database, bool HasHeaderRow)
        {
            if (HasHeaderRow == true)
            {
                _HasHeader = "YES";
            }
            else
            {
                _HasHeader = "NO";
            }

            strDatabase = Database;
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

        public List<string> GetTabNames()
        {
            try
            {
                db.ConnectionString = strConnectionString;
                db.Open();
                DataTable dbSchema = db.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string> tabs = new List<string>();
                foreach (DataRow dr in dbSchema.Rows)
                {
                    tabs.Add(dr["TABLE_NAME"].ToString());
                }
                db.Close();
                return tabs;
            }
            catch (Exception ex)
            {                
                return null;
            }
        }


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
                //Null Error
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

            System.IO.FileInfo fi = new System.IO.FileInfo(strDatabase);
            string ext = fi.Extension.ToLower();

            fi = null;


            switch (ext)
            {
                case ".xlsx":
                    strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase + ";Extended Properties='Excel 12.0 Xml;HDR=" + _HasHeader + "'";
                    break;
                case ".xlsb":
                    strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase + ";Extended Properties='Excel 12.0;HDR=" + _HasHeader + "'";
                    break;
                case ".xlsm":
                    strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase + ";Extended Properties='Excel 12.0 Macro;HDR=" + _HasHeader + "'";
                    break;
                case ".xls":
                    strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDatabase + ";Extended Properties='Excel 8.0;HDR=" + _HasHeader + "'";
                    break;
            }

        }

        public void ClearQueryResults()
        {
            if (!(this.QueryResults.Table == null))
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
            CommandBuilder = null;
            dt = null;
            da = null;

            GC.Collect();
        }
    }
}
