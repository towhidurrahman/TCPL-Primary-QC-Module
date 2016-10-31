using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using QC_Plan_Module.DAL;
using DataGrid = System.Windows.Forms.DataGrid;

namespace QC_Plan_Module.MODEL
{
    public partial class frmGENFinder : Form
    {
        DataTable dtRs=new DataTable();
        SQL sq = new SQL();       
        public string geNumber;  
      
        public frmGENFinder()
        {
            InitializeComponent();
        }
        private void frmGENFinder_Load(object sender, EventArgs e)
        {
            cmbFindBy.Text = "Gate Entry Number";

            cmbStartWith.Items.Add("Starts With");
            cmbStartWith.Items.Add("Contents");
            cmbStartWith.SelectedIndex = 0;

            chkAutoSearch.Checked = true;
            dtRs = sq.get_rs("select distinct GENNUMBER from T_GCGEMIRd where QCSTATUS = 1 order by gennumber desc");
            dataGridViewFinder.DataSource = dtRs;
            dataGridViewFinder.AllowUserToAddRows = false; 
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("..::Please Consult With Accpac Team::..");
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
           DataGridSelect();
        }
        public void DataGridSelect()
        {
            frmPrimaryQCModuleUI frmPrimaryQcModule= new frmPrimaryQCModuleUI();
        }
        private void comboFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //===================Code for Show All records=====================
            if (cmbFindBy.Text=="Show all Records")
            {
                cmbStartWith.Visible = false;
                lblFilter.Visible = false;
                txtFilter.Visible = false;
                dtRs = sq.get_rs("SELECT TRANSDATE,GENNUMBER,VEHICLENO FROM T_GCGEH ORDER BY TRANSDATE DESC");
                
                //==============DataGrid=======================================
                dataGridViewFinder.DataSource = dtRs;
                dataGridViewFinder.Refresh();

                //dataGridViewFinder.RefreshEdit();

                //DataGrid1.Refresh
                //Form_Resize
                //Set rs = Nothing
            }
            else
            {
                //==================Code for Other Value in Combo=================
                cmbFindBy.Visible = true;
                lblFilter.Visible = true;
                txtFilter.Visible = true;
            }
        }
         private void txtFilter_TextChanged(object sender, EventArgs e)
        {

        }
         private void dataGridViewFinder_CellContentClick(object sender, DataGridViewCellEventArgs e)
         {
             geNumber = this.dataGridViewFinder.Rows[dataGridViewFinder.CurrentCell.RowIndex].Cells[dataGridViewFinder.CurrentCell.ColumnIndex].Value.ToString();
             this.Close();
         }
    }
}

