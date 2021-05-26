using UnityEngine;

public class MeleeSystem : MonoBehaviour
{

    PlayerController playerController;
    InputManager inputManager;

    int currentGroundedAttack = 0; // Current attack on ground
    float resetTime;
    [SerializeField] int airAttack = 1; // The integer that represents the air attack, should come after all grounded attacks

    [SerializeField] Attacks[] attacks;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.current;
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (Time.time > resetTime)
        {
            currentGroundedAttack = 0;
        }


        
    }

    public void Attack() {
        if (!playerController.isAttacking && !playerController.isShooting)
        {

            if (playerController.isGrounded && currentGroundedAttack != attacks.Length - 1) // Do Ground Attack
            {
                DoGroundedAttack();
                currentGroundedAttack ++;
            }
            else if(playerController.isGrounded && currentGroundedAttack == attacks.Length - 1)
            {
                currentGroundedAttack = 0;
                DoGroundedAttack();
            }
            else // Do Air Attack
            {
                currentGroundedAttack = 0;
                playerController.Attack(attacks[airAttack].animationName, attacks[airAttack].attackDamage, attacks[airAttack].animationTime, attacks[airAttack].attackForce, attacks[airAttack].groundedAttack);
            }
        }
    }


    void DoGroundedAttack()
    {
        playerController.Attack(attacks[currentGroundedAttack].animationName, attacks[currentGroundedAttack].attackDamage, attacks[currentGroundedAttack].animationTime, attacks[currentGroundedAttack].attackForce, attacks[currentGroundedAttack].groundedAttack);
        resetTime = Time.time + attacks[currentGroundedAttack].endLag;
    }
}
