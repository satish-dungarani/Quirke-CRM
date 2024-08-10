namespace Quirke.CRM.Services
{
    public interface IMasterService
    {
        Task<MasterModel> GetByIdAsync(int id);
        Task<IEnumerable<MasterModel>> GetAllByMasterTypeIdAsync(int masterTypeId);
        Task<MasterModel> CreateAsync(MasterModel masterModel);
        Task<MasterModel> UpdateAsync(MasterModel masterModel);
        Task<bool> DeleteAsync(int id);
    }
}
