using UnityEngine;
public class Crosshair : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private GameObject GameManagerCanvas;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _player.OnPlayerDeath += () => Cursor.visible = true;
        Cursor.visible = false;
        this.transform.parent = GameManagerCanvas.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var mouseWorldPos = Input.mousePosition;
        this.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
    }
}
