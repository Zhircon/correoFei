using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using correoFei.Models;
using correoFei.Services.Email;
using correoFei.Services.ErrorLog;
namespace correoFei.Controllers;

public class HomeController : Controller
{
    private readonly IEmail _email;
    private readonly IErrorLog _errorlog;
    public HomeController(IErrorLog errorLog, IEmail email){
        _errorlog = errorLog;
        _email = email;
    }
    public IActionResult Index(){
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync(ContactoViewModel contacto){
        if(ModelState.IsValid)
        {
            try
            {
                await _email.EnviaCorreoAsync("Correo electronico desde FEI",contacto.Correo,null,null,CuerpoCorreo(contacto.Nombre));
                return RedirectToAction(nameof(Success));
            }
            catch(Exception ex)
            {
                await _errorlog.ErrorLogAsync(ex.Message);
            }
        }
        ModelState.AddModelError("","No ha sido posible enviar el correo. Inténtelo nuevamente.");
        return View();
    }
    public string CuerpoCorreo(string nombre){
        string Mensaje = $"<p> Estimado/Estimada usuario, {nombre}</p>";
        Mensaje += "<p>Por este medio le informamos que su participacion ha sido recibida</p>";
        Mensaje += "<p>Agradecemos su participacion.</p>";
        Mensaje += "<br /><br /><div>-------------------------------</div>";
        Mensaje += "<div>Mensaje enviado automaticamente.Favor de no responder al remitente de este mensaje.</div>";
        return Mensaje;
    }
    public IActionResult Privacy(){
        return View();
    }
    [ResponseCache(Duration= 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    public IActionResult Success()
    {
        return View();
    }
}
