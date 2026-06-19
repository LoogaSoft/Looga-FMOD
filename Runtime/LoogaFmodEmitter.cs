using System.Collections.Generic;
using FMODUnity;
using LoogaSoft.Inspector.Runtime;
using UnityEngine;

namespace LoogaSoft.FMOD.Runtime
{
    public sealed class LoogaFmodEmitter : MonoBehaviour
    {
        [Header("Event")]
        [SerializeField] private LoogaFmodEvent _sound;
        [SerializeField] private EventReference _fallbackEvent;

        [Header("Playback")]
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private bool _loopingEvent;
        [SerializeField] private bool _attachToTransform = true;
        [SerializeField] private bool _stopOnDisable = true;
        [SerializeField] private LoogaFmodStopMode _stopMode = LoogaFmodStopMode.AllowFadeout;

        [Header("Parameters")]
        [SerializeField] private List<LoogaFmodParameter> _parameters = new();

        private LoogaFmodHandle _handle;

        public LoogaFmodHandle Handle => _handle;
        public bool IsPlaying => _handle.IsPlaying;

        private void OnEnable()
        {
            if (_playOnEnable)
                Play();
        }

        private void OnDisable()
        {
            if (_stopOnDisable)
                Stop();
        }

        [Button("Play", mode: LoogaButtonMode.PlayModeOnly)]
        public void Play()
        {
            if (_loopingEvent)
            {
                StartLoop();
                return;
            }

            if (_sound != null)
            {
                if (_attachToTransform)
                    LoogaFmod.PlayOneShot(_sound, transform, _parameters);
                else
                    LoogaFmod.PlayOneShot(_sound, transform.position, _parameters);

                return;
            }

            if (_fallbackEvent.IsNull)
                return;

            if (_attachToTransform)
                LoogaFmod.PlayOneShot(_fallbackEvent, transform, _parameters);
            else
                LoogaFmod.PlayOneShot(_fallbackEvent, transform.position, _parameters);
        }

        public void StartLoop()
        {
            Stop(LoogaFmodStopMode.Immediate);

            if (_sound != null)
            {
                _handle = _attachToTransform
                    ? LoogaFmod.Start(_sound, transform, _parameters)
                    : LoogaFmod.Start(_sound, transform.position, _parameters);
                return;
            }

            if (_fallbackEvent.IsNull)
                return;

            _handle = _attachToTransform
                ? LoogaFmod.Start(_fallbackEvent, transform, _parameters)
                : LoogaFmod.Start(_fallbackEvent, transform.position, _parameters);
        }

        [Button("Stop", mode: LoogaButtonMode.PlayModeOnly)]
        public void Stop()
        {
            Stop(_stopMode);
        }

        public void Stop(LoogaFmodStopMode mode)
        {
            _handle.Stop(mode);
            _handle = default;
        }

        public void SetParameter(string parameterName, float value, bool ignoreSeekSpeed = false)
        {
            _handle.SetParameter(parameterName, value, ignoreSeekSpeed);
        }
    }
}
