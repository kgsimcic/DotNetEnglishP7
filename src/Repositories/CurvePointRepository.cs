using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository
    {
        public LocalDbContext DbContext { get; }

        public CurvePointRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<CurvePoint> FindById(int id)
        {
            return await DbContext.CurvePoints.ToAsyncEnumerable().Where(curvePoint => curvePoint.Id == id)
                                  .FirstOrDefault();
        }

        public CurvePoint[] FindAll()
        {
            return DbContext.CurvePoints.ToArray();
        }

        public async Task<CurvePoint> Add(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Add(curvePoint);
            await DbContext.SaveChangesAsync();
            return curvePoint;
        }

        public Task<int> Update(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Update(curvePoint);
            return DbContext.SaveChangesAsync();
        }

        public Task<int> Delete(int id) {
            var curvePointToDelete = DbContext.CurvePoints.Where(curvePoint => curvePoint.Id == id).FirstOrDefault();
            if (curvePointToDelete != null)
            {
                DbContext.CurvePoints.Remove(curvePointToDelete);
            }
            return DbContext.SaveChangesAsync();
        }
    }
}