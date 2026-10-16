using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float distance = 5f;

    private Vector3 startPos;
    void Start()
    {
        //start pos
        startPos = transform.position; 
    }
    void Update()
    {
        float offset = Mathf.Cos(Time.time * speed) * distance;

        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
