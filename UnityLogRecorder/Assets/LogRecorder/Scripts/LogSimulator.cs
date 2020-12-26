using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class LogSimulator : MonoBehaviour
    {
        #region PUBLIC ATTRIBUTES
        // if a simulation should be run
        public bool simulate = false;

        // gameobject to deactivate when simulating
        public GameObject globalControl;

        // max and min frame values
        public int minFrame = int.MaxValue, maxFrame = 0;

        public static LogSimulator instance;
        #endregion

        #region PRIVATE ATTRIBUTES
        // all loggers in the scene
        private LoggerController[] loggers;

        // for each logger controller,
        // a dictionary mapping frame count to line of values
        private Dictionary<int, string[]>[] logData;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            instance = this;

            if (simulate)
            {
                // deactivate global control scripts
                if (globalControl != null)
                    globalControl.SetActive(false);

                // collect loggers
                loggers = FindObjectsOfType<LoggerController>();
                logData = new Dictionary<int, string[]>[loggers.Length];

                // tell loggers to not save the current run
                foreach (LoggerController logger in loggers)
                    logger.saveLog = false;
            }
        }

        private void Start()
        {
            if (!simulate)
                return;

            // read the logged files
            for (int i = 0; i < loggers.Length; i++)
            {
                string[][] table = CSVReadWrite.Read(loggers[i].fileName);

                // find column index of frame property
                int frameIndex = Utils.FindStringInArray(table[0], "Frame");

                if (frameIndex < 0)
                {
                    Debug.LogError("[LogError] Cannot simulate file "
                        + loggers[i].fileName + " without property Frame");
                    simulate = false;
                    return;
                }

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
                }
            }
        }

        private void Update()
        {
            if (!simulate)
                return;

            int currentFrame = FrameController.instance.Frame;

            // simulate situation of logged frame
            for (int i = 0; i < loggers.Length; i++)
            {
                if (logData[i].ContainsKey(currentFrame))
                {
                    string[] row = logData[i][currentFrame];

                    for (int j = 0; j < row.Length; j++)
                        loggers[i].SetProperty(j, row[j]);
                }
            }
        }
        #endregion
    }
}
