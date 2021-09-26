using System;
using AdaptiveSystemDemo.Character;
using UnityEngine;

namespace _Scripts
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private AudioSource endScreenMusic;
        [SerializeField] private GameObject screen;

        private void Awake()
        {
            screen.SetActive(false);
            endScreenMusic.Stop();
        }

        public void PlayEndScreen()
        {
            screen.SetActive(true);
            Time.timeScale = 0;
            FpsController.instance.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            endScreenMusic.Play();
        }
    }
}