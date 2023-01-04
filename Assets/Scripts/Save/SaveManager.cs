using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Save;

namespace Mixin.TheLastMove.Save
{
    public class SaveManager : Singleton<SaveManager>
    {
        private const string _salt = "ja0s9dj0u1938zhasiduhlz989&%751z3iuhi7dtba87s82e7b87asd87btq34";
        private string GameVersion => ApplicationManager.GetGameVersion();
        private bool InTestMode => false;

        private DataFileManager<IngameData> _ingameData;
        private DataFileManager<UserSettingsData> _userSettingsData;

        public DataFileManager<IngameData> IngameData { get => _ingameData; set => _ingameData = value; }
        public DataFileManager<UserSettingsData> UserSettingsData { get => _userSettingsData; set => _userSettingsData = value; }

        protected override void Awake()
        {
            _ingameData = new DataFileManager<IngameData>(
                "data",
                FileType.Binary,
                GameVersion,
                _salt);

            _userSettingsData = new DataFileManager<UserSettingsData>(
                "settings",
                FileType.XML,
                GameVersion);

            _ingameData.Data = new IngameData();
            _userSettingsData.Data = new UserSettingsData();

            _ingameData.Data.SetFileInformation(GameVersion, InTestMode);
            _userSettingsData.Data.SetFileInformation(GameVersion, InTestMode);

            _ingameData.Save();
            _userSettingsData.Save();

            LoadAllData();
        }

        private void LoadAllData()
        {
            _ingameData.Load();
            _userSettingsData.Load();
        }
    }
}
