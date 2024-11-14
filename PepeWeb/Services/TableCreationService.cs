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
using Microsoft.AspNetCore.HttpLogging;

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

        public async Task<TableDTO> CreateTable(IntermediateNewTableData newTableData)
        {

            if (newTableData == null) throw new ArgumentNullException(nameof(newTableData));
            Table newTable = new Table
            {
                Name = newTableData.TableName,
                UserId = newTableData.UserId,
                ItemAmount = 0
            };
            try
            {
                _context.Tables.Add(newTable);
                await _context.SaveChangesAsync();

                var fields = _mapper.Map<List<Field>>(newTableData.Fields);
                foreach (Field field in fields)
                {
                    field.Table = newTable;
                    if (field.Type == null)
                    {
                        field.Type = CustomFieldType.Text;
                    }
                }

                _context.Fields.AddRange(fields);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            return _mapper.Map<TableDTO>(newTable);
        }

        public async Task EditTable(TableConstructDTO newTableData)
        {

            if (newTableData == null) throw new ArgumentNullException(nameof(newTableData));

            try
            {
                //Find the table by id and change its name,if it changed
                var liveTable = _context.Tables.Where(t => t.Id == newTableData.Id).FirstOrDefault();

                if ( liveTable != null && newTableData.Name != liveTable.Name )
                {
                    liveTable.Name = newTableData.Name;
                }

                var fields = _mapper.Map<List<Field>>(newTableData.Columns);


                foreach (Field field in fields)
                {
                    field.Table = liveTable;
                    var liveField = _context.Fields.Where(f => f.Table == liveTable && f.Id == field.Id).FirstOrDefault();

                    if (liveField != null)
                    {
                        //OPTIMIZE Field already exists. Something has changed? Don't matter just overwrite it.
                        _context.Entry(liveField).CurrentValues.SetValues(field);
                    }
                    else
                    {
                        _context.Fields.Add(field);
                    }

                }

                //Check if any field had been deleted
                var newFieldIds = newTableData.Columns.Select(c => c.Id).ToHashSet();
                var existingFields = await _context.Fields.Where(f => f.Table == liveTable).ToListAsync();
                foreach (Field existingField in existingFields)
                {

                    if (!newFieldIds.Contains(existingField.Id))
                    {
                        _context.Fields.Remove(existingField);
                    }

                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

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
