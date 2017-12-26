using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Transform pistolEndPoint;
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private int startingAmmo;

    private float health;
    private int ammo = 0;
    private int loadedAmmo = 0;

    private int magazineCapacity = 7;

    private float fireCooldown = 0.11f;
    private float reloadCooldown = 1.25f;
    private float meleeCooldown = 1.25f;

    private bool _fireCooldown = false;
    private bool _reloadCooldown = false;
    private bool _meleeCooldown = false;

    private PlayerMovement playerMovement;
    private PlayerAnimController playerAnimController;
    private PlayerSound playerSound;
    private GameController gc;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HandgunAmmo"))
        {
            ammo += 30;
            Destroy(collision.gameObject);
            UpdateUI();
        }
    }

    // Use this for initialization
    void Awake () {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimController = GetComponent<PlayerAnimController>();
        playerSound = GetComponent<PlayerSound>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        health = maxHealth;
        ammo = startingAmmo;
        playerMovement.AllowControl(true);
        gc.SetPlayerHealthMax(maxHealth);
        UpdateUI();
        GunReload();
	}
	
	// Update is called once per frame
	void Update () {
        MovementControl();
        bool isDoNothing = !_fireCooldown && !_reloadCooldown && !_meleeCooldown;
        if (!gc.IsGameover())
        {

        }
        if (Input.GetButton("Fire1") && isDoNothing)
        {
            GunFire();
        }
        if (Input.GetButton("Reload") && isDoNothing)
        {
            GunReload();
        }
        if (Input.GetButton("Fire2") && isDoNothing)
        {
            Melee();
        }
    }

    void MovementControl()
    {
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (movementInput.x != 0 || movementInput.y != 0)
        {
            if (movementInput.x != 0 && movementInput.y != 0)
            {
                movementInput /= 1.45f;
            }
            //Debug.Log(movementInput.magnitude);
            if (Input.GetButton("Run"))
            {
                playerMovement.Move(movementInput, true);

                if (!playerAnimController.isFeetRun())
                {
                    playerAnimController.setFeetRun(true);
                }
            }
            else
            {
                playerMovement.Move(movementInput);
                if (playerAnimController.isFeetRun())
                {
                    playerAnimController.setFeetRun(false);
                }
            }
            if (!playerAnimController.isFeetWalk())
            {
                playerAnimController.setFeetWalk(true);
            }
            if (!playerAnimController.isBodyMove())
            {
                playerAnimController.setBodyMove(true);
            }

        }
        else
        {
            playerMovement.Move(Vector3.zero);
            if (playerAnimController.isFeetWalk())
            {
                playerAnimController.setFeetWalk(false);
            }
            if (playerAnimController.isFeetRun())
            {
                playerAnimController.setFeetRun(false);
            }
            if (playerAnimController.isBodyMove())
            {
                playerAnimController.setBodyMove(false);
            }
        }
    }

    void GunFire()
    {
        StartCoroutine(FireCooldown());

        if (loadedAmmo > 0)
        {
            playerSound.PlayAudioShoot();
            BulletBehaviour bul = Instantiate(bulletPrefab, pistolEndPoint.transform.position, transform.rotation).GetComponent<BulletBehaviour>();
            bul.setDamage(1);
            playerAnimController.TriggerShoot();
            loadedAmmo--;
            UpdateUI();
        }
        else
        {
            playerSound.PlayAudioEmpty();
        }
    }
    void GunReload()
    {
        if (ammo > 0)
        {
            StartCoroutine(ReloadCooldown());
            playerSound.PlayAudioReload();
            playerAnimController.TriggerReload();
        }
        
    }
    void Melee()
    {
        StartCoroutine(MeleeCooldown());
        playerAnimController.TriggerMelee();
    }
    IEnumerator FireCooldown()
    {
        _fireCooldown = true;
        yield return new WaitForSeconds(fireCooldown);
        _fireCooldown = false;
    }
    IEnumerator ReloadCooldown()
    {
        _reloadCooldown = true;
        yield return new WaitForSeconds(reloadCooldown);
        if (ammo >= 7)
        {
            ammo -= magazineCapacity - loadedAmmo;
            loadedAmmo = 7;
        }
        else
        {
            loadedAmmo = ammo;
            ammo = 0;
        }
        UpdateUI();
        _reloadCooldown = false;
    }
    IEnumerator MeleeCooldown()
    {
        _meleeCooldown = true;
        yield return new WaitForSeconds(meleeCooldown);
        _meleeCooldown = false;
    }

    public void AttackPlayer(float dmg)
    {
        if (!_meleeCooldown)
        {
            if (health > 0)
            {
                health -= dmg;
                UpdateUI();
                if (health <= 0)
                {
                    playerSound.PlayAudioDie();
                    playerMovement.AllowControl(false);
                    gc.Gameover();
                }
                else
                {
                    playerSound.PlayAudioHurt();
                }
            }
        }
    }
    public float getHealth()
    {
        return health;
    }
    
    void UpdateUI()
    {
        gc.UpdateUI(health, loadedAmmo, ammo);
    }
}
