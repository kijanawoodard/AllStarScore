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

            RegisterCompanyCommand(document as IBelongToCompany, command as ICompanyCommand);
        }

        public static void RegisterCompanyCommand(this IBelongToCompany document, ICompanyCommand command)
        {
            if (document == null || command == null)
                return;

            document.CompanyId = command.CommandCompanyId;
        }
    }
}