using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class DtoFormulario : DtoPadrao
    {

        public DtoFormulario()
        {
            FormulariosFilhos = new HashSet<DtoFormulario>();
            Secoes = new HashSet<DtoSecao>();
        }

        public string CaminhoFormulario { get; set; }

        public DateTime CriadoEm { get; set; }

        public string Descricao { get; set; }

        public DtoFormulario FormularioPai { get; set; }

        public long? FormularioPaiId { get; set; }

        public ICollection<DtoFormulario> FormulariosFilhos { get; set; }

        public ICollection<DtoSecao> Secoes { get; set; }

        public EnumSituacaoFormulario Situacao { get; set; }

        public string Sumario { get; set; }

        public DtoTipoFormulario TipoFormulario { get; set; }

        public long TipoFormularioId { get; set; }

        public int? Versao { get; set; }
    }
}