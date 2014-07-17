using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EU4.Stats.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            // On error, send back a user friendly message and a developer friendly message in JSON.
            // The Web UI is expecting "message" and "developer" but in C# we use capitals, we we get
            // the best of both worlds by customizing a serializer that will do the conversion for us
            var serializer = new JsonSerializer() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            pipelines.OnError.AddItemToEndOfPipeline((with, exception) =>
                    new JsonResponse<ErrorMessage>(
                        new ErrorMessage(exception),
                            new JsonNetSerializer(serializer))
                        .WithStatusCode(HttpStatusCode.InternalServerError));

            base.RequestStartup(container, pipelines, context);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            string exe = Assembly.GetEntryAssembly().Location;
            string tmplFile = Path.Combine(Path.GetDirectoryName(exe), "template.html");
            Templater tmpl = new Templater(tmplFile);
            container.Register<ITemplate>(tmpl);
        }
    }
}
