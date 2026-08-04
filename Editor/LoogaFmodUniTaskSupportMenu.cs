using UnityEditor;
using UnityEngine;

namespace LoogaSoft.FMOD.Editor
{
    internal static class LoogaFmodUniTaskSupportProvider
    {
        private const string DefineSymbol = "LOOGA_FMOD_UNITASK_SUPPORT";

        private static readonly string[] RequiredAssemblies =
        {
            "UniTask"
        };

        public static string ProviderId => "looga-fmod.unitask";
        public static string PackageName => "Looga FMOD";
        public static string IntegrationName => "UniTask";
        public static string Description => "Adds allocation-conscious async helpers for FMOD operations.";

        public static bool IsEnabled()
        {
            return LoogaFmodOptionalSupportUtility.DefineIsEnabled(DefineSymbol);
        }

        public static string GetUnavailableReason()
        {
            return LoogaFmodOptionalSupportUtility.AllAssembliesAreAvailable(RequiredAssemblies, out string missingAssemblies)
                ? string.Empty
                : "Install UniTask. Missing assemblies: " + missingAssemblies;
        }

        public static void SetEnabled(bool enabled)
        {
            if (enabled)
                Enable();
            else
                Disable();
        }

        private static void Enable()
        {
            LoogaFmodOptionalSupportUtility.AddDefineSymbol(DefineSymbol);
            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support enabled.");
        }

        private static void Disable()
        {
            LoogaFmodOptionalSupportUtility.RemoveDefineSymbol(DefineSymbol);
            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support disabled.");
        }
    }
}
