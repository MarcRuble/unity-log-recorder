using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class AnnotationLogger : Logger
    {
        public string message = "";

        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Annotation";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return message;
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            message = value;
        }
    }
}
