using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Net.Configuration
{
    internal class Setting
    {
        public static string CurrentDate => DateTime.Now.ToString("MM-dd-yyyy");

        public static string CurrentTime => DateTime.Now.ToShortTimeString();

        public static string hostURL { get; set; } = "localhost";

        public static string portURL { get; set; } = "8000";

        public static bool DemoSetting { get; set; } = false;

        public static int printQueueNo { get; set; } = 90;

        public static int printMargin { get; set; } = 90;

        public static string printCompanyName { get; set; } = "Default";

        public static string printMachineName { get; set; } = "Default";

        public static string imageSlip { get; set; } = "default";

        public static string printBranchCode { get; set; } = "Default";

        public static string printBranchName { get; set; } = "Default";

        public static string printAddress1 { get; set; } = "Default";

        public static string printAddress2 { get; set; } = "null";

        public static string printAddress3 { get; set; } = "null";

        public static string printTaxId { get; set; } = "Default";

        public static string printer { get; set; } = "";

        public static bool salePrint { get; set; } = true;

        public static bool refillPrint { get; set; } = true;

        public static bool dispensePrint { get; set; } = true;

        public static bool dispositPrint { get; set; } = true;

        public static bool endofdayPrint { get; set; } = true;

        public static bool removePrint { get; set; } = true;

        public static bool exchangeSalePrint { get; set; } = true;

        public static bool exchangeDispPrint { get; set; } = true;
    }
}
