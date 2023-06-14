using Services.Shared.Configuration;

namespace EmailingService.FileHelper
{
    public class FileWorker
    {
        private readonly string _confirmationEmailPath;
        private readonly string _resetPasswordTokenEmailPath;
        private readonly string _logoPath = "https://play-lh.googleusercontent.com/o6kXY8vstMP3ZPHHGwwJbziG6CJMuQHqL_t4yq7A7zQrMhfG7nuY3XPHrDkLmOSafvk_";

        public readonly OthersConfiguration Configuration;
        public FileWorker(OthersConfiguration configuration)
        {
            Configuration = configuration;
            _confirmationEmailPath = $"{configuration.TemplatePath}/ConfirmationEmailTemplate.html";
            _resetPasswordTokenEmailPath = $"{configuration.TemplatePath}/ResetPasswordTokenEmailTemplate.html";
        }
        
        public string GetConfirmationEmailHTMLFile(string data)
        {
            var fileContent = File.ReadAllText(_confirmationEmailPath);

            fileContent = fileContent.Replace("[url_of_organization_site]", "");
            fileContent = fileContent.Replace("[name_of_organization]", "manga application");
            fileContent = fileContent.Replace("[type_of_action]", "email confirmation");
            fileContent = fileContent.Replace("[url_to_organizations_logo]", _logoPath);
            fileContent = fileContent.Replace("[data]", data);

            return fileContent;
        }

        public string GetResetPasswordTokenEmailHTMLFile(string data)
        {
            var fileContent = File.ReadAllText(_resetPasswordTokenEmailPath);

            fileContent = fileContent.Replace("[url_of_organization_site]", "");
            fileContent = fileContent.Replace("[name_of_organization]", "manga application");
            fileContent = fileContent.Replace("[type_of_action]", "email confirmation");
            fileContent = fileContent.Replace("[url_to_organizations_logo]", _logoPath);
            fileContent = fileContent.Replace("[data]", data);

            return fileContent;
        }
    }
}
