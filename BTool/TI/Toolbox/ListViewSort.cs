using System.Collections;
using System.Windows.Forms;

namespace TI.Toolbox
{
	public class ListViewSort : IComparer
	{
		private int columnToSort;
		private SortOrder orderOfSort;
		private CaseInsensitiveComparer objectCompare;

		public int SortColumn
		{
			get
			{
				return columnToSort;
			}
			set
			{
				columnToSort = value;
			}
		}

		public SortOrder Order
		{
			get
			{
				return orderOfSort;
			}
			set
			{
				orderOfSort = value;
			}
		}

		public ListViewSort()
		{
			columnToSort = 0;
			orderOfSort = SortOrder.None;
			objectCompare = new CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			ListViewItem listViewItem1 = (ListViewItem)x;
			ListViewItem listViewItem2 = (ListViewItem)y;
			if (listViewItem1.SubItems.Count - 1 < columnToSort || listViewItem2.SubItems.Count - 1 < columnToSort)
				return 0;
			int num = objectCompare.Compare((object)listViewItem1.SubItems[columnToSort].Text, (object)listViewItem2.SubItems[columnToSort].Text);
			if (orderOfSort == SortOrder.Ascending)
				return num;
			if (orderOfSort == SortOrder.Descending)
				return -num;
			else
				return 0;
		}
	}
}
