using System.Collections;
using System.Collections.Generic;
using GunGame.Guns;
using UnityEngine;

namespace GunGame.Guns
{
    [CreateAssetMenu(menuName = "Guns/Fire Mode/Full Auto")]
    public class FullAuto : FireMode
    {
        public override string modeName { get { return "Full Auto"; } }
       

        // Fully Automatic (Fires bullets so long as the button is held)
        public override void FireType(GunStats gunStats)
        {
            if (Input.GetMouseButton(0) && !gunStats.rechambering && !gunStats.reloading)
            {
               
                   
                    gunStats.StartCoroutine("FireBullet");
              
                
            }

        }


    }
}

