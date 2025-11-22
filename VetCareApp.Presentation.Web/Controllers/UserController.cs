
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.ViewModels.UsersVms;
using System.Security.Claims;

namespace VetCareApp.Presentation.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // GET: UserController
        public async Task<IActionResult> Perfil()
        {
            // 1️⃣ Obtener el email del veterinario autenticado
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized();

            // 2️⃣ Llamar al servicio
            var userDto = await _userService.GetUserByEmailAsync(email);

            // 3️⃣ Mapear DTO → VM
            var vm = _mapper.Map<UserVm>(userDto);

            // 4️⃣ Retornar la vista
            return View(vm);
        }
    }
}
