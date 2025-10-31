using App.Net.Configuration;
using App.Net.Model;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace App.Net.formatReceipt
{
    public class Receipt
    {
        private static string ShortAfterUnderscore(string s, int n = 8)
        {
            if (string.IsNullOrEmpty(s)) return s;

            int idx = s.IndexOf('_');
            if (idx < 0)
            {
                // กรณีไม่มี "_" ก็ย่อจากต้นสตริงแทน (ตามต้องการ)
                return s.Length > n ? s.Substring(0, n) + "..." : s;
            }

            int start = idx + 1;
            int take = Math.Min(n, Math.Max(0, s.Length - start));

            // ส่วนหน้าคงไว้ถึง "_"  +  ส่วนหลังเอาแค่ n ตัว  +  "..." ถ้ายังมีต่อ
            string head = s.Substring(0, start);
            string tail = s.Substring(start, take);
            bool needEllipsis = (start + take) < s.Length;

            return head + tail + (needEllipsis ? "..." : "");
        }

        private static IEnumerable<string> WrapFixedWidth(string text, int width)
        {
            if (string.IsNullOrEmpty(text) || width <= 0)
                yield break;

            // รักษา \r\n เดิม (ถ้ามี) แล้วค่อยตัดทีละบล็อก
            foreach (var raw in Regex.Split(text, @"\r?\n"))
            {
                if (raw.Length == 0)
                {
                    yield return string.Empty;
                    continue;
                }

                for (int i = 0; i < raw.Length; i += width)
                {
                    int len = Math.Min(width, raw.Length - i);
                    yield return raw.Substring(i, len);
                }
            }
        }

        private void DrawTextSale(PrintPageEventArgs e, string label, string value, int yPosition, Font font)
        {
            try
            {
                if (e.Graphics != null)
                {
                    SizeF textSize = e.Graphics.MeasureString(value, font);
                    float xPos = e.MarginBounds.Right - textSize.Width + Setting.printQueueNo;

                    e.Graphics.DrawString(label, font, Brushes.Black, new Point(10, yPosition));
                    e.Graphics.DrawString(value, font, Brushes.Black, xPos, yPosition);
                }
                else
                {
                    // ถ้า e.Graphics เป็น null
                    MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "DrawTextSale_Receipt e.Graphics เป็น null", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[DrawTextSale_Receipt]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "DrawTextSale_Receipt ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintReceipt(PrintPageEventArgs e, Image logoImage, string txtPayout)
        {
            try
            {
                Font printFont = new Font("Arial", 9);
                var srcRect = new Rectangle(0, 0, logoImage.Width, logoImage.Height);
                var desRect = new Rectangle(100, 0, 80, 53);

                if (e.Graphics != null)
                {
                    e.Graphics.DrawImage(logoImage, desRect, srcRect, GraphicsUnit.Pixel);
                    e.Graphics.DrawString(Setting.printCompanyName, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(Setting.printMargin, 60));
                    e.Graphics.DrawString("บันทึกการทำรายการผ่านเครื่อง", new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(10, 80));
                    e.Graphics.DrawString($"ประเภท        : {DetailDenom.printTypeTHB}", printFont, Brushes.Black, new Point(10, 100));                   
                    e.Graphics.DrawString($"สาขา            : {Setting.printBranchName}", printFont, Brushes.Black, new Point(10, 120));
                    string displayReqId = ShortAfterUnderscore(DetailDenom.txNo, 8);
                    e.Graphics.DrawString($"เลขที่รายการ  : {displayReqId}", printFont, Brushes.Black, new Point(10, 140));

                    int XPosition = 10;
                    float yPosition = 200;
                    int yCasePosition = 160; 

                    if (!string.IsNullOrEmpty(Setting.printTaxId) && Setting.printTaxId != "Default")
                    {
                        e.Graphics.DrawString($"เลขผู้เสียภาษี  : {Setting.printTaxId}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20; 
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.customer))
                    {
                        e.Graphics.DrawString($"ชื่อลูกค้า        : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20; 
                    }
                    if (!string.IsNullOrEmpty(Setting.printMachineName) && Setting.printMachineName != "Default")
                    {
                        e.Graphics.DrawString($"ชื่อเครื่อง       : {Setting.printMachineName}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20;
                    }

                    e.Graphics.DrawString($"Date            : {DetailDenom.txDate}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    yCasePosition += 20; 
                    e.Graphics.DrawString($"======================================", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    yCasePosition += 20; 

                    yPosition = yCasePosition;  // เว้นตำแหน่งจาก "======================================"
                    // พิมพ์ข้อมูลจาก txtPayout
                    string[] payoutLines = txtPayout.Split('\n');
                    foreach (string line in payoutLines)
                    {
                        SizeF lineSize = e.Graphics.MeasureString(line, printFont);
                        e.Graphics.DrawString(line, printFont, Brushes.Black, new PointF(XPosition, yPosition));
                        yPosition += lineSize.Height; 
                    }

                    if (DetailDenom.printType == "sale")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "รับเงินเข้า", DetailDenom.cashin.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "เงินทอน", DetailDenom.change.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "Cancle Sale")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "รับเงินเข้า", DetailDenom.cashin.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "เงินคืน", DetailDenom.change.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "dispense")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", "- "+DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "deposit")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "refill")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.cashin.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "exchange-sale")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "รับเงินเข้า", DetailDenom.cashin.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 20;
                        DrawTextSale(e, "เงินทอน", DetailDenom.change.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "exchange-disp")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.cashin.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "remove-cassette")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "endofday")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "cash-collect")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินนำลงกล่องรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "change-dispense")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", "- " + DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "change-deposit")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "tip-exchange")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินแลกทริปรวม", DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                    else if (DetailDenom.printType == "receive")
                    {
                        yPosition += 10;
                        DrawTextSale(e, "ยอดเงินรวม", "- " + DetailDenom.amount.ToString("N2"), (int)yPosition, printFont);

                        yPosition += 30;
                        e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, (int)yPosition));

                        yPosition += 40;
                        e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, (int)yPosition));
                    }
                }
                else
                {
                    MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceipt_Receipt e.Graphics เป็น null", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[PrintReceipt_Receipt]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceipt_Receipt ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintReceiptReport_EndOfDay(PrintPageEventArgs e, Image logoImage, string txtPayout)
        {
            try
            {
                Font printFont = new Font("Arial", 9);
                int XPosition = 10;
                int yPosition = 240;
                var srcRect = new Rectangle(0, 0, logoImage.Width, logoImage.Height);
                var desRect = new Rectangle(100, 0, 80, 53);

                if (e.Graphics != null)
                {
                    // โลโก้และหัวรายงาน
                    e.Graphics.DrawImage(logoImage, desRect, srcRect, GraphicsUnit.Pixel);
                    e.Graphics.DrawString(Setting.printCompanyName, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(Setting.printMargin, 60));
                    e.Graphics.DrawString("บันทึกสรุปการทำรายการผ่านเครื่อง", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(40, 90));

                    // หัวบิล
                    e.Graphics.DrawString($"วันที่ปิดรอบปัจจุบัน     : {DetailDenom.transactionDate}", printFont, Brushes.Black, new Point(XPosition, 120));

                    e.Graphics.DrawString($"วันที่ปิดรอบก่อนหน้า   : {DetailDenom.lastEndOfDay}", printFont, Brushes.Black, new Point(XPosition, 140));

                    e.Graphics.DrawString($"จำนวนธุรกรรมทั้งหมด : {DetailDenom.transactionCount.ToString("N0")}" +" รายการ", printFont, Brushes.Black, new Point(XPosition, 160));

                    e.Graphics.DrawString($"รหัสเครื่อง    : {DetailDenom.machineId}", printFont, Brushes.Black, new Point(XPosition, 180));

                    //e.Graphics.DrawString($"ReqId : {DetailDenom.reqId}", printFont, Brushes.Black, new Point(XPosition, 200));
                    string displayReqId = ShortAfterUnderscore(DetailDenom.reqId, 8);

                    e.Graphics.DrawString($"เลขที่รายการ : {displayReqId}", printFont, Brushes.Black, new Point(XPosition, 200));


                    e.Graphics.DrawString($"======================================", printFont, Brushes.Black, new Point(XPosition, 220));

                    // ==========================
                    // เนื้อหาหลักของรายงาน
                    // ==========================

                    if (!string.IsNullOrEmpty(DetailDenom.totalSale))
                    {
                        if (double.TryParse(DetailDenom.totalSale, out double totalSale))
                        {
                            e.Graphics.DrawString($"ยอดขายทั้งหมด            : {totalSale.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดขายทั้งหมด            : {DetailDenom.totalSale}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalReceive))
                    {
                        if (double.TryParse(DetailDenom.totalReceive, out double totalReceive))
                        {
                            e.Graphics.DrawString($"ยอดคืนเงินทั้งหมด        : {totalReceive.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดคืนเงินทั้งหมด        : {DetailDenom.totalReceive}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalDeposit))
                    {
                        if (double.TryParse(DetailDenom.totalDeposit, out double totalDeposit))
                        {
                            e.Graphics.DrawString($"ยอดฝากเงินทั้งหมด       : {totalDeposit.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดฝากเงินทั้งหมด       : {DetailDenom.totalDeposit}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalRefill))
                    {
                        if (double.TryParse(DetailDenom.totalRefill, out double totalRefill))
                        {
                            e.Graphics.DrawString($"ยอดเติมเงินทอนทั้งหมด : {totalRefill.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดเติมเงินทอนทั้งหมด : {DetailDenom.totalRefill}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalDispense))
                    {
                      
                        if (double.TryParse(DetailDenom.totalDispense, out double totalDispense))
                        {
                            e.Graphics.DrawString($"ยอดถอนเงินทั้งหมด      : {totalDispense.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดถอนเงินทั้งหมด      : {DetailDenom.totalDispense}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalChangeDispense))
                    {
                       
                        if (double.TryParse(DetailDenom.totalChangeDispense, out double totalChangeDispense))
                        {
                            e.Graphics.DrawString($"ยอดเบิกเงินทอนทั้งหมด : {totalChangeDispense.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดเบิกเงินทอนทั้งหมด : {DetailDenom.totalChangeDispense}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalChangeDeposit))
                    {
                      
                        if (double.TryParse(DetailDenom.totalChangeDeposit, out double totalChangeDeposit))
                        {
                            e.Graphics.DrawString($"ยอดคืนเงินทอนทั้งหมด  : {totalChangeDeposit.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดคืนเงินทอนทั้งหมด  : {DetailDenom.totalChangeDeposit}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalExchangeSale))
                    {
                       
                        if (double.TryParse(DetailDenom.totalExchangeSale, out double totalExchangeSale))
                        {
                            e.Graphics.DrawString($"ยอดแลกเงินรับทั้งหมด   : {totalExchangeSale.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดแลกเงินรับทั้งหมด   : {DetailDenom.totalExchangeSale}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalExchangeDispense))
                    {
                        if (double.TryParse(DetailDenom.totalExchangeDispense, out double totalExchangeDispense))
                        {
                            e.Graphics.DrawString($"ยอดแลกเงินจ่ายทั้งหมด  : {totalExchangeDispense.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดแลกเงินจ่ายทั้งหมด  : {DetailDenom.totalExchangeDispense}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.totalRemove))
                    {
                        if (double.TryParse(DetailDenom.totalRemove, out double totalRemove))
                        {
                            e.Graphics.DrawString($"ยอดนำเงินออกทั้งหมด   : {totalRemove.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดนำเงินออกทั้งหมด   : {DetailDenom.totalRemove}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.thisRemove))
                    {                      
                        if (double.TryParse(DetailDenom.thisRemove, out double thisRemove))
                        {
                            e.Graphics.DrawString($"ยอดนำเงินออกรอบนี้     : {thisRemove.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดนำเงินออกรอบนี้     : {DetailDenom.thisRemove}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    if (!string.IsNullOrEmpty(DetailDenom.thisRemaining))
                    {                       
                        if (double.TryParse(DetailDenom.thisRemaining, out double thisRemaining))
                        {
                            e.Graphics.DrawString($"ยอดคงเหลือในเครื่อง     : {thisRemaining.ToString("N2")}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString($"ยอดคงเหลือในเครื่อง     : {DetailDenom.thisRemaining}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                            yPosition += 20;
                        }
                    }

                    e.Graphics.DrawString($"--------------------------------------------------------------", printFont, Brushes.Black, new Point(XPosition, yPosition));
                    yPosition += 40;

                    // ส่วนท้ายของรายงาน
                    e.Graphics.DrawString("ลงชื่อ .................................................................", printFont, Brushes.Black, new Point(XPosition, yPosition));

                }
                else
                {
                    MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceiptReport_EndOfDay: e.Graphics เป็น null", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[PrintReceiptReport_EndOfDay]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceiptReport_EndOfDay ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DetailPrintTypePadingRight(TextBox txtPayout)
        {
            try
            {
                txtPayout.Clear();

                if (DetailDenom.ShowDetail == true)
                {
                    txtPayout.AppendText($"รายละเอียด " + "\r\n");

                    string detailData = DetailDenom.detail;
                    string[] details = detailData.Split(',');

                    for (int i = 0; i < details.Length; i++)
                    {
                        txtPayout.AppendText($"- {details[i]}\r\n");
                    }

                    txtPayout.AppendText($"----------------------------------------------------------------"+"\r\n");
                }

                if (DetailDenom.ShowRemarkes)
                {
                    txtPayout.AppendText("หมายเหตุการทำรายการ\r\n");

                    string detailData = DetailDenom.remarkes ?? string.Empty;

                    foreach (var line in WrapFixedWidth(detailData, 35)) // 30 ตัว/บรรทัด
                    {
                        txtPayout.AppendText(line + Environment.NewLine);
                    }

                    txtPayout.AppendText("----------------------------------------------------------------\r\n");
                }

                if (DetailDenom.ShowDenom == true)
                {
                    int[] cashinValues = {
                        DetailDenom.Cashin1000, DetailDenom.Cashin500, DetailDenom.Cashin100, DetailDenom.Cashin50,
                        DetailDenom.Cashin20, DetailDenom.Cashin10, DetailDenom.Cashin5, DetailDenom.Cashin2,
                        DetailDenom.Cashin1, DetailDenom.Cashin050, DetailDenom.Cashin025
                    };
                    if (cashinValues.Any(cash => cash > 0))
                    {
                        txtPayout.AppendText("เงินเข้าเครื่อง");

                        if (DetailDenom.Cashin1000 > 0)
                        {
                            if (DetailDenom.Cashin1000 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.Cashin1000}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin1000 * 1000:n0}");
                            }
                            else if (DetailDenom.Cashin1000 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.Cashin1000}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin1000 * 1000:n0}");
                            }
                            else if (DetailDenom.Cashin1000 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.Cashin1000}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin1000 * 1000:n0}");
                            }
                            else if (DetailDenom.Cashin1000<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.Cashin1000}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin1000 * 1000:n0}");
                            }
                        }
                        if (DetailDenom.Cashin500 > 0)
                        {
                            if (DetailDenom.Cashin500 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin500}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin500 * 500:n0}");
                            }
                            else if (DetailDenom.Cashin500 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin500}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin500 * 500:n0}");
                            }
                            else if (DetailDenom.Cashin500 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin500}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin500 * 500:n0}");
                            }
                            else if (DetailDenom.Cashin500<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin500}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin500 * 500:n0}");
                            }
                        }
                        if (DetailDenom.Cashin100 > 0)
                        {

                            if (DetailDenom.Cashin100 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin100}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin100 * 100:n0}");
                            }
                            else if (DetailDenom.Cashin100 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin100}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin100 * 100:n0}");
                            }
                            else if (DetailDenom.Cashin100 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin100}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin100 * 100:n0}");
                            }
                            else if (DetailDenom.Cashin100<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.Cashin100}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin100 * 100:n0}");
                            }
                        }
                        if (DetailDenom.Cashin50 > 0)
                        {
                            if (DetailDenom.Cashin50 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin50}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin50 * 50:n0}");
                            }
                            else if (DetailDenom.Cashin50 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin50}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin50 * 50:n0}");
                            }
                            else if (DetailDenom.Cashin50 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin50}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin50 * 50:n0}");
                            }
                            else if (DetailDenom.Cashin50<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin50}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin50 * 50:n0}");
                            }
                        }
                        if (DetailDenom.Cashin20 > 0)
                        {
                            if (DetailDenom.Cashin20 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin20}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin20 * 20:n0}");
                            }
                            else if (DetailDenom.Cashin20 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin20}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin20 * 20:n0}");
                            }
                            else if (DetailDenom.Cashin20 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin20}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin20 * 20:n0}");
                            }
                            else if (DetailDenom.Cashin20<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin20}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin20 * 20:n0}");
                            }
                        }
                        if (DetailDenom.Cashin10 > 0)
                        {
                            if (DetailDenom.Cashin10 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin10}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin10 * 10:n0}");
                            }
                            else if (DetailDenom.Cashin10 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin10}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin10 * 10:n0}");
                            }
                            else if (DetailDenom.Cashin10 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin10}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin10 * 10:n0}");
                            }
                            else if (DetailDenom.Cashin10<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.Cashin10}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin10 * 10:n0}");
                            }
                        }
                        if (DetailDenom.Cashin5 > 0)
                        {
                            if (DetailDenom.Cashin5 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin5}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin5 * 5:n0}");
                            }
                            else if (DetailDenom.Cashin5 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin5}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin5 * 5:n0}");
                            }
                            else if (DetailDenom.Cashin5 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin5}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin5 * 5:n0}");
                            }
                            else if (DetailDenom.Cashin5<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin5}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin5 * 5:n0}");
                            }
                        }
                        if (DetailDenom.Cashin2 > 0)
                        {
                            if (DetailDenom.Cashin2 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin2}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin2 * 2:n0}");
                            }
                            else if (DetailDenom.Cashin2 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin2}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin2 * 2:n0}");
                            }
                            else if (DetailDenom.Cashin2 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin2}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin2 * 2:n0}");
                            }
                            else if (DetailDenom.Cashin2<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin2}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin2 * 2:n0}");
                            }
                        }
                        if (DetailDenom.Cashin1 > 0)
                        {
                            if (DetailDenom.Cashin1 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin1}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin1 * 1:n0}");
                            }
                            else if (DetailDenom.Cashin1 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin1}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin1 * 1:n0}");
                            }
                            else if (DetailDenom.Cashin1 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin1}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin1 * 1:n0}");
                            }
                            else if (DetailDenom.Cashin1<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.Cashin1}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin1 * 1:n0}");
                            }
                        }
                        if (DetailDenom.Cashin050 > 0)
                        {
                            if (DetailDenom.Cashin050 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin050}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin050 * 0.50:n2}");
                            }
                            else if (DetailDenom.Cashin050 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin050}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin050 * 0.50:n2}");
                            }
                            else if (DetailDenom.Cashin050 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin050}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin050 * 0.50:n2}");
                            }
                            else if (DetailDenom.Cashin050<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin050}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin050 * 0.50:n2}");
                            }
                        }
                        if (DetailDenom.Cashin025 > 0)
                        {
                            if (DetailDenom.Cashin025 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin025}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.Cashin025 * 0.25:n2}");
                            }
                            else if (DetailDenom.Cashin025 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin025}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.Cashin025 * 0.25:n2}");
                            }
                            else if (DetailDenom.Cashin025 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin025}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.Cashin025 * 0.25:n2}");
                            }
                            else if (DetailDenom.Cashin025<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.Cashin025}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.Cashin025 * 0.25:n2}");
                            }
                        }

                        txtPayout.AppendText("\r\n");
                    }

                    int[] cashOutValues = {
                        DetailDenom.CashOut1000, DetailDenom.CashOut500, DetailDenom.CashOut100, DetailDenom.CashOut50,
                        DetailDenom.CashOut20, DetailDenom.CashOut10, DetailDenom.CashOut5, DetailDenom.CashOut2,
                        DetailDenom.CashOut1, DetailDenom.CashOut050, DetailDenom.CashOut025
                    };
                    if (cashOutValues.Any(cash => cash > 0))
                    {
                        txtPayout.AppendText("เงินออกจากเครื่อง");

                        if (DetailDenom.CashOut1000 > 0)
                        {
                            if (DetailDenom.CashOut1000 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashOut1000}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashOut1000 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashOut1000}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashOut1000 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashOut1000}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashOut1000 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashOut1000}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut1000 * 1000:n0}");
                            }
                        }
                        if (DetailDenom.CashOut500 > 0)
                        {
                            if (DetailDenom.CashOut500 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut500}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut500 * 500:n0}");
                            }
                            else if (DetailDenom.CashOut500 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut500}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut500 * 500:n0}");
                            }
                            else if (DetailDenom.CashOut500 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut500}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut500 * 500:n0}");
                            }
                            else if (DetailDenom.CashOut500 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut500}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut500 * 500:n0}");
                            }
                        }
                        if (DetailDenom.CashOut100 > 0)
                        {

                            if (DetailDenom.CashOut100 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut100}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut100 * 100:n0}");
                            }
                            else if (DetailDenom.CashOut100 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut100}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut100 * 100:n0}");
                            }
                            else if (DetailDenom.CashOut100 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut100}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut100 * 100:n0}");
                            }
                            else if (DetailDenom.CashOut100 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashOut100}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut100 * 100:n0}");
                            }
                        }
                        if (DetailDenom.CashOut50 > 0)
                        {
                            if (DetailDenom.CashOut50 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut50}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut50 * 50:n0}");
                            }
                            else if (DetailDenom.CashOut50 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut50}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut50 * 50:n0}");
                            }
                            else if (DetailDenom.CashOut50 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut50}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut50 * 50:n0}");
                            }
                            else if (DetailDenom.CashOut50<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut50}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut50 * 50:n0}");
                            }
                        }
                        if (DetailDenom.CashOut20 > 0)
                        {
                            if (DetailDenom.CashOut20 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut20}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut20 * 20:n0}");
                            }
                            else if (DetailDenom.CashOut20 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut20}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut20 * 20:n0}");
                            }
                            else if (DetailDenom.CashOut20 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut20}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut20 * 20:n0}");
                            }
                            else if (DetailDenom.CashOut20<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut20}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut20 * 20:n0}");
                            }
                        }
                        if (DetailDenom.CashOut10 > 0)
                        {
                            if (DetailDenom.CashOut10 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut10}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut10 * 10:n0}");
                            }
                            else if (DetailDenom.CashOut10 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut10}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut10 * 10:n0}");
                            }
                            else if (DetailDenom.CashOut10 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut10}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut10 * 10:n0}");
                            }
                            else if (DetailDenom.CashOut10<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashOut10}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut10 * 10:n0}");
                            }
                        }
                        if (DetailDenom.CashOut5 > 0)
                        {
                            if (DetailDenom.CashOut5 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut5}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut5 * 5:n0}");
                            }
                            else if (DetailDenom.CashOut5 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut5}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut5 * 5:n0}");
                            }
                            else if (DetailDenom.CashOut5 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut5}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut5 * 5:n0}");
                            }
                            else if (DetailDenom.CashOut5<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut5}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut5 * 5:n0}");
                            }
                        }
                        if (DetailDenom.CashOut2 > 0)
                        {
                            if (DetailDenom.CashOut2 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut2}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut2 * 2:n0}");
                            }
                            else if (DetailDenom.CashOut2 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut2}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut2 * 2:n0}");
                            }
                            else if (DetailDenom.CashOut2 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut2}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut2 * 2:n0}");
                            }
                            else if (DetailDenom.CashOut2<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut2}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut2 * 2:n0}");
                            }
                        }
                        if (DetailDenom.CashOut1 > 0)
                        {
                            if (DetailDenom.CashOut1 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut1}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut1 * 1:n0}");
                            }
                            else if (DetailDenom.CashOut1 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut1}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut1 * 1:n0}");
                            }
                            else if (DetailDenom.CashOut1 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut1}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut1 * 1:n0}");
                            }
                            else if (DetailDenom.CashOut1<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashOut1}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut1 * 1:n0}");
                            }
                        }
                        if (DetailDenom.CashOut050 > 0)
                        {
                            if (DetailDenom.CashOut050 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut050}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashOut050 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut050}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashOut050 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut050}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashOut050<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut050}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut050 * 0.50:n2}");
                            }
                        }
                        if (DetailDenom.CashOut025 > 0)
                        {
                            if (DetailDenom.CashOut025 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut025}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashOut025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashOut025 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut025}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashOut025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashOut025 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut025}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashOut025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashOut025<= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashOut025}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashOut025 * 0.25:n2}");
                            }
                        }

                        txtPayout.AppendText("\r\n");
                    }

                    int[] cashCollectValues = {
                        DetailDenom.CashCollect1000, DetailDenom.CashCollect500, DetailDenom.CashCollect100, DetailDenom.CashCollect50,
                        DetailDenom.CashCollect20, DetailDenom.CashCollect10, DetailDenom.CashCollect5, DetailDenom.CashCollect2,
                        DetailDenom.CashCollect1, DetailDenom.CashCollect050, DetailDenom.CashCollect025
                    };
                    if (cashCollectValues.Any(cash => cash > 0))
                    {
                        txtPayout.AppendText("เงินนำลงกล่องเก็บ");

                        if (DetailDenom.CashCollect1000 > 0)
                        {
                            if (DetailDenom.CashCollect1000 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashCollect1000}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashCollect1000 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashCollect1000}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashCollect1000 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashCollect1000}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect1000 * 1000:n0}");
                            }
                            else if (DetailDenom.CashCollect1000 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1,000" + "X".PadLeft(10) + "".PadLeft(13) + $"{DetailDenom.CashCollect1000}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect1000 * 1000:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect500 > 0)
                        {
                            if (DetailDenom.CashCollect500 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect500}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect500 * 500:n0}");
                            }
                            else if (DetailDenom.CashCollect500 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect500}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect500 * 500:n0}");
                            }
                            else if (DetailDenom.CashCollect500 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect500}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect500 * 500:n0}");
                            }
                            else if (DetailDenom.CashCollect500 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"500" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect500}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect500 * 500:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect100 > 0)
                        {

                            if (DetailDenom.CashCollect100 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect100}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect100 * 100:n0}");
                            }
                            else if (DetailDenom.CashCollect100 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect100}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect100 * 100:n0}");
                            }
                            else if (DetailDenom.CashCollect100 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect100}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect100 * 100:n0}");
                            }
                            else if (DetailDenom.CashCollect100 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"100" + "X".PadLeft(13) + "".PadLeft(13) + $"{DetailDenom.CashCollect100}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect100 * 100:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect50 > 0)
                        {
                            if (DetailDenom.CashCollect50 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect50}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect50 * 50:n0}");
                            }
                            else if (DetailDenom.CashCollect50 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect50}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect50 * 50:n0}");
                            }
                            else if (DetailDenom.CashCollect50 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect50}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect50 * 50:n0}");
                            }
                            else if (DetailDenom.CashCollect50 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"50" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect50}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect50 * 50:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect20 > 0)
                        {
                            if (DetailDenom.CashCollect20 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect20}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect20 * 20:n0}");
                            }
                            else if (DetailDenom.CashCollect20 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect20}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect20 * 20:n0}");
                            }
                            else if (DetailDenom.CashCollect20 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect20}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect20 * 20:n0}");
                            }
                            else if (DetailDenom.CashCollect20 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"20" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect20}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect20 * 20:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect10 > 0)
                        {
                            if (DetailDenom.CashCollect10 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect10}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect10 * 10:n0}");
                            }
                            else if (DetailDenom.CashCollect10 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect10}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect10 * 10:n0}");
                            }
                            else if (DetailDenom.CashCollect10 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect10}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect10 * 10:n0}");
                            }
                            else if (DetailDenom.CashCollect10 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"10" + "X".PadLeft(15) + "".PadLeft(13) + $"{DetailDenom.CashCollect10}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect10 * 10:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect5 > 0)
                        {
                            if (DetailDenom.CashCollect5 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect5}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect5 * 5:n0}");
                            }
                            else if (DetailDenom.CashCollect5 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect5}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect5 * 5:n0}");
                            }
                            else if (DetailDenom.CashCollect5 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect5}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect5 * 5:n0}");
                            }
                            else if (DetailDenom.CashCollect5 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"5" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect5}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect5 * 5:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect2 > 0)
                        {
                            if (DetailDenom.CashCollect2 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect2}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect2 * 2:n0}");
                            }
                            else if (DetailDenom.CashCollect2 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect2}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect2 * 2:n0}");
                            }
                            else if (DetailDenom.CashCollect2 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect2}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect2 * 2:n0}");
                            }
                            else if (DetailDenom.CashCollect2 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"2" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect2}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect2 * 2:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect1 > 0)
                        {
                            if (DetailDenom.CashCollect1 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect1}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect1 * 1:n0}");
                            }
                            else if (DetailDenom.CashCollect1 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect1}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect1 * 1:n0}");
                            }
                            else if (DetailDenom.CashCollect1 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect1}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect1 * 1:n0}");
                            }
                            else if (DetailDenom.CashCollect1 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"1" + "X".PadLeft(17) + "".PadLeft(13) + $"{DetailDenom.CashCollect1}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect1 * 1:n0}");
                            }
                        }
                        if (DetailDenom.CashCollect050 > 0)
                        {
                            if (DetailDenom.CashCollect050 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect050}"
                                                            + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashCollect050 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect050}"
                                                            + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashCollect050 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect050}"
                                                            + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect050 * 0.50:n2}");
                            }
                            else if (DetailDenom.CashCollect050 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.50" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect050}"
                                                            + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect050 * 0.50:n2}");
                            }
                        }
                        if (DetailDenom.CashCollect025 > 0)
                        {
                            if (DetailDenom.CashCollect025 > 999)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect025}"
                                                             + $"=     ".PadLeft(8) + $"{DetailDenom.CashCollect025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashCollect025 > 99)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect025}"
                                                             + $"=     ".PadLeft(10) + $"{DetailDenom.CashCollect025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashCollect025 > 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect025}"
                                                             + $"=     ".PadLeft(12) + $"{DetailDenom.CashCollect025 * 0.25:n2}");
                            }
                            else if (DetailDenom.CashCollect025 <= 9)
                            {
                                txtPayout.AppendText("\r\n" + $"0.25" + "X".PadLeft(12) + "".PadLeft(13) + $"{DetailDenom.CashCollect025}"
                                                             + $"=     ".PadLeft(14) + $"{DetailDenom.CashCollect025 * 0.25:n2}");
                            }
                        }
                    }

                    txtPayout.AppendText("\r\n" + "----------------------------------------------------------------");
                }
            }
            catch
            {
                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "DetailPrint_Test_", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
