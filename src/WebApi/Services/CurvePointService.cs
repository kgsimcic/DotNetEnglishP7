using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<CurvePoint> GetCurvePoint(int id)
        {
            return await _curvePointRepository.FindById(id);
        }

        public async Task<int> CreateCurvePoint(CurvePoint curvePoint)
        {
            return await _curvePointRepository.Create(curvePoint);
        }

        public async Task<int> DeleteCurvePoint(int id)
        {
            var existingCurvePoint = _curvePointRepository.FindById(id);
            if (existingCurvePoint == null)
            {
                throw new KeyNotFoundException("Curve point not found.");
            }

            return await _curvePointRepository.Delete(id);
        }

        public async Task<int> UpdateCurvePoint(int id, CurvePoint curvePoint)
        {
            var existingCurvePoint = _curvePointRepository.FindById(id);
            if (existingCurvePoint == null)
            {
                throw new KeyNotFoundException("Curve point not found.");
            }

            return await _curvePointRepository.Update(curvePoint);
        }
    }
}
