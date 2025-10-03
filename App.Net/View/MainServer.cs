using System.ComponentModel;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using App.Net.Configuration;
using App.Net.formatReceipt;
using App.Net.Model;

namespace App.Net
{
    public partial class Server_API_Print : Form
    {
        private bool isDragging = false;

        private Point lastCursor;

        private NotifyIcon trayIcon;

        private ContextMenuStrip trayMenu;

        public Server_API_Print()
        {
            InitializeComponent();

            trayIcon = new NotifyIcon();
            trayMenu = new ContextMenuStrip();

            InitializeListener();
        }

        private void InitializeListener()
        {
            try
            {
                LoadConfig();

                LoadPrintersToComboBox();

                string port = Setting.portURL;
                string host = string.IsNullOrWhiteSpace(Setting.hostURL) ? "localhost" : Setting.hostURL.Trim();

                AppendStatus($"API Print CCWEB configured:");
                AppendStatus($" - Host: {host}");
                AppendStatus($" - Port: {port}");
                AppendStatus($" - Print API: http://{host}:{port}/api/print");
                AppendStatus($" - Report API: http://{host}:{port}/api/report");
            }
            catch (Exception ex)
            {
                AppendStatus($"Failed to initialize configuration: {ex.Message}");
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[InitializeListener()_MainServer]");
                }
            }
        }

        private void LoadPrintersToComboBox()
        {
            // ͧԴͧ
            cbNamePrinter.Items.Clear();

            // ͧԴͧ
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cbNamePrinter.Items.Add(printer);
            }

