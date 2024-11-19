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

        public async Task AddRecord(int tableId, List<ValueDTO> valueList)
        {
            // Get the tracked table entity
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == tableId);
            if (table == null)
                throw new Exception("Table not found.");

            // Increment item count
            int newAmount = table.ItemAmount += 1;
            table.ItemAmount = newAmount;

            // Map ValueDTO to Value entities
            var values = _mapper.Map<List<Value>>(valueList);

            foreach (var value in values)
            {
                // Assign the existing table entity to avoid EF tracking duplicates
                value.Table = table;

                // If a Field is referenced, resolve it to a tracked entity
                if (value.Field != null)
                {
                    var field = await _context.Fields.FirstOrDefaultAsync(f => f.Id == value.Field.Id);
                    if (field != null)
                    {
                        value.Field = field; // Use the tracked field entity
                    }
                }

                // Assign the incremented item ID
                value.ItemId = newAmount;
            }

            // Add and save
            _context.Values.AddRange(values);
            await _context.SaveChangesAsync();
        }

        public async Task EditRecord(List<ValueDTO> valueList)
        {
            foreach (var valueDto in valueList)
            {
                // Find the existing Value entity in the database
                var existingValue = await _context.Values
                    .Include(v => v.Table) // Include related table entity if needed
                    .Include(v => v.Field) // Include related field entity if needed
                    .FirstOrDefaultAsync(v => v.Id == valueDto.Id);

                if (existingValue == null)
                {
                    throw new Exception($"Value with ID {valueDto.Id} not found.");
                }

                // Update the fields of the existing entity with the new values from the DTO
                existingValue.Val = valueDto.Val;

                // If needed, update other properties of the entity here
                if (valueDto.Field != null)
                {
                    var field = await _context.Fields.FirstOrDefaultAsync(f => f.Id == valueDto.Field.Id);
                    if (field != null)
                    {
                        existingValue.Field = field; // Ensure the Field reference is properly resolved
                    }
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
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
