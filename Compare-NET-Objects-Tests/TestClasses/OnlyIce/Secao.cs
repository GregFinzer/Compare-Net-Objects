using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class Secao : ObjetoPersistido
    {
        public Secao()
        {
            Campos = new HashSet<MetaCampo>();
        }

        public ICollection<MetaCampo> Campos { get; set; }

        public string Descricao { get; set; }

        public Formulario Formulario { get; set; }

        public long FormularioId { get; set; }

        public int Ordem { get; set; }

        public string Sumario { get; set; }

        public EnumTipoSecao TipoSecao { get; set; } 
    }
}