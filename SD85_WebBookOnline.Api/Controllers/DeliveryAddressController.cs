using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/DeliveryAddress")]
    [ApiController]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly IAllResponsitories<DeliveryAddress> _irespon;
        AppDbContext _context = new AppDbContext();
        public DeliveryAddressController()
        {
            _irespon = new AllResponsitories<DeliveryAddress>(_context, _context.DeliveryAddress);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<DeliveryAddress>> GetAllDeliveryAddress()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateDeliveryAddress(string consigneeName, string phoneNumber, string addressLine, string city, string country, string description)
        {
            var lstauthor = await _irespon.GetAll();
            var tacGiaCheck = lstauthor.FirstOrDefault(x => x.ConsigneeName == consigneeName);
            if (tacGiaCheck != null)
            {
                return false;
            }
            DeliveryAddress adr = new DeliveryAddress();
            adr.DeliveryAddressID = Guid.NewGuid();
            adr.ConsigneeName = consigneeName;
            adr.PhoneNumber = phoneNumber;
            adr.AddressLine = addressLine;
            adr.City = city;
            adr.Country = country;
            adr.Description = description;
            adr.Status = 1;

            return await _irespon.CreateItem(adr);
        }

        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateDeliveryAddress(Guid id, string consigneeName, string phoneNumber, string addressLine, string city, string country, string description, int status)
        {
            var lstauthor = await _irespon.GetAll();
            var adr = lstauthor.FirstOrDefault(x => x.DeliveryAddressID == id);
            if (adr == null)
            {
                return false;
            }
            adr.ConsigneeName = consigneeName;
            adr.PhoneNumber = phoneNumber;
            adr.AddressLine = addressLine;
            adr.City = city;
            adr.Country = country;
            adr.Description = description;
            adr.Status = status;

            return await _irespon.UpdateItem(adr);
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteDeliveryAddress(Guid id)
        {
            var lstauthor = await _irespon.GetAll();
            var adr = lstauthor.FirstOrDefault(x => x.DeliveryAddressID == id);
            if (adr == null)
            {
                return false;
            }

            adr.Status = 0;

            return await _irespon.UpdateItem(adr);
        }
    }
}
