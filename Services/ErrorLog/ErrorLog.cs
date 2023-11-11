using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace correoFei.Services.ErrorLog
{
    public class ErrorLog : IErrorLog
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IHttpContextAccessor _httpContextAcessor;
        public ErrorLog(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor){
            _webHostEnviroment = webHostEnvironment;
            _httpContextAcessor = httpContextAccessor;
        }
        [HttpPost]
        public async Task ErrorLogAsync(string Mensaje)
        {
            try{
                string webRootPath = _webHostEnviroment.WebRootPath;
                string path = "";
                path = Path.Combine(webRootPath,"log");
                if(!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                }
                var feature = _httpContextAcessor.HttpContext.Features.Get<IHttpConnectionFeature>();
                string LocalIPAddr = feature?.LocalIpAddress?.ToString();
                using(StreamWriter outputFile = new StreamWriter(Path.Combine(path,"Log.txt"),true)){
                    await outputFile.WriteLineAsync(Mensaje +" - " + _httpContextAcessor.HttpContext.User.Identity.Name +
                    " - " + LocalIPAddr + " - " + DateTime.Now.ToString());
                }
            }catch {
                
            }
        }
    }
}