using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class DtoTipoFormulario : DtoPadrao
    {
        public DtoTipoFormulario()
        {
            Formularios = new HashSet<DtoFormulario>();
        }

        public EnumSimNao Ativo { get; set; }

        public long CategoriaId { get; set; }

        public string Descricao { get; set; }

        public ICollection<DtoFormulario> Formularios { get; set; }

        public long UsuarioId { get; set; }
    }
}