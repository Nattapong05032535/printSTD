using App.Net.Configuration;
using App.Net.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Net.formatReceipt
{
    public class Receipt
    {
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

        //e.Graphics.DrawString($"ชื่อลูกค้า : {DetailDenom.customer}", printFont, Brushes.Black, new Point(10, 140));
        //e.Graphics.DrawString($"เลขผู้เสียภาษี : {Setting.printTaxId}", printFont, Brushes.Black, new Point(10, 140));
        //e.Graphics.DrawString($"TX# : {DetailDenom.txNo}", printFont, Brushes.Black, new Point(10, 160));
        //e.Graphics.DrawString($"Req# : {DetailDenom.reqNo}", printFont, Brushes.Black, new Point(10, 180));
        //e.Graphics.DrawString($"Seq# : {DetailDenom.seqNo}", printFont, Brushes.Black, new Point(10, 160));
        //e.Graphics.DrawString($"Date : {DetailDenom.txDate}", printFont, Brushes.Black, new Point(10, 160));
        //e.Graphics.DrawString($"======================================", printFont, Brushes.Black, new Point(10, 180));

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
                    //if (!string.IsNullOrEmpty(Setting.printAddress1) || Setting.printAddress1 != "Default")
                    //{
                    //    e.Graphics.DrawString(Setting.printAddress1, printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //}
                    //if (!string.IsNullOrEmpty(Setting.printAddress2) || Setting.printAddress2 != "Default")
                    //{
                    //    e.Graphics.DrawString(Setting.printAddress2, printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //}
                    //if (!string.IsNullOrEmpty(Setting.printAddress3) || Setting.printAddress3 != "Default")
                    //{
                    //    e.Graphics.DrawString(Setting.printAddress3, printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //}
                    e.Graphics.DrawString(Setting.printCompanyName, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(Setting.printMargin, 60));
                    e.Graphics.DrawString("บันทึกการทำรายการผ่านเครื่อง", new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(10, 80));
                    e.Graphics.DrawString($"ประเภท : {DetailDenom.printType}", printFont, Brushes.Black, new Point(10, 100));                   
                    e.Graphics.DrawString($"สาขา : {Setting.printBranchName}", printFont, Brushes.Black, new Point(10, 120));

                    int XPosition = 10;
                    float yPosition = 200;
                    int yCasePosition = 140; 

                    if (!string.IsNullOrEmpty(Setting.printTaxId) || Setting.printTaxId != "Default")
                    {
                        e.Graphics.DrawString($"เลขผู้เสียภาษี : {Setting.printTaxId}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20; 
                    }
                    //if (!string.IsNullOrEmpty(DetailDenom.txNo))
                    //{
                    //    e.Graphics.DrawString($"TX# : {DetailDenom.txNo}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20; 
                    //}
                    //if (!string.IsNullOrEmpty(DetailDenom.reqNo))
                    //{
                    //    e.Graphics.DrawString($"Req# : {DetailDenom.reqNo}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20; 
                    //}
                    //if (!string.IsNullOrEmpty(DetailDenom.seqNo))
                    //{
                    //    e.Graphics.DrawString($"Seq# : {DetailDenom.seqNo}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20; 
                    //}
                    if (!string.IsNullOrEmpty(DetailDenom.customer))
                    {
                        e.Graphics.DrawString($"ชื่อลูกค้า : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20; 
                    }
                    if (Setting.printMachineName != "Default")
                    {
                        e.Graphics.DrawString($"ชื่อเครื่อง : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                        yCasePosition += 20;
                    }
                    //if (Setting.printAddress1 != "Default")
                    //{
                    //    e.Graphics.DrawString($"ที่อยู่สาขา1 : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20;
                    //}
                    //if (Setting.printAddress2 != "Default")
                    //{
                    //    e.Graphics.DrawString($"ที่อยู่สาขา2 : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20;
                    //}
                    //if (Setting.printAddress3 != "Default")
                    //{
                    //    e.Graphics.DrawString($"ที่อยู่สาขา3 : {DetailDenom.customer}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
                    //    yCasePosition += 20;
                    //}

                    e.Graphics.DrawString($"Date : {DetailDenom.txDate}", printFont, Brushes.Black, new Point(XPosition, yCasePosition));
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
                        yPosition += lineSize.Height; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
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
                    else if (DetailDenom.printType == "remove-casstte")
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

        public void PrintReceiptReport(PrintPageEventArgs e, Image logoImage, string txtPayout)
        {
            try
            {
                Font printFont = new Font("Arial", 9);
                int XPosition = 10;
                int yPosition = 120;
                var srcRect = new Rectangle(0, 0, logoImage.Width, logoImage.Height);
                var desRect = new Rectangle(100, 0, 80, 53);

                if (e.Graphics != null)
                {
                    e.Graphics.DrawImage(logoImage, desRect, srcRect, GraphicsUnit.Pixel);
                    e.Graphics.DrawString(Setting.printCompanyName, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(Setting.printMargin, 60));
                    e.Graphics.DrawString("บันทึกการทำรายการผ่านเครื่อง", new Font("Arial", 9, FontStyle.Bold), Brushes.Black, new Point(XPosition, 80));
                    e.Graphics.DrawString($"Date : {DetailDenom.date.ToString()}", printFont, Brushes.Black, new Point(XPosition, 100));

                    if (!string.IsNullOrEmpty(DetailDenom.totalSale))
                    { 
                        e.Graphics.DrawString($"รวมยอดขายทั้งหมด : {DetailDenom.totalSale}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.totalFee))
                    {
                        e.Graphics.DrawString($"รวมยอดแลกเงินทั้งหมด : {DetailDenom.totalFee}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.totalRefill))
                    {
                        e.Graphics.DrawString($"รวมยอดเติมเงินทัั้งหมด : {DetailDenom.totalRefill}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.totalDeposit))
                    {
                        e.Graphics.DrawString($"รวมยอดฝากเงินทั้งหมด : {DetailDenom.totalDeposit}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.totalDispense))
                    {
                        e.Graphics.DrawString($"รวมยอดถอนเงินทัั้งหมด : {DetailDenom.totalDispense}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.totalRelease))
                    {
                        e.Graphics.DrawString($"รวมยอดนำเงินออกทั้งหมด : {DetailDenom.totalRelease}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.thisRelease))
                    {
                        e.Graphics.DrawString($"รวมยอดนำเงินออกรอบนี้ : {DetailDenom.thisRelease}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }
                    if (!string.IsNullOrEmpty(DetailDenom.thisRemaining))
                    {
                        e.Graphics.DrawString($"รวมยอดเงินคงเหลือในเครื่อง : {DetailDenom.thisRemaining}", printFont, Brushes.Black, new Point(XPosition, yPosition));
                        yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป
                    }

                    e.Graphics.DrawString($"======================================", printFont, Brushes.Black, new Point(XPosition, yPosition));
                    yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป

                    e.Graphics.DrawString("ลงชื่อ                " + DetailDenom.username, printFont, Brushes.Black, new Point(XPosition, yPosition));
                    yPosition += 20; // เพิ่มตำแหน่ง y สำหรับข้อความถัดไป

                    e.Graphics.DrawString("______________________________________", printFont, Brushes.Black, new Point(XPosition, yPosition));
                }
                else
                {
                    MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceiptReport_Receipt e.Graphics เป็น null", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[PrintReceiptReport_Receipt]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "PrintReceiptReport_Receipt ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
