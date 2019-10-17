using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Shop.DataAccess
{
    // 1. Отправить схемы таблиц в БД
    // 2. Отправить данные из методов Fill... в бд
    // 3. Вручную добавить в таблицы БД по 1 значению
    // 4. Скачать изменение в локальный DataSet;
    // 5. Используем фабрику провайдеров.

    public class AutolevelConnection
    {
        private static DataTable userTable;
        private static DataTable peopleTable;
        private readonly DbProviderFactory providerFactory;
        private DbConnection connection;
        
        public AutolevelConnection(string connectionString, string providerInvariantName)
        {
            providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
        }

        public void Action()
        {
            var dataSet = new DataSet("AutolevelShopDb"); // Название БД

            CreateUserTable();
            CreatePeopleTable();

            dataSet.Tables.AddRange(new DataTable[] { userTable, peopleTable });

            //FillPeople();
            //FillUsers();

            //dataSet.Relations.Add(new DataRelation("constraint_id_personId", peopleTable.TableName.ToString(), userTable.TableName.ToString(), new string[] { "id", "FullName" }, new string[] { "id", "personId" }, true));
            dataSet.Relations.Add("UserPeople", peopleTable.Columns["id"], userTable.Columns["personId"]);

            DbDataAdapter dataAdapterUser = providerFactory.CreateDataAdapter();
            DbDataAdapter dataAdapterPeople = providerFactory.CreateDataAdapter();

            var selectCommandUser = providerFactory.CreateCommand();
            var selectCommandPeople = providerFactory.CreateCommand();


            selectCommandUser.CommandText = "Select * From Users";
            selectCommandUser.Connection = connection;

            selectCommandPeople.CommandText = "Select * from People";
            selectCommandPeople.Connection = connection;


            dataAdapterUser.SelectCommand = selectCommandUser;
            DbCommandBuilder builderUser = providerFactory.CreateCommandBuilder();
            builderUser.DataAdapter = dataAdapterUser;

            dataAdapterPeople.SelectCommand = selectCommandPeople;
            DbCommandBuilder builderPeople = providerFactory.CreateCommandBuilder();
            builderPeople.DataAdapter = dataAdapterPeople;

            //dataAdapterPeople.Update(dataSet.Tables["People"]);
            //dataAdapterUser.Update(dataSet.Tables["Users"]);


            dataAdapterUser.Fill(dataSet.Tables["Users"]);
            dataAdapterPeople.Fill(dataSet.Tables["People"]); // Получает данные из БД

            dataSet.AcceptChanges();
            //dataAdapter.Update(dataSet); // Обновляет БД исходя их локального DataSet
        }

        private void FillUsers()
        {
            userTable.Rows.Add(1, 1);
        }

        private void FillPeople()
        {
            var idRow = peopleTable.NewRow();
            idRow.ItemArray = new object[] { 1, "Петрович" };
            peopleTable.Rows.Add(idRow);

            peopleTable.Rows.Add(2, "Василич");
        }

        private static void CreatePeopleTable()
        {
            peopleTable = new DataTable("people");
            peopleTable.Columns.Add(new DataColumn
            {
                ColumnName = "id",
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int),
            });
            peopleTable.PrimaryKey = new DataColumn[] { peopleTable.Columns["id"] };

            peopleTable.Columns.Add(new DataColumn
            {
                ColumnName = "FullName",
                AllowDBNull = false,
                Unique = false,
                DataType = typeof(string),
            });
        }

        private static void CreateUserTable()
        {
            userTable = new DataTable("Users");
            userTable.Columns.Add(new DataColumn
            {
                ColumnName = "id",
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int),
            });
            userTable.PrimaryKey = new DataColumn[] { userTable.Columns["id"] };

            userTable.Columns.Add(new DataColumn
            {
                ColumnName = "personId",
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int),
            });
        }
    }
}
