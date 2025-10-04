using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LasSharp;

namespace Kroft.Api.Entities
{
    public interface ILiDARDataRepository
    {
        Task<bool> ProcessLasFile(string areaName, LasReader lasData, string manualUTMZone = "", CancellationToken cancellationToken = default);
        
        Task<List<LiDARPoint>> QueryLiDARPoints(string areaName, PointFilter pointFilter, CancellationToken cancellationToken = default);
    }
}
