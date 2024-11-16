using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemHealthPack", menuName = "Scriptable Objects/Item/ItemHealthPack")]
    public class ItemHealthPack : UsableItemData
    {
        [SerializeField]private float healthAmount = 10.0f;


        public override void Use(PlayerController playerController)
        {
            playerController.TakeDamage(-healthAmount);
        }
    }
}
