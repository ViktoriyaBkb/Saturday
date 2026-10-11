using System;
using UnityEngine;

public class PlayerSniper : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;

    [Header("Camera settings")]
    [SerializeField] private float _normalFOV = 60f;
    [SerializeField] private float _zoomFOV = 20f;
    [SerializeField] private float _zoomSpeed = 10f;

    [Header("Firing settings")]
    [SerializeField] private float _range = 100f;  

    private float _mouseX => _playerInput.InputMouseX;
    private float _mouseY => _playerInput.InputMouseY;
    private bool _zoomHeld => _playerInput.InputZoom;
    private bool _shootingPressed => _playerInput.InputShoot;
    void Start()
    {
        if (_camera == null) _camera = GetComponentInChildren<Camera>();
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        HadleLook();
        HandleZoom();
        HadleShooting();
    }

    private void HadleLook()
    {
        //body rotation 
        transform.Rotate(Vector3.up * _mouseX);

        //camera rotation (up-down)
        _camera.transform.Rotate(Vector3.left * _mouseY);
    }

    private void HadleShooting()
    {
        if (_shootingPressed) Shoot();
    }

    private void Shoot()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycasthit;

        if (Physics.Raycast(ray, out raycasthit, _range))
        {
            Debug.Log(raycasthit.transform.name);

            if (raycasthit.transform.name == "Target")
            {
                Destroy(raycasthit.transform.gameObject);
                Debug.Log("Target destroyed.");
            }
        }
    }

    private void HandleZoom()
    {
        float targetFOV;
        if (_zoomHeld)
            targetFOV = _zoomFOV; 
        else
            targetFOV = _normalFOV;

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, Time.deltaTime * _zoomSpeed);
    }
}
