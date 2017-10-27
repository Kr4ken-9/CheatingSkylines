using System.Reflection;
using ColossalFramework;
using UnityEngine;

namespace CheatingSkylines
{
    public class Hack : MonoBehaviour
    {
        public static bool GUIEnabled = true;
        
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
            var Instance = Singleton<EconomyManager>.instance;

            long RealAmount = Amount * 100;

            FieldInfo m_CashAmount =
                typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo m_CashDelta =
                typeof(EconomyManager).GetField("m_cashDelta", BindingFlags.NonPublic | BindingFlags.Instance);
            
            m_CashAmount.SetValue(Instance, RealAmount);
            m_CashDelta.SetValue(Instance, RealAmount);
        }

        //https://gist.github.com/anonymous/c524671571c3879381b2

        public static void EnableAchievements() =>
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
    }
}