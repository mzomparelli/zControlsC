using zControlsC;
using zControlsC.Encryption;
using System;

namespace zControlsC
{
[Serializable()]
public sealed class zSchedulerReportFile
{
#region "Declarations"



	private string _FileLocation = "";
	private string _FileName = "";
	private string _FileExtension = ".xls";

	private string _Operation;
	private System.DateTime _DateTime;
	private string _Message;

	private string _ReportName = "";
	private CatalogType _Catalog = CatalogType.Starbucks;
	private string _CopyAsFileName = "";
	private bool _CopyFile = false;
	private bool _EmailReport = false;
	private bool _IncludeAttachment = false;
	private FileFormatEnum _FileFormat = FileFormatEnum.ExcelWithFormat;
	private string _IMR = "";
	private string _Prompt = "";
	private string _SaveAsFilename = "";
	private string _EmailBody = "";
	private string _EmailDL = "";
	private string _EmailSubject = "";
	private string _FromEmail = "";

    

	[NonSerialized()]
	private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public delegate void StatusChangedHandler(object sender, StatusChangedEventArgs e);
    public event StatusChangedHandler StatusChanged;
#endregion

#region "Properties and Structures"

	public enum FileFormatEnum
	{
		ExcelWithFormat,
		ExcelWithoutFormat,
		DataFile,
		DatabaseFile,
		CommaDelimitedFile,
		SQLFile,
		TextFile
	}

	public enum CatalogType
	{
		Bayer,
		BestBuy,
		CDW,
		Comcast,
		Dana,
		Diebold,
		Dow,
		Frigidaire_210,
		Fridgidaire_TMS,
		HP_210,
		HP_TMS,
		IBM_210,
		IBM_214,
		IBM_Gain_Share,
		IBM_Returns,
		IBM_TMS,
		Imation,
		LMS_Load_Planner_Workload,
		Maquet,
		Margin,
		Nike_Town,
		PhoenixBrands,
		PhoenixBrands_OpenBooking,
		Pilkington,
        LMSRates,
		Rates,
		Ricoh,
		RR_Donnelley,
		Sears_210,
		Sears_214,
		Sears_TMS,
		Sika,
		Slimfast,
		Stanley,
		Starbucks,
		Takata,
		Thomson,
		Tower,
		WhatIf,
        SQL_LMS,
        SQL_Rates,
        SQL_HHP,
        SQL_TMS,
        SQL_CDW_LoadBid
	}



	public string ReportName {
		get { return _ReportName; }
		set { _ReportName = value; }
	}

	public CatalogType Catalog {
		get { return _Catalog; }
		set { _Catalog = value; }
	}

	public string CopyAsFileName {
		get { return _CopyAsFileName; }
		set { _CopyAsFileName = value; }
	}

	public bool CopyFile {
		get { return _CopyFile; }
		set { _CopyFile = value; }
	}

	public bool EmailReport {
		get { return _EmailReport; }
		set { _EmailReport = value; }
	}

	public bool IncludeAttachment {
		get { return _IncludeAttachment; }
		set { _IncludeAttachment = value; }
	}

    public FileFormatEnum FileFormat
    {
		get { return _FileFormat; }
		set { _FileFormat = value; }
	}

	public string IMR {
		get { return _IMR; }
		set { _IMR = value; }
	}

	public string Prompt {
		get { return _Prompt; }
		set { _Prompt = value; }
	}

	public string SaveAsFilename {
		get { return _SaveAsFilename; }
		set { _SaveAsFilename = value; }
	}

	public string EmailBody {
		get { return _EmailBody; }
		set { _EmailBody = value; }
	}

	public string EmailDL {
		get { return _EmailDL; }
		set { _EmailDL = value; }
	}

	public string EmailSubject {
		get { return _EmailSubject; }
		set { _EmailSubject = value; }
	}

	public string FromEmail {
		get { return _FromEmail; }
		set { _FromEmail = value; }
	}

