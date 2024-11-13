using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;
using PepeWeb.Data.Enums;
using PepeWeb.Data.VirtualModels;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace PepeWeb.Services
{
    public class TableCreationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TableCreationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateTable(IntermediateNewTableData newTableData)
        {

            Debug.WriteLine("Etered CreateTable function");

            if (newTableData == null) throw new ArgumentNullException(nameof(newTableData));

            try
            {
                Table newTable = new Table
                {
                    Name = newTableData.TableName,
                    UserId = newTableData.UserId,
                    ItemAmount = 0
                };

                _context.Tables.Add(newTable);
                await _context.SaveChangesAsync();

                foreach (Field field in newTableData.Fields)
                {
                    field.Table = newTable;
                    if (field.Type == null)
                    {
                        field.Type = CustomFieldType.Text;
                    }
                }

                _context.Fields.AddRange(newTableData.Fields);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            // TODO return a message saying that creating table was sucessfull.
            // TODO make sure the user can't spam the submit button.
            return;
        }

        public async Task<bool> TableExists(string tableName)
        {

            if (await _context.Tables.Where(t => t.Name == tableName).FirstOrDefaultAsync() == null)
            {
                return false;
            }
            return true;
        }

            

    }
}
