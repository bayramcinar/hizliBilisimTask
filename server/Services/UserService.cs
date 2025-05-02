using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InvoiceAPI.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;

        public UserService(DataContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        //tüm kullanıcıları alma fatura oluşturuken kullanmamk için 
 
         public async Task<List<UserDto>> GetUsersAsync()
                {
                    return await _context.Users
                        .Select(u => new UserDto
                        {
                            UserId = u.UserId,
                            UserName = u.UserName
                        })
                        .ToListAsync();
                }

        // üye olma 
        public async Task<Result> RegisterAsync(User newUser)
        {
            var exists = await _context.Users.AnyAsync(u => u.UserName == newUser.UserName);
            if (exists)
            {
                return new Result
                {
                    Success = false,
                    Message = "Bu kullanıcı adı zaten alınmış."
                };
            }

            var user = new User
            {
                UserName = newUser.UserName,
                Password = newUser.Password, // SHA256 hashlenmiş olarak geliyor
                RecordDate = DateTime.UtcNow // Şu anki UTC tarih-saat
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new Result
            {
                Success = true,
                Message = "Kayıt başarılı."
            };
        }

         // giriş yapma
        public async Task<Result> LoginAsync(User loginUser)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginUser.UserName);

            if (user == null || user.Password != loginUser.Password)
            {
                return new Result
                {
                    Success = false,
                    Message = "Geçersiz şifre ya da kullanıcı adı."
                };
            }

            var token = _jwtService.GenerateToken(user.UserName);

            return new Result
            {
                Success = true,
                Message = token
            };
        }
         // token geçerliliği
        public bool VerifyToken(string token)
        {
            return _jwtService.ValidateToken(token);
        }
    }
}
