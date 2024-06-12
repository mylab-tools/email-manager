using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLab.EmailManager.Domain;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class EmailBehavior
    {
        [Fact]
        public void ShouldUpdateLabels()
        {
            //Arrange
            var email = new Email("login@host.com");
            email.Labels.Add(new EmailLabel("foo", "bar"));

            //Act
            email.UpdateLabels(new []{ new EmailLabel("baz", "qoz") });

            //Assert
            Assert.Single(email.Labels);
            Assert.Contains(email.Labels, l => l.Name.Value == "baz" && l.Value == "qoz");

        }
    }
}
