using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Globalization;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;


namespace displaying_webcam_in_console
{
    internal class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)]
        private static extern IntPtr GetConsoleHandle();

        private static FilterInfoCollection videoDevices;
        private static VideoCaptureDevice videoDevice;
        private static IntPtr handler;

        static void video_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bitmap = (Bitmap)e.Frame;
            bitmap.Save("test.bmp");
            bitmap.Dispose();

            var graphics = Graphics.FromHwnd(handler);
            var image = System.Drawing.Image.FromFile("test.bmp");
            graphics.DrawImage(image, 50, 50, 250, 250);

            graphics.Dispose();
            image.Dispose();
        }

        static void Main(string[] args)
        {
            handler = GetConsoleHandle();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            for (int i = 0; i < videoDevices.Count; i++)
            {
                Console.WriteLine($"[{i}] {videoDevices[i].Name}");
            }

            Console.Write("Select device (use numbers): ");
            int selectedDevice = int.Parse(Console.ReadLine());
            videoDevice = new VideoCaptureDevice(videoDevices[selectedDevice].MonikerString);

            videoDevice.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoDevice.Start();

            Console.Clear();
            Console.WriteLine("Live preview");
        }
    }
}
