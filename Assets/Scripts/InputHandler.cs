using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Player.Instance.rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
    }

    public void OnCameraMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Player.Instance.rawCameraInput = new Vector2(inputMovement.x, inputMovement.y);
    }

    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            //Testing
            Player.Instance.StartRun();
        }

        if (value.canceled)
        {
            Player.Instance.StopRun();
        }
    }

    public void OnEscape(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            SceneManager.LoadScene(0);
        }
    }
}
