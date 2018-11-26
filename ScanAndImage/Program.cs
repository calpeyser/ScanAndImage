using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAndImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ScanAndImageManager();
            manager.Run();
        }

        class ScanAndImageManager
        {
            private bool written = false;
            private Bitmap currentBitmap;

            public void Run()
            {
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                FilterInfo webcam = videoDevices[0];
                VideoCaptureDevice videoSource = new VideoCaptureDevice(webcam.MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(updateCurrentBitmap);

                while (true)
                {
                    written = false;
                    string input = Console.ReadLine();
                    videoSource.Start();
                    while(true)
                    {
                        if (written)
                        {
                            videoSource.SignalToStop();
                            break;
                        }
                    }
                    saveFile(input);
                }
            }

            void updateCurrentBitmap(object sender, NewFrameEventArgs eventArgs)
            {
                currentBitmap = (Bitmap)eventArgs.Frame.Clone();
                written = true;
            }

            void saveFile(string name)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "..\\" + name + ".png";
                currentBitmap.Save(filePath, ImageFormat.Png);
                Console.WriteLine("Saved image to " + filePath);
            }
        }
    }
}
