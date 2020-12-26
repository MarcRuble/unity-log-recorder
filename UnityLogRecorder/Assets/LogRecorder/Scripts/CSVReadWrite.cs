using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LogRecorder
{
    public class CSVReadWrite
    {
        // Writes the given data as CSV file with given name.
        public static void Write(List<string[]> data, string name)
        {
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
                output[i] = data[i];

            int length = output.GetLength(0);
            string delimiter = ",";

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));

            string filePath = GetPath(name);

            StreamWriter outStream = File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            Debug.LogWarning("<b>Log saved to </b>" + filePath);
        }

        // Reads the file with given name and returns the content as a 2D array.
        public static string[][] Read(string name)
        {
            string[] content = File.ReadAllLines(GetPath(name));
            // ignore empty last line
            string[][] table = new string[
                content[content.Length-1].Length > 0 ? content.Length : content.Length-1][];

            for (int i = 0; i < table.Length; i++)
                table[i] = content[i].Split(',');

            return table;
        }

        private static string GetPath(string name)
        {
            return Application.dataPath + "/CSV/" + name + ".csv";
        }
    }
}
