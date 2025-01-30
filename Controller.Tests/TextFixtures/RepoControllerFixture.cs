using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presemtation.ControllersForTest;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests.TextFixtures
{
    public class RepoControllerFixture : IDisposable
    {
        public readonly Mock<UserManager<ApplicationUser>> UserManager;
        public Mock<IServiceManager> ServiceManagerMock { get; }
        public Mapper Mapper { get; }
        public RepositoryController Sut { get; }

        public RepoControllerFixture()
        {
            ServiceManagerMock = new Mock<IServiceManager>();
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            UserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            Sut = new RepositoryController(ServiceManagerMock.Object, Mapper, UserManager.Object);
        }

        public List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id="1",
                    Name="Kalle",
                    Age=12,
                    UserName = "Kalle"
                },
                new ApplicationUser
                {
                    Id="2",
                    Name="Kalle",
                    Age=12,
                    UserName="Kalle"
                }
            };
        }
        public void Dispose()
        {
            //Not used here
        }
    }

}
