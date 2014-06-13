using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BTool
{
	public class AboutForm : Form
	{
		private IContainer components;
		private Label lblVersion;
		private Button btnOk;

		public AboutForm()
		{
			InitializeComponent();
			lblVersion.Text = FormMain.programTitle + FormMain.programVersion;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblVersion = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(12, 33);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(467, 16);
			this.lblVersion.TabIndex = 0;
			this.lblVersion.Text = "BTool - (Program version here)";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(208, 88);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(491, 131);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblVersion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About BTool";
			this.ResumeLayout(false);

		}
	}
}