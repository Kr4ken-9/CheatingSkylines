using System.Threading;
using UnityEngine;

namespace CheatingSkylines
{
    public class Hook : MonoBehaviour
    {
        public static GameObject HackObject;
        public static Hack HackInstance;
        public static Thread hookThread;
        public static bool KeepAlive = true;
        
        public static void ThreadCreater()
        {
            hookThread = new Thread(HookThread);
            hookThread.Start();
        }

        private static void HookThread()
        {
            while (KeepAlive)
            {
                Thread.Sleep(1000);

                if (HackObject == null || HackInstance == null)
                {
                    HackObject = new GameObject();
                    HackInstance = HackObject.AddComponent<Hack>();
                    DontDestroyOnLoad(HackObject);
                }

                Thread.Sleep(5000);
            }
        }
    }
}