using System;
using ColossalFramework;

namespace CheatingSkylines.Overrides
{
    public class Overrides
    {
        public int OnGetPopulationTarget(int originalTarget, int scaledTarget) => 0;

        public void OnRefreshMilestones()
        {
            UnlockManager Manager = Singleton<UnlockManager>.instance;
            
            String[] Milestones = Manager.m_MilestonesWrapper.EnumerateMilestones();
            
            for(int i = 0; i < Milestones.Length; i++)
                if(Milestones[i].Contains("Requirements"))
                    Manager.m_MilestonesWrapper.UnlockMilestone(Milestones[i]);
        }
    }
}