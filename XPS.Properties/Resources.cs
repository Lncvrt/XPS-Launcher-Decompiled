using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace XPS.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("XPS.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap Checked => (Bitmap)ResourceManager.GetObject("Checked", resourceCulture);

	internal static Bitmap Dashboard => (Bitmap)ResourceManager.GetObject("Dashboard", resourceCulture);

	internal static Bitmap Discord => (Bitmap)ResourceManager.GetObject("Discord", resourceCulture);

	internal static Bitmap Exit => (Bitmap)ResourceManager.GetObject("Exit", resourceCulture);

	internal static Bitmap logoIco => (Bitmap)ResourceManager.GetObject("logoIco", resourceCulture);

	internal static Bitmap logoIcoTransparent => (Bitmap)ResourceManager.GetObject("logoIcoTransparent", resourceCulture);

	internal static Bitmap logoPng => (Bitmap)ResourceManager.GetObject("logoPng", resourceCulture);

	internal static Bitmap logoPngTransparent => (Bitmap)ResourceManager.GetObject("logoPngTransparent", resourceCulture);

	internal static Bitmap Settings => (Bitmap)ResourceManager.GetObject("Settings", resourceCulture);

	internal static Bitmap Twitch => (Bitmap)ResourceManager.GetObject("Twitch", resourceCulture);

	internal static Bitmap Twitter => (Bitmap)ResourceManager.GetObject("Twitter", resourceCulture);

	internal static Bitmap Unchecked => (Bitmap)ResourceManager.GetObject("Unchecked", resourceCulture);

	internal static Bitmap Website => (Bitmap)ResourceManager.GetObject("Website", resourceCulture);

	internal static Bitmap YouTube => (Bitmap)ResourceManager.GetObject("YouTube", resourceCulture);

	internal Resources()
	{
	}
}
