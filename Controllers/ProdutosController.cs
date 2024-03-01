using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testapi.Context;
using testapi.Models;

namespace testapi.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController(AppDbContext appDbContext) : ControllerBase
{
  readonly AppDbContext _context = appDbContext;
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Produto>>> Get()
  {
    var produtos = await _context.Produtos.ToListAsync();
    if (produtos is null)
    {
      return NotFound("Nenhum produto encontrado?");
    }
    return produtos;
  }
  [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
  public async Task<ActionResult<Produto>> Get(int id)
  {
    var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }
    return produto;
  }
  [HttpPost]
  public async Task<ActionResult> Post(Produto produto)
  {
    /* validação feita automaticamente por ser [ApiController] */
    if (produto is null)
      return BadRequest("erro com o produto");
    _context.Produtos.Add(produto);
    await _context.SaveChangesAsync();
    return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
  }
  [HttpPut("{id:int:min(1)}")]
  public async Task<ActionResult> Put(int id, Produto produto)
  {
    if (id != produto.ProdutoId)
      return BadRequest("Conflito de Id...");
    _context.Entry(produto).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return Ok(produto);
  }
  [HttpDelete("{id:int:min(1)}")]
  public async Task<ActionResult> Delete(int id)
  {
    var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }
    _context.Produtos.Remove(produto);
    _context.SaveChanges();
    return Ok(produto);
  }
}
