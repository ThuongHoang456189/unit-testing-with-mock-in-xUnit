using Autofac;
using DataAccess;
using DataAccess.Utilities;
using DataAccess.Logic;
using System.Data;

namespace Assignment4_BookManagement
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CustomizedApplication>().As<IApplication>();
            builder.RegisterType<BookDataAccess>()
                .As<IBookDataAccess>()
                .WithParameter("bookTable", new DataTable());
            builder.RegisterType<SqlServerDataAccess>()
                .As<ISqlServerDataAccess>()
                .WithParameter("id", "BookStoreDB");

            return builder.Build();
        }
    }
}
