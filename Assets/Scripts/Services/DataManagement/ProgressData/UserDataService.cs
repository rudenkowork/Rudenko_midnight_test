namespace Services.DataManagement.ProgressData
{
    public class UserDataService : IUserData, IUserDataService
    {
        public UserData UserData { get; set; }
        
        private readonly IStaticDataService _staticDataService;
        
        public UserDataService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
    }
}