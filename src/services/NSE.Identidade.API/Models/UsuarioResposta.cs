using System.Collections.Generic;

namespace NSE.Identidade.API.Models
{
    public class UsuarioResposta {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }
    }

    public class UsuarioToken {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }

    public class UsuarioClaim {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
