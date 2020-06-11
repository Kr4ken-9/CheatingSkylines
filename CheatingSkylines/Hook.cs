using System.Threading;
using UnityEngine;

namespace CheatingSkylines
{
    public class Hook : MonoBehaviour
    {
        private static GameObject hackObject;
        public static bool keepAlive = true;
        
        public static void EntryPoint()
        {
            Thread hookThread = new Thread(KeepAliveRoutine);
            hookThread.Start();
        }

        private static void KeepAliveRoutine()
        {
            while (keepAlive)
            {
                Thread.Sleep(1000);

                if (hackObject == null)
                {
                    hackObject = new GameObject();
                    hackObject.AddComponent<Hack>();
                    DontDestroyOnLoad(hackObject);
                }

                Thread.Sleep(5000);
            }
        }
    }
}