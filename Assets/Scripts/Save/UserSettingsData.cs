using UnityEngine;
using System;
using Mixin.Save;
using Mixin.MultiLanguage;

namespace Mixin.TheLastMove.Save
{
    [Serializable]
    public class UserSettingsData : DataFile
    {
        public bool FirstTime = true;
        public int MusicVolume = 100;
        public int SoundVolume = 100;
        public int Quality = 6;
        public Language Language = Language.English;
    }
}
