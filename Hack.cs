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
        public static bool GUIEnabled = true;

        public static OffsetBackup OnGetPopulationTarget;

        public static OffsetBackup OnRefreshMilestones;
        
        private void OnGUI()
        {
            if (GUIEnabled)
                Window.DoGUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
                GUIEnabled = !GUIEnabled;
        }

        public static void AddMoney(long Amount)
        {
            EconomyManager Instance = Singleton<EconomyManager>.instance;

            long RealAmount = Amount * 100;

            FieldInfo m_CashAmount =
                typeof(EconomyManager).GetField("m_cashAmount", BFlags.NonPublicInstance);

            FieldInfo m_CashDelta =
                typeof(EconomyManager).GetField("m_cashDelta", BFlags.NonPublicInstance);
            
            m_CashAmount.SetValue(Instance, RealAmount);
            m_CashDelta.SetValue(Instance, RealAmount);
        }

        //https://gist.github.com/anonymous/c524671571c3879381b2
        public static void EnableAchievements() =>
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;

        //https://github.com/earalov/Skylines-UnlockAllWondersAndLandmarks
        public static void UnlockEverything()
        {
            UnlockManager Manager = Singleton<UnlockManager>.instance;

            IntPtr OriginalPop = typeof(MilestonesWrapper).GetMethod("OnGetPopulationTarget", BFlags.PublicInstance)
                .MethodHandle.GetFunctionPointer();

            IntPtr OriginalRefresh = typeof(MilestonesWrapper).GetMethod("OnRefreshMilestones", BFlags.PublicInstance)
                .MethodHandle.GetFunctionPointer();

            IntPtr NewPop = typeof(Overrides).GetMethod("OnGetPopulationTarget", BFlags.PublicInstance).MethodHandle
                .GetFunctionPointer();

            IntPtr NewRefresh = typeof(Overrides).GetMethod("OnRefreshMilestones", BFlags.PublicInstance).MethodHandle
                .GetFunctionPointer();

            OnGetPopulationTarget = Redirector.DetourFunction(OriginalPop, NewPop);
            OnRefreshMilestones = Redirector.DetourFunction(OriginalRefresh, NewRefresh);

            Manager.m_MilestonesWrapper.OnGetPopulationTarget(0, 0);
            Manager.m_MilestonesWrapper.OnRefreshMilestones();
        }
    }
}