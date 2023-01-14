using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove.Sound
{
    public class GeneralSoundManager : Singleton<GeneralSoundManager>
    {
        [SerializeField]
        private List<UIDocument> _allUIDocuments = new List<UIDocument>();

        [SerializeField]
        private MixinDictionary<SoundType, AudioTrackSetupSOList> _soundList;

        private List<Button> _buttonList = new List<Button>();

        public MixinDictionary<SoundType, AudioTrackSetupSOList> SoundList { get => _soundList; }

        protected override void Awake()
        {
            base.Awake();

            QueryAllUIDocuments();
        }

        private void Start()
        {
            QueryAllButtons();

            foreach (Button button in _buttonList)
                button.clicked += () => PlaySound(SoundType.ButtonClick);
        }

        private void OnDisable()
        {
            foreach (Button button in _buttonList)
                button.clicked -= () => PlaySound(SoundType.ButtonClick);
        }

        private void QueryAllUIDocuments()
        {
            _allUIDocuments.Add(FindObjectOfType<UIDocument>());
        }

        private void QueryAllButtons()
        {
            foreach (UIDocument uiDocument in _allUIDocuments)
                _buttonList.Add(uiDocument.rootVisualElement.Q<Button>());
        }

        private void PlaySound(SoundType soundType)
        {
            _soundList[soundType].PlaySound();
        }
    }
}