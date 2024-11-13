using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public static float angle { get; private set; }

    void Update()
    {
        WeaponRotate(player);
    }

    void WeaponRotate(PlayerMovement player)
    {
        angle = Mathf.Atan2(player.differenceMouseToPlayerNormalized.y, player.differenceMouseToPlayerNormalized.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = angle > -90 && angle < 90 ? Vector3.one : new Vector3(1, -1 ,1);
    }
}
