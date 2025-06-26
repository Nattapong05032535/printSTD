using System;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;

namespace App.Net.Model
{
    public class RequestReportModel
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = "";

        [JsonPropertyName("totalSale")]
        public string TotalSale { get; set; } = "";

        [JsonPropertyName("totalFee")]
        public string TotalFee { get; set; } = "";

        [JsonPropertyName("totalRefill")]
        public string TotalRefill { get; set; } = "";

        [JsonPropertyName("totalDeposit")]
        public string TotalDeposit { get; set; } = "";

        [JsonPropertyName("totalDispense")]
        public string TotalDispense { get; set; } = "";

        [JsonPropertyName("totalRelease")]
        public string TotalRelease { get; set; } = "";

        [JsonPropertyName("thisRelease")]
        public string ThisRelease { get; set; } = "";

        [JsonPropertyName("thisRemaining")]
        public string ThisRemaining { get; set; } = "";

        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            // ตรวจสอบวันที่ให้ถูกต้อง
            //if (!DateTime.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            //{
            //    errorMessage = "Date is invalid. The correct format is yyyy-MM-dd.";
            //    return false;
            //}

            // ตรวจสอบว่า TotalSale, TotalFee, TotalRefill, TotalDeposit, TotalDispense, TotalRelease, ThisRelease, ThisRemaining เป็นตัวเลขที่ถูกต้อง
            if (!decimal.TryParse(TotalSale, out _) || !decimal.TryParse(TotalFee, out _) || !decimal.TryParse(TotalRefill, out _) ||
                !decimal.TryParse(TotalDeposit, out _) || !decimal.TryParse(TotalDispense, out _) || !decimal.TryParse(TotalRelease, out _) ||
                !decimal.TryParse(ThisRelease, out _) || !decimal.TryParse(ThisRemaining, out _))
            {
                errorMessage = "One or more fields contain invalid numeric values.";
                return false;
            }

            //// Call mapDetailToFrontend to map details to frontend model.
            //if (!mapDetailToFrontendReport(out errorMessage))
            //{
            //    return false;
            //}

            string ClearDetailDenomerro;
            if (ClearDetailDenom(out ClearDetailDenomerro))
            {
                // Call mapDetailToFrontend to map details to frontend model.
                if (!mapDetailToFrontendReport(out errorMessage))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool mapDetailToFrontendReport(out string errorMessage)
        {
            errorMessage = string.Empty;  // กำหนดค่าเริ่มต้นให้ errorMessage

            try
            {
                // เก็บข้อมูลจาก RequestReportModel ไปที่ DetailDenom
                DetailDenom.date = this.Date;
                DetailDenom.totalSale = this.TotalSale;
                DetailDenom.totalFee = this.TotalFee;
                DetailDenom.totalRefill = this.TotalRefill;
                DetailDenom.totalDeposit = this.TotalDeposit;
                DetailDenom.totalDispense = this.TotalDispense;
                DetailDenom.totalRelease = this.TotalRelease;
                DetailDenom.thisRelease = this.ThisRelease;
                DetailDenom.thisRemaining = this.ThisRemaining;
            }
            catch (Exception ex)
            {
                // หากเกิดข้อผิดพลาดจะกำหนดค่าให้ errorMessage
                errorMessage = "RequestModel to mapDetailToFrontendReport Failed: " + ex.Message;

                // เขียน log ข้อผิดพลาด
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ mapDetailToFrontendReport_RequestModelReport]");
                }

                return false;  // คืนค่า false เมื่อเกิดข้อผิดพลาด
            }

            return true;  // คืนค่า true เมื่อไม่มีข้อผิดพลาด
        }

        private bool ClearDetailDenom(out string errorMessage)
        {
            try
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

                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "RequestModel to mapDetailToFrontend Failed" + ex.Message;

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ mapDetailToFrontend_RequestModel]");
                }

                return false;
            }
        }
    }
}
