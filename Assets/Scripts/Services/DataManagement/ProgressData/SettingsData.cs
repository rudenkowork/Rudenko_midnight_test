using System;

namespace Services.DataManagement.ProgressData
{
    [Serializable]
    public class SettingsData
    {
        public bool IsMusicOn = true;
        public bool IsSoundOn = true;
        public bool ShowTutorial = true;
        public bool WelcomeBonusReceived;
    }
}