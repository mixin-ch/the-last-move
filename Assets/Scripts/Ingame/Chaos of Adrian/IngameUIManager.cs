using GooglePlayGames;
using Mixin.TheLastMove.Ads;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Ingame;
using Mixin.TheLastMove.Player;
using Mixin.TheLastMove.Save;
using Mixin.TheLastMove.Scene;
using Mixin.TheLastMove.Sound;
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

        [SerializeField]
        private RewardedAdsButton _rewardedAdsButton;

        [SerializeField]
        private InterstitialAd _interstitialAd;

        private void Awake()
        {
            _showAdInt = _showAdRange.GetRandomIntBetween();
        }

        private void OnEnable()
        {
            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
            IngameOverlayUIB.OnPauseButtonClicked += IngameOverlayUIB_OnPauseButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked += WatchAdToContinue;
            IngameDeathScreenUIB.OnRestartButtonClicked += IngameDeathScreenUIB_OnRestartButtonClicked;
            IngameDeathScreenUIB.OnQuitButtonClicked += GoToMainMenu;
            IngamePauseUIB.OnQuitButtonClicked += GoToMainMenu;
            IngamePauseUIB.OnResumeButtonClicked += IngamePauseUIB_OnResumeButtonClicked;

            PlayerOperator.OnPlayerTakeDamageEvent += playerOperator_OnPlayerTakeDamageEvent;
            PlayerOperator.OnPlayerDeathEvent += playerOperator_OnPlayerDeathEvent;
            CollectableOperator.OnCollected += SetCollectableText;
            ObstacleOperator.OnKilled += SetKillText;

            //Add Rewarded Video Events
            RewardedAdsButton.AdFinished += RewardedVideoAdRewardedEvent;
            //RewardedAdManager.AdFailed += RewardedVideoAdShowFailedEvent;

            InterstitialAd.AdShowCompleted += RestartGame;
            InterstitialAd.AdFailed += RestartGame;
        }

        private void OnDisable()
        {
            EnvironmentManager.OnGameStarted -= EnvironmentManager_OnGameStarted;
            IngameOverlayUIB.OnPauseButtonClicked -= IngameOverlayUIB_OnPauseButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked -= WatchAdToContinue;
            IngameDeathScreenUIB.OnRestartButtonClicked -= IngameDeathScreenUIB_OnRestartButtonClicked;
            IngameDeathScreenUIB.OnQuitButtonClicked -= GoToMainMenu;
            IngamePauseUIB.OnQuitButtonClicked -= GoToMainMenu;
            IngamePauseUIB.OnResumeButtonClicked -= IngamePauseUIB_OnResumeButtonClicked;

            PlayerOperator.OnPlayerTakeDamageEvent -= playerOperator_OnPlayerTakeDamageEvent;
            PlayerOperator.OnPlayerDeathEvent -= playerOperator_OnPlayerDeathEvent;
            CollectableOperator.OnCollected -= SetCollectableText;
            ObstacleOperator.OnKilled -= SetKillText;

            //Add Rewarded Video Events
            RewardedAdsButton.AdFinished -= RewardedVideoAdRewardedEvent;
            //RewardedAdManager.AdFailed -= RewardedVideoAdShowFailedEvent;

            InterstitialAd.AdShowCompleted -= RestartGame;
            InterstitialAd.AdFailed -= RestartGame;
        }

        private void EnvironmentManager_OnGameStarted()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateScore());

            SetCollectableText(null);
            SetKillText(null);

            FillHearts();
            IngamePauseUIB.Instance.Show(false);
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private IEnumerator UpdateScore()
        {
            while (EnvironmentManager.Instance.IsGameRunning)
            {
                IngameOverlayUIB.Instance.ScoreText.text =
                    EnvironmentManager.Instance.Score.ToString();
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
                _interstitialAd.ShowAd();
                EnvironmentManager.PlayCounter = 0;
                _showAdInt = _showAdRange.GetRandomIntBetween();
            }
            else
                RestartGame();
        }

        private void RestartGame()
        {
            EnvironmentManager.Instance.StartGame();
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private void WatchAdToContinue()
        {
            _rewardedAdsButton.ShowAd();
        }

        private void IngameOverlayUIB_OnPauseButtonClicked()
        {
            GeneralSoundManager.Instance.PlaySound(SoundType.ButtonClick);
            IngamePauseUIB.Instance.Show(true);
        }

        private void RewardedVideoAdShowFailedEvent()
        {
            throw new NotImplementedException();
        }

        private void RewardedVideoAdRewardedEvent()
        {
            EnvironmentManager.Instance.Continue();
            IngameDeathScreenUIB.Instance.Show(false);
        }

        private void playerOperator_OnPlayerDeathEvent(PlayerOperator _)
        {
            int score = EnvironmentManager.Instance.Score;
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

        private void playerOperator_OnPlayerTakeDamageEvent(PlayerOperator _)
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

        private void SetCollectableText(CollectableOperator _)
        {
            IngameOverlayUIB.Instance.CurrencyText.text = EnvironmentManager.Instance.CollectablesCollected.ToString();
        }

        private void SetKillText(ObstacleOperator _)
        {
            IngameOverlayUIB.Instance.KillText.text = EnvironmentManager.Instance.ObstaclesKilled.ToString();
        }
    }
}
