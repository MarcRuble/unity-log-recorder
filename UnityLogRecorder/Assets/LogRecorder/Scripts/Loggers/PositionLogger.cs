using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class PositionLogger : Logger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Position";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return transform.position.ToString("F7").Replace(",", string.Empty);
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            transform.position = Utils.ParseVector3(value);
        }
    }
}
