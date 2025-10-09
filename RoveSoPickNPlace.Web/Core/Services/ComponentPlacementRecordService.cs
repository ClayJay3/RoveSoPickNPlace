using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Web.Core.Services
{
    public class ComponentPlacementRecordService
    {
        // Injected services.
        private readonly HttpClient _HttpClient;
        // Declare member variables.
        private List<ComponentPlacementRecord> _records = [];

        // Method delegates and events.
        public delegate Task SyncComponentPlacementRecordsCallback();
        private event SyncComponentPlacementRecordsCallback? SyncComponentPlacementRecordsNotifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">Implicitly passed in.</param>
        public ComponentPlacementRecordService(HttpClient httpClient)
        {
            // Initialize member variables.
            _HttpClient = httpClient;
        }

        /// <summary>
        /// Gets an updated list of records from the API.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshComponentPlacementRecords()
        {
            // Get all records from the API.
            List<ComponentPlacementRecord>? records = await _HttpClient.GetFromJsonAsync<List<ComponentPlacementRecord>>("http://localhost:5000/api/ComponentPlacementRecord");
            // Check if we got anything.
            if (records is not null)
            {
                _records = records;
            }

            // Invoke the callback to refresh page data.
            if (SyncComponentPlacementRecordsNotifier is not null)
            {
                await SyncComponentPlacementRecordsNotifier.Invoke();
            }
        }

        /// <summary>
        /// Add a callback to get invoked when the record list changes.
        /// </summary>
        /// <param name="callback">The method callback to add.</param>
        public void SubscribeToComponentPlacementRecordChanges(SyncComponentPlacementRecordsCallback callback)
        {
            SyncComponentPlacementRecordsNotifier += callback;
        }

        /// <summary>
        /// Remove a callback from getting invoked when the record list changes.
        /// </summary>
        /// <param name="callback">The method callback to remove.</param>
        public void UnsubscribeFromComponentPlacementRecordChanges(SyncComponentPlacementRecordsCallback callback)
        {
            SyncComponentPlacementRecordsNotifier -= callback;
        }

        /// <summary>
        /// Add a record to the API.
        /// </summary>
        /// <param name="record">The record to add.</param>
        /// <returns></returns>
        public async Task AddComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Add the record to the database with the API.
            await _HttpClient.PutAsJsonAsync($"http://localhost:5000/api/ComponentPlacementRecord", record);
            // Refresh data.
            await RefreshComponentPlacementRecords();
        }

        /// <summary>
        /// Update a record in the database.
        /// </summary>
        /// <param name="record">The record to update. ID must match an existing ID.</param>
        /// <returns></returns>
        public async Task UpdateComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Write the record data to the database with the API.
            await _HttpClient.PostAsJsonAsync($"http://localhost:5000/api/ComponentPlacementRecord", record);
            // Refresh data.
            await RefreshComponentPlacementRecords();
        }

        /// <summary>
        /// Given a record ID delete it from the database.
        /// </summary>
        /// <param name="record">The record to delete.</param>
        /// <returns></returns>
        public async Task DeleteComponentPlacementRecord(ComponentPlacementRecord record)
        {
            // Delete the record from the database.
            await _HttpClient.DeleteAsync($"http://localhost:5000/api/ComponentPlacementRecord/{record.ID}");
            // Refresh data.
            await RefreshComponentPlacementRecords();
        }

        /// <summary>
        /// Return a reference to the list of ComponentPlacementRecords.
        /// </summary>
        /// <returns></returns>
        public List<ComponentPlacementRecord> GetComponentPlacementRecords()
        {
            return _records;
        }

        /// <summary>
        /// Returns the record with the given ID.
        /// </summary>
        /// <param name="recordID">The ID of the record to retrieve.</param>
        /// <returns></returns>
        public ComponentPlacementRecord? GetComponentPlacementRecord(Guid recordID)
        {
            return _records.FirstOrDefault(x => x.ID == recordID);
        }
    }
}