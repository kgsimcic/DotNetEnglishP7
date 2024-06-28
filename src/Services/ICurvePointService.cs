using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface ICurvePointService
    {
        CurvePoint[] GetAllCurvePoints();
        Task<CurvePoint> GetCurvePoint(int id);
        Task<CurvePoint> CreateCurvePoint(CurvePoint curvePoint);
        Task<int> DeleteCurvePoint(int id);
        Task<int> UpdateCurvePoint(CurvePoint curvePoint);
    }
}
