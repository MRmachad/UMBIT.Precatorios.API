using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;

namespace UMBIT.Precatorios.SDK.API.Models.Fabrica
{
    public static class FabricaGenerica
    {
        public static IServiceProvider services { get; set; }

        public static T Crie<T>()
        {
            try
            {
                return services.GetService<T>();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT($"Erro ao resgatar serviço {nameof(T)}", ex);
            }
        }
    }
}
