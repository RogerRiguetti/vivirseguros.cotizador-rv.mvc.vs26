using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Estudio.Api.Startup))]

namespace Estudio.Api
{
    public class Startup
    {
        // Este método es llamado por OWIN al iniciar la aplicación
        public void Configuration(IAppBuilder app)
        {
            // Configuraciones OWIN básicas pueden colocarse aquí.
            // Ejemplo: configurar autenticación, middleware, SignalR, etc.
            // Si existe un método ConfigureAuth, descomentar la línea siguiente:
            // ConfigureAuth(app);
        }
    }
}
