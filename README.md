# CHANGELOG — Sesión 17/09/2026 [Unreleased] 
# — QR & PDF Generation Pipeline

## Features
* Generación de códigos QR
* Integración de QRCoder.
* Generación de códigos QR únicos a partir del ID_Invitado.
* Uso de ECCLevel.Q para mayor tolerancia a daños.
* Generación individual y masiva de QR.
* Exportación de QR como imágenes PNG durante las pruebas.
* Generación de documentos PDF
* Integración de QuestPDF.
* Generación de PDFs individuales para invitados.
* Generación de planillas agrupadas por líder.
* Inclusión automática del nombre del invitado y su código QR.
* Configuración inicial de documentos en formato A4.
* Soporte para múltiples invitados dentro de una misma planilla.
* Agrupación de invitados
* Implementación de agrupación automática por LiderEquipo.
* Soporte para invitados sin líder mediante el grupo SIN_LIDER.
* Conservación de la información y estado de cada invitado.
* Generación masiva de planillas
* Implementación de PlanillaService.
* Generación automática de una planilla PDF por cada líder.
* Escritura de las planillas generadas en el directorio de pruebas.
* Verificación exitosa con múltiples líderes.
  
## Architecture
* Implementación de IInvitadoRepository para desacoplar la lógica de negocio de la fuente de datos.
* Implementación de DatabaseService como repositorio temporal en memoria.
* Registro de servicios mediante Dependency Injection.

### Incorporación de:
* QrService
* PdfService
* InvitadoService
* PlanillaService

### Separación de responsabilidades entre:
* lectura de datos
* validación
* agrupación
* generación de QR
* generación de PDF
* generación de planillas

### Testing
> Se implementaron endpoints temporales para comprobar individualmente cada componente:

```Text
/test-excel
/test-db
/test-db/{id}
/test-db/{id}/registrar
/test-qr/{id}
/test-qrs
/test-qrs-files
/test-agrupar
/test-pdf/{id}
/test-pdf-lider/{lider}
/test-planillas
```

## Validaciones realizadas:

* ✅ Generación individual de QR.
* ✅ Generación masiva de QR.
* ✅ Lectura correcta de QR mediante teléfono.
* ✅ Generación de PDF individual.
* ✅ Agrupación de invitados por líder.
* ✅ Generación de planilla individual por líder.
* ✅ Generación automática de múltiples planillas.
* ✅ Generación exitosa de las planillas de:

  * María López
  * Pedro Sánchez

## Dependencies
Se incorporaron:

* QRCoder
* QuestPDF

## Checkpoint
> Estado actual del sistema
```text
                    Excel
                      │
                      ▼
                ┌───────────┐
                │ Validación│
                └─────┬─────┘
                      │
                      ▼
                ┌───────────┐
                │ Invitados │
                └─────┬─────┘
                      │
                      ▼
                ┌───────────┐
                │ Agrupación│
                │ por líder │
                └─────┬─────┘
                      │
             ┌────────┴────────┐
             ▼                 ▼
       Generar QR        Generar grupos
             │                 │
             └────────┬────────┘
                      ▼
                Generar PDF
                      │
                      ▼
             Planillas por líder
```

# Funcionalidades completadas
 * ✅ Modelo de invitado
 * ✅ Validación de Excel
 * ✅ Repositorio de invitados
 * ✅ Estados PENDIENTE / REGISTRADO
 * ✅ Registro de asistencia simulado
 * ✅ Generación de QR
 * ✅ Agrupación por líder
 * ✅ Generación de PDF
 * ✅ Generación de planillas por líder
 * ✅ Generación masiva de planillas
 * ✅ Dependency Injection
 * ✅ OpenAPI
 * ✅ Pruebas funcionales de cada módulo

## Próximo bloque

```text
Excel
  ↓
Importación real
  ↓
Validación
  ↓
Procesamiento
  ↓
Generación de planillas
  ↓
API
  ↓
Interfaz web
```

## Después:

```text
MySQL real
   ↓
WhatsApp API
   ↓
Lectura de QR
   ↓
Validación de imagen
   ↓
Registro de asistencia
```
