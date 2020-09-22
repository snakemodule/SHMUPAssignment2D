using UnityEngine;

public class MainManager : MonoBehaviorSingleton<MainManager>
{
    public Camera MainCamera { get; private set; }

    private void Awake()
    {
        RegisterSingleton();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        MainCamera = Camera.main;
    }

}
