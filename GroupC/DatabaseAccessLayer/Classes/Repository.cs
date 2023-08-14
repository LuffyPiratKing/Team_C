using DatabaseAccessLayer.Interfaces;
using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Classes
{
    public class Repository : IRepository
    {
        private IdentityServerContext _context;

        public Repository()
        {
            _context = new IdentityServerContext();
        }

        public bool Authenticate(string username, string password)
        {
            var matchingUser = _context.Users.Where(x => x.Username == username && x.Pasword == password).FirstOrDefault();
            if (matchingUser != null)
            {
                return true;
            }
            return false;
        }
    }
}
