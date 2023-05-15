using SQLite;

namespace Poliglot.Source.Database;

public class DbItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
}
