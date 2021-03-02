using UnityEngine;

public class BulletCounter : MonoBehaviour
{

    [HideInInspector] public float bulletAmount = 0; // Amount of bullets active


    public void IncreaseBulletAmount()
    {
        bulletAmount ++; // Increment by 1
    }

    public void DecreaseBulletAmount()
    {
        bulletAmount --; // Decrement by 1
    }
}
