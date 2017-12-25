using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    private GameObject uiGameover;

    [SerializeField]
    private GameObject playerPrefab;

    private GameObject player;

    private bool gameover = false;
    private bool gameStart = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Gameover()
    {
        gameover = true;
        uiGameover.SetActive(true);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
