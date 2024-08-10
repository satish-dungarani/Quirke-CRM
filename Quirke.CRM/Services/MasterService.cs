using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;

namespace Quirke.CRM.Services
{
    public class MasterService : IMasterService
    {
        private readonly ApplicationDbContext _context;

        public MasterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MasterModel> GetByIdAsync(int id)
        {
            var master = await _context.Masters.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            return master != null ? MapToModel(master) : null;
        }

        public async Task<IEnumerable<MasterModel>> GetAllByMasterTypeIdAsync(int masterTypeId)
        {
            var masters = await _context.Masters
            .Where(m => m.MasterTypeId == masterTypeId && !m.IsDeleted)
            .ToListAsync();

            return masters.Select(MapToModel);
        }

        public async Task<MasterModel> CreateAsync(MasterModel masterModel)
        {
            var master = MapToEntity(masterModel);
            master.CreatedOn = DateTime.Now;

            _context.Masters.Add(master);
            await _context.SaveChangesAsync();

            return MapToModel(master);
        }

        public async Task<MasterModel> UpdateAsync(MasterModel masterModel)
        {
            var master = await _context.Masters.FirstOrDefaultAsync(m => m.Id == masterModel.Id && !m.IsDeleted);
            if (master == null) return null;

            master.Name = masterModel.Name;
            master.MasterTypeId = masterModel.MasterTypeId;
            master.UpdatedOn = DateTime.Now;

            _context.Masters.Update(master);
            await _context.SaveChangesAsync();

            return MapToModel(master);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var master = await _context.Masters.FirstOrDefaultAsync(m => m.Id == id);
            if (master == null) return false;

            master.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // Mapping Methods
        private MasterModel MapToModel(Master master)
        {
            return new MasterModel
            {
                Id = master.Id,
                Name = master.Name,
                MasterTypeId = master.MasterTypeId,
                IsDeleted = master.IsDeleted,
                CreatedOn = master.CreatedOn,
                UpdatedOn = master.UpdatedOn
            };
        }

        private Master MapToEntity(MasterModel masterModel)
        {
            return new Master
            {
                Id = masterModel.Id,
                Name = masterModel.Name,
                MasterTypeId = masterModel.MasterTypeId,
                IsDeleted = masterModel.IsDeleted,
                CreatedOn = masterModel.CreatedOn,
                UpdatedOn = masterModel.UpdatedOn
            };
        }
    }
}
