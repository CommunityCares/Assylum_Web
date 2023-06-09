using Microsoft.AspNetCore.Mvc;
using CommunityCares_Web.Models;

namespace CommunityCares_Web.Controllers
{
    public class UserConfig : Controller
    {
        
            private static User _user;

            public static User userLogin
            {
                get { return _user; }
                set
                {
                    _user = value;
                }
            }

        
    }
}
