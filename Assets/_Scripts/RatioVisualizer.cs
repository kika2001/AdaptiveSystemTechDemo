using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveS.System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AdaptiveSystemDemo.SystemVisualizer
{
    public class RatioVisualizer : MonoBehaviour
    {
        public Slider gameSlider, playerSlider;

        private AdaptiveSystem adaptiveSystem;

        private InputMaster controls;

        private void Awake()
        {
            controls = new InputMaster();
            controls.Player.DifficultUp.Enable();
            controls.Player.DifficultDown.Enable();
            controls.Player.DifficultUp.performed += Increase;
            controls.Player.DifficultDown.performed += Decrease;
        }

        private void Decrease(InputAction.CallbackContext obj)
        {
            var adaptive = AdaptiveSystemManager.NewAdaptiveSystem("player");
            AdaptiveSystemManager.ChangeGameDifficulty(adaptive,AdaptiveSystemManager.GetCurrentGameRatio(adaptive)-0.1f);
        }

        private void Increase(InputAction.CallbackContext obj)
        {
            var adaptive = AdaptiveSystemManager.NewAdaptiveSystem("player");
            AdaptiveSystemManager.ChangeGameDifficulty(adaptive,AdaptiveSystemManager.GetCurrentGameRatio(adaptive)+0.1f);
        }
        void Update()
        {
            gameSlider.value = AdaptiveSystemManager.GetCurrentGameRatio(AdaptiveSystemManager.NewAdaptiveSystem("player"));
            playerSlider.value = AdaptiveSystemManager.GetCurrentPlayerRatio(AdaptiveSystemManager.NewAdaptiveSystem("player"));
        }
    }

}
