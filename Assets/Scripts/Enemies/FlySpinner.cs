using UnityEngine;
using Pathfinding;

public class FlySpinner : MonoBehaviour
{
    AIDestinationSetter destinationSetter;
    AIPath path;

    [SerializeField] float disableTime = 0.5f;

    private void OnEnable()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        destinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnDamage() // When the enemy is damaged
    {
        // Disable AI
        path.canMove = false;
        path.canSearch = false;

        Invoke("ReEnableAI", disableTime);
    }

    void ReEnableAI() // Renable enemy AI after a certain time period
    {
        // Enable AI
        path.canMove = true;
        path.canSearch = true;
    }
}
