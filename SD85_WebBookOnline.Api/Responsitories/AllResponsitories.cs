using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace SD85_WebBookOnline.Responsitories
{
	public class AllResponsitories<T> : IAllResponsitories<T> where T : class
	{
	    private	AppDbContext context;
		private DbSet<T> dbset;
        public AllResponsitories()
        {
            
        }
        public AllResponsitories(AppDbContext context, DbSet<T> dbset)
        {
            this.context = context;
			this.dbset = dbset;	
        }
        public async Task<bool> CreateItem(T item)
		{
			try
			{
				await dbset.AddAsync(item);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteItem(T item)
		{
			try
			{
				dbset.Remove(item);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			return await dbset.ToListAsync();
		}
        public async Task<T> GetByID(Guid id)
        {
            return await dbset.FindAsync(id);
        }

        public async Task<bool> UpdateItem(T item)
		{
			try
			{
				dbset.Update(item);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
