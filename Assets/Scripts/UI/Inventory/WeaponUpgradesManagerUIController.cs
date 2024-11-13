using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class WeaponUpgradesUIController : MonoBehaviour
    {
        [SerializeField]
        private Sprite emptySprite;
        [SerializeField]
        private Image weaponIcon;
        [SerializeField]
        private List<Image> upgradeIcons;
        
        private WeaponData _weaponData;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (!weaponIcon.sprite)
            {
                weaponIcon.sprite = emptySprite;
            }

            foreach (Image icon in upgradeIcons)
            {
                if (!icon.sprite)
                {
                    icon.sprite = emptySprite;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateWeapon(Weapon weapon)
        {
            if (!weapon || !weapon.weaponData)
            {
                _weaponData = null;
                weaponIcon.sprite = emptySprite;
                return;
            }
            Debug.Log(weapon.weaponData.sprite.ToString());

            _weaponData = weapon.weaponData;
            weaponIcon.sprite = _weaponData.sprite;
        }
    }
}
