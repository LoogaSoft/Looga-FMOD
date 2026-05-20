using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace LoogaSoft.FMOD.Runtime
{
    public static class LoogaFmod
    {
        public static void PlayOneShot(LoogaFmodEvent sound, Transform attachTo, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(sound, out EventInstance instance))
                return;

            if (attachTo != null && sound.AttachOneShots)
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo.gameObject, sound.TrackNonRigidbodyVelocity);
            else if (attachTo != null)
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(attachTo.position));

            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume * sound.Volume, pitch * sound.GetPitch());
            instance.start();
            instance.release();
        }

        public static void PlayOneShot(LoogaFmodEvent sound, Vector3 position, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(sound, out EventInstance instance))
                return;

            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume * sound.Volume, pitch * sound.GetPitch());
            instance.start();
            instance.release();
        }

        public static void PlayOneShot(EventReference eventReference, Transform attachTo, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(eventReference, out EventInstance instance))
                return;

            if (attachTo != null)
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo.gameObject);

            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume, pitch);
            instance.start();
            instance.release();
        }

        public static void PlayOneShot(EventReference eventReference, Vector3 position, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(eventReference, out EventInstance instance))
                return;

            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume, pitch);
            instance.start();
            instance.release();
        }

        public static LoogaFmodHandle Start(LoogaFmodEvent sound, Transform attachTo, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(sound, out EventInstance instance))
                return default;

            if (attachTo != null)
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo.gameObject, sound.TrackNonRigidbodyVelocity);

            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume * sound.Volume, pitch * sound.GetPitch());
            instance.start();
            return new LoogaFmodHandle(instance);
        }

        public static LoogaFmodHandle Start(LoogaFmodEvent sound, Vector3 position, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(sound, out EventInstance instance))
                return default;

            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume * sound.Volume, pitch * sound.GetPitch());
            instance.start();
            return new LoogaFmodHandle(instance);
        }

        public static LoogaFmodHandle Start(EventReference eventReference, Transform attachTo, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(eventReference, out EventInstance instance))
                return default;

            if (attachTo != null)
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo.gameObject);

            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume, pitch);
            instance.start();
            return new LoogaFmodHandle(instance);
        }

        public static LoogaFmodHandle Start(EventReference eventReference, Vector3 position, IReadOnlyList<LoogaFmodParameter> parameters = null, float volume = 1f, float pitch = 1f)
        {
            if (!TryCreate(eventReference, out EventInstance instance))
                return default;

            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            ApplyParameters(instance, parameters);
            ApplyPlayback(instance, volume, pitch);
            instance.start();
            return new LoogaFmodHandle(instance);
        }

        internal static global::FMOD.Studio.STOP_MODE ToFmodStopMode(LoogaFmodStopMode mode)
        {
            return mode == LoogaFmodStopMode.Immediate
                ? global::FMOD.Studio.STOP_MODE.IMMEDIATE
                : global::FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
        }

        private static bool TryCreate(LoogaFmodEvent sound, out EventInstance instance)
        {
            if (sound == null)
            {
                instance = default;
                return false;
            }

            return TryCreate(sound.EventReference, out instance);
        }

        private static bool TryCreate(EventReference eventReference, out EventInstance instance)
        {
            if (eventReference.IsNull)
            {
                instance = default;
                return false;
            }

            instance = RuntimeManager.CreateInstance(eventReference);
            return instance.isValid();
        }

        private static void ApplyParameters(EventInstance instance, IReadOnlyList<LoogaFmodParameter> parameters)
        {
            if (!instance.isValid() || parameters == null)
                return;

            for (int i = 0; i < parameters.Count; i++)
            {
                LoogaFmodParameter parameter = parameters[i];
                if (string.IsNullOrWhiteSpace(parameter.name))
                    continue;

                instance.setParameterByName(parameter.name, parameter.value, parameter.ignoreSeekSpeed);
            }
        }

        private static void ApplyPlayback(EventInstance instance, float volume, float pitch)
        {
            if (!instance.isValid())
                return;

            instance.setVolume(Mathf.Max(0f, volume));
            instance.setPitch(Mathf.Max(0.01f, pitch));
        }

    }
}
