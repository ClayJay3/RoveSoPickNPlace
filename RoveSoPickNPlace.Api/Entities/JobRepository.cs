using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Entities
{
    public class JobRepository : IJobRepository
    {
        // Declare member variables.
        private readonly RoveSoPickNPlaceDatabase _Database;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Implicitly passed in.</param>
        public JobRepository(RoveSoPickNPlaceDatabase database)
        {
            // Initialize member variables.
            _Database = database;
        }

        /// <summary>
        /// Add a new Job to the database.
        /// </summary>
        /// <param name="job">The job model to add.</param>
        /// <returns>The final object stored in the DB.</returns>
        public async Task<Job?> AddJob(Job job)
        {
            // Ensure the job has a new GUID if not provided.
            if (job.ID == Guid.Empty)
                job.ID = Guid.NewGuid();

            // Ensure related entities have IDs too.
            if (job.CplFile != null && job.CplFile.ID == Guid.Empty)
                job.CplFile.ID = Guid.NewGuid();

            if (job.Placements != null)
            {
                foreach (var placement in job.Placements)
                {
                    if (placement.ID == Guid.Empty)
                        placement.ID = Guid.NewGuid();

                    // Ensure placement links back to this job
                    placement.JobID = job.ID;

                    if (placement.ComponentDefinition != null && placement.ComponentDefinition.ID == Guid.Empty)
                        placement.ComponentDefinition.ID = Guid.NewGuid();

                    if (placement.InspectionResult != null && placement.InspectionResult.ID == Guid.Empty)
                        placement.InspectionResult.ID = Guid.NewGuid();
                }
            }

            if (job.LogEntries != null)
            {
                foreach (var log in job.LogEntries)
                {
                    if (log.ID == Guid.Empty)
                        log.ID = Guid.NewGuid();

                    // Link back to this job
                    log.JobID = job.ID;
                }
            }

            // Add and save.
            _Database.Jobs.Add(job);
            await _Database.SaveChangesAsync();

            // Re-query with includes so we return the full hydrated object.
            return await _Database.Jobs
                .Include(j => j.CplFile)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.ComponentDefinition)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.InspectionResult)
                .Include(j => j.LogEntries)
                .FirstOrDefaultAsync(x => x.ID == job.ID);
        }

        /// <summary>
        /// Remove a Job from the database.
        /// </summary>
        /// <param name="jobID">The ID of the job to remove.</param>
        /// <returns></returns>
        public async Task<Job?> DeleteJob(Guid jobID)
        {
            // Find the first job with the same ID.
            var result = await _Database.Jobs.FirstOrDefaultAsync(x => x.ID == jobID);
            // Check if it was found.
            if (result is not null)
            {
                // Remove the row from the database.
                _Database.Jobs.Remove(result);
                await _Database.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Get all the jobs in the database.
        /// </summary>
        /// <returns>A List containing all the Jobs.</returns>
        public async Task<List<Job>> GetAllJobs()
        {
            return await _Database.Jobs
                .Include(j => j.CplFile)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.ComponentDefinition)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.InspectionResult)
                .Include(j => j.LogEntries)
                .ToListAsync();
        }

        /// <summary>
        /// Get a job from the database.
        /// </summary>
        /// <param name="jobID">The ID of the job you want.</param>
        /// <returns>The job matching the given ID, null is not found.</returns>
        public async Task<Job?> GetJob(Guid jobID)
        {
            return await _Database.Jobs.Include(j => j.CplFile)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.ComponentDefinition)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.InspectionResult)
                .Include(j => j.LogEntries)
                .FirstOrDefaultAsync(x => x.ID == jobID);
        }

        /// <summary>
        /// Update the data for a Job in the DB.
        /// </summary>
        /// <param name="job">The Job object containing the new data.</param>
        /// <returns>The final object stored in the database</returns>
        public async Task<Job?> UpdateJob(Job job)
        {
            // Load the existing job with all relevant navigations
            var result = await _Database.Jobs
                .Include(j => j.CplFile)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.ComponentDefinition)
                .Include(j => j.Placements)
                    .ThenInclude(p => p.InspectionResult)
                .Include(j => j.LogEntries)
                .FirstOrDefaultAsync(x => x.ID == job.ID);

            if (result is null)
                return null;

            // --- Update simple scalar properties ---
            var type = typeof(Job);
            foreach (PropertyInfo property in type.GetProperties())
            {
                // Skip navigation properties (collections or entities)
                if (property.PropertyType != typeof(string) &&
                    (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType) ||
                    property.PropertyType.IsClass))
                {
                    continue;
                }

                var newValue = property.GetValue(job);
                if (newValue != null)
                    property.SetValue(result, newValue);
            }

            // --- Handle Placements manually ---
            if (job.Placements != null)
            {
                // Remove placements not present in the updated job
                var toRemove = result.Placements
                    .Where(existing => !job.Placements.Any(updated => updated.ID == existing.ID))
                    .ToList();

                foreach (var placement in toRemove)
                    _Database.Remove(placement);

                // Add or update placements
                foreach (var placement in job.Placements)
                {
                    var existingPlacement = result.Placements.FirstOrDefault(p => p.ID == placement.ID);
                    if (existingPlacement == null)
                    {
                        // New placement
                        result.Placements.Add(placement);
                    }
                    else
                    {
                        // Update scalar fields of existing placement
                        _Database.Entry(existingPlacement).CurrentValues.SetValues(placement);

                        // Update nested objects like ComponentDefinition and InspectionResult
                        if (placement.ComponentDefinition != null)
                        {
                            _Database.Entry(existingPlacement.ComponentDefinition ?? new ComponentDefinition())
                                .CurrentValues.SetValues(placement.ComponentDefinition);
                        }

                        if (placement.InspectionResult != null)
                        {
                            _Database.Entry(existingPlacement.InspectionResult ?? new InspectionResult())
                                .CurrentValues.SetValues(placement.InspectionResult);
                        }
                    }
                }
            }

            // --- Handle LogEntries manually ---
            if (job.LogEntries != null)
            {
                var toRemoveLogs = result.LogEntries
                    .Where(existing => !job.LogEntries.Any(updated => updated.ID == existing.ID))
                    .ToList();

                foreach (var log in toRemoveLogs)
                    _Database.Remove(log);

                foreach (var log in job.LogEntries)
                {
                    var existingLog = result.LogEntries.FirstOrDefault(l => l.ID == log.ID);
                    if (existingLog == null)
                    {
                        result.LogEntries.Add(log);
                    }
                    else
                    {
                        _Database.Entry(existingLog).CurrentValues.SetValues(log);
                    }
                }
            }

            // --- Handle CplFile reference safely ---
            if (job.CplFileId != Guid.Empty && result.CplFileId != job.CplFileId)
            {
                result.CplFileId = job.CplFileId;
            }

            await _Database.SaveChangesAsync();
            return result;
        }
    }
}