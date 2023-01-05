using UnityEngine;
using System;
using Mixin.Save;
using Mixin.Language;

namespace Mixin.TheLastMove.Save
{
    [Serializable]
    public class UserSettingsData : DataFile
    {
        public int MusicVolume = 100;
        public int SoundVolume = 100;
        public int Quality = 6;
        public Language.Language Language = Mixin.Language.Language.English;
    }
}
