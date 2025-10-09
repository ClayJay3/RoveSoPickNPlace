using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Entities
{
    public class ComponentPlacementRecordRepository : IComponentPlacementRecordRepository
    {
        private readonly RoveSoPickNPlaceDatabase _Database;

        public ComponentPlacementRecordRepository(RoveSoPickNPlaceDatabase database)
        {
            // Initialize member variables.
            _Database = database;
        }

        public async Task<ComponentPlacementRecord?> AddComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Ensure the record has a new GUID if not provided.
            if (record.ID == Guid.Empty)
                record.ID = Guid.NewGuid();

            // Ensure related entities have IDs too.
            if (record.ComponentDefinitionID != null && record.ComponentDefinition?.ID == Guid.Empty)
                record.ComponentDefinition.ID = Guid.NewGuid();

            if (record.InspectionResult != null && record.InspectionResult.ID == Guid.Empty)
                        record.InspectionResult.ID = Guid.NewGuid();

            // Add and save.
            _Database.ComponentPlacements.Add(record);
            await _Database.SaveChangesAsync();

            // Re-query with includes so we return the full hydrated object.
            return await _Database.ComponentPlacements
                .Include(c => c.ComponentDefinition)
                .Include(c => c.InspectionResult)
                .FirstOrDefaultAsync(x => x.ID == record.ID);
        }

        public async Task<ComponentPlacementRecord?> DeleteComponentPlacementRecord(Guid recordID)
        {
            // Find the first record with the same ID.
            var result = await _Database.ComponentPlacements.FirstOrDefaultAsync(x => x.ID == recordID);
            // Check if it was found.
            if (result is not null)
            {
                // Remove the row from the database.
                _Database.ComponentPlacements.Remove(result);
                await _Database.SaveChangesAsync();
            }
            return result;
        }

        public async Task<List<ComponentPlacementRecord>> GetAllComponentPlacementRecords()
        {
            return await _Database.ComponentPlacements
                .Include(c => c.ComponentDefinition)
                .Include(c => c.InspectionResult)
                .ToListAsync();
        }

        public async Task<ComponentPlacementRecord?> GetComponentPlacementRecord(Guid recordID)
        {
            return await _Database.ComponentPlacements
                .Include(c => c.ComponentDefinition)
                .Include(c => c.InspectionResult)
                .FirstOrDefaultAsync(x => x.ID == recordID);
        }

        public async Task<ComponentPlacementRecord?> UpdateComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Load existing record with navigations
            var result = await _Database.ComponentPlacements
                .Include(c => c.ComponentDefinition)
                .Include(c => c.InspectionResult)
                .FirstOrDefaultAsync(x => x.ID == record.ID);

            if (result is null)
                return null;

            // --- Update simple scalar properties via reflection ---
            var type = typeof(ComponentPlacementRecord);
            foreach (PropertyInfo property in type.GetProperties())
            {
                // Skip navigation properties (collections or classes except string)
                if (property.PropertyType != typeof(string) &&
                    (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType) ||
                    property.PropertyType.IsClass))
                {
                    continue;
                }

                var newValue = property.GetValue(record);
                if (newValue != null)
                    property.SetValue(result, newValue);
            }

            // --- Handle ComponentDefinition update if provided ---
            if (record.ComponentDefinition != null)
            {
                // If there's no existing ComponentDefinition, create a placeholder so EF can track updates
                var targetComponentDef = result.ComponentDefinition ?? new ComponentDefinition();
                // If incoming has no ID but caller intended to create new, ensure ID
                if (record.ComponentDefinition.ID == Guid.Empty)
                    record.ComponentDefinition.ID = Guid.NewGuid();

                _Database.Entry(targetComponentDef).CurrentValues.SetValues(record.ComponentDefinition);

                // If result didn't have a ComponentDefinition before, attach it and set FK
                if (result.ComponentDefinition == null)
                {
                    result.ComponentDefinition = record.ComponentDefinition;
                    result.ComponentDefinitionID = record.ComponentDefinition.ID;
                }
                else
                {
                    // Ensure FK points to updated/selected component definition
                    result.ComponentDefinitionID = record.ComponentDefinition.ID;
                }
            }
            else if (record.ComponentDefinitionID != null && record.ComponentDefinitionID != result.ComponentDefinitionID)
            {
                // If caller only provided an ID to associate, update the FK
                result.ComponentDefinitionID = record.ComponentDefinitionID;
            }

            // --- Handle InspectionResult update if provided ---
            if (record.InspectionResult != null)
            {
                var targetInspection = result.InspectionResult ?? new InspectionResult();
                if (record.InspectionResult.ID == Guid.Empty)
                    record.InspectionResult.ID = Guid.NewGuid();

                _Database.Entry(targetInspection).CurrentValues.SetValues(record.InspectionResult);

                if (result.InspectionResult == null)
                {
                    // Ensure the back-reference FK is correct
                    record.InspectionResult.ComponentPlacementRecordID = result.ID;
                    result.InspectionResult = record.InspectionResult;
                }
                else
                {
                    // Keep the relationship and update the FK if necessary
                    result.InspectionResult.ComponentPlacementRecordID = result.ID;
                }
            }

            await _Database.SaveChangesAsync();
            return result;
        }
    }
}