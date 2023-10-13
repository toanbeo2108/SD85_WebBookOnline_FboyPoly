using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAllResponsitories<Author> _irespon;
        AppDbContext _context = new AppDbContext();
        public AuthorController()
        {
            _irespon = new AllResponsitories<Author>(_context, _context.Authors);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Author>> GetAllAuthor()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateAuthor(string authorName, string? phoneNumber, DateTime? dateOfBirth, DateTime? dateOfDie, string? country, string? image, string? email, string? bio)
        {
            var lstauthor = await _irespon.GetAll();
            var tacGiaCheck = lstauthor.FirstOrDefault(x => x.AuthorName == authorName);
            if (tacGiaCheck != null)
            {
                return false;
            }
            Author at = new Author();
            at.AuthorID = Guid.NewGuid();
            at.AuthorName = authorName;
            at.PhoneNumber = phoneNumber;
            at.DateOfBirth = dateOfBirth;
            at.DateOfDie = dateOfDie;
            at.Country = country;
            at.Image = image;
            at.Email = email;
            at.Bio = bio;
            at.Status = 1;

            return await _irespon.CreateItem(at);
        }

        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateAuthor(Guid authorID, string authorName, string? phoneNumber, DateTime? dateOfBirth, DateTime? dateOfDie, string? country, string? image, string? email, string? bio, int? status)
        {
            var lstauthor = await _irespon.GetAll();
            var at = lstauthor.FirstOrDefault(x => x.AuthorID == authorID);
            if (at == null)
            {
                return false;
            }
            at.AuthorName = authorName;
            at.PhoneNumber = phoneNumber;
            at.DateOfBirth = dateOfBirth;
            at.DateOfDie = dateOfDie;
            at.Country = country;
            at.Image = image;
            at.Email = email;
            at.Bio = bio;
            at.Status = status;

            return await _irespon.UpdateItem(at);
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteAuthor(Guid authorID, int? status)
        {
            var lstauthor = await _irespon.GetAll();
            var at = lstauthor.FirstOrDefault(x => x.AuthorID == authorID);
            if (at == null)
            {
                return false;
            }

            at.Status = 0;

            return await _irespon.UpdateItem(at);
        }
    }
}
