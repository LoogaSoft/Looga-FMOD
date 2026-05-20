using LoogaSoft.FMOD.Runtime;
using UnityEditor;
using UnityEngine;

namespace LoogaSoft.FMOD.Editor
{
    [CustomEditor(typeof(LoogaFmodEvent))]
    public sealed class LoogaFmodEventEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(6f);

            using (new EditorGUI.DisabledScope(!Application.isPlaying))
            {
                if (GUILayout.Button(new GUIContent("Play At Origin", "Preview this event while the editor is in play mode.")))
                {
                    foreach (Object selectedTarget in targets)
                    {
                        if (selectedTarget is LoogaFmodEvent sound)
                            LoogaFmod.PlayOneShot(sound, Vector3.zero);
                    }
                }
            }

            if (!Application.isPlaying)
                EditorGUILayout.HelpBox("Enter Play Mode to preview FMOD events from this inspector.", MessageType.Info);
        }
    }
}
