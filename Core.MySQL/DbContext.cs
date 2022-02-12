using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Core.MySQL
{
    public class DbContext
    {
        public DbContext(string connect)
        {
            Connect = connect;
            RegisterDateFormat();
        }

        protected readonly string Connect;

        public int Execute(string sql)
        {
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            int rowsAffected = 0;
            try
            {
                con = new MySqlConnection(Connect);
                con.Open();
                cmd = new MySqlCommand(sql, con);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException exception)
            {
                throw exception;
            }
            finally
            {
                cmd?.Dispose();
                con?.Close();
                con?.Dispose();
            }
            return rowsAffected;
        }

        public long ExecuteById(string sql)
        {
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            long id = 0;
            try
            {
                con = new MySqlConnection(Connect);
                con.Open();
                cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                id = cmd.LastInsertedId;
            }
            catch (MySqlException exception)
            {
                throw exception;
            }
            finally
            {
                cmd?.Dispose();
                con?.Close();
                con?.Dispose();
            }
            return id;
        }

        public IEnumerable<T> QueryResult<T>(string sql) where T : new()
        {
            MySqlConnection con = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            var result = new List<T>();
            try
            {
                con = new MySqlConnection(Connect);
                con.Open();
                cmd = new MySqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var t = new T();
                    UpdateValue(reader, ref t);
                    result.Add(t);
                }
            }
            catch (MySqlException exception)
            {
                throw exception;
            }
            finally
            {
                reader?.Close();
                reader?.Dispose();
                cmd?.Dispose();
                con?.Close();
                con?.Dispose();
            }
            return result;
        }

        private void UpdateValue<T>(MySqlDataReader reader, ref T t) where T : new()
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!Enumerable.Range(0, reader.FieldCount).Any(i => reader.GetName(i).ToLower().Equals(property.Name.ToLower())))
                {
                    continue;
                }
                property.SetValue(t, reader[property.Name]);
            }
        }

        private void RegisterDateFormat()
        {
            if (!Thread.CurrentThread.CurrentCulture.Name.Equals("ru-RU"))
            {
                var culture = CultureInfo.CreateSpecificCulture("ru-RU");

                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
        }
    }
}
