using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using static PepeWeb.Components.Pages.Table;

namespace PepeWeb.Services
{
    public class WeatherService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WeatherService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TableDTO>> Get()
        {
            return await _context.Tables.Select(t => new TableDTO { Name = t.Name }).ToListAsync();
        }
    }
}
