using Microsoft.AspNetCore.Mvc;

namespace RC.CryptoAES.Api.Controllers
{
    [ApiController]
    [Route("api/Cryptography")]
    public class CryptographyController : ControllerBase
    {
        private readonly ICryptography cryptography;

        public CryptographyController(ICryptography cryptography)
        {
            this.cryptography = cryptography;
        }

        [HttpGet]
        [Route("Decrypt")]
        public string Decrypt(string text)
        {
            return this.cryptography.Decrypt(text);
        }

        [HttpGet]
        [Route("Encrypt")]
        public string Encrypt(string text)
        {
            return this.cryptography.Encrypt(text);
        }
    }
}