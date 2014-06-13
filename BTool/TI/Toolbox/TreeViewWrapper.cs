using System.Windows.Forms;

namespace TI.Toolbox
{
	public class TreeViewWrapper : TreeView
	{
		private const string moduleName = "TreeViewWrapper";

		public event ScrollEventHandler scrollEventHandler;

		protected virtual void OnScroll(ScrollEventArgs e)
		{
			if (!ContainsFocus)
				Focus();
			if (scrollEventHandler == null)
				return;
			scrollEventHandler((object)this, e);
		}

		protected override void WndProc(ref Message message)
		{
			base.WndProc(ref message);
			if (message.Msg != 277 && message.Msg != 276)
				return;
			OnScroll(new ScrollEventArgs((ScrollEventType)(message.WParam.ToInt32() & (int)ushort.MaxValue), 0));
		}
	}
}
