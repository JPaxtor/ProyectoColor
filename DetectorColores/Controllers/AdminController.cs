using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MonitorUsuarios.Hubs;

namespace DetectorColores.Controllers;

// Usamos solo [Route] para manejar tanto la vista como la API
[Route("[controller]")] 
public class AdminController : Controller // <--- IMPORTANTE: Cambiar a Controller
{
    private readonly IHubContext<AdminHub> _hubContext;

    public AdminController(IHubContext<AdminHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // --- FALTA ESTO: El método para cargar la página ---
    [HttpGet] // URL: http://localhost:5077/Admin
    public IActionResult Admin()
    {
        return View();
    }

    // Método para el ESP32
    [HttpPost("update")] // URL: http://localhost:5077/Admin/update
    public async Task<IActionResult> UpdateColor([FromBody] ColorModel model)
    {
        // Redirigir toda la telemetría al Hub
        await _hubContext.Clients.All.SendAsync("ReceiveProcessData", model);
        return Ok(new { status = "Procesando Moda" });
    }
}

// Clase para recibir el JSON del ESP32
public class ColorModel
{
    public string Estado { get; set; } // EJECUCION, DETECTANDO, ENCONTRADO, UBICANDO
    public string ColorFinal { get; set; }
    
    // Arrays de 5 posiciones para las muestras
    public int[] R { get; set; }
    public int[] G { get; set; }
    public int[] B { get; set; }
    public int[] C { get; set; } // Canal Clear / Opacidad
    
    // El valor calculado (R+G+B o el identificador del color)
    public int[] Totales { get; set; } 
    public int ModaResultado { get; set; }
}