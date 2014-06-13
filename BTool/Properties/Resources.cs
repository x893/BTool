using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BTool.Properties
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals((object)Resources.resourceMan, (object)null))
					Resources.resourceMan = new ResourceManager("BTool.Properties.Resources", typeof(Resources).Assembly);
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static Icon btw
		{
			get
			{
				return (Icon)Resources.ResourceManager.GetObject("btw", Resources.resourceCulture);
			}
		}

		internal static Bitmap ti_banner
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("ti_banner", Resources.resourceCulture);
			}
		}

		internal static Icon ti_icon
		{
			get
			{
				return (Icon)Resources.ResourceManager.GetObject("ti_icon", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
