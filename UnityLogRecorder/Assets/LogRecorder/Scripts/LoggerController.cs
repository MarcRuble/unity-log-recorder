using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    /*
     * Control logic for all loggers on the game object.
     * */
    public class LoggerController : MonoBehaviour
    {
        #region PUBLIC FIELDS
        [Tooltip("If the log should be saved to a file")]
        public bool saveLog = true;

        [Tooltip("Name of the log file")]
        public string fileName = "";

        [Tooltip("When a log should be created")]
        public LogMode mode;

        [Tooltip("Number of frames to wait if WAIT_INTERVAL is selected (1 = EACH_FRAME)")]
        public int waitInterval = 1;
        #endregion

        #region PRIVATE FIELDS
        // array of loggers to control
        private Logger[] loggers;
        // list of rows containing the log
        private List<string[]> log;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            // check file name
            if (fileName == "")
                fileName = gameObject.name;

            // generate a frame logger if necessary
            if (GetComponent<FrameLogger>() == null)
                gameObject.AddComponent<FrameLogger>();

            // setup log list
            log = new List<string[]>();

            // determine loggers to control
            loggers = GetComponents<Logger>();

            // add first row with categories
            string[] header = new string[loggers.Length];

            for (int i = 0; i < loggers.Length; i++)
                header[i] = loggers[i].GetName();

            log.Add(header);
        }

        private void Update()
        {
            if (!LogSimulator.instance.simulate
            &&  mode == LogMode.EACH_FRAME
            || (mode == LogMode.WAIT_INTERVAL && Time.frameCount % waitInterval == 0))
                AddLogRow();
        }

        private void OnApplicationQuit()
        {
            if (saveLog)
                CSVReadWrite.Write(log, fileName);
        }
        #endregion

        #region PUBLIC METHODS
        // Sets the given value for property with given index.
        public void SetProperty(int index, string value)
        {
            loggers[index].SetValue(value);
        }

        // Triggers logging of property values.
        public void TriggerLog(string annotation=null)
        {
            if (annotation != null)
            {
                AnnotationLogger annotate = GetComponent<AnnotationLogger>();
                if (annotate != null)
                    annotate.SetValue(annotation);
                else
                {
                    Debug.LogError("[LogError] Could not find an AnnotationLogger, but was given an annotation");
                    return;
                }
            }

            AddLogRow();
        }
        #endregion

        #region PRIVATE METHODS
        private void AddLogRow()
        {
            string[] row = new string[loggers.Length];

            for (int i = 0; i < loggers.Length; i++)
                row[i] = loggers[i].GetValue();

            log.Add(row);
        }
        #endregion
    }

    public enum LogMode
    {
        EACH_FRAME,
        WAIT_INTERVAL,
        ON_TRIGGER
    }
}
