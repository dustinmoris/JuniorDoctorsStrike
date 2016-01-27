using Autofac;
using Autofac.Integration.Mvc;
using JuniorDoctorsStrike.Common.Web;
using JuniorDoctorsStrike.Core;
using JuniorDoctorsStrike.TwitterApi;
using JuniorDoctorsStrike.Web.Controllers;
using System.Web.Mvc;

namespace JuniorDoctorsStrike.Web.Configuration
{
    public static class DependencyConfig
    {
        public static void Setup()
        {
            var builder = new ContainerBuilder();

            CachedMessageService.CreateBaseMessageService =
                () => new MessageService(
                    new TwitterClient(
                        new TwitterApiConfiguration(),
                        new UrlEncoder(),
                        new HtmlLinkParser(),
                        new TwitterHashtagParser()), 
                    AppConfig.GetHashtags());

            builder.RegisterInstance(CachedMessageService.Instance).As<IMessageService>();

            builder.RegisterType<HomeController>().AsSelf();
            builder.RegisterType<ApiController>().AsSelf();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}