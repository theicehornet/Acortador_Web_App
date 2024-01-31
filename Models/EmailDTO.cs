namespace Acortador_Web_App.Models
{
    public class EmailDTO
    {
        public string Para { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;

        public EmailDTO() { }

        public EmailDTO(string para,string asunto,string contenido) 
        {
            Para = para;
            Asunto = asunto;
            Contenido = contenido;
        }

    }
}
