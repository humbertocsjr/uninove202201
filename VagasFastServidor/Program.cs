using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlite<VagasDbContext>("Data Source=vagas.db");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

await CriaDbSeNaoExiste(app.Services, app.Logger);

app.MapPost("/cidade/{cidade}",  (string cidade, VagasDbContext bd) => 

    bd.Vagas.Where(v => v.Disponivel & v.Cidade == cidade).ToList()
);

app.MapPost("/solicitar/{id}", (int id, string chave, VagasDbContext bd) =>
{
    Validar(chave, bd);
    var sessaoCons = bd.Sessoes.Where(s => s.Chave == Guid.Parse(chave));
    var vagaCons = bd.Vagas.Where(v => v.Disponivel & v.Id == id);
    if (!vagaCons.Any() | !sessaoCons.Any())
    {
        return new { Mensagem = "Esta vaga não está mais disponível.", Solicitada = false };
    }

    var vaga = vagaCons.First();

    vaga.Disponivel = false;
    bd.Vagas.Update(vaga);
    bd.SaveChanges();

    var aluguel = new Aluguel()
    {
        VagaId = vaga.Id,
        Fim = DateTime.MinValue,
        Inicio = DateTime.Now,
        Status = StatusAluguel.Pendente
    };
    bd.Alugueis.Add(aluguel);
    
    return new { Mensagem = "Vaga solicitada com sucesso, aguarde o contado por parte do locador", Solicitada = true };
});

app.MapPost("/login", (string usuario, string senha, VagasDbContext bd) =>
{
    var cons = bd.Usuarios.Where(u => u.Login == usuario & u.Senha == senha);
    if(cons.Any())
    {
        Sessao s = new Sessao();
        s.UsuarioId = cons.First().Id;
        s.Vencimento = DateTime.Now.AddDays(1);
        s.Chave = Guid.NewGuid();
        bd.Sessoes.Add(s);
        return new {Mensagem = "Ok", Chave = s.Chave.ToString()};
    }
    return new {Mensagem = "Usuário e/ou senha inválidos", Chave = ""}!;
});

app.MapPost("/cadastrar", (string usuario, string senha, VagasDbContext bd) =>
{
    if (bd.Usuarios.Any(u => u.Login == usuario)) 
        return new { Mensagem = "Usuário já existe", Criado = false };

    var u = new Usuario
    {
        Login = usuario,
        Senha = senha
    };
    bd.Usuarios.Add(u);
    bd.SaveChanges();
    return new { Mensagem = "Ok", Criado = true };

});

app.Run();

void Validar(string chave, VagasDbContext bd)
{
    if(!bd.Sessoes.Any(s => s.Chave == Guid.Parse(chave)))
    {
        Sessao s= bd.Sessoes.First(s => s.Chave == Guid.Parse(chave));
        s.Vencimento = DateTime.Now.AddDays(1);
        bd.Sessoes.Update(s);
        bd.SaveChanges();
        throw new UnauthorizedAccessException();
    }

}

async Task CriaDbSeNaoExiste(IServiceProvider servicos, ILogger log)
{
    log.LogInformation("Validando Banco de dados");
    using var db = servicos.CreateScope().ServiceProvider.GetRequiredService<VagasDbContext>();
    await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();
}

class Sessao
{
    public int Id {get;set;}
    [Required]
    public Guid? Chave{get;set;}
    [Required]
    public int? UsuarioId {get;set;}
    [Required]
    public Usuario? Usuario {get;set;}
    [Required]
    public DateTime Vencimento{get;set;}
}

class Usuario
{
    public int Id {get;set;}
    [Required]public string? Login{get;set;}
    [Required]public string? Senha{get;set;}

    public ICollection<Sessao>? Sessoes{get;set;}
    
    public ICollection<Mensagem>? MensagensRecebidas { get; set; }
    public ICollection<Mensagem>? MensagensEnviadas { get; set; }
}

class Mensagem
{
    public int Id { get; set; }
    [Required] public int? UsuarioOrigemId { get; set; }
    [Required] public int? UsuarioDestinoId { get; set; }
    [Required] public DateTime? Data { get; set; }
    [Required] public string? Conteudo { get; set; }
    [Required] public bool? Recebido { get; set; }
    public Usuario? UsuarioOrigem { get; set; }
    public Usuario? UsuarioDestino { get; set; }
}

class Vaga
{
    public int Id {get;set;}
    [Required] public string? Cidade {get;set;}
    [Required] public decimal? Latitude {get;set;}
    [Required] public decimal? Longitude {get;set;}
    [Required] public bool Disponivel { get; set; } 

    public ICollection<Aluguel>? Alugueis {get;set;}

}

enum StatusAluguel
{
    Pendente = 1,
    Alugado = 2,
    Rejeitado = 3
}

class Aluguel
{
    public int Id {get;set;}

    [Required] public int? VagaId{get;set;}
    public Vaga? Vaga{get;set;}

    [Required] public DateTime? Inicio{get;set;}
    [Required] public DateTime? Fim{get;set;}
    [Required] public StatusAluguel? Status { get; set; }
}

class VagasDbContext : DbContext
{
    public VagasDbContext(DbContextOptions<VagasDbContext> options) : base(options){}
    public DbSet<Vaga> Vagas => Set<Vaga>();
    public DbSet<Aluguel> Alugueis => Set<Aluguel>();
    public DbSet<Usuario> Usuarios =>Set<Usuario>();
    public DbSet<Sessao> Sessoes => Set<Sessao>();
    public DbSet<Mensagem> Mensagens => Set<Mensagem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluguel>()
            .HasOne<Vaga>(i => i.Vaga)
            .WithMany(i => i.Alugueis)
            .HasForeignKey(i => i.VagaId);
        modelBuilder.Entity<Sessao>()
            .HasOne<Usuario>(i => i.Usuario)
            .WithMany(i => i.Sessoes)
            .HasForeignKey(i => i.UsuarioId);
        modelBuilder.Entity<Mensagem>()
            .HasOne<Usuario>(i => i.UsuarioDestino)
            .WithMany(i => i.MensagensRecebidas)
            .HasForeignKey(i => i.UsuarioDestinoId);
        modelBuilder.Entity<Mensagem>()
            .HasOne<Usuario>(i => i.UsuarioOrigem)
            .WithMany(i => i.MensagensEnviadas)
            .HasForeignKey(i => i.UsuarioOrigemId);
    }
}