using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;
using System.Diagnostics;

namespace PepeWeb.Services
{
    public class ValueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private string? _userId;

        public ValueService(ApplicationDbContext context, IMapper mapper, AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<ValueDTO>> GetValuesByItemId(int tableId,int itemId)
        {
            List<ValueDTO> vavav = _mapper.Map<List<ValueDTO>>(await _context.Values.Include(t => t.Table).Include(f => f.Field).Where(v => v.Table.Id == tableId && v.ItemId == itemId).ToListAsync());

            return vavav;
        }

    }
}
