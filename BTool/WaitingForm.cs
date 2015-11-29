using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BTool
{
	public class WaitingForm : Form
	{
		public ProgressBar pbProgressBar;

		public WaitingForm()
		{
			InitializeComponent();
			pbProgressBar.Visible = true;
			pbProgressBar.Step = 1;
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
			this.pbProgressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// pbProgressBar
			// 
			this.pbProgressBar.Cursor = System.Windows.Forms.Cursors.Default;
			this.pbProgressBar.Location = new System.Drawing.Point(12, 12);
			this.pbProgressBar.Name = "pbProgressBar";
			this.pbProgressBar.Size = new System.Drawing.Size(237, 23);
			this.pbProgressBar.Step = 1;
			this.pbProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.pbProgressBar.TabIndex = 0;
			// 
			// WaitingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 53);
			this.Controls.Add(this.pbProgressBar);
			this.Name = "WaitingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Please wait...";
			this.ResumeLayout(false);

		}
		#endregion
	}
}