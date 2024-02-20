using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Interfaces
{
    public interface ITransactionProvider
    {
        void To(IAccount account, double amount);
        void From(IAccount account, double amount);
    }
}
