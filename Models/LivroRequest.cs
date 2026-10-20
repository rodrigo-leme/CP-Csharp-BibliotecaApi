namespace BibliotecaApi.Models;

// Dados recebidos no corpo das requisições POST e PUT
public record LivroRequest(string Titulo, string Autor, int AnoPublicacao, bool Disponivel);
