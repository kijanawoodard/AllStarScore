using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public static class CommandRegistrar
    {
        public static void RegisterCommand(this ICanBeUpdatedByCommand document, ICommand command)
        {
            document.LastCommand = command.GetType().Name;
            document.LastCommandBy = command.CommandByUser;
            document.LastCommandDate = command.CommandWhen;
        }
    }
}