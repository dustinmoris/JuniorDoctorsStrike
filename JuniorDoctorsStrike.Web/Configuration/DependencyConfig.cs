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
            builder.RegisterType<UrlEncoder>().As<IUrlEncoder>();
            builder.RegisterType<HtmlLinkParser>().As<IHtmlLinkParser>();
            builder.RegisterType<TwitterApiConfiguration>().As<ITwitterApiConfiguration>();
            builder.RegisterType<TwitterHashtagParser>().As<ITwitterHashtagParser>();
            builder.RegisterType<TwitterClient>().As<ITwitterClient>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<HomeController>().AsSelf();
            builder.RegisterType<ApiController>().AsSelf();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}