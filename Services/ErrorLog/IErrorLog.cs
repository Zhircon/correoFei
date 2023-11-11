namespace correoFei.Services.ErrorLog
{
    public interface IErrorLog
    {
        public Task ErrorLogAsync(string Mensaje);
    }
}