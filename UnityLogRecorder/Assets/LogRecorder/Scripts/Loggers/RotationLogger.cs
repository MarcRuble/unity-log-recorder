using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class RotationLogger : FloatLogger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Rotation";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return Utils.QuaternionToString(transform.rotation, decimalPoints);
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            transform.rotation = Utils.ParseQuaternion(value);
        }
    }
}
