using System;
using AllStarScore.Extensions;
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

        private static void RegisterCompanyCommand(this IBelongToCompany document, ICompanyCommand command)
        {
            if (document == null || command == null)
                return;

            if (string.IsNullOrWhiteSpace(document.CompanyId))
                document.CompanyId = command.CommandCompanyId;

            if (command.CommandCompanyId == null)
                throw new ApplicationException(string.Format("Command doesn't have company id. {0}, {1}", command.GetType().Name, command.ToJson()));

            if (document.CompanyId == command.CommandCompanyId)
                return;

            var msg = string.Format("Attempt to update different company. document: {0}. command {1}",
                                    document.ToJson(), command.ToJson());
            throw new AccessViolationException(msg);
        }

        public static void CopyCommandPropertiesFrom(this ICommand target, ICommand src)
        {
            target.CommandByUser = src.CommandByUser;
            target.CommandWhen = src.CommandWhen;

            CopyCompanyCommandPropertiesFrom(target as ICompanyCommand, src as ICompanyCommand);
        }

        private static void CopyCompanyCommandPropertiesFrom(this ICompanyCommand target, ICompanyCommand src)
        {
            if (src == null || target == null)
                return;

            target.CommandCompanyId = src.CommandCompanyId;
        }
    }
}