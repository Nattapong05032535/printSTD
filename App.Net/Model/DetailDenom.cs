using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Net.Model
{
    class DetailDenom
    {
        public static string printType { get; set; } = "";
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

        public static bool isCancelSale { get; set; }
        public static string username { get; set; } = "";

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

        public static string date { get; set; } = "";
        public static string totalSale { get; set; } = "";
        public static string totalFee { get; set; } = "";
        public static string totalRefill { get; set; } = "";
        public static string totalDeposit { get; set; } = "";
        public static string totalDispense { get; set; } = "";
        public static string totalRelease { get; set; } = "";
        public static string thisRelease { get; set; } = "";
        public static string thisRemaining { get; set; } = "";
    }

}
