using UnityEngine;
using Managers;

public class SkipWaveButton : MonoBehaviour
{
    private GameObject gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        
    }

    public void SkipToNextWave() {
        Debug.Log("click!");
        gameManager.GetComponent<WaveManager>().SkipWave();
    }

}
