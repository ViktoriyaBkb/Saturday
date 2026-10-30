using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotationSpeed = 5f;

    private int _currentWaypointIndex = 0;
    private bool _isActive = true;

    private void Start()
    {
        if (_waypoints == null || _waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to " + name);
            _isActive = false;
        }
    }

    private void Update()
    {
        if (!_isActive || GameManager.Instance.IsGameOver) return;

        Patrol();
    }

    private void Patrol()
    {
        Transform targetWaypoint = _waypoints[_currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position,
            targetWaypoint.position, _speed * Time.deltaTime);

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                Time.deltaTime * _rotationSpeed);
        }
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
    }

    public void StopPatrol()
    {
        _isActive = false;
    }
}