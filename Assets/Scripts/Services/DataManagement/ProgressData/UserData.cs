using System;

namespace Services.DataManagement.ProgressData
{
    [Serializable]
    public class UserData
    {
        public SettingsData Settings = new();
        public AnalyticsData Analytics = new();
        public WalletData Wallet = new();
        public FridgeData Fridge = new();
    }
}