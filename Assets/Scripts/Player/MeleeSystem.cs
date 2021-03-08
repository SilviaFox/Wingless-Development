using UnityEngine;

public class MeleeSystem : MonoBehaviour
{

    PlayerController playerController;
    InputManager inputManager;

    int currentGroundedAttack = 0; // Current attack on ground
    [SerializeField] int airAttack = 1; // The integer that represents the air attack, should come after all grounded attacks

    [SerializeField] Attacks[] attacks;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.attackPressed && !playerController.isAttacking && !playerController.isShooting)
        {

            if (playerController.isGrounded) // Do Ground Attack
                playerController.Attack(attacks[currentGroundedAttack].animationName, attacks[currentGroundedAttack].attackDamage, attacks[currentGroundedAttack].animationTime, attacks[currentGroundedAttack].attackForce, attacks[currentGroundedAttack].groundedAttack);
            else // Do Air Attack
                playerController.Attack(attacks[airAttack].animationName, attacks[airAttack].attackDamage, attacks[airAttack].animationTime, attacks[airAttack].attackForce, attacks[airAttack].groundedAttack);
        }
    }
}
