using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Crosshair : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - transform.position);
        this.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
    }
}
