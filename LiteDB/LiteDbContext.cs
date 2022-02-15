using LiteDB;
using Microsoft.Extensions.Options;
using System;

namespace LiteDB
{
    public class LiteDbContext
    {
        public readonly LiteDatabase Context;
        public LiteDbContext(IOptions<LiteDbConfig> configs)
        {
            try
            {
                var db = new LiteDatabase($"Filename={configs.Value.DatabasePath}; Connection={configs.Value.Connection};");
                if (db != null)
                    Context = db;
            }
            catch (Exception ex)
            {
                throw new Exception("Can't find or create LiteDb database.", ex);
            }
        }
    }
}