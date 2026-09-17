using GeneradorPlantillas.Services;
using GeneradorPlantillas.Repositories;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Evaluation;

builder.Services.AddSingleton<IInvitadoRepository, DatabaseService>();
builder.Services.AddSingleton<QrService>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddSingleton<InvitadoService>();
builder.Services.AddSingleton<PlanillaService>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/test-excel", () =>
{
    var excelService = new ExcelService();
    var excelValidator = new ExcelValidator();
    
    var rutaExcel = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TestData",
        "invitados_prueba_error.xlsx"
    );

    var invitados = excelService.LeerInvitados(rutaExcel);

    var resultadoValidacion = excelValidator.Validar(invitados);

    return Results.Ok(new 
    { 
        Invitados = invitados, 
        Validacion = resultadoValidacion 
    });
    
});

app.MapGet("/test-db", (IInvitadoRepository databaseService) =>
{
    var invitados = databaseService.ObtenerInvitados();

    return Results.Ok(invitados);
});

app.MapGet("/test-db/{id}", (string id, IInvitadoRepository databaseService) =>
{
    var invitado = databaseService.ObtenerInvitadoPorId(id);

    if (invitado == null)
    {
        return Results.NotFound(new
        {
            Mensaje = $"No se encontró el invitado con el ID '{id}'"
        });
    }
    
    return Results.Ok(invitado);

});

app.MapPost("/test-db/{id}/registrar", (string id, IInvitadoRepository databaseService) =>
{
    var invitado = databaseService.ObtenerInvitadoPorId(id);

    if (invitado == null)
    {
        return Results.NotFound(new
        {
            Mensaje = $"No se encontró el invitado con el ID '{id}'"
        });
    }

    if (invitado.Estado == "REGISTRADO")
    {
        return Results.Conflict(new
        {
            Mensaje = $"El invitado '{invitado.Nombre}' ya fue registrado.",
            FechaRegistro = invitado.FechaRegistro
        });
    }

    databaseService.RegistrarInvitado(id);

    return Results.Ok(new
    {
        Mensaje = $"El invitado '{invitado.Nombre}' ha sido registrado exitosamente.",
        FechaRegistro = invitado.FechaRegistro,
        Invitado = invitado.Nombre,
        ID = invitado.IdInvitado
    });

});

app.MapGet("/test-qr/{id}", (string id, IInvitadoRepository databaseService, QrService qrService) =>
{
    var invitado = databaseService.ObtenerInvitadoPorId(id);

    if (invitado == null)
    {
        return Results.NotFound(new
        {
            Mensaje = $"No se encontró el invitado con el ID '{id}'."
        });
    }

    var qr = qrService.GenerarQr(invitado);

    return Results.File(
        qr,
        "image/png",
        $"QR_{invitado.IdInvitado}.png"
    );
});

app.MapGet("/test-qrs", (IInvitadoRepository databaseService, QrService qrService) =>
{
    var invitados = databaseService.ObtenerInvitados();
    var qrs = qrService.GenerarQrs(invitados);

    return Results.Ok(new
    {
        Cantidad = qrs.Count,
        IDs = qrs.Keys
    });
});

app.MapGet("/test-qrs-files", (IInvitadoRepository databaseService, QrService qrService) =>
{
    var invitados = databaseService.ObtenerInvitados();

    var qrs = qrService.GenerarQrs(invitados);

    var carpeta = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TestData",
        "QRs"
    );

    Directory.CreateDirectory(carpeta);

    foreach (var qr in qrs)
    {
        var nombreArchivo = $"QR_{qr.Key}.png";
        var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

        File.WriteAllBytes(rutaArchivo, qr.Value);
    }

    return Results.Ok(new
    {
        Mensaje = "QR generados correctamente.",
        Cantidad = qrs.Count,
        Carpeta = carpeta
    });
});

app.MapGet("/test-pdf/{id}", (
    string id,
    IInvitadoRepository databaseService,
    QrService qrService,
    PdfService pdfService) =>
{
    var invitado = databaseService.ObtenerInvitadoPorId(id);

    if (invitado == null)
    {
        return Results.NotFound(new
        {
            Mensaje = $"No se encontró el invitado con el ID '{id}'."
        });
    }

    var qr = qrService.GenerarQr(invitado);

    var pdf = pdfService.GenerarPdfPrueba(
        invitado,
        qr
    );

    return Results.File(
        pdf,
        "application/pdf",
        $"Planilla_{invitado.IdInvitado}.pdf"
    );
});

app.MapGet("/test-agrupar", (
    IInvitadoRepository databaseService,
    InvitadoService invitadoService) =>
{
    var invitados = databaseService.ObtenerInvitados();

    var grupos = invitadoService.AgruparPorLider(invitados);

    return Results.Ok(grupos);
});

app.MapGet("/test-pdf-lider/{lider}", (
    string lider,
    IInvitadoRepository databaseService,
    InvitadoService invitadoService,
    PdfService pdfService,
    QrService qrService) =>
{
    var invitados = databaseService.ObtenerInvitados();

    var grupos = invitadoService.AgruparPorLider(invitados);

    if (!grupos.TryGetValue(lider, out var invitadosDelLider))
    {
        return Results.NotFound(new
        {
            Mensaje = $"No se encontró el líder '{lider}'."
        });
    }

    var pdf = pdfService.GenerarPdfLider(
        lider,
        invitadosDelLider,
        qrService
    );

    return Results.File(
        pdf,
        "application/pdf",
        $"Planilla_Lider_{lider}.pdf"
    );
});

app.MapGet("/test-planillas", (
    IInvitadoRepository databaseService,
    InvitadoService invitadoService,
    PdfService pdfService,
    QrService qrService,
    PlanillaService planillaService) =>
{
    var invitados = databaseService.ObtenerInvitados();

    var planillas = planillaService.GenerarPlanillas(
        invitados,
        invitadoService,
        pdfService,
        qrService
    );

    var carpeta = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TestData",
        "Planillas"
    );

    Directory.CreateDirectory(carpeta);

    foreach (var planilla in planillas)
    {
        var nombreArchivo = $"Planilla_Lider_{planilla.Key}.pdf";

        var rutaArchivo = Path.Combine(
            carpeta,
            nombreArchivo
        );

        File.WriteAllBytes(rutaArchivo, planilla.Value);
    }

    return Results.Ok(new
    {
        Mensaje = "Planillas generadas correctamente.",
        Cantidad = planillas.Count,
        Lideres = planillas.Keys,
        Carpeta = carpeta
    });
});

app.Run();