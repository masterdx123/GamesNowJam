using UnityEngine;

namespace ScriptableObjects
{
    public abstract class UsableItemData : ItemData
    {
        public abstract void Use(PlayerController playerController);
    }
}
