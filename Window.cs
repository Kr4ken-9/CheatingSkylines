using UnityEngine;

namespace CheatingSkylines
{
    public class Window : MonoBehaviour
    {
        public static Rect windowRect = new Rect(20, 20, 500, 250);

        private static long Money = 1000000;

        public static void DoGUI() =>
            windowRect = GUILayout.Window(0, windowRect, DoWindow, "Cheating Skylines by Kr4ken");

        private static void DoWindow(int WindowID)
        {
            if (GUILayout.Button("Add Money"))
                Hack.AddMoney(Money);
            
            GUILayout.Label($"Money: {Money}");

            Money = (long)GUILayout.HorizontalSlider(Money, 1, 1000000000);
            
            if(GUILayout.Button("Enable Achievements"))
                Hack.EnableAchievements();
            
            GUI.DragWindow();
        }
    }
}