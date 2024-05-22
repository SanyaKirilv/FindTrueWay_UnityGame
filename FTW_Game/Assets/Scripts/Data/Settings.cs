using System;
using FTW.Data.Enums;

namespace FTW.Data
{
    [Serializable]
    public class Settings
    {
        public Theme Theme;
        public Language Language;
        public bool Audio;
        public bool Ads;
    }
}
