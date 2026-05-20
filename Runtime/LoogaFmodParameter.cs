using System;
using UnityEngine;

namespace LoogaSoft.FMOD.Runtime
{
    [Serializable]
    public struct LoogaFmodParameter
    {
        public string name;
        public float value;
        [Tooltip("When true, FMOD skips parameter seek speed and applies this value immediately.")]
        public bool ignoreSeekSpeed;

        public LoogaFmodParameter(string name, float value, bool ignoreSeekSpeed = false)
        {
            this.name = name;
            this.value = value;
            this.ignoreSeekSpeed = ignoreSeekSpeed;
        }
    }
}
