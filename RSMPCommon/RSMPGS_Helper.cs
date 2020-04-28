using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections;
using IniParser;
using IniParser.Model;

namespace nsRSMPGS
{
	public class cSetting
	{

		public string sKey;
		public string sDescription;

		public int RowIndex;

		public bool IsAffectedByRSMPVersion;

		private bool bActualValue;

		private bool bActualValue_RSMP_3_1_1;
		private bool bActualValue_RSMP_3_1_2;
		private bool bActualValue_RSMP_3_1_3;
		private bool bActualValue_RSMP_3_1_4;

		private bool bDefaultValue;

		private bool bDefaultValue_RSMP_3_1_1;
		private bool bDefaultValue_RSMP_3_1_2;
		private bool bDefaultValue_RSMP_3_1_3;
		private bool bDefaultValue_RSMP_3_1_4;

		public cSetting(string sKey, string sDescription, int iRowIndex, bool IsAffectedByRSMPVersion, bool bDefaultValue, bool bDefaultValue_RSMP_3_1_1, bool bDefaultValue_RSMP_3_1_2, bool bDefaultValue_RSMP_3_1_3, bool bDefaultValue_RSMP_3_1_4)
		{

			this.sKey = sKey;
			this.sDescription = sDescription;

			this.RowIndex = iRowIndex;

			this.IsAffectedByRSMPVersion = IsAffectedByRSMPVersion;

			this.bDefaultValue = bDefaultValue;

			this.bDefaultValue_RSMP_3_1_1 = bDefaultValue_RSMP_3_1_1;
			this.bDefaultValue_RSMP_3_1_2 = bDefaultValue_RSMP_3_1_2;
			this.bDefaultValue_RSMP_3_1_3 = bDefaultValue_RSMP_3_1_3;
			this.bDefaultValue_RSMP_3_1_4 = bDefaultValue_RSMP_3_1_4;

		}

		public int GetColumnIndex(cJSon.RSMPVersion rsmpVersion)
		{
			return (int)(rsmpVersion + 1);
		}

		public cJSon.RSMPVersion GetRSMPVersion(int iColumnIndex)
		{
			return (cJSon.RSMPVersion)(iColumnIndex - 1);
		}

		public bool GetActualValue(int iColumnIndex)
		{
			return GetActualValue(GetRSMPVersion(iColumnIndex));
		}

		public bool GetActualValue(cJSon.RSMPVersion rsmpVersion)
		{

			switch (rsmpVersion)
			{
				case cJSon.RSMPVersion.NotSupported:
					return bActualValue;

				case cJSon.RSMPVersion.RSMP_3_1_1:
					return bActualValue_RSMP_3_1_1;

				case cJSon.RSMPVersion.RSMP_3_1_2:
					return bActualValue_RSMP_3_1_2;

				case cJSon.RSMPVersion.RSMP_3_1_3:
					return bActualValue_RSMP_3_1_3;

				case cJSon.RSMPVersion.RSMP_3_1_4:
					return bActualValue_RSMP_3_1_4;
			}
			return false;
		}

		public bool GetDefaultValue(int iColumnIndex)
		{
			return GetDefaultValue(GetRSMPVersion(iColumnIndex));
		}

		public bool GetDefaultValue(cJSon.RSMPVersion rsmpVersion)
		{

			switch (rsmpVersion)
			{

				case cJSon.RSMPVersion.NotSupported:
					return bDefaultValue;

				case cJSon.RSMPVersion.RSMP_3_1_1:
					return bDefaultValue_RSMP_3_1_1;

				case cJSon.RSMPVersion.RSMP_3_1_2:
					return bDefaultValue_RSMP_3_1_2;

				case cJSon.RSMPVersion.RSMP_3_1_3:
					return bDefaultValue_RSMP_3_1_3;

				case cJSon.RSMPVersion.RSMP_3_1_4:
					return bDefaultValue_RSMP_3_1_4;

				default:
					return false;
			}
		}

		public void SetActualValue(int iColumnIndex, bool bValue)
		{
			SetActualValue(GetRSMPVersion(iColumnIndex), bValue);
		}

		public void SetActualValue(cJSon.RSMPVersion rsmpVersion, bool bValue)
		{
			switch (rsmpVersion)
			{

				case cJSon.RSMPVersion.NotSupported:
					bActualValue = bValue;
					break;

				case cJSon.RSMPVersion.RSMP_3_1_1:
					bActualValue_RSMP_3_1_1 = bValue;
					break;

				case cJSon.RSMPVersion.RSMP_3_1_2:
					bActualValue_RSMP_3_1_2 = bValue;
					break;

				case cJSon.RSMPVersion.RSMP_3_1_3:
					bActualValue_RSMP_3_1_3 = bValue;
					break;

				case cJSon.RSMPVersion.RSMP_3_1_4:
					bActualValue_RSMP_3_1_4 = bValue;
					break;

			}
		}
	}

	public class cPrivateProfile
	{
		public static string ApplicationPath()
		{

			if (RSMPGS.SpecifiedPath.Length == 0)
			{
#if DEBUG
				return Path.Combine("..", "..");
#else
                string AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                System.IO.FileInfo file = new System.IO.FileInfo(AssemblyName);
                return file.Directory.FullName;
#endif
			}
			else
			{
				return RSMPGS.SpecifiedPath;
			}
		}

        public static string ConfigPath()
        {
#if DEBUG
            return Path.Combine("..", "..");
#else
                string cPath;
                if (Environment.OSVersion.Platform == PlatformID.Unix || 
                    Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    cPath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".config");
                }
                else
                {
                    // My Documents
                    cPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }

#if _RSMPGS1
                cPath = Path.Combine(cPath, "RSMPGS1");
#endif

#if _RSMPGS2
                cPath = Path.Combine(cPath, "RSMPGS2");
#endif

            System.IO.FileInfo file = new System.IO.FileInfo(cPath);
            return file.FullName;
#endif
        }

        public static string SettingsPath()
		{
			return Path.Combine(ConfigPath(), "Settings");
		}

		public static string ObjectFilesPath()
		{
			return Path.Combine(ConfigPath(), "Objects");
		}

		public static string LogFilesPath()
		{
			return Path.Combine(ConfigPath(), "LogFiles");
		}

		public static string SysLogFilesPath()
		{
			return Path.Combine(LogFilesPath(), "SysLogFiles");
		}

		public static string DebugFilesPath()
		{
			return Path.Combine(LogFilesPath(), "DebugFiles");
		}

		public static string EventFilesPath()
		{
			return Path.Combine(LogFilesPath(), "EventFiles");
		}

		public static string ProcessImageFileFullName()
		{
			return Path.Combine(cPrivateProfile.ObjectFilesPath(), "ProcessImage.dat");
		}

		public static string GetIniFileString(string category, string key, string defaultValue)
		{
			return GetIniFileString(RSMPGS.IniFileFullname, category, key, defaultValue);
		}

		public static string GetIniFileString(string iniFile, string category, string key, string defaultValue)
		{
			var parser = new FileIniDataParser ();
			IniData data = parser.ReadFile (RSMPGS.IniFileFullname);
			if (data [category] [key] == null)
				return defaultValue; 
			return data [category] [key];
		}

		public static int GetIniFileInt(string category, string key, int defaultValue)
		{
			return GetIniFileInt(RSMPGS.IniFileFullname, category, key, defaultValue);
		}

		public static int GetIniFileInt(string iniFile, string category, string key, int defaultValue)
		{
			var parser = new FileIniDataParser ();
			IniData data = parser.ReadFile (RSMPGS.IniFileFullname);
			string sData = data [category] [key];
			if (sData == null)
				return defaultValue;
			return int.Parse (sData);
		}

		public static void WriteIniFileString(string category, string key, string value)
		{
			WriteIniFileString(RSMPGS.IniFileFullname, category, key, value);
		}

		public static void WriteIniFileString(string iniFile, string category, string key, string value)
		{
			var parser = new FileIniDataParser ();
			IniData data = parser.ReadFile (RSMPGS.IniFileFullname);
			data [category] [key] = value;
			parser.WriteFile (RSMPGS.IniFileFullname, data);
		}

