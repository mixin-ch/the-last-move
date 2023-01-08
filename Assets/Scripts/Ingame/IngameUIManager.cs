using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class IngameUIManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerOperator _playerOperator;

        private void EnvironmentManager_OnGameStarted()
        {
            FillHearts();
        }

        private void Update()
        {
            IngameOverlayUIB.Instance.ScoreText.text =
                ((int)EnvironmentManager.Instance.Distance).ToString();
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
            throw new System.NotImplementedException();
        }

        private void IngameOverlayUIB_OnPauseButtonClicked()
        {
            IngamePauseUIB.Instance.Show(true);
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            IngameDeathScreenUIB.Instance.Show(true);
        }

        private void _playerOperator_OnPlayerTakeDamageEvent()
        {
            IngameOverlayUIB.Instance.RemoveHeart();
            IngameOverlayUIB.Instance.DamageOverlay.AddToClassList("active");
            StartCoroutine(RemoveClassAfterDelay(0.1f));
        }

        IEnumerator RemoveClassAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            IngameOverlayUIB.Instance.DamageOverlay.RemoveFromClassList("active");
        }

        private void FillHearts()
        {
            IngameOverlayUIB.Instance.HealthContainer.Clear();

            for (int i = 0; i < _playerOperator.Health; i++)
                IngameOverlayUIB.Instance.AddHeart();
        }
    }
}
