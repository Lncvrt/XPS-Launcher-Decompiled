using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace XPS;

public class BrowserForm : Form
{
	private static BrowserForm instance;

	private WebView2 webView;

	private IContainer components;

	public string FormName { get; private set; }

	private BrowserForm(string url, string formname)
	{
		InitializeComponent();
		InitializeBrowser();
		LoadUrl(url);
		SetFormName(formname);
	}

	public static BrowserForm GetInstance(string url, string formname)
	{
		if (instance == null || instance.IsDisposed)
		{
			instance = new BrowserForm(url, formname);
		}
		else
		{
			instance.LoadUrl(url);
			instance.SetFormName(formname);
		}
		return instance;
	}

	private void InitializeBrowser()
	{
		webView = new WebView2
		{
			Dock = DockStyle.Fill
		};
		base.Controls.Add(webView);
		base.StartPosition = FormStartPosition.CenterScreen;
		base.WindowState = FormWindowState.Maximized;
	}

	private async void LoadUrl(string url)
	{
		if (webView.CoreWebView2 == null)
		{
			await webView.EnsureCoreWebView2Async(null);
		}
		webView.CoreWebView2.Navigate(url);
	}

	private void SetFormName(string formname)
	{
		FormName = formname;
		Text = formname;
	}

	public new void Show()
	{
		if (!base.Visible)
		{
			base.Show();
		}
		BringToFront();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XPS.BrowserForm));
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1262, 673);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "BrowserForm";
		base.ResumeLayout(false);
	}
}
