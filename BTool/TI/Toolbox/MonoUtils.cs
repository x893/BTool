using System.Drawing;
using System.Windows.Forms;

namespace TI.Toolbox
{
	internal class MonoUtils
	{
		private SharedObjects sharedObjs = new SharedObjects();
		private const string moduleName = "MonoUtils";

		public bool SetMaximumSize(Form form)
		{
			bool flag = true;
			if (sharedObjs.IsMonoRunning())
				form.MaximumSize = new Size(form.Size.Width, form.Size.Height);
			return flag;
		}
	}
}
