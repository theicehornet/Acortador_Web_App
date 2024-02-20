using Acortador_Web_App.Models;
namespace Acortador_Web_App.Services.Contract
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
