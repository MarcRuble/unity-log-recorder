using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace LogRecorder
{
    public class CSVReadWrite
    {
        // Writes the given data as CSV file with given name.
        public static void Write(List<string[]> data, string name, int folderID)
        {
            // collect data
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
                output[i] = data[i];

            int length = output.GetLength(0);
            string delimiter = ",";

            // write data to string
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));

            // make sure folder exists
            Directory.CreateDirectory(GetMainFolderPath() + "/" + folderID);
            string filePath = GetFilePath(name, folderID);

            // write data to file
            StreamWriter outStream = File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            Debug.LogWarning("<b>Log saved to </b>" + filePath);
        }

        // Reads the file with given name and returns the content as a 2D array.
        public static string[][] Read(string name)
        {
            // find file
            int highestFolder = GetHighestFolderName();

            if (highestFolder < 0)
            {
                Debug.LogError("[CSV-READ] Could not read because no numeric folder was available");
                return null;
            }

            string[] content;
            try
            {
                content = File.ReadAllLines(GetFilePath(name, highestFolder));
            } 
            catch(System.Exception e)
            {
                Debug.LogError("[CSV-READ] Skipping " + name + " because no file was available");
                return null;
            }

            // ignore empty last line
            string[][] table = new string[
                content[content.Length-1].Length > 0 ? content.Length : content.Length-1][];

            for (int i = 0; i < table.Length; i++)
                table[i] = content[i].Split(',');

            return table;
        }

        // Returns the highest among the numeric folder names.
        public static int GetHighestFolderName()
        {
            List<string> names = GetFolderNames().ToList();
            names.Sort();

            // find highest that is an integer
            for (int i = names.Count - 1; i >= 0; i--)
            {
                int highest;
                if (int.TryParse(names[i], out highest))
                    return highest;
            }
            // signal there was no numeric folder name
            return -1;
        }

        private static string GetMainFolderPath()
        {
            return Application.dataPath + "/CSV";
        }

        private static string GetFilePath(string name, int folderID)
        {
            return GetMainFolderPath() + "/" + folderID + "/" + name + ".csv";
        }

        private static string[] GetFolderNames()
        {
            return Directory.GetDirectories(GetMainFolderPath())
                            .Select(Path.GetFileName)
                            .ToArray();
        }
    }
}
