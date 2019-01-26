using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    Credits
}

public class GameController : Singleton<GameController>
{
    private readonly int targetFps = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFps;
    }
}
