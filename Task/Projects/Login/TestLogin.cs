using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Login
{
    public class TestLogin : ILogin
    {
        public bool Login(string? username, string? password)
        {
            return username == "harrison" && password == "test";
        }
    }
}
