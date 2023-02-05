using Mixin.TheLastMove.Ads;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Ingame;
using Mixin.TheLastMove.Player;
using Mixin.TheLastMove.Save;
using Mixin.TheLastMove.Scene;
using Mixin.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Collectable.OnCollected += (collectable) => SetCollectableText();
            ObstacleOperator.OnKilled += (obstacle) => SetKillText();
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
            //_playerOperator.OnPlayerTakeDamageEvent -= _playerOperator_OnPlayerTakeDamageEvent;
            //_playerOperator.OnPlayerDeathEvent -= _playerOperator_OnPlayerDeathEvent;
        }

        private void EnvironmentManager_OnGameStarted()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateScore());

            SetCollectableText();
            SetKillText();

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
            SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.MainMenu, LoadSceneMode.Single);
        }

        private void IngamePauseUIB_OnResumeButtonClicked()
        {
            IngamePauseUIB.Instance.Show(false);
            StopAllCoroutines();
            StartCoroutine(UpdateScore());
        }

        private void IngameDeathScreenUIB_OnRestartButtonClicked()
        {
            EnvironmentManager.Instance.StartGame();
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private void IngameDeathScreenUIB_OnRespawnButtonClicked()
        {
            AdsManager.Instance.ShowRewardedAd();
            //IngameSceneManager.Instance.ShowRespawnAd();
        }

        private void IngameOverlayUIB_OnPauseButtonClicked()
        {
            IngamePauseUIB.Instance.Show(true);
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            int score = EnvironmentManager.Instance.Distance.RoundToInt();
            int highscore = SaveManager.Instance.IngameData.Data.Highscore;
            int kills = ObstacleOperator.Counter;
            int collectable = Collectable.Counter;

            // Set score text
            IngameDeathScreenUIB.Instance.ScoreText.text = $"Score: {score}";

            // Set new highscore
            if (score > highscore)
            {
                highscore = score;
                SaveManager.Instance.IngameData.Data.Highscore = score;
                SaveManager.Instance.IngameData.Save();
            }

            IngameDeathScreenUIB.Instance.HighscoreText.text = $"Your Highscore: {highscore}";
            IngameDeathScreenUIB.Instance.KillText.text = $"Kills: {kills}";
            IngameDeathScreenUIB.Instance.CurrencyText.text = $"Figures Collected: {collectable}";

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

        private void SetCollectableText()
        {
            IngameOverlayUIB.Instance.CurrencyText.text = Collectable.Counter.ToString();
        }

        private void SetKillText()
        {
            IngameOverlayUIB.Instance.KillText.text = ObstacleOperator.Counter.ToString();
        }
    }
}
