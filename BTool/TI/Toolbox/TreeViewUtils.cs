using System.Windows.Forms;

namespace TI.Toolbox
{
	public class TreeViewUtils
	{
		private static bool m_recursiveDestroy;

		static TreeViewUtils()
		{
		}

		public bool TreeNodeTextSearchAndDestroy(TreeNode treeNode, string target)
		{
			m_recursiveDestroy = false;
			return RecursiveTextSearchAndDestroy(treeNode, target);
		}

		public bool TextViewSearchAndDestroy(TreeView treeView, string target)
		{
			bool flag = false;
			m_recursiveDestroy = false;
			foreach (TreeNode treeNode in treeView.Nodes)
			{
				flag = RecursiveTextSearchAndDestroy(treeNode, target);
				if (flag)
					break;
			}
			return flag;
		}

		private bool RecursiveTextSearchAndDestroy(TreeNode treeNode, string target)
		{
			bool flag1 = false;
			if (treeNode.Text == target)
			{
				bool flag2 = true;
				m_recursiveDestroy = true;
				return flag2;
			}
			else
			{
				foreach (TreeNode treeNode1 in treeNode.Nodes)
				{
					if (treeNode1 != null)
					{
						flag1 = RecursiveTextSearchAndDestroy(treeNode1, target);
						if (flag1 && m_recursiveDestroy)
						{
							treeNode.Remove();
							m_recursiveDestroy = false;
							break;
						}
					}
				}
				return flag1;
			}
		}

		public void RemoveNameFromTree(TreeView treeView, string name)
		{
			foreach (TreeNode treeNode in treeView.Nodes)
				if (treeNode.Name == name)
					treeNode.Remove();
		}

		public void RemoveTextFromTree(TreeView treeView, string text)
		{
			foreach (TreeNode treeNode in treeView.Nodes)
				if (treeNode != null && treeNode.Text == text)
					treeNode.Remove();
		}

		public void ClearSelectedNode(TreeView treeView)
		{
			treeView.SelectedNode = (TreeNode)null;
		}
	}
}
