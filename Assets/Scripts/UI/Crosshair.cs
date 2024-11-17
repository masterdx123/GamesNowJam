using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Crosshair : MonoBehaviour
{
    private PlayerController _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _player.OnPlayerDeath += () => Cursor.visible = true;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - transform.position);
        this.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
    }
}
