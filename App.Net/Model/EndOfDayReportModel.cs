using System.Globalization;
using System.Text.Json.Serialization;

namespace App.Net.Model
{
    public class EndOfDayReportModel
    {
        [JsonPropertyName("printType")]
        public string PrintType { get; set; } = "";

        [JsonPropertyName("transactionDate")]
        public string TransactionDate { get; set; } = "";

        [JsonPropertyName("lastEndOfDay")]
        public string LastEndOfDay { get; set; } = "";

        [JsonPropertyName("totalSale")]
        public string TotalSale { get; set; } = "";

        [JsonPropertyName("totalReceive")]
        public string TotalReceive { get; set; } = "";

        [JsonPropertyName("totalDeposit")]
        public string TotalDeposit { get; set; } = "";

        [JsonPropertyName("totalRefill")]
        public string TotalRefill { get; set; } = "";

        [JsonPropertyName("totalDispense")]
        public string TotalDispense { get; set; } = "";

        [JsonPropertyName("totalChangeDispense")]
        public string TotalChangeDispense { get; set; } = "";

        [JsonPropertyName("totalChangeDeposit")]
        public string TotalChangeDeposit { get; set; } = "";

        [JsonPropertyName("totalExchangeSale")]
        public string TotalExchangeSale { get; set; } = "";

        [JsonPropertyName("totalExchangeDispense")]
        public string TotalExchangeDispense { get; set; } = "";

        [JsonPropertyName("totalRemove")]
        public string TotalRemove { get; set; } = "";

        [JsonPropertyName("thisRemove")]
        public string ThisRemove { get; set; } = "";

        [JsonPropertyName("thisRemaining")]
        public string ThisRemaining { get; set; } = "";

        [JsonPropertyName("machineId")]
        public string MachineId { get; set; } = "";

        [JsonPropertyName("reqId")]
        public string ReqId { get; set; } = "";

        [JsonPropertyName("transactionCount")]
        public int TransactionCount { get; set; }

        // ตรวจสอบค่าทั้งหมดรวมถึงการแปลง double
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                //หากผ่านทั้งหมด ล้างค่าเก่าและแม็ปไป frontend
                string clearError;
                if (ClearDetailDenom(out clearError))
                {
                    if (!MapDetailToFrontendReport(out errorMessage))
                        return false;
                }
                else
                {
                    errorMessage = clearError;
                    return false;
                }

                //ตรวจสอบวันที่
                if (!DateTime.TryParseExact(TransactionDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    errorMessage = "TransactionDate format is invalid. Expected yyyy-MM-dd HH:mm:ss";
                    //return false;
                }

                if (!DateTime.TryParseExact(LastEndOfDay, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    errorMessage = "LastEndOfDay format is invalid. Expected yyyy-MM-dd HH:mm:ss";
                    //return false;
                }

                // ตรวจสอบค่าที่ต้องเป็นตัวเลข double
                var numericFields = new (string fieldName, string value)[]
                {
                    ("totalSale", TotalSale),
                    ("totalReceive", TotalReceive),
                    ("totalDeposit", TotalDeposit),
                    ("totalRefill", TotalRefill),
                    ("totalDispense", TotalDispense),
                    ("totalChangeDispense", TotalChangeDispense),
                    ("totalChangeDeposit", TotalChangeDeposit),
                    ("totalExchangeSale", TotalExchangeSale),
                    ("totalExchangeDispense", TotalExchangeDispense),
                    ("totalRemove", TotalRemove),
                    ("thisRemove", ThisRemove),
                    ("thisRemaining", ThisRemaining)
                };

                foreach (var (fieldName, value) in numericFields)
                {
                    if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    {
                        errorMessage = $"Invalid numeric value for '{fieldName}': \"{value}\"";
                        return false;
                    }
                }

                //ตรวจสอบ TransactionCount
                if (TransactionCount < 0)
                {
                    errorMessage = "TransactionCount cannot be negative.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Validation failed: " + ex.Message;
                LogError(ex, "[Validate_EndOfDayReportModel]");
                return false;
            }
        }

        private bool MapDetailToFrontendReport(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                DetailDenom.printType = this.PrintType;
                DetailDenom.transactionDate = this.TransactionDate;
                DetailDenom.lastEndOfDay = this.LastEndOfDay;
                DetailDenom.totalSale = this.TotalSale;
                DetailDenom.totalReceive = this.TotalReceive;
                DetailDenom.totalDeposit = this.TotalDeposit;
                DetailDenom.totalRefill = this.TotalRefill;
                DetailDenom.totalDispense = this.TotalDispense;
                DetailDenom.totalChangeDispense = this.TotalChangeDispense;
                DetailDenom.totalChangeDeposit = this.TotalChangeDeposit;
                DetailDenom.totalExchangeSale = this.TotalExchangeSale;
                DetailDenom.totalExchangeDispense = this.TotalExchangeDispense;
                DetailDenom.totalRemove = this.TotalRemove;
                DetailDenom.thisRemove = this.ThisRemove;
                DetailDenom.thisRemaining = this.ThisRemaining;
                DetailDenom.machineId = this.MachineId;
                DetailDenom.reqId = this.ReqId;
                DetailDenom.transactionCount = this.TransactionCount;
            }
            catch (Exception ex)
            {
                errorMessage = "MapDetailToFrontendReport failed: " + ex.Message;
                LogError(ex, "[MapDetailToFrontendReport_EndOfDayReportModel]");
                return false;
            }

            return true;
        }

        private bool ClearDetailDenom(out string errorMessage)
        {
            try
            {
                DetailDenom.transactionDate = "";
                DetailDenom.lastEndOfDay = "";
                DetailDenom.totalSale = "";
                DetailDenom.totalReceive = "";
                DetailDenom.totalDeposit = "";
                DetailDenom.totalRefill = "";
                DetailDenom.totalDispense = "";
                DetailDenom.totalChangeDispense = "";
                DetailDenom.totalChangeDeposit = "";
                DetailDenom.totalExchangeSale = "";
                DetailDenom.totalExchangeDispense = "";
                DetailDenom.totalRemove = "";
                DetailDenom.thisRemove = "";
                DetailDenom.thisRemaining = "";
                DetailDenom.machineId = "";
                DetailDenom.reqId = "";
                DetailDenom.transactionCount = 0;

                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "ClearDetailDenom failed: " + ex.Message;
                LogError(ex, "[ClearDetailDenom_EndOfDayReportModel]");
                return false;
            }
        }

        private void LogError(Exception ex, string tag)
        {
            try
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------{tag}");
                }
            }
            catch { /* ignore logging errors */ }
        }
    }
}
