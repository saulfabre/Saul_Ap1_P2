using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Context;
using Saul_Ap1_P2.Models;

namespace Saul_Ap1_P2.Services;

public class ProductosServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Productos, int>
{
    public async Task<Result<Productos?>> Buscar(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Productos
            .FirstOrDefaultAsync(p => p.ProductoId == productoId);
    }

    public async Task<Result<List<Productos>>> GetList(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Productos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Productos
            .AnyAsync(p => p.ProductoId == productoId);
    }

    public async Task<bool> Insertar(Productos producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Productos.Add(producto);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Productos producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Update(producto);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Productos
            .Where(p => p.ProductoId == productoId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }

    public async Task<Result> Guardar(Productos producto)
    {
        if (!await Existe(producto.ProductoId))
        {
            return Result.Success(await Insertar(producto));
        }
        else
        {
            return Result.Success(await Modificar(producto));
        }
    }
}
