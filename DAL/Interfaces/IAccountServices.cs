using DTO;

namespace DAL.Interfaces
{
    public interface IAccountServices
    {
        Task<SignupDTO> SignupUserAsync(SignupDTO model);

        Task<bool> IsUserExists(SignInDTO signInDTO);
    }
}
