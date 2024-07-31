using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CPOC_AIMS_II_Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using CPOC_AIMS_II_Backend;

namespace TestAPI.Services
{
	public class AuthService
	{
		private const double EXPIRE_HOURS = 48.0;
		public static string CreateToken(ResultLogin user,string secret)
		{
			var key = Encoding.ASCII.GetBytes(secret);
			var tokenHandler = new JwtSecurityTokenHandler();
			var descriptor = new SecurityTokenDescriptor
			{
				Subject = getClaimsIdentity(user),
				Expires = DateTime.Now.AddHours(EXPIRE_HOURS),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			};
			var token = tokenHandler.CreateToken(descriptor);

			return tokenHandler.WriteToken(token);
		}

		public static ClaimsIdentity getClaimsIdentity(ResultLogin user)
		{
			return new ClaimsIdentity(getClaims());

			Claim[] getClaims()
			{
				List<Claim> claims = new List<Claim>();
				claims.Add(new Claim(ClaimTypes.Name, user.username.ToString()));
				return claims.ToArray();
			}
		}

		public static string Sha256Hash(string value, string secret)
		{
			string finalKey = string.Empty;
			byte[] encode = new byte[value.Length];
			encode = Encoding.UTF8.GetBytes(value + secret);
			finalKey = Convert.ToBase64String(encode);
			return finalKey;

		}

		internal static object CreateToken(JsonResult queryResult)
		{
			throw new NotImplementedException();
		}
	}
}