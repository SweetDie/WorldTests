using System.Configuration;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using WorldTests.BLL.Interfaces;
using WorldTests.BLL.Services;
using WorldTests.BLL.Utilities;
using WorldTests.DAL;
using WorldTests.DAL.Interfaces;
using WorldTests.DAL.Repository;

namespace WorldTests.Server
{
    public class ServicesInitializer
    {
        private readonly IContainer _container;

        public ServicesInitializer() : this(new MapperConfigTest(), new MapperConfigUser())
        {
        }

        public ServicesInitializer(params Profile[] profiles)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationContext>().As<DbContext>();
            builder.RegisterType<ApplicationContext>().As<ApplicationContext>();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IDataGenericRepository<>));
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(profiles)));
            builder.RegisterInstance(mapper).AsSelf();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<TestService>().As<ITestService>();
            _container = builder.Build();
        }

        public void InitializeUserService(out IUserService userService)
        {
            userService = _container.Resolve<IUserService>();
        }

        public void InitializeTestService(out ITestService testService)
        {
            testService = _container.Resolve<ITestService>();
        }
    }
}
