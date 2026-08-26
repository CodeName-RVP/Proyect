using GeneradorPlantillas.Models;

namespace GeneradorPlantillas.Services;

public class ExcelValidator
{
    public ValidationResult Validar(List<Invitado> invitados)
    {
        var resultado = new ValidationResult();

        if (invitados.Count == 0)
        {
            resultado.Errores.Add(
                "El archivo no contiene ningún invitado. Por favor, asegúrese de que el archivo tenga datos."
            );

            return resultado;
        }

        var idsEncontrados = new Dictionary<string, int>(
            StringComparer.OrdinalIgnoreCase
        );

        for (int i = 0; i < invitados.Count; i++)
        {
            var invitado = invitados[i];
            var filaExcel = i + 2; // +2 porque la primera fila es el encabezado y la lista es 0-indexada

            if (string.IsNullOrWhiteSpace(invitado.IdInvitado))
            {
                resultado.Errores.Add(
                    $"Fila {filaExcel}: El campo 'IdInvitado' está vacío. Por favor, complete este campo."
                );
            }
            else
            {
                if (idsEncontrados.TryGetValue(
                    invitado.IdInvitado,
                    out int primeraFila))
                {
                    resultado.Errores.Add(
                        $"Fila {filaExcel}: El 'IdInvitado' '{invitado.IdInvitado}' ya fue utilizado en la fila {primeraFila}. Por favor, use un 'IdInvitado' único."
                    );
                }
                else
                {
                    idsEncontrados.Add(
                        invitado.IdInvitado, 
                        filaExcel
                    );
                }
            }
            
            if (string.IsNullOrWhiteSpace(invitado.Nombre))
            {
                resultado.Errores.Add(
                    $"Fila {filaExcel}: El campo 'Nombre' está vacío. Por favor, complete este campo."
                );
            }
        }

        return resultado;
    }

}