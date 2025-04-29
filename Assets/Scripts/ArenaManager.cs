using System;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    private bool _hasScored;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ResetArena()
    {
        _hasScored = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasScored)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            
            if (player.player1)
            {
                SumoManager.Instance.Player2WinsRound();
            }

            if (player.player2)
            {
                SumoManager.Instance.Player1WinsRound();
            }

            _hasScored = true;
        }
    }
}
