using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameFlow Flow;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateGameFlow(GameFlow.Start);
    }

    public void UpdateGameFlow(GameFlow newFlow)
    {
        Flow = newFlow;

        switch (newFlow)
        {
            case GameFlow.Start:
                break;
            case GameFlow.Playing:
                break;
            case GameFlow.Pausing:
                break;
            case GameFlow.Lose:
                break;
        }
    }
}

public enum GameFlow
{
    Start,
    Playing,
    Pausing,
    Lose
}
