using SysProgDrugiDeo19461.Caching;
using SysProgDrugiDeo19461.Const;
using SysProgDrugiDeo19461.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProgDrugiDeo19461.WebServer
{
    public class RequestHandler
    {
        public static async Task HandleRequest(object? state)
        {
            HttpListenerContext? context = null;
            try
            {
                Logger.Log("Obradjivanje zahteva");

                if (state == null)
                    throw new Exception("Greska prilikom obrade");

                context = (HttpListenerContext)state;
                await Obrada(context);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                if (context != null)
                    await SendResponseAsync(context, $"{ex.Message}", true);
            }
        }

        public static async Task Obrada(HttpListenerContext context)
        {
            string nazivFajla = context?.Request?.Url?.AbsolutePath.TrimStart('/') ?? "";

            if (string.IsNullOrWhiteSpace(nazivFajla))
                throw new Exception("Naziv fajla je obavezan");

            Logger.Log($"Obradjivanje za fajl: {nazivFajla}");

            string odgovor = CacheManger.GetItem(nazivFajla);
            if (!string.IsNullOrWhiteSpace(odgovor))
            {
                await SendResponseAsync(context!, odgovor);
                Logger.Log($"Uspesno vracanje odgovora iz kesa: {odgovor}");
                return;
            }

            string fajl = FileManager.FileManager.FindFile(AppSettings.RootFolder, nazivFajla);

            if (string.IsNullOrWhiteSpace(fajl))
                throw new Exception("Trazeni fajl ne postoji.");

            int avg = FileManager.FileManager.FindAverageWordLength(fajl);
            if (avg == 0)
                throw new Exception("Fajl koji je poslat je prazan");

            CacheManger.SetItem(nazivFajla, avg.ToString());

            Logger.Log($"Uspesna obrada, slanje odgovora: {avg}");
            await SendResponseAsync(context!, $"{avg}");
        }

        private static async Task SendResponseAsync(HttpListenerContext context, string response, bool isError = false)
        {
            try
            {
                if (isError)
                    context.Response.StatusCode = (short)HttpStatusCode.BadRequest;

                byte[] buffer = Encoding.UTF8.GetBytes(response);
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex.Message}");
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }
    }
}
