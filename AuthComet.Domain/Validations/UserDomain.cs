﻿using AuthComet.Domain.Dtos;
using AuthComet.Domain.Entities;
using AuthComet.Domain.Response;
using AuthComet.Domain.Validations.Messages;
using System.Text.RegularExpressions;

namespace AuthComet.Domain.Validations
{
    public class UserDomain
    {
        public Response<User> CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username))
                return Response<User>.Fail(UserMessages.UsernameRequired);

            if (string.IsNullOrWhiteSpace(userDto.Password))
                return Response<User>.Fail(UserMessages.PasswordRequired);

            if (string.IsNullOrWhiteSpace(userDto.Email) || !IsValidEmail(userDto.Email))
                return Response<User>.Fail(UserMessages.InvalidEmail);

            var user = new User()
            {
                Id = userDto.Id, // Assuming Id is handled externally or auto-incremented
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email,
                CreationDate = DateTime.Now
            };

            return Response<User>.Success(user);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            return Regex.IsMatch(email, pattern);
        }
    }
}