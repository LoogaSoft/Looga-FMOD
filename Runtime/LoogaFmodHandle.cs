using FMOD.Studio;

namespace LoogaSoft.FMOD.Runtime
{
    public readonly struct LoogaFmodHandle
    {
        private readonly EventInstance _instance;

        internal LoogaFmodHandle(EventInstance instance)
        {
            _instance = instance;
        }

        public bool IsValid => _instance.isValid();

        public PLAYBACK_STATE PlaybackState
        {
            get
            {
                if (!IsValid)
                    return PLAYBACK_STATE.STOPPED;

                _instance.getPlaybackState(out PLAYBACK_STATE state);
                return state;
            }
        }

        public bool IsPlaying
        {
            get
            {
                PLAYBACK_STATE state = PlaybackState;
                return state is PLAYBACK_STATE.PLAYING or PLAYBACK_STATE.STARTING or PLAYBACK_STATE.SUSTAINING;
            }
        }

        public void SetParameter(string name, float value, bool ignoreSeekSpeed = false)
        {
            if (!IsValid || string.IsNullOrWhiteSpace(name))
                return;

            _instance.setParameterByName(name, value, ignoreSeekSpeed);
        }

        public void SetParameter(LoogaFmodParameter parameter)
        {
            SetParameter(parameter.name, parameter.value, parameter.ignoreSeekSpeed);
        }

        public void SetVolume(float volume)
        {
            if (!IsValid)
                return;

            _instance.setVolume(volume);
        }

        public void SetPitch(float pitch)
        {
            if (!IsValid)
                return;

            _instance.setPitch(pitch);
        }

        public void Stop(LoogaFmodStopMode mode = LoogaFmodStopMode.AllowFadeout, bool release = true)
        {
            if (!IsValid)
                return;

            _instance.stop(LoogaFmod.ToFmodStopMode(mode));

            if (release)
                _instance.release();
        }

        public void Release()
        {
            if (IsValid)
                _instance.release();
        }
    }
}
