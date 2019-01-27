using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    GameOver,
    Credits,
}

public class GameController : Singleton<GameController>
{
    private GameState currentGameState = GameState.Game;

    private readonly int targetFps = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFps;
    }

    public void Update()
    {
        if (currentGameState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void SetGameState(GameState gameState)
    {
        if (currentGameState != gameState)
        {
            currentGameState = gameState;

            if (gameState == GameState.GameOver)
            {
                StartCoroutine(GameOverCoroutine());
            }
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);

        CameraController.Instance.ZoomOut(0.02f, 120f);
        AudioController.Instance.MinimizeAmbientVolume();
        AudioController.Instance.MaximizeMusicVolume();
    }
}
