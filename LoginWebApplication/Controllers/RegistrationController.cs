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
        public IHttpActionResult CreateUser(Person person)
        {
            Person p = new Person();
            p.FirstName = "Chelsey";
            p.LastName = "Coughlin";
            p.Email = "chelsey74@live.ca";
            p.Password = "1234";
            CreatePerson(p);
            return Ok();
        }

        //[HttpPost, Route("api/password")]
        public Person SaltAndHash(Person person)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: person.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            person.Password = hashed;

            return person;
        }

        public IHttpActionResult CreatePerson(Person person)
        {
            Person user = person;
            ValidateUser validate = new ValidateUser();
            Operations.UserOperations userOps = new Operations.UserOperations();

            if (validate.ValidateUserEmail(user) && !validate.ValidateEmailNotRegistered(user) && !validate.ValidateUserIsNotEmpty(user))
            {
               user = SaltAndHash(person);
               user.Id = Guid.NewGuid();
               userOps.CreateUser(user);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
