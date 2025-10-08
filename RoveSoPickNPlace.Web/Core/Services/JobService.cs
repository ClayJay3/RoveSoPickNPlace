using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Web.Core.Services
{
    public class JobService
    {
        // Injected services.
        private readonly HttpClient _HttpClient;
        // Declare member variables.
        private List<Job> _jobs = [];

        // Method delegates and events.
        public delegate Task SyncJobsCallback();
        private event SyncJobsCallback? SyncJobsNotifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">Implicitly passed in.</param>
        public JobService(HttpClient httpClient)
        {
            // Initialize member variables.
            _HttpClient = httpClient;
        }

        /// <summary>
        /// Gets an updated list of jobs from the API.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshJobs()
        {
            // Get all jobs from the API.
            List<Job>? jobs = await _HttpClient.GetFromJsonAsync<List<Job>>("http://localhost:5000/api/Job");
            // Check if we got anything.
            if (jobs is not null)
            {
                _jobs = jobs;
            }

            // Invoke the callback to refresh page data.
            if (SyncJobsNotifier is not null)
            {
                await SyncJobsNotifier.Invoke();
            }
        }

        /// <summary>
        /// Add a callback to get invoked when the job list changes.
        /// </summary>
        /// <param name="callback">The method callback to add.</param>
        public void SubscribeToJobChanges(SyncJobsCallback callback)
        {
            SyncJobsNotifier += callback;
        }

        /// <summary>
        /// Remove a callback from getting invoked when the job list changes.
        /// </summary>
        /// <param name="callback">The method callback to remove.</param>
        public void UnsubscribeFromJobChanges(SyncJobsCallback callback)
        {
            SyncJobsNotifier -= callback;
        }

        /// <summary>
        /// Add a job to the API.
        /// </summary>
        /// <param name="job">The job to add.</param>
        /// <returns></returns>
        public async Task AddJob(Job job)
        {
            // Add the job to the database with the API.
            await _HttpClient.PutAsJsonAsync($"http://localhost:5000/api/Job", job);
            // Refresh data.
            await RefreshJobs();
        }

        /// <summary>
        /// Update a job in the database.
        /// </summary>
        /// <param name="job">The job to update. ID must match an existing ID.</param>
        /// <returns></returns>
        public async Task UpdateJob(Job job)
        {
            // Write the job data to the database with the API.
            await _HttpClient.PostAsJsonAsync($"http://localhost:5000/api/Job", job);
            // Refresh data.
            await RefreshJobs();
        }

        /// <summary>
        /// Given a job ID delete it from the database.
        /// </summary>
        /// <param name="job">The job to delete.</param>
        /// <returns></returns>
        public async Task DeleteJob(Job job)
        {
            // Delete the job from the database.
            await _HttpClient.DeleteAsync($"http://localhost:5000/api/Job/{job.ID}");
            // Refresh data.
            await RefreshJobs();
        }

        /// <summary>
        /// Return a reference to the list of Jobs.
        /// </summary>
        /// <returns></returns>
        public List<Job> GetJobs()
        {
            return _jobs;
        }

        /// <summary>
        /// Returns the job with the given ID.
        /// </summary>
        /// <param name="jobID">The ID of the job to retrieve.</param>
        /// <returns></returns>
        public Job? GetJob(Guid jobID)
        {
            return _jobs.FirstOrDefault(x => x.ID == jobID);
        }
    }
}