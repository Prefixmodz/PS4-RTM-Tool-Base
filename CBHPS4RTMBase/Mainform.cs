using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBH_WinForm_Theme_Library_NET;
using libdebug;

namespace CBHPS4RTMBase
{
    public partial class Mainform : CrEaTiiOn_Form
    {
        public static PS4DBG PS4;
        ProcessList proclist;
        private int pid = 0;
        public Mainform()
        {
            InitializeComponent();
        }

        private void crEaTiiOn_LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string ThemeLink = "https://github.com/EternalModz/CrEaTiiOn-Brotherhood-Theme-Library-NET";
            System.Diagnostics.Process.Start(ThemeLink);
        }

        private void crEaTiiOn_LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string EternalModzGithubLink = "https://github.com/EternalModz";
            System.Diagnostics.Process.Start(EternalModzGithubLink);
        }

        private void TimerDnT_Tick(object sender, EventArgs e)
        {
            LabelDate.Text = DateTime.Now.ToLongDateString();
            LabelTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            TimerDnT.Start();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                PS4 = new PS4DBG(BoxIP.Text);
                PS4.Connect();
                LabelStatus.Text = "Connected";
                LabelStatus.ForeColor = Color.Green;
                PS4.Notify(222, "Connected");
            }
            catch
            {
                MessageBox.Show("Failed to connect to console make sure u injected PS4Debug payload!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void AttachProcessName(string processName)
        {
            try
            {
                if (pid == 0)
                {
                    string TheProcName = processName;
                    proclist = PS4.GetProcessList();
                    pid = proclist.FindProcess(TheProcName, false).pid;
                }
                else
                {

                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "Could not attach", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            try
            {
                AttachProcessName("eboot.bin");
                LabelStatus.Text = "Connected + Attached";
                LabelStatus.ForeColor = Color.Green;
                PS4.Notify(222, "Attached to current game!");
            }
            catch
            {
                MessageBox.Show("Make sure you have a game running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSetBO4Name_Click(object sender, EventArgs e)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(BoxBO4Name.Text);
            Array.Resize(ref bytes, bytes.Length + 1);
            PS4.WriteMemory(pid, 0x95F2654, bytes);
        }

        private void btnGetBO4Name_Click(object sender, EventArgs e)
        {
            string BO4Name;
            BO4Name = Encoding.ASCII.GetString(PS4.ReadMemory(pid, 0x95F2654, 20));
            BoxBO4Name.Text = BO4Name;
        }

        private void btnSendNoti_Click(object sender, EventArgs e)
        {
            PS4.Notify(222, BoxNotify.Text);
        }
    }
}
