using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public abstract class DataAccess<T> : IDataAccess<T>
        where T : DomainObject
    {
        bool _getAllExecuted;


        protected DataAccess(bool useCache = true)
        {
            UseCache = useCache;
        }


        protected Cache<T, int?> Cache { get; } = new Cache<T, int?>();
        protected bool UseCache { get; set; }


        protected abstract string InsertStatement { get; }
        protected abstract string UpdateStatement { get; }
        protected abstract string DeleteStatement { get; }
        protected abstract string GetByIdStatement { get; }
        protected abstract string GetAllStatement { get; }


        public virtual IEnumerable<T> Get()
        {
            if (string.IsNullOrEmpty(GetAllStatement))
                throw new NotImplementedException();

            if (UseCache && _getAllExecuted)
                return Cache.GetAll();

            var cmd = GetCommand();
            cmd.CommandText = GetAllStatement;
            var collection = ExecuteSelectCommand(cmd, Parse, true);
            if (UseCache)
                _getAllExecuted = true;
            return collection;
        }

        public virtual T Get(int id)
        {
            if (string.IsNullOrEmpty(GetByIdStatement))
                throw new NotImplementedException();

            var cmd = GetCommand();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandText = GetByIdStatement;

            if (UseCache)
                return Cache.GetOrAdd(id, () => ExecuteSelectCommand(cmd, Parse).FirstOrDefault());

            return ExecuteSelectCommand(cmd, Parse).FirstOrDefault();
        }

        public virtual void Save(T obj)
        {
            ValidateSave(obj);
            PreSave(obj);

            if (obj.Id.HasValue)
                Update(obj);
            else
                Insert(obj);

            PostSave(obj);
        }

        protected virtual void Insert(T obj)
        {
            var cmd = GetCommand();
            cmd.CommandText = InsertStatement;
            AddParametersToInsertUpdateCommand(cmd, obj);
            ExecuteInsertCommand(cmd, obj);
        }

        protected virtual void Update(T obj)
        {
            var cmd = GetCommand();
            cmd.CommandText = UpdateStatement;
            AddParametersToInsertUpdateCommand(cmd, obj);
            ExecuteUpdateCommand(cmd, obj);
        }

        public virtual void Delete(T obj)
        {
            var cmd = GetCommand();
            cmd.CommandText = DeleteStatement;
            ExecuteDeleteCommand(cmd, obj);
        }

        protected abstract void ValidateSave(T obj);
        protected abstract T Parse(IDataRecord record);
        protected abstract void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, T obj);

        protected virtual void PreSave(T obj)
        {
        }

        protected virtual void PostSave(T obj)
        {
        }

        protected SQLiteCommand GetCommand()
        {
            return new SQLiteCommand(Database.OpenConnection());
        }

        protected IEnumerable<T> ExecuteSelectCommand(SQLiteCommand command, Func<IDataRecord, T> transform,
            bool useCache = false)
        {
            using (command)
            using (var reader = command.ExecuteReader())
            {
                var collection = new List<T>();
                while (reader.Read())
                    collection.Add(transform(reader));

                if (useCache)
                    return Cache.GetOrAdd(collection);

                return collection;
            }
        }

        protected void ExecuteInsertCommand(SQLiteCommand command, T obj)
        {
            using (command)
            {
                obj.Id = (int) (long) command.ExecuteScalar();
            }

            if (UseCache)
                Cache.Add(obj);
        }

        protected void ExecuteUpdateCommand(SQLiteCommand command, T obj)
        {
            if (!obj.Id.HasValue)
                throw new InvalidOperationException("Cannot update an object that does not have an id");

            if (!command.Parameters.Contains("@id"))
                command.Parameters.AddWithValue("@id", obj.Id.Value);

            using (command)
                command.ExecuteNonQuery();

            if (UseCache)
                Cache.Add(obj);
        }

        protected void ExecuteDeleteCommand(SQLiteCommand command, T obj)
        {
            if (!obj.Id.HasValue)
                throw new InvalidOperationException("Cannot update an object that does not have an id");

            if (!command.Parameters.Contains("@id"))
                command.Parameters.AddWithValue("@id", obj.Id.Value);

            using (command)
                command.ExecuteNonQuery();

            if (UseCache)
                Cache.Remove(obj);

            obj.Id = null;
        }
    }
}