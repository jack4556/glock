using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;
using GunGame.Guns;




/// <summary>
/// Any code pertaining to the inventory
/// </summary>
namespace GunGame.Inventory
{

    /// <summary>
    /// The inventory management for the player
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        #region Variables
        [Header("Inventory")]
        public int maxCapacity;
        int currentCapacity;
        public List<Gun> weapons;
        public Gun selectedGun;
        public Gun currentGun;
        public int gunIndex = 0;
        public Gun[] gunsInArea;
        public int areaIndex;
       

        [Header("Utility")]
        public Camera cam;
        public float pickupDist;
        public bool pickupRay;
        public bool pickupArea;
        public Transform hands;
        [SerializeField]
        public float gunThrowVelocity;

        [Header("UI")]
        public Text capacityText;
        public Text pickupText;
        public Text weaponNameText;
        public Text magAmmoText;
        public Text carryAmmoText;
        public Text fireModeText;

        #endregion
        #region Start
        private void Start()
        {
            foreach (Gun weapon in weapons)
            {
                currentCapacity += weapon.weight;
            }
            UpdateCapacity();
            SetWeaponText();
            ShowPickupText();
        }
        #endregion
        #region Update
        private void Update()
        {
            // If there is a gun in hand, get the current fire mode
            if (currentGun != null)
            {
                currentGun.GunStatsMain();
            }
            // Drop Weapon
            if (Input.GetKeyDown(KeyCode.Backspace) && currentGun != null)
            {
                DropWeapon(currentGun);

            }
            // Switch weapon if the player has a weapon
            if(weapons != null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SwitchWeapon(false);
                }
            }

