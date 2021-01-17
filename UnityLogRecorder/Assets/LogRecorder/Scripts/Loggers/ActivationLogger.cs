using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class ActivationLogger : Logger
    {
        // Returns the name of this logger type.
        public override string GetName()
        {
            return "Activation";
        }

        // Returns the current observed value description.
        public override string GetValue()
        {
            return gameObject.activeInHierarchy ? "T" : "F";
        }

        // Interprets and applies the given value description.
        public override void SetValue(string value)
        {
            gameObject.SetActive(value.Equals("T"));
        }
    }
}
