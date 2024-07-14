using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface ICurvePointService
    {
        Task<CurvePoint[]> GetAllCurvePoints();
# nullable enable
        Task<CurvePoint?> GetCurvePoint(int id);
# nullable disable
        Task<Result> CreateCurvePoint(CurvePoint curvePoint);
        Task<int> DeleteCurvePoint(int id);
        Task<Result> UpdateCurvePoint(int id, CurvePoint curvePoint);
    }
}
