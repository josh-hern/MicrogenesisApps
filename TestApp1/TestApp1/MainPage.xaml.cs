using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TestApp1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //int count = 0;
        void Button_Clicked(object sender, System.EventArgs e)
        {
            Process P = new Process();
            P.StartInfo.FileName = "ping";
            P.StartInfo.Arguments = "-c 3 1.1.1.1 ";
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardOutput = true;

            string readData = "";
            if (P.Start())
                readData = P.StandardOutput.ReadToEnd();
            Console.Write(readData.ToString());

            //count++;
            //((Button)sender).Text = $"Return time: {readData.ToString()}";

            List<string> Lines = new List<string>(readData.Replace("\r\n", "\n").Split('\n'));
            Boolean allLost = true;
            while (Lines.Count > 0 && !Lines[0].StartsWith("---"))
            {
                
                Match M = Regex.Match(Lines[0], @"^[\d]+ bytes from ([^:]+): [^ ]+ ttl=([\d]+) time=([^ ]+) ms");

                if (M != null && M.Success)
                {
                    allLost = false;
                    string IP = M.Groups[1].Value;
                    string TTL = M.Groups[2].Value;
                    string timeStr = M.Groups[3].Value;

                    Console.WriteLine(String.Format("Ping to {0} took {2} ms with a ttl of {1}", IP, TTL, timeStr));
                }

                Lines.RemoveAt(0);
            }
            int number = 0;
            if (Lines[0].StartsWith("---"))
            {
                Console.WriteLine("PINGSTATS-----------------------------------------------------------------");
                Match M = Regex.Match(readData.ToString(), @"(?:([0-9]*)% packet loss)");
                number = 100 - int.Parse(M.Groups[1].Value);//subtracts 100 from amount lost
                ((Button)sender).Text = "The number of successful connections: " + number;
                Console.WriteLine("PINGSTATS-----------------------------------------------------------------");
            }
            Console.WriteLine(allLost);
            if (number == 0)
            {
                ((Button)sender).TextColor = Color.White;
                ((Button)sender).BackgroundColor = Color.Black;
            }
            if (number == 50)
            {
                ((Button)sender).BackgroundColor = Color.Pink;
            }
            if (number == 75)
            {
                ((Button)sender).BackgroundColor = Color.Pink;
            }
            if (number == 100)
            {
                ((Button)sender).BackgroundColor = Color.White;
            }

        }
    }
    
}
