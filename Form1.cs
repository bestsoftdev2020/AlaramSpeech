using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Timers;

namespace AlarmSpeech
{
    public partial class Form1 : Form
    {
        private int num;
        public Form1()
        {
            InitializeComponent();
            num = 0;
        }


        private void OnExitProg(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnStart(object sender, PaintEventArgs e)
        {
            this.Hide();

            System.Timers.Timer mainTimer = new System.Timers.Timer(1000*60);
            mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.SpeechTimer);
            mainTimer.Enabled = true;
        }

        public async void SpeechTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.num++;
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 0;     // -10...10

            string line;
            string[] data;
            bool flag = false;
            System.IO.StreamReader file = new System.IO.StreamReader("Read_It.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] spearator = { "<", ">" };

                data = line.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in data)
                {
                    int n;
                    bool is_num = int.TryParse(str, out n);
                    if (is_num)
                    {
                        int id = Int32.Parse(str);
                        if ((this.num % id) == 0)
                            flag = true;
                        else
                            flag = false;
                    }
                    else if (flag)
                    {
                        synthesizer.SpeakAsync(str);
                    }
                }
            }

            file.Close();
        }
    }
}
