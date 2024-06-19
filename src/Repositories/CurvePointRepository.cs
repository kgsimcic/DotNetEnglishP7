using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository
    {
        public LocalDbContext DbContext { get; }

        public CurvePointRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public CurvePoint FindById(int id)
        {
            return DbContext.CurvePoints.Where(curvePoint => curvePoint.Id == id)
                                  .FirstOrDefault();
        }

        public CurvePoint[] FindAll()
        {
            return DbContext.CurvePoints.ToArray();
        }

        public void Add(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Add(curvePoint);
        }

        public void Update(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Update(curvePoint);
        }

        public void Delete(int id) {
            var curvePointToDelete = DbContext.CurvePoints.Where(curvePoint => curvePoint.Id == id).FirstOrDefault();
            if (curvePointToDelete != null)
            {
                DbContext.CurvePoints.Remove(curvePointToDelete);
            }
        }
    }
}