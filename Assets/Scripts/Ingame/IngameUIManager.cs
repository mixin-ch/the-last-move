using Mixin.TheLastMove.Ads;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Ingame;
using Mixin.TheLastMove.Player;
using Mixin.TheLastMove.Save;
using Mixin.Utils;
using System.Collections;
using UnityEngine;

namespace Mixin.TheLastMove.Ingame.UI
{
    public class IngameUIManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerOperator _playerOperator => EnvironmentManager.Instance.PlayerOperator;

        private void OnEnable()
        {
            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
            IngameOverlayUIB.OnPauseButtonClicked += IngameOverlayUIB_OnPauseButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked += IngameDeathScreenUIB_OnRespawnButtonClicked;
            IngameDeathScreenUIB.OnRestartButtonClicked += IngameDeathScreenUIB_OnRestartButtonClicked;
            IngameDeathScreenUIB.OnQuitButtonClicked += GoToMainMenu;
            IngamePauseUIB.OnQuitButtonClicked += GoToMainMenu;
            IngamePauseUIB.OnResumeButtonClicked += IngamePauseUIB_OnResumeButtonClicked;
            _playerOperator.OnPlayerTakeDamageEvent += _playerOperator_OnPlayerTakeDamageEvent;
            _playerOperator.OnPlayerDeathEvent += _playerOperator_OnPlayerDeathEvent;
        }

        private void OnDisable()
        {
            EnvironmentManager.OnGameStarted -= EnvironmentManager_OnGameStarted;
            IngameOverlayUIB.OnPauseButtonClicked -= IngameOverlayUIB_OnPauseButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked -= IngameDeathScreenUIB_OnRespawnButtonClicked;
            IngameDeathScreenUIB.OnRestartButtonClicked -= IngameDeathScreenUIB_OnRestartButtonClicked;
            IngameDeathScreenUIB.OnQuitButtonClicked -= GoToMainMenu;
            IngamePauseUIB.OnQuitButtonClicked -= GoToMainMenu;
            IngamePauseUIB.OnResumeButtonClicked -= IngamePauseUIB_OnResumeButtonClicked;
            _playerOperator.OnPlayerTakeDamageEvent -= _playerOperator_OnPlayerTakeDamageEvent;
            _playerOperator.OnPlayerDeathEvent -= _playerOperator_OnPlayerDeathEvent;
        }

        private void EnvironmentManager_OnGameStarted()
        {
            $"EnvironmentManager_OnGameStarted".Log();
            StartCoroutine(UpdateScore());
            FillHearts();
            IngamePauseUIB.Instance.Show(false);
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private IEnumerator UpdateScore()
        {
            while (EnvironmentManager.Instance.IsGameRunning)
            {
                IngameOverlayUIB.Instance.ScoreText.text =
                    EnvironmentManager.Instance.Distance.RoundToInt().ToString();
                yield return new WaitForEndOfFrame();
            }
        }

        private void GoToMainMenu()
        {
            SceneManager.Instance.LoadScene(SceneName.MainMenu.ToString());
        }

        private void IngamePauseUIB_OnResumeButtonClicked()
        {
            IngamePauseUIB.Instance.Show(false);
        }

        private void IngameDeathScreenUIB_OnRestartButtonClicked()
        {
            EnvironmentManager.Instance.StartGame();
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private void IngameDeathScreenUIB_OnRespawnButtonClicked()
        {
            IngameSceneManager.Instance.ShowRespawnAd();
        }

        private void IngameOverlayUIB_OnPauseButtonClicked()
        {
            IngamePauseUIB.Instance.Show(true);
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            int score = EnvironmentManager.Instance.Distance.RoundToInt();
            int highscore = SaveManager.Instance.IngameData.Data.Highscore;

            // Set score text
            IngameDeathScreenUIB.Instance.ScoreText.text = $"Score: {score}";

            // Set new highscore
            if (score > highscore)
            {
                highscore = score;
                SaveManager.Instance.IngameData.Data.Highscore = score;
            }

            IngameDeathScreenUIB.Instance.HighscoreText.text = $"Your Highscore: {highscore}";
            IngameDeathScreenUIB.Instance.KillText.text = $"Kills: 0";
            IngameDeathScreenUIB.Instance.CurrencyText.text = $"Figures Collected: 0";

            IngameDeathScreenUIB.Instance.Show(true);
        }

        private void _playerOperator_OnPlayerTakeDamageEvent()
        {
            IngameOverlayUIB.Instance.RemoveHeart();
            IngameOverlayUIB.Instance.DamageOverlay.RemoveFromClassList("inactive");
            StartCoroutine(RemoveClassAfterDelay(.1f));
        }

        IEnumerator RemoveClassAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            IngameOverlayUIB.Instance.DamageOverlay.AddToClassList("inactive");
        }

        private void FillHearts()
        {
            IngameOverlayUIB.Instance.HealthContainer.Clear();

            for (int i = 0; i < _playerOperator.Health; i++)
                IngameOverlayUIB.Instance.AddHeart();
        }
    }
}
