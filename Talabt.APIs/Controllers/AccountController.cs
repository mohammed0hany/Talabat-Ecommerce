using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabt.APIs.DTOs;
using Talabt.APIs.Errors;
using Talabt.APIs.Extentions;

namespace Talabt.APIs.Controllers
{

    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager ,SignInManager<AppUser> signInManager,IAuthService authService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")] //POST api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user= await _userManager.FindByEmailAsync(model.Email);
            if(user is null) 
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)
            });

        }

        [HttpPost("register")] //POST api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {   if(CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] {"this email already exists"} });
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        //get current user
        [Authorize]
        [HttpGet] //Get api/account
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email = user.Email,
                Token= await _authService.CreateTokenAsync(user,_userManager)
            });
        }

        [Authorize]             //Get: api/account
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUsersWithAddressAsync(User);
            var address= _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(address);
        }

        //update user address
        [Authorize]
        [HttpPut("address")] //Put: api/account
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = _userManager.FindByEmailAsync(email);

            var user = await _userManager.FindUsersWithAddressAsync(User);
            address.Id = user.Address.Id;
            user.Address= address;
            var result= await _userManager.UpdateAsync(user);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);

        }

        [HttpGet("emailexists")] //Get: api/accounts/emailexists?email=ahmed.nasr@linkdev.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
     
        }

    }
}