	public string zSchedulerFileDir {
		get { return _FileLocation; }
		set { _FileLocation = value; }
	}

	public string FileName {
		get { return _FileName; }
	}

	public string FileExtension {
		get { return _FileExtension; }
	}
#endregion

#region "Public Methods"

    //protected virtual void OnStatusChanged(StatusChangedEventArgs e)
    //{
    //    if (StatusChanged != null)
    //    {
    //        StatusChanged(this, e);
    //    }
    //}



    public void Reset()
    {
        _FileLocation = "";
        _FileName = "";
        _FileExtension = ".xls";
        _ReportName = "";
        _Catalog = CatalogType.Starbucks;
        _CopyAsFileName = "";
        _CopyFile = false;
        _EmailReport = false;
        _IncludeAttachment = false;
        _FileFormat = FileFormatEnum.ExcelWithFormat;
        _IMR = "";
        _Prompt = "";
        _SaveAsFilename = "";
        _EmailBody = "";
        _EmailDL = "";
        _EmailSubject = "";
        _FromEmail = "";
    }

	public zSchedulerReportFile()
	{
		timer.Interval = 3000;
        timer.Tick += new EventHandler(this.timer_Tick);
	}

	public void GetStatus()
	{
		if (System.IO.File.Exists(_FileLocation + "\\" + _FileName + ".status")) {
        Start:
            
			try {
				System.IO.StreamReader oRead;
				oRead = System.IO.File.OpenText(_FileLocation + "\\" + _FileName + ".status");

				string Operation;
				System.DateTime DateTime;
				string Message;

				if (oRead.Peek() == -1) return;
 
				Operation = zControlsC.Encryption.EncryptStrings.DecryptString(oRead.ReadLine());

				if (oRead.Peek() == -1) return;

                DateTime = DateTime.Parse(zControlsC.Encryption.EncryptStrings.DecryptString(oRead.ReadLine()));

                if (oRead.Peek() == -1) return;

                Message = zControlsC.Encryption.EncryptStrings.DecryptString(oRead.ReadLine());

				oRead.Close();
				oRead = null;
				Handler:

				if (Operation == "Object Disposed") {
                    timer.Stop();
					try {
						System.IO.File.Delete(_FileLocation + "\\" + _FileName + ".status");
					}
					catch (Exception) {
						goto Handler;
					}
				}

				_Operation = Operation;
				_DateTime = DateTime;
				_Message = Message;
				if (Operation == "Object Disposed") {
					if (StatusChanged != null) {
                        StatusChanged(this, new StatusChangedEventArgs(_Operation, _DateTime, _Message, true));
					}
				}
				else {
					if (StatusChanged != null) {
                        StatusChanged(this, new StatusChangedEventArgs(_Operation, _DateTime, _Message, false));
					}
				}
			}
			catch (Exception) {
				goto Start;
			}
			finally {

			}

		}
	}

    public void DeleteStatusFile()
    {

        try
        {

        }
        catch (Exception ex)
        {
            
           
        }
    }

