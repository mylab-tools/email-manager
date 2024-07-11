using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.EmailManager.Domain.Exceptions
{
    public class InvalidConfirmationSeedException() : DomainException("Invalid confirmation seed");
}
