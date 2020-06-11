using System;
using ColossalFramework;
using HarmonyLib;

namespace CheatingSkylines
{
    [HarmonyPatch(typeof(MilestonesWrapper))]
    internal static class OV_MilestonesWrapper
    {
        [HarmonyPatch("OnGetPopulationTarget")]
        [HarmonyPrefix]
        internal static bool OV_OnGetPopulationTarget(int originalTarget, int scaledTarget, int __result)
        {
            __result = 0;
            return false;
        }

        [HarmonyPatch("OnRefreshMilestones")]
        [HarmonyPrefix]
        internal static bool OV_OnRefreshMilestones()
        {
            UnlockManager Manager = Singleton<UnlockManager>.instance;
            
            String[] Milestones = Manager.m_MilestonesWrapper.EnumerateMilestones();
            
            for(int i = 0; i < Milestones.Length; i++)
                if (Milestones[i].Contains("Requirements"))
                    Manager.m_MilestonesWrapper.UnlockMilestone(Milestones[i]);

            return false;
        }
    }
}