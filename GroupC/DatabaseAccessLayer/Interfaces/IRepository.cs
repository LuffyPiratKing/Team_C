using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Interfaces
{
    interface IRepository
    {
        bool Authenticate(string username, string password);
    }
}
