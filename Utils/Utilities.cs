﻿using Acortador_Web_App.Models;
using QRCoder;
using System.Security.Cryptography;
using System.Text;

namespace Acortador_Web_App.Utils
{
    public class Utilities
    {
        public static string CrearQr(string text)
        {
            var qrgenerator = new QRCodeGenerator();
            var qrCodeData = qrgenerator.CreateQrCode(text,QRCodeGenerator.ECCLevel.L);
            BitmapByteQRCode bitmapbytecode = new BitmapByteQRCode(qrCodeData);
            var bitmap = bitmapbytecode.GetGraphic(20);
            
            using var ms = new MemoryStream();
            ms.Write(bitmap);
            byte[] byteimage = ms.ToArray();
            return Convert.ToBase64String(byteimage);
        }

        public static Acortador CrearAcortador(string link)
        {
            Acortador ac = new Acortador
            {
                Id = IdAcortador(),
                Link = link,
                Lasttime = null
            };
            return ac;
        }

        public static string IdAcortador()
        {
            while (true)
            {
                string link = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                if (!link.Contains('/'))
                    return link[..10];
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

        internal static bool IsURL(string url)
        {
            return url.StartsWith("https://") || url.StartsWith("http://");
        }

        public static string EncodePassword(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));
                foreach(byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
