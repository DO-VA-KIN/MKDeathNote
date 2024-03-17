using System;
using System.IO;
using System.Windows.Threading;
using System.Xml.Serialization;

using Snake.Models;

namespace Snake.ViewModels
{
    [Serializable]
    public struct SettingsStruct
    {
        public ushort Speed { get; set; }
        public uint IterationsCount { get; set; }
        public bool TimeSkip { get; set; }
    }

    public static class Settings
    {
        private const string settingsFileName = "Settings.xml";
        public static DispatcherTimer TimerAutosave = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };

        public static SettingsStruct Values;
        private static Exception LastException;

        public static Exception GetException()
        { return LastException; }

        public static bool Initialize()
        {
            if (TimerAutosave != null)
                TimerAutosave.Tick += TimerAutosave_Tick;

            if (File.Exists(settingsFileName))
            {
                ReadSettings();
                return true;
            }
            else
            {
                SetDefault();
                SaveSettings();
                return false;
            }
        }
      

        public static void Dispose()
        {
            TimerAutosave.Stop();
            SaveSettings();
        }


        private static void TimerAutosave_Tick(object sender, EventArgs e)
        {
            SaveSettings();
        }

        public static void SetDefault()
        {
            Values = new SettingsStruct
            {
                Speed = 200,
                TimeSkip = true,
                IterationsCount = 1000
            };
        }


        public static bool SaveSettings()
        {
            try
            {
                if (File.Exists(settingsFileName))
                    DelSettingsFile();
                XmlSerializer ser = new XmlSerializer(typeof(SettingsStruct));
                using (FileStream fs = new FileStream(settingsFileName, FileMode.Create))
                    ser.Serialize(fs, Values);

                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                return false;
            }
        }

        public static bool ReadSettings()
        {
            try
            {
                File.SetAttributes(settingsFileName, FileAttributes.Normal);
                XmlSerializer ser = new XmlSerializer(typeof(SettingsStruct));
                using (FileStream fs = new FileStream(settingsFileName, FileMode.Open))
                    Values = (SettingsStruct)ser.Deserialize(fs);
                File.SetAttributes(settingsFileName, FileAttributes.System);

                return true;
            }
            catch (Exception ex)
            {
                SetDefault();
                LastException = ex;
                return false;
            }
        }


        public static bool DelSettingsFile()
        {
            AnyFuncs.KillProcessByName(settingsFileName);
            return AnyFuncs.DeleteFile(settingsFileName);
        }
    }
}


