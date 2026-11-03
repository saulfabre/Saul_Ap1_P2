using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Context;
using Saul_Ap1_P2.Models;

namespace Saul_Ap1_P2.Services;

public class PrestamosServices(IDbContextFactory<Contexto> contextFactory
    ) : IService<Modelo, int>
{
    public Task<Modelo?> Buscar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Modelo>> GetList(Expression<Func<Modelo, bool>> criterio)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Guardar(Modelo entidad)
    {
        throw new NotImplementedException();
    }
}
