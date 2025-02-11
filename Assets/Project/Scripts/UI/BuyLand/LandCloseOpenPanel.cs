using Bonjoura.Managers;
using Bonjoura.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LandCloseOpenPanel : MonoBehaviour
{
    [SerializeField] private GameObject _menuToOpen;
    [SerializeField] private InputAction _openCloseAction;

    public bool IsOpened { get; private set; }

    private void Awake()
    {
        IsOpened = _menuToOpen.activeSelf;
    }

    private void OnEnable()
    {
        _openCloseAction.Enable();
        _openCloseAction.performed += Toggle;
    }

    private void OnDisable()
    {
        _openCloseAction.Disable();
        _openCloseAction.performed -= Toggle;
    }

    private void Toggle(InputAction.CallbackContext callbackContext)
    {
        // Debug.Log($"MenuOpenClose toggle {InputManager.Instance.CursorShowed}");
        if (InputManager.Instance.CursorShowed && IsOpened == false)
            return;

        IsOpened = !IsOpened;
        _menuToOpen.SetActive(IsOpened);
        PlayerController.Instance.FPSCamera.enabled = !IsOpened;
        InputManager.Instance.ChangeCursorState(IsOpened);
    }
}
