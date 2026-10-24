using UnityEngine;

public class TargetHealth : MonoBehaviour, IDamagable
{
    public void ReceiveDamage(float amount)
    {
        Destroy(gameObject);
    }

}
