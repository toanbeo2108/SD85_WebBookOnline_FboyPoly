using SD85_WebBookOnline.Share.ViewModels;

namespace SD85_WebBookOnline.Api.IServices
{
    public interface IRegisterServices
    {
        Task<Response> RegisterAsync(RegisterUser registerUser, string role);

    }
}
