using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LoogaSoft.FMOD.Editor
{
    internal static class LoogaFmodUniTaskSupportMenu
    {
        private const string MenuPath = "LoogaSoft/FMOD/Enable UniTask Support";
        private const string GeneratedFolder = "Assets/LoogaSoft/Generated/FMOD UniTask";
        private const string GeneratedAsmdefPath = GeneratedFolder + "/LoogaSoft.FMOD.UniTask.asmdef";
        private const string GeneratedSourcePath = GeneratedFolder + "/LoogaFmodUniTaskExtensions.cs";

        private const string GeneratedAsmdef = @"{
    ""name"": ""LoogaSoft.FMOD.UniTask"",
    ""rootNamespace"": ""LoogaSoft.FMOD.Runtime"",
    ""references"": [
        ""LoogaSoft.FMOD.Runtime"",
        ""UniTask""
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}";

        private const string GeneratedSource = @"using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LoogaSoft.FMOD.Runtime
{
    public static class LoogaFmodUniTaskExtensions
    {
        public static async UniTask WaitUntilStoppedAsync(
            this LoogaFmodHandle handle,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            while (handle.IsPlaying)
            {
                await UniTask.Yield(timing, cancellationToken);
            }
        }

        public static async UniTask StopAndWaitAsync(
            this LoogaFmodHandle handle,
            LoogaFmodStopMode mode = LoogaFmodStopMode.AllowFadeout,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            if (!handle.IsValid)
                return;

            handle.Stop(mode, release: false);
            await handle.WaitUntilStoppedAsync(timing, cancellationToken);
            handle.Release();
        }

        public static async UniTask PlayLoopUntilCanceledAsync(
            this LoogaFmodEmitter emitter,
            CancellationToken cancellationToken,
            LoogaFmodStopMode stopMode = LoogaFmodStopMode.AllowFadeout,
            PlayerLoopTiming timing = PlayerLoopTiming.Update)
        {
            if (emitter == null)
                throw new ArgumentNullException(nameof(emitter));

            emitter.StartLoop();

            try
            {
                await UniTask.WaitUntilCanceled(cancellationToken, timing);
            }
            finally
            {
                emitter.Stop(stopMode);
            }
        }
    }
}";

        [MenuItem(MenuPath, priority = 203)]
        private static void ToggleUniTaskSupport()
        {
            if (IsEnabled())
            {
                Disable();
                return;
            }

            if (!AssemblyIsAvailable("UniTask"))
            {
                EditorUtility.DisplayDialog("UniTask Not Found", "Install UniTask before enabling Looga FMOD UniTask support.", "OK");
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
            return File.Exists(GeneratedAsmdefPath) && File.Exists(GeneratedSourcePath);
        }

        private static bool AssemblyIsAvailable(string assemblyName)
        {
            if (CompilationPipeline.GetAssemblies().Any(assembly => assembly.name == assemblyName))
                return true;

            if (AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetName().Name == assemblyName))
                return true;

            return AssetDatabase.FindAssets($"{assemblyName} t:AssemblyDefinitionAsset").Length > 0;
        }

        private static void Enable()
        {
            Directory.CreateDirectory(GeneratedFolder);
            File.WriteAllText(GeneratedAsmdefPath, GeneratedAsmdef);
            File.WriteAllText(GeneratedSourcePath, GeneratedSource);
            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support enabled.");
        }

        private static void Disable()
        {
            DeleteAssetAndMeta(GeneratedSourcePath);
            DeleteAssetAndMeta(GeneratedAsmdefPath);

            if (Directory.Exists(GeneratedFolder) && Directory.GetFiles(GeneratedFolder).Length == 0)
            {
                Directory.Delete(GeneratedFolder);
                DeleteAssetAndMeta(GeneratedFolder + ".meta");
            }

            AssetDatabase.Refresh();
            Debug.Log("Looga FMOD UniTask support disabled.");
        }

        private static void DeleteAssetAndMeta(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            string metaPath = path.EndsWith(".meta") ? path : path + ".meta";
            if (File.Exists(metaPath))
                File.Delete(metaPath);
        }
    }
}