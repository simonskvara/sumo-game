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
            Player player = other.GetComponent<Player>();

            if (SumoManager.Instance != null)
            {
                SumoManager.Instance.RoundEnd(player);
            }
            
            _hasScored = true;
        }
    }
}
