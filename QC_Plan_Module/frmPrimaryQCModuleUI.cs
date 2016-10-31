using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using QC_Plan_Module.DAL;
using QC_Plan_Module.MODEL;

namespace QC_Plan_Module
{
    public partial class frmPrimaryQCModuleUI : Form
    {
        SQL sq = new SQL();
        DataTable dtMain = new DataTable();
        DataTable dtDetails = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtInsert = new DataTable();

        public int totalRowCnt;
        public int i;
        public int rowIndex;
        public int slNo;
        public Boolean oldRecord;
        public Boolean enableEdit;

        public AccpacCOMAPI.AccpacSession session;
        public AccpacCOMAPI.AccpacDBLink mDBLinkCmpRW;
        public AccpacCOMAPI.AccpacDBLink mDBLinkSysRW;
        public frmPrimaryQCModuleUI()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmPrimaryQCModuleUI_Load(object sender, EventArgs e)
        {
            //Hello QC this is from tuhin
            enableEdit = false;
            rowIndex = 0;
            btnFirst.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipY);
            btnNext.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipY);

            cmbDoorSeal.Items.Add("");
            cmbDoorSeal.Items.Add("Yes");
            cmbDoorSeal.Items.Add("BROKEN");
            cmbDoorSeal.Items.Add("MISSING");

            cmbInsideOfCareer.Items.Add("");
            cmbInsideOfCareer.Items.Add("OK");
            cmbInsideOfCareer.Items.Add("WET");
            cmbInsideOfCareer.Items.Add("OILY");
            cmbInsideOfCareer.Items.Add("SOUR ODOUR");
            cmbInsideOfCareer.Items.Add("OFF ODOUR");
            cmbInsideOfCareer.Items.Add("SPILLED");
            cmbInsideOfCareer.Items.Add("POWDER");
            cmbInsideOfCareer.Items.Add("OTHER");

            cmbOutsideOfCareer.Items.Add("");
            cmbOutsideOfCareer.Items.Add("OK");
            cmbOutsideOfCareer.Items.Add("CONTAMINATED");
            cmbOutsideOfCareer.Items.Add("DAMAGED");
            cmbOutsideOfCareer.Items.Add("OTHER");

            cmbFlashUVLight.Items.Add("");
            cmbFlashUVLight.Items.Add("OK");
            cmbFlashUVLight.Items.Add("DAMAGED");
            cmbFlashUVLight.Items.Add("TRASH");
            cmbFlashUVLight.Items.Add("WET");
            cmbFlashUVLight.Items.Add("OILY");
            cmbFlashUVLight.Items.Add("SPILLED");

            cmbNonFoodItem.Items.Add("");
            cmbNonFoodItem.Items.Add("No");
            cmbNonFoodItem.Items.Add("Yes");

            cmbQCResult.Items.Add("");
            cmbQCResult.Items.Add("Accepted");    // font
            cmbQCResult.Items.Add("Rejected");

            cmbQCSample.Items.Add("");
            cmbQCSample.Items.Add("No");
            cmbQCSample.Items.Add("Yes");

            cmbCleanUnloaded.Items.Add("");
            cmbCleanUnloaded.Items.Add("No");
            cmbCleanUnloaded.Items.Add("Yes");

            cmbContamination.Items.Add("");
            cmbContamination.Items.Add("No");
            cmbContamination.Items.Add("Yes");

            cmbDamagedMaterial.Items.Add("");
            cmbDamagedMaterial.Items.Add("No");
            cmbDamagedMaterial.Items.Add("Yes");

            string[] column = new string[] { "Sl No", "Item No", "Description", "UOM", "Quantity", "Batch No.", "Expiry Date", "Comments" };
            int col = 8;
            dataGridViewShow.ColumnCount = 8;
            for (int i = 0; i < col; i++)
            {
                dataGridViewShow.Columns[i].Name = column[i];
            }
            dataGridViewShow.Columns["Sl No"].Width = 40;
            dataGridViewShow.Columns["Item No"].Width = 78;
            dataGridViewShow.Columns["Description"].Width = 200;
            //dataGridViewShow.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewShow.Columns["UOM"].Width = 35;
            dataGridViewShow.Columns["Quantity"].Width = 60;
            dataGridViewShow.Columns["Batch No."].Width = 100;
            dataGridViewShow.Columns["Expiry Date"].Width = 100;
            dataGridViewShow.Columns["Comments"].Width = 180;

