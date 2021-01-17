using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class TimeLogger : FloatLogger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Time";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return Utils.FloatToString(Time.time, decimalPoints);
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            return;
        }
    }
}
