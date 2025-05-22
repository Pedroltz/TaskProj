using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace TaskApi.Services
{
    public class FirebaseAuthService
    {
        public FirebaseAuthService(IWebHostEnvironment env)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(env.ContentRootPath, "firebase-key.json"))
                });
            }
        }

        public async Task<string> CreateUserAsync(string email, string password)
        {
            var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
            {
                Email = email,
                Password = password
            });
            return user.Uid;
        }

        public async Task UpdateUserAsync(string uid, string? email = null, string? password = null)
        {
            var args = new UserRecordArgs { Uid = uid };
            if (email != null) args.Email = email;
            if (password != null) args.Password = password;

            await FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
        }

        public async Task DeleteUserAsync(string uid)
        {
            await FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);
        }
    }
}
