using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface ICurvePointService
    {
        CurvePoint[] GetAllCurvePoints();
# nullable enable
        CurvePoint? GetCurvePoint(int id);
# nullable disable
        Task<int> CreateCurvePoint(CurvePoint curvePoint);
        Task<int> DeleteCurvePoint(int id);
        Task<int> UpdateCurvePoint(int id, CurvePoint curvePoint);
    }
}
