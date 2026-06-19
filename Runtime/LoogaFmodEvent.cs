using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoogaSoft.FMOD.Runtime
{
    [CreateAssetMenu(fileName = "New Looga FMOD Event", menuName = "LoogaSoft/Integrations/FMOD/Event")]
    public sealed class LoogaFmodEvent : ScriptableObject
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private bool _attachOneShots = true;
        [SerializeField] private bool _trackNonRigidbodyVelocity;
        [SerializeField, Min(0f)] private float _volume = 1f;
        [FormerlySerializedAs("_pitch"), SerializeField, HideInInspector]
        private float _legacyPitch = 1f;
        [SerializeField, HideInInspector]
        private bool _pitchRangeMigrated;
        [SerializeField, Min(0.01f)] private Vector2 _pitchRange = Vector2.one;

        public EventReference EventReference => _eventReference;
        public bool AttachOneShots => _attachOneShots;
        public bool TrackNonRigidbodyVelocity => _trackNonRigidbodyVelocity;
        public float Volume => _volume;
        public Vector2 PitchRange => _pitchRange;
        public bool IsValid => !_eventReference.IsNull;

        private void OnEnable()
        {
            MigratePitchRange();
            ValidatePitchRange();
        }

        private void OnValidate()
        {
            MigratePitchRange();
            ValidatePitchRange();
        }

        public float GetPitch()
        {
            ValidatePitchRange();
            return Random.Range(_pitchRange.x, _pitchRange.y);
        }

        private void MigratePitchRange()
        {
            if (_pitchRangeMigrated)
                return;

            float pitch = Mathf.Max(0.01f, _legacyPitch);
            _pitchRange = new Vector2(pitch, pitch);
            _pitchRangeMigrated = true;
        }

        private void ValidatePitchRange()
        {
            float min = Mathf.Max(0.01f, Mathf.Min(_pitchRange.x, _pitchRange.y));
            float max = Mathf.Max(0.01f, Mathf.Max(_pitchRange.x, _pitchRange.y));
            _pitchRange = new Vector2(min, max);
        }
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("LoogaSoft/FMOD/Looga FMOD Surface")]
    public sealed class LoogaFmodSurface : MonoBehaviour
    {
        [SerializeField, Tooltip("Numeric material value passed to gameplay audio events. For Kubera footsteps: 0=concrete/stone, 1=dirt, 2=sand, 3=grass, 4=wood, 5=metal.")]
        private float _terrainValue;

        public float TerrainValue => _terrainValue;
    }
}
