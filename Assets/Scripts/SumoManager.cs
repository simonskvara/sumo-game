using System;
using System.Collections;
using skv_toolkit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Sorry about all those texts, had only one day to do this
/// </summary>
public class SumoManager : MonoBehaviour
{
    public static SumoManager Instance;

    public GameObject player1;
    public GameObject player2;

    private Vector3 _player1StartPosition;
    private Vector3 _player2StartPosition;
    
    private Quaternion _player1StartRotation;
    private Quaternion _player2StartRotation;

    public int player1RoundsWon;
    public int player2RoundsWon;
    private int _currentRound;
    
    [Header("Texts")] 
    public TextMeshProUGUI player1RoundsWonText;
    public TextMeshProUGUI player2RoundsWonText;
    public GameObject player1WonTheRound;
    public GameObject player2WonTheRound;
    public GameObject player1WonTheGame;
    public GameObject player2WonTheGame;
    public GameObject endGamePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateText();
        _player1StartPosition = player1.transform.position;
        _player2StartPosition = player2.transform.position;

        _player1StartRotation = player1.transform.rotation;
        _player2StartRotation = player2.transform.rotation;
    }

    public void ResetPlayers()
    {
        /*player1RoundsWon = 0;
        player2RoundsWon = 0;*/
        
        player1.transform.position = _player1StartPosition;
        player1.transform.rotation = _player1StartRotation;
        
        player2.transform.position = _player2StartPosition;
        player2.transform.rotation = _player2StartRotation;
    }

    public void Player1WinsRound()
    {
        player1RoundsWon++;
        UpdateText();
        
        if (player1RoundsWon >= 3)
        {
            GameWin(true, false);
            return;
        }
        
        StartCoroutine(RoundWon());
        player1WonTheRound.SetActive(true);
        
        Debug.Log("Player 1 wins the round");
    }

    public void Player2WinsRound()
    {
        player2RoundsWon++;
        UpdateText();
        
        if (player2RoundsWon >= 3)
        {
            GameWin(false, true);
            return;
        }

        StartCoroutine(RoundWon());
        player2WonTheRound.SetActive(true);
        
        Debug.Log("Player 2 wins the round");
    }

    IEnumerator RoundWon()
    {
        yield return new WaitForSeconds(2f);
        ArenaManager.Instance.ResetArena();
        ResetPlayers();
        
        player1WonTheRound.SetActive(false);
        player2WonTheRound.SetActive(false);
    }

    void UpdateText()
    {
        player1RoundsWonText.text = $"{player1RoundsWon}/3";
        player2RoundsWonText.text = $"{player2RoundsWon}/3";
    }

    void GameWin(bool isPlayer1, bool isPlayer2)
    {
        UpdateText();

        if (isPlayer1)
        {
            player1WonTheGame.SetActive(true);
        }

        if (isPlayer2)
        {
            player2WonTheGame.SetActive(true);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
}
