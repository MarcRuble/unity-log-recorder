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
            if (!EditorApplication.isPlaying)
                return;

            FrameController frameCon = FrameController.instance;
            Rect progressRect = new Rect(10, 10, position.width, 20);

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
                EditorGUI.Slider(progressRect, frameCon.GetFrameProgress(), 
                LogSimulator.instance.minFrame, LogSimulator.instance.maxFrame));

            // pause or resume button
            bool running = frameCon.FramesPerUnityFrame != 0f;
            Rect buttonRect = new Rect(position.x + position.width / 2, 40, 50, 50);
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

            // speed input
            Rect speedRect = new Rect(buttonRect.x + position.width / 4, 40, 60, 20);
            GUI.Label(new Rect(speedRect.x - 60, speedRect.y, 50, 20), "Speed");
            string speedText = EditorGUI.TextField(speedRect, frameCon.FramesPerUnityFrame.ToString());
            float trySpeed;
            if (float.TryParse(speedText, out trySpeed))
                frameCon.FramesPerUnityFrame = trySpeed;// Utils.StringToFloat(speedText);
        }

        private void Update()
        {
            Repaint();
        }
    }
}
