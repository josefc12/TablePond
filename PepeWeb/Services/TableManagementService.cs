using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;
using PepeWeb.Data.VirtualModels;

namespace PepeWeb.Services
{
    public class TableManagementService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TableManagementService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TableDTO> CreateTable(IntermediateNewTableData newTableData)
        {

            if (newTableData == null) throw new ArgumentNullException(nameof(newTableData));

            // Init new table object
            Table newTable = new Table
            {
                Name = newTableData.TableName,
                UserId = newTableData.UserId,
                ItemAmount = 0
            };

            try
            {
                // Make sure the table is added before anything else
                _context.Tables.Add(newTable);
                await _context.SaveChangesAsync();

                // Get the fields
                var fields = _mapper.Map<List<Field>>(newTableData.Fields);

                // Assign the table to each field
                foreach (Field field in fields)
                {
                    field.Table = newTable;
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

                if (liveTable != null)
                {
                    foreach (Field field in fields)
                    {
                        field.Table = liveTable;
                        var liveField = _context.Fields.Where(f => f.Table == liveTable && f.Id == field.Id).FirstOrDefault();

                        if (liveField != null)
                        {
                            //Field already exists. Something has changed? Don't matter just overwrite it.
                            _context.Entry(liveField).CurrentValues.SetValues(field);
                        }
                        else
                        {
                            _context.Fields.Add(field);
                        }

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
