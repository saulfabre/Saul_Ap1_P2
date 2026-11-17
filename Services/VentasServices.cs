using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Context;
using Saul_Ap1_P2.Models;

namespace Saul_Ap1_P2.Services;

public class VentasServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Ventas, int>
{
    public async Task<Result<Ventas?>> Buscar(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Ventas
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Productos)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId);
    }

    public async Task<bool> Existe(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Ventas
            .AnyAsync(v => v.VentaId == ventaId);
    }

    public async Task<Result<List<Ventas>>> GetList(Expression<Func<Ventas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Ventas
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Insertar(Ventas venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Ventas.Add(venta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Ventas venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Update(venta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var venta = await contexto.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId);

        if (venta == null)
        {
            return Result.Failure(new Error("VentaNoEncontrada", "La venta no existe."));
        }

        await AfectarProductos(venta.Detalles.ToArray(), false);

        contexto.VentasDetalles.RemoveRange(venta.Detalles);

        contexto.Ventas.Remove(venta);

        var eliminadoResult = await contexto.SaveChangesAsync() > 0;

        return Result.Success(eliminadoResult);
    }

    public async Task AfectarProductos(VentasDetalles[] detalles, bool op)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        foreach(var item in detalles)
        {
            var producto = await contexto.Productos.SingleAsync(p => p.ProductoId == item.ProductoId);

            if (op)
            {
                producto.Stock -= item.Cantidad;
            }
            else
            {
                producto.Stock += item.Cantidad;
            }
        }
    }

    public async Task<Result> Guardar(Ventas venta)
    {
        if (!await Existe(venta.VentaId))
        {
            return Result.Success(await Insertar(venta));
        }
        else
        {
            return Result.Success(await Modificar(venta));
        }
    }
}
