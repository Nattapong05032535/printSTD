namespace App.Net.Model
{
    class DetailDenom
    {
        public static string printType { get; set; } = "";
        public static string printTypeTHB { get; set; } = "";
        public static string txNo { get; set; } = "";
        public static string reqNo { get; set; } = "";
        public static string seqNo { get; set; } = "";
        public static string customer { get; set; } = "";
        public static double amount { get; set; }
        public static double change { get; set; }
        public static double cashin { get; set; }
        public static string denom { get; set; } = "";
        public static string txDate { get; set; } = "";
        public static string detail { get; set; } = "";
        public static bool ShowDetail { get; set; }
        public static bool ShowDenom { get; set; }
        public static bool ShowRemarkes { get; set; } = true;
        public static bool isCancelSale { get; set; }
        public static string username { get; set; } = "";
        public static string remarkes { get; set; } = "";

        public static int Cashin1000 { get; set; }
        public static int Cashin500 { get; set; }
        public static int Cashin100 { get; set; }
        public static int Cashin50 { get; set; }
        public static int Cashin20 { get; set; }
        public static int Cashin10 { get; set; }
        public static int Cashin5 { get; set; }
        public static int Cashin2 { get; set; }
        public static int Cashin1 { get; set; }
        public static int Cashin050 { get; set; }
        public static int Cashin025 { get; set; }

        public static int CashOut1000 { get; set; }
        public static int CashOut500 { get; set; }
        public static int CashOut100 { get; set; }
        public static int CashOut50 { get; set; }
        public static int CashOut20 { get; set; }
        public static int CashOut10 { get; set; }
        public static int CashOut5 { get; set; }
        public static int CashOut2 { get; set; }
        public static int CashOut1 { get; set; }
        public static int CashOut050 { get; set; }
        public static int CashOut025 { get; set; }

        public static int CashCollect1000 { get; set; }
        public static int CashCollect500 { get; set; }
        public static int CashCollect100 { get; set; }
        public static int CashCollect50 { get; set; }
        public static int CashCollect20 { get; set; }
        public static int CashCollect10 { get; set; }
        public static int CashCollect5 { get; set; }
        public static int CashCollect2 { get; set; }
        public static int CashCollect1 { get; set; }
        public static int CashCollect050 { get; set; }
        public static int CashCollect025 { get; set; }

        public static string date { get; set; } = "";
        public static string totalSale { get; set; } = "";
        public static string totalReceive { get; set; } = "";
        public static string totalFee { get; set; } = "";
        public static string totalRefill { get; set; } = "";
        public static string totalDeposit { get; set; } = "";
        public static string totalDispense { get; set; } = "";
        public static string totalChangeDispense { get; set; } = "";
        public static string totalChangeDeposit { get; set; } = "";
        public static string totalExchangeSale { get; set; } = "";
        public static string totalExchangeDispense { get; set; } = "";
        public static string totalRelease { get; set; } = "";
        public static string totalRemove { get; set; } = "";
        public static string thisRelease { get; set; } = "";
        public static string thisRemove { get; set; } = "";
        public static string thisRemaining { get; set; } = "";
        public static string machineId { get; set; } = "";
        public static string reqId { get; set; } = "";
        public static string transactionDate { get; set; } = "";
        public static string lastEndOfDay { get; set; } = "";
        public static int transactionCount { get; set; } = 0;

    }

}