            //dataGridViewShow.AllowUserToAddRows = false;
            //GridColumn();
            
            dtMain = sq.get_rs("Select *  from T_QCPRH order by PQCNUMBER");

            if (dtMain.Rows.Count < 1)
            {
                goto exit;
            }
            //FillGEData();          
         
            fillData();
            btnNew.PerformClick();
            rowIndex = -1;           //Revise for next item
            exit:
            ;
        }
        public void fillData()
        {
            oldRecord = true;

            txtQCNumber.Text = dtMain.Rows[rowIndex]["PQCNUMBER"].ToString();
            txtInspectorName.Text = dtMain.Rows[rowIndex]["INSPNAME"].ToString();
            dtpInspectionDate.Value = new DateTime(Convert.ToInt16(dtMain.Rows[rowIndex]["inspDate"].ToString().Substring(0, 4)), Convert.ToInt16(dtMain.Rows[rowIndex]["inspDate"].ToString().Substring(4, 2)), Convert.ToInt16(dtMain.Rows[rowIndex]["inspDate"].ToString().Substring(6, 2)));
            
            txtGE.Text = dtMain.Rows[rowIndex]["GENUMBER"].ToString();
            dtpGENDate.Value = new DateTime(Convert.ToInt16(dtMain.Rows[rowIndex]["geDate"].ToString().Substring(0, 4)), Convert.ToInt16(dtMain.Rows[rowIndex]["geDate"].ToString().Substring(4, 2)), Convert.ToInt16(dtMain.Rows[rowIndex]["geDate"].ToString().Substring(6, 2)));
            
            txtVendorQCRef.Text = dtMain.Rows[rowIndex]["VENDQCREF"].ToString();
            txtContainerSample.Text = dtMain.Rows[rowIndex]["CONTSAMPL"].ToString();
            txtChallanNo.Text = dtMain.Rows[rowIndex]["CHALLANNO"].ToString();
            dtpChallanDate.Value = new DateTime(Convert.ToInt16(dtMain.Rows[rowIndex]["challanDate"].ToString().Substring(0, 4)), Convert.ToInt16(dtMain.Rows[rowIndex]["challanDate"].ToString().Substring(4, 2)), Convert.ToInt16(dtMain.Rows[rowIndex]["challanDate"].ToString().Substring(6, 2)));
            
            txtVendorNo.Text = dtMain.Rows[rowIndex]["VENDORID"].ToString();
            txtVendorName.Text = dtMain.Rows[rowIndex]["VENDORNAME"].ToString();
            txtUnloadedBy.Text = dtMain.Rows[rowIndex]["UNLOADBY"].ToString();

            //Dim i As Integer

            //oldRecord = True

            if (dtMain.Rows[rowIndex]["QCRESULT"].ToString() == "Accepted")
            {
                cmbQCResult.SelectedIndex = 1;
            }
            else if (dtMain.Rows[rowIndex]["QCRESULT"].ToString() == "Rejected")
            {
                cmbQCResult.SelectedIndex = 2;
            }
            txtQcResult.Text = dtMain.Rows[rowIndex]["QCRESULTCOMMENT"].ToString();

            dtDetails = sq.get_rs("Select * from T_QCPRD where PQCNUMBER = '" + txtQCNumber.Text + "'");



            if (dtDetails.Rows[0]["DOORSEAL"].ToString()=="YES")
            {
                cmbDoorSeal.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["DOORSEAL"].ToString()=="BROKEN")
            {
                 cmbDoorSeal.SelectedIndex = 2;
            }
            else if (dtDetails.Rows[0]["DOORSEAL"].ToString()=="MISSING")
            {
                 cmbDoorSeal.SelectedIndex = 3;
            }
            txtDoorSeal.Text = dtDetails.Rows[0]["DOORSEALCOMMENT"].ToString();



            if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="OK")
            {
                cmbInsideOfCareer.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="WET")
            {
                cmbInsideOfCareer.SelectedIndex = 2;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="OILY")
            {
                cmbInsideOfCareer.SelectedIndex = 3;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="SOUR ODOUR")
            {
                cmbInsideOfCareer.SelectedIndex = 4;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="OFF ODOUR")
            {
                cmbInsideOfCareer.SelectedIndex = 5;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="SPILLED")
            {
                cmbInsideOfCareer.SelectedIndex = 6;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="POWDER")
            {
                cmbInsideOfCareer.SelectedIndex = 7;
            }
              else if (dtDetails.Rows[0]["INSIDECAREER"].ToString()=="OTHER")
            {
                cmbInsideOfCareer.SelectedIndex = 8;
            }           
            txtInsideCareer.Text = dtDetails.Rows[0]["INSIDECAREERCOMMENT"].ToString();



            if (dtDetails.Rows[0]["OUTSIDECAREER"].ToString() == "OK")
            {
                cmbOutsideOfCareer.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["OUTSIDECAREER"].ToString() == "CONTAMINATED")
            {
                cmbOutsideOfCareer.SelectedIndex = 2;
            }
            else if (dtDetails.Rows[0]["OUTSIDECAREER"].ToString() == "DAMAGED")
            {
                cmbOutsideOfCareer.SelectedIndex = 3;
            }
            else if (dtDetails.Rows[0]["OUTSIDECAREER"].ToString() == "OTHER")
            {
                cmbOutsideOfCareer.SelectedIndex = 4;
            }
            txtOutsideCareer.Text = dtDetails.Rows[0]["OUTSIDECAREERCOMMENT"].ToString();



            if (dtDetails.Rows[0]["UVFLASH"].ToString() == "OK")
            {
                cmbFlashUVLight.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["UVFLASH"].ToString() == "DAMAGED")
            {
                cmbFlashUVLight.SelectedIndex = 2;
            }
            else if (dtDetails.Rows[0]["UVFLASH"].ToString() == "TRASH")
            {
                cmbFlashUVLight.SelectedIndex = 3;
            }
            else if (dtDetails.Rows[0]["UVFLASH"].ToString() == "WET")
            {
                cmbFlashUVLight.SelectedIndex = 4;
            }
            else if (dtDetails.Rows[0]["UVFLASH"].ToString() == "OILY")
            {
                cmbFlashUVLight.SelectedIndex = 5;
            }
            else if (dtDetails.Rows[0]["UVFLASH"].ToString() == "SPILLED")
            {
                cmbFlashUVLight.SelectedIndex = 6;
            }
            txtUVFlash.Text = dtDetails.Rows[0]["UVFLASHCOMMENT"].ToString();



            if (dtDetails.Rows[0]["NONFOOD"].ToString() == "OK")
            {
                cmbNonFoodItem.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["NONFOOD"].ToString() == "Yes")
            {
                cmbNonFoodItem.SelectedIndex = 2;
            }
            txtNonFood.Text = dtDetails.Rows[0]["NONFOODCOMMENT"].ToString();


            if (dtDetails.Rows[0]["NONFOOD"].ToString() == "No")
            {
                cmbNonFoodItem.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["NONFOOD"].ToString() == "Yes")
            {
                cmbNonFoodItem.SelectedIndex = 2;
            }
            txtNonFood.Text = dtDetails.Rows[0]["NONFOODCOMMENT"].ToString();
         

            if (dtDetails.Rows[0]["QASAMPLE"].ToString() == "No")
            {
                cmbQCSample.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["QASAMPLE"].ToString() == "Yes")
            {
                cmbQCSample.SelectedIndex = 2;
            }
            txtQASample.Text = dtDetails.Rows[0]["QASAMPLECOMMENT"].ToString();
            

            if (dtDetails.Rows[0]["CLEANUNLOAD"].ToString() == "No")
            {
                cmbCleanUnloaded.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["CLEANUNLOAD"].ToString() == "Yes")
            {
                cmbCleanUnloaded.SelectedIndex = 2;
            }
            txtCleanUnloading.Text = dtDetails.Rows[0]["CLEANUNLOADCOMMENT"].ToString();

            
            if (dtDetails.Rows[0]["CONTAMINATION"].ToString() == "No")
            {
                cmbContamination.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["CONTAMINATION"].ToString() == "Yes")
            {
                cmbContamination.SelectedIndex = 2;
            }
            txtContamination.Text = dtDetails.Rows[0]["CONTAMINATIONCOMMENT"].ToString();
            
              
            if (dtDetails.Rows[0]["DAMAGEMATERIAL"].ToString() == "No")
            {
                cmbDamagedMaterial.SelectedIndex = 1;
            }
            else if (dtDetails.Rows[0]["DAMAGEMATERIAL"].ToString() == "Yes")
            {
                cmbDamagedMaterial.SelectedIndex = 2;
            }
            txtDamageMaterial.Text = dtDetails.Rows[0]["DAMAGEMATERIALCOMMENT"].ToString();


            dtTemp = sq.get_rs("select * from T_GCGEMIRd where GENNUMBER = '" + txtGE.Text + "'");

            //totalRowCnt = dataGridViewShow.RowCount - 1; ////////////////
            slNo=1;          
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Refresh();

            foreach (DataRow r in dtTemp.Rows)
            {              
                //this.dataGridViewShow.Rows.Add(slNo);
                //dataGridViewShow.Rows[i].Cells[j + 1].Value = dtTemp.Rows[0]["ITEMID"];
                //dataGridViewShow.Rows[i].Cells[j + 2].Value = dtTemp.Rows[0]["ITEMNAME"];
                //dataGridViewShow.Rows[i].Cells[j + 3].Value = dtTemp.Rows[0]["UOM"];
                //dataGridViewShow.Rows[i].Cells[j + 4].Value = dtTemp.Rows[0]["QTY"];
                //dataGridViewShow.Rows[i].Cells[j + 5].Value = dtTemp.Rows[0]["BATCHNO"];
                //dataGridViewShow.Rows[i].Cells[j + 6].Value = dtTemp.Rows[0]["expDate"];
                //dataGridViewShow.Rows[i].Cells[j + 7].Value = dtTemp.Rows[0]["REMARKS"];
                //slNo++;

                this.dataGridViewShow.Rows.Add(slNo, r["ITEMID"].ToString(), r["ITEMNAME"].ToString(), r["UOM"].ToString(), r["QTY"].ToString(), r["BATCHNO"].ToString(), r["expDate"].ToString(), r["REMARKS"].ToString());
                slNo++;
             }                          
            dataGridViewShow.AllowUserToAddRows = false;   
            ButtonStatus();
        }
        public void FillGEData()
        {
            //Dim i As Integer
            
            dtTemp = sq.get_rs("Select *, TRANSDATE as FMTDATE from T_GCGEH where GENNUMBER = '" + txtGE.Text + "'");

            dtpGENDate.Value = new DateTime(Convert.ToInt16(dtTemp.Rows[0]["FMTDATE"].ToString().Substring(0, 4)), Convert.ToInt16(dtTemp.Rows[0]["FMTDATE"].ToString().Substring(4, 2)), Convert.ToInt16(dtTemp.Rows[0]["FMTDATE"].ToString().Substring(6, 2)));

            dtTemp = sq.get_rs("select *, CHALLANDATE as FMTCHALLANDATE from T_GCGEMIRH WHERE GENNUMBER = '" + txtGE.Text + "'");
            txtChallanNo.Text = dtTemp.Rows[0]["CHALLANNUM"].ToString();

            dtpChallanDate.Value = new DateTime(Convert.ToInt16(dtTemp.Rows[0]["FMTCHALLANDATE"].ToString().Substring(0, 4)), Convert.ToInt16(dtTemp.Rows[0]["FMTCHALLANDATE"].ToString().Substring(4, 2)), Convert.ToInt16(dtTemp.Rows[0]["FMTCHALLANDATE"].ToString().Substring(6, 2)));
            
            txtVendorQCRef.Text = "";
            txtContainerSample.Text = "";

            txtVendorNo.Text = dtTemp.Rows[0]["VENDORID"].ToString();
            txtVendorName.Text = dtTemp.Rows[0]["VENDORNAME"].ToString();
            
            cmbDoorSeal.SelectedIndex = 0;
            txtDoorSeal.Text = "";
            cmbInsideOfCareer.SelectedIndex = 0;
            txtInsideCareer.Text = "";
            cmbOutsideOfCareer.SelectedIndex = 0;

            txtOutsideCareer.Text = "";
            cmbFlashUVLight.SelectedIndex = 0;

            txtUVFlash.Text = "";
            cmbNonFoodItem.SelectedIndex = 0;

            txtNonFood.Text = "";
            cmbQCResult.SelectedIndex = 0;


            txtQcResult.Text = "";
            cmbQCSample.SelectedIndex = 0;

            txtQASample.Text = "";
            cmbCleanUnloaded.SelectedIndex = 0;

            txtCleanUnloading.Text = "";
            cmbContamination.SelectedIndex = 0;


            txtContamination.Text = "";
            cmbDamagedMaterial.SelectedIndex = 0;

            txtDamageMaterial.Text = "";
            txtUnloadedBy.Text = "";

            dtTemp = sq.get_rs("select * from T_GCGEMIRd where GENNUMBER = '" + txtGE.Text + "'");
    
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Refresh();

            slNo = 1;
            foreach (DataRow r in dtTemp.Rows)
            {
                this.dataGridViewShow.Rows.Add(slNo, r["ITEMID"].ToString(), r["ITEMNAME"].ToString(), r["UOM"].ToString(), r["QTY"].ToString(), r["BATCHNO"].ToString(), r["expDate"].ToString(), r["REMARKS"].ToString());
                slNo++;
            }
            dataGridViewShow.AllowUserToAddRows = false;     
            
        }
        private void btnNext_Click(object sender, EventArgs e)

        {        
            if (rowIndex < dtMain.Rows.Count - 1)
            {
                rowIndex = rowIndex + 1;
           
            }
            fillData();            
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            rowIndex = dtMain.Rows.Count-1;
            fillData();
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {          
            rowIndex = 0;
            fillData();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (rowIndex > 0)
            {
                rowIndex = rowIndex - 1;
            }
            fillData();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            //fillData();
            ClearData();
            txtQCNumber.Text = "**NEW**";
            dtpInspectionDate.Value =DateTime.Today;
            oldRecord = false;
            enableEdit = false;
            ButtonStatus();
        }
        public void ClearData()
        {
            dataGridViewShow.Rows.Clear();
           
            dataGridViewShow.Refresh();

            txtInspectorName.Text = "";
            dtpInspectionDate.Value = DateTime.Today;

            txtGE.Text = "";
            txtChallanNo.Text = "";
            dtpChallanDate.Value = DateTime.Today;

            txtVendorQCRef.Text = "";
            txtContainerSample.Text = "";
            txtVendorNo.Text = "";
            txtVendorName.Text = "";
            cmbDoorSeal.SelectedIndex = 0;
            txtDoorSeal.Text = "";
            cmbInsideOfCareer.SelectedIndex = 0;
            txtInsideCareer.Text = "";
            cmbOutsideOfCareer.SelectedIndex = 0;
            txtOutsideCareer.Text = "";
            cmbFlashUVLight.SelectedIndex = 0;
            txtUVFlash.Text = "";

            cmbNonFoodItem.SelectedIndex = 0;
            txtNonFood.Text = "";

            cmbQCResult.SelectedIndex = 0;
            txtQcResult.Text = "";
            cmbQCSample.SelectedIndex = 0;
            txtQASample.Text = "";

            cmbCleanUnloaded.SelectedIndex = 0;

            txtCleanUnloading.Text = "";
            cmbContamination.SelectedIndex = 0;

            txtContamination.Text = "";
            cmbDamagedMaterial.SelectedIndex = 0;

            txtDamageMaterial.Text = "";
            txtUnloadedBy.Text = "";
        }
        private void btnFinder_Click(object sender, EventArgs e)
        {

        }
        private void btnGENOFinder_Click(object sender, EventArgs e)
        {
            frmGENFinder frmGenfinder = new frmGENFinder();
            frmGenfinder.ShowDialog();
            if (frmGenfinder.geNumber != null)
            {
                txtGE.Text = frmGenfinder.geNumber;
                FillGEData();
            }           
        }
        public void ButtonStatus()
        {
            if (oldRecord == true && enableEdit == false && Convert.ToInt32(dtMain.Rows[0]["POSTSTATUS"].ToString()) == 0)
            {

                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnPost.Enabled = true;
                btnGENOFinder.Enabled = false;

            }
            if(oldRecord ==true && enableEdit == true && Convert.ToInt32(dtMain.Rows[0]["POSTSTATUS"].ToString()) == 0)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
                btnGENOFinder.Enabled = true;
            }

            if (Convert.ToInt32(dtMain.Rows[0]["POSTSTATUS"].ToString()) == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
                btnGENOFinder.Enabled = false;
            }

           if(oldRecord == false)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
                btnGENOFinder.Enabled = true;
           }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            enableEdit = true;
            ButtonStatus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            string strSQl;
            string strDOCNumber="";
            string BomNo;

            Boolean modifiable;
            Boolean newRecord;
            Double standardCost;
            string productionCategory;
            string audtDate;
            string audtTime;
            string rcpDate, rcpTime, inspDate, geDate, challanDate;

            if (txtInspectorName.Text=="" || txtGE.Text =="" ||cmbDoorSeal.Text == ""|| cmbInsideOfCareer.Text=="" || cmbOutsideOfCareer.Text==""|| cmbFlashUVLight.Text==""|| cmbNonFoodItem.Text=="" ||cmbQCResult.Text==""||cmbQCSample.Text==""||cmbCleanUnloaded.Text==""||cmbContamination.Text== ""||cmbDamagedMaterial.Text==""||txtUnloadedBy.Text== "")
            {
                MessageBox.Show("Enter all Data....");
            }
           
            dtpNow.Value = DateTime.Now;

            audtDate=dtpNow.Value.ToString("yyyyMMdd");
            inspDate = dtpInspectionDate.Value.ToString("yyyyMMdd");
            challanDate = dtpChallanDate.Value.ToString("yyyyMMdd");
            geDate=dtpGENDate.Value.ToString("yyyyMMdd");
            audtTime = dtpNow.Value.ToString("HHmm");
           
            DataTable dtRs= new DataTable();
                    
            if (oldRecord==false)
            {           
               dtRs = sq.get_rs("Select MAX(PQCNUMBER) as DOCNUMBER from T_QCPRH");

                if (string.IsNullOrEmpty(dtRs.Rows[0]["Docnumber"].ToString()))
                {
                    strDOCNumber = "PQI-00000001";  /////////////////See later///////////
                }
    
                else
                {
                    strDOCNumber = dtRs.Rows[0]["Docnumber"].ToString().Substring(4,8);
                }               
                string docSerial = strDOCNumber + 1;
  
                strDOCNumber = "";

                int DocLenth = docSerial.Length;

                for (i = 1; i <= 8 - DocLenth; i++)
                {
                    strDOCNumber = strDOCNumber + "0";
                }
                strDOCNumber = "PQI-" + strDOCNumber + docSerial.Trim();

                //MessageBox.Show(strDOCNumber);

                strSQl = " Insert Into T_QCPRH (PQCNUMBER, INSPNAME, INSPDATE, " +
                         " GENUMBER, GEDATE, VENDQCREF, " +
                         " CONTSAMPL, VENDORID, VENDORNAME, " +
                         " POSTSTATUS, AUDTDATE, AUDTTIME, " +
                         " POSTDATE, POSTTIME, AUDTUSER, " +
                         " CHALLANNO, CHALLANDATE, QCRESULTCOMMENT, " +
                         " POSTUSER, QCRESULT, UNLOADBY) " +
                         " Values ('" + strDOCNumber + "', '" + txtInspectorName.Text.Trim() + "', '" + inspDate + "', " +
                         " '" + txtGE.Text.Trim() + "', '" + geDate + "', '" + txtVendorQCRef.Text.Trim() + "', " +
                         " '" + txtContainerSample.Text.Trim() + "', '" + txtVendorNo.Text.Trim() + "', '" +
                         txtVendorName.Text.Trim() + "', " +
                         " '0', '" + audtDate + "', '" + audtTime + "', " +
                         " '0', '0', '" + "Abul" + "', " +
                         " '" + txtChallanNo.Text + "', '" + challanDate + "', '" + txtQcResult.Text + "', " +
                         " '', '" + cmbQCResult.Text + "', '" + txtUnloadedBy.Text.Trim() + "')";

                 dtInsert = sq.get_rs(strSQl);

                 strSQl = "Insert Into T_QCPRD (PQCNUMBER, DOORSEAL, DOORSEALCOMMENT, " +
                         " INSIDECAREER, INSIDECAREERCOMMENT, OUTSIDECAREER, " +
                         " OUTSIDECAREERCOMMENT, UVFLASH, UVFLASHCOMMENT, " +
                         " NONFOOD, NONFOODCOMMENT, QASAMPLE, " +
                         " QASAMPLECOMMENT, CLEANUNLOAD, CLEANUNLOADCOMMENT, " +
                         " CONTAMINATION, CONTAMINATIONCOMMENT, DAMAGEMATERIAL, " +
                         " DAMAGEMATERIALCOMMENT) " +
                         " Values ('" + strDOCNumber + "', '" + cmbDoorSeal.Text + "', '" + txtDoorSeal.Text.Trim() +
                         "', " +
                         " '" + cmbInsideOfCareer.Text + "', '" + txtInsideCareer.Text + "', '" + cmbOutsideOfCareer.Text +
                         "', " +
                         " '" + txtOutsideCareer.Text + "', '" + cmbFlashUVLight.Text + "', '" + txtUVFlash.Text + "', " +
                         " '" + cmbNonFoodItem.Text + "', '" + txtNonFood.Text + "', '" + cmbQCSample.Text + "', " +
                         " '" + txtQASample.Text + "', '" + cmbCleanUnloaded.Text + "', '" + txtCleanUnloading.Text +
                         "', " +
                         " '" + cmbContamination.Text + "', '" + txtContamination.Text + "', '" + cmbDamagedMaterial.Text +
                         "', " +
                         " '" + txtDamageMaterial.Text + "')";

                   dtInsert = sq.get_rs(strSQl);

                   strSQl = "UPDATE T_GCGEMIRd SET QCSTATUS = 2 WHERE GENNUMBER = '" + txtGE.Text + "'";

                   dtInsert = sq.get_rs(strSQl);
                }

            if (oldRecord == true)
            {
                strDOCNumber = txtQCNumber.Text;

                strSQl = "Delete from T_QCPRH WHERE PQCNUMBER = '" + strDOCNumber + "'";
               
                 dtInsert = sq.get_rs(strSQl);

                strSQl = "Delete from T_QCPRD WHERE PQCNUMBER = '" + strDOCNumber + "'";
                 
                dtInsert = sq.get_rs(strSQl);

                strSQl = " Insert Into T_QCPRH (PQCNUMBER, INSPNAME, INSPDATE, " +
                         " GENUMBER, GEDATE, VENDQCREF, " +
                         " CONTSAMPL, VENDORID, VENDORNAME, " +
                         " POSTSTATUS, AUDTDATE, AUDTTIME, " +
                         " POSTDATE, POSTTIME, AUDTUSER, " +
                         " CHALLANNO, CHALLANDATE, QCRESULTCOMMENT, " +
                         " POSTUSER, QCRESULT, UNLOADBY) " +
                         " Values ('" + strDOCNumber + "', '" + txtInspectorName.Text.Trim() + "', '" + inspDate + "', " +
                         " '" + txtGE.Text.Trim() + "', '" + geDate + "', '" + txtVendorQCRef.Text.Trim() + "', " +

                         " '" + txtContainerSample.Text.Trim() + "', '" + txtVendorNo.Text.Trim() + "', '" +
                         txtVendorName.Text.Trim() + "', " +
                         " '0', '" + audtDate + "', '" + audtTime + "', " +
                         " '0', '0', '" + "Babul" + "', " +
                         " '" + txtChallanNo.Text + "', '" + challanDate + "', '" + txtQcResult.Text + "', " +
                         " '', '" + cmbQCResult.Text.Trim() + "', '" + txtUnloadedBy.Text.Trim() + "')";

                dtInsert = sq.get_rs(strSQl);
          
                strSQl = " Insert Into T_QCPRD (PQCNUMBER, DOORSEAL, DOORSEALCOMMENT, "+
                         " INSIDECAREER, INSIDECAREERCOMMENT, OUTSIDECAREER, "+
                         " OUTSIDECAREERCOMMENT, UVFLASH, UVFLASHCOMMENT, "+
                         " NONFOOD, NONFOODCOMMENT, QASAMPLE, "+
                
                         " QASAMPLECOMMENT, CLEANUNLOAD, CLEANUNLOADCOMMENT, "+
                
                         " CONTAMINATION, CONTAMINATIONCOMMENT, DAMAGEMATERIAL, "+
                
                         " DAMAGEMATERIALCOMMENT) "+
                
                         " Values ('" + strDOCNumber + "', '" + cmbDoorSeal.Text + "', '" + txtDoorSeal.Text.Trim() + "', "+
                
                         " '" + cmbInsideOfCareer.Text + "', '" + txtInsideCareer.Text + "', '" + cmbOutsideOfCareer.Text + "', "+
                
                         " '" + txtOutsideCareer.Text + "', '" + cmbFlashUVLight.Text + "', '" + txtUVFlash.Text + "', "+
                
                         " '" + cmbNonFoodItem.Text + "', '" + txtNonFood.Text + "', '" + cmbQCSample.Text + "', "+
                
                         " '" + txtQASample.Text + "', '" + cmbCleanUnloaded.Text + "', '" + txtCleanUnloading.Text + "', "+
                
                
                        " '" + cmbContamination.Text + "', '" + txtContamination.Text + "', '" + cmbDamagedMaterial.Text + "', "+
                
                         " '" + txtDamageMaterial.Text + "')";
                dtInsert = sq.get_rs(strSQl);

                strSQl = "UPDATE T_GCGEMIRd SET QCSTATUS = 2 WHERE GENNUMBER = '" + txtGE.Text + "'";
                dtInsert = sq.get_rs(strSQl);
            }

            txtQCNumber.Text = strDOCNumber;

            dtMain = sq.get_rs("Select *  from T_QCPRH order by PQCNUMBER");

            if (dtMain.Rows.Count<1)
            {
                goto End;

            }
            DataRow[] foundRows = dtMain.Select("PQCNUMBER = '" + txtQCNumber.Text.Trim() + "'");
            rowIndex = dtMain.Rows.IndexOf(foundRows[0]);
            enableEdit = false;
            fillData();
            MessageBox.Show("Records to Save Successfully....");
            End:
            ;

            //else
            //{
            //    MessageBox.Show("cannot save record!!!");
            //}
        }
        private void btnPost_Click(object sender, EventArgs e)
        {
            string strSQL;
            string strDOCNumber;
            string BomNo;

            Boolean modifiable;
            Boolean newRecord;
            double standardCost;

            string productCategory;
            string audtDate;
            string audtTime;

            dtpNow.Value = DateTime.Now;
            audtDate=dtpNow.Value.ToString("yyyyMMdd");
            audtTime = dtpNow.Value.ToString("HHmm");

            if (oldRecord==true)
            {
                if (Convert.ToInt32(dtMain.Rows[0]["POSTSTATUS"])==0)
                {
                   //**************QC posted***************
                    strSQL = "Update T_QCPRH set POSTDATE = '" + audtDate + "', POSTTIME = '" + audtTime +
                             "', POSTUSER = 'Hanif', POSTSTATUS = 1  WHERE PQCNUMBER= '" + txtQCNumber.Text + "'";

                    dtInsert = sq.get_rs(strSQL);

                    //***************PRIMARY QC COMPLETE************
                    strSQL = "UPDATE T_GCGEMIRD SET QCSTATUS = 3 WHERE GENNUMBER = '" + txtGE.Text + "'";
                    dtInsert = sq.get_rs(strSQL);

                    dtMain = sq.get_rs("Select *  from T_QCPRH order by PQCNUMBER");

                    if (dtMain.Rows.Count < 1)
                    {
                        goto Exit;
                    }                    
                        //DataRow[] foundRows = dtMain.Select("PQCNUMBER='" + txtQCNumber.Text.Trim() + "'");
                        DataRow[] foundRows = dtMain.Select("PQCNUMBER = '" + txtQCNumber.Text.Trim() + "'");
                        rowIndex = dtMain.Rows.IndexOf(foundRows[0]);

                        enableEdit = false;
                        fillData();
                        MessageBox.Show("Data Post Successful....");
                    //else
                    //{
                    //    MessageBox.Show("Cannot Post Record!!....");
                    //}
                    Exit:
                    ;

                }
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrimaryQC();
        }
        public void PrimaryQC()
        {
            string productionDate = dtpNow.Value.ToString("yyyyMMdd");

            frmRPTViewer rptViewer = new frmRPTViewer();
            DataSet dsGeneral = new DataSet();
            //rptGeneral objRptGeneral = new rptGeneral();

            //string strSql = "select * from t_pcpln where productiondate ='" + productionDate + "' and [SHIFTTYPE]='" + cmbProductShift.Text + "'  and RIGHT(LEFT(WIPITEMNO,5),2) = '" + cmbProductGroup.Text.Substring(0, 2) + "'   order by WIPITEMNO";
            //dsGeneral = ReportData.getDataSet(strSql, "t_pcpln", "TCPL");
            //objRptGeneral.SetDataSource(dsGeneral);
            //objRptGeneral.VerifyDatabase();

            //rptViewer.crystalReportViewer.ReportSource = objRptGeneral;
            rptViewer.Refresh();
            rptViewer.Show();
        }
    }
}
