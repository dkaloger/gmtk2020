using UnityEngine;

public class HealthDecrementer : MonoBehaviour
{
    public int damageAmount;
    public float damageRateInSeconds;
    public bool damagedEnabled = false;

    private float secondsUntilZero = 0;

    private void OnCollisionStay(Collision col)
    {
        health h = col.gameObject.GetComponent<health>();
        if (damagedEnabled && h != null)
        {
            if (secondsUntilZero > 0)
            {
                secondsUntilZero -= Time.deltaTime;
            }
            else
            {
                h.HP_now -= damageAmount;
                secondsUntilZero = damageRateInSeconds;
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        secondsUntilZero = 0;
    }
}
