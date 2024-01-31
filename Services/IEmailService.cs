using Acortador_Web_App.Models;
namespace Acortador_Web_App.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
