namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce
{
    public class MetaCampo : ObjetoPersistido
    {
        public long CampoPersonalizadoId { get; set; }

        public EnumSimNao Obrigatorio { get; set; }

        public int Ordem { get; set; }

        public Secao Secao { get; set; }

        public long SecaoId { get; set; }

        public string Sumario { get; set; }

        public string Titulo { get; set; }
    }
}