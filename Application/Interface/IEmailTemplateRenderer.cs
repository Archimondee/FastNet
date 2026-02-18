namespace Application.Interface;

public interface IEmailTemplateRenderer
{
  Task<string> RenderAsync(string templateName, string payloadJson);
}