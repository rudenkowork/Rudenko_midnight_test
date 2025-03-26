using Services.DataManagement.ProgressData;

namespace Services.DataManagement
{
    public interface ILoadUserData
    {
        void LoadUserData(IUserData userData);
    }

    public interface ISetUserData
    {
        void SetUserData(IUserData userData);
    }

    public interface ISetLoadUserData : ILoadUserData, ISetUserData
    {
    }
}