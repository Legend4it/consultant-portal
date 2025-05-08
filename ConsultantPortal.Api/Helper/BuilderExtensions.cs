using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantPortal.Api.Helper;

public static class BuilderExtensions
{
    public static WebApplicationBuilder BuildServices(
        this WebApplicationBuilder builder,
        Action<IServiceCollection> configure)
    {
        configure(builder.Services);
        return builder;
    }
}
