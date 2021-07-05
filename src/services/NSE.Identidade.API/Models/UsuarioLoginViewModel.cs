using System.ComponentModel.DataAnnotations;

namespace NSE.Identidade.API.Models {
    public class UsuarioLoginViewModel {
        private const string MsgErroCampoRequerido = "O campo {0}, é obrigatório!";
        private const string MsgErroEmailFormatoInvalido = "O campo {0} está no formato inválido!";
        private const string MsgErroLimiteSenha = "O campo {0}, deve ter entre {2} e {1} caracteres!";
        private const int SenhaTamMin = 6;
        private const int SenhaTamMax = 100;

        [Required(ErrorMessage = MsgErroCampoRequerido)]
        [EmailAddress(ErrorMessage = MsgErroEmailFormatoInvalido)]
        public string Email { get; set; }

        [Required(ErrorMessage = MsgErroCampoRequerido)]
        [StringLength(SenhaTamMax, ErrorMessage = MsgErroLimiteSenha, MinimumLength = SenhaTamMin)]
        public string Senha { get; set; }
    }
}