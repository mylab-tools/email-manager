namespace MyLab.EmailManager.App.ViewModels
{
    public record MessageViewModel
        (
            DateTime CreateDt,
            DateTime SendDt,
            string Title,
            string Content
        );
}
