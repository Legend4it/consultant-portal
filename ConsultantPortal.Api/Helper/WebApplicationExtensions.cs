using ConsultantPortal.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantPortal.Api.Helper;

public static class WebApplicationExtensions
{
    public static WebApplication MapAllEndpoints(this WebApplication app)
    {
        app.MapTimeLogEndpoints();
        app.MapProjectEndpoints();
        app.MapClientEndpoints();
        app.MapGenerateSummaryEndpoint();
        return app;
    }
}
