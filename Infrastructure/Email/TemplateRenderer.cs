using System.Text.Json;
using Application.Interface;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Email;

public class TemplateRenderer : IEmailTemplateRenderer
{
  private readonly IWebHostEnvironment _env;

  public TemplateRenderer(IWebHostEnvironment env)
  {
    _env = env;
  }

  public async Task<string> RenderAsync(string templateName, string payloadJson)
  {
    var path = Path.Combine(
      _env.ContentRootPath,
      "Template",
      "Email",
      $"{templateName}.html");

    if (!File.Exists(path))
      throw new Exception("Template not found");

    var html = await File.ReadAllTextAsync(path);

    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(payloadJson);

    if (data == null)
      throw new Exception("Invalid payload");

    foreach (var item in data)
    {
      html = html.Replace($"{{{{{item.Key}}}}}", item.Value);
    }

    return html;
  }
}