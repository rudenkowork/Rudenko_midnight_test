using Services.DataManagement.ProgressData;

namespace Services.DataManagement
{
    public interface ISaveLoadService
    {
        UserData LoadProgress();
        void SetData(ISetUserData userDataSaver);
        void LoadData(ILoadUserData dataReceiver);
        UserData InitialLoad();
        void Update();
    }
}