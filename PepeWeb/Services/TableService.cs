using AutoMapper;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.VirtualModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PepeWeb.Services
{
    public class TableService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private string? _userId;

        public TableService(ApplicationDbContext context, IMapper mapper, AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<TableDTO>> GetUserTables()
        {
            // Get user ID
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            // Load user's tables
            var userTables = await _context.Tables
                                   .Where(t => t.UserId == _userId)
                                   .OrderBy(x => x.Name)
                                   .ToListAsync();

            List<TableDTO> userTableDTOs = _mapper.Map<List<TableDTO>>(userTables);

            return userTableDTOs;

        }

        public async Task<TableConstructDTO> GetUserTableData(int tableId)
        {

            // Get user ID
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            // Get data
            var userTable = _context.Tables.Where(t => t.UserId == _userId && t.Id == tableId).FirstOrDefault();
            var tableFields = _context.Fields.Where(f => f.Table == userTable).ToList();
            var tableValues = _context.Values.Where(v => v.Table == userTable).ToList();

            // Init rows
            List<TableRow> tableRows = new List<TableRow>();

            if (userTable != null)
            {
                for (int i = 00; i < userTable.ItemAmount; i++)
                {

                    // Collect values and assign them to a row
                    var rowValues = tableValues.Where(v => v.ItemId == i+1).ToList();
                    var row = new TableRow() { ItemId = i, Values = new List<string>() };
                    foreach (var field in tableFields)
                    { 
                        var value = rowValues.Where(vr => vr.Field.Id == field.Id).FirstOrDefault();

                        if (value != null && value.Val != null)
                        {
                            row.Values.Add(value.Val.ToString());
                        }
                        else
                        {
                            row.Values.Add("");
                        }
                    };

                    // Add the row to the table rows
                    tableRows.Add(row);
                }
            }
            else
            {
                throw new Exception("User table not found");
            }

            // Construct an object that structures and holds the collected data
            TableConstructDTO constructedUserTable = new TableConstructDTO()
            {
                Id = tableId,
                Name = userTable.Name,
                Columns = _mapper.Map<List<FieldDTO>>(tableFields),
                Rows = tableRows
            };

            return constructedUserTable;

        }

        public async Task<TableDTO> GetTable(int tableId)
        {
            return _mapper.Map<TableDTO>(await _context.Tables.Where(t => t.Id == tableId).FirstOrDefaultAsync());
        }
    }
}
