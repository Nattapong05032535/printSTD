using System.ComponentModel;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using App.Net.Configuration;
using App.Net.Controller;
using App.Net.formatReceipt;
using App.Net.Model;

namespace App.Net
{
    public partial class Server_API_Print : Form
    {
        private bool isDragging = false;

        private Point lastCursor;

        private HttpListener _listener;

        private Thread? _listenerThread;

        private NotifyIcon trayIcon;

        private ContextMenuStrip trayMenu;

        private ConcretePrintController _printController;

        private CancellationTokenSource _cancellationTokenSource;

        public Server_API_Print()
        {
            InitializeComponent();

            _cancellationTokenSource = new CancellationTokenSource();
            _listener = new HttpListener();
            trayIcon = new NotifyIcon();
            trayMenu = new ContextMenuStrip();

            _printController = new ConcretePrintController(this);

            InitializeListener();

            Task.Run(() => ListenForRequests(_cancellationTokenSource.Token)).ContinueWith(t =>
            {
                // จัดการข้อผิดพลาดจากงาน background
                if (t.Exception != null)
                {
                    Log($"Error: {t.Exception.Message}");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void InitializeListener()
        {
            try
            {
                LoadConfig();

                LoadPrintersToComboBox();

                if (_listener == null)
                {
                    _listener = new HttpListener();
                }

                if (_listener.IsListening)
                {
                    _listener.Stop();
                    _listener.Close();
                    AppendStatus("Stopped the existing listener before restarting.");
                }

                string port = Setting.portURL;
                string urlPrint = $"http://localhost:{port}/api/print/";
                string urlReport = $"http://localhost:{port}/api/report/";

                _listener.Prefixes.Add(urlPrint);
                _listener.Prefixes.Add(urlReport);

                if (IsPortInUse(port))
                {
                    AppendStatus($"The port {port} is already in use. Please choose another port.");
                    MessageBox.Show($"The port {port} is already in use. Please choose another port.",
                                     "มีการเปิดใช้งานพอร์ตหรือโปรแกรมแล้ว",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);

                    Environment.Exit(0);
                    return;
                }

                _listener.Start();
                AppendStatus($"Start API Print CCWEB:");
                AppendStatus($" - {urlPrint}");
                AppendStatus($" - {urlReport}");
            }
            catch (Exception ex)
            {
                AppendStatus($"Failed to start the API Print CCWEB service at http://localhost:{Setting.portURL}/api/print/");
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
            // ลบรายการที่มีอยู่ใน ComboBox ออกก่อน
            cbNamePrinter.Items.Clear();

            // เพิ่มเครื่องพิมพ์ทั้งหมดที่ติดตั้งในเครื่อง
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cbNamePrinter.Items.Add(printer);
            }

            // ตรวจสอบว่า ComboBox มีรายการแล้วหรือยัง
            if (cbNamePrinter.Items.Count > 0)
            {
                // เลือกเครื่องพิมพ์แรกโดยอัตโนมัติ
                cbNamePrinter.SelectedIndex = 0;
            }
        }

        private bool IsPortInUse(string port)
        {
            try
            {
                var tcpListener = new TcpListener(IPAddress.Loopback, int.Parse(port));
                tcpListener.Start();
                tcpListener.Stop();
                return false;
            }
            catch (SocketException)
            {
                return true;
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
                        // อ่านชื่อเครื่องพิมพ์จาก DetailDenom.printer
                        string printer = Setting.printer;

                        // สร้าง PrintDocument และกำหนด PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // ตรวจสอบว่าเครื่องพิมพ์ที่เลือกมีอยู่จริง
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "เครื่องพิมพ์ที่เลือกไม่ถูกต้อง หรือไม่สามารถเข้าถึงได้." + Environment.NewLine;
                            return;  // ออกจากฟังก์ชันหากเครื่องพิมพ์ไม่สามารถใช้งานได้
                        }
                        else
                        {
                            if (DetailDenom.printType == "sale" && Setting.salePrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "refill" && Setting.refillPrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "dispense" && Setting.dispensePrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "deposit" && Setting.dispositPrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "endofday" && Setting.endofdayPrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "remove-casstte" && Setting.removePrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "exchange-sale" && Setting.exchangeSalePrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
                            }
                            
                            else if (DetailDenom.printType == "exchange-disp" && Setting.exchangeDispPrint == true)
                            {
                                // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                                printDoc.PrintPage += new PrintPageEventHandler(ReciveDocument_PrintPage);
                                printDoc.Print();  // ส่งคำสั่งพิมพ์
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "UpdateFields(RequestModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    //if (Setting.DemoSetting)
                    //{
                    //    ReportDocument1.PrintController = new StandardPrintController();
                    //    ReportDocument1.Print();
                    //}

                    if (Setting.DemoSetting)
                    {
                        // อ่านชื่อเครื่องพิมพ์จาก DetailDenom.printer
                        string printer = Setting.printer;

                        // สร้าง PrintDocument และกำหนด PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // ตรวจสอบว่าเครื่องพิมพ์ที่เลือกมีอยู่จริง
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "เครื่องพิมพ์ที่เลือกไม่ถูกต้อง หรือไม่สามารถเข้าถึงได้." + Environment.NewLine;
                            return;  // ออกจากฟังก์ชันหากเครื่องพิมพ์ไม่สามารถใช้งานได้
                        }
                        else
                        {
                            // ถ้าเครื่องพิมพ์ถูกต้องให้พิมพ์เอกสาร
                            printDoc.PrintPage += new PrintPageEventHandler(ReportDocument1_PrintPage);
                            printDoc.Print();  // ส่งคำสั่งพิมพ์
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "UpdateFields(RequestReportModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ListenForRequests(CancellationToken token)
        {
            try
            {
                while (_listener.IsListening && !token.IsCancellationRequested)
                {
                    var context = await _listener.GetContextAsync();

                    await _printController.ProcessRequest(context);
                }
            }
            catch (Exception ex)
            {
                Log($"ListenForRequests Error: {ex.Message}");
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

                // ระบุ Path สำหรับ Icon (หากมีไฟล์ .ico)
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

                // ระบุ Path สำหรับ Icon ของหน้าต่างหลัก (ถ้ามี)
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "handleDesige() ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "UpdateTextBox ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            // Form หรือ Control ถูกปิดไปแล้ว จัดการข้อผิดพลาดนี้โดยไม่ทำอะไร
                        }
                        catch (InvalidAsynchronousStateException)
                        {
                            // เธรดถูกปิด จัดการข้อผิดพลาดนี้โดยไม่ทำอะไร หรือ log ตามต้องการ
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
                        MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "Log ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    // ในกรณีที่เกิดข้อผิดพลาดในการบันทึก log หรือแสดง MessageBox ก็ไม่ทำอะไร
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


                        // อ่านชื่อเครื่องพิมพ์จาก DetailDenom.printer
                        string printer = Setting.printer;

                        // สร้าง PrintDocument และกำหนด PrinterSettings
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrinterSettings.PrinterName = printer;

                        // ตรวจสอบว่าเครื่องพิมพ์ที่เลือกมีอยู่จริง
                        if (!printDoc.PrinterSettings.IsValid)
                        {
                            txtstatus.Text += "เครื่องพิมพ์ที่เลือกไม่ถูกต้อง หรือไม่สามารถเข้าถึงได้." + Environment.NewLine;
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "ImageSlip ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "ImageSlip ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "AppendStatus ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "OnMinimizet() ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "OnMinimize ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "OnOpen ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            try
            {
                _listener?.Stop();
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
                _listener?.Stop();
                _cancellationTokenSource?.Cancel();
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

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "Form1_FormClosing ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            try
            {
                _listener?.Stop();
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
                // ตรวจสอบว่าได้เลือกเครื่องพิมพ์จาก ComboBox หรือยัง
                if (cbNamePrinter.SelectedItem != null)
                {
                    // อ่านชื่อเครื่องพิมพ์ที่เลือกจาก ComboBox
                    string selectedPrinter = cbNamePrinter.SelectedItem?.ToString() ?? "DefaultPrinter";  // ใช้ null-coalescing

                    // อ่านข้อมูลจากไฟล์ Connect.txt
                    string filePath = Application.StartupPath + @"\Connect.txt";
                    string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                    // สร้างตัวแปรเพื่อเก็บข้อมูลที่อัปเดตแล้ว
                    List<string> updatedLines = new List<string>();

                    // แก้ไขบรรทัดที่มี 'printerName' โดยแทนที่ค่าที่มีอยู่เดิม
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("printerName;"))
                        {
                            // ถ้ามี printerName, แทนที่ค่าหลังเครื่องหมาย ; ด้วยชื่อเครื่องพิมพ์ที่เลือก
                            updatedLines.Add("printerName;" + selectedPrinter);
                        }
                        else
                        {
                            // ถ้าไม่ใช่ printerName, ให้เพิ่มบรรทัดนี้ตามปกติ
                            updatedLines.Add(line);
                        }
                    }

                    // เขียนข้อมูลที่แก้ไขแล้วกลับไปที่ไฟล์ Connect.txt
                    File.WriteAllLines(filePath, updatedLines, Encoding.UTF8);

                    // อัปเดตการตั้งค่าเครื่องพิมพ์ใน Settings
                    Setting.printer = selectedPrinter;

                    AppendStatus("printerName set to: " + selectedPrinter);

                    //LoadConfig();
                }
                else
                {
                    MessageBox.Show("กรุณาเลือกเครื่องพิมพ์ก่อน.", "เลือกเครื่องพิมพ์", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
