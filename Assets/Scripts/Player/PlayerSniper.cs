using System;
using UnityEngine;
using TMPro;

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
    [SerializeField] private float _range = 200f;

    [Header("UI")]
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _sniperScore;

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
        if (GameManager.Instance != null && (GameManager.Instance.IsGameOver || Time.timeScale == 0f))
            return;

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

        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            if (hit.transform.CompareTag("Target"))
            {
                if (hit.transform.TryGetComponent(out IDamagable damagableTarget))
                {
                    damagableTarget.ReceiveDamage(100f);
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnTargetKilled(true);
                }
            }
        }
    }


    private void HandleZoom()
    {
        bool zoomed = _zoomHeld;
        float targetFOV;
        if (_zoomHeld)
            targetFOV = _zoomFOV; 
        else
            targetFOV = _normalFOV;

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV,
            Time.deltaTime * _zoomSpeed);

        if (_crosshair != null) _crosshair.SetActive(!zoomed);
        if (_sniperScore != null) _sniperScore.SetActive(zoomed);

    }
}
