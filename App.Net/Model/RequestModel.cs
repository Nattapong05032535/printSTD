using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace App.Net.Model
{
    public class RequestModel
    {
        [JsonPropertyName("printType")]
        public string PrintType { get; set; } = "";

        [JsonPropertyName("txNo")]
        public string TxNo { get; set; } = "";

        [JsonPropertyName("reqNo")]
        public string ReqNo { get; set; } = "";

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

            public bool IsCashInAsBool => bool.TryParse(IsCashIn, out bool isCashIn) && isCashIn;
        }

        public bool Validate(out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;

                // Validate PrintType
                switch (PrintType)
                {
                    case "sale":
                    case "refill":
                    case "dispense":
                    case "deposit":
                    case "endofday":
                    case "remove-casstte":
                    case "exchange-sale":
                    case "exchange-disp":
                        break;
                    default:
                        errorMessage = "Invalid PrintType.";
                        return false;
                }

                // Validate SeqNo
                //if (!decimal.TryParse(SeqNo, out _))
                //{
                //    errorMessage = "Invalid SeqNo format.";
                //    return false;
                //}

                // Validate and parse Amount as double
                if (double.TryParse(Amount, out double parsedAmount))
                {
                    DetailDenom.amount = parsedAmount;
                }
                else
                {
                    errorMessage = "Invalid amount format for double.";
                    return false;
                }

                // Validate and parse Change as double
                if (double.TryParse(Change, out double parsedChange))
                {
                    DetailDenom.change = parsedChange;
                }
                else
                {
                    errorMessage = "Invalid change format for double.";
                    return false;
                }

                // Validate Date
                //if (!DateTime.TryParseExact(TxDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTxDate))
                //{
                //    if (DateTime.TryParse(TxDate, out parsedTxDate))
                //    {
                //        TxDate = parsedTxDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //    }
                //    else
                //    {
                //        errorMessage = "TxDate is invalid. The correct format is yyyy-MM-dd HH:mm:ss.";
                //        return false;
                //    }
                //}

                // Validate Denominations and their Quantity, Denom, and IsCashIn status
                if (Denoms == null || !Denoms.Any() || Denoms.Any(d => d == null || string.IsNullOrWhiteSpace(d.Denom)))
                {
                    errorMessage = "Denom is missing or empty in the request.";
                    return false;
                }

                var denomCheck = new Dictionary<string, bool>();
                foreach (var denom in Denoms)
                {
                    // Validate Denom is a positive numeric value
                    if (string.IsNullOrWhiteSpace(denom.Denom) || denom.DenomAsDecimal <= 0)
                    {
                        errorMessage = $"Denom '{denom.Denom}' is invalid. It must be a positive numeric value.";
                        return false;
                    }

                    // Validate Qty is greater than zero
                    if (denom.QtyAsInt <= 0)
                    {
                        errorMessage = $"Qty '{denom.Qty}' is invalid. It must be a positive integer.";
                        return false;
                    }

                    // Validate IsCashIn is a valid boolean
                    if (string.IsNullOrWhiteSpace(denom.IsCashIn) || !bool.TryParse(denom.IsCashIn, out _))
                    {
                        errorMessage = $"IsCashIn '{denom.IsCashIn}' is invalid. It must be 'True' or 'False'.";
                        return false;
                    }

                    // Validate no duplicates with the same Denom and IsCashIn status
                    var key = $"{denom.Denom}-{denom.IsCashIn}";
                    if (denomCheck.ContainsKey(key))
                    {
                        errorMessage = $"Duplicate Denoms with the same IsCashIn status (True or False) are not allowed.";
                        return false;
                    }

                    denomCheck[key] = true;
                }

                // Validate ShowDetail (ตรวจสอบว่า ShowDetail เป็น true หรือ false)
                if (!(ShowDetail == true || ShowDetail == false))
                {
                    errorMessage = "ShowDetail must be a boolean value (true or false).";
                    return false;
                }

                // Validate ShowDenom (ตรวจสอบว่า ShowDenom เป็น true หรือ false)
                if (!(ShowDenom == true || ShowDenom == false))
                {
                    errorMessage = "ShowDenom must be a boolean value (true or false).";
                    return false;
                }

                // Validate ShowDenom (ตรวจสอบว่า ShowDenom เป็น true หรือ false)
                if (!(IsCancelSale == true || IsCancelSale == false))
                {
                    errorMessage = "IsCancelSale must be a boolean value (true or false).";
                    return false;
                }

                string ClearDetailDenomerro;
                if (ClearDetailDenom(out ClearDetailDenomerro))
                {
                    // Call mapDetailToFrontend to map details to frontend model.
                    if (!mapDetailToFrontend(out errorMessage))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                //// Call mapDetailToFrontend to map details to frontend model.
                if (!mapDetailToFrontend(out errorMessage))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "RequestModel to Validate Failed" + ex.Message;

                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[Validate_RequestModel]");
                }
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
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[CalculateTotalCashIn_RequestModel]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "CalculateTotalCashIn(RequestModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return 0;
            }
        }

        private bool mapDetailToFrontend(out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;

                //if (string.IsNullOrWhiteSpace(PrintType) || string.IsNullOrWhiteSpace(TxNo) 
                //    || string.IsNullOrWhiteSpace(ReqNo) || string.IsNullOrWhiteSpace(SeqNo)
                //    || string.IsNullOrWhiteSpace(Customer) || string.IsNullOrWhiteSpace(Username))
                //{
                //    errorMessage = "PrintType, TxNo, ReqNo, SeqNo, Customer, or Username cannot be null or empty.";
                //    return false;
                //}

                if (string.IsNullOrWhiteSpace(PrintType) )
                {
                    errorMessage = "PrintType cannot be null or empty.";
                    return false;
                }

                if (CalculateTotalCashIn() is decimal totalCashIn)
                {
                    DetailDenom.cashin = (double)totalCashIn;
                }
                else
                {
                    errorMessage = "Invalid cashin calculation.";
                    return false;
                }

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

                foreach (var denom in Denoms)
                {
                    if (denom.IsCashInAsBool)
                    {
                        AddDetailDenom(denom, true);
                    }
                    else
                    {
                        AddDetailDenom(denom, false);
                    }
                }

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

        private void AddDetailDenom(DenomModel denom, bool isCashIn)
        {
            try
            {
                foreach (var d in Denoms.Where(d => bool.TryParse(d.IsCashIn, out bool result) && result == isCashIn))
                {
                    var quantity = d.QtyAsInt;

                    switch (d.DenomAsDecimal)
                    {
                        case 1000:
                            if (isCashIn) DetailDenom.Cashin1000 = quantity;
                            else DetailDenom.CashOut1000 = quantity;
                            break;

                        case 500:
                            if (isCashIn) DetailDenom.Cashin500 = quantity;
                            else DetailDenom.CashOut500 = quantity;
                            break;

                        case 100:
                            if (isCashIn) DetailDenom.Cashin100 = quantity;
                            else DetailDenom.CashOut100 = quantity;
                            break;

                        case 50:
                            if (isCashIn) DetailDenom.Cashin50 = quantity;
                            else DetailDenom.CashOut50 = quantity;
                            break;

                        case 20:
                            if (isCashIn) DetailDenom.Cashin20 = quantity;
                            else DetailDenom.CashOut20 = quantity;
                            break;

                        case 10:
                            if (isCashIn) DetailDenom.Cashin10 = quantity;
                            else DetailDenom.CashOut10 = quantity;
                            break;

                        case 5:
                            if (isCashIn) DetailDenom.Cashin5 = quantity;
                            else DetailDenom.CashOut5 = quantity;
                            break;

                        case 2:
                            if (isCashIn) DetailDenom.Cashin2 = quantity;
                            else DetailDenom.CashOut2 = quantity;
                            break;

                        case 1:
                            if (isCashIn) DetailDenom.Cashin1 = quantity;
                            else DetailDenom.CashOut1 = quantity;
                            break;

                        case 0.50m:
                            if (isCashIn) DetailDenom.Cashin050 = quantity;
                            else DetailDenom.CashOut050 = quantity;
                            break;

                        case 0.25m:
                            if (isCashIn) DetailDenom.Cashin025 = quantity;
                            else DetailDenom.CashOut025 = quantity;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string filePath = Application.StartupPath + @"\errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ AddDetailDenom_RequestModel]");
                }

                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "AddDetailDenom(RequestModel data) ApiPrinteeCCWEB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ClearDetailDenom(out string errorMessage)
        {
            try
            {
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

                DetailDenom.printType = "";
                DetailDenom.txNo = "";
                DetailDenom.reqNo = "";
                DetailDenom.seqNo = "";
                DetailDenom.customer = "";

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
