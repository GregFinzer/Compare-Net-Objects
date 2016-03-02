using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class DtoSecao : DtoPadrao
    {
        public DtoSecao()
        {
            Campos = new HashSet<DtoMetaCampo>();
        }

        public ICollection<DtoMetaCampo> Campos { get; set; }

        public string Descricao { get; set; }

        public DtoFormulario Formulario { get; set; }

        public long FormularioId { get; set; }

        public int Ordem { get; set; }

        public string Sumario { get; set; }

        public EnumTipoSecao TipoSecao { get; set; } 
    }
}