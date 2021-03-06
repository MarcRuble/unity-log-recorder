﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
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

        public static string FloatToString(float f, int d=4)
        {
            f = (float)Math.Round(f, d);
            return f.ToString().Replace(",", ".");
        }

        public static string Vector3ToString(Vector3 v, int d=4)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");
            sb.Append(FloatToString(v.x, d));
            sb.Append(" ");
            sb.Append(FloatToString(v.y, d));
            sb.Append(" ");
            sb.Append(FloatToString(v.z, d));
            sb.Append(")");

            return sb.ToString();
        }

        public static string QuaternionToString(Quaternion q, int d = 4)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");
            sb.Append(FloatToString(q.x, d));
            sb.Append(" ");
            sb.Append(FloatToString(q.y, d));
            sb.Append(" ");
            sb.Append(FloatToString(q.z, d));
            sb.Append(" ");
            sb.Append(FloatToString(q.w, d));
            sb.Append(")");

            return sb.ToString();
        }

        public static float StringToFloat(string s)
        {
            return float.Parse(s.Replace('.', ','));
        }

        public static string GetTimestamp()
        {
            return System.DateTime.Now.ToString().Replace(' ', '_').Replace(".", "").Replace(":", "");
        }
    }
}
