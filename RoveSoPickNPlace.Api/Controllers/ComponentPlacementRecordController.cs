using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoveSoPickNPlace.Api.Entities;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentPlacementRecordController : ControllerBase
    {
        // Declare member variables.
        private readonly IComponentPlacementRecordRepository _ComponentPlacementRecordRepository;

        public ComponentPlacementRecordController(IComponentPlacementRecordRepository recordRepository)
        {
            _ComponentPlacementRecordRepository = recordRepository;
        }

        [HttpPut]
        public async Task<IActionResult> AddComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Add the record to the database.
            var dbComponentPlacementRecord = await _ComponentPlacementRecordRepository.AddComponentPlacementRecord(record);
            // Check if successful.
            if (dbComponentPlacementRecord is not null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("The given record was not valid.");
            }
        }

        [HttpDelete("{recordID}")]
        public async Task<IActionResult> DeleteComponentPlacementRecord(Guid recordID)
        {
            // Delete from the database.
            var dbComponentPlacementRecord = await _ComponentPlacementRecordRepository.DeleteComponentPlacementRecord(recordID);
            // Check if successful.
            if (dbComponentPlacementRecord is not null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComponentPlacementRecords()
        {
            return Ok(await _ComponentPlacementRecordRepository.GetAllComponentPlacementRecords());
        }

        [HttpGet("{recordID}")]
        public async Task<IActionResult> GetComponentPlacementRecord(Guid recordID)
        {
            // Get from the database.
            var dbComponentPlacementRecord = await _ComponentPlacementRecordRepository.GetComponentPlacementRecord(recordID);
            // Check if successful.
            if (dbComponentPlacementRecord is not null)
            {
                return Ok(dbComponentPlacementRecord);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Update the database.
            var dbComponentPlacementRecord = await _ComponentPlacementRecordRepository.UpdateComponentPlacementRecord(record);
            // Check if successful.
            if (dbComponentPlacementRecord is not null)
            {
                return Ok(dbComponentPlacementRecord);
            }
            else
            {
                return NotFound();
            }
        }
    }
}