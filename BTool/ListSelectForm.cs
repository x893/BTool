using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BTool
{
	public class ListSelectForm : Form
	{
		private Button btnOk;
		private Button btnCancel;
		private ListBox lbDataItems;

		public ListSelectForm()
		{
			InitializeComponent();
		}

		public bool LoadFormData(List<string> dataItems)
		{
			bool flag = true;
			if (dataItems != null)
			{
				lbDataItems.BeginUpdate();
				lbDataItems.Items.Clear();
				foreach (object obj in dataItems)
					lbDataItems.Items.Add(obj);
				if (lbDataItems.Items.Count > 0)
					lbDataItems.SetSelected(0, true);
				lbDataItems.EndUpdate();
			}
			else
				flag = false;
			return flag;
		}

		public string GetUserSelection()
		{
			return lbDataItems.Items[lbDataItems.SelectedIndex].ToString();
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
		}

		private void sysFormClosing(object sender, FormClosingEventArgs e)
		{
		}

		#region Windows Form Designer generated code
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListSelectForm));
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbDataItems = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(24, 135);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(84, 29);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(132, 135);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(84, 29);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// lbDataItems
			// 
			this.lbDataItems.FormattingEnabled = true;
			this.lbDataItems.Items.AddRange(new object[] {
            "88:88:88:88:88:88:88:88",
            "88:88:88:88:88:88:88:88",
            "88:88:88:88:88:88:88:88"});
			this.lbDataItems.Location = new System.Drawing.Point(46, 27);
			this.lbDataItems.Name = "lbDataItems";
			this.lbDataItems.ScrollAlwaysVisible = true;
			this.lbDataItems.Size = new System.Drawing.Size(150, 82);
			this.lbDataItems.TabIndex = 3;
			// 
			// ListSelectForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(235, 189);
			this.Controls.Add(this.lbDataItems);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(243, 220);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(243, 220);
			this.Name = "ListSelectForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Connection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.sysFormClosing);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
