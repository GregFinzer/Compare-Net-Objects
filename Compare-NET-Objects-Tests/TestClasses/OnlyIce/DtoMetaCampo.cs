namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class DtoMetaCampo : DtoPadrao
    {
        public long CampoPersonalizadoId { get; set; }

        public EnumSimNao Obrigatorio { get; set; }

        public int Ordem { get; set; }

        public DtoSecao Secao { get; set; }

        public long SecaoId { get; set; }

        public string Sumario { get; set; }

        public string Titulo { get; set; }
    }
}