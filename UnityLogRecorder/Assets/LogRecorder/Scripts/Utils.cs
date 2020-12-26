using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class Utils
    {
        public static Vector3 ParseVector3(string s)
        {
            s = s.Trim().Trim('(').Trim(')');
            string[] xyz = s.Split(' ');
            return new Vector3(StringToFloat(xyz[0]), StringToFloat(xyz[1]), StringToFloat(xyz[2]));
        }

        public static Quaternion ParseQuaternion(string s)
        {
            s = s.Trim().Trim('(').Trim(')');
            string[] xyzw = s.Split(' ');
            return new Quaternion(StringToFloat(xyzw[0]), StringToFloat(xyzw[1]), 
                                    StringToFloat(xyzw[2]), StringToFloat(xyzw[3]));
        }

        public static int FindStringInArray(string[] row, string val)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i].Equals(val))
                    return i;
            }
            return -1;
        }

        public static float StringToFloat(string s)
        {
            return float.Parse(s.Replace('.', ','));
        }
    }
}
