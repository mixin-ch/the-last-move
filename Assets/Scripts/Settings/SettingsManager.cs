using System;
using UnityEngine;
using UnityEngine.UIElements;
using Mixin.TheLastMove.Save;
using Mixin.Utils;
using Mixin.MultiLanguage;
using Mixin.TheLastMove.Sound;
using System.Collections;

namespace Mixin.TheLastMove.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private UserSettingsData _data => SaveManager.Instance.UserSettingsData.Data;
        private SettingsUIB _uib => SettingsUIB.Instance;

        public static event Action OnLanguageChange;

        private void Start()
        {
            _uib.MusicVolumeSlider.value = _data.MusicVolume;
            _uib.SoundVolumeSlider.value = _data.SoundVolume;

            _uib.EnglishButton.clicked += () => UpdateLanguage(Language.English);
            _uib.GermanButton.clicked += () => UpdateLanguage(Language.German);
            _uib.SwissGermanButton.clicked += () => UpdateLanguage(Language.SwissGerman);
            _uib.FrenchButton.clicked += () => UpdateLanguage(Language.French);

            _uib.SaveButton.clicked += OnSaveButtonClicked;
            _uib.MusicVolumeSlider.RegisterValueChangedCallback(UpdateMusicVolume);
            _uib.SoundVolumeSlider.RegisterValueChangedCallback(UpdateSoundVolume);

            SetLanguageButtonActive();
        }

        private void OnDisable()
        {
            _uib.EnglishButton.clicked -= () => UpdateLanguage(Language.English);
            _uib.GermanButton.clicked -= () => UpdateLanguage(Language.German);
            _uib.SwissGermanButton.clicked -= () => UpdateLanguage(Language.SwissGerman);
            _uib.FrenchButton.clicked -= () => UpdateLanguage(Language.French);

            _uib.SaveButton.clicked -= OnSaveButtonClicked;
            _uib.MusicVolumeSlider.UnregisterValueChangedCallback(UpdateMusicVolume);
            _uib.SoundVolumeSlider.UnregisterValueChangedCallback(UpdateSoundVolume);
        }

        private void OnSaveButtonClicked()
        {
            QualitySettings.SetQualityLevel(_data.Quality);
            SaveManager.Instance.UserSettingsData.Save();
        }

        private void UpdateMusicVolume(ChangeEvent<float> evt)
        {
            _data.MusicVolume = evt.newValue.RoundToInt();
            SetVolume("MusicVolume", evt.newValue);
        }

        private void UpdateSoundVolume(ChangeEvent<float> evt)
        {
            _data.SoundVolume = evt.newValue.RoundToInt();
            SetVolume("SoundVolume", evt.newValue);
            if (_canPlaySound)
            {
                StartCoroutine(PlaySoundWithCooldown());
            }
        }

        private bool _canPlaySound = true;
        private IEnumerator PlaySoundWithCooldown()
        {
            _canPlaySound = false;
            GeneralSoundManager.Instance.PlaySound(SoundType.SliderDrag);
            yield return new WaitForSeconds(0.12f); // Adjust the cooldown time as needed
            _canPlaySound = true;
        }

        /*private void UpdateQuality(ChangeEvent<string> evt)
        {
            _data.Quality = Array.IndexOf(QualitySettings.names, evt.newValue);
        }*/

        private void UpdateLanguage(Language language)
        {
            _data.Language = language;
            LanguageManager.Instance.SelectedLanguage = language;

            GeneralSoundManager.Instance.PlaySound(SoundType.LanguageSelect);

            SetLanguageButtonActive();

            _uib.Init();

            OnLanguageChange?.Invoke();
        }

        private void SetLanguageButtonActive()
        {
            ResetAllLanguageButtonClasses();
            AddActiveClassToLanguageButton();
        }

        private void AddActiveClassToLanguageButton()
        {
            switch (_data.Language)
            {
                case Language.English:
                    _uib.EnglishButton.AddToClassList("active");
                    break;
                case Language.German:
                    _uib.GermanButton.AddToClassList("active");
                    break;
                case Language.SwissGerman:
                    _uib.SwissGermanButton.AddToClassList("active");
                    break;
                case Language.French:
                    _uib.FrenchButton.AddToClassList("active");
                    break;
                default:
                    $"Language {_data.Language} is not defined.".LogWarning();
                    break;
            }
        }

        private void ResetAllLanguageButtonClasses()
        {
            _uib.EnglishButton.RemoveFromClassList("active");
            _uib.GermanButton.RemoveFromClassList("active");
            _uib.SwissGermanButton.RemoveFromClassList("active");
            _uib.FrenchButton.RemoveFromClassList("active");
        }

        public static void SetVolume(string mixerGroup, float value)
        {
            ApplicationManager.Instance.AudioMixer.SetFloat(mixerGroup, CalculateVolume(value));
        }

        public static float CalculateVolume(float value)
        {
            float volume = Mathf.Log10(value / 100f) * 20f;
            if (value == 0) volume = -80f;
            return volume;
        }
    }
}
