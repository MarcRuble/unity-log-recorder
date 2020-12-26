using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class RotationLogger : Logger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Rotation";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return transform.rotation.ToString("F7").Replace(",", string.Empty);
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            transform.rotation = Utils.ParseQuaternion(value);
        }
    }
}
