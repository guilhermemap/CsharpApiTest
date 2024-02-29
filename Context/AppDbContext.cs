using Microsoft.EntityFrameworkCore;
using testapi.Models;

namespace testapi.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Categoria>? Categorias { get; set; }
  public DbSet<Produto>? Produtos { get; set; }
}