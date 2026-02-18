using System.Text.Json;
using Application.Interface;

namespace Infrastructure.Email;

public class TemplateRenderer : IEmailTemplateRenderer
{
  public async Task<string> RenderAsync(string templateName, string payloadJson)
  {
    var path = Path.Combine("Template", "Email", $"{templateName}.html");
    var html = await File.ReadAllTextAsync(path);
    if (!File.Exists(path))
      throw new Exception("Template not found");

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