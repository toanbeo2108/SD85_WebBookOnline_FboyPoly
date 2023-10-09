using SD85_WebBookOnline.Share.ViewModels;

namespace SD85_WebBookOnline.Api.IResponsitories
{
    public interface ILoginServices
    {
        Task<Response> LoginAsync(LoginUser loginUser);
    }
}