	public void SendReportToQueue()
	{

		if (CheckProperties()) {

			_FileName = RandomFileName();

			System.IO.StreamWriter oWrite;
			oWrite = System.IO.File.CreateText(_FileLocation + "\\" + _FileName + ".srf");

            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_FileLocation));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_ReportName));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)CatalogLocation(_Catalog)));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_CopyAsFileName));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_CopyFile.ToString()));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_EmailReport.ToString()));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_IncludeAttachment.ToString()));

			string Format = "";
			switch (_FileFormat) {
				case FileFormatEnum.CommaDelimitedFile:
					Format = "csv";
					_FileExtension = ".csv";
                    break;
				case FileFormatEnum.DatabaseFile:
					Format = "dbf";
					_FileExtension = ".dbf";
                    break;
				case FileFormatEnum.DataFile:
					Format = "dat";
					_FileExtension = ".dat";
                    break;
				case FileFormatEnum.ExcelWithFormat:
					Format = "xlsf";
					_FileExtension = ".xls";
                    break;
				case FileFormatEnum.ExcelWithoutFormat:
					Format = "xls";
					_FileExtension = ".xls";
                    break;
				case FileFormatEnum.SQLFile:
					Format = "sql";
					_FileExtension = ".sql";
                    break;
				case FileFormatEnum.TextFile:
					Format = "txt";
					_FileExtension = ".txt";
                    break;
			}

            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)Format));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_IMR));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_Prompt));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)CatalogPassword(_Catalog)));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_SaveAsFilename));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_EmailBody));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_EmailDL));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_EmailSubject));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_FileName + _FileExtension));
            oWrite.WriteLine(zControlsC.Encryption.EncryptStrings.EncryptString((string)_FileName));

			oWrite.Close();
			oWrite = null;

			timer.Start();

		}

	}
#endregion

