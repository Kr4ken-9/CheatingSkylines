using System;
using UnityEngine;

namespace CheatingSkylines
{
    public class Window : MonoBehaviour
    {
        public static Rect windowRect = new Rect(20, 20, 500, 250);

        private static string Money = "10000";

        public static void DoGUI() =>
            windowRect = GUILayout.Window(0, windowRect, DoWindow, "Cheating Skylines by Kr4ken");

        private static void DoWindow(int WindowID)
        {
            if (GUILayout.Button("Add Money"))
                Hack.AddMoney(long.Parse(Money));
            
            GUILayout.Label("Money:");

            Money = GUILayout.TextField(Money);
            
            if(GUILayout.Button("Enable Achievements"))
                Hack.EnableAchievements();
            
            if(GUILayout.Button("Unlock Everything"))
                Hack.UnlockEverything();
            
            GUI.DragWindow();
        }
    }
}