using Exam.DAL;
using Microsoft.EntityFrameworkCore;

namespace Exam.Services
{
    public class LayoutService
    {
        private readonly AppDBContext _context;

        public LayoutService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            return settings;
        }
    }
}
