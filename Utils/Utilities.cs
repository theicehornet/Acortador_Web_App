using Acortador_Web_App.Models;
using QRCoder;

namespace Acortador_Web_App.Utils
{
    public class Utilities
    {
        public static string CrearQr(string text)
        {
            var qrgenerator = new QRCodeGenerator();
            var qrCodeData = qrgenerator.CreateQrCode(text,QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode bitmapbytecode = new BitmapByteQRCode(qrCodeData);
            var bitmap = bitmapbytecode.GetGraphic(20);
            
            using var ms = new MemoryStream();
            ms.Write(bitmap);
            byte[] byteimage = ms.ToArray();
            return Convert.ToBase64String(byteimage);
        }

        public static Acortador CrearAcortador(string text)
        {
            Acortador ac = new Acortador();
            ac.Id = IdAcortador();
            ac.Link = text;
            return ac;
        }

        public static string IdAcortador()
        {
            while (true)
            {
                string link = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                if (!link.Contains('/'))
                    return link;
            }
        }

        public static bool LinkExiste(string link,List<Acortador> acortadores)
        {
            foreach(Acortador ac in acortadores)
            {
                if(ac.Link == link) return true;
            }
            throw new Exception("Link no registrado");
        }

        public static Acortador HallarAcortador(string link, List<Acortador> acortadores)
        {
            foreach (Acortador ac in acortadores)
            {
                if (ac.Link == link) return ac;
            }
            throw new Exception("No encontrado");
        }

    }
}
