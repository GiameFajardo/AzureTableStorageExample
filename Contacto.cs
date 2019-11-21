using Microsoft.Azure.Cosmos.Table;

namespace TableConsole
{
    public class Contacto : TableEntity
    {
        public Contacto()
        {
            
        }
        public Contacto(string nombre, string apellido)
        {
            PartitionKey = apellido;
            RowKey = nombre;
        }
        public string Email { get; set; } 
        public string Telefono { get; set; }

    }
}