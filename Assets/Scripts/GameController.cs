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
        yield return new WaitForSeconds(1f);

        CameraController.Instance.ZoomOut(0.1f, 70f);
        AudioController.Instance.MinimizeAmbientVolume();
        AudioController.Instance.MaximizeMusicVolume();
    }
}
