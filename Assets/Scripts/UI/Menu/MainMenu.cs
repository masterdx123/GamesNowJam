using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _optionsMenu;

        void Start(){}

        void Update() {}

        public void StartGame() 
        {
            SceneManager.LoadScene("Frazao_TestScene");
        }

        public void OpenOptions() {
            gameObject.SetActive(false);
            _optionsMenu.SetActive(true);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}