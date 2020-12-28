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
	public partial class Intro : Form
	{
		public Intro()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnHKP_Click(object sender, EventArgs e)
		{
			fHKP f = new fHKP();
			this.Hide();
			f.ShowDialog();
			this.Show();
		}
		private void btnLTQ_Click(object sender, EventArgs e)
		{

		}

		private void btnPLXT_Click(object sender, EventArgs e)
		{

		}

		private void btnSchool_Click(object sender, EventArgs e)
		{

		}
		private void btnDic_Click(object sender, EventArgs e)
		{
			fMain f = new fMain();
			this.Hide();
			f.ShowDialog();
			this.Show();
		}
		private void btnSentence_Click(object sender, EventArgs e)
		{
			fTranslate f = new fTranslate();
			this.Hide();
			f.ShowDialog();
			this.Show();
		}
		private void btnOxford_Click(object sender, EventArgs e)
		{
			fOxfordWords f = new fOxfordWords();
			this.Hide();
			f.ShowDialog();
			this.Show();

		}
		private void btnProject_Click(object sender, EventArgs e)
		{
			btnDic.Visible = true;
			btnOxford.Visible = true;
			btnSentence.Visible = true;
			btnHKP.Visible = false;
			btnLTQ.Visible = false;
			btnPLXT.Visible = false;
		}

		private void btHome_Click(object sender, EventArgs e)
		{
			btnDic.Visible = false;
			btnOxford.Visible = false;
			btnSentence.Visible = false;
			btnHKP.Visible = true;
			btnLTQ.Visible = true;
			btnPLXT.Visible = true;
		}


	}
}
