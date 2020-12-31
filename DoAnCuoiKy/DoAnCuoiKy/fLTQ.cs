using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoAnCuoiKy
{
	public partial class fLTQ : Form
	{
		public fLTQ()
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

		private void btnFace_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.facebook.com/profile.php?id=100009019617956");

		}

		private void btnIns_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.instagram.com/quytthdt/");

		}

		private void btnGit_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.instagram.com/quytthdt/");
		}
	}
}
