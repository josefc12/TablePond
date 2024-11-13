using AutoMapper;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.VirtualModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PepeWeb.Data.Models;
using System.Diagnostics;

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

        public async Task<TableConstructDTO> GetUserTableData(int tableId)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var userTable = _context.Tables.Where(t => t.UserId == _userId && t.Id == tableId).FirstOrDefault();

            var tableFields = _context.Fields.Where(f => f.Table == userTable).ToList();

            var tableValues = _context.Values.Where(v => v.Table == userTable).ToList();

            List<TableRow> tableRows = new List<TableRow>();
            
            for (int i = 0; i < userTable.ItemAmount; i++)
            {
                var rowValues = tableValues.Where(v => v.ItemId == i).ToList();
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

                tableRows.Add(row);

            }

            TableConstructDTO constructedUserTable = new TableConstructDTO()
            {
                Id = tableId,
                Name = userTable.Name,
                Columns = _mapper.Map<List<FieldDTO>>(tableFields),
                Rows = tableRows
            };

            return constructedUserTable;

        }
    }
}
