using System.ComponentModel.DataAnnotations;

namespace Saul_Ap1_P2.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }

    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "La observación de la venta es obligatoria.")]
    public string Observacion { get; set; } = null!;

    public decimal Total { get; set; }

    public ICollection<VentasDetalles> Detalles { get; set; } = new List<VentasDetalles>();
}