            if(pickupRay)
                DetectWeaponRaycast();
            
           
            if (Input.GetKeyDown(KeyCode.F) && selectedGun == true && selectedGun.weight + currentCapacity <= maxCapacity)
            {
                Debug.Log("Weapon Grab Attempted");
                //Add the weapon to the inventory
                PickupWeapon(selectedGun);
                selectedGun = null;

            }
            DestroyWeapon();

        }
        #endregion
        
        #region Switch Weapons
        /// <summary>
        /// The switching of the  one weapon to another
        /// </summary>
        /// <param name="removingGun"> Whter the weapon is being switched</param>
        void SwitchWeapon(bool removingGun)
        {
            // If the gun index is about to exceed the count
            if(gunIndex >= weapons.Count -1)
            {
                ReloadCancel();
                if (!removingGun)
                {
                    //Hide gun 1 
                    currentGun.gameObject.SetActive(false);
                }
                DetachCam();

                //Set index to 0
                gunIndex = 0;
                currentGun = weapons[gunIndex];

                //Show gun 2
                currentGun.gameObject.SetActive(true);
                AttachCam();
            }
            // If it isn't
            else
            {
                ReloadCancel();
                if (!removingGun)
                {
                    //Hide gun 1 
                    currentGun.gameObject.SetActive(false);
                }
                //Next gun in index
                DetachCam();

                gunIndex++;
                currentGun = weapons[gunIndex];

                //Show gun 2
                currentGun.gameObject.SetActive(true);
                AttachCam();
            }

        }
        #endregion
        
        #region Weapon Detection and Pickup

        private void DetectWeaponRaycast()
        {
            
            RaycastHit hit;
            
            //Shoot out ray
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, pickupDist))
            {

                // Get gun script of highlighted object
                Gun selectedGun = hit.transform.gameObject.GetComponent<Gun>();
                
                ShowPickupText();

            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (pickupArea)
                if (other.gameObject.GetComponent<Gun>())
                {
                    selectedGun = other.gameObject.GetComponent<Gun>();
                    
                    Debug.Log("Weapon Pickup Detected");
                    ShowPickupText();
                }
               
                    
            
        }
        private void OnTriggerExit(Collider other)
        {
            if (pickupArea)
                if (other.gameObject.GetComponent<Gun>())
                
                    Debug.Log("Weapon Pickup Removed");
            ShowPickupText();
        }

        private void ShowPickupText()
        {
            if(selectedGun != null && pickupText != null)
            {
                pickupText.gameObject.SetActive(true);


                // if the weight of the highlighted gun + your current weight exceeds the capacity, show the player that
                if(selectedGun.weight + currentCapacity > maxCapacity)
                {
                    pickupText.text = selectedGun.gunName + " is too heavy!\nWeight: " + selectedGun.weight;
                }
                else
                {

                    // Show propmt to pick up the weapon and the weight of the highlighted gun
                    pickupText.text = "Press F to pick up " + selectedGun.gunName + "\nWeight: " + selectedGun.weight;

                    Debug.Log("Prompt Added");
                    
                    
                }
            }
            // if there is no gun, no prompt
            else
            {
                pickupText.gameObject.SetActive(false);
            }
        }

        #region Pickup Weapon
        /// <summary>
        /// Picking up the selected weapon
        /// </summary>
        /// <param name="weapon"> The weapon being picked up</param>
        void PickupWeapon(Gun weapon)
        {
            
            Debug.Log("Picked Up "+ selectedGun);
            ReloadCancel();
            // If the player has a gun in hand, put it away
            if (currentGun)
            {
                DetachCam();
                currentGun.gameObject.SetActive(false);
            }
            // Add picked up weapon to inventory
            weapons.Add(weapon);
            

            // Add weight from new weapon
            currentCapacity += weapon.weight;
            UpdateCapacity();

            // Set the current weapon to the new weapon
            currentGun = weapon;
            AttachCam();
            gunIndex = weapons.Count -1;
            currentGun.inv = this;
            SetWeaponText();
            
            // Attach the weapon model to the player's hands
            weapon.transform.position = hands.position;
            weapon.transform.rotation = hands.rotation;
            weapon.transform.SetParent(hands);
            Rigidbody rigi = weapon.GetComponent<Rigidbody>();
            rigi.constraints = RigidbodyConstraints.FreezeAll;
            
            
        }
        #endregion
        #endregion
        
        #region Drop Weapon

        /// <summary>
        /// The dropping of the held weapon
        /// </summary>
        /// <param name="weapon"> The weapon being dropped</param>
        void DropWeapon(Gun weapon)
        {

            // Reduce players current weight
            currentCapacity -= currentGun.weight;
            UpdateCapacity();

            // Physically drop the weapon
            weapon.transform.SetParent(null);
            Rigidbody rigi = weapon.GetComponent<Rigidbody>();
            rigi.constraints = RigidbodyConstraints.None;

            //Chuck gun forward
            rigi.AddRelativeForce(0,0,currentGun.weight / gunThrowVelocity);
            DetachCam();

            //Change Weapons
            weapons.Remove(currentGun);
            
            if (currentCapacity == 0)
            {
                currentGun = null;
            }
            else
            {
                SwitchWeapon(true);
                currentGun = weapons[gunIndex];
                AttachCam();
            }
            SetWeaponText();
        }
        #endregion
        
        #region Destroy Weapon

        public void DestroyWeapon()
        {
            if (currentGun.destroyOnEmpty && currentGun.currentMag <= 0)
            {
                Debug.Log("Current Gun weight = " + currentGun.weight );
                currentCapacity -= currentGun.weight;
                Destroy(currentGun.gameObject);
                weapons.Remove(currentGun);
                UpdateCapacity();
                SwitchWeapon(true);
                SetWeaponText();
                
                Debug.Log(currentGun.gunName);
            }
            
        }

        #endregion
        
        #region UI
        
        #region Set Weapon Text
        /// <summary>
        /// Set the stat texts for the weapons
        /// </summary>
        void SetWeaponText()
        {
            int statFieldIndex = 0;
            Text currentField;
            // If there is a weapon in hand, show it's stats
            if (currentGun != null)
            {
                
                if(weaponNameText != null)
                weaponNameText.text = currentGun.gunName;
                else NullWarning(weaponNameText);
                
                if(magAmmoText != null)
                magAmmoText.text = currentGun.currentMag.ToString();
                else NullWarning(magAmmoText);
                
                if(carryAmmoText != null)
                carryAmmoText.text = currentGun.carryAmmo.ToString();
                else NullWarning(carryAmmoText);
                
                if(fireModeText.text != null)
                fireModeText.text = currentGun.fireModes[currentGun.currentFireMode].modeName;
                else NullWarning(fireModeText);
                
                
            }
            // If not, hide the appropriate hud elements
            else
            {
                if(weaponNameText != null)
                weaponNameText.text = "";
                else NullWarning(weaponNameText);
                
                if(magAmmoText != null)
                magAmmoText.text = "";
                else NullWarning(magAmmoText);
                
                if(carryAmmoText != null)
                carryAmmoText.text = "";
                else NullWarning(carryAmmoText);
                
                if(fireModeText != null)
                fireModeText.text = "";
                else NullWarning(fireModeText);
            }
        }

       
        
        #endregion
        #region Update Capacity
        /// <summary>
        /// Update the weight capacity ui
        /// </summary>
        void UpdateCapacity()
        {
            if (capacityText != null)
            {
                // Refresh the Weight on the UI
                capacityText.text = "Capacity: " + currentCapacity + "/" + maxCapacity;
            }
            else
            {
                NullWarning(capacityText);
            }
        }
        #endregion
        #region Update Clip
        /// <summary>
        /// Update the clip capacity ui
        /// </summary>
        public void UpdateClip()
        {
            // Refresh the Mag Capacity on the UI
            magAmmoText.text = currentGun.currentMag.ToString();
        }
        #endregion
        #region Update Stock Ammo
        /// <summary>
        /// Update the reserve ammo capacity ui
        /// </summary>
        public void UpdateAmmo()
        {
            if (carryAmmoText != null)
            {
                // Refresh the Reserve Ammo Capacity on the UI
                carryAmmoText.text = currentGun.carryAmmo.ToString();
            }
           
        }
        #endregion
        #region Update Fire Mode
        /// <summary>
        /// Update the fire mode ui
        /// </summary>
        public void UpdateFireMode()
        {
            // Refresh the Fire Mode on the UI
            fireModeText.text = currentGun.fireModes[currentGun.currentFireMode].modeName;

        }
        #endregion
        #region Null Warnings
        public void NullWarning(Text statText)
        {
            Debug.LogWarning(statText + "is not set");
        }
        
        #endregion
        #endregion

        #region Reload Cancel

        void ReloadCancel()
        {
            if (currentGun.reloading == true)
            {
                currentGun.reloading = false;
            }
        }

        #endregion
        
        #region Cam
        /// <summary>
        /// Attach the players camera
        /// </summary>
        public void AttachCam()
        {
            currentGun.cam = cam;
        }

        /// <summary>
        /// Detach the players camera
        /// </summary>
        public void DetachCam()
        {
            currentGun.cam = null;
        }
        #endregion
    }

}

