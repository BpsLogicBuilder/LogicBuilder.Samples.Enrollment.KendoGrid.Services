using Enrollment.Contexts;
using Enrollment.Data.Entities;
using Enrollment.Domain.Entities;
using Enrollment.KendoGrid.Bsl.Controllers;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using LogicBuilder.App.KendoGrid.Bsl.Business.Requests;
using LogicBuilder.App.KendoGrid.Bsl.Utils.Interfaces;
using LogicBuilder.EntityFrameworkCore.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Enrollment.KendoGrid.Bsl.Tests
{
    [Collection("DatabaseCollection")]
    public class GridControllerTest
    {
        public GridControllerTest(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            Initialize();
        }

        #region Fields
        private readonly DatabaseFixture databaseFixture;
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public async Task Get_persons_ungrouped_with_aggregates()
        {
            //arrange
            KendoGridDataRequest request = new()
            {
                Options = new KendoGridDataSourceRequestOptions
                {
                    Aggregate = "lastName-count~zipCode-min",
                    Filter = null,
                    Group = null,
                    Page = 1,
                    Sort = "zipCode-asc",
                    PageSize = 5
                },
                ModelType = typeof(PersonalModel).AssemblyQualifiedName,
                DataType = typeof(Personal).AssemblyQualifiedName
            };

            IRequestHelper helper = serviceProvider.GetRequiredService<IRequestHelper>();
            GridController controller = new(helper);

            //act
            DataSourceResult result = await controller.GetData(request);

            //assert
            Assert.Equal(2, result.Total);
            Assert.Equal(2, ((IEnumerable<PersonalModel>)result.Data).Count());
            Assert.Equal(2, result.AggregateResults.Count());
            Assert.Equal("Count", result.AggregateResults.First().AggregateMethodName);
            Assert.Equal(2, (int)result.AggregateResults.First().Value);
        }

        [Fact]
        public async Task Get_persons_grouped_with_aggregates()
        {
            //arrange
            KendoGridDataRequest request = new()
            {
                Options = new KendoGridDataSourceRequestOptions
                {
                    Aggregate = "lastName-count~zipCode-min",
                    Filter = null,
                    Group = "zipCode-asc",
                    Page = 1,
                    Sort = null,
                    PageSize = 5
                },
                ModelType = typeof(PersonalModel).AssemblyQualifiedName,
                DataType = typeof(Personal).AssemblyQualifiedName
            };

            IRequestHelper helper = serviceProvider.GetRequiredService<IRequestHelper>();
            GridController controller = new(helper);

            //act
            DataSourceResult result = await controller.GetData(request);

            Assert.Equal(2, result.Total);
            Assert.Single((IEnumerable<AggregateFunctionsGroup>)result.Data);
            Assert.Equal(2, result.AggregateResults.Count());

            AggregateResult[] aggregateResults = [.. result.AggregateResults];
            Assert.Equal("Count", aggregateResults[0].AggregateMethodName);
            Assert.Equal(2, (int)aggregateResults[0].Value);
            Assert.Equal("Min", aggregateResults[1].AggregateMethodName);
            Assert.Equal("30060", (string)aggregateResults[1].Value);
        }

        #region Helpers
        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSqlServerDatabaseConfiguration(databaseFixture.GetConnectionString($"{GetType().Name}_{Guid.NewGuid():N}"))
                .AddLogging()
                .AddAutoMapperConfiguration()
                .AddKendoGridBslUtilsServices()
                .AddAppUtilsMappingOperations()
                .AddGridRequestServices()
                .BuildServiceProvider();

            ReCreateDataBase(serviceProvider.GetRequiredService<EnrollmentContext>()).GetAwaiter().GetResult();
            DatabaseSeeder.Seed_Database(serviceProvider.GetRequiredService<IContextRepository>()).GetAwaiter().GetResult();
        }

        private static async Task ReCreateDataBase(EnrollmentContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        #endregion Helpers
    }
}
