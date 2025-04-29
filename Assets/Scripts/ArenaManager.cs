using System;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    
    
}
