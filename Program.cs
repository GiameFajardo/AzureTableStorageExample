using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace TableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("apsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            CreateTableAsync("newTable02", config["connectionString"]).Wait();
        }
        public static async Task<CloudTable> CreateTableAsync(string tableName, string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = client.GetTableReference(tableName);

            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("tabla creada" + tableName);
            }
            else
            {
                Console.WriteLine("La tabla " + tableName + "ya existe.");
            }

            // await  InsertOperationAsync(table);
            // Contacto contacto = GetSingleOperation(table, "Del barrio", "Princesa").Result;
           

            //     Console.WriteLine("Aqui tienes: {0}\t{1}\t{2}\t{3}",
            //         contacto.PartitionKey,
            //         contacto.RowKey,
            //         contacto.Telefono,
            //         contacto.Email);

            var message = DeleteOperation(table,"Princesa", "Del barrio").Result;
            Console.WriteLine(message);
            return table;
        }

        private static async Task<Contacto> InsertOperationAsync(CloudTable table)
        {
            Contacto contacto = new Contacto("Princesa", "Del barrio")
            {
                Email = "gmcrls13@gmail.com",
                Telefono = "8097241590"
            };

            TableOperation insertOperation = TableOperation.InsertOrMerge(contacto);
            TableResult result = await table.ExecuteAsync(insertOperation);
            Contacto contactoInserted = result.Result as Contacto;

            return contactoInserted;
        }
        private static async Task<Contacto> GetSingleOperation(CloudTable table, string name, string lastName)
        {
            TableOperation retreaveOperation = TableOperation.Retrieve<Contacto>(name, lastName);
            TableResult result = await table.ExecuteAsync(retreaveOperation);
            Contacto contacto = result.Result as Contacto;

            return contacto;
        }
        private static async Task<List<Contacto>> GetAllOperation(CloudTable table)
        {
            TableOperation retreaveOperation = TableOperation.Retrieve<Contacto>(null, null);
            TableResult result = await table.ExecuteAsync(retreaveOperation);
            List<Contacto> contacto = result.Result as List<Contacto>;

            return contacto;
        }
        private static async Task<string> DeleteOperation(CloudTable table, string name, string lastName)
        {
            var contact = new Contacto(name,lastName)
            {
                ETag = "*"

            };
            TableOperation deleteOperation = TableOperation.Delete(contact);
            TableResult result = await table.ExecuteAsync(deleteOperation);
            string resultMessage = result.RequestCharge.ToString();

            return resultMessage;
        }
    }
}
