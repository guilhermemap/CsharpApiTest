using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testapi.Context;
using testapi.Models;

namespace testapi.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController(AppDbContext appDbContext) : ControllerBase
{
  readonly AppDbContext _context = appDbContext;
  [HttpGet("produtos")]
  public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
  {
    return await _context.Categorias.Include(c => c.Produtos).ToListAsync();
  }
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Categoria>>> Get()
  {
    var categorias = await _context.Categorias.ToListAsync();
    if (categorias is null)
    {
      return NotFound("Nenhum produto encontrado?");
    }
    return categorias;
  }
  [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
  public async Task<ActionResult<Categoria>> Get(int id)
  {
    var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
    if (categoria is null)
    {
      return NotFound("Produto não encontrado...");
    }
    return categoria;
  }
  [HttpPost]
  public async Task<ActionResult> Post(Categoria categoria)
  {
    /* validação feita automaticamente por ser [ApiController] */
    if (categoria is null)
      return BadRequest("erro com o produto");
    _context.Categorias.Add(categoria);
    await _context.SaveChangesAsync();
    return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
  }
  [HttpPut("{id:int:min(1)}")]
  public async Task<ActionResult> Put(int id, Categoria categoria)
  {
    if (id != categoria.CategoriaId)
      return BadRequest("Conflito de Id...");
    _context.Entry(categoria).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return Ok(categoria);
  }
  [HttpDelete("{id:int:min(1)}")]
  public async Task<ActionResult> Delete(int id)
  {
    var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
    if (categoria is null)
    {
      return NotFound("Produto não encontrado...");
    }
    _context.Categorias.Remove(categoria);
    _context.SaveChanges();
    return Ok(categoria);
  }
}
