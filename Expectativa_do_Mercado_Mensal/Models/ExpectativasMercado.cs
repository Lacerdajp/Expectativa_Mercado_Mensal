using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expectativa_do_Mercado_Mensal.Models
{
  
    internal class ExpectativasMercado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Indicador { get; set; }
        public string Data { get; set; }
        public string DataReferencia { get; set; }
        public float Media { get; set; }
        public float Mediana { get; set; } 
        public float DesvioPadrao { get; set; }
        public float Minimo { get; set; }
        public float Maximo { get; set; }
        public float numeroRespondentes { get; set; }
        public float baseCalculo { get; set; }
    }
}
