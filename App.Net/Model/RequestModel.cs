using System.Globalization;
using System.Text.Json.Serialization;

namespace App.Net.Model
{
    public class RequestModel
    {
        [JsonPropertyName("printType")]
        public string PrintType { get; set; } = "";

        //P-fix
        [JsonPropertyName("txNo")]
        public string TxNo { get; set; } = "";

        //ไคลแอน
        [JsonPropertyName("reqNo")]
        public string ReqNo { get; set; } = "";

        //ลำดับรายการ
        [JsonPropertyName("seqNo")]
        public string SeqNo { get; set; } = "";

        [JsonPropertyName("customer")]
        public string Customer { get; set; } = "";

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = "";

        [JsonPropertyName("change")]
        public string Change { get; set; } = "";

        [JsonPropertyName("txDate")]
        public string TxDate { get; set; } = "";

        [JsonPropertyName("denom")]
        public List<DenomModel> Denoms { get; set; } = new List<DenomModel>();

        [JsonPropertyName("details")]
        public List<string> Details { get; set; } = new List<string>();

        [JsonPropertyName("showDetail")]
        public bool ShowDetail { get; set; }

        [JsonPropertyName("showDenom")]
        public bool ShowDenom { get; set; }

        [JsonPropertyName("isCancelSale")]
        public bool IsCancelSale { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        [JsonPropertyName("remarkes")]
        public string remarkes { get; set; } = "";

        public class DenomModel
        {
            [JsonPropertyName("denom")]
            public string Denom { get; set; } = "";

            [JsonPropertyName("qty")]
            public string Qty { get; set; } = "";

            [JsonPropertyName("iscashin")]
            public string IsCashIn { get; set; } = "";

            public decimal DenomAsDecimal => decimal.TryParse(Denom, out decimal denom) ? denom : 0;
            public int QtyAsInt => int.TryParse(Qty, out int qty) ? qty : 0;
            public bool IsCashInAsBool => string.Equals(IsCashIn, "true", StringComparison.OrdinalIgnoreCase);
            public bool IsCashOutAsBool => string.Equals(IsCashIn, "false", StringComparison.OrdinalIgnoreCase);
            public bool IsInternal => string.Equals(IsCashIn, "internal", StringComparison.OrdinalIgnoreCase);
        }

        public bool Validate(out string errorMessage)
        {
            try
            {
                // Clear existing
                if (!ClearDetailDenom(out string clearError)) { errorMessage = clearError; return false; }

                errorMessage = string.Empty;

                // Validate PrintType
                // Validate PrintType และ map เป็นภาษาไทย
                switch (PrintType)
                {
                    case "sale":
                        DetailDenom.printTypeTHB = "รายการขาย";
                        break;
                    case "refill":
                        DetailDenom.printTypeTHB = "รายการเติมเงินทอน";
                        break;
                    case "dispense":
                        DetailDenom.printTypeTHB = "รายการถอนเงิน";
                        break;
                    case "deposit":
                        DetailDenom.printTypeTHB = "รายการฝากเงิน";
                        break;
                    case "endofday":
                        DetailDenom.printTypeTHB = "รายการปิดรอบสิ้นวัน";
                        break;
                    case "remove-cassette":
                        DetailDenom.printTypeTHB = "รายการนำเงินออกระหว่างวัน";
                        break;
                    case "exchange-sale":
                        DetailDenom.printTypeTHB = "รายการแลกเงิน-เข้า";
                        break;
                    case "exchange-disp":
                        DetailDenom.printTypeTHB = "รายการแลกเงิน-ออก";
                        break;
                    case "cash-collect":
                        DetailDenom.printTypeTHB = "รายการนำเงินลงกล่อง";
                        break;
                    case "change-deposit":
                        DetailDenom.printTypeTHB = "รายการคืนเงินทอน";
                        break;
                    case "change-dispense":
                        DetailDenom.printTypeTHB = "รายการเบิกเงินทอน";
                        break;
                    case "tip-exchange":
                        DetailDenom.printTypeTHB = "รายการแลกเงินทริป";
                        break;
                    case "receive":
                        DetailDenom.printTypeTHB = "รายการคืนเงิน";
                        break;
                    default:
                        errorMessage = "Invalid PrintType.";
                        return false;
                }


                // Amount
                if (!double.TryParse(Amount, out double parsedAmount))
                {
                    errorMessage = "Invalid amount format.";
                    return false;
                }
                DetailDenom.amount = parsedAmount;

                // Change
                if (!double.TryParse(Change, out double parsedChange))
                {
                    errorMessage = "Invalid change format.";
                    return false;
                }
                DetailDenom.change = parsedChange;

                // TxDate
                if (!DateTime.TryParseExact(TxDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTxDate))
                {
                    if (!DateTime.TryParse(TxDate, out parsedTxDate))
                    {
                        errorMessage = "TxDate invalid. Format yyyy-MM-dd HH:mm:ss.";
                        //return false;
                    }
                    TxDate = parsedTxDate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Denoms
                if (Denoms == null || !Denoms.Any() || Denoms.Any(d => d == null || string.IsNullOrWhiteSpace(d.Denom)))
                {
                    errorMessage = "Denom missing or empty.";
                    return false;
                }

                var denomCheck = new Dictionary<string, bool>();
                foreach (var d in Denoms)
                {
                    if (d.DenomAsDecimal <= 0)
                    {
                        errorMessage = $"Denom '{d.Denom}' invalid.";
                        return false;
                    }

                    if (d.QtyAsInt <= 0)
                    {
                        errorMessage = $"Qty '{d.Qty}' invalid.";
                        return false;
                    }

                    if (!(d.IsCashInAsBool || d.IsCashOutAsBool || d.IsInternal))
                    {
                        errorMessage = $"IsCashIn '{d.IsCashIn}' invalid.";
                        return false;
                    }

                    var key = $"{d.Denom}-{d.IsCashIn}";
                    if (denomCheck.ContainsKey(key))
                    {
                        errorMessage = $"Duplicate Denoms with same status.";
                        return false;
                    }
                    denomCheck[key] = true;
                }

                // ShowDetail / ShowDenom / IsCancelSale
                if (!(ShowDetail == true || ShowDetail == false)) { errorMessage = "ShowDetail invalid."; return false; }
                if (!(ShowDenom == true || ShowDenom == false)) { errorMessage = "ShowDenom invalid."; return false; }
                if (!(IsCancelSale == true || IsCancelSale == false)) { errorMessage = "IsCancelSale invalid."; return false; }

                // Map
                if (!MapDetailToFrontend(out errorMessage)) return false;

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Validate failed: " + ex.Message;
                LogError(ex, "[Validate_RequestModel]");
                return false;
            }
        }

        private bool MapDetailToFrontend(out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;

                DetailDenom.printType = PrintType;
                DetailDenom.txNo = TxNo;
                DetailDenom.reqNo = ReqNo;
                DetailDenom.seqNo = SeqNo;
                DetailDenom.customer = Customer;
                DetailDenom.denom = string.Join(",", Denoms.Select(d => $"{d.Denom}:{d.Qty}"));
                DetailDenom.txDate = TxDate;
                DetailDenom.detail = string.Join(",", Details);
                DetailDenom.ShowDetail = ShowDetail;
                DetailDenom.ShowDenom = ShowDenom;
                DetailDenom.isCancelSale = IsCancelSale;
                DetailDenom.username = Username;
                DetailDenom.remarkes = "- "+remarkes;

                foreach (var denom in Denoms)
                {
                    if (denom.IsCashInAsBool)
                        AddDetailDenom(denom, "cashin");
                    else if (denom.IsCashOutAsBool)
                        AddDetailDenom(denom, "cashout");
                    else if (denom.IsInternal)
                        AddDetailDenom(denom, "internal");
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "MapDetailToFrontend failed: " + ex.Message;
                LogError(ex, "[MapDetailToFrontend_RequestModel]");
                return false;
            }
        }

        private void AddDetailDenom(DenomModel denom, string type)
        {
            int qty = denom.QtyAsInt;
            switch (type.ToLower())
            {
                case "cashin":
                    SetCashin(denom.DenomAsDecimal, qty);
                    break;
                case "cashout":
                    SetCashOut(denom.DenomAsDecimal, qty);
                    break;
                case "internal":
                    SetCashCollect(denom.DenomAsDecimal, qty);
                    break;
            }
        }

        private void SetCashin(decimal denom, int qty)
        {
            switch (denom)
            {
                case 1000: DetailDenom.Cashin1000 = qty; break;
                case 500: DetailDenom.Cashin500 = qty; break;
                case 100: DetailDenom.Cashin100 = qty; break;
                case 50: DetailDenom.Cashin50 = qty; break;
                case 20: DetailDenom.Cashin20 = qty; break;
                case 10: DetailDenom.Cashin10 = qty; break;
                case 5: DetailDenom.Cashin5 = qty; break;
                case 2: DetailDenom.Cashin2 = qty; break;
                case 1: DetailDenom.Cashin1 = qty; break;
                case 0.50m: DetailDenom.Cashin050 = qty; break;
                case 0.25m: DetailDenom.Cashin025 = qty; break;
            }
        }

        private void SetCashOut(decimal denom, int qty)
        {
            switch (denom)
            {
                case 1000: DetailDenom.CashOut1000 = qty; break;
                case 500: DetailDenom.CashOut500 = qty; break;
                case 100: DetailDenom.CashOut100 = qty; break;
                case 50: DetailDenom.CashOut50 = qty; break;
                case 20: DetailDenom.CashOut20 = qty; break;
                case 10: DetailDenom.CashOut10 = qty; break;
                case 5: DetailDenom.CashOut5 = qty; break;
                case 2: DetailDenom.CashOut2 = qty; break;
                case 1: DetailDenom.CashOut1 = qty; break;
                case 0.50m: DetailDenom.CashOut050 = qty; break;
                case 0.25m: DetailDenom.CashOut025 = qty; break;
            }
        }

        private void SetCashCollect(decimal denom, int qty)
        {
            switch (denom)
            {
                case 1000: DetailDenom.CashCollect1000 = qty; break;
                case 500: DetailDenom.CashCollect500 = qty; break;
                case 100: DetailDenom.CashCollect100 = qty; break;
                case 50: DetailDenom.CashCollect50 = qty; break;
                case 20: DetailDenom.CashCollect20 = qty; break;
                case 10: DetailDenom.CashCollect10 = qty; break;
                case 5: DetailDenom.CashCollect5 = qty; break;
                case 2: DetailDenom.CashCollect2 = qty; break;
                case 1: DetailDenom.CashCollect1 = qty; break;
                case 0.50m: DetailDenom.CashCollect050 = qty; break;
                case 0.25m: DetailDenom.CashCollect025 = qty; break;
            }
        }

        private bool ClearDetailDenom(out string errorMessage)
        {
            try
            {
                // CashOut
                DetailDenom.CashOut1000 = 0;
                DetailDenom.CashOut500 = 0;
                DetailDenom.CashOut100 = 0;
                DetailDenom.CashOut50 = 0;
                DetailDenom.CashOut20 = 0;
                DetailDenom.CashOut10 = 0;
                DetailDenom.CashOut5 = 0;
                DetailDenom.CashOut2 = 0;
                DetailDenom.CashOut1 = 0;
                DetailDenom.CashOut050 = 0;
                DetailDenom.CashOut025 = 0;

                // CashIn
                DetailDenom.Cashin1000 = 0;
                DetailDenom.Cashin500 = 0;
                DetailDenom.Cashin100 = 0;
                DetailDenom.Cashin50 = 0;
                DetailDenom.Cashin20 = 0;
                DetailDenom.Cashin10 = 0;
                DetailDenom.Cashin5 = 0;
                DetailDenom.Cashin2 = 0;
                DetailDenom.Cashin1 = 0;
                DetailDenom.Cashin050 = 0;
                DetailDenom.Cashin025 = 0;

                // CashCollect
                DetailDenom.CashCollect1000 = 0;
                DetailDenom.CashCollect500 = 0;
                DetailDenom.CashCollect100 = 0;
                DetailDenom.CashCollect50 = 0;
                DetailDenom.CashCollect20 = 0;
                DetailDenom.CashCollect10 = 0;
                DetailDenom.CashCollect5 = 0;
                DetailDenom.CashCollect2 = 0;
                DetailDenom.CashCollect1 = 0;
                DetailDenom.CashCollect050 = 0;
                DetailDenom.CashCollect025 = 0;

                // Basic info
                DetailDenom.printType = "";
                DetailDenom.printTypeTHB = "";
                DetailDenom.txNo = "";
                DetailDenom.reqNo = "";
                DetailDenom.seqNo = "";
                DetailDenom.customer = "";
                DetailDenom.amount = 0;
                DetailDenom.change = 0;
                DetailDenom.cashin = 0;
                DetailDenom.denom = "";
                DetailDenom.txDate = "";
                DetailDenom.detail = "";
                DetailDenom.ShowDetail = false;
                DetailDenom.ShowDenom = false;
                DetailDenom.isCancelSale = false;
                DetailDenom.username = "";
                DetailDenom.remarkes = "";

                // Totals
                DetailDenom.date = "";
                DetailDenom.totalSale = "";
                DetailDenom.totalReceive = "";
                DetailDenom.totalFee = "";
                DetailDenom.totalRefill = "";
                DetailDenom.totalDeposit = "";
                DetailDenom.totalDispense = "";
                DetailDenom.totalChangeDispense = "";
                DetailDenom.totalChangeDeposit = "";
                DetailDenom.totalExchangeSale = "";
                DetailDenom.totalExchangeDispense = "";
                DetailDenom.totalRelease = "";
                DetailDenom.totalRemove = "";
                DetailDenom.thisRelease = "";
                DetailDenom.thisRemove = "";
                DetailDenom.thisRemaining = "";
                DetailDenom.machineId = "";
                DetailDenom.reqId = "";
                DetailDenom.transactionDate = "";
                DetailDenom.lastEndOfDay = "";
                DetailDenom.transactionCount = 0;

                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "ClearDetailDenom failed: " + ex.Message;
                LogError(ex, "[ClearDetailDenom_RequestModel]");
                return false;
            }
        }


        public decimal CalculateTotalCashIn()
        {
            try
            {
                return Denoms
                    .Where(d => d.IsCashInAsBool)
                    .Sum(d => d.DenomAsDecimal * d.QtyAsInt);
            }
            catch (Exception ex)
            {
                string filePath = Path.Combine(Application.StartupPath, "errorLog.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[CalculateTotalCashIn_RequestModel]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "CalculateTotalCashIn(RequestModel data)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private void LogError(Exception ex, string label)
        {
            try
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine($"----------------------------------------{label}");
                }
            }
            catch { }
        }
    }
}