		public static void WriteIniFileInt(string category, string key, int value)
		{
			WriteIniFileInt(RSMPGS.IniFileFullname, category, key, value);
		}

		public static void WriteIniFileInt(string iniFile, string category, string key, int value)
		{
			var parser = new FileIniDataParser ();
			IniData data = parser.ReadFile (RSMPGS.IniFileFullname);
			data [category] [key] = value.ToString ();
			parser.WriteFile (RSMPGS.IniFileFullname, data);
		}

    public static string Base64Encode(string plainText)
    {
      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
      return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
      var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
      return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

  }

  public class cHelper
  {
    public static void CreateDirectories()
    {

      try
      {
        Directory.CreateDirectory(cPrivateProfile.SettingsPath());
      }
      catch { }

      try
      {
        Directory.CreateDirectory(cPrivateProfile.ObjectFilesPath());
      }
      catch { }

      try
      {
        Directory.CreateDirectory(cPrivateProfile.LogFilesPath());
      }
      catch { }

      try
      {
        Directory.CreateDirectory(cPrivateProfile.SysLogFilesPath());
      }
      catch { }

      try
      {
        Directory.CreateDirectory(cPrivateProfile.DebugFilesPath());
      }
      catch { }
#if _RSMPGS2

      try
      {
        Directory.CreateDirectory(cPrivateProfile.EventFilesPath());
      }
      catch { }

#endif
    }

    public static void RestoreDebugForms()
    {

      int iDebugFormIndex;
      RSMPGS_Debug DebugForm;

      //
      // Create debug windows (if any)
      //
      for (iDebugFormIndex = 0; ; iDebugFormIndex++)
      {
        string sPrefix = "DebugForm_" + iDebugFormIndex.ToString() + "_";
        if (cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Width", -1) > 0)
        {
          DebugForm = new RSMPGS_Debug();
          //          DebugForm.MainForm = this;
          DebugForm.Left = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Left", iDebugFormIndex * 50);
          DebugForm.Top = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Top", iDebugFormIndex * 50);
          DebugForm.Width = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Width", 500);
          DebugForm.Height = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Height", 500);
          DebugForm.ToolStripMenuItem_PacketTypes_Raw.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Raw", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_All.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "All", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Version.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Version", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Alarm.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Alarm", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_AggStatus.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "AggStatus", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Status.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Status", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Command.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Command", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Watchdog.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Watchdog", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_PacketAck.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "PacketAck", 0) != 0;
          DebugForm.ToolStripMenuItem_PacketTypes_Unknown.Checked = cPrivateProfile.GetIniFileInt("Debug", sPrefix + "Unknown", 0) != 0;

          DebugForm.CalcNewCaption();
          // Forms will be shown at show event
          RSMPGS.DebugForms.Add(DebugForm);
        }
        else
        {
          break;
        }
      }
    }

    public static void StoreDebugForms()
    {

      int iDebugFormIndex = 0;

      // Clear section
      cPrivateProfile.WriteIniFileString("Debug", null, null);

      //
      // Store debug window stuff
      //
      foreach (RSMPGS_Debug DebugForm in RSMPGS.DebugForms)
      {
        string sPrefix = "DebugForm_" + iDebugFormIndex.ToString() + "_";
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Left", DebugForm.Left);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Top", DebugForm.Top);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Width", DebugForm.Width);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Height", DebugForm.Height);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "All", DebugForm.ToolStripMenuItem_PacketTypes_All.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Raw", DebugForm.ToolStripMenuItem_PacketTypes_Raw.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Version", DebugForm.ToolStripMenuItem_PacketTypes_Version.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Alarm", DebugForm.ToolStripMenuItem_PacketTypes_Alarm.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "AggStatus", DebugForm.ToolStripMenuItem_PacketTypes_AggStatus.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Status", DebugForm.ToolStripMenuItem_PacketTypes_Status.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Command", DebugForm.ToolStripMenuItem_PacketTypes_Command.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Watchdog", DebugForm.ToolStripMenuItem_PacketTypes_Watchdog.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "PacketAck", DebugForm.ToolStripMenuItem_PacketTypes_PacketAck.Checked == true ? 1 : 0);
        cPrivateProfile.WriteIniFileInt("Debug", sPrefix + "Unknown", DebugForm.ToolStripMenuItem_PacketTypes_Unknown.Checked == true ? 1 : 0);

