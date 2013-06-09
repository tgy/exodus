using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame
{
    public class Resource
    {
        public int Steel;
        public int Iron;
        public int Graphene;
        public int Hydrogen;
        public int Electricity;
        public Resource(int steel, int iron, int graphene, int hydrogen, int electricity)
        {
            Steel = steel;
            Iron = iron;
            Graphene = graphene;
            Hydrogen = hydrogen;
            Electricity = electricity;
        }
        public static Resource operator *(float i, Resource p)
        {
            return new Resource(
                (int)(p.Steel * i),
                (int)(p.Iron * i),
                (int)(p.Graphene * i),
                (int)(p.Hydrogen * i),
                (int)(p.Electricity * i)
            );
        }
        public static Resource operator *(Resource p, float i)
        {
            return i * p;
        }
        public static Resource operator +(Resource p, Resource q)
        {
            return new Resource(p.Steel + q.Steel,
                             p.Iron + q.Iron,
                             p.Graphene + q.Graphene,
                             p.Hydrogen + q.Hydrogen,
                             p.Electricity + q.Electricity);

        }
        public static Resource operator -(Resource p, Resource q)
        {
            return new Resource(p.Steel - q.Steel,
                                p.Iron - q.Iron,
                                p.Graphene - q.Graphene,
                                p.Hydrogen - q.Hydrogen,
                                p.Electricity - q.Electricity);
        }
    }
}
