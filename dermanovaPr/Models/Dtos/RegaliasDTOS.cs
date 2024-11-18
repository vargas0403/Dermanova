using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class RegaliasDTOS
    {
        public int RegaliasId { get; set; }

  
        public string Name { get; set; }
      
        public string Marcas { get; set; }
       
        public int Unidades { get; set; }

        public bool State { get; set; }
    }
}
