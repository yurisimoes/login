using Api.Entities.Base;

namespace Api.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }

        public string UsernameNormalized
        {
            get => Username.ToLowerInvariant();
            set { }
        }

        public string Hash { get; set; }
    }
}