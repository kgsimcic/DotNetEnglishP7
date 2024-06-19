using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface ICurvePointService
    {
        CurvePoint[] GetAllCurvePoints();
        CurvePoint GetCurvePoint(int id);
        void CreateCurvePoint(CurvePoint curvePoint);
        void DeleteCurvePoint(int id);
        void UpdateCurvePoint(CurvePoint curvePoint);
    }
}
