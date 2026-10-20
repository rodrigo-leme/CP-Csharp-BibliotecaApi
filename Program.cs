using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// GET /livros - lista todos os livros
app.MapGet("/livros", async (BibliotecaContext db) =>
{
    var livros = await db.Livros.ToListAsync();
    return Results.Ok(livros);
});

// GET /livros/{id} - busca um livro pelo id
app.MapGet("/livros/{id}", async (int id, BibliotecaContext db) =>
{
    var livro = await db.Livros.FindAsync(id);

    if (livro is null)
        return Results.NotFound(new { mensagem = $"Livro com id {id} não encontrado." });

    return Results.Ok(livro);
});

// POST /livros - cadastra um novo livro
app.MapPost("/livros", async (LivroRequest request, BibliotecaContext db) =>
{
    var erro = ValidarLivro(request);
    if (erro is not null)
        return Results.BadRequest(new { mensagem = erro });

    var livro = new Livro
    {
        Titulo = request.Titulo,
        Autor = request.Autor,
        AnoPublicacao = request.AnoPublicacao,
        Disponivel = request.Disponivel
    };

    db.Livros.Add(livro);
    await db.SaveChangesAsync();

    return Results.Created($"/livros/{livro.Id}", livro);
});

// PUT /livros/{id} - atualiza um livro existente
app.MapPut("/livros/{id}", async (int id, LivroRequest request, BibliotecaContext db) =>
{
    var livro = await db.Livros.FindAsync(id);

    if (livro is null)
        return Results.NotFound(new { mensagem = $"Livro com id {id} não encontrado." });

    var erro = ValidarLivro(request);
    if (erro is not null)
        return Results.BadRequest(new { mensagem = erro });

    livro.Titulo = request.Titulo;
    livro.Autor = request.Autor;
    livro.AnoPublicacao = request.AnoPublicacao;
    livro.Disponivel = request.Disponivel;

    await db.SaveChangesAsync();

    return Results.Ok(livro);
});

// DELETE /livros/{id} - remove um livro
app.MapDelete("/livros/{id}", async (int id, BibliotecaContext db) =>
{
    var livro = await db.Livros.FindAsync(id);

    if (livro is null)
        return Results.NotFound(new { mensagem = $"Livro com id {id} não encontrado." });

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

// Validação simples dos dados enviados
static string? ValidarLivro(LivroRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Titulo))
        return "O título é obrigatório.";

    if (string.IsNullOrWhiteSpace(request.Autor))
        return "O autor é obrigatório.";

    if (request.AnoPublicacao <= 0 || request.AnoPublicacao > DateTime.Now.Year)
        return "O ano de publicação é inválido.";

    return null;
}
