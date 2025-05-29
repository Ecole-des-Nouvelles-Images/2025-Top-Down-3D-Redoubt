using System;
using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Interactable.PowerPlant;
using Hugo.I.Scripts.Interactable.Resources;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Shield;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        // PRIVATE VAR
        private float _currentHealth;
        
        // PUBLIC VAR
        [Header("<size=14><color=#E74C3C>   DATA</color></size>")]
        [Space(3)]
        [Header("<size=13><color=#58D68D>Player Base Stats</color></size>")]
        public int PlayerId;
        public PlayerBaseStats PlayerBaseStats;
        
        [Header("<size=13><color=#58D68D>Properties</color></size>")]
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, PlayerBaseStats.MaxHealth);
                if (Mathf.Approximately(_currentHealth, PlayerBaseStats.MaxHealth))
                {
                    WantToHeal = false;
                }
                // if (_currentHealth == 0)
                // {
                //     Die();
                // }
            }
        }
        
        [Header("<size=13><color=#58D68D>Player States</color></size>")]
        public bool IsShooting;
        public bool IsAiming;
        public bool IsInteracting;
        public bool PressesButtonSouth;
        public bool WantToReload;
        public bool WantToHeal;
        public bool IsCarrying;
        public bool IsDead;
        
        [Header("<size=13><color=#58D68D>Player Inventory</color></size>")]
        public Dictionary<ResourcesEnum, int> Inventory = new Dictionary<ResourcesEnum, int>()
        {
            { ResourcesEnum.Stone, 0 },
            { ResourcesEnum.Metal, 0 },
            { ResourcesEnum.ElectricalCircuit, 0 }
        };
        
        [Header("<size=14><color=#E74C3C>   REFERENCES</color></size>")]
        [Space(3)]
        [Header("<size=13><color=#58D68D>Player References</color></size>")]
        public GameObject UpperBodyAimGameObject;
        public TriggerCollider InteractableTriggerCollider;
        public TriggerCollider RepelTriggerCollider;
        public Transform CarrieShieldTransform;
        
        [Header("<size=13><color=#58D68D>Weapons References</color></size>")]
        public WeaponHandler RevolverWeapon;
        public WeaponHandler RifleWeapon;

        [Header("<size=13><color=#58D68D>Display References</color></size>")]
        public CanvasHandler CanvasHandler;
        public PlayerWorldSpaceDisplay PlayerWorldSpaceDisplay;
        public PlayerWorldSpaceDisplayInteractions PlayerWorldSpaceDisplayInteractions;
        public Image CircleImage;
        
        [Header("<size=13><color=#58D68D>Player Weapon</color></size>")]
        public WeaponHandler EquippedWeapon;
        
        [Header("<size=13><color=#58D68D>Player Interaction</color></size>")]
        public PadQte ActualPadQte;
        public string ActualInteractableName;
        public ResourceHandler LastInteractableResource;
        public TowerHandler LastInteractableTower;
        public PowerPlantHandler LastInteractablePowerPlant;
        public ReloadHealingHandler LastInteractableReloadHealing;
        public ShieldHandler LastInteractableShield;
        
        private IEnumerator TmeBeforeCollecting(string interactableName)
        {
            float time = 0f;
            while (PressesButtonSouth)
            {
                time += 0.01f;
                PlayerWorldSpaceDisplayInteractions.UpdateInteractionButtonFill(time, PlayerBaseStats.TimeBeforeCollecting);

                if (time >= PlayerBaseStats.TimeBeforeCollecting)
                {
                    IsInteracting = true;
                    ActualInteractableName = interactableName;
                    
                    // _playerTwoBonesIkHandler.DisableTwoBonesIk();
                    EquippedWeapon.gameObject.SetActive(false);

                    if (interactableName == "Resource")
                    {
                        (ResourcesEnum, int) resource = LastInteractableResource.ResourcesICanCollect();
                        int size = 0;
                        
                        if (resource.Item1 == ResourcesEnum.Stone)
                        {
                            size = Mathf.Min(PlayerBaseStats.MaxStone - Inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.Metal)
                        {
                            size = Mathf.Min(PlayerBaseStats.MaxMetal - Inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.ElectricalCircuit)
                        {
                            size = Mathf.Min(PlayerBaseStats.MaxCircuit - Inventory[resource.Item1], resource.Item2);
                        }
                        
                        ActualPadQte = new PadQte(size);
                    }
                    if (interactableName == "PowerPlant")
                    {
                        ActualPadQte = new PadQte(LastInteractablePowerPlant.QteSize);
                    }
                    
                    // Display
                    PlayerWorldSpaceDisplayInteractions.HideInteractionsButton();
                    PlayerWorldSpaceDisplayInteractions.DisplayQteButton(ActualPadQte.Qte);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