#region "Private Methods"

    private string RandomFileName()
    {
        //Random r = new Random();
        string d = zRandom.RandomAlphaNumericSequence(25, zRandom.RandomAlphaCharCase.Random);
        return d;
    }

	private string CatalogPassword(CatalogType c)
	{
		switch (c) {
			case CatalogType.Bayer:
				return "garden";
			case CatalogType.BestBuy:
				return "garden";
			case CatalogType.CDW:
				return "garden";
			case CatalogType.Comcast:
				return "garden";
			case CatalogType.Dana:
				return "garden";
			case CatalogType.Diebold:
				return "garden";
			case CatalogType.Dow:
				return "garden";
			case CatalogType.Fridgidaire_TMS:
				return "pluto";
			case CatalogType.Frigidaire_210:
				return "pluto";
			case CatalogType.HP_210:
				return "monalisa";
			case CatalogType.HP_TMS:
				return "monalisa";
			case CatalogType.IBM_210:
				return "coffee";
			case CatalogType.IBM_214:
				return "coffee";
			case CatalogType.IBM_Gain_Share:
				return "coffee";
			case CatalogType.IBM_Returns:
				return "coffee";
			case CatalogType.IBM_TMS:
				return "coffee";
			case CatalogType.Imation:
				return "eagle";
			case CatalogType.LMS_Load_Planner_Workload:
				return "garden";
			case CatalogType.Maquet:
				return "garden";
			case CatalogType.Margin:
				return "garden";
			case CatalogType.Nike_Town:
				return "garden";
			case CatalogType.PhoenixBrands:
				return "garden";
			case CatalogType.PhoenixBrands_OpenBooking:
				return "garden";
			case CatalogType.Pilkington:
				return "garden";
			case CatalogType.Rates:
				return "maggiesfarm";
            case CatalogType.LMSRates:
                return "maggiesfarm";
			case CatalogType.Ricoh:
				return "garden";
			case CatalogType.RR_Donnelley:
				return "garden";
			case CatalogType.Sears_210:
				return "sailor";
			case CatalogType.Sears_214:
				return "sailor";
			case CatalogType.Sears_TMS:
				return "sailor";
			case CatalogType.Sika:
				return "garden";
			case CatalogType.Slimfast:
				return "garden";
			case CatalogType.Stanley:
				return "tools";
			case CatalogType.Starbucks:
				return "garden";
			case CatalogType.Takata:
				return "garden";
			case CatalogType.Thomson:
				return "garden";
			case CatalogType.Tower:
				return "garden";
			case CatalogType.WhatIf:
				return "czarlite";
            case CatalogType.SQL_HHP:
                return "monalisa";
            case CatalogType.SQL_LMS:
                return "garden";
            case CatalogType.SQL_Rates:
                return "maggiesfarm";
            case CatalogType.SQL_TMS:
                return "garden";
            case CatalogType.SQL_CDW_LoadBid:
                return "garden";
            default:
                return "";
		}
	}

	private string CatalogLocation(CatalogType c)
	{
		switch (c) {
			case CatalogType.Bayer:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD BMTDCP - 2013-11-01.cat";
			case CatalogType.BestBuy:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\CDW PROD - 2013-11-01.cat";
			case CatalogType.CDW:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\CDW PROD - 2013-11-01.cat";
			case CatalogType.Comcast:
                return @"N:\Catalogs\LMS\LMS STANDARD COMDCP 2009_01_06.cat";
			case CatalogType.Dana:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD DANDCP - 2013-11-01.cat";
			case CatalogType.Diebold:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD DBTDCP - 2013-11-01.cat";
			case CatalogType.Dow:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD KDCDCP - 2013-11-01.cat";
			case CatalogType.Fridgidaire_TMS:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\TMS Frigidaire KFRDCP - 2013-11-01.cat";
			case CatalogType.Frigidaire_210:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\210 RECYCLE Files FRIGIDAIRE - 2013-11-01.CAT";
			case CatalogType.HP_210:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\210 RECYCLE Files Hewlett Packard - 2013-11-01.CAT";
			case CatalogType.HP_TMS:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\TMS Hewlett Packard HHPDCP ODS - 2013-11-01.CAT";
			case CatalogType.IBM_210:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\210 RECYCLE  Files IBMNAD - 2013-11-01.CAT";
			case CatalogType.IBM_214:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\IBM KXB 214 - 2014-05-27.cat";
			case CatalogType.IBM_Gain_Share:
                return @"N:\Catalogs\Data Marts\KXB GAIN SHARE.cat";
			case CatalogType.IBM_Returns:
                return @"N:\Catalogs\Data Marts\INVENTORY_ON_HAND.CAT";
			case CatalogType.IBM_TMS:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\TMS IBM KXBDCP - 2013-11-01.cat";
			case CatalogType.Imation:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\TMS Imation KIMDCP - 2013-11-01.cat";
			case CatalogType.LMS_Load_Planner_Workload:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS Load Planner Workload AUTDCP with TNT - 2013-11-01.cat";
			case CatalogType.Maquet:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD KMXDCP - 2013-11-01.cat";
			case CatalogType.Margin:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\DW Load Summary - 2013-11-01.cat";
			case CatalogType.Nike_Town:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD KNRDCP - 2013-11-01.cat";
			case CatalogType.PhoenixBrands:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD KPBDCP - 2013-11-01.cat";
			case CatalogType.PhoenixBrands_OpenBooking:
                return @"N:\Reports\IJP318603\Open Booking\Open_Booking_App\TEST KPBDCP 2009_08_06.cat";
			case CatalogType.Pilkington:
                return @"N:\Catalogs\LMS\LMS STANDARD PNADCP 2008_06_20.cat";
			case CatalogType.Rates:
                return @"N:\Catalogs\TMS\Rates V7.cat";
            case CatalogType.LMSRates:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS RATES - 2013-11-01.cat";
			case CatalogType.Ricoh:
                return @"N:\Catalogs\LMS\LMS STANDARD RTTDCP 2009_01_06.cat";
			case CatalogType.RR_Donnelley:
                return @"N:\Catalogs\LMS\LMS STANDARD DRRDCP 2009_01_06.cat";
			case CatalogType.Sears_210:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\210 RECYCLE Files SEARS - 2013-11-01 .CAT";
			case CatalogType.Sears_214:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\Sears KSL 214 - 2013-11-01.CAT";
			case CatalogType.Sears_TMS:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\TMS Sears KSLDCP - 0213-11-01.cat";
			case CatalogType.Sika:
                return @"N:\\Catalogs\\LMS\\LMS STANDARD KSADCP 2009_01_06.cat";
			case CatalogType.Slimfast:
                return @"N:\Catalogs\LMS\LMS STANDARD KSBDCP 2009_01_06.cat";
			case CatalogType.Stanley:
                return @"N:\Catalogs\TMS\DW Stanley V7.cat";
			case CatalogType.Starbucks:
                return @"N:\Catalogs\LMS\LMS STANDARD SBXDCP 2009_01_06.cat";
			case CatalogType.Takata:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD TAKDCP - 2013-11-01.cat";
			case CatalogType.Thomson:
                return @"N:\Catalogs\LMS\LMS STANDARD TMCDCP 2009_01_06.cat";
			case CatalogType.Tower:
                return @"N:\Catalogs\Impromptu 7.5 Catalogs\LMS STANDARD TWUDCP - 2013-11-01.cat";
			case CatalogType.WhatIf:
                return @"N:\Catalogs\Data Marts\WHAT_IF_RATING MLGAS400.CAT";
            case CatalogType.SQL_HHP:
                return @"N:\Catalogs\SQL Server\TMS SQL HHP ODS V7 UsrRpt 2014_10_12 Prod.cat";
            case CatalogType.SQL_LMS:
                return @"N:\Catalogs\SQL Server\LMS SQL STANDARD UsrRpt 2015_01_29 Prod load order.cat";
            case CatalogType.SQL_Rates:
                return @"N:\Catalogs\SQL Server\LMS SQL Rates Impromptu 7.5 - UsrRpt 2014_10_06 Prod.cat";
            case CatalogType.SQL_TMS:
                return @"N:\Catalogs\SQL Server\TMS SQL STANDARD UsrRpt 2014_10_12 Prod.cat";
            case CatalogType.SQL_CDW_LoadBid:
                return @"N:\Catalogs\SQL Server\CDW SQL STANDARD BASELINE UsrRpt 2014_12_04 Prod.cat";
            default:
                return "";
		}
	}

	private bool CheckProperties()
	{

		if (_FileLocation == "") {
			throw new Exception("File location must be specified.");
		}

		if (System.IO.File.Exists(_FileLocation + "\\" + _FileName)) {
			throw new Exception("File already exists. Please choose another filename");
		}

		if (_ReportName == "") {
			throw new Exception("Report name must be specified.");
		}

		if (_CopyFile == true) {
			if (_CopyAsFileName == "") {
				throw new Exception("Copy filename must be specified or CopyFile must be set to FALSE.");
			}
		}

		if (_IMR == "") {
			throw new Exception("IMR must be specified.");
		}

		if (_SaveAsFilename == "") {
			throw new Exception("Export location must be specified.");
		}

		if (_EmailReport) {
			if (_EmailBody == "") {
				throw new Exception("Email body must be specified or EmailReport must be set to FALSE.");
			}
			if (_EmailDL == "") {
				throw new Exception("Email DL must be specified or EmailReport must be set to FALSE.");
			}
			if (_EmailSubject == "") {
				throw new Exception("Email subject must be specified or EmailReport must be set to FALSE.");
			}
			if (_FromEmail == "") {
				throw new Exception("Email sender must be specified or EmailReport must be set to FALSE.");
			}
		}

		return true;


	}
#endregion


	private void timer_Tick(object sender, System.EventArgs e)
	{
		GetStatus();
	}
}

public class StatusChangedEventArgs : System.EventArgs
{
    protected string _Operation;
    protected DateTime _DateTime;
    protected string _Message;
    protected bool _IsCompleted;

    public string Operation
    {
        get { return _Operation; }
    }

    public DateTime DateTime
    {
        get { return _DateTime; }
    }

    public string Message
    {
        get { return _Message; }
    }

    public bool IsCompleted
    {
        get { return _IsCompleted; }
    }

    private StatusChangedEventArgs()
    {

    }

    public StatusChangedEventArgs(string operation, DateTime datetime, string message, bool iscompleted)
    {
        _Operation = operation;
        _DateTime = datetime;
        _Message = message;
        _IsCompleted = iscompleted;
    }
}
}
