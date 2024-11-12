using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;
using PepeWeb.Data.Enums;
using PepeWeb.Data.VirtualModels;
using System.Diagnostics;

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

        public async Task<string> CreateTable(IntermediateNewTableData newTableData)
        {

            Debug.WriteLine("Etered CreateTable function");

            if (newTableData == null) throw new ArgumentNullException(nameof(newTableData));

            try
            {
                Table newTable = new Table
                {
                    Name = newTableData.TableName,
                    UserId = newTableData.UserId,
                };

                await _context.Tables.AddAsync(newTable);
                await _context.SaveChangesAsync();

                foreach (Field field in newTableData.Fields)
                {
                    field.TableId = newTable.Id;
                    if (field.Type == null)
                    {
                        field.Type = CustomFieldType.Text;
                    }
                }

                await _context.Fields.AddRangeAsync(newTableData.Fields);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            // TODO return a message saying that creating table was sucessfull.
            // TODO make sure the user can't spam the submit button.
            return "Done";
        }
    }
}
