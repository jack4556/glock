using System.Collections;
using System.Collections.Generic;
using GunGame.Guns;
using UnityEngine;

public class ChargeUp : GunStats
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Has to spin up before firing 
    public void SpinFire(bool active)
    {
        float currentSpinTime = 0;
      
        if (active)
        {
            if (Input.GetMouseButton(0))
            {
                if(currentSpinTime < spinTime)
                {
                    currentSpinTime += Time.deltaTime;
                }
                else
                {
                    FireBullet();
                }
                    
            }
        }
    }
}
