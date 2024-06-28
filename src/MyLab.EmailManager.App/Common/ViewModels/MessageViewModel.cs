namespace MyLab.EmailManager.App.Common.ViewModels
{
    public record MessageViewModel
        (
            DateTime CreateDt,
            DateTime SendDt,
            string Title,
            string Content
        );
}
