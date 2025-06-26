namespace App.Net.Configuration
{
    class ImageLoader
    {
        private static Dictionary<string, Image> _images = new Dictionary<string, Image>();

        private static string _imageFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImagesSlip");

        public static void LoadImages()
        {
            try
            {
                if (!Directory.Exists(_imageFolderPath))
                {
                    throw new DirectoryNotFoundException("The specified folder does not exist.");
                }

                string[] extensions = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif", "*.tiff" };

                foreach (var ext in extensions)
                {
                    string[] files = Directory.GetFiles(_imageFolderPath, ext);

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        Image image = Image.FromFile(file);
                        _images[fileName] = image;
                    }
                }
            }
            catch (Exception ex)
            {
                // บันทึกข้อผิดพลาดลงไฟล์
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorLog.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error Time: {DateTime.Now}");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("----------------------------------------[ImageLoader.LoadImages()]");
                }

                // ใช้ MessageBox ใน Windows Forms หรือให้ใช้วิธีอื่นถ้าต้องการ
                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "ImageLoader.LoadImages()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static Image? GetImage(string imageName)
        {
            if (_images.TryGetValue(imageName, out Image? image))
            {
                return image;
            }
            else
            {
                if (_images.TryGetValue("Creatus", out image))
                {
                    return image;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
