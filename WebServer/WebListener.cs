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
    public class WebListener
    {
        private static HttpListener listener = new HttpListener();
        public async Task ListenAsync()
        {
            listener.Prefixes.Add(AppSettings.ListenerUrl);
            listener.Start();
            Logger.Log($"Server slusa na adresi: {AppSettings.ListenerUrl}");

            while (true)
            {
                var context = await listener.GetContextAsync();
                await Task.Run(() => RequestHandler.HandleRequest(context));
            }
        }
    }
}
