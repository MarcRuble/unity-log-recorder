using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    /*
     * Base class for loggers of different attributes.
     * */
    public abstract class Logger : MonoBehaviour
    {
        [Tooltip("If this logger should be logging")]
        public bool log = true;

        // Returns the name of this logger type.
        public abstract string GetName();

        // Returns the current observed value description.
        public abstract string GetValue();

        // Interprets and applies the given value description.
        public abstract void SetValue(string value);
    }
}
