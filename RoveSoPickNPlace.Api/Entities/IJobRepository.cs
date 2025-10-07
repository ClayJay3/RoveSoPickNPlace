using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Entities
{
    public interface IJobRepository
    {
        Task<Job?> AddJob(Job job);
        Task<Job?> DeleteJob(Guid jobID);
        Task<List<Job>> GetAllJobs();
        Task<Job?> GetJob(Guid jobID);
        Task<Job?> UpdateJob(Job job);
    }
}