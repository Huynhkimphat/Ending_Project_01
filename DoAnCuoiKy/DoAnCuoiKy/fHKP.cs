﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoAnCuoiKy
{
	public partial class fHKP : Form
	{
		public fHKP()
		{
			InitializeComponent();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnAbout_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.facebook.com/huynhkimphat2708");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.instagram.com/hkimphat2012/");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/Huynhkimphat");
		}
	}
}
