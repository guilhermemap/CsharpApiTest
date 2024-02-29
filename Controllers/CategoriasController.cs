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
  public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
  {
    return _context.Categorias.Include(c => c.Produtos).ToList();
  }
  [HttpGet]
  public ActionResult<IEnumerable<Categoria>> Get()
  {
    var categorias = _context.Categorias.ToList();
    if (categorias is null)
    {
      return NotFound("Nenhum produto encontrado?");
    }
    return categorias;
  }
  [HttpGet("{id:int}", Name = "ObterCategoria")]
  public ActionResult<Categoria> Get(int id)
  {
    var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    if (categoria is null)
    {
      return NotFound("Produto não encontrado...");
    }
    return categoria;
  }
  [HttpPost]
  public ActionResult Post(Categoria categoria)
  {
    /* validação feita automaticamente por ser [ApiController] */
    if (categoria is null)
      return BadRequest("erro com o produto");
    _context.Categorias.Add(categoria);
    _context.SaveChanges();
    return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
  }
  [HttpPut("{id:int}")]
  public ActionResult Put(int id, Categoria categoria)
  {
    if (id != categoria.CategoriaId)
      return BadRequest("Conflito de Id...");
    _context.Entry(categoria).State = EntityState.Modified;
    _context.SaveChanges();
    return Ok(categoria);
  }
  [HttpDelete("{id:int}")]
  public ActionResult Delete(int id)
  {
    var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    if (categoria is null)
    {
      return NotFound("Produto não encontrado...");
    }
    _context.Categorias.Remove(categoria);
    _context.SaveChanges();
    return Ok(categoria);
  }
}
