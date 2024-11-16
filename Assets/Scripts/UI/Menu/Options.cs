using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private GameObject _pauseMenu;

    private const string VolumeParameter = "MasterVolume";

    void Start()
    {
        float value;
        if (_audioMixer.GetFloat(VolumeParameter, out value))
        {
            // Convert dB to linear
            _slider.value = Mathf.Pow(10, value / 20); 
        }

        _slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float sliderValue)
    {
        // Convert linear slider value to logarithmic (dB)
        float volume = Mathf.Log10(sliderValue) * 20;
        _audioMixer.SetFloat(VolumeParameter, volume);
    }

    public void CloseOptions() {
        _pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
