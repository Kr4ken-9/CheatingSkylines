using System;
using System.IO;
using System.Reflection;
using ColossalFramework;
using HarmonyLib;
using Redirection;
using UnityEngine;

namespace CheatingSkylines
{
    public class Hack : MonoBehaviour
    {
        private bool _guiEnabled = true;

        #region Harmony
        
        private Harmony _harmony;
        
        private MethodInfo _originalPopulationTarget;
        private MethodInfo _newPopulationTarget;
        private HarmonyMethod _harmonyPopulationTarget;

        private MethodInfo _originalRefreshMilestones;
        private MethodInfo _newRefreshMilestones;
        private HarmonyMethod _harmonyRefreshMilestones;

        private MethodInfo _originalUpdate;
        private MethodInfo _newUpdate;
        
        #endregion
        
        #region GUI
        
        private Rect _windowRect = new Rect(20, 20, 250, 250);
        private readonly Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        private DeveloperUI _devUi;
        private bool _omg = false;

        private string GetAchievementsStatus()
        {
            SimulationManager simManager = Singleton<SimulationManager>.instance;

            if (simManager.m_metaData == null)
                return "N/A";
            
            switch (Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements)
            {
                case SimulationMetaData.MetaBool.True:
                    return "Disabled";
                case SimulationMetaData.MetaBool.False:
                    return "Enabled";
                default:
                    return "You Fucked Something Up";
            }
        }

        private void DoWindow(int windowID)
        {
            GUILayout.Label($"Achievements Status: {GetAchievementsStatus()}");
            if (GUILayout.Button("Enable Achievements"))
                EnableAchievements();

            if (GUILayout.Button("Unlock Everything"))
                UnlockEverything();

            if (GUILayout.Button("Toggle Dev UI"))
            {
                _omg = !_omg;
                DevToggled(_omg);
            }
            
            GUI.DragWindow();
        }

        private void DevToggled(bool newValue)
        {
            if (newValue) _devUi = gameObject.AddComponent<DeveloperUI>();
            else Destroy(_devUi);
        }
        
        #endregion

        private void Start()
        {
            _harmony = new Harmony("wtf.is.this");
            Type milestonesWrapper = typeof(MilestonesWrapper);
            Type ovMilestonesWrapper = typeof(OV_MilestonesWrapper);

            _originalPopulationTarget =
                milestonesWrapper.GetMethod("OnGetPopulationTarget", BFlags.PublicInstance);
            _newPopulationTarget =
                ovMilestonesWrapper.GetMethod("OV_OnGetPopulationTarget", BFlags.NonPublicStatic);
            _harmonyPopulationTarget = new HarmonyMethod(_newPopulationTarget);

            _originalRefreshMilestones = milestonesWrapper.GetMethod("OnRefreshMilestones", BFlags.PublicInstance);
            _newRefreshMilestones = ovMilestonesWrapper.GetMethod("OV_OnRefreshMilestones", BFlags.NonPublicStatic);
            _harmonyRefreshMilestones = new HarmonyMethod(_newRefreshMilestones);
            
            _originalUpdate = typeof(DeveloperUI).GetMethod("Update", BFlags.NonPublicInstance);
            _newUpdate = typeof(OV_DeveloperUI).GetMethod("OV_Update", BFlags.NonPublicStatic);
            _harmony.Patch(_originalUpdate, transpiler: new HarmonyMethod(_newUpdate));
        }

        private void OnGUI()
        {
            if (_guiEnabled)
                _windowRect = GUILayout.Window(0, _windowRect, DoWindow, $"Cheating Skylines by Kr4ken v{_version}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
                _guiEnabled = !_guiEnabled;
        }

        //https://gist.github.com/anonymous/c524671571c3879381b2
        private void EnableAchievements() =>
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;

        //https://github.com/earalov/Skylines-UnlockAllWondersAndLandmarks
        private void UnlockEverything()
        {
            UnlockManager manager = Singleton<UnlockManager>.instance;

            _harmony.Patch(_originalPopulationTarget, _harmonyPopulationTarget);
            _harmony.Patch(_originalRefreshMilestones, _harmonyRefreshMilestones);

            manager.m_MilestonesWrapper.OnGetPopulationTarget(0, 0);
            manager.m_MilestonesWrapper.OnRefreshMilestones();
            
            _harmony.Unpatch(_originalPopulationTarget, _newPopulationTarget);
            _harmony.Unpatch(_originalRefreshMilestones, _newRefreshMilestones);
        }
    }
}