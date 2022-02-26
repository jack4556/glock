using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using GunGame.Guns;
using GunGame.Inventory;

/// <summary>
/// Any code pertaining to gun weapons systems
/// </summary>
namespace GunGame.Guns
{
    /// <summary>
    /// The stats and functionality of the gun
    /// </summary>
    public class GunStats : MonoBehaviour
    {
        #region Variables

        [Header("Gun")] 
        [SerializeField]
        private GameObject gunModel;
        public int weight;
        public string gunName;
        public PlayerInventory inv;
        
        
        [Header("Damage")]
        public int headshotMultiplier;
        public int burstSize;
        public int burstDelay;
        [SerializeField]
        public Vector3 recoil;
        public Vector3 startingRot;
        public float recoilCompMulti;
        public float bulletVelocity;
        public float fireRate;
        public float spinTime;
        float currentSpinTime = 0;
        public bool rechambering = false;
        public float burstTime = 0;
        int burstLeft;

        public List<FireMode> fireModes = new List<FireMode>();


        [Header("Ammo")]
        public GameObject gunBarrel;
        [SerializeField]
        private GameObject projectile;
        public float reloadTime;
        public int fullLoadSize;
        public int shotAmount;
        public int magSize;
        public int currentMag;
        public int carryAmmoMax;
        public int carryAmmo;
        public bool unlimitedMag;
        public bool unlimitedStock;
        public bool destroyOnEmpty;
        public GameObject magazineObj;
        public Transform magazineExit;

        [Header("Mechanism")]
        public int currentFireMode;
        
        public Camera cam;
        public bool reloading = false;
        #endregion
      
        #region Update
        private void Update()
        {
            // Change Weapon Mode
            if (Input.GetKeyDown(KeyCode.M))
            {
                UpdateMode();
            }
            // Reload Weapon
            if (Input.GetKeyDown(KeyCode.R) && !reloading && currentMag < fullLoadSize)
            {
                
                StartCoroutine(Reloading());
            }
            

        }
        #endregion
        #region Reloading

        /// <summary>
        /// The removing of ammo from the reserve ammo and adding of ammo to the clip
        /// </summary>
        public void Reload()
        {
            // if the weapon has unlimited stock ammo
            if (unlimitedStock)
            {
                currentMag = magSize;
            }
            // if there is enough ammo to fill the magazine
            else if(carryAmmo >= magSize)
            {
                // Add bullets and count remainder
                currentMag += magSize;
                carryAmmo -= magSize;
                int remainder =  currentMag % fullLoadSize;
                Debug.Log(remainder);

                // If the remainder is the same as the mag size, simply add the mag size
                if(remainder == magSize)
                {
                    currentMag += magSize;
                    carryAmmo -= magSize;
                }

                // Take away the extra remainder of bullets from the mag and shift it to the reserve ammo
                currentMag -= remainder;
                carryAmmo += remainder;
            }
            //if there isn't enough ammo to fill the magazine
            else
            {
                // Add the remaining ammo
                magSize += carryAmmo;
                carryAmmo = 0;
            }
           

            // Update the Clip and Ammo to match the capacity
            if(inv != null)
            {
                inv.UpdateClip();
                inv.UpdateAmmo();
            }
           
        }
        #endregion
        #region Fire Mode
        /// <summary>
        /// Get the current fire mode
        /// </summary>
        public void GunStatsMain()
        {
            fireModes[currentFireMode].FireType(this);
        }
        /// <summary>
        /// Change fire mode
        /// </summary>
        public void UpdateMode()
        {
            if (currentFireMode < fireModes.Count - 1)
            {
                currentFireMode++;

               
                if (inv != null)
                {
                    inv.UpdateFireMode();
                }
            }
            else
            {
                currentFireMode = 0;
                if (inv != null)
                {
                    inv.UpdateFireMode();
                }
            }
        }
        #endregion
        #region Fire Bullet
        
        /// <summary>
        /// Shoot bullet
        /// </summary>
        /// <returns></returns>
        public IEnumerator FireBullet()
        {
            

            if (currentMag > 0)
            {
                Debug.Log("bulletShot");
                rechambering = true;
                Instantiate(projectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
                currentMag -= 1;
                inv.UpdateClip();

                //Recoil
                cam.transform.localEulerAngles += recoil;

                //Wait Rate of Fire Delay
                yield return new WaitForSeconds(fireRate);
                //Ready to fire again
                rechambering = false;
                yield return null;
            }

        }
        #endregion
        #region ReloadTimer
        /// <summary>
        /// The timer for the reload
        /// </summary>
        /// <returns></returns>
        IEnumerator Reloading()
        {
            if (magazineObj != null || magazineExit != null)
            {
                Instantiate(magazineObj, magazineExit);
            }
            reloading = true;
            //Wait reload time
            yield return new WaitForSeconds(reloadTime);
            reloading = false;
            //Add bullets
            Reload();
            yield return null;
        }
        #endregion

       
    }


}

