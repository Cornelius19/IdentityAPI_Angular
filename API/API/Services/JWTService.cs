using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        public JWTService(IConfiguration config)
        {
            this._config = config;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])); //Encoding to utf8 transform our key string from JWT key in appsettings.dec.json into byte arrays
                                                                                            //and SymmetricSecurityKey is gonna create a key from us that is gonna be stored in _jwtKey with this we
                                                                                            //are gonna encrypt and decrypt a JSON web token
        }
        public string CreateJWT(User user) // a method for creating JWT that recieve an argument of type User
        {
            var userClaims = new List<Claim> // a list of claims inside my toke
                                                // a claim is like our data included in JWT in our care Id,Email,Names
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var credentials = new SigningCredentials(_jwtKey,SecurityAlgorithms.HmacSha256Signature); //this signin credential is going to take a key and a algorith to encrypt our credential this one is the most secured one
            var tokenDescriptor = new SecurityTokenDescriptor // our description of our token
            {
                //Subject contains our list of claims
                Subject = new ClaimsIdentity(userClaims),
                //In how many time it will expire and we need to parse it into integer
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                //SingingCredential is base of our credential
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();//our token handler 
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
