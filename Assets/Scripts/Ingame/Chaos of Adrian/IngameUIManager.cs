using GooglePlayGames;
using Mixin.TheLastMove.Ads;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Ingame;
using Mixin.TheLastMove.Player;
using Mixin.TheLastMove.Save;
using Mixin.TheLastMove.Scene;
using Mixin.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mixin.TheLastMove.Ingame.UI
{
    public class IngameUIManager : MonoBehaviour
    {
        [SerializeField]
        private MinMaxInt _showAdRange;

        private int? _showAdInt = null;

        private PlayerOperator _playerOperator => EnvironmentManager.Instance.PlayerOperator;

        private void Awake()
        {
            _showAdInt = _showAdRange.GetRandomBetween();
        }

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
            CollectableOperator.OnCollected += (collectable) => SetCollectableText();
            ObstacleOperator.OnKilled += (obstacle) => SetKillText();

            //Add Rewarded Video Events
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

            IronSourceEvents.onInterstitialAdShowSucceededEvent += RestartGame;
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
            // Show ad or restart
            if (EnvironmentManager.PlayCounter > _showAdInt)
            {
                IronSource.Agent.showInterstitial();
                EnvironmentManager.PlayCounter = 0;
                _showAdInt = _showAdRange.GetRandomBetween();
            }
            else
                RestartGame();
        }

        private void RestartGame()
        {
            EnvironmentManager.Instance.StartGame();
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private void IngameDeathScreenUIB_OnRespawnButtonClicked()
        {
            IronSource.Agent.showRewardedVideo();
        }

        private void IngameOverlayUIB_OnPauseButtonClicked()
        {
            IngamePauseUIB.Instance.Show(true);
        }

        private void RewardedVideoAdShowFailedEvent(IronSourceError obj)
        {
            throw new NotImplementedException();
        }

        private void RewardedVideoAdRewardedEvent(IronSourcePlacement obj)
        {
            // TODO: Continue
            EnvironmentManager.Instance.StartGame();
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            int score = EnvironmentManager.Instance.Distance.RoundToInt();
            int highscore = SaveManager.Instance.IngameData.Data.Highscore;
            int kills = EnvironmentManager.Instance.ObstaclesKilled;
            int collectable = EnvironmentManager.Instance.CollectablesCollected;

            // Set score text
            IngameDeathScreenUIB.Instance.ScoreText.text = $"Score: {score}";

            // Set new highscore
            if (score > highscore)
            {
                highscore = score;
                SaveManager.Instance.IngameData.Data.Highscore = score;
                SaveManager.Instance.IngameData.Save();
            }

            // Set leaderboard
            if (UnityEngine.Social.localUser.authenticated)
                UnityEngine.Social.ReportScore(score, GPGSIds.leaderboard_highest_score, null);

            // Set texts
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
            IngameOverlayUIB.Instance.CurrencyText.text = EnvironmentManager.Instance.CollectablesCollected.ToString();
        }

        private void SetKillText()
        {
            IngameOverlayUIB.Instance.KillText.text = EnvironmentManager.Instance.ObstaclesKilled.ToString();
        }
    }
}
