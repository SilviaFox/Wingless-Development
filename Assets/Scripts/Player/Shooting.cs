using UnityEngine;

public class Shooting : MonoBehaviour
{

    [SerializeField] GameObject[] bullets; // The bullet prefab
    [SerializeField] Transform bulletSpawnPosition;
    [SerializeField] Transform leftBulletSpawnPosition;
    [SerializeField] float maxBullets; // Maximum amount of bullets
    [SerializeField] float timeBetweenFire = 0.5f; // Cooldown between each time the player fires
    float nextFireTime = 0;
    [SerializeField] float[] chargeLevelTimes; // Times between each charge
    int currentChargeLevel = 0; // Current charge level, starts at 0, goes to 2
    float nextChargeTime; // next charge time is set by charge level times
    bool hasBeenDamaged = false;
    bool ableToShoot = true;

    string[] shootSounds = new string[] {"Shoot1", "Shoot2", "Shoot3"}; // Sounds used for shooting

    Vector2 currentBulletSpawnPos;

    [SerializeField] GameObject chargeParticles;
    [SerializeField] Color[] chargeParticleColors; // colors that the charge particles will use

    ParticleSystem.MainModule chargeParticleSystemMain;

    BulletCounter bulletCounter;
    ObjectAudioManager playerAudioManager; // Audio Manager for playing local player sounds
    PlayerController playerController; // For checking if the player is dead or alive
    InputManager inputManager;

    private void Start()
    {

        // Get components
        playerController = GetComponent<PlayerController>();

        bulletCounter = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletCounter>();
        playerAudioManager = GameObject.FindGameObjectWithTag("PlayerAudio").GetComponent<ObjectAudioManager>();

        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        // Get main particle system
        ParticleSystem chargeParticleSystem = chargeParticles.GetComponent<ParticleSystem>();
        chargeParticleSystemMain = chargeParticleSystem.main;

    }

    

    // Update is called once per frame
    void Update()
    {
        // Disable everything if the player gets hurt
        if (playerController.isHurt || playerController.isDead)
        {
            chargeParticles.SetActive(false);
            currentChargeLevel = 0;
            hasBeenDamaged = true;
        }

        // Shoot when the button is pressed
        if (ableToShoot && inputManager.shootPressed)
        {
            playerController.isShooting = true; // Play Shooting animation

            if (bulletCounter.bulletAmount < maxBullets) // If the maxiumum amount of bullets has not been reached
                Shoot(0);
            
            nextChargeTime = Time.time + chargeLevelTimes[0];
        }

        if (nextFireTime <= Time.time && !playerController.isDead && !playerController.isHurt && !playerController.isAttacking && !hasBeenDamaged)
            ableToShoot = true;
        else
            ableToShoot = false;
        
        // Charge if button is held

        if (ableToShoot && inputManager.shootHeld)
        {
            chargeParticles.SetActive(true); // Activate the charging particles
            chargeParticleSystemMain.startColor = chargeParticleColors[currentChargeLevel]; // Change color of particles

            if (Time.time >= nextChargeTime && currentChargeLevel != 2)
            {
                playerController.isShooting = true; // Play Shooting animation
                nextChargeTime = Time.time + chargeLevelTimes[currentChargeLevel];
                currentChargeLevel ++;
            }
        }
        else if (hasBeenDamaged)
        {
            playerController.isShooting = false;
        }

        

        // Release a charged shot
        if (ableToShoot && inputManager.shootReleased) // When the player presses the shoot button
        {
            chargeParticles.SetActive(false);
            
            if (bulletCounter.bulletAmount < maxBullets) // If the maxiumum amount of bullets has not been reached
                Shoot(currentChargeLevel);
            
        }
        else if (inputManager.shootReleased && hasBeenDamaged)
            hasBeenDamaged = false;

        if (nextFireTime <= Time.time && !inputManager.shootHeld)
        {
            playerController.isShooting = false;
            
        }

        

        if (inputManager.shootReleased)
            chargeParticles.SetActive(false);

    }

    void Shoot(int chargeLevel)
    {
        if (!playerController.isOnWall)
        {
            if (playerController.isFacingLeft)
            {
                currentBulletSpawnPos = leftBulletSpawnPosition.position;
            }
            else
            {
            currentBulletSpawnPos = bulletSpawnPosition.position;
            }
        }
        else
        {
            if (playerController.isFacingLeft)
            {
                currentBulletSpawnPos = bulletSpawnPosition.position;
                
            }
            else
            {
                currentBulletSpawnPos = leftBulletSpawnPosition.position;
            }   
        }

        bulletCounter.IncreaseBulletAmount(); // Add 1 to the current amount of bullets
        Instantiate(bullets[currentChargeLevel], currentBulletSpawnPos, bulletSpawnPosition.rotation); // Create a bullet

        playerAudioManager.Play(shootSounds[currentChargeLevel]);
        nextFireTime = Time.time + timeBetweenFire;
        currentChargeLevel = 0;
    }


}
