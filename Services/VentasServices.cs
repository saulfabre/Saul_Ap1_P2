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

        foreach (var detalle in venta.Detalles)
        {
            var producto = await contexto.Productos
                .SingleAsync(p => p.ProductoId == detalle.ProductoId);

            producto.Stock -= detalle.Cantidad;
        }

        contexto.Ventas.Add(venta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Ventas venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var existeVenta = await contexto.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.VentaId == venta.VentaId);

        if (existeVenta == null)
        {
            return false;
        }

        foreach (var detalle in existeVenta.Detalles.ToList())
        {

            var producto = await contexto.Productos
                .SingleAsync(p => p.ProductoId == detalle.ProductoId);

            producto.Stock += detalle.Cantidad;

            contexto.VentasDetalles.Remove(detalle);
        }

        foreach (var detalle in venta.Detalles)
        {
            var producto = await contexto.Productos
                .SingleAsync(p => p.ProductoId == detalle.ProductoId);

            if (producto.Stock < detalle.Cantidad)
                return false;

            producto.Stock -= detalle.Cantidad;

            contexto.VentasDetalles.Add(new VentasDetalles
            {
                VentaId = venta.VentaId,
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            });
        }

        existeVenta.Fecha = venta.Fecha;
        existeVenta.Observacion = venta.Observacion;
        existeVenta.Total = venta.Detalles.Sum(d => d.Cantidad * d.Precio);

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

        foreach (var item in detalles)
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

        await contexto.SaveChangesAsync();
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
