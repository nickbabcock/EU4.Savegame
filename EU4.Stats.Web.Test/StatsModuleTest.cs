using Moq;
using Nancy;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;
using Nancy.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU4.Stats.Web.Test
{
    [TestFixture]
    public class StatsModuleTest
    {
        Browser browser = new Browser(with =>
        {
            with.Module<StatsModule>();
            with.Dependency(Mock.Of<ITemplate>(x =>
                x.Render(It.IsAny<object>()) == Task.FromResult("Hi")));
            with.Dependency(Mock.Of<IIdGenerator>(x =>
                x.NextId() == "1"));
            with.Dependency(new SavegameStorage("data"));
            with.RequestStartup((container, pipelines, context) => {
                var serializer = new JsonSerializer() 
                { ContractResolver = 
                    new CamelCasePropertyNamesContractResolver()
                };
                pipelines.OnError.AddItemToEndOfPipeline((with2, exception) =>
                        new JsonResponse<ErrorMessage>(
                            new ErrorMessage(exception),
                                new JsonNetSerializer(serializer))
                            .WithStatusCode(HttpStatusCode.InternalServerError));
            });
        });

        public StatsModuleTest()
        {
            foreach (var file in Directory.EnumerateFiles("data", "*.html"))
                File.Delete(file);
        }

        [Test]
        public void NeedXFileHeader()
        {
            var response = browser.Post("/games", with =>
            {
                with.HttpRequest();
                with.Header("X-FILE-EXTENSION", ".eu4");
            });

            StringAssert.Contains("File can't be null", response.Body.AsString());
        }

        [Test]
        public void NeedXFileExtension()
        {
            var response = browser.Post("/games", with =>
            {
                with.HttpRequest();
                with.Header("X-FILE", ".");
            });

            StringAssert.Contains("File extension can't be null", response.Body.AsString());
        }

        [Test]
        public void CanRenderEU4()
        {
            var response = browser.Post("/games", with =>
                {
                    with.HttpRequest();
                    with.Header("X-FILE", Path.Combine("data", "test.eu4"));
                    with.Header("X-FILE-EXTENSION", ".eu4");
                });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CanRenderEU4Gz()
        {
            var response = browser.Post("/games", with =>
            {
                with.HttpRequest();
                with.Header("X-FILE", Path.Combine("data", "test.eu4.gz"));
                with.Header("X-FILE-EXTENSION", ".gz");
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CanRenderEU4Zip()
        {
            var response = browser.Post("/games", with =>
            {
                with.HttpRequest();
                with.Header("X-FILE", Path.Combine("data", "test.zip"));
                with.Header("X-FILE-EXTENSION", ".zip");
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
