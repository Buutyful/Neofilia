using ErrorOr;

namespace Neofilia.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class LocalErrors
        {
            public static Error DuplicatedTable => Error.Conflict(
               code: "Table.Duplicated",
               description: "The same table already exists");
            public static Error DuplicatedMenu => Error.Conflict(
                code: "Menu.Duplicated",
                description: "The same menu already exists");
            public static Error TableNotFound(int id) => Error.NotFound(
                code: "Table.NotFound",
                description: $"Table with id {id} was not found");

        }
    }
}
