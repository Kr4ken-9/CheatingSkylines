using System;
using System.IO;
using System.Reflection;
using ColossalFramework;
using Redirection;
using UnityEngine;

namespace CheatingSkylines
{
    public class Hack : MonoBehaviour
    {
        private bool GUIEnabled = true;
        private OffsetBackup OnGetPopulationTarget;
        private OffsetBackup OnRefreshMilestones;
        
        #region GUI
        
        private Rect _windowRect = new Rect(20, 20, 250, 250);
        private readonly Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        private DeveloperUI _devUI;
        private bool omg = false;

        private void DoWindow(int WindowID)
        {
            if(GUILayout.Button("Enable Achievements"))
                EnableAchievements();
            
            if(GUILayout.Button("Unlock Everything"))
                UnlockEverything();

            if (GUILayout.Button("Toggle Dev UI"))
            {
                omg = !omg;
                DevToggled(omg);
            }
            
            GUI.DragWindow();
        }

        private void DevToggled(bool newValue)
        {
            if (newValue) _devUI = gameObject.AddComponent<DeveloperUI>();
            else Destroy(_devUI);
        }
        
        #endregion

        private void OnGUI()
        {
            if (GUIEnabled)
                _windowRect = GUILayout.Window(0, _windowRect, DoWindow, $"Cheating Skylines by Kr4ken v{_version}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
                GUIEnabled = !GUIEnabled;
        }

        //https://gist.github.com/anonymous/c524671571c3879381b2
        private void EnableAchievements() =>
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;

        //https://github.com/earalov/Skylines-UnlockAllWondersAndLandmarks
        private void UnlockEverything()
        {
            UnlockManager Manager = Singleton<UnlockManager>.instance;

            IntPtr OriginalPop = typeof(MilestonesWrapper).GetMethod("OnGetPopulationTarget", BFlags.PublicInstance)
                .MethodHandle.GetFunctionPointer();

            IntPtr OriginalRefresh = typeof(MilestonesWrapper).GetMethod("OnRefreshMilestones", BFlags.PublicInstance)
                .MethodHandle.GetFunctionPointer();

            IntPtr NewPop = typeof(Overrides.Overrides).GetMethod("OnGetPopulationTarget", BFlags.PublicInstance).MethodHandle
                .GetFunctionPointer();

            IntPtr NewRefresh = typeof(Overrides.Overrides).GetMethod("OnRefreshMilestones", BFlags.PublicInstance).MethodHandle
                .GetFunctionPointer();

            OnGetPopulationTarget = Redirector.DetourFunction(OriginalPop, NewPop);
            OnRefreshMilestones = Redirector.DetourFunction(OriginalRefresh, NewRefresh);

            Manager.m_MilestonesWrapper.OnGetPopulationTarget(0, 0);
            Manager.m_MilestonesWrapper.OnRefreshMilestones();
        }
    }
}