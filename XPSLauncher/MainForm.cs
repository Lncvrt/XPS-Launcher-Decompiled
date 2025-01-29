using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using XPS;
using XPS.Properties;

namespace XPSLauncher;

public class MainForm : Form
{
	private static readonly HttpClient client = new HttpClient();

	private static readonly string currentVersion = "2.6.1";

	private PrivateFontCollection privateFonts = new PrivateFontCollection();

	private Dictionary<string, bool> downloadingVersions = new Dictionary<string, bool>
	{
		{ "2.2", false },
		{ "2.1", false },
		{ "2.0", false },
		{ "1.9", false }
	};

	private Dictionary<string, bool> errorVersions = new Dictionary<string, bool>
	{
		{ "2.2", false },
		{ "2.1", false },
		{ "2.0", false },
		{ "1.9", false }
	};

	private Dictionary<string, bool> launcheQueue = new Dictionary<string, bool>
	{
		{ "2.2", false },
		{ "2.1", false },
		{ "2.0", false },
		{ "1.9", false }
	};

	private static bool settingsOpen = false;

	private static bool settingCloseOnLoad = false;

	private static bool settingAllowMultipleInstances = false;

	private IContainer components;

	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Label label1;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	private PictureBox pictureBox4;

	private PictureBox pictureBox5;

	private PictureBox pictureBox6;

	private PictureBox pictureBox7;

	private PictureBox pictureBox8;

	private Button button5;

	private Button button6;

	private Label label2;

	private Panel panel1;

	private Label label3;

	private Label label4;

	private Panel panel2;

	private Label label5;

	private Panel panel3;

	private Label label6;

	private Panel panel4;

	private Button button7;

	private Button button8;

	private PictureBox pictureBox9;

	private Label label7;

	private Label label8;

	private PictureBox pictureBox10;

	private PictureBox pictureBox11;

	private PictureBox pictureBox12;

	private PictureBox pictureBox13;

	private Label label9;

	private Button button9;

