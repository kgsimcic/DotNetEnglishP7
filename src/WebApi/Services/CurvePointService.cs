using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        protected IRepository<CurvePoint> _curvePointRepository { get; }
        public CurvePointService(IRepository<CurvePoint> curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public CurvePoint[] GetAllCurvePoints()
        {
            return _curvePointRepository.GetAll();
        }
# nullable enable
        public CurvePoint? GetCurvePoint(int id)
        {
            return _curvePointRepository.GetById(id);
        }
# nullable disable

        public async Task<Result> CreateCurvePoint(CurvePoint curvePoint)
        {
            var validationResult = ValidateCurvePoint(curvePoint);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _curvePointRepository.Add(curvePoint);
            await _curvePointRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<int> DeleteCurvePoint(int id)
        {
            var existingCurvePoint = _curvePointRepository.GetById(id);
            if (existingCurvePoint == null)
            {
                throw new KeyNotFoundException("Curve point not found.");
            }

            _curvePointRepository.Delete(existingCurvePoint);
            return await _curvePointRepository.SaveChangesAsync();
        }

        public async Task<Result> UpdateCurvePoint(int id, CurvePoint curvePoint)
        {
            var existingCurvePoint = _curvePointRepository.GetById(id);
            if (existingCurvePoint == null)
            {
                throw new KeyNotFoundException("Curve point not found.");
            }

            var validationResult = ValidateCurvePoint(curvePoint);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _curvePointRepository.Update(curvePoint);
            await _curvePointRepository.SaveChangesAsync();
            return validationResult;
        }
    }
}
