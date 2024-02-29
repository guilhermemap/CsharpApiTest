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
  public ActionResult<IEnumerable<Produto>> Get()
  {
    var produtos = _context.Produtos.ToList();
    if (produtos is null)
    {
      return NotFound("Nenhum produto encontrado?");
    }
    return produtos;
  }
  [HttpGet("{id:int}", Name = "ObterProduto")]
  public ActionResult<Produto> Get(int id)
  {
    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }
    return produto;
  }
  [HttpPost]
  public ActionResult Post(Produto produto)
  {
    /* validação feita automaticamente por ser [ApiController] */
    if (produto is null)
      return BadRequest("erro com o produto");
    _context.Produtos.Add(produto);
    _context.SaveChanges();
    return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
  }
  [HttpPut("{id:int}")]
  public ActionResult Put(int id, Produto produto)
  {
    if (id != produto.ProdutoId)
      return BadRequest("Conflito de Id...");
    _context.Entry(produto).State = EntityState.Modified;
    _context.SaveChanges();
    return Ok(produto);
  }
  [HttpDelete("{id:int}")]
  public ActionResult Delete(int id)
  {
    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }
    _context.Produtos.Remove(produto);
    _context.SaveChanges();
    return Ok(produto);
  }
}