	public MainForm()
	{
			if (!VerifiedPath())
			{
				MessageBox.Show("XPS failed to launch due to incorrect path detected. Uninstall XPS from known apps and then reinstall XPS.\n\nYou will need to move GDPS files manually if you want them. Your save files will be safe the entire time.", "Failed to verify path", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
			}
			InitializeComponent();
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			pictureBox1.SendToBack();
			ConvertOnLoad();
			LoadFontFromFile();
			CheckVersion();
			CheckDownloaded();
	}

	private void LoadFontFromFile()
	{
		string filename = "Pusab.ttf";
		privateFonts.AddFontFile(filename);
		button1.Font = new Font(privateFonts.Families[0], 17.5f);
		button2.Font = new Font(privateFonts.Families[0], 17.5f);
		button3.Font = new Font(privateFonts.Families[0], 17.5f);
		button4.Font = new Font(privateFonts.Families[0], 17.5f);
		button5.Font = new Font(privateFonts.Families[0], 15.75f);
		button6.Font = new Font(privateFonts.Families[0], 12.5f);
		button7.Font = new Font(privateFonts.Families[0], 14.5f);
		button8.Font = new Font(privateFonts.Families[0], 17.5f);
		button9.Font = new Font(privateFonts.Families[0], 13.5f);
		label1.Font = new Font(privateFonts.Families[0], 15.75f);
		label2.Font = new Font(privateFonts.Families[0], 20f);
		label3.Font = new Font(privateFonts.Families[0], 13.5f);
		label4.Font = new Font(privateFonts.Families[0], 13.5f);
		label5.Font = new Font(privateFonts.Families[0], 13.5f);
		label6.Font = new Font(privateFonts.Families[0], 13.5f);
		label7.Font = new Font(privateFonts.Families[0], 15.75f);
		label8.Font = new Font(privateFonts.Families[0], 15.75f);
		label9.Font = new Font(privateFonts.Families[0], 13.5f);
		label9.Text = "Launcher v" + Regex.Replace(currentVersion, "(\\.0)+$", "");
	}

	private void load22(object sender, EventArgs e)
	{
		launchGDPS("2.2");
	}

	private void load21(object sender, EventArgs e)
	{
		launchGDPS("2.1");
	}

	private void load20(object sender, EventArgs e)
	{
		launchGDPS("2.0");
	}

	private void load19(object sender, EventArgs e)
	{
		launchGDPS("1.9");
	}

	private void OpenUrl(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show("Failed to open URL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void launchGDPS(string version, bool forceOpen = false)
	{
		if (!button1.Visible || !button2.Visible || !button3.Visible || !button4.Visible)
		{
			return;
        }
		MessageBox.Show("Can't launch GDPS versions", "Can't launch GDPS versions", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
		string executionPath = GetExecutionPath();
		string text = "";
		string path = Path.Combine(executionPath, "gdps", "1.9", "XPS-Proxy.exe");
		if (Control.ModifierKeys == Keys.Shift)
		{
			ResetGDPS(version);
		}
		else
		{
			if (launcheQueue[version])
			{
				return;
			}
			if (downloadingVersions[version])
			{
				if (MessageBox.Show("Version " + version + " is currently being downloaded. Would you like to add it to the launch queue?", "Downloading " + version, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes && downloadingVersions[version])
				{
					launcheQueue[version] = true;
				}
				return;
			}
			if (errorVersions[version])
			{
				MessageBox.Show("Version " + version + " had an issue while downloading and cannot be launched. Please create a support ticket in our Discord server for assistance.", "Error downloading " + version, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			switch (version)
			{
			default:
				return;
			case "2.2":
				text = Path.Combine(executionPath, "gdps", "2.2", "XPS.exe");
				break;
			case "2.1":
				text = Path.Combine(executionPath, "gdps", "2.1", "XPS.exe");
				break;
			case "2.0":
				text = Path.Combine(executionPath, "gdps", "2.0", "XPS.exe");
				break;
			case "1.9":
				text = Path.Combine(executionPath, "gdps", "1.9", "XPS.exe");
				break;
			}
			if (File.Exists(text) && !downloadingVersions[version] && !errorVersions[version])
			{
				if (settingAllowMultipleInstances || !IsProcessOpen(text) || forceOpen)
				{
					StartProcess(text);
					if (version == "1.9" && File.Exists(path))
					{
						StartProcess(path);
					}
					if (settingCloseOnLoad && !downloadingVersions["2.2"] && !downloadingVersions["2.1"] && !downloadingVersions["2.0"] && !downloadingVersions["1.9"])
					{
						Environment.Exit(0);
					}
				}
				else if (MessageBox.Show("XPS is already running. Would you like to force open XPS?\n\nContinuing can result in save data loss.", "XPS already open", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
				{
					launchGDPS(version, forceOpen: true);
				}
			}
			else
			{
				MessageBox.Show("Error loading version " + version + ". Create a support ticket in the Discord and we will try to help you.", "Error loading " + version, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void ResetGDPS(string version, bool versionOverride = false)
	{
		if (downloadingVersions[version])
		{
			return;
		}
		if (versionOverride)
		{
			downloadingVersions[version] = true;
			string path = Path.Combine(GetExecutionPath(), "gdps", version);
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
			DownloadAndExtractFiles(version);
		}
		else if (MessageBox.Show("Are you sure you would like to reset " + version + "? This will delete all game files and replace them with fresh ones. This will not affect save files.", "Reset keybind pressed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			downloadingVersions[version] = true;
			string path2 = Path.Combine(GetExecutionPath(), "gdps", version);
			if (Directory.Exists(path2))
			{
				Directory.Delete(path2, recursive: true);
			}
			DownloadAndExtractFiles(version);
		}
	}

	private void DiscordButton(object sender, EventArgs e)
	{
		OpenUrl("https://xps.lncvrt.xyz/discord");
	}

	private void TwitterButton(object sender, EventArgs e)
	{
		OpenUrl("https://xps.lncvrt.xyz/twitter");
	}

	private void YoutubeButton(object sender, EventArgs e)
	{
		OpenUrl("https://xps.lncvrt.xyz/youtube");
	}

	private void TwitchButton(object sender, EventArgs e)
	{
		OpenUrl("https://xps.lncvrt.xyz/twitch");
	}

	private void WebsiteButton(object sender, EventArgs e)
	{
		BrowserForm.GetInstance("https://xps.lncvrt.xyz", "XPS Website").Show();
	}

	private void DashboardButton(object sender, EventArgs e)
	{
		BrowserForm.GetInstance("https://xps.lncvrt.xyz/dashboard", "XPS Dashboard").Show();
	}

	private void SettingsButton(object sender, EventArgs e)
	{
		ToggleSettings();
	}

	private void ExitButton(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void RestartButton(object sender, EventArgs e)
	{
		Application.Restart();
		Environment.Exit(0);
	}

	private void ResetConfigButton(object sender, EventArgs e)
	{
		ResetConfig();
		MessageBox.Show("XPS Launcher has been reset to the default settings!", "Successfully reset XPS Launcher", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private void DarkThemeButton(object sender, EventArgs e)
	{
		WriteConfig("theme", 0);
		LoadTheme();
	}

	private void AmoledThemeButton(object sender, EventArgs e)
	{
		WriteConfig("theme", 1);
		LoadTheme();
	}

	private void PurpleThemeButton(object sender, EventArgs e)
	{
		WriteConfig("theme", 2);
		LoadTheme();
	}

	private void RedThemeButton(object sender, EventArgs e)
	{
		WriteConfig("theme", 3);
		LoadTheme();
	}

	private void OpenAppDataButton(object sender, EventArgs e)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XPS");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		try
		{
			Process.Start("explorer.exe", text);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error opening the XPS folder. Create a support ticket in the Discord and we will try to help you.\n\nError message: " + ex.Message, "Error opening XPS folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void OpenAppFolderButton(object sender, EventArgs e)
	{
		string executionPath = GetExecutionPath();
		try
		{
			Process.Start("explorer.exe", executionPath);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error opening the XPS folder. Create a support ticket in the Discord and we will try to help you.\n\nError message: " + ex.Message, "Error opening XPS folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async void CheckVersion()
	{
		return;
		HttpClient client = new HttpClient();
		try
		{
			_ = 1;
			try
			{
				string text = "https://xps.lncvrt.xyz/getLatestWindowsVersion.php";
				HttpResponseMessage obj = await client.GetAsync(text);
				obj.EnsureSuccessStatusCode();
				if ((await obj.Content.ReadAsStringAsync()).Trim() != currentVersion)
				{
					button1.UseWaitCursor = false;
					button2.UseWaitCursor = false;
					button3.UseWaitCursor = false;
					button4.UseWaitCursor = false;
					button5.UseWaitCursor = false;
					button6.UseWaitCursor = false;
					button1.Cursor = Cursors.No;
					button2.Cursor = Cursors.No;
					button3.Cursor = Cursors.No;
					button4.Cursor = Cursors.No;
					button5.Cursor = Cursors.No;
					button6.Cursor = Cursors.No;
					MessageBox.Show("New version released! Click \"OK\" to download the update", "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					OpenUrl("https://xps.lncvrt.xyz/download/windows");
				}
				else
				{
					button1.UseWaitCursor = false;
					button2.UseWaitCursor = false;
					button3.UseWaitCursor = false;
					button4.UseWaitCursor = false;
					button5.UseWaitCursor = false;
					button6.UseWaitCursor = false;
					button1.Cursor = Cursors.Hand;
					button2.Cursor = Cursors.Hand;
					button3.Cursor = Cursors.Hand;
					button4.Cursor = Cursors.Hand;
					button5.Cursor = Cursors.Hand;
					button6.Cursor = Cursors.Hand;
					button1.Click += load22;
					button2.Click += load21;
					button3.Click += load20;
					button4.Click += load19;
					button5.Click += OpenAppFolderButton;
					button6.Click += OpenAppDataButton;
					base.KeyPreview = true;
					base.KeyPress += MainForm_KeyPress;
				}
			}
			catch
			{
				MessageBox.Show("Error checking version. Check your internet connection or try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		finally
		{
			((IDisposable)client)?.Dispose();
		}
	}

	private void CheckDownloaded()
	{
		return;
		string executionPath = GetExecutionPath();
		string[] array = new string[4] { "2.2", "2.1", "2.0", "1.9" };
		foreach (string text in array)
		{
			string path = Path.Combine(executionPath, "gdps", text);
			string path2 = Path.Combine(executionPath, "gdps", text, "XPS.exe");
			if (!Directory.Exists(path) || !File.Exists(path2))
			{
				downloadingVersions[text] = true;
				Directory.CreateDirectory(path);
				DownloadAndExtractFiles(text);
			}
		}
	}

	private async void DownloadAndExtractFiles(string version)
	{
		return;
		string executionPath = GetExecutionPath();
		string zipPath = Path.Combine(executionPath, "pkg-" + version + ".zip");
		string extractPath = Path.Combine(executionPath, "gdps", version);
		string text = "https://xps.lncvrt.xyz/download/files/packages/win-" + version + ".zip";
		try
		{
			HttpClient client = new HttpClient();
			try
			{
				HttpResponseMessage val = await client.GetAsync(text, (HttpCompletionOption)1);
				if (val.IsSuccessStatusCode)
				{
					using (FileStream fileStream = File.Create(zipPath))
					{
						await val.Content.CopyToAsync((Stream)fileStream);
					}
					if (Directory.Exists(extractPath))
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(extractPath);
						FileInfo[] files = directoryInfo.GetFiles();
						for (int i = 0; i < files.Length; i++)
						{
							files[i].Delete();
						}
						DirectoryInfo[] directories = directoryInfo.GetDirectories();
						for (int i = 0; i < directories.Length; i++)
						{
							directories[i].Delete(recursive: true);
						}
					}
					ZipFile.ExtractToDirectory(zipPath, extractPath);
					if (File.Exists(zipPath))
					{
						File.Delete(zipPath);
					}
					downloadingVersions[version] = false;
					errorVersions[version] = false;
					if (launcheQueue[version])
					{
						launcheQueue[version] = false;
						launchGDPS(version);
					}
					return;
				}
				errorVersions[version] = true;
				downloadingVersions[version] = false;
				if (Directory.Exists(extractPath))
				{
					Directory.Delete(extractPath);
				}
				if (File.Exists(zipPath))
				{
					File.Delete(zipPath);
				}
				MessageBox.Show("Error downloading files for version " + version + ". Create a support ticket in the Discord and we will try to help you.", "Error downloading " + version, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			errorVersions[version] = true;
			downloadingVersions[version] = false;
			if (Directory.Exists(extractPath))
			{
				Directory.Delete(extractPath);
			}
			if (File.Exists(zipPath))
			{
				File.Delete(zipPath);
			}
			MessageBox.Show("Error downloading files for version " + version + ". Create a support ticket in the Discord and we will try to help you.\n\nError message: " + ex.Message, "Error downloading " + version, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private static string GetExecutionPath()
	{
		return AppDomain.CurrentDomain.BaseDirectory;
	}

	private static void StartProcess(string path)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = path,
				WorkingDirectory = Path.GetDirectoryName(path),
				UseShellExecute = false
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error launching that GDPS version. Create a support ticket in the Discord and we will try to help you.\n\nError message: " + ex.Message, "Error laucnhing GDPS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private static bool VerifiedPath()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		if (directoryName.Equals("C:\\Program Files (x86)\\Xytrtiza\\XPS", StringComparison.OrdinalIgnoreCase) || directoryName.Equals("C:\\Program Files (x86)\\Xytriza\\XPS", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
	{
		switch (e.KeyChar)
		{
		case '1':
			button1.Focus();
			button1.PerformClick();
			break;
		case '!':
			button1.Focus();
			ResetGDPS("2.2");
			break;
		case '2':
			button2.Focus();
			button2.PerformClick();
			break;
		case '@':
			button2.Focus();
			ResetGDPS("2.1");
			break;
		case '3':
			button3.Focus();
			button3.PerformClick();
			break;
		case '#':
			button3.Focus();
			ResetGDPS("2.0");
			break;
		case '4':
			button4.Focus();
			button4.PerformClick();
			break;
		case '$':
			button4.Focus();
			ResetGDPS("1.9");
			break;
		}
	}

	private void ToggleSettings()
	{
		if (settingsOpen)
		{
			button1.Visible = true;
			button2.Visible = true;
			button3.Visible = true;
			button4.Visible = true;
			label1.Visible = true;
			pictureBox7.Visible = true;
			pictureBox6.Visible = true;
			pictureBox5.Visible = true;
			pictureBox4.Visible = true;
			pictureBox3.Visible = true;
			pictureBox2.Visible = true;
			button5.Visible = false;
			button6.Visible = false;
			button7.Visible = false;
			button8.Visible = false;
			button9.Visible = false;
			label2.Visible = false;
			panel1.Visible = false;
			label3.Visible = false;
			panel2.Visible = false;
			label4.Visible = false;
			panel3.Visible = false;
			label5.Visible = false;
			panel4.Visible = false;
			label6.Visible = false;
			label7.Visible = false;
			label8.Visible = false;
			label9.Visible = false;
			pictureBox10.Visible = false;
			pictureBox11.Visible = false;
			pictureBox12.Visible = false;
			pictureBox13.Visible = false;
			button1.Focus();
			settingsOpen = false;
			return;
		}
		button1.Visible = false;
		button2.Visible = false;
		button3.Visible = false;
		button4.Visible = false;
		label1.Visible = false;
		pictureBox7.Visible = false;
		pictureBox6.Visible = false;
		pictureBox5.Visible = false;
		pictureBox4.Visible = false;
		pictureBox3.Visible = false;
		pictureBox2.Visible = false;
		button5.Visible = true;
		button6.Visible = true;
		button7.Visible = true;
		button8.Visible = true;
		button9.Visible = true;
		label2.Visible = true;
		panel1.Visible = true;
		label3.Visible = true;
		panel2.Visible = true;
		label4.Visible = true;
		panel3.Visible = true;
		label5.Visible = true;
		panel4.Visible = true;
		label6.Visible = true;
		label7.Visible = true;
		label8.Visible = true;
		label9.Visible = true;
		if (!settingCloseOnLoad)
		{
			pictureBox11.Visible = true;
		}
		else
		{
			pictureBox13.Visible = true;
		}
		if (!settingAllowMultipleInstances)
		{
			pictureBox10.Visible = true;
		}
		else
		{
			pictureBox12.Visible = true;
		}
		button9.Focus();
		settingsOpen = true;
	}

	private void ResetConfig()
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Xytriza", "XPS");
		string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "xytriza", "xpslauncher.json");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		if (File.Exists(path2))
		{
			File.Delete(path2);
		}
		if (settingsOpen && settingCloseOnLoad)
		{
			pictureBox13.Visible = false;
			pictureBox11.Visible = true;
		}
		if (settingsOpen && settingAllowMultipleInstances)
		{
			pictureBox12.Visible = false;
			pictureBox10.Visible = true;
		}
		settingAllowMultipleInstances = false;
		settingAllowMultipleInstances = false;
		SetThemeColor(Color.FromArgb(50, 50, 50));
		dynamic val = new
		{
			lastVersion = currentVersion,
			closeOnLoad = false,
			allowMultipleInstances = false,
			theme = 0
		};
		string contents = JsonConvert.SerializeObject(val, Formatting.Indented);
		File.WriteAllText(path2, contents);
	}

	private void WriteConfig<T>(string key, T value)
	{
		dynamic val = ReadConfig();
		val[key] = value;
		string contents = JsonConvert.SerializeObject(val, Formatting.Indented);
		File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "xytriza", "xpslauncher.json"), contents);
	}

	private void RemoveConfigValue(string key)
	{
		dynamic val = ReadConfig();
		val.Remove(key);
		string contents = JsonConvert.SerializeObject(val, Formatting.Indented);
		File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "xytriza", "xpslauncher.json"), contents);
	}

	private dynamic ReadConfig()
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "xytriza", "xpslauncher.json");
		string obj = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XPS");
		string path2 = Path.Combine(obj, "launcher.json");
		if (!File.Exists(path))
		{
			ResetConfig();
		}
		if (Directory.Exists(obj) && File.Exists(path2))
		{
			string contents = File.ReadAllText(path2);
			File.WriteAllText(path, contents);
			File.Delete(path2);
		}
		return JsonConvert.DeserializeObject(File.ReadAllText(path)) ?? new { };
	}

	public static bool IsProcessOpen(string filePath)
	{
		return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(filePath)).Any((Process p) => p.MainModule.FileName.Equals(filePath, StringComparison.OrdinalIgnoreCase));
	}

	private void ConvertOnLoad()
	{
		try
		{
			dynamic val = ReadConfig();
			if (val.lastVersion == "2.2.0")
			{
				RemoveConfigValue("closeOnExit");
				WriteConfig("closeOnLoad", val.closeOnExit ?? ((object)false));
				WriteConfig("allowMultipleInstances", value: false);
				WriteConfig("theme", 0);
				val.allowMultipleInstances = false;
				val.closeOnLoad = val.closeOnExit ?? ((object)false);
			}
			settingCloseOnLoad = val.closeOnLoad;
			settingAllowMultipleInstances = val.allowMultipleInstances;
			if (settingsOpen && settingCloseOnLoad)
			{
				pictureBox13.Visible = false;
				pictureBox11.Visible = true;
			}
			if (settingsOpen && settingAllowMultipleInstances)
			{
				pictureBox12.Visible = false;
				pictureBox10.Visible = true;
			}
			LoadTheme();
			WriteConfig("lastVersion", currentVersion);
		}
		catch
		{
			ResetConfig();
		}
	}

	private void LoadTheme()
	{
		dynamic val = ReadConfig();
		int num = val.theme;
		try
		{
			switch (num)
			{
			case 1:
				SetThemeColor(Color.FromArgb(0, 0, 0));
				break;
			case 2:
				SetThemeColor(Color.FromArgb(25, 0, 50));
				break;
			case 3:
				SetThemeColor(Color.FromArgb(35, 0, 0));
				break;
			default:
				SetThemeColor(Color.FromArgb(50, 50, 50));
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading theme. Error message: " + ex.Message, "Error loading theme", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SetThemeColor(Color color)
	{
		BackColor = color;
		pictureBox8.BackColor = color;
		pictureBox7.BackColor = color;
		pictureBox6.BackColor = color;
		pictureBox5.BackColor = color;
		pictureBox4.BackColor = color;
		pictureBox3.BackColor = color;
		pictureBox2.BackColor = color;
		pictureBox1.BackColor = color;
	}

	private void ToggleCloseOnLoad(object sender = null, EventArgs e = null)
	{
		if (settingCloseOnLoad)
		{
			settingCloseOnLoad = false;
			pictureBox13.Visible = false;
			pictureBox11.Visible = true;
		}
		else
		{
			settingCloseOnLoad = true;
			pictureBox13.Visible = true;
			pictureBox11.Visible = false;
		}
		WriteConfig("closeOnLoad", settingCloseOnLoad);
	}

	private void ToggleMultiInstance(object sender = null, EventArgs e = null)
	{
		if (settingAllowMultipleInstances)
		{
			settingAllowMultipleInstances = false;
			pictureBox12.Visible = false;
			pictureBox10.Visible = true;
		}
		else
		{
			settingAllowMultipleInstances = true;
			pictureBox12.Visible = true;
			pictureBox10.Visible = false;
		}
		WriteConfig("allowMultipleInstances", settingAllowMultipleInstances);
	}

	private void ResetXPSBrowserButton(object sender, EventArgs e)
	{
		string path = Path.Combine(GetExecutionPath(), "XPS.exe.WebView2");
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		MessageBox.Show("XPS Browser has been reset to the default settings!", "Successfully reset XPS Browser", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XPSLauncher.MainForm));
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.button7 = new System.Windows.Forms.Button();
		this.button8 = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.pictureBox13 = new System.Windows.Forms.PictureBox();
		this.pictureBox12 = new System.Windows.Forms.PictureBox();
		this.pictureBox11 = new System.Windows.Forms.PictureBox();
		this.pictureBox10 = new System.Windows.Forms.PictureBox();
		this.pictureBox9 = new System.Windows.Forms.PictureBox();
		this.pictureBox8 = new System.Windows.Forms.PictureBox();
		this.pictureBox7 = new System.Windows.Forms.PictureBox();
		this.pictureBox6 = new System.Windows.Forms.PictureBox();
		this.pictureBox5 = new System.Windows.Forms.PictureBox();
		this.pictureBox4 = new System.Windows.Forms.PictureBox();
		this.pictureBox3 = new System.Windows.Forms.PictureBox();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label9 = new System.Windows.Forms.Label();
		this.button9 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.pictureBox13).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox12).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox11).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox10).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox9).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.button1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.button1.Location = new System.Drawing.Point(320, 217);
		this.button1.Margin = new System.Windows.Forms.Padding(4);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(160, 49);
		this.button1.TabIndex = 0;
		this.button1.Text = "2.2";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.UseWaitCursor = true;
		this.button2.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.button2.Location = new System.Drawing.Point(320, 338);
		this.button2.Margin = new System.Windows.Forms.Padding(4);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(160, 49);
		this.button2.TabIndex = 1;
		this.button2.Text = "2.1";
		this.button2.UseVisualStyleBackColor = true;
		this.button3.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.button3.Location = new System.Drawing.Point(320, 460);
		this.button3.Margin = new System.Windows.Forms.Padding(4);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(160, 49);
		this.button3.TabIndex = 2;
		this.button3.Text = "2.0";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.UseWaitCursor = true;
		this.button4.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button4.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.button4.Location = new System.Drawing.Point(320, 582);
		this.button4.Margin = new System.Windows.Forms.Padding(4);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(160, 49);
		this.button4.TabIndex = 3;
		this.button4.Text = "1.9";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.UseWaitCursor = true;
		this.label1.AutoSize = true;
		this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label1.Location = new System.Drawing.Point(128, 148);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(210, 16);
		this.label1.TabIndex = 4;
		this.label1.Text = "What version do you want to load?";
		this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button5.ForeColor = System.Drawing.Color.White;
		this.button5.Location = new System.Drawing.Point(253, 558);
		this.button5.Margin = new System.Windows.Forms.Padding(4);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(296, 49);
		this.button5.TabIndex = 14;
		this.button5.Text = "Open App Folder";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.UseWaitCursor = true;
		this.button5.Visible = false;
		this.button6.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button6.ForeColor = System.Drawing.Color.White;
		this.button6.Location = new System.Drawing.Point(253, 614);
		this.button6.Margin = new System.Windows.Forms.Padding(4);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(296, 49);
		this.button6.TabIndex = 15;
		this.button6.Text = "Open AppData Folder";
		this.button6.UseVisualStyleBackColor = true;
		this.button6.UseWaitCursor = true;
		this.button6.Visible = false;
		this.label2.AutoSize = true;
		this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label2.Location = new System.Drawing.Point(325, 290);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 16);
		this.label2.TabIndex = 17;
		this.label2.Text = "Themes";
		this.label2.Visible = false;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel1.Location = new System.Drawing.Point(201, 350);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(85, 78);
		this.panel1.TabIndex = 18;
		this.panel1.Visible = false;
		this.panel1.Click += new System.EventHandler(DarkThemeButton);
		this.label3.AutoSize = true;
		this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label3.Location = new System.Drawing.Point(209, 436);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(36, 16);
		this.label3.TabIndex = 19;
		this.label3.Text = "Dark";
		this.label3.Visible = false;
		this.label4.AutoSize = true;
		this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label4.Location = new System.Drawing.Point(300, 436);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 16);
		this.label4.TabIndex = 21;
		this.label4.Text = "Amoled";
		this.label4.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel2.Location = new System.Drawing.Point(306, 350);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(85, 78);
		this.panel2.TabIndex = 20;
		this.panel2.Visible = false;
		this.panel2.Click += new System.EventHandler(AmoledThemeButton);
		this.label5.AutoSize = true;
		this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label5.Location = new System.Drawing.Point(406, 436);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 16);
		this.label5.TabIndex = 23;
		this.label5.Text = "Purple";
		this.label5.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(25, 0, 50);
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel3.Location = new System.Drawing.Point(413, 350);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(85, 78);
		this.panel3.TabIndex = 22;
		this.panel3.Visible = false;
		this.panel3.Click += new System.EventHandler(PurpleThemeButton);
		this.label6.AutoSize = true;
		this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label6.Location = new System.Drawing.Point(533, 436);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(33, 16);
		this.label6.TabIndex = 25;
		this.label6.Text = "Red";
		this.label6.Visible = false;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(35, 0, 0);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel4.Location = new System.Drawing.Point(518, 350);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(85, 78);
		this.panel4.TabIndex = 24;
		this.panel4.Visible = false;
		this.panel4.Click += new System.EventHandler(RedThemeButton);
		this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
		this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button7.ForeColor = System.Drawing.Color.White;
		this.button7.Location = new System.Drawing.Point(253, 670);
		this.button7.Margin = new System.Windows.Forms.Padding(4);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(296, 49);
		this.button7.TabIndex = 26;
		this.button7.Text = "Restart Launcher";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Visible = false;
		this.button7.Click += new System.EventHandler(RestartButton);
		this.button8.Cursor = System.Windows.Forms.Cursors.Hand;
		this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button8.ForeColor = System.Drawing.Color.White;
		this.button8.Location = new System.Drawing.Point(253, 726);
		this.button8.Margin = new System.Windows.Forms.Padding(4);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(296, 49);
		this.button8.TabIndex = 27;
		this.button8.Text = "Reset Launcher";
		this.button8.UseVisualStyleBackColor = true;
		this.button8.Visible = false;
		this.button8.Click += new System.EventHandler(ResetConfigButton);
		this.label7.AutoSize = true;
		this.label7.Cursor = System.Windows.Forms.Cursors.Hand;
		this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label7.Location = new System.Drawing.Point(188, 151);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(182, 16);
		this.label7.TabIndex = 29;
		this.label7.Text = "Close launcher on game load";
		this.label7.Visible = false;
		this.label7.Click += new System.EventHandler(ToggleCloseOnLoad);
		this.label8.AutoSize = true;
		this.label8.Cursor = System.Windows.Forms.Cursors.Hand;
		this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label8.Location = new System.Drawing.Point(221, 214);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(148, 16);
		this.label8.TabIndex = 30;
		this.label8.Text = "Allow multiple instances";
		this.label8.Visible = false;
		this.label8.Click += new System.EventHandler(ToggleMultiInstance);
		this.pictureBox13.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox13.Image = XPS.Properties.Resources.Checked;
		this.pictureBox13.Location = new System.Drawing.Point(145, 146);
		this.pictureBox13.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox13.Name = "pictureBox13";
		this.pictureBox13.Size = new System.Drawing.Size(41, 38);
		this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox13.TabIndex = 34;
		this.pictureBox13.TabStop = false;
		this.pictureBox13.Visible = false;
		this.pictureBox13.Click += new System.EventHandler(ToggleCloseOnLoad);
		this.pictureBox12.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox12.Image = XPS.Properties.Resources.Checked;
		this.pictureBox12.Location = new System.Drawing.Point(180, 209);
		this.pictureBox12.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox12.Name = "pictureBox12";
		this.pictureBox12.Size = new System.Drawing.Size(41, 38);
		this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox12.TabIndex = 33;
		this.pictureBox12.TabStop = false;
		this.pictureBox12.Visible = false;
		this.pictureBox12.Click += new System.EventHandler(ToggleMultiInstance);
		this.pictureBox11.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox11.Image = XPS.Properties.Resources.Unchecked;
		this.pictureBox11.Location = new System.Drawing.Point(149, 148);
		this.pictureBox11.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox11.Name = "pictureBox11";
		this.pictureBox11.Size = new System.Drawing.Size(37, 34);
		this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox11.TabIndex = 32;
		this.pictureBox11.TabStop = false;
		this.pictureBox11.Visible = false;
		this.pictureBox11.Click += new System.EventHandler(ToggleCloseOnLoad);
		this.pictureBox10.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox10.Image = XPS.Properties.Resources.Unchecked;
		this.pictureBox10.Location = new System.Drawing.Point(183, 210);
		this.pictureBox10.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox10.Name = "pictureBox10";
		this.pictureBox10.Size = new System.Drawing.Size(37, 34);
		this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox10.TabIndex = 31;
		this.pictureBox10.TabStop = false;
		this.pictureBox10.Visible = false;
		this.pictureBox10.Click += new System.EventHandler(ToggleMultiInstance);
		this.pictureBox9.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox9.Image = XPS.Properties.Resources.Exit;
		this.pictureBox9.Location = new System.Drawing.Point(0, 0);
		this.pictureBox9.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox9.Name = "pictureBox9";
		this.pictureBox9.Size = new System.Drawing.Size(112, 114);
		this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox9.TabIndex = 28;
		this.pictureBox9.TabStop = false;
		this.pictureBox9.Click += new System.EventHandler(ExitButton);
		this.pictureBox8.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox8.Image = XPS.Properties.Resources.Settings;
		this.pictureBox8.Location = new System.Drawing.Point(667, 0);
		this.pictureBox8.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox8.Name = "pictureBox8";
		this.pictureBox8.Size = new System.Drawing.Size(112, 114);
		this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox8.TabIndex = 12;
		this.pictureBox8.TabStop = false;
		this.pictureBox8.Click += new System.EventHandler(SettingsButton);
		this.pictureBox7.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox7.Image = XPS.Properties.Resources.Dashboard;
		this.pictureBox7.Location = new System.Drawing.Point(5, 640);
		this.pictureBox7.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox7.Name = "pictureBox7";
		this.pictureBox7.Size = new System.Drawing.Size(207, 76);
		this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox7.TabIndex = 11;
		this.pictureBox7.TabStop = false;
		this.pictureBox7.Click += new System.EventHandler(DashboardButton);
		this.pictureBox6.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox6.Image = XPS.Properties.Resources.Website;
		this.pictureBox6.Location = new System.Drawing.Point(5, 708);
		this.pictureBox6.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox6.Name = "pictureBox6";
		this.pictureBox6.Size = new System.Drawing.Size(207, 76);
		this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox6.TabIndex = 10;
		this.pictureBox6.TabStop = false;
		this.pictureBox6.Click += new System.EventHandler(WebsiteButton);
		this.pictureBox5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox5.Image = XPS.Properties.Resources.Twitch;
		this.pictureBox5.Location = new System.Drawing.Point(689, 703);
		this.pictureBox5.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox5.Name = "pictureBox5";
		this.pictureBox5.Size = new System.Drawing.Size(85, 79);
		this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox5.TabIndex = 9;
		this.pictureBox5.TabStop = false;
		this.pictureBox5.Click += new System.EventHandler(TwitchButton);
		this.pictureBox4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox4.Image = XPS.Properties.Resources.YouTube;
		this.pictureBox4.Location = new System.Drawing.Point(596, 703);
		this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox4.Name = "pictureBox4";
		this.pictureBox4.Size = new System.Drawing.Size(85, 79);
		this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox4.TabIndex = 8;
		this.pictureBox4.TabStop = false;
		this.pictureBox4.Click += new System.EventHandler(YoutubeButton);
		this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox3.Image = XPS.Properties.Resources.Twitter;
		this.pictureBox3.Location = new System.Drawing.Point(501, 703);
		this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox3.Name = "pictureBox3";
		this.pictureBox3.Size = new System.Drawing.Size(85, 79);
		this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox3.TabIndex = 7;
		this.pictureBox3.TabStop = false;
		this.pictureBox3.Click += new System.EventHandler(TwitterButton);
		this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBox2.Image = XPS.Properties.Resources.Discord;
		this.pictureBox2.Location = new System.Drawing.Point(408, 703);
		this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(85, 79);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 6;
		this.pictureBox2.TabStop = false;
		this.pictureBox2.Click += new System.EventHandler(DiscordButton);
		this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
		this.pictureBox1.Image = XPS.Properties.Resources.logoPngTransparent;
		this.pictureBox1.Location = new System.Drawing.Point(253, -74);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(296, 293);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 5;
		this.pictureBox1.TabStop = false;
		this.label9.AutoSize = true;
		this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
		this.label9.Location = new System.Drawing.Point(3, 753);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(0, 16);
		this.label9.TabIndex = 35;
		this.label9.Visible = false;
		this.button9.Cursor = System.Windows.Forms.Cursors.Hand;
		this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button9.ForeColor = System.Drawing.Color.White;
		this.button9.Location = new System.Drawing.Point(252, 502);
		this.button9.Margin = new System.Windows.Forms.Padding(4);
		this.button9.Name = "button9";
		this.button9.Size = new System.Drawing.Size(296, 49);
		this.button9.TabIndex = 36;
		this.button9.Text = "Reset XPS Browser";
		this.button9.UseVisualStyleBackColor = true;
		this.button9.Visible = false;
		this.button9.Click += new System.EventHandler(ResetXPSBrowserButton);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
		base.ClientSize = new System.Drawing.Size(779, 783);
		base.Controls.Add(this.button9);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.pictureBox13);
		base.Controls.Add(this.pictureBox12);
		base.Controls.Add(this.pictureBox11);
		base.Controls.Add(this.pictureBox10);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.pictureBox9);
		base.Controls.Add(this.button8);
		base.Controls.Add(this.button7);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button5);
		base.Controls.Add(this.pictureBox8);
		base.Controls.Add(this.pictureBox7);
		base.Controls.Add(this.pictureBox6);
		base.Controls.Add(this.pictureBox5);
		base.Controls.Add(this.pictureBox4);
		base.Controls.Add(this.pictureBox3);
		base.Controls.Add(this.pictureBox2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.button3);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.pictureBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "MainForm";
		this.Text = "XPS";
		((System.ComponentModel.ISupportInitialize)this.pictureBox13).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox12).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox11).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox10).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox9).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
