using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TotalHRInsight.DTO;
using TotalHRInsight.DAL;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return BadRequest(new { success = false, message = "El usuario no está autenticado." });
        }
        try
        {
            await _signInManager.SignOutAsync();
            return Ok(new { success = true, message = "Usuario desconectado exitosamente." });
        }
        catch (System.Exception ex)
        {
            // Registrar la excepción (usar un marco de registro)
            return StatusCode(500, new { success = false, message = "Ocurrió un error al intentar desconectar al usuario.", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized(new { success = false, message = "Modelo de datos no válido." });
        }

        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Usuario no encontrado." });
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { success = false, message = "Contraseña incorrecta." });
            }

            if (!user.Estado)
            {
                return Unauthorized(new { success = false, message = "Usuario inactivo." });
            }

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                idUser = user.Id,
                nombreUser = user.Nombre,
                apellidoUser = user.PrimerApellido,
                success = true,
                message = "Autenticación exitosa."
            });
        }
        catch (Exception ex)
        {
            // Log the exception (use a logging framework)
            return StatusCode(500, new { success = false, message = "Ocurrió un error durante la autenticación.", error = ex.Message });
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto model)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized(new { success = false, message = "Modelo de datos no válido." });
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Dirección de correo electrónico inválida." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return Ok(new { success = true, message = "Correo electrónico válido.", token = encodedToken });
        }
        catch (Exception ex)
        {
            // Registrar la excepción (usar un marco de registro)
            return StatusCode(500, new { success = false, message = "Ocurrió un error al generar el token de restablecimiento de contraseña.", error = ex.Message });
        }
    }

    [HttpPost("change_password")]
    public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized(new { success = false, message = "Modelo de datos no válido.", errors = ModelState });
        }

        // Validar que las contraseñas coincidan
        if (model.NewPassword != model.ConfirmPassword)
        {
            return StatusCode(500, new { success = false, message = "La contraseña y la confirmación de la contraseña no coinciden." });
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Dirección de correo electrónico inválida." });
            }

            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(model.Token));

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "La contraseña ha sido cambiada exitosamente." });
            }

            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return StatusCode(500, new { success = false, message = "Error al cambiar la contraseña.", errors });
        }
        catch (Exception ex)
        {
            // Registrar la excepción (usar un marco de registro)
            return StatusCode(500, new { success = false, message = "Ocurrió un error al restablecer la contraseña.", error = ex.Message });
        }

    }

}

