using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace CheatingSkylines
{
    [HarmonyPatch(typeof(DeveloperUI), "Update")]
    internal static class OV_DeveloperUI
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> OV_Update(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> insList = new List<CodeInstruction>(instructions);
            insList.RemoveRange(0, 18);

            return insList.AsEnumerable();
        }
    }
}