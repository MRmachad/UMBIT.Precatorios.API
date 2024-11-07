using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UMBIT.Precatorios.Dominio.Configuradores;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;
using UMBIT.Precatorios.SDK.Notificacao;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;

namespace UMBIT.Precatorios.Dominio.Servicos.auth
{
    public class ServicoDeCadastro : IServicoDeCadastro
    {
        private readonly IMediator Mediator;
        private readonly INotificador Notificador;
        private readonly UserManager<Usuario> UserManager;
        private readonly IUserValidator<Usuario> UserValidator;
        private readonly IOptions<IdentitySettings> IdentitySettings;
        private readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        public ServicoDeCadastro(
            IMediator Mediator,
            INotificador notificador,
            UserManager<Usuario> userManager,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IUserValidator<Usuario> UserValidator,
            IOptions<IdentitySettings> identitySettings)
        {
            this.Mediator = Mediator;
            Notificador = notificador;
            UserManager = userManager;
            this.UserValidator = UserValidator;
            UnidadeDeTrabalho = unidadeDeTrabalho;
            IdentitySettings = identitySettings ?? Options.Create(new IdentitySettings()); ;
        }


        public async Task ForgotPassword(string email)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("O email parece não estar vinculado a nenhum Usuario"));
                }
                else
                {
                    var tokenReset = await UserManager.GeneratePasswordResetTokenAsync(user);
                    //implemente envio de email? (token e idenitificador de usuario)
                }
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao gerar token de redefinição de senha de usuario!", ex);
            }
        }

        public async Task ResendConfirmationEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string email, string senha, string nome, string fotoData, DateTime dataNascimento)
        {
            await UnidadeDeTrabalho.InicieTransacao();
            try
            {
                var usuario = new Usuario
                {
                    Nome = nome,
                    Email = email,
                    UserName = email,
                    TwoFactorEnabled = IdentitySettings.Value.TwoFactorEnabled,
                    EmailConfirmed = !IdentitySettings.Value.RequireConfirmedEmail,
                };

                var result = await UserManager.CreateAsync(usuario, senha);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                        Notificador.AdicionarNotificacao(new Notificacao(item.Code, item.Description));
                }
                else if (IdentitySettings.Value.RequireConfirmedEmail)
                {
                    //implemente envio de email confirmação
                }


                await UnidadeDeTrabalho.FinalizeTransacao();

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                await UnidadeDeTrabalho.RevertaTransacao();
                throw new ExcecaoBasicaUMBIT("Erro ao criar usuario!", ex);
            }
        }

        public async Task ResetPassword(string email, string resetCode, string novaSenha)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("O email parece não estar vinculado a nenhum Usuario"));
                }
                else
                {
                    var resetResult = await UserManager.ResetPasswordAsync(user, resetCode, novaSenha);
                    if (!resetResult.Succeeded)
                        resetResult.Errors.ToList().ForEach(t => Notificador.AdicionarNotificacao(new Notificacao(t.Code, t.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao Redefinir senha de usuario!", ex);
            }
        }

        public async Task ConfirmEmail(string email, string code)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("O email parece não estar vinculado a nenhum Usuario"));
                }
                else
                {
                    var resetResult = await UserManager.ConfirmEmailAsync(user, code);
                    if (!resetResult.Succeeded)
                        resetResult.Errors.ToList().ForEach(t => Notificador.AdicionarNotificacao(new Notificacao(t.Code, t.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao Confirmar Email!", ex);
            }
        }

        public async Task Twofactor(bool enable, string twoFactorCode, bool resetShareKey, bool resetRecoveryCodes, bool forgetMachine)
        {
            throw new NotImplementedException();
        }
    }
}
