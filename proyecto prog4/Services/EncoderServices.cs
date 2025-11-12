using BC = BCrypt.Net.BCrypt;

namespace proyecto_prog4.Services
{
    public interface IEncoderServices
    {
        string Encode(string value);

        bool Verify(string value, string hash);
    }

    public class EncoderServices : IEncoderServices
    {
        public string Encode(string value)
        {
            string salt = BC.GenerateSalt(13);
            return BC.HashPassword(value, salt);
        }

        public bool Verify(string value, string hash)
        {
            return BC.Verify(value, hash);
        }
    }
}
