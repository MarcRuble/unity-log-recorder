using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class FrameLogger : Logger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Frame";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return Time.frameCount.ToString();
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            return;
        }
    }
}
