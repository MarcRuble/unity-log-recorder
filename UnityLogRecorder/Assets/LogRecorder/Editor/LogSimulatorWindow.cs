using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LogRecorder
{
    public class LogSimulatorWindow : EditorWindow
    {
        private float pausedByDrag = 0f;
        private float pausedByButton = 0f;
        private int speedIndex = 2;
        private Texture2D playTexture;
        private Texture2D pauseTexture;

        [MenuItem("Window/Log Simulator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LogSimulatorWindow));
        }

        private void OnEnable()
        {
            playTexture = Resources.Load<Texture2D>("Images/play");
            pauseTexture = Resources.Load<Texture2D>("Images/pause");
        }

        private void OnGUI()
        {
            // only in play mode
            if (!EditorApplication.isPlaying || LogSimulator.instance.mode != LogRecorderMode.SIMULATE)
                return;

            // get some usefull variables
            FrameController frameCon = FrameController.instance;
            float padding = 15;


            /**************/
            /*** SLIDER ***/
            /**************/
            float sliderWidth = 20;
            Rect progressRect = new Rect(4*padding-sliderWidth/2f, padding, sliderWidth, position.height-6*padding);

            // check if progress bar is currently moved
            if (Event.current.type == EventType.MouseDown
                || Event.current.type == EventType.MouseDrag
                && progressRect.Contains(Event.current.mousePosition))
            {
                // pause frame controller
                pausedByDrag = frameCon.FramesPerUnityFrame;
                frameCon.FramesPerUnityFrame = 0f;
            }
            else
            {
                // give back control to frame controller
                if (pausedByDrag != 0f)
                {
                    frameCon.FramesPerUnityFrame = pausedByDrag;
                    pausedByDrag = 0f;
                }
            }

            // progress bar 
            frameCon.SetFrameProgress(
                GUI.VerticalSlider(progressRect, frameCon.GetFrameProgress(), 
                LogSimulator.instance.minFrame, LogSimulator.instance.maxFrame));


            /********************/
            /*** PAUSE/RESUME ***/
            /********************/
            bool running = frameCon.FramesPerUnityFrame != 0f;
            float buttonWidth = 50f;
            Rect buttonRect = new Rect(4 * padding - buttonWidth / 2f,
                position.height-buttonWidth-padding, buttonWidth, buttonWidth);
            GUIStyle style = new GUIStyle("button");
            style.padding = new RectOffset(10, 10, 10, 10);
            if (GUI.Button(buttonRect, running ? pauseTexture : playTexture, style))
            {
                if (running)
                {
                    // pause frame controller
                    pausedByButton = frameCon.FramesPerUnityFrame;
                    frameCon.FramesPerUnityFrame = 0f;
                }
                else
                {
                    // resume frame controller
                    frameCon.FramesPerUnityFrame = pausedByButton;
                    pausedByButton = 0f;
                }
            }


            /*************/
            /*** SPEED ***/
            /*************/
            float speedWidth = 60f;
            float speedHeight = 20f;
            Rect speedRect = new Rect(buttonRect.x + buttonRect.width + padding + speedWidth,
                position.height - speedHeight - padding, speedWidth, speedHeight);
            string[] speedSteps = { "0.5", "0.75", "1.0", "1.5", "2.0" };
            GUI.Label(new Rect(speedRect.x - speedWidth, speedRect.y, speedWidth, speedHeight), "Speed");
            int newIndex = EditorGUI.Popup(speedRect, speedIndex, speedSteps);
            if (newIndex != speedIndex)
            {
                speedIndex = newIndex;
                frameCon.FramesPerUnityFrame = Utils.StringToFloat(speedSteps[speedIndex]);
            }

            /*******************/
            /*** ANNOTATIONS ***/
            /*******************/
            Dictionary<int, string> annotations = LogSimulator.instance.GetAnnotations();
            style.padding = new RectOffset(2, 2, 2, 2);

            foreach (int key in annotations.Keys)
            {
                float progressFraction = (float)key / (float)LogSimulator.instance.maxFrame;
                string message = annotations[key];
                Rect annoRect = new Rect(
                    progressRect.x + progressRect.width + 2 * padding,
                    progressRect.y + progressFraction * progressRect.height,
                    message.Length * 7f,
                    20f);
                if (GUI.Button(annoRect, message, style))
                    frameCon.SetFrameProgress(key);
            }
        }

        private void Update()
        {
            Repaint();
        }
    }
}
