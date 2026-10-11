using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float InputMouseX;
    public float InputMouseY;
    public bool InputZoom;
    public bool InputShoot;

    private void Update()
    {
        InputMouseX = Input.GetAxis("Mouse X");
        InputMouseY = Input.GetAxis("Mouse Y");

        InputZoom = Input.GetMouseButton(1);
        InputShoot = Input.GetMouseButton(0);
    }
}
