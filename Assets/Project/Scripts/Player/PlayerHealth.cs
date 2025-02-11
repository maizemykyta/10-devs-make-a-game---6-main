using Bonjoura.Managers;
using Bonjoura.Services;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerHealth : Health
    {
        public void PlayerDeth(GameObject LosePanle)
        {
            LosePanle.SetActive(true);
            InputManager.Instance.ChangeCursorState(true);
            PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
            Time.timeScale = 0;
        }
    }
}