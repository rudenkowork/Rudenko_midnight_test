using System.IO;
using UnityEngine;
using Services.DataManagement.ProgressData;
using Tools.Extensions;

namespace Services.DataManagement
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string FileName = "UserData.json";
        private readonly IUserData _userData;
        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public SaveLoadService(IUserData userData)
        {
            _userData = userData;
        }

        public UserData LoadProgress() =>
            File.Exists(FilePath) ? File.ReadAllText(FilePath).ToDeserialized<UserData>() : null;

        public void SetData(ISetUserData userDataSaver)
        {
            userDataSaver.SetUserData(_userData);
            Update();
        }

        public void LoadData(ILoadUserData dataReceiver) =>
            dataReceiver.LoadUserData(_userData);

        public UserData InitialLoad() => new UserData();

        public void Update() =>
            File.WriteAllText(FilePath, _userData.UserData.ToJson());
    }
}