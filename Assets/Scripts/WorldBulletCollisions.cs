using UnityEngine;

public class WorldBulletCollisions : MonoBehaviour
{
    BulletCounter bulletCounter;

    private void Awake()
    {
        bulletCounter = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletCounter>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet" || other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
            bulletCounter.DecreaseBulletAmount();
        }
    }
}
