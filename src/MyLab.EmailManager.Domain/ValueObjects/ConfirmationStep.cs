using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.EmailManager.Domain.ValueObjects
{
    public enum ConfirmationStep
    {
        Undefined,
        Created,
        Sent,
        Confirmed
    }
}
