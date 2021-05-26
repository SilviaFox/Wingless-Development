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
    [HideInInspector] public int currentChargeLevel = 0; // Current charge level, starts at 0, goes to 2
    float nextChargeTime; // next charge time is set by charge level times
    bool hasBeenDamaged = false;
    bool ableToShoot = true;

    string[] shootSounds = new string[] {"Shoot1", "Shoot2", "Shoot3"}; // Sounds used for shooting

    Vector2 currentBulletSpawnPos;

    [SerializeField] GameObject chargeParticles;
    [SerializeField] Color[] chargeParticleColors; // colors that the charge particles will use

    ParticleSystem.MainModule chargeParticleSystemMain;

    BulletCounter bulletCounter;
    PlayerController playerController; // For checking if the player is dead or alive
    InputManager inputManager;

    private void OnEnable()
    {
        // Get main particle system
        ParticleSystem chargeParticleSystem = chargeParticles.GetComponent<ParticleSystem>();
        chargeParticleSystemMain = chargeParticleSystem.main;
    }

    private void Start()
    {

        // Get components
        playerController = GetComponent<PlayerController>();
        inputManager = InputManager.instance;

        bulletCounter = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletCounter>();

        // Get main particle system
        // ParticleSystem chargeParticleSystem = chargeParticles.GetComponent<ParticleSystem>();
        // chargeParticleSystemMain = chargeParticleSystem.main;

        bulletCounter.IncreaseBulletAmount(); // Add 1 to the current amount of bullets


    }

    public void InitialShot() {

        if (ableToShoot)
        {
            playerController.isShooting = true; // Play Shooting animation

            if (bulletCounter.bulletAmount < maxBullets) // If the maxiumum amount of bullets has not been reached
            {
                Shoot(0);
            }

            if (currentChargeLevel == 0)
                    PlayerController.playerAudioManager.Play("Charge");

            nextChargeTime = Time.time + chargeLevelTimes[0];   
        }
    }

    public void HoldShot() {

        if (ableToShoot)
        {
            playerController.isShooting = true;
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
    }

    public void ReleaseShot() {
        
        // Release a charged shot
        PlayerController.playerAudioManager.Stop("Charge");

        chargeParticles.SetActive(false);

        if (ableToShoot) // When the player presses the shoot button
        {    
            if (bulletCounter.bulletAmount < maxBullets) // If the maxiumum amount of bullets has not been reached
                Shoot(currentChargeLevel);
            
        }
        else if (hasBeenDamaged)
            hasBeenDamaged = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime && !playerController.isDead && !playerController.isHurt && !playerController.isAttacking)
            ableToShoot = true;
        else
            ableToShoot = false;

        if (Time.time >= nextFireTime && !inputManager.shootHeld)
        {
            playerController.isShooting = false;
            
        }
        // Disable everything if the player gets hurt
        if (playerController.isHurt || playerController.isDead)
        {
            chargeParticles.SetActive(false);
            currentChargeLevel = 0;
            hasBeenDamaged = true;
        }
    }

    void Shoot(int chargeLevel)
    {
        

        if (!playerController.isOnWall) // If not on wall
        {
            if (playerController.isFacingLeft) // Facing left
                currentBulletSpawnPos = leftBulletSpawnPosition.position; // Spawn from the left
        
            else
                currentBulletSpawnPos = bulletSpawnPosition.position; // Spawn from the right
        
        }
        else
        {
            if (playerController.isFacingLeft) // Shoot from the oppsosite position if on a wall
                currentBulletSpawnPos = bulletSpawnPosition.position;

            else
                currentBulletSpawnPos = leftBulletSpawnPosition.position;
        }
        
        nextChargeTime = Time.time + chargeLevelTimes[currentChargeLevel];
        
        PlayerController.playerAudioManager.Play(shootSounds[currentChargeLevel]);
        // PlayerController.playerAudioManager.Stop("Charge");
        nextFireTime = Time.time + timeBetweenFire;
        if (!playerController.foundWall)
        {
            bulletCounter.IncreaseBulletAmount(); // Add 1 to the current amount of bullets
            Instantiate(bullets[currentChargeLevel], currentBulletSpawnPos, bulletSpawnPosition.rotation); // Create a bullet
        }
        currentChargeLevel = 0;
    }

    public void OnUnpause()
    {
        // if(!inputManager.shootHeld && currentChargeLevel > 0)
        // {   
        //     if (bulletCounter.bulletAmount < maxBullets) // If the maxiumum amount of bullets has not been reached
        //         Shoot(currentChargeLevel);

        //     chargeParticles.SetActive(false);
        // }
        // else if (!inputManager.shootHeld)
        // {
        //     currentChargeLevel = 0;
        //     if (chargeParticles != null)
        //         chargeParticles.SetActive(false);
        // }
    }




}
