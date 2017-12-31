using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    private GameObject uiGameoverPanel;
    [SerializeField]
    private GameObject uiGameScreenPanel;
    [SerializeField]
    private GameObject uiStartScreenPanel;
    [SerializeField]
    private RectTransform uiHealthbar;
    [SerializeField]
    private Text uiAmmoText;
    [SerializeField]
    private Text uiTimer;
    [SerializeField]
    private CameraBehaviour camBehaviour;
    [SerializeField]
    private bool useMobileInput;
    [SerializeField]
    private VirtualJoystickHandler mobileLeftJoystick;
    [SerializeField]
    private VirtualJoystickHandler mobileRightJoystick;
    [SerializeField]
    private GameObject mobileUI;

    [SerializeField]
    private GameObject playerPrefab;

    private GameObject player;
    private PlayerController playerController;
    private ZombieSpawner zombieSpawner;
    private GameSound gameSound;


    private bool gameover = false;
    private bool gameStart = false;

    private float uiHealthbarRectMax;
    private float playerHealthMax = 0;

    private float timer = 0;

    // Use this for initialization
    void Start () {
        gameSound = GetComponent<GameSound>();
        zombieSpawner = GetComponent<ZombieSpawner>();
        if (!camBehaviour)
        {
            camBehaviour = Camera.main.GetComponent<CameraBehaviour>();
        }
        uiGameScreenPanel.SetActive(false);
        uiGameoverPanel.SetActive(false);
        uiStartScreenPanel.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (gameStart && !gameover)
        {
            timer += Time.deltaTime;
            UpdateTimer();
        }
	}

    public void GameStart()
    {
        gameStart = true;
        uiStartScreenPanel.SetActive(false);
        uiGameScreenPanel.SetActive(true);

        uiHealthbarRectMax = uiHealthbar.rect.width;

        player = Instantiate(playerPrefab);
        playerController = player.GetComponent<PlayerController>();

        zombieSpawner.StartSpawning();
        camBehaviour.SetTarget(player.transform);

        if (useMobileInput)
        {
            if (!mobileUI.activeSelf)
            {
                mobileUI.SetActive(true);
            }
            playerController.InitMobileInput(mobileLeftJoystick,mobileRightJoystick);
        }
        else
        {
            if (mobileUI.activeSelf)
            {
                mobileUI.SetActive(false);
            }
        }
    }

    public void Gameover()
    {
        gameover = true;
        uiGameScreenPanel.SetActive(false);
        uiGameoverPanel.SetActive(true);
        zombieSpawner.StopSpawning();
        gameSound.PlayAudioGameover();
    }
    public bool IsGameover()
    {
        return gameover;
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateUI(float health,int loadedAmmo,int ammo)
    {
        uiHealthbar.sizeDelta = new Vector2(CalculateHealthVisual(health), uiHealthbar.rect.height);
        //uiHealthbar.rect.Set(uiHealthbar.rect.x, uiHealthbar.rect.y, );
        uiAmmoText.text = loadedAmmo + " | " + ammo;

    }
    private float CalculateHealthVisual(float health)
    {
        return health / playerHealthMax * uiHealthbarRectMax;
    }
    public void SetPlayerHealthMax(float health)
    {
        playerHealthMax = health;

    }
    private void UpdateTimer()
    {
        int h, m, s;
        if (timer > 3600)
        {

            h = (int)timer / 3600;
            m = ((int)timer % 3600) / 60;
            s = ((int)timer % 3600) % 60;
            uiTimer.text = h + ":" + m + ":" + s;
        }
        else if(timer > 60){
            m = (int)timer / 60;
            s = (int)timer % 60;
            uiTimer.text = m + ":" + s;
        }
        else
        {
            uiTimer.text = (int)timer+"" ;
        }
    }

    public void MobileInputHandler(int n)
    {
        if (playerController)
        {
            playerController.InputHandler(n);
        }
        else
        {
            Debug.Log("Null refference to playerController for Mobile Input");
        }
    }
}
