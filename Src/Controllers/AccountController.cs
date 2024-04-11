using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace courses_dotnet_api.Src.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountController(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    [HttpPost("register")]
    public async Task<IResult> Register(RegisterDto registerDto)
    {
        if (//busca en los correos registrados existentes
            await _userRepository.UserExistsByEmailAsync(registerDto.Email)
            || await _userRepository.UserExistsByRutAsync(registerDto.Rut)
        )
        {
            return TypedResults.BadRequest("User already exists");
        }

        await _accountRepository.AddAccountAsync(registerDto);

        if (!await _accountRepository.SaveChangesAsync())
        {
            return TypedResults.BadRequest("Failed to save user");
        }

        AccountDto? accountDto = await _accountRepository.GetAccountAsync(registerDto.Email);

        return TypedResults.Ok(accountDto);
    }

        [HttpPost("login")]


    public async Task<IResult> Login(LoginDto loginDto){
        //buscar usuario por email LISTO
        //si email no existe retorna error/bad request LISTO
        //validar password
        //contraseña correcta -> retorna un account DTO
        //contraña incorrecta -> retorna un bad request
         if (//busca en los correos registrados existentes
            !await _userRepository.UserExistsByEmailAsync(loginDto.Email)
            
        )
        {
             return TypedResults.BadRequest("Email doesn't exists");
        }

        bool passwordOK = await _accountRepository.CheckPassword(loginDto.Email, loginDto.Password);

        if(!passwordOK)
        {
            return TypedResults.BadRequest("Password doesn't match");
        }
        AccountDto? accountDto = await _accountRepository.GetAccountAsync(loginDto.Email);

        return TypedResults.Ok(accountDto);

        
    }


  




}
