using System.ComponentModel.DataAnnotations;

namespace Saul_Ap1_P2.Models;

public class VentasDetalles
{
    [Key]
    public int DetalleId { get; set; }

    public int VentaId { get; set; }
    public Ventas Ventas { get; set; } = null!;
    
    public int ProductoId { get; set; }
    public Productos Productos { get; set; } = null!;

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El precio del producto es obligatorio.")]
    [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }
}
