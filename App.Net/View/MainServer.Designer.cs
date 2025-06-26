namespace App.Net
{
    partial class Server_API_Print
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtstatus = new TextBox();
            bt_detail = new Button();
            bt_minimize = new Button();
            bt_exit = new Button();
            label1 = new Label();
            txtOut1000 = new TextBox();
            txtOut500 = new TextBox();
            txtOut100 = new TextBox();
            txtOut50 = new TextBox();
            txtOut20 = new TextBox();
            txtOut10 = new TextBox();
            txtOut5 = new TextBox();
            txtOut2 = new TextBox();
            txtOut1 = new TextBox();
            txtOut050 = new TextBox();
            txtOut025 = new TextBox();
            txtIn1000 = new TextBox();
            txtIn500 = new TextBox();
            txtIn100 = new TextBox();
            txtIn50 = new TextBox();
            txtIn20 = new TextBox();
            txtIn10 = new TextBox();
            txtIn5 = new TextBox();
            txtIn2 = new TextBox();
            txtIn1 = new TextBox();
            txtIn050 = new TextBox();
            txtIn025 = new TextBox();
            txt_printType = new TextBox();
            txt_txNo = new TextBox();
            txt_reqNo = new TextBox();
            txt_seqNo = new TextBox();
            txt_amount = new TextBox();
            txt_cashin = new TextBox();
            txt_change = new TextBox();
            txt_denom = new TextBox();
            txt_txDate = new TextBox();
            txt_details = new TextBox();
            txt_showDetail = new TextBox();
            txt_showDenom = new TextBox();
            txtvalue = new TextBox();
            txtPayout = new TextBox();
            bt_set = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label15 = new Label();
            panel1 = new Panel();
            label36 = new Label();
            cbNamePrinter = new ComboBox();
            txt_thisRemaining = new TextBox();
            label37 = new Label();
            txt_thisRelease = new TextBox();
            label35 = new Label();
            txt_totalRelease = new TextBox();
            label34 = new Label();
            txt_totalDispense = new TextBox();
            label33 = new Label();
            txt_totalDeposit = new TextBox();
            label32 = new Label();
            txt_totalRefill = new TextBox();
            label31 = new Label();
            txt_totalFee = new TextBox();
            label30 = new Label();
            txt_totalSale = new TextBox();
            label29 = new Label();
            txt_customer = new TextBox();
            label28 = new Label();
            txt_username = new TextBox();
            label27 = new Label();
            txt_isCancelSale = new TextBox();
            label26 = new Label();
            label25 = new Label();
            label24 = new Label();
            label23 = new Label();
            label22 = new Label();
            label21 = new Label();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            PTCreatus = new PictureBox();
            textBox1 = new TextBox();
            ReciveDocument = new System.Drawing.Printing.PrintDocument();
            ReportDocument1 = new System.Drawing.Printing.PrintDocument();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PTCreatus).BeginInit();
            SuspendLayout();
            // 
            // txtstatus
            // 
            txtstatus.BackColor = Color.Black;
            txtstatus.BorderStyle = BorderStyle.None;
            txtstatus.Cursor = Cursors.Hand;
            txtstatus.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtstatus.ForeColor = Color.White;
            txtstatus.Location = new Point(5, 43);
            txtstatus.Multiline = true;
            txtstatus.Name = "txtstatus";
            txtstatus.ReadOnly = true;
            txtstatus.ScrollBars = ScrollBars.Both;
            txtstatus.Size = new Size(1336, 623);
            txtstatus.TabIndex = 0;
            // 
            // bt_detail
            // 
            bt_detail.Cursor = Cursors.Help;
            bt_detail.FlatAppearance.BorderSize = 0;
            bt_detail.FlatStyle = FlatStyle.Flat;
            bt_detail.Font = new Font("Century Gothic", 12F);
            bt_detail.ForeColor = Color.White;
            bt_detail.Location = new Point(1190, -3);
            bt_detail.Name = "bt_detail";
            bt_detail.Size = new Size(46, 56);
            bt_detail.TabIndex = 1;
            bt_detail.Text = ".......";
            bt_detail.UseVisualStyleBackColor = true;
            bt_detail.Click += bt_detail_Click;
            // 
            // bt_minimize
            // 
            bt_minimize.Cursor = Cursors.Hand;
            bt_minimize.FlatAppearance.BorderSize = 0;
            bt_minimize.FlatStyle = FlatStyle.Flat;
            bt_minimize.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bt_minimize.ForeColor = Color.White;
            bt_minimize.Location = new Point(1242, -4);
            bt_minimize.Name = "bt_minimize";
            bt_minimize.Size = new Size(46, 56);
            bt_minimize.TabIndex = 2;
            bt_minimize.Text = "____";
            bt_minimize.UseVisualStyleBackColor = true;
            bt_minimize.Click += bt_minimize_Click;
            // 
            // bt_exit
            // 
            bt_exit.Cursor = Cursors.Hand;
            bt_exit.FlatAppearance.BorderSize = 0;
            bt_exit.FlatStyle = FlatStyle.Flat;
            bt_exit.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bt_exit.ForeColor = Color.White;
            bt_exit.Location = new Point(1294, -6);
            bt_exit.Name = "bt_exit";
            bt_exit.Size = new Size(46, 56);
            bt_exit.TabIndex = 3;
            bt_exit.Text = "X";
            bt_exit.UseVisualStyleBackColor = true;
            bt_exit.Click += bt_exit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DarkGray;
            label1.Location = new Point(7, 9);
            label1.Name = "label1";
            label1.Size = new Size(137, 30);
            label1.TabIndex = 4;
            label1.Text = "Server Printer";
            // 
            // txtOut1000
            // 
            txtOut1000.Anchor = AnchorStyles.None;
            txtOut1000.BackColor = Color.Black;
            txtOut1000.BorderStyle = BorderStyle.None;
            txtOut1000.Font = new Font("Segoe UI", 12F);
            txtOut1000.ForeColor = Color.White;
            txtOut1000.Location = new Point(419, 109);
            txtOut1000.Name = "txtOut1000";
            txtOut1000.ReadOnly = true;
            txtOut1000.Size = new Size(52, 22);
            txtOut1000.TabIndex = 5;
            txtOut1000.Text = "-";
            txtOut1000.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut500
            // 
            txtOut500.Anchor = AnchorStyles.None;
            txtOut500.BackColor = Color.Black;
            txtOut500.BorderStyle = BorderStyle.None;
            txtOut500.Font = new Font("Segoe UI", 12F);
            txtOut500.ForeColor = Color.White;
            txtOut500.Location = new Point(419, 130);
            txtOut500.Name = "txtOut500";
            txtOut500.ReadOnly = true;
            txtOut500.Size = new Size(52, 22);
            txtOut500.TabIndex = 6;
            txtOut500.Text = "-";
            txtOut500.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut100
            // 
            txtOut100.Anchor = AnchorStyles.None;
            txtOut100.BackColor = Color.Black;
            txtOut100.BorderStyle = BorderStyle.None;
            txtOut100.Font = new Font("Segoe UI", 12F);
            txtOut100.ForeColor = Color.White;
            txtOut100.Location = new Point(419, 151);
            txtOut100.Name = "txtOut100";
            txtOut100.ReadOnly = true;
            txtOut100.Size = new Size(52, 22);
            txtOut100.TabIndex = 7;
            txtOut100.Text = "-";
            txtOut100.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut50
            // 
            txtOut50.Anchor = AnchorStyles.None;
            txtOut50.BackColor = Color.Black;
            txtOut50.BorderStyle = BorderStyle.None;
            txtOut50.Font = new Font("Segoe UI", 12F);
            txtOut50.ForeColor = Color.White;
            txtOut50.Location = new Point(419, 172);
            txtOut50.Name = "txtOut50";
            txtOut50.ReadOnly = true;
            txtOut50.Size = new Size(52, 22);
            txtOut50.TabIndex = 8;
            txtOut50.Text = "-";
            txtOut50.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut20
            // 
            txtOut20.Anchor = AnchorStyles.None;
            txtOut20.BackColor = Color.Black;
            txtOut20.BorderStyle = BorderStyle.None;
            txtOut20.Font = new Font("Segoe UI", 12F);
            txtOut20.ForeColor = Color.White;
            txtOut20.Location = new Point(419, 193);
            txtOut20.Name = "txtOut20";
            txtOut20.ReadOnly = true;
            txtOut20.Size = new Size(52, 22);
            txtOut20.TabIndex = 9;
            txtOut20.Text = "-";
            txtOut20.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut10
            // 
            txtOut10.Anchor = AnchorStyles.None;
            txtOut10.BackColor = Color.Black;
            txtOut10.BorderStyle = BorderStyle.None;
            txtOut10.Font = new Font("Segoe UI", 12F);
            txtOut10.ForeColor = Color.White;
            txtOut10.Location = new Point(419, 214);
            txtOut10.Name = "txtOut10";
            txtOut10.ReadOnly = true;
            txtOut10.Size = new Size(52, 22);
            txtOut10.TabIndex = 10;
            txtOut10.Text = "-";
            txtOut10.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut5
            // 
            txtOut5.Anchor = AnchorStyles.None;
            txtOut5.BackColor = Color.Black;
            txtOut5.BorderStyle = BorderStyle.None;
            txtOut5.Font = new Font("Segoe UI", 12F);
            txtOut5.ForeColor = Color.White;
            txtOut5.Location = new Point(419, 235);
            txtOut5.Name = "txtOut5";
            txtOut5.ReadOnly = true;
            txtOut5.Size = new Size(52, 22);
            txtOut5.TabIndex = 11;
            txtOut5.Text = "-";
            txtOut5.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut2
            // 
            txtOut2.Anchor = AnchorStyles.None;
            txtOut2.BackColor = Color.Black;
            txtOut2.BorderStyle = BorderStyle.None;
            txtOut2.Font = new Font("Segoe UI", 12F);
            txtOut2.ForeColor = Color.White;
            txtOut2.Location = new Point(419, 256);
            txtOut2.Name = "txtOut2";
            txtOut2.ReadOnly = true;
            txtOut2.Size = new Size(52, 22);
            txtOut2.TabIndex = 12;
            txtOut2.Text = "-";
            txtOut2.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut1
            // 
            txtOut1.Anchor = AnchorStyles.None;
            txtOut1.BackColor = Color.Black;
            txtOut1.BorderStyle = BorderStyle.None;
            txtOut1.Font = new Font("Segoe UI", 12F);
            txtOut1.ForeColor = Color.White;
            txtOut1.Location = new Point(419, 277);
            txtOut1.Name = "txtOut1";
            txtOut1.ReadOnly = true;
            txtOut1.Size = new Size(52, 22);
            txtOut1.TabIndex = 13;
            txtOut1.Text = "-";
            txtOut1.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut050
            // 
            txtOut050.Anchor = AnchorStyles.None;
            txtOut050.BackColor = Color.Black;
            txtOut050.BorderStyle = BorderStyle.None;
            txtOut050.Font = new Font("Segoe UI", 12F);
            txtOut050.ForeColor = Color.White;
            txtOut050.Location = new Point(419, 298);
            txtOut050.Name = "txtOut050";
            txtOut050.ReadOnly = true;
            txtOut050.Size = new Size(52, 22);
            txtOut050.TabIndex = 14;
            txtOut050.Text = "-";
            txtOut050.TextAlign = HorizontalAlignment.Right;
            // 
            // txtOut025
            // 
            txtOut025.Anchor = AnchorStyles.None;
            txtOut025.BackColor = Color.Black;
            txtOut025.BorderStyle = BorderStyle.None;
            txtOut025.Font = new Font("Segoe UI", 12F);
            txtOut025.ForeColor = Color.White;
            txtOut025.Location = new Point(420, 320);
            txtOut025.Name = "txtOut025";
            txtOut025.ReadOnly = true;
            txtOut025.Size = new Size(52, 22);
            txtOut025.TabIndex = 15;
            txtOut025.Text = "-";
            txtOut025.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn1000
            // 
            txtIn1000.Anchor = AnchorStyles.None;
            txtIn1000.BackColor = Color.Black;
            txtIn1000.BorderStyle = BorderStyle.None;
            txtIn1000.Font = new Font("Segoe UI", 12F);
            txtIn1000.ForeColor = Color.White;
            txtIn1000.Location = new Point(344, 109);
            txtIn1000.Name = "txtIn1000";
            txtIn1000.ReadOnly = true;
            txtIn1000.Size = new Size(52, 22);
            txtIn1000.TabIndex = 16;
            txtIn1000.Text = "-";
            txtIn1000.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn500
            // 
            txtIn500.Anchor = AnchorStyles.None;
            txtIn500.BackColor = Color.Black;
            txtIn500.BorderStyle = BorderStyle.None;
            txtIn500.Font = new Font("Segoe UI", 12F);
            txtIn500.ForeColor = Color.White;
            txtIn500.Location = new Point(344, 130);
            txtIn500.Name = "txtIn500";
            txtIn500.ReadOnly = true;
            txtIn500.Size = new Size(52, 22);
            txtIn500.TabIndex = 17;
            txtIn500.Text = "-";
            txtIn500.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn100
            // 
            txtIn100.Anchor = AnchorStyles.None;
            txtIn100.BackColor = Color.Black;
            txtIn100.BorderStyle = BorderStyle.None;
            txtIn100.Font = new Font("Segoe UI", 12F);
            txtIn100.ForeColor = Color.White;
            txtIn100.Location = new Point(344, 151);
            txtIn100.Name = "txtIn100";
            txtIn100.ReadOnly = true;
            txtIn100.Size = new Size(52, 22);
            txtIn100.TabIndex = 18;
            txtIn100.Text = "-";
            txtIn100.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn50
            // 
            txtIn50.Anchor = AnchorStyles.None;
            txtIn50.BackColor = Color.Black;
            txtIn50.BorderStyle = BorderStyle.None;
            txtIn50.Font = new Font("Segoe UI", 12F);
            txtIn50.ForeColor = Color.White;
            txtIn50.Location = new Point(344, 172);
            txtIn50.Name = "txtIn50";
            txtIn50.ReadOnly = true;
            txtIn50.Size = new Size(52, 22);
            txtIn50.TabIndex = 19;
            txtIn50.Text = "-";
            txtIn50.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn20
            // 
            txtIn20.Anchor = AnchorStyles.None;
            txtIn20.BackColor = Color.Black;
            txtIn20.BorderStyle = BorderStyle.None;
            txtIn20.Font = new Font("Segoe UI", 12F);
            txtIn20.ForeColor = Color.White;
            txtIn20.Location = new Point(344, 193);
            txtIn20.Name = "txtIn20";
            txtIn20.ReadOnly = true;
            txtIn20.Size = new Size(52, 22);
            txtIn20.TabIndex = 20;
            txtIn20.Text = "-";
            txtIn20.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn10
            // 
            txtIn10.Anchor = AnchorStyles.None;
            txtIn10.BackColor = Color.Black;
            txtIn10.BorderStyle = BorderStyle.None;
            txtIn10.Font = new Font("Segoe UI", 12F);
            txtIn10.ForeColor = Color.White;
            txtIn10.Location = new Point(344, 214);
            txtIn10.Name = "txtIn10";
            txtIn10.ReadOnly = true;
            txtIn10.Size = new Size(52, 22);
            txtIn10.TabIndex = 21;
            txtIn10.Text = "-";
            txtIn10.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn5
            // 
            txtIn5.Anchor = AnchorStyles.None;
            txtIn5.BackColor = Color.Black;
            txtIn5.BorderStyle = BorderStyle.None;
            txtIn5.Font = new Font("Segoe UI", 12F);
            txtIn5.ForeColor = Color.White;
            txtIn5.Location = new Point(344, 235);
            txtIn5.Name = "txtIn5";
            txtIn5.ReadOnly = true;
            txtIn5.Size = new Size(52, 22);
            txtIn5.TabIndex = 22;
            txtIn5.Text = "-";
            txtIn5.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn2
            // 
            txtIn2.Anchor = AnchorStyles.None;
            txtIn2.BackColor = Color.Black;
            txtIn2.BorderStyle = BorderStyle.None;
            txtIn2.Font = new Font("Segoe UI", 12F);
            txtIn2.ForeColor = Color.White;
            txtIn2.Location = new Point(344, 256);
            txtIn2.Name = "txtIn2";
            txtIn2.ReadOnly = true;
            txtIn2.Size = new Size(52, 22);
            txtIn2.TabIndex = 23;
            txtIn2.Text = "-";
            txtIn2.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn1
            // 
            txtIn1.Anchor = AnchorStyles.None;
            txtIn1.BackColor = Color.Black;
            txtIn1.BorderStyle = BorderStyle.None;
            txtIn1.Font = new Font("Segoe UI", 12F);
            txtIn1.ForeColor = Color.White;
            txtIn1.Location = new Point(344, 277);
            txtIn1.Name = "txtIn1";
            txtIn1.ReadOnly = true;
            txtIn1.Size = new Size(52, 22);
            txtIn1.TabIndex = 24;
            txtIn1.Text = "-";
            txtIn1.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn050
            // 
            txtIn050.Anchor = AnchorStyles.None;
            txtIn050.BackColor = Color.Black;
            txtIn050.BorderStyle = BorderStyle.None;
            txtIn050.Font = new Font("Segoe UI", 12F);
            txtIn050.ForeColor = Color.White;
            txtIn050.Location = new Point(344, 298);
            txtIn050.Name = "txtIn050";
            txtIn050.ReadOnly = true;
            txtIn050.Size = new Size(52, 22);
            txtIn050.TabIndex = 25;
            txtIn050.Text = "-";
            txtIn050.TextAlign = HorizontalAlignment.Right;
            // 
            // txtIn025
            // 
            txtIn025.Anchor = AnchorStyles.None;
            txtIn025.BackColor = Color.Black;
            txtIn025.BorderStyle = BorderStyle.None;
            txtIn025.Font = new Font("Segoe UI", 12F);
            txtIn025.ForeColor = Color.White;
            txtIn025.Location = new Point(345, 320);
            txtIn025.Name = "txtIn025";
            txtIn025.ReadOnly = true;
            txtIn025.Size = new Size(52, 22);
            txtIn025.TabIndex = 26;
            txtIn025.Text = "-";
            txtIn025.TextAlign = HorizontalAlignment.Right;
            // 
            // txt_printType
            // 
            txt_printType.Anchor = AnchorStyles.None;
            txt_printType.BackColor = Color.Black;
            txt_printType.BorderStyle = BorderStyle.None;
            txt_printType.Font = new Font("Segoe UI", 12F);
            txt_printType.ForeColor = Color.White;
            txt_printType.Location = new Point(150, 50);
            txt_printType.Name = "txt_printType";
            txt_printType.ReadOnly = true;
            txt_printType.Size = new Size(114, 22);
            txt_printType.TabIndex = 27;
            txt_printType.Text = "-";
            // 
            // txt_txNo
            // 
            txt_txNo.Anchor = AnchorStyles.None;
            txt_txNo.BackColor = Color.Black;
            txt_txNo.BorderStyle = BorderStyle.None;
            txt_txNo.Font = new Font("Segoe UI", 12F);
            txt_txNo.ForeColor = Color.White;
            txt_txNo.Location = new Point(150, 71);
            txt_txNo.Name = "txt_txNo";
            txt_txNo.ReadOnly = true;
            txt_txNo.Size = new Size(114, 22);
            txt_txNo.TabIndex = 28;
            txt_txNo.Text = "-";
            // 
            // txt_reqNo
            // 
            txt_reqNo.Anchor = AnchorStyles.None;
            txt_reqNo.BackColor = Color.Black;
            txt_reqNo.BorderStyle = BorderStyle.None;
            txt_reqNo.Font = new Font("Segoe UI", 12F);
            txt_reqNo.ForeColor = Color.White;
            txt_reqNo.Location = new Point(150, 92);
            txt_reqNo.Name = "txt_reqNo";
            txt_reqNo.ReadOnly = true;
            txt_reqNo.Size = new Size(114, 22);
            txt_reqNo.TabIndex = 29;
            txt_reqNo.Text = "-";
            // 
            // txt_seqNo
            // 
            txt_seqNo.Anchor = AnchorStyles.None;
            txt_seqNo.BackColor = Color.Black;
            txt_seqNo.BorderStyle = BorderStyle.None;
            txt_seqNo.Font = new Font("Segoe UI", 12F);
            txt_seqNo.ForeColor = Color.White;
            txt_seqNo.Location = new Point(150, 113);
            txt_seqNo.Name = "txt_seqNo";
            txt_seqNo.ReadOnly = true;
            txt_seqNo.Size = new Size(114, 22);
            txt_seqNo.TabIndex = 30;
            txt_seqNo.Text = "-";
            // 
            // txt_amount
            // 
            txt_amount.Anchor = AnchorStyles.None;
            txt_amount.BackColor = Color.Black;
            txt_amount.BorderStyle = BorderStyle.None;
            txt_amount.Font = new Font("Segoe UI", 12F);
            txt_amount.ForeColor = Color.White;
            txt_amount.Location = new Point(150, 134);
            txt_amount.Name = "txt_amount";
            txt_amount.ReadOnly = true;
            txt_amount.Size = new Size(114, 22);
            txt_amount.TabIndex = 31;
            txt_amount.Text = "-";
            // 
            // txt_cashin
            // 
            txt_cashin.Anchor = AnchorStyles.None;
            txt_cashin.BackColor = Color.Black;
            txt_cashin.BorderStyle = BorderStyle.None;
            txt_cashin.Font = new Font("Segoe UI", 12F);
            txt_cashin.ForeColor = Color.White;
            txt_cashin.Location = new Point(150, 155);
            txt_cashin.Name = "txt_cashin";
            txt_cashin.ReadOnly = true;
            txt_cashin.Size = new Size(114, 22);
            txt_cashin.TabIndex = 32;
            txt_cashin.Text = "-";
            // 
            // txt_change
            // 
            txt_change.Anchor = AnchorStyles.None;
            txt_change.BackColor = Color.Black;
            txt_change.BorderStyle = BorderStyle.None;
            txt_change.Font = new Font("Segoe UI", 12F);
            txt_change.ForeColor = Color.White;
            txt_change.Location = new Point(150, 176);
            txt_change.Name = "txt_change";
            txt_change.ReadOnly = true;
            txt_change.Size = new Size(114, 22);
            txt_change.TabIndex = 33;
            txt_change.Text = "-";
            // 
            // txt_denom
            // 
            txt_denom.Anchor = AnchorStyles.None;
            txt_denom.BackColor = Color.Black;
            txt_denom.BorderStyle = BorderStyle.None;
            txt_denom.Font = new Font("Segoe UI", 12F);
            txt_denom.ForeColor = Color.White;
            txt_denom.Location = new Point(150, 197);
            txt_denom.Name = "txt_denom";
            txt_denom.ReadOnly = true;
            txt_denom.Size = new Size(114, 22);
            txt_denom.TabIndex = 34;
            txt_denom.Text = "-";
            // 
            // txt_txDate
            // 
            txt_txDate.Anchor = AnchorStyles.None;
            txt_txDate.BackColor = Color.Black;
            txt_txDate.BorderStyle = BorderStyle.None;
            txt_txDate.Font = new Font("Segoe UI", 12F);
            txt_txDate.ForeColor = Color.White;
            txt_txDate.Location = new Point(150, 218);
            txt_txDate.Name = "txt_txDate";
            txt_txDate.ReadOnly = true;
            txt_txDate.Size = new Size(114, 22);
            txt_txDate.TabIndex = 35;
            txt_txDate.Text = "-";
            // 
            // txt_details
            // 
            txt_details.Anchor = AnchorStyles.None;
            txt_details.BackColor = Color.Black;
            txt_details.BorderStyle = BorderStyle.None;
            txt_details.Font = new Font("Segoe UI", 12F);
            txt_details.ForeColor = Color.White;
            txt_details.Location = new Point(150, 239);
            txt_details.Name = "txt_details";
            txt_details.ReadOnly = true;
            txt_details.Size = new Size(114, 22);
            txt_details.TabIndex = 36;
            txt_details.Text = "-";
            // 
            // txt_showDetail
            // 
            txt_showDetail.Anchor = AnchorStyles.None;
            txt_showDetail.BackColor = Color.Black;
            txt_showDetail.BorderStyle = BorderStyle.None;
            txt_showDetail.Font = new Font("Segoe UI", 12F);
            txt_showDetail.ForeColor = Color.White;
            txt_showDetail.Location = new Point(150, 260);
            txt_showDetail.Name = "txt_showDetail";
            txt_showDetail.ReadOnly = true;
            txt_showDetail.Size = new Size(114, 22);
            txt_showDetail.TabIndex = 37;
            txt_showDetail.Text = "-";
            // 
            // txt_showDenom
            // 
            txt_showDenom.Anchor = AnchorStyles.None;
            txt_showDenom.BackColor = Color.Black;
            txt_showDenom.BorderStyle = BorderStyle.None;
            txt_showDenom.Font = new Font("Segoe UI", 12F);
            txt_showDenom.ForeColor = Color.White;
            txt_showDenom.Location = new Point(150, 281);
            txt_showDenom.Name = "txt_showDenom";
            txt_showDenom.ReadOnly = true;
            txt_showDenom.Size = new Size(114, 22);
            txt_showDenom.TabIndex = 38;
            txt_showDenom.Text = "-";
            // 
            // txtvalue
            // 
            txtvalue.Anchor = AnchorStyles.None;
            txtvalue.BackColor = Color.Black;
            txtvalue.BorderStyle = BorderStyle.FixedSingle;
            txtvalue.Font = new Font("Segoe UI", 12F);
            txtvalue.ForeColor = Color.White;
            txtvalue.Location = new Point(363, 348);
            txtvalue.Name = "txtvalue";
            txtvalue.Size = new Size(114, 29);
            txtvalue.TabIndex = 39;
            txtvalue.Visible = false;
            // 
            // txtPayout
            // 
            txtPayout.Anchor = AnchorStyles.None;
            txtPayout.BackColor = Color.Black;
            txtPayout.BorderStyle = BorderStyle.FixedSingle;
            txtPayout.Font = new Font("Microsoft Sans Serif", 9.75F);
            txtPayout.ForeColor = Color.White;
            txtPayout.Location = new Point(363, 386);
            txtPayout.Name = "txtPayout";
            txtPayout.Size = new Size(114, 22);
            txtPayout.TabIndex = 40;
            txtPayout.Visible = false;
            // 
            // bt_set
            // 
            bt_set.Anchor = AnchorStyles.None;
            bt_set.BackColor = Color.FromArgb(24, 68, 195);
            bt_set.ForeColor = Color.White;
            bt_set.Location = new Point(386, 13);
            bt_set.Name = "bt_set";
            bt_set.Size = new Size(91, 29);
            bt_set.TabIndex = 41;
            bt_set.Text = "setprintter";
            bt_set.UseVisualStyleBackColor = false;
            bt_set.Click += bt_set_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(101, 50);
            label2.Name = "label2";
            label2.Size = new Size(47, 21);
            label2.TabIndex = 42;
            label2.Text = "type :";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(98, 72);
            label3.Name = "label3";
            label3.Size = new Size(50, 21);
            label3.TabIndex = 43;
            label3.Text = "txNo :";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(93, 91);
            label4.Name = "label4";
            label4.Size = new Size(55, 21);
            label4.TabIndex = 44;
            label4.Text = "reqID :";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(85, 113);
            label5.Name = "label5";
            label5.Size = new Size(62, 21);
            label5.TabIndex = 45;
            label5.Text = "seqNo :";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.ForeColor = Color.White;
            label6.Location = new Point(77, 135);
            label6.Name = "label6";
            label6.Size = new Size(71, 21);
            label6.TabIndex = 46;
            label6.Text = "amount :";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.None;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.ForeColor = Color.White;
            label7.Location = new Point(86, 156);
            label7.Name = "label7";
            label7.Size = new Size(61, 21);
            label7.TabIndex = 47;
            label7.Text = "cashin :";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.None;
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F);
            label8.ForeColor = Color.White;
            label8.Location = new Point(80, 177);
            label8.Name = "label8";
            label8.Size = new Size(67, 21);
            label8.TabIndex = 48;
            label8.Text = "change :";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.None;
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F);
            label9.ForeColor = Color.White;
            label9.Location = new Point(82, 198);
            label9.Name = "label9";
            label9.Size = new Size(66, 21);
            label9.TabIndex = 49;
            label9.Text = "denom :";
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.None;
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 12F);
            label10.ForeColor = Color.White;
            label10.Location = new Point(100, 218);
            label10.Name = "label10";
            label10.Size = new Size(49, 21);
            label10.TabIndex = 50;
            label10.Text = "Date :";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.None;
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 12F);
            label11.ForeColor = Color.White;
            label11.Location = new Point(86, 239);
            label11.Name = "label11";
            label11.Size = new Size(62, 21);
            label11.TabIndex = 51;
            label11.Text = "details :";
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.None;
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 12F);
            label12.ForeColor = Color.White;
            label12.Location = new Point(43, 261);
            label12.Name = "label12";
            label12.Size = new Size(105, 21);
            label12.TabIndex = 52;
            label12.Text = "showDenom :";
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.None;
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F);
            label13.ForeColor = Color.White;
            label13.Location = new Point(43, 281);
            label13.Name = "label13";
            label13.Size = new Size(105, 21);
            label13.TabIndex = 53;
            label13.Text = "showDenom :";
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.None;
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 12F);
            label14.ForeColor = Color.White;
            label14.Location = new Point(347, 79);
            label14.Name = "label14";
            label14.Size = new Size(57, 21);
            label14.TabIndex = 54;
            label14.Text = "CashIn";
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.None;
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 12F);
            label15.ForeColor = Color.White;
            label15.Location = new Point(407, 79);
            label15.Name = "label15";
            label15.Size = new Size(70, 21);
            label15.TabIndex = 55;
            label15.Text = "CashOut";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(label36);
            panel1.Controls.Add(cbNamePrinter);
            panel1.Controls.Add(txt_thisRemaining);
            panel1.Controls.Add(label37);
            panel1.Controls.Add(txt_thisRelease);
            panel1.Controls.Add(label35);
            panel1.Controls.Add(txt_totalRelease);
            panel1.Controls.Add(label34);
            panel1.Controls.Add(txt_totalDispense);
            panel1.Controls.Add(label33);
            panel1.Controls.Add(txt_totalDeposit);
            panel1.Controls.Add(label32);
            panel1.Controls.Add(txt_totalRefill);
            panel1.Controls.Add(label31);
            panel1.Controls.Add(txt_totalFee);
            panel1.Controls.Add(label30);
            panel1.Controls.Add(txt_totalSale);
            panel1.Controls.Add(label29);
            panel1.Controls.Add(txt_customer);
            panel1.Controls.Add(label28);
            panel1.Controls.Add(txt_username);
            panel1.Controls.Add(label27);
            panel1.Controls.Add(txt_isCancelSale);
            panel1.Controls.Add(label26);
            panel1.Controls.Add(label25);
            panel1.Controls.Add(label24);
            panel1.Controls.Add(label23);
            panel1.Controls.Add(label22);
            panel1.Controls.Add(label21);
            panel1.Controls.Add(label20);
            panel1.Controls.Add(label19);
            panel1.Controls.Add(label18);
            panel1.Controls.Add(label17);
            panel1.Controls.Add(label16);
            panel1.Controls.Add(PTCreatus);
            panel1.Controls.Add(label15);
            panel1.Controls.Add(label14);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(bt_set);
            panel1.Controls.Add(txtPayout);
            panel1.Controls.Add(txtvalue);
            panel1.Controls.Add(txt_showDenom);
            panel1.Controls.Add(txt_showDetail);
            panel1.Controls.Add(txt_details);
            panel1.Controls.Add(txt_txDate);
            panel1.Controls.Add(txt_denom);
            panel1.Controls.Add(txt_change);
            panel1.Controls.Add(txt_cashin);
            panel1.Controls.Add(txt_amount);
            panel1.Controls.Add(txt_seqNo);
            panel1.Controls.Add(txt_reqNo);
            panel1.Controls.Add(txt_txNo);
            panel1.Controls.Add(txt_printType);
            panel1.Controls.Add(txtIn025);
            panel1.Controls.Add(txtIn050);
            panel1.Controls.Add(txtIn1);
            panel1.Controls.Add(txtIn2);
            panel1.Controls.Add(txtIn5);
            panel1.Controls.Add(txtIn10);
            panel1.Controls.Add(txtIn20);
            panel1.Controls.Add(txtIn50);
            panel1.Controls.Add(txtIn100);
            panel1.Controls.Add(txtIn500);
            panel1.Controls.Add(txtIn1000);
            panel1.Controls.Add(txtOut025);
            panel1.Controls.Add(txtOut050);
            panel1.Controls.Add(txtOut1);
            panel1.Controls.Add(txtOut2);
            panel1.Controls.Add(txtOut5);
            panel1.Controls.Add(txtOut10);
            panel1.Controls.Add(txtOut20);
            panel1.Controls.Add(txtOut50);
            panel1.Controls.Add(txtOut100);
            panel1.Controls.Add(txtOut500);
            panel1.Controls.Add(txtOut1000);
            panel1.Location = new Point(828, 47);
            panel1.Name = "panel1";
            panel1.Size = new Size(505, 600);
            panel1.TabIndex = 56;
            panel1.Visible = false;
            // 
            // label36
            // 
            label36.Anchor = AnchorStyles.None;
            label36.AutoSize = true;
            label36.Font = new Font("Segoe UI", 12F);
            label36.ForeColor = Color.White;
            label36.Location = new Point(32, 571);
            label36.Name = "label36";
            label36.Size = new Size(117, 21);
            label36.TabIndex = 89;
            label36.Text = "thisRemaining :";
            // 
            // cbNamePrinter
            // 
            cbNamePrinter.BackColor = Color.Black;
            cbNamePrinter.Cursor = Cursors.Hand;
            cbNamePrinter.Font = new Font("Segoe UI", 12F);
            cbNamePrinter.ForeColor = Color.White;
            cbNamePrinter.FormattingEnabled = true;
            cbNamePrinter.Location = new Point(37, 13);
            cbNamePrinter.Name = "cbNamePrinter";
            cbNamePrinter.Size = new Size(343, 29);
            cbNamePrinter.TabIndex = 58;
            cbNamePrinter.Text = "asdasdasd";
            // 
            // txt_thisRemaining
            // 
            txt_thisRemaining.Anchor = AnchorStyles.None;
            txt_thisRemaining.BackColor = Color.Black;
            txt_thisRemaining.BorderStyle = BorderStyle.None;
            txt_thisRemaining.Font = new Font("Segoe UI", 12F);
            txt_thisRemaining.ForeColor = Color.White;
            txt_thisRemaining.Location = new Point(149, 570);
            txt_thisRemaining.Name = "txt_thisRemaining";
            txt_thisRemaining.ReadOnly = true;
            txt_thisRemaining.Size = new Size(114, 22);
            txt_thisRemaining.TabIndex = 88;
            txt_thisRemaining.Text = "-";
            // 
            // label37
            // 
            label37.Anchor = AnchorStyles.None;
            label37.AutoSize = true;
            label37.Font = new Font("Segoe UI", 12F);
            label37.ForeColor = Color.White;
            label37.Location = new Point(53, 542);
            label37.Name = "label37";
            label37.Size = new Size(95, 21);
            label37.TabIndex = 87;
            label37.Text = "thisRelease :";
            // 
            // txt_thisRelease
            // 
            txt_thisRelease.Anchor = AnchorStyles.None;
            txt_thisRelease.BackColor = Color.Black;
            txt_thisRelease.BorderStyle = BorderStyle.None;
            txt_thisRelease.Font = new Font("Segoe UI", 12F);
            txt_thisRelease.ForeColor = Color.White;
            txt_thisRelease.Location = new Point(150, 542);
            txt_thisRelease.Name = "txt_thisRelease";
            txt_thisRelease.ReadOnly = true;
            txt_thisRelease.Size = new Size(114, 22);
            txt_thisRelease.TabIndex = 86;
            txt_thisRelease.Text = "-";
            // 
            // label35
            // 
            label35.Anchor = AnchorStyles.None;
            label35.AutoSize = true;
            label35.Font = new Font("Segoe UI", 12F);
            label35.ForeColor = Color.White;
            label35.Location = new Point(46, 514);
            label35.Name = "label35";
            label35.Size = new Size(101, 21);
            label35.TabIndex = 85;
            label35.Text = "totalRelease :";
            // 
            // txt_totalRelease
            // 
            txt_totalRelease.Anchor = AnchorStyles.None;
            txt_totalRelease.BackColor = Color.Black;
            txt_totalRelease.BorderStyle = BorderStyle.None;
            txt_totalRelease.Font = new Font("Segoe UI", 12F);
            txt_totalRelease.ForeColor = Color.White;
            txt_totalRelease.Location = new Point(149, 514);
            txt_totalRelease.Name = "txt_totalRelease";
            txt_totalRelease.ReadOnly = true;
            txt_totalRelease.Size = new Size(114, 22);
            txt_totalRelease.TabIndex = 84;
            txt_totalRelease.Text = "-";
            // 
            // label34
            // 
            label34.Anchor = AnchorStyles.None;
            label34.AutoSize = true;
            label34.Font = new Font("Segoe UI", 12F);
            label34.ForeColor = Color.White;
            label34.Location = new Point(37, 486);
            label34.Name = "label34";
            label34.Size = new Size(111, 21);
            label34.TabIndex = 83;
            label34.Text = "totalDispense :";
            // 
            // txt_totalDispense
            // 
            txt_totalDispense.Anchor = AnchorStyles.None;
            txt_totalDispense.BackColor = Color.Black;
            txt_totalDispense.BorderStyle = BorderStyle.None;
            txt_totalDispense.Font = new Font("Segoe UI", 12F);
            txt_totalDispense.ForeColor = Color.White;
            txt_totalDispense.Location = new Point(150, 486);
            txt_totalDispense.Name = "txt_totalDispense";
            txt_totalDispense.ReadOnly = true;
            txt_totalDispense.Size = new Size(114, 22);
            txt_totalDispense.TabIndex = 82;
            txt_totalDispense.Text = "-";
            // 
            // label33
            // 
            label33.Anchor = AnchorStyles.None;
            label33.AutoSize = true;
            label33.Font = new Font("Segoe UI", 12F);
            label33.ForeColor = Color.White;
            label33.Location = new Point(46, 459);
            label33.Name = "label33";
            label33.Size = new Size(101, 21);
            label33.TabIndex = 81;
            label33.Text = "totalDeposit :";
            // 
            // txt_totalDeposit
            // 
            txt_totalDeposit.Anchor = AnchorStyles.None;
            txt_totalDeposit.BackColor = Color.Black;
            txt_totalDeposit.BorderStyle = BorderStyle.None;
            txt_totalDeposit.Font = new Font("Segoe UI", 12F);
            txt_totalDeposit.ForeColor = Color.White;
            txt_totalDeposit.Location = new Point(150, 458);
            txt_totalDeposit.Name = "txt_totalDeposit";
            txt_totalDeposit.ReadOnly = true;
            txt_totalDeposit.Size = new Size(114, 22);
            txt_totalDeposit.TabIndex = 80;
            txt_totalDeposit.Text = "-";
            // 
            // label32
            // 
            label32.Anchor = AnchorStyles.None;
            label32.AutoSize = true;
            label32.Font = new Font("Segoe UI", 12F);
            label32.ForeColor = Color.White;
            label32.Location = new Point(66, 431);
            label32.Name = "label32";
            label32.Size = new Size(83, 21);
            label32.TabIndex = 79;
            label32.Text = "totalRefill :";
            // 
            // txt_totalRefill
            // 
            txt_totalRefill.Anchor = AnchorStyles.None;
            txt_totalRefill.BackColor = Color.Black;
            txt_totalRefill.BorderStyle = BorderStyle.None;
            txt_totalRefill.Font = new Font("Segoe UI", 12F);
            txt_totalRefill.ForeColor = Color.White;
            txt_totalRefill.Location = new Point(149, 430);
            txt_totalRefill.Name = "txt_totalRefill";
            txt_totalRefill.ReadOnly = true;
            txt_totalRefill.Size = new Size(114, 22);
            txt_totalRefill.TabIndex = 78;
            txt_totalRefill.Text = "-";
            // 
            // label31
            // 
            label31.Anchor = AnchorStyles.None;
            label31.AutoSize = true;
            label31.Font = new Font("Segoe UI", 12F);
            label31.ForeColor = Color.White;
            label31.Location = new Point(77, 403);
            label31.Name = "label31";
            label31.Size = new Size(72, 21);
            label31.TabIndex = 77;
            label31.Text = "totalFee :";
            // 
            // txt_totalFee
            // 
            txt_totalFee.Anchor = AnchorStyles.None;
            txt_totalFee.BackColor = Color.Black;
            txt_totalFee.BorderStyle = BorderStyle.None;
            txt_totalFee.Font = new Font("Segoe UI", 12F);
            txt_totalFee.ForeColor = Color.White;
            txt_totalFee.Location = new Point(149, 402);
            txt_totalFee.Name = "txt_totalFee";
            txt_totalFee.ReadOnly = true;
            txt_totalFee.Size = new Size(114, 22);
            txt_totalFee.TabIndex = 76;
            txt_totalFee.Text = "-";
            // 
            // label30
            // 
            label30.Anchor = AnchorStyles.None;
            label30.AutoSize = true;
            label30.Font = new Font("Segoe UI", 12F);
            label30.ForeColor = Color.White;
            label30.Location = new Point(72, 375);
            label30.Name = "label30";
            label30.Size = new Size(77, 21);
            label30.TabIndex = 75;
            label30.Text = "totalSale :";
            // 
            // txt_totalSale
            // 
            txt_totalSale.Anchor = AnchorStyles.None;
            txt_totalSale.BackColor = Color.Black;
            txt_totalSale.BorderStyle = BorderStyle.None;
            txt_totalSale.Font = new Font("Segoe UI", 12F);
            txt_totalSale.ForeColor = Color.White;
            txt_totalSale.Location = new Point(149, 374);
            txt_totalSale.Name = "txt_totalSale";
            txt_totalSale.ReadOnly = true;
            txt_totalSale.Size = new Size(114, 22);
            txt_totalSale.TabIndex = 74;
            txt_totalSale.Text = "-";
            // 
            // label29
            // 
            label29.Anchor = AnchorStyles.None;
            label29.AutoSize = true;
            label29.Font = new Font("Segoe UI", 12F);
            label29.ForeColor = Color.White;
            label29.Location = new Point(67, 344);
            label29.Name = "label29";
            label29.Size = new Size(82, 21);
            label29.TabIndex = 73;
            label29.Text = "customer :";
            // 
            // txt_customer
            // 
            txt_customer.Anchor = AnchorStyles.None;
            txt_customer.BackColor = Color.Black;
            txt_customer.BorderStyle = BorderStyle.None;
            txt_customer.Font = new Font("Segoe UI", 12F);
            txt_customer.ForeColor = Color.White;
            txt_customer.Location = new Point(149, 343);
            txt_customer.Name = "txt_customer";
            txt_customer.ReadOnly = true;
            txt_customer.Size = new Size(114, 22);
            txt_customer.TabIndex = 72;
            txt_customer.Text = "-";
            // 
            // label28
            // 
            label28.Anchor = AnchorStyles.None;
            label28.AutoSize = true;
            label28.Font = new Font("Segoe UI", 12F);
            label28.ForeColor = Color.White;
            label28.Location = new Point(63, 323);
            label28.Name = "label28";
            label28.Size = new Size(86, 21);
            label28.TabIndex = 71;
            label28.Text = "username :";
            // 
            // txt_username
            // 
            txt_username.Anchor = AnchorStyles.None;
            txt_username.BackColor = Color.Black;
            txt_username.BorderStyle = BorderStyle.None;
            txt_username.Font = new Font("Segoe UI", 12F);
            txt_username.ForeColor = Color.White;
            txt_username.Location = new Point(149, 323);
            txt_username.Name = "txt_username";
            txt_username.ReadOnly = true;
            txt_username.Size = new Size(114, 22);
            txt_username.TabIndex = 70;
            txt_username.Text = "-";
            // 
            // label27
            // 
            label27.Anchor = AnchorStyles.None;
            label27.AutoSize = true;
            label27.Font = new Font("Segoe UI", 12F);
            label27.ForeColor = Color.White;
            label27.Location = new Point(46, 302);
            label27.Name = "label27";
            label27.Size = new Size(103, 21);
            label27.TabIndex = 69;
            label27.Text = "isCancelSale :";
            // 
            // txt_isCancelSale
            // 
            txt_isCancelSale.Anchor = AnchorStyles.None;
            txt_isCancelSale.BackColor = Color.Black;
            txt_isCancelSale.BorderStyle = BorderStyle.None;
            txt_isCancelSale.Font = new Font("Segoe UI", 12F);
            txt_isCancelSale.ForeColor = Color.White;
            txt_isCancelSale.Location = new Point(149, 302);
            txt_isCancelSale.Name = "txt_isCancelSale";
            txt_isCancelSale.ReadOnly = true;
            txt_isCancelSale.Size = new Size(114, 22);
            txt_isCancelSale.TabIndex = 68;
            txt_isCancelSale.Text = "-";
            // 
            // label26
            // 
            label26.Anchor = AnchorStyles.None;
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 12F);
            label26.ForeColor = Color.White;
            label26.Location = new Point(305, 317);
            label26.Name = "label26";
            label26.Size = new Size(47, 21);
            label26.TabIndex = 67;
            label26.Text = "0.25 :";
            // 
            // label25
            // 
            label25.Anchor = AnchorStyles.None;
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI", 12F);
            label25.ForeColor = Color.White;
            label25.Location = new Point(305, 296);
            label25.Name = "label25";
            label25.Size = new Size(47, 21);
            label25.TabIndex = 66;
            label25.Text = "0.50 :";
            // 
            // label24
            // 
            label24.Anchor = AnchorStyles.None;
            label24.AutoSize = true;
            label24.Font = new Font("Segoe UI", 12F);
            label24.ForeColor = Color.White;
            label24.Location = new Point(320, 274);
            label24.Name = "label24";
            label24.Size = new Size(26, 21);
            label24.TabIndex = 65;
            label24.Text = "1 :";
            // 
            // label23
            // 
            label23.Anchor = AnchorStyles.None;
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI", 12F);
            label23.ForeColor = Color.White;
            label23.Location = new Point(320, 253);
            label23.Name = "label23";
            label23.Size = new Size(26, 21);
            label23.TabIndex = 64;
            label23.Text = "2 :";
            // 
            // label22
            // 
            label22.Anchor = AnchorStyles.None;
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI", 12F);
            label22.ForeColor = Color.White;
            label22.Location = new Point(320, 232);
            label22.Name = "label22";
            label22.Size = new Size(26, 21);
            label22.TabIndex = 63;
            label22.Text = "5 :";
            // 
            // label21
            // 
            label21.Anchor = AnchorStyles.None;
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 12F);
            label21.ForeColor = Color.White;
            label21.Location = new Point(314, 211);
            label21.Name = "label21";
            label21.Size = new Size(35, 21);
            label21.TabIndex = 62;
            label21.Text = "10 :";
            // 
            // label20
            // 
            label20.Anchor = AnchorStyles.None;
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 12F);
            label20.ForeColor = Color.White;
            label20.Location = new Point(314, 190);
            label20.Name = "label20";
            label20.Size = new Size(35, 21);
            label20.TabIndex = 61;
            label20.Text = "20 :";
            // 
            // label19
            // 
            label19.Anchor = AnchorStyles.None;
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI", 12F);
            label19.ForeColor = Color.White;
            label19.Location = new Point(314, 170);
            label19.Name = "label19";
            label19.Size = new Size(35, 21);
            label19.TabIndex = 60;
            label19.Text = "50 :";
            // 
            // label18
            // 
            label18.Anchor = AnchorStyles.None;
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 12F);
            label18.ForeColor = Color.White;
            label18.Location = new Point(308, 148);
            label18.Name = "label18";
            label18.Size = new Size(44, 21);
            label18.TabIndex = 59;
            label18.Text = "100 :";
            // 
            // label17
            // 
            label17.Anchor = AnchorStyles.None;
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 12F);
            label17.ForeColor = Color.White;
            label17.Location = new Point(308, 127);
            label17.Name = "label17";
            label17.Size = new Size(44, 21);
            label17.TabIndex = 58;
            label17.Text = "500 :";
            // 
            // label16
            // 
            label16.Anchor = AnchorStyles.None;
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 12F);
            label16.ForeColor = Color.White;
            label16.Location = new Point(297, 107);
            label16.Name = "label16";
            label16.Size = new Size(56, 21);
            label16.TabIndex = 57;
            label16.Text = "1,000 :";
            // 
            // PTCreatus
            // 
            PTCreatus.Anchor = AnchorStyles.None;
            PTCreatus.BackColor = Color.Transparent;
            PTCreatus.Location = new Point(360, 452);
            PTCreatus.Name = "PTCreatus";
            PTCreatus.Size = new Size(125, 125);
            PTCreatus.SizeMode = PictureBoxSizeMode.Zoom;
            PTCreatus.TabIndex = 56;
            PTCreatus.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Black;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Font = new Font("Microsoft Sans Serif", 14.25F);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(1321, 43);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(20, 623);
            textBox1.TabIndex = 57;
            // 
            // ReciveDocument
            // 
            ReciveDocument.PrintPage += ReciveDocument_PrintPage;
            // 
            // ReportDocument1
            // 
            ReportDocument1.PrintPage += ReportDocument1_PrintPage;
            // 
            // Server_API_Print
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(39, 39, 39);
            ClientSize = new Size(1345, 671);
            Controls.Add(panel1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(txtstatus);
            Controls.Add(bt_detail);
            Controls.Add(bt_exit);
            Controls.Add(bt_minimize);
            Cursor = Cursors.NoMove2D;
            FormBorderStyle = FormBorderStyle.None;
            Name = "Server_API_Print";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Server_API_Print";
            Load += Server_API_Print_Load;
            MouseDown += Server_API_Print_MouseDown;
            MouseMove += Server_API_Print_MouseMove;
            MouseUp += Server_API_Print_MouseUp;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PTCreatus).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtstatus;
        private Button bt_detail;
        private Button bt_minimize;
        private Button bt_exit;
        private Label label1;
        private TextBox txtOut1000;
        private TextBox txtOut500;
        private TextBox txtOut100;
        private TextBox txtOut50;
        private TextBox txtOut20;
        private TextBox txtOut10;
        private TextBox txtOut5;
        private TextBox txtOut2;
        private TextBox txtOut1;
        private TextBox txtOut050;
        private TextBox txtOut025;
        private TextBox txtIn1000;
        private TextBox txtIn500;
        private TextBox txtIn100;
        private TextBox txtIn50;
        private TextBox txtIn20;
        private TextBox txtIn10;
        private TextBox txtIn5;
        private TextBox txtIn2;
        private TextBox txtIn1;
        private TextBox txtIn050;
        private TextBox txtIn025;
        private TextBox txt_printType;
        private TextBox txt_txNo;
        private TextBox txt_reqNo;
        private TextBox txt_seqNo;
        private TextBox txt_amount;
        private TextBox txt_cashin;
        private TextBox txt_change;
        private TextBox txt_denom;
        private TextBox txt_txDate;
        private TextBox txt_details;
        private TextBox txt_showDetail;
        private TextBox txt_showDenom;
        private TextBox txtvalue;
        private TextBox txtPayout;
        private Button bt_set;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Panel panel1;
        private PictureBox PTCreatus;
        private TextBox textBox1;
        private Label label26;
        private Label label25;
        private Label label24;
        private Label label23;
        private Label label22;
        private Label label21;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private System.Drawing.Printing.PrintDocument ReciveDocument;
        private Label label28;
        private TextBox txt_username;
        private Label label27;
        private TextBox txt_isCancelSale;
        private Label label29;
        private TextBox txt_customer;
        private System.Drawing.Printing.PrintDocument ReportDocument1;
        private Label label36;
        private TextBox txt_thisRemaining;
        private Label label37;
        private TextBox txt_thisRelease;
        private Label label35;
        private TextBox txt_totalRelease;
        private Label label34;
        private TextBox txt_totalDispense;
        private Label label33;
        private TextBox txt_totalDeposit;
        private Label label32;
        private TextBox txt_totalRefill;
        private Label label31;
        private TextBox txt_totalFee;
        private Label label30;
        private TextBox txt_totalSale;
        private ComboBox cbNamePrinter;
    }
}
