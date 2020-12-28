using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using SpeechLib;

namespace DoAnCuoiKy
{
    public partial class fMain : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static int GetWindowLong(IntPtr hWnd, int index);

        public static bool VerticalScrollBarVisible(Control ctl)
        {
            int style = GetWindowLong(ctl.Handle, -16);
            return (style & 0x200000) != 0;
        }

        public List<String> fWordList = new List<string>();
        long startID, endID;
        public fMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DictType == "EV")
            {
                cbType.SelectedIndex = 0;
            }
            else
            {
                cbType.SelectedIndex = 1;

            }
            toolStripStatusLabel4.Text = DateTime.Now.TimeOfDay.ToString().Split(':')[0].Trim() + ":" + DateTime.Now.TimeOfDay.ToString().Split(':')[1].Trim();
            button1.Visible = richTextBox1.Text != "";
        }

        public List<String> lst = new List<string>();
        public Dictionary<String, String> dictIndex = new Dictionary<string, string>();
        //
        public List<String> lstVA = new List<string>();
        public Dictionary<String, String> dictIndexVA = new Dictionary<string, string>();

        public void LoadIndexFileVA()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists(Application.StartupPath + "/Database/vietanh.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/vietanh.txt");
                while (reader.Peek() >= 0)
                {
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        try
                        {
                            lstVA.Add(s.Split(':')[0].Trim());
                            dictIndexVA.Add(s.Trim().Split(':')[0].Trim(), s.Trim().Split(':')[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                reader.Close();
            }
        }

        public void LoadIndexFile()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists(Application.StartupPath + "/Database/anhviet.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/anhviet.txt");
                while (reader.Peek() >= 0)
                {
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        try
                        {
                            lst.Add(s.Split(':')[0].Trim());
                            dictIndex.Add(s.Trim().Split(':')[0].Trim(), s.Trim().Split(':')[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                reader.Close();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DictType == "EV")
            {
                button1.Visible = richTextBox1.Text != "";

                if (VerticalScrollBarVisible(richTextBox1))
                {
                    button1.Left = this.Width - 81;
                }
                else
                {
                    button1.Left = this.Width - 65; ;

                }
            }
            else
            {
                button1.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (Properties.Settings.Default.DictType == "EV") //E - V dic
            {
                if (textBox1.Text != "")
                {
                    pictureBox2.Visible = true;
                    int j = 0;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].StartsWith(textBox1.Text.Trim().ToLower()))
                        {
                            j = i;
                            break;
                        }

                    }

                    for (int i = j; i < lst.Count; i++)
                    {
                        if (lst[i].Trim().Length >= textBox1.Text.Trim().Length)
                        {
                            if (lst[i].Trim().Substring(0, textBox1.Text.Trim().Length).ToLower() == textBox1.Text.Trim().ToLower())
                            {
                                if (listView1.Items.Count < Properties.Settings.Default.SuggesttedWordMaximum)
                                {
                                    listView1.Items.Add(lst[i].Trim(), 0);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    pictureBox2.Visible = false;
                }
            }
            else //V - E dic
            {
                if (textBox1.Text != "")
                {
                    pictureBox2.Visible = true;
                    int j = 0;
                    for (int i = 0; i < lstVA.Count; i++)
                    {
                        if (lstVA[i].StartsWith(textBox1.Text.Trim().ToLower()))
                        {
                            j = i;
                            break;
                        }

                    }

                    for (int i = j; i < lstVA.Count; i++)
                    {
                        if (lstVA[i].Trim().Length >= textBox1.Text.Trim().Length)
                        {
                            if (lstVA[i].Trim().Substring(0, textBox1.Text.Trim().Length).ToLower() == textBox1.Text.Trim().ToLower())
                            {
                                if (listView1.Items.Count < Properties.Settings.Default.SuggesttedWordMaximum)
                                {
                                    listView1.Items.Add(lstVA[i].Trim(), 0);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    pictureBox2.Visible = false;
                }

            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadfWordList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                toolStripStatusLabel1.Text = listView1.SelectedItems[0].Text;
                //
                if (Properties.Settings.Default.DoubleClickTranslate == false)
                {
                    if (File.Exists(Application.StartupPath + "/Database/anhviet_nghia.txt"))
                    {
                        StreamReader reader = new StreamReader(Application.StartupPath + "/Database/anhviet_nghia.txt");
                        long i = -1;
                        startID = long.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[0].Trim());
                        endID = startID + long.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[1].Trim());
                        //MessageBox.Show(startID.ToString() + ", " + endID.ToString());
                        richTextBox1.Text = "";
                        while (reader.Peek() >= 0)
                        {
                            i += 1;
                            string s = reader.ReadLine();
                            if ((i >= startID) && (i <= endID))
                            {
                                richTextBox1.AppendText(s + Environment.NewLine);
                            }

                        }
                        reader.Close();
                        //Highlight
                        richTextBox1.Text = richTextBox1.Text.Trim().Replace("*", "•").Replace("+", Environment.NewLine + "+").Replace("-", "- ").Replace("=", "*  ");
                        richTextBox1.Text = richTextBox1.Text.Substring(1, richTextBox1.Text.Length - 1);
                        HighLight();
                    }
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "No word selected";
                richTextBox1.Text = "";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
        }

        public void HighLight()
        {
            string word;
            if (Properties.Settings.Default.DictType == "EV")
            {
                word = richTextBox1.Lines[0].Replace("/", "[").Substring(0, richTextBox1.Lines[0].Length - 1) + "]";
            }
            else
            {
                word = richTextBox1.Lines[0].Trim();
            }

            string temp = "";
            for (int k = 1; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].Trim() != "")
                {
                    temp += Environment.NewLine + richTextBox1.Lines[k].Trim();
                }
            }
            richTextBox1.Text = word + temp;

            //
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = listView1.SelectedItems[0].Text.Trim().Length;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);
            //
            //richTextBox1.SelectionStart = "";
            for (int k = 0; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].StartsWith("•"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

                }
                else if (richTextBox1.Lines[k].StartsWith("-"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.TranslatedWordColor;

                }
                else if (richTextBox1.Lines[k].StartsWith("*"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.WordTypeColor;

                }
            }
            richTextBox1.Refresh();

        }

        public Point GetLineRange(int ID)
        {
            int i = 0;
            for (int k = 0; k < ID; k++)
            {
                i += richTextBox1.Lines[k].Length;
            }

            return new Point(i + ID + 1, richTextBox1.Lines[ID].Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Create a TTS voice and speak.
            try
            {
                Voice = new SpVoice();
                Voice.Voice = Voice.GetVoices().Item(Properties.Settings.Default.VoiceType);
                Voice.Volume = Properties.Settings.Default.Volume;
                String word = richTextBox1.Text.Split('[')[0].Trim();
                Voice.Speak(word, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
            }
            catch
            {
                MessageBox.Show("Speak error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


		List<String> temp = new List<string>();

        public SpVoice Voice = new SpVoice();
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel4.Text = DateTime.Now.TimeOfDay.ToString().Split(':')[0].Trim() + ":" + DateTime.Now.TimeOfDay.ToString().Split(':')[1].Trim();
        }
   

        public void LoadfWordList()
        {
            if (File.Exists(Application.StartupPath + "/Favorite Words/Favorite Words.txt"))
            {
                fWordList.Clear();
                StreamReader reader = new StreamReader(Application.StartupPath + "/Favorite Words/Favorite Words.txt");
                while (reader.Peek() >= 0)
                {
                    fWordList.Add(reader.ReadLine());
                }
                reader.Close();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DictType == "EV")
            {
                if (Properties.Settings.Default.DoubleClickTranslate)
                {
                    if (File.Exists(Application.StartupPath + "/Database/anhviet_nghia.txt"))
                    {
                        StreamReader reader = new StreamReader(Application.StartupPath + "/Database/anhviet_nghia.txt");
                        long i = -1;
                        startID = long.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[0].Trim());
                        endID = startID + long.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[1].Trim());
                        //MessageBox.Show(startID.ToString() + ", " + endID.ToString());
                        richTextBox1.Text = "";
                        while (reader.Peek() >= 0)
                        {
                            i += 1;
                            string s = reader.ReadLine();
                            if ((i >= startID) && (i <= endID))
                            {
                                richTextBox1.AppendText(s + Environment.NewLine);
                            }

                        }
                        reader.Close();
                        //Highlight
                        richTextBox1.Text = richTextBox1.Text.Trim().Replace("*", "•").Replace("+", Environment.NewLine + "+").Replace("-", "- ").Replace("=", "*  ");
                        richTextBox1.Text = richTextBox1.Text.Substring(1, richTextBox1.Text.Length - 1);
                        HighLight();
                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.DoubleClickTranslate)
                {
                    if (File.Exists(Application.StartupPath + "/Database/vietanh_nghia.txt"))
                    {
                        StreamReader reader = new StreamReader(Application.StartupPath + "/Database/vietanh_nghia.txt");
                        long i = -1;
                        startID = long.Parse(dictIndexVA[listView1.SelectedItems[0].Text].Split(',')[0].Trim());
                        endID = startID + long.Parse(dictIndexVA[listView1.SelectedItems[0].Text].Split(',')[1].Trim());
                        //MessageBox.Show(startID.ToString() + ", " + endID.ToString());
                        richTextBox1.Text = "";
                        while (reader.Peek() >= 0)
                        {
                            i += 1;
                            string s = reader.ReadLine();
                            if ((i >= startID) && (i <= endID))
                            {
                                richTextBox1.AppendText(s + Environment.NewLine);
                            }

                        }
                        reader.Close();
                        //Highlight
                        richTextBox1.Text = richTextBox1.Text.Trim().Replace("*", "•").Replace("+", Environment.NewLine + "+").Replace("-", "- ").Replace("=", "*  ");
                        richTextBox1.Text = richTextBox1.Text.Substring(1, richTextBox1.Text.Length - 1);
                        HighLight();
                    }
                }

            }
        }

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			textBox1.Focus();
			System.Diagnostics.Process.Start("osk.exe");
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{
            System.Diagnostics.Process.Start("https://translate.google.com/?hl=vi");
        }

		private void pictureBox3_Click(object sender, EventArgs e)
		{
            this.Close();
		}

		private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (cbType.SelectedIndex == 0)
            {
                Properties.Settings.Default.DictType = "EV";
                if (dictIndex.Count == 0)
                {
                    LoadIndexFile();
                }
            }
            else
            {
                Properties.Settings.Default.DictType = "VE";
                if (dictIndexVA.Count == 0)
                {
                    LoadIndexFileVA();
                }
            }
            textBox1.Select();
        }
    }
}
