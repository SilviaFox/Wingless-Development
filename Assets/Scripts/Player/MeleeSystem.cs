using UnityEngine;

public class MeleeSystem : MonoBehaviour
{

    PlayerController playerController;

    int currentGroundedAttack = 0; // Current attack on ground

    [SerializeField] Attacks[] attacks;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && !playerController.isAttacking && !playerController.isShooting)
        {

            if (playerController.isGrounded)
                playerController.Attack(attacks[currentGroundedAttack].animationName, attacks[currentGroundedAttack].attackDamage, attacks[currentGroundedAttack].animationTime, attacks[currentGroundedAttack].attackForce);
        }
    }
}
