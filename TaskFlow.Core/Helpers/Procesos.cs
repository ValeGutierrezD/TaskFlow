using System.Globalization;

namespace TaskFlow.Core.Helpers
{
    public static class Procesos
    {
        public static string? ParseFechaFlexible(string? fechaTexto)
        {
            if (fechaTexto is null) return null;
            fechaTexto = fechaTexto.Replace("a. m.", "AM").Replace("p. m.", "PM").Trim();
            string[] formatos = {
                "d-M-yyyy", "dd-MM-yyyy",
                "d-M-yyyy h:mm:ss tt", "d-M-yyyy H:mm:ss", "d-M-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy h:mm:ss tt",
                "M-d-yyyy", "MM-dd-yyyy",
                "M-d-yyyy h:mm:ss tt", "M-d-yyyy H:mm:ss", "M-d-yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss", "MM-dd-yyyy h:mm:ss tt"
            };
            var cultura = new CultureInfo("es-BO");
            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(fechaTexto, formato, cultura, DateTimeStyles.None, out DateTime resultado))
                    return resultado.ToString("dd/MM/yyyy");
            }
            return null;
        }
    }
}
