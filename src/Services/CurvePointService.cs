using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

namespace WebApi.Services
{
    public class CurvePointService : ICurvePointService
    {

        private readonly CurvePointRepository _curvePointRepository;
        public CurvePointService(CurvePointRepository curvePointRepository) {
            _curvePointRepository = curvePointRepository;
        }

        public CurvePoint[] GetAllCurvePoints()
        {
            return _curvePointRepository.FindAll();
        }

        public CurvePoint GetCurvePoint(int id)
        {
            return _curvePointRepository.FindById(id);
        }

        public void CreateCurvePoint(CurvePoint curvePoint)
        {
            _curvePointRepository.Add(curvePoint);
        }

        public void DeleteCurvePoint(int id)
        {
            _curvePointRepository.Delete(id);
        }

        public void UpdateCurvePoint(CurvePoint curvePoint)
        {
            _curvePointRepository.Update(curvePoint);
        }
    }
}
