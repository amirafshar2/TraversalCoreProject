using Microsoft.Data.Sqlite;

namespace TraversalCoreProje.Infrastructure
{
    public static class SqlitePath
    {
        /// <summary>
        /// Macht den Pfad "Data Source=App_Data/traversal.db" relativ zum Projektordner absolut
        /// und legt den Ordner an, falls er noch nicht existiert.
        /// </summary>
        public static string Resolve(string connectionString, string contentRoot)
        {
            var csb = new SqliteConnectionStringBuilder(connectionString);
            if (!Path.IsPathRooted(csb.DataSource))
                csb.DataSource = Path.Combine(contentRoot, csb.DataSource);
            Directory.CreateDirectory(Path.GetDirectoryName(csb.DataSource)!);
            return csb.ToString();
        }
    }
}
