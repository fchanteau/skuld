﻿using System.ComponentModel.DataAnnotations;

namespace Skuld.WebApi.Features.Auth.Dto
{
	public class LoginPayload
	{
		[Required]
		[RegularExpression (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}