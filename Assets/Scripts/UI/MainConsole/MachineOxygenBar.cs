using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainConsole {
    public class MachineOxygenBar : MonoBehaviour
    {
        [SerializeField]
        private OxygenSystem _oxygenSystem;
        private Slider _machineOxygenBarSlider;

        void Start() {
            _machineOxygenBarSlider = GetComponent<Slider>();
        }

        void Update() {
            UpdateOxygenBar();
        }

        private void UpdateOxygenBar() {
            _machineOxygenBarSlider.value = _oxygenSystem.GetCurrentEnergy() / _oxygenSystem.GetMaxEnergy();
        }
    }
}