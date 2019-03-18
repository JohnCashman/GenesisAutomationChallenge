using Autofac;
using AutofacSerilogIntegration;
using AutoMapper;
using Challenge.Infrastructure;
using Challenge.Profiles;
using Challenge.Queries;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Challenge
{
    internal static class Program
    {
        private static IContainer Container { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BootStrap();
            Application.ThreadException += Application_ThreadException;
            using (var scope = Container.BeginLifetimeScope())
            {
                Application.Run(scope.Resolve<OrdersForm>());
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.ForContext(typeof(Program)).Fatal(e.Exception, "Unhandled Exception");
        }

        private static void BootStrap()
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                        .WriteTo.RollingFile("log-{Date}.txt")
                        .WriteTo.Console()
                        .CreateLogger();

            var builder = new ContainerBuilder();
            builder.RegisterLogger();

            builder.RegisterType<OrdersContext>().AsSelf();

            builder.RegisterType<OrdersForm>();
            builder.RegisterType<EditCustomerForm>();

            var profiles = from t in typeof(OrderProfile).Assembly.GetTypes() where typeof(Profile).IsAssignableFrom(t) select (Profile)Activator.CreateInstance(t);
            builder.Register(_ => new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            // mediator itself
            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(OrderSumaryQueries).GetTypeInfo().Assembly).AsImplementedInterfaces();

            Container = builder.Build();
        }
    }
}
