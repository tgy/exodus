using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.PlayGame
{
    [Serializable]
    public class Resource
    {
        public double Steel;
        public double Iron;
        public double Graphene;
        public double Hydrogen;
        public double Electricity;
        public Resource(double steel, double iron, double graphene, double hydrogen, double electricity)
        {
            Steel = steel;
            Iron = iron;
            Graphene = graphene;
            Hydrogen = hydrogen;
            Electricity = electricity;
        }
        public static Resource operator *(double i, Resource p)
        {
            return new Resource(
                p.Steel * i,
                p.Iron * i,
                p.Graphene * i,
                p.Hydrogen * i,
                p.Electricity * i
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
        public static bool operator >(Resource p, Resource q)
        {
            return (p.Hydrogen > q.Hydrogen && p.Iron > q.Iron && p.Graphene > q.Graphene && p.Steel > q.Steel && p.Electricity > q.Electricity);
        }
        public static bool operator <(Resource p, Resource q)
        {
            return (p.Hydrogen < q.Hydrogen && p.Iron < q.Iron && p.Graphene < q.Graphene && p.Steel < q.Steel && p.Electricity < q.Electricity);
        }
        public static bool operator >=(Resource p, Resource q)
        {
            return (p.Hydrogen >= q.Hydrogen && p.Iron >= q.Iron && p.Graphene >= q.Graphene && p.Steel >= q.Steel && p.Electricity >= q.Electricity);
        }
        public static bool operator <=(Resource p, Resource q)
        {
            return (p.Hydrogen <= q.Hydrogen && p.Iron <= q.Iron && p.Graphene <= q.Graphene && p.Steel <= q.Steel && p.Electricity <= q.Electricity);
        }
    }
}
