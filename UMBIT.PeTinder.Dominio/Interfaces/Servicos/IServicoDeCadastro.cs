using Microsoft.AspNetCore.Mvc;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeCadastro
    {
        Task ForgotPassword(string email);
        Task ResendConfirmationEmail(string email);
        Task ConfirmEmail(string email, string code);
        Task ResetPassword(string email, string resetCode, string novaSenha);
        Task<bool> Register(string email, string senha, string nome, string fotoData, DateTime dataNascimento);
    }
}