            // Ǩͺ ComboBox¡ѧ
            if (cbNamePrinter.Items.Count > 0)
            {
                // ͧ͡áѵѵ
                cbNamePrinter.SelectedIndex = 0;
            }
        }

        public void HandleFrontendAndPrint(RequestModel data, string status)
        {
            try
            {
                // Display UI             
                if (status == "Success")
                {
                    // display detail object
                    txtstatus.Text += status + " : " + data.TxNo + Environment.NewLine;
                    txt_printType.Text = DetailDenom.printType;
                    txt_txNo.Text = DetailDenom.txNo;
                    txt_reqNo.Text = DetailDenom.reqNo;
                    txt_seqNo.Text = DetailDenom.seqNo;
                    txt_customer.Text = DetailDenom.customer;
                    txt_amount.Text = DetailDenom.amount.ToString("N2");
                    txt_cashin.Text = DetailDenom.cashin.ToString("N2");
                    txt_change.Text = DetailDenom.change.ToString("N2");
                    txt_denom.Text = DetailDenom.denom;
                    txt_txDate.Text = DetailDenom.txDate;
                    txt_details.Text = DetailDenom.detail;
                    txt_showDetail.Text = DetailDenom.ShowDetail.ToString();
                    txt_showDenom.Text = DetailDenom.ShowDenom.ToString();
                    txt_isCancelSale.Text = DetailDenom.isCancelSale.ToString();
                    txt_username.Text = DetailDenom.username;

                    // display detail Denom Cashin
                    txtIn1000.Text = DetailDenom.Cashin1000.ToString();
                    txtIn500.Text = DetailDenom.Cashin500.ToString();
                    txtIn100.Text = DetailDenom.Cashin100.ToString();
                    txtIn20.Text = DetailDenom.Cashin20.ToString();
                    txtIn10.Text = DetailDenom.Cashin10.ToString();
                    txtIn5.Text = DetailDenom.Cashin5.ToString();
                    txtIn2.Text = DetailDenom.Cashin2.ToString();
                    txtIn1.Text = DetailDenom.Cashin1.ToString();
                    txtIn050.Text = DetailDenom.Cashin050.ToString();
                    txtIn025.Text = DetailDenom.Cashin025.ToString();

                    // display detail Denom Cashout
                    txtOut1000.Text = DetailDenom.CashOut1000.ToString();
                    txtOut500.Text = DetailDenom.CashOut500.ToString();
                    txtOut100.Text = DetailDenom.CashOut100.ToString();
                    txtOut50.Text = DetailDenom.CashOut50.ToString();
                    txtOut20.Text = DetailDenom.CashOut20.ToString();
                    txtOut10.Text = DetailDenom.CashOut10.ToString();
                    txtOut5.Text = DetailDenom.CashOut5.ToString();
                    txtOut2.Text = DetailDenom.CashOut2.ToString();
                    txtOut1.Text = DetailDenom.CashOut1.ToString();
                    txtOut050.Text = DetailDenom.CashOut050.ToString();
                    txtOut025.Text = DetailDenom.CashOut025.ToString();

                    if (DetailDenom.printType == "sale" && DetailDenom.isCancelSale == true)
                    {
                        DetailDenom.printType = "Cancle Sale";
                        txt_printType.Text = DetailDenom.printType;
                    }

                    if (Setting.DemoSetting && !string.IsNullOrEmpty(DetailDenom.txDate))
                    {
                        // ҹͧҡ DetailDenom.printer
                        string printer = Setting.printer;

                        // ҧ PrintDocumentС˹ PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // Ǩͺͧ͡ԧ
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "ͧ͡١ͧ öҶ֧." + Environment.NewLine;
                            return;  //͡ҡѧѹҡͧöҹ
                        }
                        else
                        {
                            if (DetailDenom.printType == "sale" && Setting.salePrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "refill" && Setting.refillPrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "dispense" && Setting.dispensePrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "deposit" && Setting.dispositPrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "endofday" && Setting.endofdayPrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "remove-casstte" && Setting.removePrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "exchange-sale" && Setting.exchangeSalePrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }
                            
                            else if (DetailDenom.printType == "exchange-disp" && Setting.exchangeDispPrint == true)
                            {
                                // ͧ١ͧ͡
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // 觤觾
                            }

                            else
                            {
                                txtstatus.Text += status + data + Environment.NewLine;
                            }
                        }
                    }
                    else
                    { 
                        txtstatus.Text += status + data + Environment.NewLine;
                    }
                }
                else
                {
                    txtstatus.Text += status + data + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Log($"UpdateFields Error in listener loop: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[UpdateFields(RequestModel data)]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "UpdateFields(RequestModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandleFrontendAndPrint(RequestReportModel data, string status)
        {
            try
            {
                // Display UI             
                if (status == "Success")
                {
                    // display detail object
                    txt_txDate.Text = DetailDenom.date;
                    txt_totalSale.Text = DetailDenom.totalSale;
                    txt_totalFee.Text = DetailDenom.totalFee;
                    txt_totalRefill.Text = DetailDenom.totalRefill;
                    txt_totalDeposit.Text = DetailDenom.totalDeposit;
                    txt_totalDispense.Text = DetailDenom.totalDispense;
                    txt_totalRelease.Text = DetailDenom.totalRelease;
                    txt_thisRelease.Text = DetailDenom.thisRelease;
                    txt_thisRemaining.Text = DetailDenom.thisRemaining;

                    if (Setting.DemoSetting)
                    {
                        // ҹͧҡ DetailDenom.printer
                        string printer = Setting.printer;

                        // ҧ PrintDocumentС˹ PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // Ǩͺͧ͡ԧ
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "ͧ͡١ͧ öҶ֧." + Environment.NewLine;
                            return;  //͡ҡѧѹҡͧöҹ
                        }
                        else
                        {
                            // ͧ١ͧ͡
                            printDoc.PrintPage += new PrintPageEventHandler(ReportDocument1_PrintPage);
                            printDoc.Print();  // 觤觾
                        }
                    }
                }
                else
                {
                    txtstatus.Text += status + data + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Log($"UpdateFields Error in listener loop: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[UpdateFields(RequestReportModel data)]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "UpdateFields(RequestReportModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Server_API_Print_Load(object sender, EventArgs e)
        {
            handleDesige();
            ImageLoader.LoadImages();
            ImageSlip();
            label17.Focus();
        }

        private void handleDesige()
        {
            try
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.Location = new Point(-3000, -3000);
                this.ShowInTaskbar = false;

                trayIcon = new NotifyIcon
                {
                    Text = "ApiPrinteeCCWEB",
                    Icon = new Icon(SystemIcons.Application, 40, 40),
                    Visible = true
                };

                //к Path Ѻ Icon (ҡ .ico)
                string iconPath = Path.Combine(Application.StartupPath, "iconBar", "printer.ico");
                if (File.Exists(iconPath))
                {
                    trayIcon.Icon = new Icon(iconPath);
                }

                trayMenu = new ContextMenuStrip();
                trayMenu.Items.Add("Open", null, OnOpen);
                trayMenu.Items.Add("Minimize", null, OnMinimize);
                trayMenu.Items.Add("Exit", null, OnExit);

                trayIcon.ContextMenuStrip = trayMenu;

                //к Path Ѻ Icon ͧ˹ҵҧѡ ()
                string iconPathbar = Path.Combine(Application.StartupPath, "iconBar", "statusbar.ico");
                if (File.Exists(iconPathbar))
                {
                    this.Icon = new Icon(iconPathbar);
                }

            }
            catch (Exception ex)
            {
                Log($"handleDesige() Error in listener loop: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[handleDesige()]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "handleDesige() ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTextBox(string textBoxName, string qtyValue)
        {
            try
            {
                Control[] controls = Controls.Find(textBoxName, true);

                if (controls.Length > 0 && controls[0] is System.Windows.Forms.TextBox textBox)
                {
                    textBox.Text = qtyValue;
                }
            }
            catch (Exception ex)
            {
                Log($"UpdateTextBox Error in listener loop: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[UpdateTextBox]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "UpdateTextBox ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Log(string message)
        {
            try
            {
                if (this != null && !this.IsDisposed && txtstatus != null && !txtstatus.IsDisposed)
                {
                    if (InvokeRequired)
                    {
                        try
                        {
                            if (!this.IsDisposed && !this.Disposing)
                            {
                                Invoke((MethodInvoker)delegate { Log(message); });
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            // Form Control١Դ ѴâͼԴҴ
                        }
                        catch (InvalidAsynchronousStateException)
                        {
                            //ô١Դ ѴâͼԴҴ log ͧ
                        }
                    }
                    else
                    {
                        if (!txtstatus.IsDisposed && !txtstatus.Disposing)
                        {
                            txtstatus.AppendText(message + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                try
                {
                    string logMessage = $"Log Error while processing request: {ex.Message}";
                    string filePath = Application.StartupPath + @"\errorLog.txt";

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Error Time: {DateTime.Now}");
                        writer.WriteLine(ex.ToString());
                        writer.WriteLine("----------------------------------------[Log]");
                    }

                    if (!this.IsDisposed)
                    {
                        MessageBox.Show("سҵԴ˹ҷ", "Log ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    // 㹡óշԴͼԴҴ㹡úѹ֡ log ʴ MessageBox 
                }
            }
        }

        private bool LoadConfig()
        {
            try
            {
                string filePath = Application.StartupPath + @"\Connect.txt";

                if (File.Exists(filePath))
                {
                    try
                    {
                        AppendStatus("Loading configuration from: " + filePath);
                        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                        foreach (string line in lines)
                        {
                            string[] parts = line.Split(';');

                            if (parts.Length == 2)
                            {
                                string key = parts[0];
                                string value = parts[1];

                                switch (key)
                                {

                                    case "printCompanyName":
                                        Setting.printCompanyName = value;
                                        AppendStatus("printCompanyName:" + value);
                                        break;
                                    case "printBranchName":
                                        Setting.printBranchName = value;
                                        AppendStatus("printBranchName:" + value);
                                        break;
                                    case "printAddress1":
                                        Setting.printAddress1 = value;
                                        AppendStatus("printAddress1:" + value);
                                        break;
                                    case "printAddress2":
                                        Setting.printAddress2 = value;
                                        AppendStatus("printAddress2:" + value);
                                        break;
                                    case "printAddress3":
                                        Setting.printAddress3 = value;
                                        AppendStatus("printAddress3:" + value);
                                        break;
                                    case "printTaxId":
                                        Setting.printTaxId = value;
                                        AppendStatus("printTaxId:" + value);
                                        break;
                                    case "printMachineName":
                                        Setting.printMachineName = value;
                                        AppendStatus("printMachineName:" + value);
                                        break;
                                    case "printBranchCode":
                                        Setting.printBranchCode = value;
                                        AppendStatus("printBranchCode:" + value);
                                        break;
                                    case "printMargin":
                                        Setting.printMargin = Convert.ToInt16(value);
                                        AppendStatus("printMargin:" + Setting.printMargin.ToString());
                                        break;
                                    case "printQueueNo":
                                        Setting.printQueueNo = Convert.ToInt16(value);
                                        AppendStatus("printQueueNo:" + Setting.printQueueNo.ToString());
                                        break;
                                    case "imageSlip":
                                        Setting.imageSlip = value;
                                        AppendStatus("imageSlip:" + Setting.imageSlip);
                                        break;
                                    case "DemoSetting":
                                        if (!bool.TryParse(value, out bool demoSettingValue))
                                        {
                                            demoSettingValue = false;
                                        }
                                        Setting.DemoSetting = demoSettingValue;
                                        AppendStatus("DemoSetting: " + Setting.DemoSetting.ToString());
                                        break;
                                    case "portURL":
                                        Setting.portURL = value;
                                        AppendStatus("portURL:" + Setting.portURL);
                                        break;
                                    case "hostURL":
                                        Setting.hostURL = value;
                                        AppendStatus("hostURL:" + Setting.hostURL);
                                        break;
                                    case "printerName":
                                        Setting.printer = value;
                                        AppendStatus("printerName:" + Setting.printer);
                                        break;
                                    default:
                                        AppendStatus("Unknown key: " + key);
                                        break;

                                    case "salePrint":
                                        if (!bool.TryParse(value, out bool salePrintSettingValue))
                                        {
                                            salePrintSettingValue = true;
                                        }
                                        Setting.salePrint = salePrintSettingValue;
                                        AppendStatus("salePrint: " + Setting.salePrint.ToString());
                                        break;
                                    case "refillPrint":
                                        if (!bool.TryParse(value, out bool refillPrintSettingValue))
                                        {
                                            refillPrintSettingValue = true;
                                        }
                                        Setting.refillPrint = refillPrintSettingValue;
                                        AppendStatus("refillPrint: " + Setting.refillPrint.ToString());
                                        break;
                                    case "dispensePrint":
                                        if (!bool.TryParse(value, out bool dispensePrintSettingValue))
                                        {
                                            dispensePrintSettingValue = true;
                                        }
                                        Setting.dispensePrint = dispensePrintSettingValue;
                                        AppendStatus("dispensePrint: " + Setting.dispensePrint.ToString());
                                        break;
                                    case "dispositPrint":
                                        if (!bool.TryParse(value, out bool dispositPrintSettingValue))
                                        {
                                            dispositPrintSettingValue = true;
                                        }
                                        Setting.dispositPrint = dispositPrintSettingValue;
                                        AppendStatus("dispositPrint: " + Setting.dispositPrint.ToString());
                                        break;
                                    case "endofdayPrint":
                                        if (!bool.TryParse(value, out bool endofdayPrintSettingValue))
                                        {
                                            endofdayPrintSettingValue = true;
                                        }
                                        Setting.endofdayPrint = endofdayPrintSettingValue;
                                        AppendStatus("endofdayPrint: " + Setting.endofdayPrint.ToString());
                                        break;
                                    case "removePrint":
                                        if (!bool.TryParse(value, out bool removePrintSettingValue))
                                        {
                                            removePrintSettingValue = true;
                                        }
                                        Setting.removePrint = removePrintSettingValue;
                                        AppendStatus("removePrint: " + Setting.removePrint.ToString());
                                        break;
                                    case "exchangeSalePrint":
                                        if (!bool.TryParse(value, out bool exchangeSalePrintSettingValue))
                                        {
                                            exchangeSalePrintSettingValue = true;
                                        }
                                        Setting.exchangeSalePrint = exchangeSalePrintSettingValue;
                                        AppendStatus("exchangeSalePrint: " + Setting.exchangeSalePrint.ToString());
                                        break;
                                    case "exchangeDispPrint":
                                        if (!bool.TryParse(value, out bool exchangeDispPrintSettingValue))
                                        {
                                            exchangeDispPrintSettingValue = true;
                                        }
                                        Setting.exchangeDispPrint = exchangeDispPrintSettingValue;
                                        AppendStatus("exchangeDispPrint: " + Setting.exchangeDispPrint.ToString());
                                        break;
                                }
                            }
                        }


                        // ҹͧҡ DetailDenom.printer
                        string printer = Setting.printer;

                        // ҧ PrintDocumentС˹ PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // Ǩͺͧ͡ԧ
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "ͧ͡١ͧ öҶ֧." + Environment.NewLine;
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        AppendStatus("Error: " + ex.Message);
                        return false;
                    }
                }
                else
                {
                    AppendStatus("Error: Configuration file not found at " + filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"LoadConfig failed!: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ImageSlip]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "ImageSlip ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ImageSlip()
        {
            try
            {
                Image? image = ImageLoader.GetImage("HeaderImage");

                if (image != null)
                {
                    PTCreatus.Image = image;
                }
                else
                {
                    string filePath = Application.StartupPath + @"\errorLog.txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Error Time: {DateTime.Now}");
                        writer.WriteLine("ImageSlip()_ApiPrinteeCCWEB image is null");
                        writer.WriteLine("----------------------------------------[ImageSlip()_ApiPrinteeCCWEB]");
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"ImageSlip Error while processing request: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ImageSlip]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "ImageSlip ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendStatus(string message)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action<string>(AppendStatus), message);
                    return;
                }

                txtstatus.AppendText(message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Log($"AppendStatus Error while processing request: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[AppendStatus]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "AppendStatus ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnMinimizet()
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;

                Rectangle workingArea = Screen.GetWorkingArea(this);
                int formWidth = this.Width;
                int formHeight = this.Height;

                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(workingArea.Right - formWidth, workingArea.Bottom - formHeight);
            }
            catch (Exception ex)
            {

                Log($"OnMinimizet() Error while processing request: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[OnMinimizet()]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "OnMinimizet() ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnMinimize(object? sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            catch (Exception ex)
            {
                Log($"OnMinimize Error while processing request: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[OnMinimize]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "OnMinimize ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnOpen(object? sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized || !this.Visible)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                }

                this.Activate();
                this.ShowInTaskbar = true;

                Rectangle screenBounds = Screen.GetBounds(this);
                int x = (screenBounds.Width - this.Width) / 2;
                int y = (screenBounds.Height - this.Height) / 2;

                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(x, y);
            }
            catch (Exception ex)
            {
                Log($"OnOpen Error while processing request: {ex.Message}");

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[OnOpen]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "OnOpen ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            try
            {
                Program.StopWebHost();
                Application.Exit();
            }
            catch
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Program.StopWebHost();
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[Form1_FormClosing]");
                }

                MessageBox.Show("سҵԴ˹ҷ", "Form1_FormClosing ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            try
            {
                Program.StopWebHost();
                Application.Exit();
            }
            catch
            {
            }
        }

        private void bt_minimize_Click(object sender, EventArgs e)
        {
            OnMinimizet();
        }

        private void bt_detail_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            else if (panel1.Visible == false)
            {
                panel1.Visible = true;
            }
        }

        private void ReciveDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Image logoImage = PTCreatus.Image;

            Receipt helper = new Receipt();
            helper.DetailPrintTypePadingRight(txtPayout);
            helper.PrintReceipt(e, logoImage, txtPayout.Text);

            if (!string.IsNullOrEmpty(txtPayout.Text))
            {
                DetailDenom.printType = "";
                DetailDenom.txNo = "";
                DetailDenom.reqNo = "";
                DetailDenom.seqNo = "";
                DetailDenom.customer = "";
                txtPayout.Text = "";
            }
        }

        private void Server_API_Print_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastCursor = e.Location;
        }

        private void Server_API_Print_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point delta = new Point(e.Location.X - lastCursor.X, e.Location.Y - lastCursor.Y);
                this.Location = new Point(this.Left + delta.X, this.Top + delta.Y);
            }
        }

        private void Server_API_Print_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void ReportDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Image logoImage = PTCreatus.Image;

            Receipt helper = new Receipt();
            helper.PrintReceiptReport(e, logoImage, txtPayout.Text);

            if (!string.IsNullOrEmpty(txtPayout.Text))
            {
                DetailDenom.date = "";
                DetailDenom.totalSale = "";
                DetailDenom.totalFee = "";
                DetailDenom.totalRefill = "";
                DetailDenom.totalDeposit = "";
                DetailDenom.totalDispense = "";
                DetailDenom.totalRelease = "";
                DetailDenom.thisRelease = "";
                DetailDenom.thisRemaining = "";
                txtPayout.Text = "";
            }
        }

        private void bt_set_Click(object sender, EventArgs e)
        {
            try
            {
                // Ǩͺͧ͡ҡ ComboBox ѧ
                if (cbNamePrinter.SelectedItem != null)
                {
                    // ҹͧ͡ҡ ComboBox
                    string selectedPrinter = cbNamePrinter.SelectedItem?.ToString() ?? "DefaultPrinter";  // null-coalescing

                    // ҹŨҡ Connect.txt
                    string filePath = Application.StartupPath + @"\Connect.txt";
                    string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                    // ҧ红ŷѻവ
                    List<string> updatedLines = new List<string>();

                    // 䢺÷Ѵ 'printerName' ᷹ҷ
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("printerName;"))
                        {
                            //  printerName, ᷹ѧͧ ; ªͧ͡
                            updatedLines.Add("printerName;" + selectedPrinter);
                        }
                        else
                        {
                            //  printerName, ÷Ѵ
                            updatedLines.Add(line);
                        }
                    }

                    //¹ŷǡѺ价 Connect.txt
                    File.WriteAllLines(filePath, updatedLines, Encoding.UTF8);

                    // ѻവõ駤ͧ Settings
                    Setting.printer = selectedPrinter;

                    AppendStatus("printerName set to: " + selectedPrinter);

                    //LoadConfig();
                }
                else
                {
                    MessageBox.Show("سͧ͡͹.", "ͧ͡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // ѴâͼԴҴ
                MessageBox.Show("ԴͼԴҴ㹡úѹ֡: " + ex.Message, "ͼԴҴ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}