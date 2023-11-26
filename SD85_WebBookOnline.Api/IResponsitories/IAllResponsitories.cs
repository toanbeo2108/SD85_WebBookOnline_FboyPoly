using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SD85_WebBookOnline.Api.IResponsitories
{
	public interface IAllResponsitories<T>
	{
		Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(Guid id);
        Task<bool> CreateItem(T item);
		Task<bool> DeleteItem(T item);
		Task<bool> UpdateItem(T item);
	}
}
