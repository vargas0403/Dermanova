using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class TrabajadoresDTOS/* : IValidatableObject*/
{
    public int TrabajadoresId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [RegularExpression(@"^\d{3}-\d{6}-\d{4}[A-Z]$", ErrorMessage = "La cédula no tiene un formato válido.")]
    public string? Cedula { get; set; }

    [Phone(ErrorMessage = "El número de celular no es válido.")]
    public string? Celular { get; set; }  // Cambiado a string para mayor flexibilidad

    // Llave foránea al ID de IdentityUser
    public string? UserId { get; set; }

    [Required(ErrorMessage = "El puesto es obligatorio.")]
    [StringLength(50, ErrorMessage = "El puesto no puede exceder los 50 caracteres.")]
    public string? Puesto { get; set; }

    public bool? State { get; set; }

    //// Implementación de la validación personalizada
    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    var validationResults = new List<ValidationResult>();

    //    // Validar la cédula con el validador personalizado
    //    if (!string.IsNullOrWhiteSpace(Cedula) && !CedulaValidator.ValidarCedula(Cedula))
    //    {
    //        validationResults.Add(new ValidationResult(
    //            "La cédula proporcionada no es válida.", new[] { nameof(Cedula) }));
    //    }

    //    return validationResults;
    //}
}

//public class CedulaValidator
//{
//    public static bool ValidarCedula(string cedula)
//    {
//        // Expresión regular para el formato 075-050408-1007K
//        string pattern = @"^\d{3}-\d{6}-\d{4}[A-Z]$";
//        Regex regex = new Regex(pattern);

//        if (!regex.IsMatch(cedula))
//        {
//            // Si el formato no coincide, devolver falso
//            return false;
//        }

//        // Separar las partes de la cédula
//        string[] partes = cedula.Split('-');
//        string region = partes[0]; // Parte de la región
//        string fecha = partes[1];  // Parte de la fecha
//        string serie = partes[2].Substring(0, 4); // Parte del número de serie
//        char letraVerificacion = partes[2][4];    // Letra de verificación

//        // Validar que la región tenga sentido (ejemplo: puede ser entre 001 y 999)
//        int numeroRegion;
//        if (!int.TryParse(region, out numeroRegion) || numeroRegion < 1 || numeroRegion > 999)
//        {
//            return false;
//        }

//        // Validar que la fecha tenga sentido (ejemplo: fecha válida)
//        if (!EsFechaValida(fecha))
//        {
//            return false;
//        }

//        // Aquí podrías agregar más validaciones, como la letra de verificación
//        // Si todo es válido, devolver verdadero
//        return true;
//    }

//    private static bool EsFechaValida(string fecha)
//    {
//        // La fecha está en formato ddMMyy
//        if (fecha.Length != 6)
//        {
//            return false;
//        }

//        int dia = int.Parse(fecha.Substring(0, 2));
//        int mes = int.Parse(fecha.Substring(2, 2));
//        int año = int.Parse(fecha.Substring(4, 2)) + 1900; // Ajuste de año

//        // Verificar si la fecha es válida
//        try
//        {
//            DateTime fechaNacimiento = new DateTime(año, mes, dia);
//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}
