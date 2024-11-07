using Microsoft.EntityFrameworkCore;
using System.Reflection;
using  UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.SDK.Repositorio.EF
{

    public abstract class BaseContext<T> : DbContext  where T : DbContext
    {
        public BaseContext(DbContextOptions<T> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (assembly != null)
                        assembly.GetTypes()
                        .Where(t =>
                            t != null &&
                            t.Namespace != null &&
                            t.BaseType != null &&
                            t.IsClass &&
                            t.BaseType.IsGenericType &&
                            (t.BaseType.GetGenericTypeDefinition() == typeof(CoreEntityConfigurate<>)))
                        .ToList().ForEach((t) =>
                        {
                            dynamic instanciaDeConfiguracao = Activator.CreateInstance(t);
                            modelBuilder.ApplyConfiguration(instanciaDeConfiguracao);
                        });
                }
                catch
                {
                    continue;
                }

            }

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                ChangeTracker.DetectChanges();

                var entries = ChangeTracker.Entries<BaseEntity>()
                    .Where(entry =>
                       entry.State == EntityState.Added ||
                       entry.State == EntityState.Modified ||
                       entry.State == EntityState.Deleted);

                if (entries.Any())
                {
                    foreach (var entry in entries)
                    {
                        var now = DateTime.Now;

                        if (entry.State == EntityState.Added)
                        {
                            entry.Property(entidade => entidade.DataCriacao).CurrentValue = now;
                        }

                        entry.Property(entidade => entidade.DataAtualizacao).CurrentValue = now;
                    }
                }

                var result = await base.SaveChangesAsync(cancellationToken); 

                ChangeTracker.Clear();

                return result;

            }
            catch (DbUpdateException ex)
            {
                ChangeTracker.Clear();
                throw ex;

            }
            catch (Exception ex)
            {
                ChangeTracker.Clear();
                throw ex;
            }

            return 0;
        }

    }

}
