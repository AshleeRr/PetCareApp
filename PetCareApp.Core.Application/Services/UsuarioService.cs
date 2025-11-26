using PetCareApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Domain.Entities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace PetCareApp.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;
        public UsuarioService(IUsuarioRepository repo) { _repo = repo; }

        public async Task<UserDto> CrearAsync(CrearUsuarioRequest req)
        {
            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null) throw new System.Exception("Email ya registrado");

            var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password); // usar BCrypt (agrega paquete)
            var u = new Usuario { UserName = req.UserName, Email = req.Email, PasswordHashed = hashed, RoleId = req.RoleId };
            await _repo.AddAsync(u);
            return new UserDto { Id = u.Id, Email = u.Email, UserName = u.UserName, Role = u.RoleId };
        }

        public async Task<UserDto> LoginAsync(string email, string password)
        {
            var u = await _repo.GetByEmailAsync(email);
            if (u == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, u.PasswordHashed)) return null;
            return new UserDto { Id = u.Id, Email = u.Email, UserName = u.UserName, Role = u.RoleId };
        }
    }
}
