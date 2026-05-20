using LoogaSoft.FMOD.Runtime;
using UnityEditor;
using UnityEngine;

namespace LoogaSoft.FMOD.Editor
{
    [CustomEditor(typeof(LoogaFmodEmitter))]
    public sealed class LoogaFmodEmitterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(6f);

            using (new EditorGUI.DisabledScope(!Application.isPlaying))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Play"))
                    {
                        foreach (Object selectedTarget in targets)
                        {
                            if (selectedTarget is LoogaFmodEmitter emitter)
                                emitter.Play();
                        }
                    }

                    if (GUILayout.Button("Stop"))
                    {
                        foreach (Object selectedTarget in targets)
                        {
                            if (selectedTarget is LoogaFmodEmitter emitter)
                                emitter.Stop();
                        }
                    }
                }
            }
        }
    }
}
