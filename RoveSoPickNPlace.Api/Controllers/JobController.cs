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
    public class JobController : ControllerBase
    {
        // Declare member variables.
        private readonly IJobRepository _JobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _JobRepository = jobRepository;
        }

        /// <summary>
        /// IN-Code API Endpoint for adding a waypoint to the DB.
        /// </summary>
        /// <param name="job">The job object.</param>
        /// <returns>API response object.</returns>
        [HttpPut]
        public async Task<IActionResult> AddJob(Job job)
        {
            // Add the job to the database.
            var dbJob = await _JobRepository.AddJob(job);
            // Check if successful.
            if (dbJob is not null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("The given job was not valid.");
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for removing a job from the DB.
        /// </summary>
        /// <param name="jobID">The job ID to remove.</param>
        /// <returns>API response object.</returns>
        [HttpDelete("{jobID}")]
        public async Task<IActionResult> DeleteJob(Guid jobID)
        {
            // Delete from the database.
            var dbJob = await _JobRepository.DeleteJob(jobID);
            // Check if successful.
            if (dbJob is not null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting all jobs from the DB.
        /// </summary>
        /// <returns>API Response object.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            return Ok(await _JobRepository.GetAllJobs());
        }

        /// <summary>
        /// IN-Code API Endpoint for getting a job from the DB.
        /// </summary>
        /// <param name="jobID">The job id.</param>
        /// <returns>API response object.</returns>
        [HttpGet("{jobID}")]
        public async Task<IActionResult> GetJob(Guid jobID)
        {
            // Get from the database.
            var dbJob = await _JobRepository.GetJob(jobID);
            // Check if successful.
            if (dbJob is not null)
            {
                return Ok(dbJob);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for updating a job in the DB.
        /// </summary>
        /// <param name="job">The job object to update.</param>
        /// <returns>API response object.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateJob(Job job)
        {
            // Update the database.
            var dbJob = await _JobRepository.UpdateJob(job);
            // Check if successful.
            if (dbJob is not null)
            {
                return Ok(dbJob);
            }
            else
            {
                return NotFound();
            }
        }
    }
}