using System.ComponentModel.DataAnnotations;

namespace NSE.Identidade.API.Models {
    public class UsuarioRegistroViewModel {
        private const string MsgErroCampoRequerido = "O campo {0}, é obrigatório!";
        private const string MsgErroEmailFormatoInvalido = "O campo {0} está no formato inválido!";
        private const string MsgErroLimiteSenha = "O campo {0}, deve ter entre {2} e {1} caracteres!";
        private const string MsgSenhaNaoConfere = "As senhas não conferem";
        private const int SenhaTamMin = 6;
        private const int SenhaTamMax = 100;

        [Required(ErrorMessage = MsgErroCampoRequerido)]
        [EmailAddress(ErrorMessage = MsgErroEmailFormatoInvalido)]
        public string Email { get; set; }

        [Required(ErrorMessage = MsgErroCampoRequerido)]
        [StringLength(SenhaTamMax, ErrorMessage = MsgErroLimiteSenha, MinimumLength = SenhaTamMin)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = MsgSenhaNaoConfere)]
        public string SenhaConfirmacao { get; set; }
    }
}