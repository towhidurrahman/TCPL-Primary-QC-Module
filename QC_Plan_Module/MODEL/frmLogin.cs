using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Windows.Forms;

namespace QC_Plan_Module.MODEL
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmPrimaryQCModuleUI mainForm= new frmPrimaryQCModuleUI();
             AccpacCOMAPI.AccpacSession session = new AccpacCOMAPI.AccpacSession();
            // this.Cursor = Cursors.WaitCursor;
            session.Init("", "IC60A", "IC2000", "60A");
            try
            {
                session.Open(txtUserId.Text, txtPassword.Text, "TCPDAT", dtpSessionDate.Value, 0, "");
                AccpacCOMAPI.AccpacDBLink mDBLinkCmpRW = session.OpenDBLink(AccpacCOMAPI.tagDBLinkTypeEnum.DBLINK_COMPANY, AccpacCOMAPI.tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);
                AccpacCOMAPI.AccpacDBLink mDBLinkSysRW = session.OpenDBLink(AccpacCOMAPI.tagDBLinkTypeEnum.DBLINK_SYSTEM, AccpacCOMAPI.tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);
                
                mainForm.session = session;
                mainForm.mDBLinkCmpRW = mDBLinkCmpRW;
                mainForm.mDBLinkSysRW = mDBLinkSysRW;

                this.Hide();
                mainForm.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                txtPassword.Focus();
            }
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                btnLogin.PerformClick();
            }
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
    }
}
