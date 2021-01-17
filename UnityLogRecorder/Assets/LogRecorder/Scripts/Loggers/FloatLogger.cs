using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;

namespace LogRecorder
{
    /*
     * Base class for loggers with floating point attributes.
     * */
    public abstract class FloatLogger : Logger
    {
        [Tooltip("Number of decimal points saved for floating point attributes")]
        public int decimalPoints = 4;
    }
}
