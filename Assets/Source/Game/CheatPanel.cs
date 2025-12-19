using System;
using Source.Event;
using Source.Game.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Game
{
    public class CheatPanel : MonoBehaviour
    {
        private GameController _gc;
        
        private void Awake()
        {
            _gc = GetComponent<GameController>();
        }

        private void Update()
        {
            if (Keyboard.current.numpad1Key.wasPressedThisFrame)
                _gc.CResource.ChangeCoin(1000);
            if (Keyboard.current.numpad2Key.wasPressedThisFrame)
                _gc.CResource.ChangePower(1000);
            if (Keyboard.current.numpad7Key.wasPressedThisFrame)
                Time.timeScale = .1f;
            if (Keyboard.current.numpad8Key.wasPressedThisFrame)
                Time.timeScale = 1f;
            if (Keyboard.current.numpad9Key.wasPressedThisFrame)
                Time.timeScale = 2f;
        }
    }
}