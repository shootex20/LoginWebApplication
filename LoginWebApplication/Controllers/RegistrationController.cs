using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using Microsoft.AspNetCore.Mvc;
using LoginWebApplication.Operations;

namespace LoginWebApplication.Controllers
{
    public class RegistrationController : ApiController
    {

        private string connString = ConfigurationManager.AppSettings["sqlConnection"];
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("api/register")]
        public ActionResult<string> CreateUser(User user)
        {
            /*
            User p = new User();
            p.FirstName = "Chelsey";
            p.LastName = "Coughlin";
            p.Email = "chelsey74@live.ca";
            p.Password = "1234";
            */

            //User user = p;
            ValidateUser validate = new ValidateUser();
            UserOperations userOps = new UserOperations();

            if (validate.ValidateUserEmail(user) && !validate.ValidateEmailNotRegistered(user) && !validate.ValidateUserIsNotEmpty(user))
            {
                user = userOps.SaltAndHash(user);
                user.Id = Guid.NewGuid();
                userOps.CreateUser(user);
                return "User created successfully.";
            }
            else
            {
                return "User not created successfully";
            }
        }
        /*
        public User SaltAndHash(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            user.Password = hashed;

            return user;
        }
         */
    }
}
