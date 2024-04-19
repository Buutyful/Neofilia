using ErrorOr;

namespace Neofilia.Domain.Common.Errors
{
    public static class Errors
    {
        public static class LocalErrors
        {
            public static Error DuplicatedTable => Error.Conflict(
               code: "Table.Duplicated",
               description: "The same table already exists");
            public static Error DuplicatedMenu => Error.Conflict(
                code: "Menu.Duplicated",
                description: "The same menu already exists");
            public static Error LocalNotFound(int id) => Error.NotFound(
                code: "Local.NotFound",
                description: $"Local with id {id} was not found");
            //TODO: move this into Errors.TableErrors and make the Errors class partial
            public static Error TableNotFound(int id) => Error.NotFound(
                code: "Table.NotFound",
                description: $"Table with id {id} was not found");

        }
    }
}
