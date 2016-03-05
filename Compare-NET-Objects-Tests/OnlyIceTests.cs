using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class OnlyIceTests
    {
        [Test]
        public void ChildPropertyTypeShouldBeIgnored()
        {
            var objeto = CreateSecao();
            var dto = CreateDtoSecao();
            var comparador = new CompareLogic();

            comparador.Config.MembersToIgnore.Add("Capacity");
            comparador.Config.IgnoreObjectTypes = true;
            var resultado = comparador.Compare(dto, objeto);
            Assert.IsTrue(resultado.AreEqual, resultado.DifferencesString);
        }

        private static Secao CreateSecao()
        {
            var objeto = new Secao
            {
                Id = 10,
                Descricao = "Descricao",
                Formulario = new Formulario { Id = 101 },
                TipoSecao = EnumTipoSecao.Fixo,
                Ordem = 1,
                Sumario = "Sumario",
            };

            objeto.Campos.Add(
                new MetaCampo
                {
                    Id = 11,
                    Obrigatorio = EnumSimNao.SIM,
                    Ordem = 11,
                    Secao = new Secao { Id = 112 },
                    SecaoId = 112,
                    Sumario = "Sumario1",
                    Titulo = "Titulo1"
                });

            return objeto;
        }

        private static DtoSecao CreateDtoSecao()
        {
            var dto = new DtoSecao
            {
                Id = 10,
                Descricao = "Descricao",
                Formulario = new DtoFormulario { Id = 101 },
                TipoSecao = EnumTipoSecao.Fixo,
                Ordem = 1,
                Sumario = "Sumario",
            };

            dto.Campos.Add(
                new DtoMetaCampo
                {
                    Id = 11,
                    Obrigatorio = EnumSimNao.SIM,
                    Ordem = 11,
                    SecaoId = 112,
                    Secao = new DtoSecao { Id = 112 },
                    Sumario = "Sumario1",
                    Titulo = "Titulo1"
                });

            return dto;
        }
    }
}