        iDebugFormIndex++;
      }

      //
      // Close the debug forms
      //
      while (RSMPGS.DebugForms.Count > 0)
      {
        // Will remove itself from the list when closed
        RSMPGS.DebugForms[0].Close();
      }

    }

    public static void LoadRSMPSettings()
    {

      AddSetting("AllowUseRSMPVersion", "Allow/use RSMP version in protocol negotiation", true, true, true, true);

      AddSetting("SendVersionInfoAtConnect", "Send and expect version info when connecting", true);
      AddSetting("SXL_VersionIgnore", "Ignore client RSMP and SXL (SUL) version incompability", false);
      AddSetting("SendWatchdogPacketAtStartup", "Send and expect Watchdog packet when connecting", true);

#if _RSMPGS1

      AddSetting("SendAggregatedStatusAtConnect", "Send aggregated status when connecting", false, false, true, true);

      AddSetting("SendAllAlarmsWhenConnect", "Send all alarms when connecting", false, false, true, true);
      AddSetting("BufferAndSendAlarmsWhenConnect", "Buffer alarm events when disconnected and send them when connecting", false, false, true, true);
      AddSetting("BufferAndSendAggregatedStatusWhenConnect", "Buffer aggregated status when disconnected and send them when connecting", false, false, true, true);
      AddSetting("BufferAndSendStatusUpdatesWhenConnect", "Buffer status updates when disconnected and send them when connecting", false, false, true, true);
      AddSetting("ClearSubscriptionsAtDisconnect", "Clear subscriptions when disconnecting", true, true, false, false);
      AddSetting("Buffer10000Messages", "Buffer upto 10000 messages (instead of 1000)", false, false, false, true);

#endif
      AddSetting("UseStrictProtocolAnalysis", "Use strict and unforgiving protocol parsing", false, true, true, true);
      AddSetting("UseCaseSensitiveIds", "Use case sensitive lookup for object id's and references", false, true, true, true);
      AddSetting("DontAckPackets", "Never Ack or NAck packets", false);
      AddSetting("ResendUnackedPackets", "Resend unacked packets", true);
      AddSetting("WaitInfiniteForUnackedPackets", "Wait infinite for packet Ack / NAcks", false);
      AddSetting("JSonPropertyCaseChange10", "Change JSon property name characters randomly to ucase/lcase (10% change rate)", false);
      AddSetting("DropBytesInNegotiationPackets10", "Drop random bytes in negotiation packets (10% dropped)", false);
      AddSetting("CloseConnectionIfNegotiationIsOutOfSequence", "Close connection if negotiation is out of sequence", true);

      AddSetting("SendWatchdogPacketCyclically", "Send Watchdog packets cyclically", true);
      AddSetting("ExpectWatchdogPackets", "Expect Watchdog packets", true);

      AddSetting("UseEncryption", "Use encryption (TLS)", false);

      RSMPGS.MainForm.dataGridView_Behaviour.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

    }

    private static void AddSetting(string sKey, string sDescription, bool bDefaultValue)
    {
      AddSetting(sKey, sDescription, false, bDefaultValue, false, false, false, false);
    }

    private static void AddSetting(string sKey, string sDescription, bool bRSMP_3_1_1, bool bRSMP_3_1_2, bool bRSMP_3_1_3, bool bRSMP_3_1_4)
    {
      AddSetting(sKey, sDescription, true, false, bRSMP_3_1_1, bRSMP_3_1_2, bRSMP_3_1_3, bRSMP_3_1_4);
    }

    private static void AddSetting(string sKey, string sDescription, bool IsAffectedByRSMPVersion, bool bDefaultValue, bool bRSMP_3_1_1, bool bRSMP_3_1_2, bool bRSMP_3_1_3, bool bRSMP_3_1_4)
    {

      int iRowIndex = RSMPGS.MainForm.dataGridView_Behaviour.Rows.Add(sDescription, bDefaultValue, bRSMP_3_1_1, bRSMP_3_1_2, bRSMP_3_1_3, bRSMP_3_1_4);

      //RSMPGS.MainForm.dataGridView_Behaviour.Rows.Add(
      //RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[2]

      cSetting Setting = new cSetting(sKey, sDescription, iRowIndex, IsAffectedByRSMPVersion, bDefaultValue, bRSMP_3_1_1, bRSMP_3_1_2, bRSMP_3_1_3, bRSMP_3_1_4);

      RSMPGS.Settings.Add(sKey, Setting);

      RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Tag = Setting;

      // Prevent blue for text
      RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[0].Style.SelectionBackColor = RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[0].Style.BackColor;

      if (IsAffectedByRSMPVersion)
      {
        int iColumnIndex;

        iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_1);
        Setting.SetActualValue(iColumnIndex, cPrivateProfile.GetIniFileInt("Behaviour_RSMP_3_1_1", sKey, Setting.GetDefaultValue(iColumnIndex) ? 1 : 0) != 0);
        RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetActualValue(iColumnIndex);

        iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_2);
        Setting.SetActualValue(iColumnIndex, cPrivateProfile.GetIniFileInt("Behaviour_RSMP_3_1_2", sKey, Setting.GetDefaultValue(iColumnIndex) ? 1 : 0) != 0);
        RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetActualValue(iColumnIndex);

        iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_3);
        Setting.SetActualValue(iColumnIndex, cPrivateProfile.GetIniFileInt("Behaviour_RSMP_3_1_3", sKey, Setting.GetDefaultValue(iColumnIndex) ? 1 : 0) != 0);
        RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetActualValue(iColumnIndex);

        iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_4);
        Setting.SetActualValue(iColumnIndex, cPrivateProfile.GetIniFileInt("Behaviour_RSMP_3_1_4", sKey, Setting.GetDefaultValue(iColumnIndex) ? 1 : 0) != 0);
        RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetActualValue(iColumnIndex);

        HideSettingCell(iRowIndex, Setting.GetColumnIndex(cJSon.RSMPVersion.NotSupported));

      }
      else
      {

        int iColumnIndex;

        iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.NotSupported);
        Setting.SetActualValue(iColumnIndex, cPrivateProfile.GetIniFileInt("Behaviour_Other", sKey, Setting.GetDefaultValue(iColumnIndex) ? 1 : 0) != 0);
        RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetActualValue(iColumnIndex);

        HideSettingCell(iRowIndex, Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_1));
        HideSettingCell(iRowIndex, Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_2));
        HideSettingCell(iRowIndex, Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_3));
        HideSettingCell(iRowIndex, Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_4));

      }

      ApplySettingBackColor(iRowIndex, cJSon.RSMPVersion.NotSupported);

      ApplySettingBackColor(iRowIndex, cJSon.RSMPVersion.RSMP_3_1_1);
      ApplySettingBackColor(iRowIndex, cJSon.RSMPVersion.RSMP_3_1_2);
      ApplySettingBackColor(iRowIndex, cJSon.RSMPVersion.RSMP_3_1_3);
      ApplySettingBackColor(iRowIndex, cJSon.RSMPVersion.RSMP_3_1_4);

      //RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[1].Style.BackColor = Color.Red;

      //RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[1].Style.SelectionBackColor = Color.;

    }

    public static void HideSettingCell(int iRowIndex, int iColumnIndex)
    {
      RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[iColumnIndex].Value = false;
      RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[iColumnIndex] = new DataGridViewTextBoxCell();
      RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Cells[iColumnIndex].Value = "";
    }

    public static void ApplySettingBackColor(int iRowIndex, cJSon.RSMPVersion rsmpVersion)
    {

      if (iRowIndex == -1)
      {
        return;
      }

      cSetting setting = (cSetting)RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Tag;

      ApplySettingBackColor(setting, setting.GetColumnIndex(rsmpVersion), setting.GetActualValue(rsmpVersion), setting.GetDefaultValue(rsmpVersion));

    }

    public static void ApplySettingBackColor(int iRowIndex, int iColumnIndex)
    {

      if (iRowIndex == -1)
      {
        return;
      }

      cSetting setting = (cSetting)RSMPGS.MainForm.dataGridView_Behaviour.Rows[iRowIndex].Tag;

      ApplySettingBackColor(setting, iColumnIndex, setting.GetActualValue(iColumnIndex), setting.GetDefaultValue(iColumnIndex));

    }

    public static void ApplySettingBackColor(cSetting setting, int iColumnIndex, bool bValue, bool bDefaultValue)
    {

      Color backColor = SystemColors.Window;

      if (setting.sKey.Equals("AllowUseRSMPVersion", StringComparison.OrdinalIgnoreCase) == false)
      {
        backColor = bValue != bDefaultValue ? Color.Red : SystemColors.Window;
      }

      RSMPGS.MainForm.dataGridView_Behaviour.Rows[setting.RowIndex].Cells[iColumnIndex].Style.BackColor = backColor;
      RSMPGS.MainForm.dataGridView_Behaviour.Rows[setting.RowIndex].Cells[iColumnIndex].Style.SelectionBackColor = backColor;
    }

    public static bool IsSettingChecked(string sKey)
    {

      cJSon.RSMPVersion rsmpVersion = RSMPGS.JSon.NegotiatedRSMPVersion;

      cSetting setting = RSMPGS.Settings[sKey];

      if (setting.IsAffectedByRSMPVersion)
      {
        if (rsmpVersion == cJSon.RSMPVersion.NotSupported)
        {
          rsmpVersion = RSMPGS.JSon.FindOutHighestCheckedRSMPVersion();
          bool bValue;
          if (rsmpVersion == cJSon.RSMPVersion.NotSupported)
          {
            bValue = false;
            RSMPGS.SysLog.SysLog(cSysLogAndDebug.Severity.Info, "Setting '{0}' could not be fetched as we have not negotiated any version, we will use '{1}'", setting.sDescription, bValue);
          }
          else
          {
            bValue = setting.GetActualValue(rsmpVersion);
            RSMPGS.SysLog.SysLog(cSysLogAndDebug.Severity.Info, "Setting '{0}' could not be fetched as we have not negotiated any version, we will use '{1}' from {2}", setting.sDescription, bValue, RSMPGS.JSon.sRSMPVersions[(int)rsmpVersion]);
          }
          return bValue;
        }
        else
        {
          return setting.GetActualValue(rsmpVersion);
        }
      }
      else
      {
        return setting.GetActualValue(cJSon.RSMPVersion.NotSupported);
      }

    }

    public static void SettingCheckChanged(DataGridViewCellEventArgs e)
    {

      if (e.RowIndex < 0)
      {
        return;
      }

      if (RSMPGS.MainForm.bIsLoading)
      {
        return;
      }

      try
      {
        cSetting setting = (cSetting)RSMPGS.MainForm.dataGridView_Behaviour.Rows[e.RowIndex].Tag;
        if (RSMPGS.MainForm.dataGridView_Behaviour.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType() == typeof(bool))
        {
          bool bValue = (bool)RSMPGS.MainForm.dataGridView_Behaviour.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
          //bool pValue = setting.GetActualValue(e.ColumnIndex);
          setting.SetActualValue(e.ColumnIndex, bValue);
          //Debug.WriteLine(setting.sKey + "." + setting.GetRSMPVersion(e.ColumnIndex) + ", " + pValue + " -> " + bValue);
          cHelper.ApplySettingBackColor(e.RowIndex, e.ColumnIndex);
        }
      }
      catch
      {
      }

    }

    public static void SaveRSMPSettings()
    {

      foreach (string sKey in RSMPGS.Settings.Keys)
      {
        cSetting Setting = RSMPGS.Settings[sKey];

        if (Setting.IsAffectedByRSMPVersion)
        {
          cPrivateProfile.WriteIniFileInt("Behaviour_RSMP_3_1_1", sKey, Setting.GetActualValue(cJSon.RSMPVersion.RSMP_3_1_1) ? 1 : 0);
          cPrivateProfile.WriteIniFileInt("Behaviour_RSMP_3_1_2", sKey, Setting.GetActualValue(cJSon.RSMPVersion.RSMP_3_1_2) ? 1 : 0);
          cPrivateProfile.WriteIniFileInt("Behaviour_RSMP_3_1_3", sKey, Setting.GetActualValue(cJSon.RSMPVersion.RSMP_3_1_3) ? 1 : 0);
          cPrivateProfile.WriteIniFileInt("Behaviour_RSMP_3_1_4", sKey, Setting.GetActualValue(cJSon.RSMPVersion.RSMP_3_1_4) ? 1 : 0);
        }
        else
        {
          cPrivateProfile.WriteIniFileInt("Behaviour_Other", sKey, Setting.GetActualValue(cJSon.RSMPVersion.NotSupported) ? 1 : 0);
        }

      }

    }

    public static void ResetRSMPSettingToDefault()
    {
      int iColumnIndex;

      foreach (cSetting Setting in RSMPGS.Settings.Values)
      {
        if (Setting.IsAffectedByRSMPVersion)
        {
          iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_1);
          RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetDefaultValue(iColumnIndex);
          iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_2);
          RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetDefaultValue(iColumnIndex);
          iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_3);
          RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetDefaultValue(iColumnIndex);
          iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.RSMP_3_1_4);
          RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetDefaultValue(iColumnIndex);
        }
        else
        {
          iColumnIndex = Setting.GetColumnIndex(cJSon.RSMPVersion.NotSupported);
          RSMPGS.MainForm.dataGridView_Behaviour.Rows[Setting.RowIndex].Cells[iColumnIndex].Value = Setting.GetDefaultValue(iColumnIndex);
        }

      }
    }

    public static void AddStatistics()
    {
      AddStatistic("TxPackets", "Sent data", "packets");
      AddStatistic("RxPackets", "Received data", "packets");
      AddStatistic("TxBytes", "Sent data", "bytes");
      AddStatistic("RxBytes", "Received data", "bytes");
      AddStatistic("TxAvLength", "Average sent packet length", "bytes");
      AddStatistic("RxAvLength", "Average received packet length", "bytes");
      AddStatistic("TxRTTimeInMsec", "Roundtrip time", "msec");
      AddStatistic("TxAvRTTimeInMsec", "Average Roundtrip time", "msec");

      // Used as memory variable for calc
      RSMPGS.Statistics.Add("TxRTTimeNoOfPackets", 0);
      RSMPGS.Statistics.Add("TxRTTimeTotalTimeInMsec", 0);


    }

    public static void AddStatistic(string sKey, string sDescription, string sUnit)
    {

      ListViewItem lvItem = RSMPGS.MainForm.listView_Statistics.Items.Add(sKey, sDescription, -1);

      lvItem.SubItems.Add("");
      lvItem.SubItems.Add(sUnit);

      RSMPGS.Statistics.Add(sKey, 0);

    }

    public static void ClearStatistics()
    {
      List<string> sKeys = new List<string>(RSMPGS.Statistics.Keys);
      foreach (string sKey in sKeys)
      {
        RSMPGS.Statistics[sKey] = 0;
        UpdateStatisticsRow(sKey, "");
      }

    }

    public static void UpdateStatistics(int iInterval)
    {

      int iLastConnectedStatus = -1;

      UpdateStatisticsRow("TxPackets", RSMPGS.Statistics["TxPackets"].ToString());
      UpdateStatisticsRow("RxPackets", RSMPGS.Statistics["RxPackets"].ToString());
      UpdateStatisticsRow("TxBytes", RSMPGS.Statistics["TxBytes"].ToString());
      UpdateStatisticsRow("RxBytes", RSMPGS.Statistics["RxBytes"].ToString());

      UpdateStatisticsRow("TxRTTimeInMsec", RSMPGS.Statistics["TxRTTimeInMsec"].ToString("n1"));

      if (RSMPGS.Statistics["TxPackets"] > 0)
      {
        UpdateStatisticsRow("TxAvLength", (RSMPGS.Statistics["TxBytes"] / RSMPGS.Statistics["TxPackets"]).ToString("n0"));
      }
      if (RSMPGS.Statistics["RxPackets"] > 0)
      {
        UpdateStatisticsRow("RxAvLength", (RSMPGS.Statistics["RxBytes"] / RSMPGS.Statistics["RxPackets"]).ToString("n0"));
      }

      if (RSMPGS.Statistics["TxRTTimeNoOfPackets"] > 0)
      {
        UpdateStatisticsRow("TxAvRTTimeInMsec", (RSMPGS.Statistics["TxRTTimeTotalTimeInMsec"] / RSMPGS.Statistics["TxRTTimeNoOfPackets"]).ToString("n1"));
      }

      RSMPGS.DebugConnection.DebugConnectionStatisticsTimer += iInterval;

      if (RSMPGS.DebugConnection.DebugConnectionStatisticsTimer >= 1000 || iLastConnectedStatus != RSMPGS.RSMPConnection.ConnectionStatus())
      {

        iLastConnectedStatus = RSMPGS.RSMPConnection.ConnectionStatus();

        string sStatisticPacket = "S" + "\t" + ((iLastConnectedStatus == cTcpSocket.ConnectionStatus_Connected) ? "connected" : "");

        foreach (ListViewItem lvItem in RSMPGS.MainForm.listView_Statistics.Items)
        {
          sStatisticPacket += "\t" + lvItem.SubItems[1].Text;
        }

        RSMPGS.DebugConnection.SendPacket(sStatisticPacket);
        RSMPGS.DebugConnection.DebugConnectionStatisticsTimer = 0;

      }

    }

    public static void UpdateStatisticsRow(string sColumnKey, string sNewValue)
    {
      try
      {
        // Prevent flicker
        ListViewItem lvItem = RSMPGS.MainForm.listView_Statistics.Items[sColumnKey];
        if (lvItem.SubItems[1].Text != sNewValue)
        {
          RSMPGS.MainForm.listView_Statistics.Items[sColumnKey].SubItems[1].Text = sNewValue;
        }
      }
      catch
      {
      }
    }

    public static void ChangeJSONPropertiesCasing(object obj, ref string sJSon, int iErrorPercentage)
    {

      Random rnd = new Random();

      Type T = obj.GetType();

      FieldInfo[] fields = T.GetFields();

      char[] chArray = sJSon.ToCharArray();

      foreach (FieldInfo field in fields)
      {
        int iStartIndex = sJSon.IndexOf("\"" + field.Name + "\":", StringComparison.OrdinalIgnoreCase);

        if (iStartIndex >= 0)
        {
          for (int iCharIndex = 0; iCharIndex < field.Name.Length; iCharIndex++)
          {
            if (iErrorPercentage > rnd.Next(0, 100))
            {
              char c = chArray[iStartIndex + iCharIndex + 1];
              c = Char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c);
              chArray[iStartIndex + iCharIndex + 1] = c;
            }
          }
        }
      }
      sJSon = new string(chArray);
    }

    public static string Item(string sString, int iItem, char chDelimiter)
    {

      string[] sItems = sString.Split(chDelimiter);

      if (sItems.GetLength(0) > iItem)
      {
        return sItems[iItem];
      }
      else
      {
        return "";
      }
    }

    public static string DOSAscii2ISOLatin1(string InString)
    {

      string OutString = InString;

      OutString = OutString.Replace("\x8e", "�");
      OutString = OutString.Replace("\x8f", "�");
      OutString = OutString.Replace("\x99", "�");

      OutString = OutString.Replace("\x84", "�");
      OutString = OutString.Replace("\x86", "�");
      OutString = OutString.Replace("\x94", "�");

      OutString = OutString.Replace("\xf8", "�");

      return OutString;

    }

    public static bool IsGuid(string guidString)
    {
      // Length of a proper GUID, without any surrounding braces.
      const int len_without_braces = 36;

      // Delimiter for GUID data parts.
      const char delim = '-';

      // Delimiter positions.
      const int d_0 = 8;
      const int d_1 = 13;
      const int d_2 = 18;
      const int d_3 = 23;

      // Before Delimiter positions.
      const int bd_0 = 7;
      const int bd_1 = 12;
      const int bd_2 = 17;
      const int bd_3 = 22;

      if (guidString == null)
        return false;

      if (guidString.Length != len_without_braces)
        return false;

      if (guidString[d_0] != delim ||
          guidString[d_1] != delim ||
          guidString[d_2] != delim ||
          guidString[d_3] != delim)
        return false;

      for (int i = 0;
          i < guidString.Length;
          i = i + (i == bd_0 ||
                  i == bd_1 ||
                  i == bd_2 ||
                  i == bd_3
                  ? 2 : 1))
      {
        if (!IsHex(guidString[i])) return false;
      }

      return true;
    }

    private static bool IsHex(char c)
    {
      return ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
    }

    public static cRoadSideObject FindRoadSideObject(string ntsOId, string cId, bool bUseCaseSensitiveIds)
    {

      StringComparison sc = bUseCaseSensitiveIds ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

      cRoadSideObject RoadSideObject = null;

      if (RSMPGS.ProcessImage.RoadSideObjects.TryGetValue(ntsOId + "\t" + cId, out RoadSideObject))
      {
        // The collection is case insensitive, ensure it has correct case if that is what we want
        if (bUseCaseSensitiveIds)
        {
          if (RoadSideObject.sNTSObjectId != ntsOId || RoadSideObject.sComponentId != cId)
          {
            RoadSideObject = null;
          }
        }
      }
      /*

      foreach (cSiteIdObject siteIdObject in RSMPGS.ProcessImage.SiteIdObjects)
      {
        cRoadSideObject RoadSideObject = siteIdObject.RoadSideObjects.Find(x => x.sNTSObjectId.Equals(ntsOId, sc) && x.sComponentId.Equals(cId, sc));
        if (RoadSideObject != null)
        {
          break;
        }
      }
      */

      return RoadSideObject;

    }
  }

  public class cDebugConnection
  {

    public TcpClient DebugConnectionTcpClient = null;
    public NetworkStream DebugConnectionNetworkStream = null;
    public int DebugConnectionStatisticsTimer = 0;

    private IPAddress ipDebugServerAddress = null;
    private int iDebugServerPort = 0;

    private bool ThreadIsRunning = false;
    private bool ThreadShouldRun = false;

    public cDebugConnection(string sDebugName, string sDebugServer)
    {

      if (sDebugName == "" || sDebugServer == "")
      {
        return;
      }

      try
      {
        ipDebugServerAddress = IPAddress.Parse(sDebugServer.Split(':')[0]);
      }
      catch
      {
        try
        {

          IPHostEntry host = Dns.GetHostEntry(sDebugServer.Split(':')[0]);

          // Lookup some IPv4 address (fails in Windows 7 otherwise)
          foreach (IPAddress ip in host.AddressList)
          {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
              ipDebugServerAddress = ip;
            }
          }
        }
        catch
        {
        }
      }

      try
      {
        iDebugServerPort = Int32.Parse(sDebugServer.Split(':')[1]);
      }
      catch
      {
      }

      if (iDebugServerPort > 0 && iDebugServerPort < 65536 && ipDebugServerAddress != null)
      {
        ThreadShouldRun = true;
        ThreadIsRunning = true;
        new Thread(new ThreadStart(RunThread)).Start();
      }

    }

    public void Shutdown()
    {

      ThreadShouldRun = false;

      while (ThreadIsRunning)
      {
        cTcpHelper.CloseAndDeleteStreamAndSocket(ref DebugConnectionNetworkStream, ref DebugConnectionTcpClient);
        Thread.Sleep(100);
      }

    }

    public void SendPacket(string sPacket)
    {

      if (DebugConnectionTcpClient == null || DebugConnectionNetworkStream == null)
      {
        return;
      }

      Encoding encoding = Encoding.GetEncoding("iso-8859-1");

      byte[] SendBytes = encoding.GetBytes(sPacket + "\n");

      try
      {
        DebugConnectionNetworkStream.Write(SendBytes, 0, SendBytes.GetLength(0));
      }
      catch (Exception exc)
      {
        cTcpHelper.CloseAndDeleteStreamAndSocket(ref DebugConnectionNetworkStream, ref DebugConnectionTcpClient);
      }

    }

    public void RunThread()
    {

      byte[] bBuffer = new byte[10240];
      Encoding encoding = Encoding.GetEncoding("iso-8859-1");

      while (ThreadShouldRun)
      {
        try
        {

          string sBuffer = "";

          DebugConnectionTcpClient = new TcpClient();
          DebugConnectionTcpClient.Connect(ipDebugServerAddress, iDebugServerPort);

          DebugConnectionNetworkStream = DebugConnectionTcpClient.GetStream();

          SendPacket("I" + "\t" + RSMPGS.DebugName);

          while (ThreadShouldRun == true && DebugConnectionNetworkStream != null)
          {

            int iReadBytes = DebugConnectionNetworkStream.Read(bBuffer, 0, bBuffer.GetLength(0));

            if (iReadBytes <= 0)
            {
              break;
            }

            sBuffer += encoding.GetString(bBuffer, 0, iReadBytes);

            while (sBuffer.Contains('\n'))
            {

              string sPacket = sBuffer.Substring(0, sBuffer.IndexOf('\n'));
              string[] sPacketElements = sPacket.Split('\t');

              switch (sPacketElements[0].ToUpper())
              {

                case "C":
                  if (RSMPGS.RSMPConnection.ConnectionStatus() != cTcpSocket.ConnectionStatus_Connected)
                  {
                    RSMPGS.RSMPConnection.Connect();
                  }
                  break;

                case "D":
                  RSMPGS.RSMPConnection.Disconnect();
                  break;

                default:
                  break;

              }

              sBuffer = sBuffer.Substring(sBuffer.IndexOf('\n') + 1);

            }


          }

        }
        catch (Exception exc)
        {
          cTcpHelper.CloseAndDeleteStreamAndSocket(ref DebugConnectionNetworkStream, ref DebugConnectionTcpClient);
        }
        for (int iDelay = 0; iDelay < 10000 && ThreadShouldRun == true; iDelay += 100)
        {
          Thread.Sleep(100);
        }
      }

      cTcpHelper.CloseAndDeleteStreamAndSocket(ref DebugConnectionNetworkStream, ref DebugConnectionTcpClient);
      ThreadIsRunning = false;

    }

  }

  public class cSysLogAndDebug
	{

		public enum Severity
		{
			Info = 0,
			Warning = 1,
			Error = 2
		}

		private int DaysToKeepLogFiles = cPrivateProfile.GetIniFileInt("RSMP", "DaysToKeepLogFiles", 365);

		public const int Direction_In = 0;
		public const int Direction_Out = 1;

		public string sSysLogFilePath;

		public string sEventFilePath;

		private int LastCleanupDay;

		public bool bEnableSysLog = true;

		public cSysLogAndDebug()
		{
			sSysLogFilePath = cPrivateProfile.SysLogFilesPath();

			sEventFilePath = cPrivateProfile.EventFilesPath();

		}

		// Delete old logfiles
		public void CyclicCleanup(int iElapsedMillisecs)
		{
			if (LastCleanupDay != DateTime.Now.Day)
			{
				DeleteLogFiles(sSysLogFilePath, "SysLog_????????.Log");
#if _RSMPGS2
				DeleteLogFiles(sEventFilePath, "AlarmEvents_????????.txt");
				DeleteLogFiles(sEventFilePath, "CommandEvents_????????.txt");
				DeleteLogFiles(sEventFilePath, "StatusEvents_????????.txt");
				DeleteLogFiles(sEventFilePath, "AggregatedStatusEvents_????????.txt");
#endif
				LastCleanupDay = DateTime.Now.Day;

			}

			lock (RSMPGS.SysLogItems)
			{
				if (RSMPGS.SysLogItems.Count > 0)
				{

					RSMPGS.MainForm.listView_SysLog.BeginUpdate();

					bool bShowLastItem;

					// Show last only if it was selected or none was selected
					if (RSMPGS.MainForm.listView_SysLog.SelectedItems.Count == 0)
					{
						bShowLastItem = true;
					}
					else
					{
						if (RSMPGS.MainForm.listView_SysLog.SelectedItems[0].Index == RSMPGS.MainForm.listView_SysLog.Items.Count - 1)
						{
							RSMPGS.MainForm.listView_SysLog.SelectedItems[0].Selected = false;
							bShowLastItem = true;
						}
						else
						{
							bShowLastItem = false;
						}
					}

					for (int iIndex = 0; iIndex < RSMPGS.SysLogItems.Count; iIndex++)
					{
						RSMPGS.MainForm.listView_SysLog.Items.Add(RSMPGS.SysLogItems[iIndex]);
					}

					while (RSMPGS.MainForm.listView_SysLog.Items.Count > 500)
					{
						RSMPGS.MainForm.listView_SysLog.Items.RemoveAt(0);
					}

					RSMPGS.SysLogItems.Clear();

					if (bShowLastItem)
					{
						RSMPGS.MainForm.listView_SysLog.EnsureVisible(RSMPGS.MainForm.listView_SysLog.Items.Count - 1);
						RSMPGS.MainForm.listView_SysLog.Items[RSMPGS.MainForm.listView_SysLog.Items.Count - 1].Selected = true;
					}

					RSMPGS.MainForm.listView_SysLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

					RSMPGS.MainForm.listView_SysLog.EndUpdate();

					RSMPGS.MainForm.Refresh();

				}

			}

		}

		public void DeleteLogFiles(string LogFilePath, string LogFileName)
		{
			IFormatProvider culture = new CultureInfo("en-US");

			string[] LogFiles = Directory.GetFiles(LogFilePath, LogFileName);

			foreach (string LogFile in LogFiles)
			{
				if (LogFile.Length > 12)
				{
					string sDateTime = LogFile.Substring(LogFile.Length - 12, 8);
					try
					{
						DateTime LogFileDateTime = DateTime.ParseExact(sDateTime, "yyyyMMdd", culture);
						if (LogFileDateTime.AddDays(DaysToKeepLogFiles).CompareTo(DateTime.Now) < 0)
						{
							File.Delete(LogFile);
						}
					}
					catch { }
				}
			}
		}

    public void SysLog(Severity severity, string sFormat, params object[] pArg)
    {
      string sLogText = String.Format(sFormat, pArg);
      SysLog(severity, sLogText);
    }

    public void SysLog(Severity severity, string sLogText)
		{

			if (bEnableSysLog == false)
			{
				return;
			}

			string sDateTime = String.Format("{0:HH:mm:ss.fff}", DateTime.Now);

			string sFileName = Path.Combine(sSysLogFilePath, "SysLog_") + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".Log";

			RSMPGS.MainForm.BeginInvoke(RSMPGS.MainForm.DelegateAddSysLogListItem, new Object[] { severity, sDateTime, sLogText });

			lock (this)
			{
				try
				{
					StreamWriter swSysLogFile = File.AppendText(sFileName);
					if (sLogText.Length == 0)
					{
						swSysLogFile.WriteLine("------------------------------------------------------------------------------------------------------------------------");
					}
					else
					{
						swSysLogFile.WriteLine(severity + "\t" + sDateTime + "\t" + sLogText);
					}
					swSysLogFile.Close();
				}
				catch { }
			}
		}

		public void EventLog(string sFormat, params object[] pArg)
		{

			string sLogText = String.Format(sFormat, pArg);
			string sEventType = "";

			if (sLogText.ToLower().Contains("alarm")) sEventType = "Alarm";
			if (sLogText.ToLower().Contains("command")) sEventType = "Command";
			if (sLogText.ToLower().Contains("status")) sEventType = "Status";
			if (sLogText.ToLower().Contains("aggregatedstatus")) sEventType = "AggregatedStatus";

			sLogText = sLogText.Replace(sEventType + ";", "");
			string sFileName = Path.Combine(sEventFilePath, sEventType) + "Event_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".txt";

			lock (this)
			{
				try
				{
					StreamWriter swSysLogFile = File.AppendText(sFileName);
					swSysLogFile.WriteLine(sLogText.Trim());
					swSysLogFile.Close();
				}
				catch { }
			}
		}

		public void AddRawDebugData(bool bNewPacket, int iDirection, bool bForceHexCode, byte[] bBuffer, int iOffset, int iBufferLength)
		{

			lock (this)
			{
				foreach (RSMPGS_Debug DebugForm in RSMPGS.DebugForms)
				{
					try
					{
						DebugForm.BeginInvoke(DebugForm.DelegateAddRawDebugData, new Object[] { DateTime.Now, bNewPacket, iDirection, bForceHexCode, bBuffer, iOffset, iBufferLength });
					}
					catch { }
				}
			}
		}

		public void AddJSonDebugData(int iDirection, string sPacketType, string sDebugData)
		{
			lock (this)
			{
				foreach (RSMPGS_Debug DebugForm in RSMPGS.DebugForms)
				{
					try
					{
						DebugForm.BeginInvoke(DebugForm.DelegateAddJSonDebugData, new Object[] { DateTime.Now, iDirection, sPacketType, sDebugData });
					}
					catch { }
				}
			}
		}
		public string StoreBase64DebugData(string sValue)
		{

			string sBase64Info = "(failed to store)";

			try
			{
				Random Rnd = new Random();
				string sFileName = Path.Combine(cPrivateProfile.DebugFilesPath(), "Base64_") + String.Format("{0:yyyyMMdd}_{0:HHmmss_fff}", DateTime.Now) + "_" + Rnd.Next(4095).ToString("x3") + ".Bin";
				Encoding encoding;
				encoding = Encoding.GetEncoding("IBM437");
				byte[] Base64Bytes = encoding.GetBytes(sValue);
				char[] Base64Chars = encoding.GetChars(Base64Bytes);
				byte[] Base8Bytes = System.Convert.FromBase64CharArray(Base64Chars, 0, Base64Chars.GetLength(0));

				System.IO.FileStream fsBase8 = new System.IO.FileStream(sFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
				System.IO.BinaryWriter bwBase8 = new System.IO.BinaryWriter(fsBase8);
				bwBase8.Write(Base8Bytes);
				fsBase8.Close();
				fsBase8.Dispose();
				bwBase8.Close();
				sBase64Info = "base64 (" + Base8Bytes.GetLength(0).ToString() + " bytes) updated " + String.Format("{0:HH:mm:ss.fff}", DateTime.Now);
				RSMPGS.SysLog.SysLog(cSysLogAndDebug.Severity.Info, "Stored base64 decoded binary data to: {0}", sFileName);
			}
			catch (Exception e)
			{
				RSMPGS.SysLog.SysLog(cSysLogAndDebug.Severity.Error, "Failed to store base64 data: " + e.Message);
			}
			return sBase64Info;
		}
	}

	public class cTcpHelper
	{

		public const int WrapMethod_None = 0;
		public const int WrapMethod_FormFeed = 1;
		public const int WrapMethod_LengthPrefix = 2;

		public static bool CloseAndDeleteStreamAndSocket(ref cSocketStream socketStream, ref TcpClient tcpClient)
		{

			bool bDidCloseSomething = false;

			if (socketStream != null)
			{
				try
				{
          socketStream.Close();
          socketStream = null;
				}
				catch
				{
				}
				bDidCloseSomething = true;
			}
			if (tcpClient != null)
			{
				try
				{
					tcpClient.Close();
					tcpClient = null;
				}
				catch
				{
				}
				bDidCloseSomething = true;
			}

			return bDidCloseSomething;

		}
    public static bool CloseAndDeleteStreamAndSocket(ref NetworkStream networkStream, ref TcpClient tcpClient)
    {

      bool bDidCloseSomething = false;

      if (networkStream != null)
      {
        try
        {
          networkStream.Close();
          networkStream = null;
        }
        catch
        {
        }
        bDidCloseSomething = true;
      }
      if (tcpClient != null)
      {
        try
        {
          tcpClient.Close();
          tcpClient = null;
        }
        catch
        {
        }
        bDidCloseSomething = true;
      }

      return bDidCloseSomething;

    }

  }

  public class cJSonMessageIdAndTimeStamp
	{
		public string PacketType;
		public string MessageId;
		public string SendString;
		public DateTime TimeStamp;
		public double TimeToWaitForAck;
		public bool ResendPacketIfWeGetNoAck;

		public cJSonMessageIdAndTimeStamp(string sPacketType, string sMessageId, string sSendString, double dTimeToWaitForAck, bool bResendPacketIfWeGetNoAck)
		{
			PacketType = sPacketType;
			MessageId = sMessageId;
			SendString = sSendString;
			TimeToWaitForAck = dTimeToWaitForAck;
			TimeStamp = DateTime.Now;
			ResendPacketIfWeGetNoAck = bResendPacketIfWeGetNoAck;
		}

		public bool IsPacketToOld()
		{
			if (cHelper.IsSettingChecked("WaitInfiniteForUnackedPackets"))
			{
				return false;
			}
			else
			{
				return (TimeStamp.AddMilliseconds(TimeToWaitForAck) < DateTime.Now) ? true : false;
			}
		}
	}


  public class ListViewColumnSorter : IComparer
  {
    /// <summary>
    /// Specifies the column to be sorted
    /// </summary>
    private int ColumnToSort;
    /// <summary>
    /// Specifies the order in which to sort (i.e. 'Ascending').
    /// </summary>
    private SortOrder OrderOfSort;
    /// <summary>
    /// Case insensitive comparer object
    /// </summary>
    private CaseInsensitiveComparer ObjectCompare;

    /// <summary>
    /// Class constructor.  Initializes various elements
    /// </summary>
    public ListViewColumnSorter()
    {
      // Initialize the column to '0'
      ColumnToSort = 0;

      // Initialize the sort order to 'none'
      OrderOfSort = SortOrder.None;

      // Initialize the CaseInsensitiveComparer object
      ObjectCompare = new CaseInsensitiveComparer();
    }

    /// <summary>
    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// </summary>
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare(object x, object y)
    {
      int compareResult;
      ListViewItem listviewX, listviewY;

      // Cast the objects to be compared to ListViewItem objects
      listviewX = (ListViewItem)x;
      listviewY = (ListViewItem)y;

      // Compare the two items
      compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

      // Calculate correct return value based on object comparison
      if (OrderOfSort == SortOrder.Ascending)
      {
        // Ascending sort is selected, return normal result of compare operation
        return compareResult;
      }
      else if (OrderOfSort == SortOrder.Descending)
      {
        // Descending sort is selected, return negative result of compare operation
        return (-compareResult);
      }
      else
      {
        // Return '0' to indicate they are equal
        return 0;
      }
    }

    /// <summary>
    /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
    /// </summary>
    public int SortColumn
    {
      set
      {
        ColumnToSort = value;
      }
      get
      {
        return ColumnToSort;
      }
    }

    /// <summary>
    /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
    /// </summary>
    public SortOrder Order
    {
      set
      {
        OrderOfSort = value;
      }
      get
      {
        return OrderOfSort;
      }
    }

  }

  internal static class ListViewExtensions
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct LVCOLUMN
    {
      public Int32 mask;
      public Int32 cx;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszText;
      public IntPtr hbm;
      public Int32 cchTextMax;
      public Int32 fmt;
      public Int32 iSubItem;
      public Int32 iImage;
      public Int32 iOrder;
    }

    const Int32 HDI_WIDTH = 0x0001;
    const Int32 HDI_HEIGHT = HDI_WIDTH;
    const Int32 HDI_TEXT = 0x0002;
    const Int32 HDI_FORMAT = 0x0004;
    const Int32 HDI_LPARAM = 0x0008;
    const Int32 HDI_BITMAP = 0x0010;
    const Int32 HDI_IMAGE = 0x0020;
    const Int32 HDI_DI_SETITEM = 0x0040;
    const Int32 HDI_ORDER = 0x0080;
    const Int32 HDI_FILTER = 0x0100;

    const Int32 HDF_LEFT = 0x0000;
    const Int32 HDF_RIGHT = 0x0001;
    const Int32 HDF_CENTER = 0x0002;
    const Int32 HDF_JUSTIFYMASK = 0x0003;
    const Int32 HDF_RTLREADING = 0x0004;
    const Int32 HDF_OWNERDRAW = 0x8000;
    const Int32 HDF_STRING = 0x4000;
    const Int32 HDF_BITMAP = 0x2000;
    const Int32 HDF_BITMAP_ON_RIGHT = 0x1000;
    const Int32 HDF_IMAGE = 0x0800;
    const Int32 HDF_SORTUP = 0x0400;
    const Int32 HDF_SORTDOWN = 0x0200;

    const Int32 LVM_FIRST = 0x1000;         // List messages
    const Int32 LVM_GETHEADER = LVM_FIRST + 31;
    const Int32 HDM_FIRST = 0x1200;         // Header messages
    const Int32 HDM_SETIMAGELIST = HDM_FIRST + 8;
    const Int32 HDM_GETIMAGELIST = HDM_FIRST + 9;
    const Int32 HDM_GETITEM = HDM_FIRST + 11;
    const Int32 HDM_SETITEM = HDM_FIRST + 12;

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);


    //This method used to set arrow icon
    public static void SetSortIcon(this ListView listView, int columnIndex, SortOrder order)
    {
      IntPtr columnHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

      for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
      {
        IntPtr columnPtr = new IntPtr(columnNumber);
        LVCOLUMN lvColumn = new LVCOLUMN();
        lvColumn.mask = HDI_FORMAT;

        SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

        if (!(order == SortOrder.None) && columnNumber == columnIndex)
        {
          switch (order)
          {
            case System.Windows.Forms.SortOrder.Ascending:
              lvColumn.fmt &= ~HDF_SORTDOWN;
              lvColumn.fmt |= HDF_SORTUP;
              break;
            case System.Windows.Forms.SortOrder.Descending:
              lvColumn.fmt &= ~HDF_SORTUP;
              lvColumn.fmt |= HDF_SORTDOWN;
              break;
          }
          lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
        }
        else
        {
          lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
        }

        SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
      }
    }
  }

  public class ListViewDoubleBuffered : ListView
  {

    public ListViewColumnSorter ColumnSorter = new nsRSMPGS.ListViewColumnSorter();

    public void StopSorting()
    {
      ListViewItemSorter = null;
    }

    public void ResumeSorting()
    {
      ListViewItemSorter = ColumnSorter;
      Sort();
    }

    public ListViewDoubleBuffered()
    {
      ListViewItemSorter = ColumnSorter;
      DoubleBuffered = true;
    }
  }


  public class cFormsHelper
  {

    public static string sFileName = "";

    public static void ColumnClick(object sender, ColumnClickEventArgs e)
    {

      ListViewDoubleBuffered listView = (ListViewDoubleBuffered)sender;

      // Determine if clicked column is already the column that is being sorted.
      if (e.Column == listView.ColumnSorter.SortColumn)
      {

        // Reverse the current sort direction for this column.
        if (listView.ColumnSorter.Order == SortOrder.Ascending)
        {
          listView.ColumnSorter.Order = SortOrder.Descending;
        }
        else
        {
          listView.ColumnSorter.Order = SortOrder.Ascending;
        }
      }
      else
      {
        // Set the column number that is to be sorted; default to ascending.
        listView.ColumnSorter.SortColumn = e.Column;
        listView.ColumnSorter.Order = SortOrder.Ascending;
      }

      // Perform the sort with these new sort options.
      listView.Sort();
      listView.SetSortIcon(listView.ColumnSorter.SortColumn, listView.ColumnSorter.Order);

    }

    public static DialogResult InputStatusBox(string title, string promptText, ref string value, string sType, string sValues, string sComment, bool bReturnCancelIfValueHasNotChanged)
    {

      Form form = new Form();
      Label label = new Label();
      ComboBox comboBox = new ComboBox();
      Button buttonOk = new Button();
      Button buttonCancel = new Button();
      Button buttonBrowse = new Button();

      form.Text = title + " (" + sType + ")";
      label.Text = promptText;
      comboBox.Text = value;
      comboBox.DropDownStyle = ComboBoxStyle.DropDown;

      if (sType.ToLower() == "boolean")
      {
        comboBox.Items.Add("true");
        comboBox.Items.Add("false");
      }
      else
      {
        if (sValues.StartsWith("["))
        {
          form.Text += " " + sValues;
        }
        else
        {
          foreach (string sValue in sValues.Split('\n'))
          {
            comboBox.Items.Add(sValue.TrimStart('-'));
          }
        }
      }

      buttonBrowse.Text = "Browse...";
      buttonCancel.Text = "Cancel";
      buttonOk.Text = "OK";

      buttonOk.DialogResult = DialogResult.OK;
      buttonCancel.DialogResult = DialogResult.Cancel;

      if (sType.ToLower() == "base64")
      {
        comboBox.SetBounds(131, 17, 260, 22);
        buttonBrowse.SetBounds(408, 13, 93, 31);
        buttonCancel.SetBounds(309, 59, 93, 31);
        buttonOk.SetBounds(408, 59, 93, 31);
        form.ClientSize = new Size(530, 100);      }
      else
      {
        comboBox.SetBounds(131, 17, 270, 22);
        //textBox.SetBounds(12, 36, 372, 20);
        buttonBrowse.Visible = false;
        buttonCancel.SetBounds(209, 59, 93, 31);
        buttonOk.SetBounds(308, 59, 93, 31);
        form.ClientSize = new Size(430, 100);
      }

      label.SetBounds(12, 17, 88, 17);

      label.AutoSize = true;
      label.TextAlign = ContentAlignment.MiddleRight;

      comboBox.Anchor = comboBox.Anchor | AnchorStyles.Right;
      buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonBrowse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

      form.Controls.AddRange(new Control[] { label, comboBox, buttonOk, buttonCancel, buttonBrowse });
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.MinimizeBox = false;
      form.MaximizeBox = false;
      form.AcceptButton = buttonOk;
      form.CancelButton = buttonCancel;

      buttonBrowse.Click += new System.EventHandler(buttonBrowse_Click);

      comboBox.SelectedIndexChanged += new System.EventHandler(InputStatusBoxComboBox_SelectionChanged);
      comboBox.TextChanged += new System.EventHandler(InputStatusBoxComboBox_TextChanged);

      comboBox.Tag = new nsRSMPGS.cInputBoxValue(sType, sValues);

      DialogResult dialogResult = form.ShowDialog();

      if (bReturnCancelIfValueHasNotChanged)
      {
        // Changed value ?
        if (comboBox.Text.Equals(value))
        {
          return DialogResult.Cancel;
        }
        else
        {

/*
      switch (sType.ToLower())
      {

        case "string":
          bValueIsValid = true;
          break;

        case "integer":
          try
          {
            Int16 iValue = Int16.Parse(sValue);
            bValueIsValid = true;
          }
          catch { }
          break;

        case "long":
          try
          {
            Int32 iValue = Int32.Parse(sValue);
            bValueIsValid = true;
          }
          catch { }
          break;

        case "real":
          try
          {
            Double dValue = Double.Parse(sValue);
            bValueIsValid = true;
          }
          catch { }
          break;

        case "boolean":
          bValueIsValid = sValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            sValue.Equals("false", StringComparison.OrdinalIgnoreCase) ||
            sValue.Equals("0", StringComparison.OrdinalIgnoreCase) ||
            sValue.Equals("1", StringComparison.OrdinalIgnoreCase);
          break;

        case "base64":
          try
          {
            Encoding encoding;
            encoding = Encoding.GetEncoding("IBM437");
            byte[] Base64Bytes = encoding.GetBytes(sValue);
            char[] Base64Chars = encoding.GetChars(Base64Bytes);
            byte[] Base8Bytes = System.Convert.FromBase64CharArray(Base64Chars, 0, Base64Chars.GetLength(0));
            bValueIsValid = true;
          }
          catch { }
          break;

        case "ordinal":

          if (NegotiatedRSMPVersion == RSMPVersion.RSMP_3_1_1 || NegotiatedRSMPVersion == RSMPVersion.RSMP_3_1_2)
          {
            try
            {
              UInt32 iValue = UInt32.Parse(sValue);
              bValueIsValid = true;
            }
            catch { }
          }
          break;

        // These are all valid
        case "raw":
        case "scale":
        case "unit":
          if (NegotiatedRSMPVersion == RSMPVersion.RSMP_3_1_1 || NegotiatedRSMPVersion == RSMPVersion.RSMP_3_1_2)
          {
            bValueIsValid = true;
          }
          break;
      }


  */

          value = comboBox.Text;
          return dialogResult;
        }
      }
      else
      {
        value = comboBox.Text;
        return dialogResult;
      }

    }

    private static void InputStatusBoxComboBox_SelectionChanged(object sender, EventArgs e)
    {
      InputStatusBoxComboBox_ValidateValue((ComboBox)sender);
    }

    private static void InputStatusBoxComboBox_TextChanged(object sender, EventArgs e)
    {
      InputStatusBoxComboBox_ValidateValue((ComboBox)sender);
    }

    private static void InputStatusBoxComboBox_ValidateValue(ComboBox comboBox)
    {

      cInputBoxValue InputBoxValue = (cInputBoxValue)comboBox.Tag;

      if (RSMPGS.JSon.ValidateTypeAndRange(InputBoxValue.sType, comboBox.Text, InputBoxValue.sValues))
      {
        comboBox.ForeColor = default(Color);
        comboBox.BackColor = default(Color);
      }
      else
      {
        comboBox.ForeColor = Color.White;
        comboBox.BackColor = Color.Red;
      }

    }

    public static DialogResult InputBox(string title, string promptText, ref string value, bool bAllowFileBrowse, bool bReturnCancelIfValueHasNotChanged)
    {
      return InputBox(title, promptText, ref value, bAllowFileBrowse, bReturnCancelIfValueHasNotChanged, false);
    }

    public static DialogResult InputBox(string title, string promptText, ref string value, bool bAllowFileBrowse, bool bReturnCancelIfValueHasNotChanged, bool bIsPassword)
    {

      Form form = new Form();
      Label label = new Label();
      TextBox textBox = new TextBox();
      Button buttonOk = new Button();
      Button buttonCancel = new Button();
      Button buttonBrowse = new Button();

      form.Text = title;
      label.Text = promptText;
      textBox.Text = value;
      textBox.TextAlign = HorizontalAlignment.Center;

      if (bIsPassword)
      {
        textBox.PasswordChar = '*';
      }

      buttonBrowse.Text = "Browse...";
      buttonCancel.Text = "Cancel";
      buttonOk.Text = "OK";

      buttonOk.DialogResult = DialogResult.OK;
      buttonCancel.DialogResult = DialogResult.Cancel;

      if (bAllowFileBrowse)
      {
        textBox.SetBounds(131, 17, 260, 22);
        buttonBrowse.SetBounds(408, 13, 93, 31);
        buttonCancel.SetBounds(309, 59, 93, 31);
        buttonOk.SetBounds(408, 59, 93, 31);
        form.ClientSize = new Size(530, 100);
      }
      else
      {
        textBox.SetBounds(131, 17, 270, 22);
        //textBox.SetBounds(12, 36, 372, 20);
        buttonBrowse.Visible = false;
        buttonCancel.SetBounds(209, 59, 93, 31);
        buttonOk.SetBounds(308, 59, 93, 31);
        form.ClientSize = new Size(430, 100);
      }

      label.SetBounds(12, 17, 88, 17);

      label.AutoSize = true;
      label.TextAlign = ContentAlignment.MiddleRight;

      textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
      buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonBrowse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

      form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel, buttonBrowse });
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.MinimizeBox = false;
      form.MaximizeBox = false;
      form.AcceptButton = buttonOk;
      form.CancelButton = buttonCancel;

      buttonBrowse.Click += new System.EventHandler(buttonBrowse_Click);

      DialogResult dialogResult = form.ShowDialog();

      if (bReturnCancelIfValueHasNotChanged)
      {
        // Changed value ?
        if (textBox.Text.Equals(value))
        {
          return DialogResult.Cancel;
        }
        else
        {
          value = textBox.Text;
          return dialogResult;
        }
      }
      else
      {
        value = textBox.Text;
        return dialogResult;
      }

    }

    private static void buttonBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = sFileName;
      openFileDialog.Filter = "All files|*.*";
      openFileDialog.RestoreDirectory = true;
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        sFileName = openFileDialog.FileName;
      }
    }
  }
  public class cInputBoxValue
  {
    public string sType;
    public string sValues;
    public cInputBoxValue(string sType, string sValues)
    {
      this.sType = sType;
      this.sValues = sValues;
    }
  }

}