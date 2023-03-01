using K4os.Compression.LZ4.Streams;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace PromptBox
{
    public static class HistoryHelper
    {
        public static string ToMarkdown(this History[] histories)
        {
            StringBuilder sb = new StringBuilder();
            foreach (History history in histories)
            {
                sb.AppendLine($"# {history.Time:yyyy-MM-dd hh:mm:ss}");
                sb.AppendLine();
                sb.AppendLine(history.Input);
                sb.AppendLine();
                sb.AppendLine(history.Output);
            }
            return sb.ToString();
        }
    }
    public record History(string Input, string Output, DateTime Time);
    public class ApplicationData
    {
        public List<History> Histories { get; set; } = new List<History>();

        #region Methods
        public static void Save(string filepath, ApplicationData data, bool compressed = true)
        {
            if (compressed)
            {
                using LZ4EncoderStream stream = LZ4Stream.Encode(File.Create(filepath));
                using BinaryWriter writer = new(stream, Encoding.UTF8, false);
                WriteToStream(writer, data);
            }
            else
            {
                using FileStream stream = File.Open(filepath, FileMode.Create);
                using BinaryWriter writer = new(stream, Encoding.UTF8, false);
                WriteToStream(writer, data);
            }
        }
        public static ApplicationData Load(string filepath, bool compressed = true)
        {
            if (compressed)
            {
                using LZ4DecoderStream source = LZ4Stream.Decode(File.OpenRead(filepath));
                using BinaryReader reader = new(source, Encoding.UTF8, false);
                return ReadFromStream(reader);
            }
            else
            {
                using FileStream stream = File.Open(filepath, FileMode.Open);
                using BinaryReader reader = new(stream, Encoding.UTF8, false);
                return ReadFromStream(reader);
            }
        }
        #endregion

        #region Routines
        private static void WriteToStream(BinaryWriter writer, ApplicationData data)
        {
            writer.Write(data.Histories.Count);
            foreach (History history in data.Histories)
            {
                writer.Write(history.Input);
                writer.Write(history.Output);
                writer.Write(history.Time.ToString("yyyy-MM-dd hh:mm:ss"));
            }
        }
        private static ApplicationData ReadFromStream(BinaryReader reader)
        {
            ApplicationData applicationData = new();

            {
                var historyCount = reader.ReadInt32();
                for (int i = 0; i < historyCount; i++)
                {
                    History history = new History(reader.ReadString(), reader.ReadString(), DateTime.ParseExact(reader.ReadString(), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture));
                    applicationData.Histories.Add(history);
                }
            }

            return applicationData;
        }
        #endregion
    }
}
