using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace wesingchangescore
{
    public partial class Form1 : Form
    {
        string datapath = @"C:\Users\" + Environment.UserName + @"\\AppData\Roaming\Tencent\WeSing\User Data\KSongsDataInfo.xml";
        string data = "";

        public Form1()
        {
            InitializeComponent();
        }

        private ArrayList getmidtextarraylist(string origintext, string lefttext, string righttext)
        {
            int start = 0;
            int left = origintext.IndexOf(lefttext, start);
            int right;
            int length;
            ArrayList re = new ArrayList();
            while (left != -1)
            {
                right = origintext.IndexOf(righttext, left) + righttext.Length;
                length = right - left;
                re.Add(origintext.Substring(left, right - left));
                left = origintext.IndexOf("<Outputs", right);
            }
            return re;
        }

        private string getmidtext(string origintext, string lefttext, string righttext)
        {
            int start = 0;
            int left = origintext.IndexOf(lefttext, start) + lefttext.Length;
            int right;
            string re = "";
            right = origintext.IndexOf(righttext, left);
            re = origintext.Substring(left, right - left);
            return re;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(datapath);
            data = sr.ReadToEnd();
            sr.Close();
            ArrayList datalist = getmidtextarraylist(data, "<Outputs", @"/>");

            for (int i = 0; i < datalist.Count; i++)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.Text = getmidtext(datalist[i].ToString(), "Name=\"", "\"");
                lvItem.SubItems.Add(getmidtext(datalist[i].ToString(), "Score=\"", "\""));
                switch (getmidtext(datalist[i].ToString(), "ScoreRank=\"", "\""))
                {
                    case "0": lvItem.SubItems.Add("D"); break;
                    case "1": lvItem.SubItems.Add("C"); break;
                    case "2": lvItem.SubItems.Add("B"); break;
                    case "3": lvItem.SubItems.Add("A"); break;
                    case "4": lvItem.SubItems.Add("S"); break;
                    case "5": lvItem.SubItems.Add("SS"); break;
                    case "6": lvItem.SubItems.Add("SSS"); break;
                }

                lvItem.SubItems.Add((getmidtext(datalist[i].ToString(), "AllScores=\"", "\"").Split(',').Length - 1).ToString());
                lvItem.SubItems.Add(((getmidtext(datalist[i].ToString(), "AllScores=\"", "\"").Split(',').Length - 1) * 100).ToString());
                listView1.Items.Add(lvItem);
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                ArrayList datalist = getmidtextarraylist(data, "<Outputs", @"/>");
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
                comboBox1.Text = listView1.SelectedItems[0].SubItems[2].Text;
                textBox2.Text = getmidtext(datalist[listView1.SelectedItems[0].Index].ToString(), "AllScores=\"", "\"");
                label4.Text = listView1.SelectedItems[0].Index.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList datalist = getmidtextarraylist(data, "<Outputs", @"/>");
            string scorerank = "";
            string line = datalist[Int32.Parse(label4.Text)].ToString();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "D": scorerank = "0"; break;
                case "C": scorerank = "1"; break;
                case "B": scorerank = "2"; break;
                case "A": scorerank = "3"; break;
                case "S": scorerank = "4"; break;
                case "SS": scorerank = "5"; break;
                case "SSS": scorerank = "6"; break;
            }
            string[] a = textBox2.Text.Split(',');
            int s = 0;
            for (int i = 0; i < a.Length - 1; i++)
            {
                s += Int32.Parse(a[i]);
            }
            line = line.Replace("Score=\"" + getmidtext(datalist[Int32.Parse(label4.Text)].ToString(), "Score=\"", "\""), "Score=\"" + s);
            line = line.Replace("ScoreRank=\"" + getmidtext(datalist[Int32.Parse(label4.Text)].ToString(), "ScoreRank=\"", "\""), "ScoreRank=\"" + scorerank);
            line = line.Replace(getmidtext(datalist[Int32.Parse(label4.Text)].ToString(), "AllScores=\"", "\""), textBox2.Text);
            data = data.Replace(datalist[Int32.Parse(label4.Text)].ToString(), line);
            //MessageBox.Show(data);

            StreamWriter sw = new StreamWriter(datapath);
            sw.Write(data);
            sw.Flush();
            sw.Close();
            listView1.Items.Clear();
            Form1_Load(this, new EventArgs());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Form1_Load(this, new EventArgs());
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = textBox2.Text;
            string[] _s = s.Split(',');
            string res = "";
            int r = 0;
            try
            {
                foreach (string a in _s)
                {
                    r += Int32.Parse(a);
                }
            }
            catch
            { }

            int chazhi = 0;
            if (comboBox1.SelectedIndex == 0)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.6)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.6 - r);
                }
            }
            if (comboBox1.SelectedIndex == 1)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.5)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.5 - r);
                }
            }
            if (comboBox1.SelectedIndex == 2)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.4)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.4 - r);
                }
            }
            if (comboBox1.SelectedIndex == 3)
            {

            }
            if (comboBox1.SelectedIndex == 4)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.7)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.7 - r);
                }
            }
            if (comboBox1.SelectedIndex == 5)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.8)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.8 - r);
                }
            }
            if (comboBox1.SelectedIndex == 6)
            {
                if (r < Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.9)
                {
                    chazhi = (int)(Int32.Parse(listView1.SelectedItems[0].SubItems[4].Text) * 0.9 - r);
                }
            }

            if (chazhi > 0)
            {
                for (int i = 0; i < _s.Length - 1; i++)
                {
                    if (Int32.Parse(_s[i]) < 100)
                    {
                        int _chazhi = Int32.Parse(_s[i]) + chazhi - 100;
                        if (_chazhi > 0)
                        {
                            chazhi = _chazhi;
                            _s[i] = "100";
                        }
                        else
                        {
                            _s[i] = chazhi.ToString();
                            break;
                        }
                    }
                }
            }
            foreach (string a in _s)
            {
                if (a != "")
                { res += a + ","; }
            }
            textBox2.Text = res;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe","https://www.nitian1207.top");
        }
    }
}
