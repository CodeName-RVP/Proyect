using ClosedXML.Excel;
using GeneradorPlantillas.Models;

namespace GeneradorPlantillas.Services;

public class ExcelService
{
    public List<Invitado> LeerInvitados(string filePath)
    {
        var invitados = new List<Invitado>();

        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheets.Worksheet(1); // Asumiendo que los datos están en la primera hoja

        // Buscar la primera fila usada para determinar dónde comienzan los encabezados
        var primeraFila = worksheet.FirstRowUsed()?.RowNumber();
        
        if (primeraFila == null)
        {
            // La hoja está vacía
            throw new InvalidDataException(
                "La hoja de Excel está vacía."
            );
        }

        int filaEncabezados = primeraFila.Value;

        var columnas = new Dictionary<string, int>(
            StringComparer.OrdinalIgnoreCase
        );

        foreach (var celda in worksheet.Row(filaEncabezados).CellsUsed())
        {
            var encabezado = celda.GetString().Trim();

            if (!string.IsNullOrEmpty(encabezado))
            {
                columnas[encabezado] = celda.Address.ColumnNumber;
            }
        }

        var columnasRequeridas = new[]
        {
            "IdInvitado",
            "Nombre",
            "LiderEquipo"
        };

        foreach (var columna in columnasRequeridas)
        {
            if (!columnas.ContainsKey(columna))
            {
                throw new InvalidDataException(
                    $"La columna requerida '{columna}' no se encuentra en el archivo de Excel."
                );
            }
        }

        int columnaId = columnas["IdInvitado"];
        int columnaNombre = columnas["Nombre"];
        int columnaLiderEquipo = columnas["LiderEquipo"];

        var ultimaFila = worksheet.LastRowUsed();

        if (ultimaFila == null)
        {
            return invitados; // La hoja solo tiene encabezados, no hay datos
        }

        int ultimaFilaNumero = ultimaFila.RowNumber();
        
        // Iterar desde la fila después de los encabezados hasta la última fila usada
        
        for (int fila = filaEncabezados + 1; fila <= ultimaFilaNumero; fila++)
        {
            var invitado = new Invitado
            {
                IdInvitado = worksheet.Cell(fila, columnaId)
                    .GetString().Trim(),

                Nombre = worksheet.Cell(fila, columnaNombre)
                    .GetString().Trim(),

                LiderEquipo = worksheet.Cell(fila, columnaLiderEquipo)
                    .GetString().Trim()
            };

            invitados.Add(invitado);
        }

        return invitados;
    }
}