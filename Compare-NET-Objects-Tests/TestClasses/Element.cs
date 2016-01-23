using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class Element
    {
        private static uint Counter = 0;

        public uint Value { get; set; }
        public ICollection<Element> Elements { get; set; }

        public Element()
        {
            this.Value = Counter++;
            this.Elements = new List<Element>();
        }

        public Element(uint generationDepth, uint nElements)
        {
            this.Value = Counter++;
            this.Elements = new List<Element>();

            if (generationDepth > 0)
            {
                generationDepth--;
                for (int i = 0; i < nElements; i++)
                {
                    this.Elements.Add(new Element(generationDepth, nElements));
                }
            }
        }

        public static Element ReverseCopy(Element rootElement)
        {
            return new Element
            {
                Value = rootElement.Value,
                Elements = rootElement.Elements
                                      .Reverse()
                                      .Select(el => ReverseCopy(el))
                                      .ToList()
            };
        }

        public override string ToString()
        {
            var result = this.Value + "-";
            foreach (var element in Elements)
            {
                result += element.ToString();
            }
            return result;
        }
    }
}
