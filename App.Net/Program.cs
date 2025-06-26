namespace App.Net
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // กำหนดค่า Application Configuration
            ApplicationConfiguration.Initialize();

            // แสดง Splash Screen
            using (var splash = new SplashScreen())
            {
                splash.ShowDialog();
            }

            // เปิดฟอร์มหลัก
            Application.Run(new Server_API_Print());
        }
    }
}