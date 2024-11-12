using AutoMapper;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.VirtualModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace PepeWeb.Services
{
    public class TableService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private string _userId;

        public TableService(ApplicationDbContext context, IMapper mapper, AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<TableDTO>> GetUserTables()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var userTables = await _context.Tables
                                   .Where(t => t.UserId == _userId)
                                   .ToListAsync();

            List<TableDTO> userTableDTOs = _mapper.Map<List<TableDTO>>(userTables);

            return userTableDTOs;

        }
    }
}
