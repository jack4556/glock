using System.Collections;
using System.Collections.Generic;
using GunGame.Guns;
using UnityEngine;

namespace GunGame.Guns
{
    [CreateAssetMenu(menuName = "Guns/Fire Mode/Semi Auto")]
    public class SemiAuto : FireMode
    {

        public override string modeName { get { return "Semi Auto"; } }
      
        // Semi Automatic (1 shot every trigger pull)
        public override void FireType(GunStats gunStats)
        {

            if (Input.GetMouseButtonDown(0) && !gunStats.rechambering && !gunStats.reloading)
            {

                gunStats.StartCoroutine("FireBullet");

            }

            
        }
    }
}

