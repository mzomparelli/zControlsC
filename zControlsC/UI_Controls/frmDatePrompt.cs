using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using zControlsC;


namespace zControlsC.UI_Controls
{
    public partial class frmDatePrompt : Form
    {
        public frmDatePrompt()
        {
            InitializeComponent();
        }

        string startDate;
        string endDate;
        bool userCancelled;
        bool userClicked;



        public string StartDate
        {
            get { return startDate; }
            
        }

        public string EndDate
        {
            get { return endDate; }
        }

        public bool UserCancelled
        {
            get { return userCancelled; }
        }

        public bool UserClicked
        {
            get { return userClicked; }
        }

        private void frmDatePrompt_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            userCancelled = true;
            userClicked = true;
            this.Close();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((zRegexEx.IsDate(dateStart.Value.ToString())) && (zRegexEx.IsDate(dateEnd.Value.ToString())))
            {
                startDate = dateStart.Value.Month.ToString() + "/" + dateStart.Value.Day.ToString() + "/" + dateStart.Value.Year.ToString() + " 12:00 AM";
                endDate = dateEnd.Value.Month.ToString() + "/" + dateEnd.Value.Day.ToString() + "/" + dateEnd.Value.Year.ToString() + " 11:59 PM";
                userClicked = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(@"You have entered an invalid date for one or both of the fields.");
            }
        }
    }
}
