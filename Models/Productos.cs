using System.ComponentModel.DataAnnotations;

namespace Saul_Ap1_P2.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El precio del producto es obligatorio.")]
    [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock del producto es obligatorio.")]
    [Range(1, double.MaxValue, ErrorMessage = "El stock debe ser mayor que 0.")]
    public int Stock { get; set; }
}
