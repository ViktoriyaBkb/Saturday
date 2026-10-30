using UnityEngine;

public class TargetHealth : MonoBehaviour, IDamagable
{
    private TargetMover _mover;

    private void Awake()
    {
        _mover = GetComponent<TargetMover>();
    }

    public void ReceiveDamage(float amount)
    {
        Debug.Log($"Target dying. Old layer: {gameObject.layer}, Old tag: {gameObject.tag}");

        gameObject.tag = "DeadTarget";

        gameObject.layer = 7;

        if (_mover != null) _mover.StopPatrol();

        EnemyVision vision = GetComponentInChildren<EnemyVision>();
        if (vision != null)
        {
            vision.gameObject.SetActive(false);
            Debug.Log($"[TargetHealth] Vision GameObject disabled for {name}");
        }


        transform.rotation = Quaternion.Euler(90, 0, 0);
        GetComponent<Renderer>().material.color = Color.gray;

        Debug.Log($"New layer: {gameObject.layer}, New tag: {gameObject.tag}");
    }
}