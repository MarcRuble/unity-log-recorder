﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class ScaleLogger : FloatLogger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Scale";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return Utils.Vector3ToString(transform.localScale, decimalPoints);
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            transform.localScale = Utils.ParseVector3(value);
        }
    }
}
