using System;
using System.IO;

namespace PromptBox
{
    public static class HistoryManager
    {
        private static string HistoryFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PromptBox");
        private static string HistoryFilePath
        {
            get
            {
                Directory.CreateDirectory(HistoryFolder);
                string path = Path.Combine(HistoryFolder, "History.bin");
                if (!File.Exists(path))
                    ApplicationData.Save(path, new ApplicationData());
                return path;
            }
        }
        public static History[] GetHistory()
        {
            return ApplicationData.Load(HistoryFilePath).Histories.ToArray();
        }
        public static void AddHistory(History item)
        {
            ApplicationData data = ApplicationData.Load(HistoryFilePath);
            data.Histories.Add(item);
            ApplicationData.Save(HistoryFilePath, data);
        }
    }
}
