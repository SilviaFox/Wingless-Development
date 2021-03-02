using UnityEngine;

[System.Serializable]
public class Attacks
{
    public string animationName; // Name of the animation the attack will use
    public float animationTime; // Time each attack animation takes

    public float endLag; // lag on end of attack, marks the amount of time before the attack will end

    public float attackRange = 1; // Attack range, default to 1
    public float attackDamage = 10; // Damage attack will deal

    public Vector2 attackForce; // The force of the attack
}
