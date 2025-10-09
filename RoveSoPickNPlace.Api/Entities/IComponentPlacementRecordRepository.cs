using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Entities
{
    public interface IComponentPlacementRecordRepository
    {
        Task<ComponentPlacementRecord?> AddComponentPlacementRecord(ComponentPlacementRecord record);
        Task<ComponentPlacementRecord?> DeleteComponentPlacementRecord(Guid recordID);
        Task<List<ComponentPlacementRecord>> GetAllComponentPlacementRecords();
        Task<ComponentPlacementRecord?> GetComponentPlacementRecord(Guid recordID);
        Task<ComponentPlacementRecord?> UpdateComponentPlacementRecord(ComponentPlacementRecord record);
    }
}