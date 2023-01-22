using Mixin.Utils;
using Mixin.Save;
using UnityEngine;

namespace Mixin.TheLastMove.Save
{
    public class SaveManager : Singleton<SaveManager>
    {
        private const string _salt = "ja0s9dj0u1938zhasiduhlz989&%751z3iuhi7dtba87s82e7b87asd87btq34";
        private string GameVersion => Application.version;
        private bool InTestMode => false;

        private DataFileManager<IngameData> _ingameData;
        private DataFileManager<UserSettingsData> _userSettingsData;

        public DataFileManager<IngameData> IngameData { get => _ingameData; set => _ingameData = value; }
        public DataFileManager<UserSettingsData> UserSettingsData { get => _userSettingsData; set => _userSettingsData = value; }

        protected override void Awake()
        {
            //_ingameData = new DataFileManager<IngameData>(
            //    "data.mxn",
            //    FileType.JSON,
            //    _salt);

            //_userSettingsData = new DataFileManager<UserSettingsData>(
            //    "settings.json",
            //    FileType.JSON);

            //_ingameData.Data = new IngameData();
            //_userSettingsData.Data = new UserSettingsData();

            //_ingameData.Data.SetFileInformation(GameVersion, InTestMode);
            //_userSettingsData.Data.SetFileInformation(GameVersion, InTestMode);

            //LoadAllData();
        }

        private void LoadAllData()
        {
            _ingameData.Load();
            _userSettingsData.Load();
        }
    }
}
