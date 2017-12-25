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

    private float health;

    private float fireCooldown = 0.11f;
    private float reloadCooldown = 1.25f;
    private float meleeCooldown = 1.25f;

    private bool _fireCooldown = false;
    private bool _reloadCooldown = false;
    private bool _meleeCooldown = false;

    private PlayerMovement playerMovement;
    private PlayerAnimController playerAnimController;
    private GameController gc;


	// Use this for initialization
	void Awake () {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimController = GetComponent<PlayerAnimController>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        health = maxHealth;
        playerMovement.AllowControl();
	}
	
	// Update is called once per frame
	void Update () {
        MovementControl();
        bool isDoNothing = !_fireCooldown && !_reloadCooldown && !_meleeCooldown;
        if (Input.GetButton("Fire1") && isDoNothing)
        {
            StartCoroutine(FireCooldown());
            GunFire();
            playerAnimController.TriggerShoot();
        }
        if (Input.GetButton("Reload") && isDoNothing)
        {
            StartCoroutine(ReloadCooldown());
            playerAnimController.TriggerReload();
        }
        if (Input.GetButton("Fire2") && isDoNothing)
        {
            StartCoroutine(MeleeCooldown());
            playerAnimController.TriggerMelee();
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
        BulletBehaviour bul = Instantiate(bulletPrefab, pistolEndPoint.transform.position, transform.rotation).GetComponent<BulletBehaviour>();
        bul.setDamage(1);
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
                if (health <= 0)
                {
                    gc.Gameover();
                }
            }
        }
    }
    public float getHealth()
    {
        return health;
    }
}
