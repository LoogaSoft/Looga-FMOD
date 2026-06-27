using UnityEditor;
using UnityEngine;

namespace LoogaSoft.FMOD.Editor
{
    internal static class LoogaFmodUniTaskSupportMenu
    {
        private const string MenuPath = "LoogaSoft/FMOD/Enable UniTask Support";
        private const string DefineSymbol = "LOOGA_FMOD_UNITASK_SUPPORT";
        private const string RuntimeAsmdef = "LoogaSoft.FMOD.Runtime";

        private static readonly string[] RequiredAssemblies =
        {
            "UniTask"
        };

        [MenuItem(MenuPath, priority = 203)]
        private static void ToggleUniTaskSupport()
        {
            if (IsEnabled())
            {
                Disable();
                return;
            }

            if (!LoogaFmodOptionalSupportUtility.AllAssembliesAreAvailable(RequiredAssemblies, out string missingAssemblies))
            {
                EditorUtility.DisplayDialog("UniTask Not Found", "Install UniTask before enabling Looga FMOD UniTask support.\n\nMissing: " + missingAssemblies, "OK");
                return;
            }

            Enable();
        }

        [MenuItem(MenuPath, true)]
        private static bool ValidateToggle()
        {
            UnityEditor.Menu.SetChecked(MenuPath, IsEnabled());
            return true;
        }

        private static bool IsEnabled()
        {
            return LoogaFmodOptionalSupportUtility.DefineIsEnabled(DefineSymbol)
                && LoogaFmodOptionalSupportUtility.AsmdefReferences(RuntimeAsmdef, "UniTask");
        }

        private static void Enable()
        {
            LoogaFmodOptionalSupportUtility.AddDefineSymbol(DefineSymbol);
            if (!LoogaFmodOptionalSupportUtility.SetAsmdefReferences(RuntimeAsmdef, RequiredAssemblies, include: true, out string error))
                EditorUtility.DisplayDialog("Unable To Update FMOD", error, "OK");

            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support enabled.");
        }

        private static void Disable()
        {
            LoogaFmodOptionalSupportUtility.RemoveDefineSymbol(DefineSymbol);
            if (!LoogaFmodOptionalSupportUtility.SetAsmdefReferences(RuntimeAsmdef, RequiredAssemblies, include: false, out string error))
                EditorUtility.DisplayDialog("Unable To Update FMOD", error, "OK");

            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support disabled.");
        }
    }
}