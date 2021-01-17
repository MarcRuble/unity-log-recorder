using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace LogRecorder
{
    public enum LogRecorderMode
    {
        RECORD,
        SIMULATE,
        NONE
    }

    public class LogSimulator : MonoBehaviour
    {
        #region PUBLIC ATTRIBUTES
        // if a recording or simulation should be run
        public LogRecorderMode mode;

        // event called in simulation mode
        // to disable game logic and prevent
        // interference with simulation
        public UnityEvent disableLogic;

        // max and min frame values
        public int minFrame = int.MaxValue, maxFrame = 0;

        public static LogSimulator instance;
        #endregion

        #region EVENTS
        public static event Action OnLogInit;
        public static event Action OnLogUpdate;
        public static event Action OnLogSave;
        #endregion

        #region PRIVATE ATTRIBUTES
        // all loggers in the scene
        private LoggerController[] loggers;

        // for each logger controller,
        // a dictionary mapping frame count to line of values
        private Dictionary<int, string[]>[] logData;

        // mapping frame count to annotation message
        private Dictionary<int, string> annotations;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            instance = this;

            // initialize logger controllers
            loggers = Resources.FindObjectsOfTypeAll<LoggerController>();
            foreach (LoggerController logger in loggers)
            {
                if (mode == LogRecorderMode.RECORD)
                    logger.Subscribe();

                logger.Init();
            }

            if (mode == LogRecorderMode.SIMULATE)
            {
                // deactivate global control scripts
                if (disableLogic != null)
                    disableLogic.Invoke();

                // initialize log data containers
                logData = new Dictionary<int, string[]>[loggers.Length];
                annotations = new Dictionary<int, string>();
            }  
        }

        private void Start()
        {
            if (mode != LogRecorderMode.SIMULATE)
                return;

            // read the logged files
            for (int i = 0; i < loggers.Length; i++)
            {
                string[][] table = CSVReadWrite.Read(loggers[i].fileName);
                if (table == null)
                    continue; // no file -> skip

                // find column index of frame property
                int frameIndex = Utils.FindStringInArray(table[0], "Frame");

                if (frameIndex < 0)
                {
                    Debug.LogError("[LogError] Cannot simulate file "
                        + loggers[i].fileName + " without property Frame");
                    mode = LogRecorderMode.NONE;
                    return;
                }

                // find column index of annotation (if available)
                int annoIndex = Utils.FindStringInArray(table[0], "Annotation");

                // create a dictionary for this logger controller
                logData[i] = new Dictionary<int, string[]>();

                // go through lines (skipping header)
                for (int l = 1; l < table.Length; l++)
                {
                    // get frame for this line
                    int frame = int.Parse(table[l][frameIndex]);

                    // check max and min
                    if (frame < minFrame)
                        minFrame = frame;
                    if (frame > maxFrame)
                        maxFrame = frame;

                    // add this line values to dictionary
                    logData[i].Add(frame, table[l]);

                    // if available and not empty, add annotation to dictionary
                    if (annoIndex >= 0 && table[l][annoIndex].Length > 0)
                        annotations.Add(frame, table[l][annoIndex]);
                }
            }
        }

        private void Update()
        {
            if (mode == LogRecorderMode.RECORD)
                OnLogUpdate();
            if (mode != LogRecorderMode.SIMULATE)
                return;

            int currentFrame = FrameController.instance.Frame;

            // simulate situation of logged frame
            for (int i = 0; i < loggers.Length; i++)
            {
                if (logData[i] != null && logData[i].ContainsKey(currentFrame))
                {
                    string[] row = logData[i][currentFrame];

                    for (int j = 0; j < row.Length; j++)
                        loggers[i].SetProperty(j, row[j]);
                }
            }
        }

        private void OnApplicationQuit()
        {
            if (mode == LogRecorderMode.RECORD)
                OnLogSave();
        }
        #endregion

        #region PUBLIC METHODS
        public Dictionary<int,string> GetAnnotations()
        {
            return annotations;
        }
        #endregion
    }
}
