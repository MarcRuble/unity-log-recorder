using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogRecorder
{
    public class FrameController : MonoBehaviour
    {
        #region PUBLIC ATTRIBUTES
        public float FramesPerUnityFrame
        {
            get { return framesPerUnityFrame; }
            set { framesPerUnityFrame = value; }
        }

        public int Frame
        {
            get { return (int)Mathf.Round(frameProgress); }
            set { frameProgress = (float)value; }
        }    

        public static FrameController instance;
        #endregion

        #region PRIVATE ATTRIBUTES
        private float framesPerUnityFrame = 1f;
        private float frameProgress = 0f;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            frameProgress += framesPerUnityFrame;
        }
        #endregion

        #region PUBLIC METHODS
        public void SetFrameProgress(float frameProgress)
        {
            this.frameProgress = frameProgress;
        }

        public float GetFrameProgress()
        {
            return frameProgress;
        }
        #endregion
    }
}
