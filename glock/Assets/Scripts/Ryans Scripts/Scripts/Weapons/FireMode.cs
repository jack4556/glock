using System.Collections;
using System.Collections.Generic;
using GunGame.Guns;
using UnityEngine;

namespace GunGame.Guns
{
    /// <summary>
    /// The fire mode used for the gun
    /// </summary>
    public abstract class FireMode : ScriptableObject
    {
        public abstract string modeName { get; }
        public abstract void FireType(GunStats gunStats);

    }
}

