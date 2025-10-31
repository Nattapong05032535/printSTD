namespace App.Net
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            string iconPathbar = Path.Combine(Application.StartupPath, "iconBar", "statusbar.ico");

            if (File.Exists(iconPathbar))
            {
                this.Icon = new Icon(iconPathbar);
            }

            timer1.Interval = 2500; // ตั้งเวลา 3 วินาที
            timer1.Start(); // เริ่มต้น Timer เมื่อเปิดฟอร์ม
        }

    }
}
